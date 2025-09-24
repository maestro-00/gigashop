using System.Text.Json;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Stripe;
using MassTransit;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace Basket.API.Basket.ConfirmCheckout;
public record ConfirmCheckoutCommand(string StripeSessionId) : ICommand<ConfirmCheckoutResult>;

public record ConfirmCheckoutResult(bool IsSuccess);

public class ConfirmCheckoutCommandHandler(IBasketRepository repository, SessionService sessionService, IOptions<StripeModel> stripeOptions, IPublishEndpoint publishEndpoint) : ICommandHandler<ConfirmCheckoutCommand, ConfirmCheckoutResult>
{
    private SessionService _sessionService = sessionService;
    public async Task<ConfirmCheckoutResult> Handle(ConfirmCheckoutCommand request, CancellationToken cancellationToken)
    {
        //Confirming Payment Completion
        _sessionService = new SessionService();

        var session = await _sessionService.GetAsync(request.StripeSessionId, cancellationToken: cancellationToken);

        if (session.PaymentStatus != "paid")
        {
            return new ConfirmCheckoutResult(false);
        }
        //Retrieving basket
        var basket = await repository.GetBasket(session.Metadata["userName"], cancellationToken);
        
        //Creating Checkout Event
        var message = new BasketCheckoutEvent
        {
            CustomerId = Guid.Parse(session.Metadata["customerId"]),
            UserName = session.Metadata["userName"],
            SerializedBillingAddress = session.Metadata["billingAddress"],
            SerializedShippingAddress = session.Metadata["shippingAddress"],
            SerializedPayment = session.Metadata["payment"],
            SerializedOrderItems = JsonSerializer.Serialize(basket.Items),
            TotalPrice = basket.TotalPrice,
        };   
        
        await publishEndpoint.Publish(message, cancellationToken);

        await repository.DeleteBasket(basket.UserName, cancellationToken);

        return new ConfirmCheckoutResult(true);
    }
}