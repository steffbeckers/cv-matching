using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime.CredentialManagement;
using Amazon.Textract;
using Amazon.Textract.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RJM.BackgroundTasks
{
    public class DocumentParser : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<DocumentParser> logger;
        private IConnection connection;
        private IModel channel;
        private string exchange = "rjm.background.tasks";
        private string queue = "rjm.background.tasks";
        private string routingKey = "document.parser";

        // Amazon AWS Textract
        private AmazonTextractTextDetectionService textDetectionService;

        public DocumentParser(IConfiguration configuration, ILogger<DocumentParser> logger)
        {
            this.configuration = configuration;
            this.logger = logger;

            // Amazon AWS Textract config
            CredentialProfileOptions credentialProfileOptions = new CredentialProfileOptions
            {
                AccessKey = this.configuration.GetSection("AWS")
                                              .GetValue<string>("AccessKey"),
                SecretKey = this.configuration.GetSection("AWS")
                                              .GetValue<string>("SecretAccessKey")
            };

            CredentialProfile credentialProfile = new CredentialProfile("default", credentialProfileOptions);
            credentialProfile.Region = RegionEndpoint.EUWest2;

            NetSDKCredentialsFile netSDKCredentialsFile = new NetSDKCredentialsFile();
            netSDKCredentialsFile.RegisterProfile(credentialProfile);

            AWSOptions awsOptions = this.configuration.GetAWSOptions("AWS");
            this.textDetectionService = new AmazonTextractTextDetectionService(awsOptions.CreateServiceClient<IAmazonTextract>());

            // RabbitMQ
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = this.configuration.GetSection("RabbitMQService").GetValue<string>("HostName"),
                UserName = this.configuration.GetSection("RabbitMQService").GetValue<string>("UserName"),
                Password = this.configuration.GetSection("RabbitMQService").GetValue<string>("Password"),
                Port = this.configuration.GetSection("RabbitMQService").GetValue<int>("Port"),
                VirtualHost = this.configuration.GetSection("RabbitMQService").GetValue<string>("VirtualHost")
            };

            // Create connection to RabbitMQ
            this.connection = factory.CreateConnection();

            // Create channel  
            this.channel = connection.CreateModel();

            // Append suffix
            this.exchange += this.configuration.GetSection("RabbitMQService").GetValue<string>("Suffix");
            this.queue += this.configuration.GetSection("RabbitMQService").GetValue<string>("Suffix");
            this.routingKey += this.configuration.GetSection("RabbitMQService").GetValue<string>("Suffix");

            this.channel.ExchangeDeclare(
                exchange,
                ExchangeType.Topic,
                true
            );
            this.channel.QueueDeclare(
                queue,
                true,
                false,
                false,
                null
            );
            this.channel.QueueBind(
                queue,
                exchange,
                routingKey,
                null
            );
            this.channel.BasicQos(0, 1, false);

            this.connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            // Setup consumer
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (ch, ea) =>
            {
                // Received message  
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);

                // Handle the received message  
                await HandleMessage(content);

                // Acknowledge message is delivered
                channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            // Start consuming
            this.channel.BasicConsume(
                this.queue,
                false,
                consumer
            );

            return Task.CompletedTask;
        }

        private async Task HandleMessage(string content)
        {
            this.logger.LogInformation($"Received content: {content}");
            this.logger.LogInformation("Parsing content to Document");

            Models.Document document = JsonConvert.DeserializeObject<Models.Document>(content);

            this.logger.LogInformation($"Content parsed to Document model: {document}");
            this.logger.LogInformation($"Document mime type: {document.MimeType}");

            switch (document.MimeType)
            {
                // Text
                case "text/plain":
                    // TODO: Retrieve file check contents, save in database
                    break;
                // Images, PDF
                default:
                    this.logger.LogInformation($"Amazon Textract - Start text detection of document with ID: {document.Id}");

                    string jobId = await this.textDetectionService.StartDocumentTextDetection(
                        this.configuration.GetSection("AWS")
                            .GetSection("Bucket")
                            .GetValue<string>("Name"),
                        document.Path
                    );

                    this.logger.LogInformation($"Amazon Textract - Created a new job with ID: {jobId}");
                    this.logger.LogInformation("Amazon Textract - Waiting for job completion");

                    await this.textDetectionService.WaitForJobCompletion(jobId);

                    this.logger.LogInformation($"Amazon Textract - Job with ID: {jobId} is completed");
                    this.logger.LogInformation($"Amazon Textract - Retrieving results for job with ID: {jobId}");

                    List<GetDocumentTextDetectionResponse> response = await this.textDetectionService.GetJobResult(jobId);

                    this.logger.LogInformation($"Amazon Textract - Saving results to JSON text file");

                    string jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
                    File.WriteAllText($"DocumentParser/{document.Id}.result.json", jsonResponse);

                    this.logger.LogInformation($"Text detected in document with ID: {document.Id}");

                    // TODO: Save response in database

                    break;
            }

            this.logger.LogInformation($"Document with ID: {document.Id} parsed");
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            this.channel.Close();
            this.connection.Close();

            base.Dispose();
        }
    }

    public class AmazonTextractTextDetectionService
    {
        private IAmazonTextract textract;

        public AmazonTextractTextDetectionService(IAmazonTextract textract)
        {
            this.textract = textract;
        }

        public async Task<string> StartDocumentTextDetection(string bucketName, string key)
        {
            StartDocumentTextDetectionRequest request = new StartDocumentTextDetectionRequest();

            request.DocumentLocation = new DocumentLocation
            {
                S3Object = new S3Object
                {
                    Bucket = bucketName,
                    Name = key
                }
            };

            StartDocumentTextDetectionResponse response = await this.textract.StartDocumentTextDetectionAsync(request);
            
            return response.JobId;
        }

        public async Task WaitForJobCompletion(string jobId, int delay = 5000)
        {
            while (!await IsJobComplete(jobId))
            {
                this.Wait(delay);
            }
        }

        public async Task<bool> IsJobComplete(string jobId)
        {
            GetDocumentTextDetectionResponse response = await this.textract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
            {
                JobId = jobId
            });

            return !response.JobStatus.Equals("IN_PROGRESS");
        }

        public async Task<List<GetDocumentTextDetectionResponse>> GetJobResult(string jobId)
        {
            List<GetDocumentTextDetectionResponse> result = new List<GetDocumentTextDetectionResponse>();

            // Wait for response of Amazon Textract
            GetDocumentTextDetectionResponse response = await this.textract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
            {
                JobId = jobId
            });

            // Add response to the result
            result.Add(response);

            // If there is a next token in the result, we need to check the data again
            string nextToken = response.NextToken;
            while (nextToken != null)
            {
                this.Wait();
                response = await this.textract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
                {
                    JobId = jobId,
                    NextToken = response.NextToken
                });

                // Add next result to the response
                result.Add(response);

                // If there is a next token in the result, we need to check the data again
                nextToken = response.NextToken;
            }

            return result;
        }

        private void Wait(int delay = 5000)
        {
            Task.Delay(delay).Wait();
            Console.Write(".");
        }

        // TODO: Check what this does
        public async Task<DetectDocumentTextResponse> DetectTextS3(string bucketName, string key)
        {
            DetectDocumentTextResponse result = new DetectDocumentTextResponse();

            S3Object s3Object = new S3Object
            {
                Bucket = bucketName,
                Name = key
            };

            DetectDocumentTextRequest request = new DetectDocumentTextRequest();
            request.Document = new Document
            {
                S3Object = s3Object
            };

            return await this.textract.DetectDocumentTextAsync(request);
        }
    }
}
