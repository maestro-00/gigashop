namespace Order.Application.Data;

public interface IOrderDbContext
{
    DbSet<Product> Products { get;}
    DbSet<Customer> Customers { get;}
    DbSet<Domain.Models.Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}