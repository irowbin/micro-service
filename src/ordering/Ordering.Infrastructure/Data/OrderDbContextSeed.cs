using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderDbContextSeed
    {
        public static async Task SeedAsync(OrderDbContext context)
        {
            try
            {
                await context.Database.MigrateAsync();
               await context.Database.EnsureCreatedAsync();

                if (!(await context.Orders.AnyAsync()))
                {
                    await context.Orders.AddRangeAsync(new List<Order>
                    {
                        new()
                        {
                            FirstName = "Robin",
                            EmailAddress = "test@mail.com",
                            Country = "Nepal",
                            Expiration = "12-12-2025",
                            State = "Province 3",
                            AddressLine = "hills 5th",
                            CardName = "PayPal",
                            CardNumber = "1234",
                            LastName = "n/a",
                            PaymentMethod = 1,
                            TotalPrice = 20,
                            UserName = "robin",
                            ZipCode = "1234",
                            CVV = "SSDS"

                        }
                    });
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}