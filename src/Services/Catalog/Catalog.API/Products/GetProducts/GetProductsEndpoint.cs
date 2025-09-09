namespace Catalog.API.Products.GetProducts;

public record GetProductRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters]GetProductRequest request,ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>();
            var products = await sender.Send(query);
            return Results.Ok(products);
        }).WithName("GetProducts")
        .Produces<GetProductsResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get products")
        .WithDescription("Gets all products");

    }
}