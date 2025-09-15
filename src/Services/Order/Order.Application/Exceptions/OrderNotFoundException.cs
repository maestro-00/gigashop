using BuildingBlocks.Exceptions;

namespace Order.Application.Exceptions;

public class OrderNotFoundException(Guid key) : NotFoundException("Domain", key)
{
    
}