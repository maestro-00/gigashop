namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IQuery<GetProductResult>;
public record GetProductResult(IEnumerable<Product> Products);

public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().ToPagedListAsync(request.PageNumber,request.PageSize,cancellationToken);
        
        return new GetProductResult(products);
    }
}