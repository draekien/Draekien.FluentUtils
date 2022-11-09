using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FluentUtils.MediatR.PipelineBehaviours
{
    /// <summary>
    ///     Catches unhandled exceptions so they can be logged.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class UnhandledExceptionHandlingBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<UnhandledExceptionHandlingBehaviour<TRequest, TResponse>> _logger;

        /// <summary>
        ///     Creates a new unhandled exception handling behaviour.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UnhandledExceptionHandlingBehaviour(ILogger<UnhandledExceptionHandlingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex) when (ex is ValidationException)
            {
                _logger.LogWarning(
                    ex,
                    "Unhandled exception: {ExceptionType} for request {@Request} with message {Message}",
                    ex.GetType().Name,
                    request,
                    ex.Message
                );

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception: {ExceptionType} for request {@Request} with message {Message}",
                    ex.GetType().Name,
                    request,
                    ex.Message
                );

                throw;
            }
        }
    }
}
