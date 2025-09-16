using Order.Application.Orders.Commands.DeleteOrder;

namespace Order.API.Endpoints;

// public record DeleteOrderRequest(Guid OrderId);
public record DeleteOrderResponse(bool IsSuccess);
public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id:guid}", async (Guid id, ISender sender) =>
        { 
            var result = await sender.Send(new DeleteOrderCommand(id));
            
            var response = result.Adapt<DeleteOrderResponse>();
            return Results.Accepted($"/orders", response);
        }).WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status202Accepted)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Order")
        .WithDescription("Delete order");
    }
}