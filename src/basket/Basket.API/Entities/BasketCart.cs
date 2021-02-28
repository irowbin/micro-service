using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class BasketCart
    {
        public string               UserName { get; set; }
        public List<BasketCartItem> Items    { get; set; } = new List<BasketCartItem>();

        public BasketCart()
        {

        }

        public BasketCart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice=> Items.Sum(s => s.Price * s.Quantity);
    }
} 