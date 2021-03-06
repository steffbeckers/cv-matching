﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RJM.API.Services.RabbitMQ
{
    public class RabbitMQService
    {
        private readonly IConfiguration configuration;
        private readonly DefaultObjectPool<IModel> objectPool;

        public RabbitMQService(IConfiguration configuration, IPooledObjectPolicy<IModel> objectPolicy)
        {
            this.configuration = configuration;
            objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        public void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class
        {
            if (message == null)
                return;

            // Name suffixes
            exchangeName += this.configuration.GetSection("RabbitMQService").GetValue<string>("Suffix");
            routeKey += this.configuration.GetSection("RabbitMQService").GetValue<string>("Suffix");

            IModel channel = objectPool.Get();

            try
            {
                channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);

                byte[] sendBytes = Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        message,
                        Formatting.None,
                        new JsonSerializerSettings()
                        {
                            MaxDepth = 1,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }
                    )
                ); ;

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchangeName, routeKey, properties, sendBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objectPool.Return(channel);
            }
        }
    }
}
