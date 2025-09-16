namespace Order.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Order name is required.");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("Order Items must not be empty.");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("Customer Id is required.");
    }
}