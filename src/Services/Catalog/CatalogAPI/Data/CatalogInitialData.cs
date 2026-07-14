using Marten.Schema;

namespace CatalogAPI.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if(await session.Query<Product>().AnyAsync(cancellation))
            {
                return;
            }

            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        public static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new[]
            {
                new Product { Id = Guid.NewGuid(), Name = "Laptop", Description = "High-performance laptop", Price = 1200.00m, Category = new List<string> { "Electronics" } },
                new Product { Id = Guid.NewGuid(), Name = "Smartphone", Description = "Latest model smartphone", Price = 800.00m, Category = new List<string> { "Electronics" ,} },
                new Product { Id = Guid.NewGuid(), Name = "Desk Chair", Description = "Ergonomic office chair", Price = 200.00m, Category = new List<string> { "Furniture" } },
                new Product { Id = Guid.NewGuid(), Name = "Book", Description = "Programming book", Price = 29.99m, Category = new List<string> { "Books" } }, 
                new Product { Id = Guid.NewGuid(), Name = "Headphones", Description = "Noise-cancelling headphones", Price = 150.00m, Category = new List<string> { "Electronics" } },
                new Product { Id = Guid.NewGuid(), Name = "Coffee Maker", Description = "Automatic coffee maker", Price = 99.99m, Category = new List<string> { "Appliances" } },
                new Product { Id = Guid.NewGuid(), Name = "Backpack", Description = "Durable travel backpack", Price = 79.99m, Category = new List<string> { "Accessories" } },
                new Product { Id = Guid.NewGuid(), Name = "Running Shoes", Description = "Comfortable running shoes", Price = 120.00m, Category = new List<string> { "Footwear" } }

            };
        }
    }
}
