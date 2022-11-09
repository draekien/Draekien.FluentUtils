using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace FluentUtils.MediatR.PipelineBehaviours
{
    /// <summary>
    ///     A MediatR pipeline behaviour that executes any registered fluent validators for a particular
    ///     MediatR Request.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        ///     Creates a new request validation behaviour.
        /// </summary>
        /// <param name="validators">The validators for the current request.</param>
        public RequestValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
            #pragma warning restore CS1591
        {
            _validators = validators;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next();

            ValidationContext<TRequest> context = new(request);
            IEnumerable<Task<ValidationResult>> validateTasks = _validators.Select(validator => validator.ValidateAsync(context, cancellationToken));

            ValidationResult[] validationResults = await Task.WhenAll(validateTasks);

            List<ValidationFailure> validationFailures = validationResults.SelectMany(result => result.Errors)
                                                                          .Where(failure => failure is not null)
                                                                          .ToList();

            if (validationFailures.Any()) throw new ValidationException(validationFailures);

            return await next();
        }
    }
}
