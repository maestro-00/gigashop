namespace BuildingBlocks.Pagination;

public record PaginationResult<T>(int PageIndex, int PageSize, long TotalCount, IEnumerable<T> Data) where T : class;