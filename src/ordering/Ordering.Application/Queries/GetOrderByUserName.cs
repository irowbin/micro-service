using System.Collections.Generic;
using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries
{
    public class GetOrderByUserName: IRequest<IEnumerable<OrderDto>>
    {
        public string UserName { get; set; }

        public GetOrderByUserName(string userName)
        {
            UserName = userName;
        }
    }
}