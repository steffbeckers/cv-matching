using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        public AmazonTextractDocumentParser(IConfiguration configuration, ILogger<AmazonTextractDocumentParser> logger)
        {
            this.configuration = configuration;
            this.logger = logger;

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

            consumer.Received += (ch, ea) =>
            {
                // Received message  
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);

                // Handle the received message  
                HandleMessage(content);

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

        private void HandleMessage(string content)
        {
            this.logger.LogInformation($"Received: {content}");
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
}
