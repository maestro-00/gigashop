using System.Text.Json;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Application.Orders.Commands.CreateOrder;
using Order.Domain.Enums;

namespace Order.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger) : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        logger.LogInformation("Integration event handled: {event}", context.GetType().Name);
        var createOrderCommand = MapToCreateOrderCommand(context.Message);

        await sender.Send(createOrderCommand);
        
    }
    
    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent checkoutEvent)
    {
        var paymentDto = JsonSerializer.Deserialize<PaymentDto>(checkoutEvent.SerializedPayment);
        var shippingAddress = JsonSerializer.Deserialize<AddressDto>(checkoutEvent.SerializedShippingAddress);
        var billingAddress = JsonSerializer.Deserialize<AddressDto>(checkoutEvent.SerializedBillingAddress);
        var orderItems = JsonSerializer.Deserialize<List<OrderItemDto>>(checkoutEvent.SerializedOrderItems);

        var orderDto = new OrderDto(
            Guid.NewGuid(), checkoutEvent.CustomerId, checkoutEvent.UserName, shippingAddress,
            billingAddress, paymentDto, OrderStatus.Pending, orderItems);

        return new CreateOrderCommand(orderDto);
    }
}