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

public class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateProductCommandHandler started with {Command}", command);
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
        //Returning Results
        return new CreateProductResult(product.Id); 
    }
}