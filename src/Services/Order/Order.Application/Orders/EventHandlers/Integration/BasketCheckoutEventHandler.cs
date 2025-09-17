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
        var paymentDto = new PaymentDto(checkoutEvent.CardName, checkoutEvent.CardNumber,
            checkoutEvent.Expiration, checkoutEvent.CVV, checkoutEvent.PaymentMethod);
        var addressDto = new AddressDto(checkoutEvent.FirstName, checkoutEvent.LastName,
            checkoutEvent.EmailAddress, checkoutEvent.AddressLine, checkoutEvent.Country,
            checkoutEvent.State, checkoutEvent.ZipCode);

        var orderItems = JsonSerializer.Deserialize<List<OrderItemDto>>(checkoutEvent.SerializedOrderItems);
        // orderItems.ForEach(oi => oi.OrderId = o);

        var orderDto = new OrderDto(
            Guid.NewGuid(), checkoutEvent.CustomerId, checkoutEvent.UserName, addressDto,
            addressDto, paymentDto, OrderStatus.Pending, orderItems);

        return new CreateOrderCommand(orderDto);
    }
}