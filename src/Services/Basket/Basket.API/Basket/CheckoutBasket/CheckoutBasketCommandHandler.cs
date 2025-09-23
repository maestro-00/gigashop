using System.Text.Json;
using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(CheckoutBasketDto Basket) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.Basket).NotNull().NotEmpty().WithMessage("Basket must not be empty");
        RuleFor(b => b.Basket.UserName).NotEmpty().WithMessage("Username is required.");
    }
}

public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(request.Basket.UserName, cancellationToken);

        if (basket == null)
        {
            return new CheckoutBasketResult(false);
        }

        var message = request.Basket.Adapt<BasketCheckoutEvent>();
        var serializedOrderItems = JsonSerializer.Serialize(basket.Items);
        message.SerializedOrderItems = serializedOrderItems;
        message.TotalPrice = basket.TotalPrice;
        
        await publishEndpoint.Publish(message, cancellationToken);

        await repository.DeleteBasket(request.Basket.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}