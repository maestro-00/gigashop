namespace Order.Application.Extensions;

public static class OrderExtensions
{
    public static IEnumerable<OrderDto> ToOrderDtoList(this List<Domain.Models.Order> orders)
    {
        return orders.Select(x => new OrderDto
        (x.Id.Value,x.CustomerId.Value,x.OrderName.Value,
            new AddressDto(x.ShippingAddress.FirstName, x.ShippingAddress.LastName,
                x.ShippingAddress.EmailAddress,
                x.ShippingAddress.AddressLine, x.ShippingAddress.Country, x.ShippingAddress.State,
                x.ShippingAddress.ZipCode),
            new AddressDto(x.BillingAddress.FirstName, x.BillingAddress.LastName,
                x.BillingAddress.EmailAddress,
                x.BillingAddress.AddressLine, x.BillingAddress.Country, x.BillingAddress.State,
                x.BillingAddress.ZipCode),
            new PaymentDto(x.Payment.CardName, x.Payment.CardNumber, x.Payment.Expiration, x.Payment.CVV,
                x.Payment.PaymentMethod),
            x.Status,
            new(x.OrderItems.Select(oi =>
                new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)))
        ));
    }
}