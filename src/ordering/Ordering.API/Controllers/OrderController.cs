using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IMediator                _mediator;

        public OrderController(ILogger<OrderController> logger, IMediator mediator)
        {
            _logger   = logger;
            _mediator = mediator;
        }

        [HttpGet,
        ProducesResponseType(typeof(IEnumerable<OrderDto>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get(string username)
        {
            var u = new GetOrderByUserName(username);
            var o = await _mediator.Send(u);

            return Ok(o);
        }
        
        [HttpPost,
         ProducesResponseType(typeof(OrderDto), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<OrderDto>> CheckoutOrder(CheckoutOrderCommand command)
        {
            var o = await _mediator.Send(command);
            return Ok(o);
        }
    }
}