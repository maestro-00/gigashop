using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation)) return;

        session.Store(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>()
        {
            new()
            {
                Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                Name = "Premium Wireless Headphones",
                Description =
                    "High-quality wireless headphones with noise cancellation and premium sound quality. Perfect for music lovers and professionals.",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1484704849700-f032a568e944?w=800&h=600&fit=crop",
                    "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=800&h=600&fit=crop"
                },
                Sizes = new List<string> { "One Size" },
                Price = 299.99M,
                Category = new List<string> { "Electronics" },
                InStock = true,
                Rating = 4.8M,
                ReviewCount = 124,
                Colors = new List<Color>
                {
                    new Color("Black", "#000000"),
                    new Color("White", "#FFFFFF"),
                    new Color("Silver", "#C0C0C0")
                }
            },
            new()
            {
                Id = new Guid("b6a9d14d-26ff-45d3-ae0e-9386cfc3c442"),
                Name = "Ergonomic Office Chair",
                Description =
                    "Comfortable ergonomic office chair with adjustable height, lumbar support, and breathable mesh back. Ideal for long working hours.",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1598300053650-8c65f75e3d0f?w=800&h=600&fit=crop"
                },
                Sizes = new List<string> { "Standard" },
                Price = 189.50M,
                Category = new List<string> { "Furniture" },
                InStock = true,
                Rating = 4.5M,
                ReviewCount = 87,
                Colors = new List<Color>
                {
                    new Color("Black", "#000000"),
                    new Color("Gray", "#808080")
                }
            },
            new()
            {
                Id = new Guid("e93745d2-7fae-4f31-802a-89c469afc1c8"),
                Name = "Smartwatch Series X",
                Description =
                    "Next-gen smartwatch with health tracking, GPS, and customizable watch faces. Water-resistant and compatible with iOS and Android.",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1511732351157-1865efcb7b7b?w=800&h=600&fit=crop",
                    "https://images.unsplash.com/photo-1516222338250-863216ce01f2?w=800&h=600&fit=crop"
                },
                Sizes = new List<string> { "Small", "Medium", "Large" },
                Price = 249.00M,
                Category = new List<string> { "Wearables", "Electronics" },
                InStock = true,
                Rating = 4.6M,
                ReviewCount = 210,
                Colors = new List<Color>
                {
                    new Color("Black", "#000000"),
                    new Color("Blue", "#0000FF"),
                    new Color("Rose Gold", "#B76E79")
                }
            }
        };

}