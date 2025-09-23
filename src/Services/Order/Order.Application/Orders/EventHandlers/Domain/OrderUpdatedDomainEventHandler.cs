using Microsoft.Extensions.Logging;
using Order.Domain.Events;

namespace Order.Application.Orders.EventHandlers.Domain;

public class OrderUpdatedDomainEventHandler(ILogger<OrderUpdatedDomainEventHandler> logger) : INotificationHandler<OrderUpdatedEvent>
{
    public async Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Event: Order Has Been Updated. {Time}", notification.ToString());
        await Task.CompletedTask;
    }
}