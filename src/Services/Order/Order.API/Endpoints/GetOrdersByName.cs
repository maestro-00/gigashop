using Order.Application.Orders.Queries.GetOrdersByName;

namespace Order.API.Endpoints;

public record GetOrdersByNameRequest(string Name);

public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);
public class GetOrdersByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
        {
            var query = await sender.Send(new GetOrdersByNameQuery(orderName));

            var response = query.Adapt<GetOrdersByNameResponse>();
            
            return Results.Ok(response);
        }).WithName("GetOrdersByName")
        .Produces<GetOrdersByNameResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Gets order by name.")
        .WithDescription("Gets order by name.");
    }
}