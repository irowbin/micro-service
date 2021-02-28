using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.repositories.interfaces
{
    public interface IBasketRepository
    {
        Task<BasketCart> GetBasketAsync(string username);
        Task<BasketCart> UpdateAsync(BasketCart b);
        Task<bool> DeleteAsync(string username);
    }
}