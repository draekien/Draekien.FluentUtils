using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FluentUtils.MediatR.PipelineBehaviours
{
    /// <summary>
    ///     The fluent api for registering pipeline behaviours.
    /// </summary>
    public class FluentPipelinePipelineBehaviourBuilder : IPipelineBehaviourRegistrator
    {
        private readonly IServiceCollection _services;

        private FluentPipelinePipelineBehaviourBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        ///     Creates a new instance of this fluent api.
        /// </summary>
        /// <param name="services">The current service collection.</param>
        /// <returns>The created registrator.</returns>
        internal static IPipelineBehaviourRegistrator CreateBuilder(IServiceCollection services)
        {
            return new FluentPipelinePipelineBehaviourBuilder(services);
        }

        /// <inheritdoc />
        public IPipelineBehaviourRegistrator WithExceptionHandlingBehaviour()
        {
            _services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionHandlingBehaviour<,>));

            return this;
        }

        /// <inheritdoc />
        public IPipelineBehaviourRegistrator WithRequestLoggingBehaviour()
        {
            _services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingBehaviour<,>));

            return this;
        }

        /// <inheritdoc />
        public IPipelineBehaviourRegistrator WithRequestValidationBehaviour()
        {
            _services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

            return this;
        }

        /// <inheritdoc />
        public IServiceCollection WithAllBehaviours()
        {
            WithExceptionHandlingBehaviour();
            WithRequestLoggingBehaviour();
            WithRequestValidationBehaviour();

            return _services;
        }
    }
}
