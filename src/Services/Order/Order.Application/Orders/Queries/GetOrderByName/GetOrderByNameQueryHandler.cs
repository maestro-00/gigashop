namespace Order.Application.Orders.Queries.GetOrderByName;

public class GetOrderByNameQueryHandler(IOrderDbContext context) : IQueryHandler<GetOrderByNameQuery, GetOrderByNameResult>
{
    public async Task<GetOrderByNameResult> Handle(GetOrderByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await context.Orders.Include(x => x.OrderItems)
            .Where(x => x.OrderName == OrderName.Of(query.OrderName))
            .OrderBy(x => x.OrderName.Value)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new(orders.ToOrderDtoList());
    }
}