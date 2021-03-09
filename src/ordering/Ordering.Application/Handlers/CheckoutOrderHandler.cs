using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mapper;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderHandler: IRequestHandler<CheckoutOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _repository;

        public CheckoutOrderHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderDto> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var m = MapperObject.Mapper.Map<Order>(request);
            var o = await _repository.CreateAsync(m);
            return MapperObject.Mapper.Map<OrderDto>(o);
        }
    }
}