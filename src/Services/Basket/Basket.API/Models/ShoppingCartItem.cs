namespace Basket.API.Models;

public class ShoppingCartItem
{
    public Product Product {
        get;
        set;
    }
    public string Size { get; set; } = default!; 
    public int Quantity { get; set; } = default!;
    public Color Color { get; set; } = default!;
}