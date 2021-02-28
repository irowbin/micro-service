using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.data.interfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}