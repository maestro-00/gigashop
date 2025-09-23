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

    public static OrderDto ToOrderDto(this Domain.Models.Order order)
    {
        var shippingAddressDto = new AddressDto(order.ShippingAddress.FirstName, order.ShippingAddress.LastName,
            order.ShippingAddress.EmailAddress, order.ShippingAddress.AddressLine, order.ShippingAddress.Country,
            order.ShippingAddress.State, order.ShippingAddress.ZipCode);
        
        var billingAddressDto = new AddressDto(order.BillingAddress.FirstName, order.BillingAddress.LastName,
            order.BillingAddress.EmailAddress, order.BillingAddress.AddressLine, order.BillingAddress.Country,
            order.BillingAddress.State, order.BillingAddress.ZipCode);

        var paymentDto = new PaymentDto(order.Payment.CardName, order.Payment.CardNumber, 
            order.Payment.Expiration, order.Payment.CVV,
            order.Payment.PaymentMethod);
        
        var orderItemsDto = order.OrderItems.Select(oi => 
            new OrderItemDto(oi.OrderId.Value,oi.ProductId.Value,oi.Quantity,oi.Price)).ToList();
        
        return new OrderDto(order.Id.Value, order.CustomerId.Value, order.OrderName.Value,
            shippingAddressDto,
            billingAddressDto, paymentDto, order.Status, orderItemsDto);
    }
}