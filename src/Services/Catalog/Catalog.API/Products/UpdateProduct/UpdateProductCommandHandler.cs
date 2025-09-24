using BuildingBlocks.Stripe;
using Catalog.API.Exceptions;
using Microsoft.Extensions.Options;
using Stripe;
using Product = Catalog.API.Models.Product;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price is required");
    }
}
public class UpdateProductCommandHandler(IDocumentSession session, ProductService productService, IOptions<StripeModel> stripeOptions) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private ProductService _productService = productService;
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    { 
        var productToUpdate = await session.LoadAsync<Product>(command.Id,cancellationToken);

        if (productToUpdate is null) throw new ProductNotFoundException(command.Id);
        
        productToUpdate.Name = command.Name;
        productToUpdate.Description = command.Description;
        productToUpdate.Price = command.Price;
        productToUpdate.Category = command.Category;
        productToUpdate.ImageFile = command.ImageFile;
        
        //Stripe
        StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;
        _productService = new ProductService();
        
        //Deleting Existing Stripe Product
        await _productService.DeleteAsync(command.Id.ToString(), cancellationToken: cancellationToken);
        
        //Creating New Stripe Product
        var options = new ProductCreateOptions { 
            Id = productToUpdate.Id.ToString(),
            Name = productToUpdate.Name,
            Description = productToUpdate.Description,
            Images = [productToUpdate.ImageFile],
            DefaultPriceData = new ProductDefaultPriceDataOptions
            {
                Currency = "USD",
                UnitAmountDecimal = productToUpdate.Price * 100
            }
        };
        var stripeProduct = await _productService.CreateAsync(options, cancellationToken: cancellationToken);
        productToUpdate.StripePriceId = stripeProduct.DefaultPriceId;
        
        //Updating To Database
        session.Update(productToUpdate);
        await session.SaveChangesAsync(cancellationToken);
        
        return new UpdateProductResult(true);
    }
}