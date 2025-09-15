using System.Reflection;
using Order.Application.Data;

namespace Order.Infrastructure.Data;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options), IOrderDbContext
{
    public DbSet<Product> Products =>  Set<Product>();
    public DbSet<Customer> Customers =>  Set<Customer>();
    public DbSet<Domain.Models.Order> Orders =>  Set<Domain.Models.Order>();
    public DbSet<OrderItem> OrderItems =>  Set<OrderItem>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}