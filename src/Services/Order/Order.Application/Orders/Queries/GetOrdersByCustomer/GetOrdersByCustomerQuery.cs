namespace Order.Application.Orders.Queries.GetOrdersByCustomer;

public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrderByCustomerResult>;

public record GetOrderByCustomerResult(IEnumerable<OrderDto> Orders);
