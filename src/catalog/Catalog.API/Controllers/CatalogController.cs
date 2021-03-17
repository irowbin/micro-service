using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.repositories.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository           _product;

        public ProductController(ILogger<ProductController> logger, IProductRepository product)
        {
            _logger = logger;
            _product = product;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            var prod = await _product.GetProductsAsync();
            return Ok(prod);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK),
         ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var prod = await _product.GetProductAsync(id);
            if (prod is null) return NotFound();
            return Ok(prod);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> Create(Product p)
        {
           await _product.CreateAsync(p);

           return Created(new Uri("", UriKind.Relative), p);
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Update(Product p)
        {
            await _product.UpdateAsync(p);

            return Ok(p);
        }
        
        [HttpGet("ByName/{name}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK),
         ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetByName(string name)
        {
            var prod = await _product.GetProductsByNameAsync(name);
            if (prod is null) return NotFound();
            return Ok(prod);
        }
        
        [HttpGet("ByCategory/{category}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK),
         ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetByCategory(string category)
        {
            var prod = await _product.GetProductsByCategoryAsync(category);
            if (prod is null) return NotFound();
            return Ok(prod);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK),
         ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var prod = await _product.DeleteAsync(id);
            if (!prod) return NotFound();
            return Ok(true);
        }

    }
}