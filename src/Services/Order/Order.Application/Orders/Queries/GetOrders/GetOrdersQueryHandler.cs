namespace Order.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler(IOrderDbContext context) : IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>
{
    public async Task<GetOrdersQueryResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.Pagination.PageIndex;
        var pageSize = query.Pagination.PageSize;
        var totalCount = await context.Orders.LongCountAsync(cancellationToken);
        var orders = await context.Orders.Include(x => x.OrderItems)
            .OrderBy(x => x.OrderName.Value)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .AsNoTracking().ToListAsync(cancellationToken);
        
        return new(new(pageIndex, pageSize, totalCount, orders.ToOrderDtoList()));
    }
}