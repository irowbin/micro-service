using StackExchange.Redis;

namespace Basket.API.Data.Interfaces
{
    public class BasketContext : IBasketContext
    {
        private readonly ConnectionMultiplexer _connection;

        public BasketContext(ConnectionMultiplexer connection)
        {
            _connection = connection;
            Redis = _connection.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}