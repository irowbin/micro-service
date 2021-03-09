using AutoMapper;
using EventBusRabbitMQ.Events;
using Ordering.Application.Commands;

namespace Ordering.API
{
    public class OrderMapping: Profile
    {
        public OrderMapping()
        {
            CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
        }   
    }
}