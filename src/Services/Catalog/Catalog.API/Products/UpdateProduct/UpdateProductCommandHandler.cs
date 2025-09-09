using Catalog.API.Exceptions;

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
public class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    { 
        var productToUpdate = await session.LoadAsync<Product>(command.Id,cancellationToken);

        if (productToUpdate is null) throw new ProductNotFoundException(command.Id);
        
        productToUpdate.Name = command.Name;
        productToUpdate.Description = command.Description;
        productToUpdate.Price = command.Price;
        productToUpdate.Category = command.Category;
        productToUpdate.ImageFile = command.ImageFile;
        
        session.Update(productToUpdate);
        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }
}