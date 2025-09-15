namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
    {
        var cart = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);
        
        return cart ?? throw new BasketNotFoundException(userName);
    }

    public async Task<string> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken)
    {
        session.Store(basket);
        await session.SaveChangesAsync(cancellationToken);
        return basket.UserName;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
    {
        session.Delete<ShoppingCart>(userName);
        await  session.SaveChangesAsync(cancellationToken);
        return true;
    }
}