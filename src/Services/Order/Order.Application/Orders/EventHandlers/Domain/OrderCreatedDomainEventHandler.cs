using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Order.Domain.Events;

namespace Order.Application.Orders.EventHandlers.Domain;

public class OrderCreatedDomainEventHandler(IPublishEndpoint publishEndpoint,
    IFeatureManager featureManager,ILogger<OrderCreatedDomainEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent createdEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain event handled: {event}", createdEvent.GetType().Name);
        
        if (await featureManager.IsEnabledAsync("OrderFulfilment"))
        {
            var orderCreatedDto = createdEvent.Order.ToOrderDto();

            await publishEndpoint.Publish(orderCreatedDto, cancellationToken);
        }
    }
}