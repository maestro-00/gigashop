namespace BuildingBlocks.Stripe;

public class StripeModel
{
    public string? SecretKey { get; set; }
    public string? PublicKey { get; set; }
    public string? CancelUrl { get; set; }
    public string? SuccessUrl { get; set; }
}