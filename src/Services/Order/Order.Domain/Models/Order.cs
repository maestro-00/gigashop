using Order.Domain.Enums; 

namespace Order.Domain.Models;

public class Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = new();

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public CustomerId CustomerId { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }

    public static Order Create(OrderId id,CustomerId customerId, OrderName orderName, Address billingAddress,
        Address shippingAddress, Payment payment)
    {
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            BillingAddress = billingAddress,
            ShippingAddress = shippingAddress,
            Payment = payment,
            Status = OrderStatus.Pending
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));
        
        return order;
    }
    
    public static Order Update(OrderName orderName, Address billingAddress,
        Address shippingAddress, Payment payment, OrderStatus status)
    {
        var order = new Order
        {  
            OrderName = orderName,
            BillingAddress = billingAddress,
            ShippingAddress = shippingAddress,
            Payment = payment,
            Status = status
        };

        order.AddDomainEvent(new OrderUpdatedEvent(order));
        
        return order;
    }
    
    public void AddItem(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        
        var orderItem = new OrderItem(Id, productId, quantity, price);
        
        _orderItems.Add(orderItem);
    }
    
    public void RemoveItem(ProductId productId)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        
        if (orderItem != null) _orderItems.Remove(orderItem);
    }
    
}