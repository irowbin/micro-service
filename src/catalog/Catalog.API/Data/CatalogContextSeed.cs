using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> p)
        {
            if (!p.Find(c => true).Any())
            {
                p.InsertManyAsync(new []
                {
                    new Product
                    {
                        Name = "S21 ultra",
                        Category = "electric",
                        Summary = "samsung",
                        Description = "desc",
                        ImageFile = "www.google.com",
                        Price =  2.10M
                    },
                    new Product
                    {
                        Name = "iPhone 12 pro max",
                        Category = "electric",
                        Summary = "sum",
                        Description = "desc",
                        ImageFile = "www.google.com",
                        Price = 2.10M
                    }
                });
            }
        }
    }
}