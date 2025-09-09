namespace Catalog.API.Products.GetProductsByCategory;

public record GetProductsByCategoryRequest(int? PageNumber = 1, int? PageSize = 10);

public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category,[AsParameters] GetProductsByCategoryRequest request, ISender sender) =>
            { 
                var query = request.Adapt<GetProductsByCategoryQuery>() with { Category = category };
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductsByCategoryResponse>();
            return Results.Ok(response);
        }).WithName("GetProductsByCategory")
        .Produces<GetProductsByCategoryResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get product by category")
        .WithDescription("Gets a single product by category");
    }
}