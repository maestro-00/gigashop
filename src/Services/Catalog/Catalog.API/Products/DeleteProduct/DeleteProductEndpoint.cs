namespace Catalog.API.Products.DeleteProduct;

// public record DeleteProductRequest(Guid id);

public record DeleteProductResponse(bool IsSuccess);
public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            var response = result.Adapt<DeleteProductResponse> ();
            return Results.Accepted("/products", response);
            
        }).WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status202Accepted)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Product")
        .WithDescription("Delete product");
    }
}