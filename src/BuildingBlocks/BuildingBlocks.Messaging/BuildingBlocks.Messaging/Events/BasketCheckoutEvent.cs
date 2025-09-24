namespace BuildingBlocks.Messaging.Events;

public record BasketCheckoutEvent : IntegrationEvent
{
    public string UserName { get; set; } = default!;
    public Guid CustomerId { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;

    // Shipping and BillingAddress
    public string SerializedShippingAddress { get; set; } = default!;
    public string SerializedBillingAddress { get; set; } = default!;

    // Payment
    public string SerializedPayment { get; set; } = default!;
    
    public string SerializedOrderItems { get; set; } =  default!;
}