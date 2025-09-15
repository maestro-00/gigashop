using Microsoft.Extensions.Logging;
using Order.Domain.Events;

namespace Order.Application.Orders.EventHandlers;

public class OrderCreatedDomainEventHandler(ILogger<OrderCreatedDomainEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Event: Order Has Been Created. {Time}", notification.ToString());
        await Task.CompletedTask;
    }
}