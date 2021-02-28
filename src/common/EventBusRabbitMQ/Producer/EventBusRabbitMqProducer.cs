using System;
using System.Text;
using System.Text.Json;
using EventBusRabbitMQ.Data;
using EventBusRabbitMQ.Events;

namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMqProducer
    {
        private readonly IRabbitMqConnection _mqConnection;

        public EventBusRabbitMqProducer(IRabbitMqConnection mqConnection)
        {
            _mqConnection = mqConnection;
        }

        public void PublishBasketCheckout(EventBusEnum queueName, BasketCheckoutEvent publishModel)
        {
            using var channel = _mqConnection.CreateModel();
            channel.QueueDeclare(queueName.ToString(), durable: false, exclusive: false, autoDelete: false, arguments: null);
            var message = JsonSerializer.Serialize(publishModel);
            var body = Encoding.UTF8.GetBytes(message);

            var bp = channel.CreateBasicProperties();
            bp.Persistent = true;
            bp.DeliveryMode = 2;
            channel.ConfirmSelect();
            channel.BasicPublish(exchange: "", routingKey: queueName.ToString(), mandatory: true, basicProperties: bp, body: body);
            channel.WaitForConfirmsOrDie();
            channel.BasicAcks += (sender, args) => { Console.WriteLine($"sent rabbit MQ {publishModel}"); };
            channel.ConfirmSelect();
        }
    }
}