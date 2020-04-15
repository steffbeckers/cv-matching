using System;
using System.Collections.Generic;
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
    public class AmazonTextractDocumentParser : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AmazonTextractDocumentParser> logger;
        private IConnection connection;
        private IModel channel;
        private string exchange = "rjm.background.tasks";
        private string queue = "rjm.background.tasks";
        private string routingKey = "document.parsing.amazon.textract";

        // Amazon AWS Textract
        private AmazonTextractTextDetectionService textDetectionService;

        public AmazonTextractDocumentParser(IConfiguration configuration, ILogger<AmazonTextractDocumentParser> logger)
        {
            this.configuration = configuration;
            this.logger = logger;

            // Amazon AWS Textract config
            CredentialProfileOptions credentialProfileOptions = new CredentialProfileOptions
            {
                AccessKey = this.configuration.GetSection("AmazonTextractDocumentParser").GetValue<string>("AccessKey"),
                SecretKey = this.configuration.GetSection("AmazonTextractDocumentParser").GetValue<string>("SecretAccessKey")
            };

            CredentialProfile credentialProfile = new CredentialProfile("default", credentialProfileOptions);
            credentialProfile.Region = RegionEndpoint.EUWest2;

            NetSDKCredentialsFile netSDKCredentialsFile = new NetSDKCredentialsFile();
            netSDKCredentialsFile.RegisterProfile(credentialProfile);

            AWSOptions awsOptions = this.configuration.GetAWSOptions("AmazonTextractDocumentParser");
            this.textDetectionService = new AmazonTextractTextDetectionService(awsOptions.CreateServiceClient<IAmazonTextract>());

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

            this.logger.LogInformation("Parse content to Document");

            Models.Document document = JsonConvert.DeserializeObject<Models.Document>(content);

            this.logger.LogInformation("Start document text detection with Amazon Textract");

            string jobId = await this.textDetectionService.StartDocumentTextDetection(
                this.configuration.GetSection("AmazonTextractDocumentParser")
                    .GetSection("Bucket")
                    .GetValue<string>("Name"),
                document.Path
            );

            this.logger.LogInformation("Text detected in document, job ID: " + jobId);
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
            var request = new StartDocumentTextDetectionRequest();

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

        public void WaitForJobCompletion(string jobId, int delay = 5000)
        {
            while (!IsJobComplete(jobId))
            {
                this.Wait(delay);
            }
        }

        public bool IsJobComplete(string jobId)
        {
            var response = this.textract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
            {
                JobId = jobId
            });
            response.Wait();
            return !response.Result.JobStatus.Equals("IN_PROGRESS");
        }

        public List<GetDocumentTextDetectionResponse> GetJobResults(string jobId)
        {
            var result = new List<GetDocumentTextDetectionResponse>();
            var response = this.textract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
            {
                JobId = jobId
            });
            response.Wait();
            result.Add(response.Result);
            var nextToken = response.Result.NextToken;
            while (nextToken != null)
            {
                this.Wait();
                response = this.textract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
                {
                    JobId = jobId,
                    NextToken = response.Result.NextToken
                });
                response.Wait();
                result.Add(response.Result);
                nextToken = response.Result.NextToken;
            }
            return result;
        }

        private void Wait(int delay = 5000)
        {
            Task.Delay(delay).Wait();
            Console.Write(".");
        }

        public async Task<DetectDocumentTextResponse> DetectTextS3(string bucketName, string key)
        {
            var result = new DetectDocumentTextResponse();
            var s3Object = new S3Object
            {
                Bucket = bucketName,
                Name = key
            };
            var request = new DetectDocumentTextRequest();
            request.Document = new Document
            {
                S3Object = s3Object
            };
            return await this.textract.DetectDocumentTextAsync(request);
        }

        private void Print(List<Block> blocks)
        {
            blocks.ForEach(x => {
                if (x.BlockType.Equals("LINE"))
                {
                    Console.WriteLine(x.Text);
                }
            });
        }

        public void Print(DetectDocumentTextResponse response)
        {
            if (response != null)
            {
                this.Print(response.Blocks);
            }
        }

        public void Print(List<GetDocumentTextDetectionResponse> response)
        {
            if (response != null && response.Count > 0)
            {
                response.ForEach(r => this.Print(r.Blocks));
            }
        }

        public List<string> GetLines(DetectDocumentTextResponse result)
        {
            var lines = new List<string>();
            result.Blocks.FindAll(block => block.BlockType == "LINE").ForEach(block => lines.Add(block.Text));
            return lines;
        }
    }
}
