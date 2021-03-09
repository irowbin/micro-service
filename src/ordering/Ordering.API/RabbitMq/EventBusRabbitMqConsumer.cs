using System.Text;
using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Data;
using EventBusRabbitMQ.Events;
using MediatR;
using Ordering.Core.Repositories;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using Ordering.Application.Commands;

namespace Ordering.API.RabbitMq
{
    public class EventBusRabbitMqConsumer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IMediator           _mediator;
        private readonly IMapper             _mapper;
        private readonly IOrderRepository    _repository;

        public EventBusRabbitMqConsumer(IRabbitMqConnection connection,
                                        IMediator mediator,
                                        IMapper mapper,
                                        IOrderRepository repository)
        {
            _connection = connection;
            _mediator   = mediator;
            _mapper     = mapper;
            _repository = repository;
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: EventBusEnum.BASKET_CHECKOUT_QUEUE.ToString(), durable: false, false, false,
                                 null);
            var con = new EventingBasicConsumer(channel);
            con.Received += ReceivedEvent;
            channel.BasicConsume(queue: EventBusEnum.BASKET_CHECKOUT_QUEUE.ToString(), autoAck: true, consumer: con,
                                 noLocal: false, exclusive: false, arguments: null);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey != EventBusEnum.BASKET_CHECKOUT_QUEUE.ToString()) return;
            var message        = Encoding.UTF8.GetString(e.Body.Span);
            var basketCheckout = JsonSerializer.Deserialize<BasketCheckoutEvent>(message);
            var commend        = _mapper.Map<CheckoutOrderCommand>(basketCheckout);
            var result         = await _mediator.Send(commend);
        }

        public void Disconnect() => _connection.Dispose();
    }
}