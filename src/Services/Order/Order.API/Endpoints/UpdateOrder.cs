using Order.Application.Orders.Commands.UpdateOrder;

namespace Order.API.Endpoints;

public record UpdateOrderRequest(OrderDto Order);
public record UpdateOrderResponse(bool IsSuccess);
public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
        {  
            var command = request.Adapt<UpdateOrderCommand>();
            var result = await sender.Send(command);
            
            var response = result.Adapt<UpdateOrderResponse>();
            
            return Results.Accepted("/orders",response);
        }).WithName("UpdateOrder")
        .Produces<UpdateOrderResponse>(StatusCodes.Status202Accepted)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Updates Order")
        .WithDescription("Updates order");
    }
}