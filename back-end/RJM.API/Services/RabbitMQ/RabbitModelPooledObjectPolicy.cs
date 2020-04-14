using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace RJM.API.Services.RabbitMQ
{
    public class RabbitMQPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly IConfiguration configuration;
        private readonly IConnection connection;

        public RabbitMQPooledObjectPolicy(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = this.configuration.GetSection("RabbitMQService").GetValue<string>("HostName"),
                UserName = this.configuration.GetSection("RabbitMQService").GetValue<string>("UserName"),
                Password = this.configuration.GetSection("RabbitMQService").GetValue<string>("Password"),
                Port = this.configuration.GetSection("RabbitMQService").GetValue<int>("Port"),
                VirtualHost = this.configuration.GetSection("RabbitMQService").GetValue<string>("VirtualHost")
            };

            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();

                return false;
            }
        }
    }
}
