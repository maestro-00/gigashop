namespace Order.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(PaginationRequest Pagination) : IQuery<GetOrdersQueryResult>;

public record GetOrdersQueryResult(PaginationResult<OrderDto> Orders);