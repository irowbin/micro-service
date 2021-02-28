using System;
using RabbitMQ.Client;

namespace EventBusRabbitMQ
{
    public interface IRabbitMqConnection: IDisposable
    {
        public bool IsConnected { get; }
        public bool TryConnect();
        IModel CreateModel();
    }
}