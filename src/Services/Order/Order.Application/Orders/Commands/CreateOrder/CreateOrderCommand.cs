namespace Order.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Dto) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.Dto.OrderName).NotEmpty().WithMessage("Order name is required.");
        RuleFor(x => x.Dto.OrderItems).NotEmpty().WithMessage("Order Items must not be empty.");
        RuleFor(x => x.Dto.CustomerId).NotEmpty().WithMessage("Customer Id is required.");
    }
}