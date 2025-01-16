using BuildingBlocks.CQRS;
using Catalog.API.Models; 

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    List<string> Category,
    string ImageFile) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
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
        //Returning Results
        return new CreateProductResult(Guid.NewGuid()); 
    }
}