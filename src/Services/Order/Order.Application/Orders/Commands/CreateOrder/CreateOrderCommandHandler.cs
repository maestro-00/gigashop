namespace Order.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderDbContext context) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateOrder(command.Dto);
        
        context.Orders.Add(order);
        await context.SaveChangesAsync(cancellationToken);

        return new (order.Id.Value);
    }

    private Domain.Models.Order CreateOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName
        , orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country
        , orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        
        var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName
        , orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country
        , orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

        var payment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration,
            orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);
        
        var order = Domain.Models.Order.Create(
            OrderId.Of(orderDto.Id),
            CustomerId.Of(orderDto.CustomerId),
            OrderName.Of(orderDto.OrderName),
            billingAddress,
            shippingAddress,
            payment
        );
        
        return order;
    }
}