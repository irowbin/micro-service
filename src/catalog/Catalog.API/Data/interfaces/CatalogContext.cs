using Catalog.API.Entities;
using Catalog.API.settings;
using MongoDB.Driver;

namespace Catalog.API.data.interfaces
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(ICatalogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Products = database.GetCollection<Product>(settings.CollectionName);
            CatalogContextSeed.SeedData(Products);

        }

        public IMongoCollection<Product> Products { get; }
    }
}