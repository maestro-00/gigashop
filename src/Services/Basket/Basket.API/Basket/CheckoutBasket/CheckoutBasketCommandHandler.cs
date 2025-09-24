using System.Text.Json;
using Basket.API.Dtos; 
using BuildingBlocks.Stripe; 
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(CheckoutBasketDto CheckoutDto) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess, string? Url = null, string? StripeSessionId = null);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.CheckoutDto).NotNull().NotEmpty().WithMessage("Basket must not be empty");
        RuleFor(b => b.CheckoutDto.UserName).NotEmpty().WithMessage("Username is required.");
        RuleFor(b => b.CheckoutDto.BillingAddress).NotNull().WithMessage("Billing Address is required.");
        RuleFor(b => b.CheckoutDto.ShippingAddress).NotNull().WithMessage("Shipping Address is required.");
        RuleFor(b => b.CheckoutDto.Payment).NotNull().WithMessage("Payment information is required.");
    }
}

public class CheckoutBasketCommandHandler(IBasketRepository repository, SessionService sessionService, IOptions<StripeModel> stripeOptions) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    private SessionService _sessionService = sessionService;
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(request.CheckoutDto.UserName, cancellationToken);

        if (basket == null)
        {
            return new CheckoutBasketResult(false);
        }
        
        //Checkout With Stripe
        StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;

        var lineItems = basket.Items.Select(item => new SessionLineItemOptions
        {
            Quantity = item.Quantity,
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = "USD",
                UnitAmountDecimal = item.Price * 100,
                Product = item.ProductId.ToString(),
            }
        }).ToList();
        
        var options = new SessionCreateOptions
        {
            LineItems = lineItems,
            Mode = "payment",
            CustomerEmail = request.CheckoutDto.BillingAddress.EmailAddress,
            CancelUrl = stripeOptions.Value.CancelUrl,
            SuccessUrl = stripeOptions.Value.SuccessUrl,
            Metadata = new Dictionary<string, string>
            {
                { "customerId", request.CheckoutDto.CustomerId.ToString()},
                {"userName", request.CheckoutDto.UserName},
                { "shippingAddress", JsonSerializer.Serialize(request.CheckoutDto.ShippingAddress) },
                { "billingAddress", JsonSerializer.Serialize(request.CheckoutDto.BillingAddress)},
                { "payment", JsonSerializer.Serialize(request.CheckoutDto.Payment)}
            }
        };
        _sessionService = new SessionService();
        var session = await _sessionService.CreateAsync(options, cancellationToken: cancellationToken);
        
        return new CheckoutBasketResult(true, session.Url, session.Id);
    }
}