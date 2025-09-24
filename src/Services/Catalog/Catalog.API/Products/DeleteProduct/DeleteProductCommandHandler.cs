using BuildingBlocks.Stripe;
using Microsoft.Extensions.Options;
using Stripe;
using Product = Catalog.API.Models.Product;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
    }
}

public class DeleteProductCommandHandler(IDocumentSession session, ProductService productService, IOptions<StripeModel> stripeOptions) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private ProductService _productService = productService;
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(cancellationToken);
        //Delete In Stripe
        StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;

        _productService = new ProductService();
        await _productService.DeleteAsync(command.Id.ToString(), cancellationToken: cancellationToken);
        
        return new DeleteProductResult(true);
    }
}