using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Basket.API.Data.Interfaces;
using Basket.API.Entities;

namespace Basket.API.repositories.interfaces
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context;
        }
         private readonly JsonSerializerOptions DefaultOptions =  new()
         {
             DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
             
         };


        public  async Task<BasketCart> GetBasketAsync(string username)
        {
            var b = await _context.Redis.StringGetAsync(username ?? "");

            return b.IsNullOrEmpty
                ? new BasketCart(username)
                : JsonSerializer.Deserialize<BasketCart>(b, DefaultOptions);

        }

        public async Task<BasketCart> UpdateAsync(BasketCart b)
        {
            var updated = await _context.Redis.StringSetAsync(b.UserName, JsonSerializer.Serialize(b));
            return updated ? await GetBasketAsync(b.UserName) : null;
        }

        public Task<bool> DeleteAsync(string username) => _context.Redis.KeyDeleteAsync(username);
    }
}