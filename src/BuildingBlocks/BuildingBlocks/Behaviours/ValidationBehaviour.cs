using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace BuildingBlocks.Behaviours;

public class ValidationBehaviour<TRequest,TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where  TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        var failures =  validationResults.Where(x => x.Errors.Any()).SelectMany(v => v.Errors).ToList();
        
        if(failures.Any()) throw new ValidationException(failures);
        
        return await next();
    }
}