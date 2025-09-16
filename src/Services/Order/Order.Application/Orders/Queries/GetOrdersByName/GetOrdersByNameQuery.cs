namespace Order.Application.Orders.Queries.GetOrdersByName;

public record GetOrdersByNameQuery(string OrderName) : IQuery<GetOrderByNameResult>;

public record GetOrderByNameResult(IEnumerable<OrderDto> Orders);