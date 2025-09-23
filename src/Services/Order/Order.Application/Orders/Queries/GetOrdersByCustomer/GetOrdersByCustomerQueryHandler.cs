namespace Order.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerQueryHandler(IOrderDbContext context) : IQueryHandler<GetOrdersByCustomerQuery, GetOrderByCustomerResult>
{
    public async Task<GetOrderByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        var orders = await context.Orders.Include(x => x.OrderItems)
            .Where(x => x.CustomerId == CustomerId.Of(query.CustomerId))
            .OrderBy(x => x.OrderName.Value)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new(orders.ToOrderDtoList());
    }
}