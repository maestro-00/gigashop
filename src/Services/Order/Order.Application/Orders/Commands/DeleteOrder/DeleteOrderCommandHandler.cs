namespace Order.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IOrderDbContext context) :  ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Id);
        var order = await context.Orders.FindAsync([orderId], cancellationToken);
        if (order == null) throw new OrderNotFoundException(command.Id);
        
        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);
        
        return new (true);
    }
}