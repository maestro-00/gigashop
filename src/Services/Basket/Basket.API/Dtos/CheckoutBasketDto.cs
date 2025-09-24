namespace Basket.API.Dtos;

public class CheckoutBasketDto
{
    public string UserName { get; set; } = default!;
    public Guid CustomerId { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;

    // Shipping and BillingAddress
    public ShippingAddressDto ShippingAddress { get; set; } = default!;
    public BillingAddressDto BillingAddress { get; set; } = default!;

    // Payment
    public PaymentDto Payment { get; set; } = default!; 
}