namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(string Name, string Description, decimal Price, List<string> Category,
    List<string> Images,
    List<string> Sizes,
    bool InStock,
    decimal Rating,
    int ReviewCount,
    List<Color> Colors);

public record UpdateProductResponse(bool IsSuccess);
public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id:guid}", async (Guid id, UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>() with { Id = id }; 
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateProductResponse>();
            return Results.Accepted("/products", response);
        }).WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status202Accepted)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Product")
        .WithDescription("Update product");
    }
}