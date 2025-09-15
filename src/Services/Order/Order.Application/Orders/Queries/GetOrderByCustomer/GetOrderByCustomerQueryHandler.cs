namespace Order.Application.Orders.Queries.GetOrderByCustomer;

public class GetOrderByCustomerQueryHandler(IOrderDbContext context) : IQueryHandler<GetOrderByCustomerQuery, GetOrderByCustomerResult>
{
    public async Task<GetOrderByCustomerResult> Handle(GetOrderByCustomerQuery query, CancellationToken cancellationToken)
    {
        var orders = await context.Orders.Include(x => x.OrderItems)
            .Where(x => x.CustomerId == CustomerId.Of(query.CustomerId))
            .OrderBy(x => x.OrderName.Value)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new(orders.ToOrderDtoList());
    }
}