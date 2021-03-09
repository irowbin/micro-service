using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Mapper;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class GetOrderByUserNameHandler : IRequestHandler<GetOrderByUserName, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _repository;

        public GetOrderByUserNameHandler(IOrderRepository repository)
        {
            _repository  = repository;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrderByUserName request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetOrderByUserNameAsync(request.UserName);
            return MapperObject.Mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }
}