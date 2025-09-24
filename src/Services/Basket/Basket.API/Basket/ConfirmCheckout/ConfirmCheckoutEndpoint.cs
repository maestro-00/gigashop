namespace Basket.API.Basket.ConfirmCheckout;

// public record ConfirmCheckoutRequest(string StripeSessionId);
public record ConfirmCheckoutResponse(bool IsSuccess);
    
public class ConfirmCheckoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout/{sessionId}", async (string sessionId, ISender sender) =>
        {
            var command = new ConfirmCheckoutCommand(sessionId);

            var result = await sender.Send(command);

            var response = result.Adapt<ConfirmCheckoutResponse>();

            return Results.Ok(response);
        }).WithName("Confirm Checkout")
        .Produces<ConfirmCheckoutResponse>(StatusCodes.Status202Accepted)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Confirms basket Checkout")
        .WithDescription("Confirms basket checkout");
    }
}