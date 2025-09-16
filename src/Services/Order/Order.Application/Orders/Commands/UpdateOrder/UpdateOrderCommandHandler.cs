namespace Order.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler(IOrderDbContext context) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Order.Id);
        var order = await context.Orders.FindAsync([orderId], cancellationToken: cancellationToken);
        if (order == null) throw new OrderNotFoundException(command.Order.Id);
        
        UpdateOrderWithNewValues(order, command.Order);
        
        context.Orders.Update(order);
        await context.SaveChangesAsync(cancellationToken);

        return new(true);
    }

    private void UpdateOrderWithNewValues(Domain.Models.Order order, OrderDto orderDto)
    {
        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName
            , orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country
            , orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        
        var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName
            , orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country
            , orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

        var payment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration,
            orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

        order.Update(
            OrderName.Of(orderDto.OrderName),
            billingAddress,
            shippingAddress,
            payment,
            orderDto.Status
        );
    } 
}