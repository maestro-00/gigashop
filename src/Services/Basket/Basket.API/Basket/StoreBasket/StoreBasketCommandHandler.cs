using Discount.Grpc;
using Grpc.Core;

namespace Basket.API.Basket.StoreBasket;
public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart is required");
        RuleFor(x => x.Cart.UserName).NotNull().WithMessage("Username is required");
    }
}
public class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command.Cart, cancellationToken);
        await repository.StoreBasket(command.Cart, cancellationToken);
        return new StoreBasketResult(command.Cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            try
            {
                var coupon = await discountProto.GetDiscountAsync(new() { ProductName = item.Product.Name },
                    cancellationToken: cancellationToken);
                
                item.Product.Price -= coupon.Amount;
            }
            catch (RpcException e)
            {
                Console.WriteLine(e); 
            }
        }
    }
}