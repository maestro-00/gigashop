namespace Catalog.API.Products.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category,int PageNumber = 1, int PageSize = 10) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<Product> Products);

public class GetProductsByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    { 
        var products = await session.Query<Product>().Where(p => p.Category.Contains(request.Category)).ToPagedListAsync(request.PageNumber,request.PageSize,cancellationToken);
        
        return new GetProductsByCategoryResult(products);
    }
}