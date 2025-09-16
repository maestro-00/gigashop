using BuildingBlocks.Pagination; 
using Order.Application.Orders.Queries.GetOrders;

namespace Order.API.Endpoints;

// public record GetOrdersRequest(PaginationRequest Request);
public record GetOrdersResponse(PaginationResult<OrderDto> Orders);

public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var query = await sender.Send(new GetOrdersQuery(request));

            var response = query.Adapt<GetOrdersResponse>();
            
            return Results.Ok(response);
        }).WithName("GetOrders")
        .Produces<GetOrdersResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Gets orders")
        .WithDescription("Gets orders");;
    }
}