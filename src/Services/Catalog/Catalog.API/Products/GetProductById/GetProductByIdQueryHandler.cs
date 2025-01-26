using Catalog.API.Exceptions;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

public class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQuery> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQueryHandler started with {Query}", request);
        var response = await session.LoadAsync<Product>(request.Id,cancellationToken);
        if (response is null)
        {
            throw new ProductNotFoundException();
        }
        return new GetProductByIdResult(response);
    }
}