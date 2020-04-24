using Amazon.Textract;
using Amazon.Textract.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RJM.API.ViewModels;
using RJM.BackgroundTasks.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RJM.BackgroundTasks
{
    public class DocumentParser : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<DocumentParser> logger;

        // RJM.API
        private readonly APIService apiService;

        // RabbitMQ
        private IConnection connection;
        private IModel channel;
        private string exchange = "rjm.background.tasks";
        private string queue = "rjm.background.tasks";
        private string routingKey = "document.parser";

        // Amazon AWS Textract
        private AmazonTextractTextDetectionService textDetectionService;

        public DocumentParser(IConfiguration configuration, ILogger<DocumentParser> logger, APIService apiService, AmazonTextractTextDetectionService textDetectionService)
        {
            this.configuration = configuration;
            this.logger = logger;

            // RJM.API
            this.apiService = apiService;

            // Amazon AWS Textract
            this.textDetectionService = textDetectionService;

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
            List<DocumentContentVM> documentContentVMs = new List<DocumentContentVM>();

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
                // PDF, JPG, PNG
                case "application/pdf":
                case "image/jpeg":
                case "image/png":
                    this.logger.LogInformation($"Amazon Textract - Start text detection of document with ID: {document.Id}");

                    string jobId = await this.textDetectionService.StartDocumentTextDetection(
                        this.configuration.GetSection("AWS")
                            .GetSection("S3")
                            .GetSection("Bucket")
                            .GetValue<string>("Name"),
                        document.Path
                    );

                    this.logger.LogInformation($"Amazon Textract - Created a new job with ID: {jobId}");
                    this.logger.LogInformation("Amazon Textract - Waiting for job completion");

                    await this.textDetectionService.WaitForJobCompletion(jobId);

                    this.logger.LogInformation($"Amazon Textract - Job with ID: {jobId} is completed");
                    this.logger.LogInformation($"Amazon Textract - Retrieving results for job with ID: {jobId}");

                    List<GetDocumentTextDetectionResponse> textDetectionResponses = await this.textDetectionService.GetJobResult(jobId);

                    this.logger.LogInformation("Amazon Textract - Saving results to JSON text file");

                    string jsonResponse = JsonConvert.SerializeObject(textDetectionResponses, Formatting.Indented);
                    File.WriteAllText($"Results/DocumentParser/{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{document.Id}.textracted.json", jsonResponse);

                    this.logger.LogInformation("Amazon Textract - Converting results to document contents");

                    foreach (GetDocumentTextDetectionResponse textDetectionResponse in textDetectionResponses)
                    {
                        foreach (Block block in textDetectionResponse.Blocks)
                        {
                            if (block.BlockType.Equals("LINE") && !string.IsNullOrEmpty(block.Text))
                            {
                                DocumentContentVM newDocumentContentVM = new DocumentContentVM()
                                {
                                    DocumentId = document.Id,
                                    Text = block.Text,
                                    Confidence = block.Confidence
                                };

                                documentContentVMs.Add(newDocumentContentVM);
                            }
                        }
                    }

                    break;
            }

            if (documentContentVMs.Any())
            {
                this.logger.LogInformation($"Text detected in document with ID: {document.Id}");

                this.logger.LogInformation($"Saving results in database through API");

                HttpResponseMessage httpResponseMessage = await this.apiService.DocumentsCreateDocumentContents(document.Id, documentContentVMs);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    this.logger.LogInformation($"Saved in database");
                }
            }
            else
            {
                this.logger.LogInformation($"No text detected in document with ID: {document.Id}");
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
        private IAmazonTextract amazonTextract;

        public AmazonTextractTextDetectionService(IAmazonTextract amazonTextract)
        {
            this.amazonTextract = amazonTextract;
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

            StartDocumentTextDetectionResponse response = await this.amazonTextract.StartDocumentTextDetectionAsync(request);

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
            GetDocumentTextDetectionResponse response = await this.amazonTextract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
            {
                JobId = jobId
            });

            return !response.JobStatus.Equals("IN_PROGRESS");
        }

        public async Task<List<GetDocumentTextDetectionResponse>> GetJobResult(string jobId)
        {
            List<GetDocumentTextDetectionResponse> result = new List<GetDocumentTextDetectionResponse>();

            // Wait for response of Amazon Textract
            GetDocumentTextDetectionResponse response = await this.amazonTextract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
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
                response = await this.amazonTextract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
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
    }
}
