
using FluentValidation;
using FluentValidation.Results;
using Kommand;
using Kommand.Abstractions;


namespace ZenBlog.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IInterceptor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<FluentValidation.IValidator<TRequest>> _validators;
 
    public ValidationBehavior(IEnumerable<FluentValidation.IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
 
       public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
         if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var errorDictionary = _validators
            .Select(s => s.Validate(context))
            .SelectMany(s => s.Errors)
            .Where(s => s != null)
            .GroupBy(
            s => s.PropertyName,
            s => s.ErrorMessage, (propertyName, errorMessage) => new
            {
                Key = propertyName,
                Values = errorMessage.Distinct().ToArray()
            })
            .ToDictionary(s => s.Key, s => s.Values[0]);

        if (errorDictionary.Any())
        {
            var errors = errorDictionary.Select(s => new ValidationFailure
            {
                PropertyName = s.Key,
                ErrorMessage = s.Value
            });
            throw new FluentValidation.ValidationException(errors);
        }

        return await next();
    }
}