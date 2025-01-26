namespace Catalog.API.Products.GetProductById;

//public record GetProductByIdRequest();

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetProductByIdQuery(id);
            var response = await sender.Send(query);
            var result = response.Adapt<GetProductByIdResponse>();
            return Results.Ok(result);
        }).WithName("GetProductById")
        .Produces<GetProductByIdResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get product by id")
        .WithDescription("Gets a single product by id");
    }
}