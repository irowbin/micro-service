using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.repositories.interfaces;
using EventBusRabbitMQ.Data;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository         _repository;
        private readonly ILogger<BasketController> _logger;
        private readonly IMapper                   _mapper;
        private readonly EventBusRabbitMqProducer  _eventBus;

        public BasketController(ILogger<BasketController> logger, IBasketRepository repository, IMapper mapper, EventBusRabbitMqProducer eventBus)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> Get(string username)
        {
            var bc = await _repository.GetBasketAsync(username);
            return Ok(bc ?? new BasketCart(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK),
         ProducesResponseType(typeof(bool), (int) HttpStatusCode.NotFound)]
        public async Task< ActionResult<BasketCart>> UpdateBasket([FromBody]BasketCart b)
        {
            var updated = await _repository.UpdateAsync(b);
            return updated is null ? BadRequest(b) :  Ok(updated);
        }
        
        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK),
         ProducesResponseType(typeof(bool), (int) HttpStatusCode.NotFound)]
        public async  Task<ActionResult<bool>> Delete(string username)
        {
            var isDeleted = await _repository.DeleteAsync(username);
            return isDeleted ? Ok(true) : NotFound();
        }
        
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.Accepted),
         ProducesResponseType(typeof(bool), (int) HttpStatusCode.BadRequest),
         ProducesResponseType(typeof(bool), (int) HttpStatusCode.NotFound)]
        public async  Task<ActionResult<BasketCheckout>> Checkout(BasketCheckout b)
        {
            var en = await _repository.GetBasketAsync(b.UserName);
            if (en is null) return NotFound("There is no such basket for this user.");
            var isDeleted = await _repository.DeleteAsync(b.UserName);
            if (!isDeleted) return BadRequest("Deletion is unsuccessful.");
            var p = _mapper.Map<BasketCheckoutEvent>(b);
            p.RequestId = Guid.NewGuid();
            p.TotalPrice = b.TotalPrice;
            _eventBus.PublishBasketCheckout(EventBusEnum.BASKET_CHECKOUT_QUEUE, p);
            return Accepted();
        }
    }
}