using BuildingBlocks.Stripe;
using Microsoft.Extensions.Options;
using Stripe;
using Product = Catalog.API.Models.Product;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    List<string> Category,
    string ImageFile) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price is required");
    }
}

public class CreateProductCommandHandler(IDocumentSession session, ProductService productService, IOptions<StripeModel> stripeOptions) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private ProductService _productService = productService;

    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    { 
        //Creating Product Entity
        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Category = command.Category,
            ImageFile = command.ImageFile
        };
        //Saving To Database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        //Store in Stripe
        StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;
        
        var options = new ProductCreateOptions { 
            Id = product.Id.ToString(),
            Name = product.Name,
            Description = product.Description,
            Images = [product.ImageFile],
            DefaultPriceData = new ProductDefaultPriceDataOptions
            {
                Currency = "USD",
                UnitAmountDecimal = product.Price * 100
            }
        };
        _productService = new ProductService();
        var stripeProduct = await _productService.CreateAsync(options, cancellationToken: cancellationToken);
        product.StripePriceId = stripeProduct.DefaultPriceId;
        
        //Saving Stripe Product Price Id
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        
        //Returning Results
        return new CreateProductResult(product.Id); 
    }
}