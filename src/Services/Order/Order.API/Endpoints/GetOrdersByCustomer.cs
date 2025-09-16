using Order.Application.Orders.Queries.GetOrdersByCustomer;

namespace Order.API.Endpoints;
// public record GetOrdersByCustomerRequest(Guid CustomerId);

public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{customerId:guid}", async (Guid customerId, ISender sender) =>
        {
            var query = await sender.Send(new GetOrdersByCustomerQuery(customerId));
            
            var response = query.Adapt<GetOrderByCustomerResult>();
            
            return Results.Ok(response);
        }).WithName("GetOrdersByCustomer")
        .Produces<GetOrderByCustomerResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Gets order by customer.")
        .WithDescription("Gets order by customer.");
    }
}