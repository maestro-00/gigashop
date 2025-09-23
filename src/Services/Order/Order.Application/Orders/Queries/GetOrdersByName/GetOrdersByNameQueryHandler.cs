namespace Order.Application.Orders.Queries.GetOrdersByName;

public class GetOrdersByNameQueryHandler(IOrderDbContext context) : IQueryHandler<GetOrdersByNameQuery, GetOrderByNameResult>
{
    public async Task<GetOrderByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await context.Orders.Include(x => x.OrderItems)
            .Where(x => x.OrderName == OrderName.Of(query.OrderName))
            .OrderBy(x => x.OrderName.Value)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new(orders.ToOrderDtoList());
    }
}