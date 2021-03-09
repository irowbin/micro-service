using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.data.interfaces;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.repositories.interfaces
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync() =>
            await _context.Products.Find<Product>(p => true).ToListAsync();

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            return await _context.Products.Find(p=>p.Name == name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _context.Products.Find(p => p.Category == category).ToListAsync();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstAsync();
        }

        public Task CreateAsync(Product p)
        {
          return  _context.Products.InsertOneAsync(p);
        }

        public async Task<bool> UpdateAsync(Product p)
        {
            var replaced =  await _context.Products.ReplaceOneAsync(x=> x.Id == p.Id, p);
            return replaced.IsAcknowledged && replaced.ModifiedCount >0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleted = await _context.Products.DeleteOneAsync(d => d.Id == id);
            return deleted.IsAcknowledged && deleted.DeletedCount > 0;
        }
    }
}