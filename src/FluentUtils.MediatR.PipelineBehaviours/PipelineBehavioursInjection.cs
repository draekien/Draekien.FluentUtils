using Microsoft.Extensions.DependencyInjection;

namespace FluentUtils.MediatR.PipelineBehaviours
{
    /// <summary>
    ///     Microsoft DI helper
    /// </summary>
    public static class PipelineBehavioursInjection
    {
        /// <summary>
        ///     Creates a new instance of the pipeline behaviour registrator.
        /// </summary>
        /// <param name="services">The current service collection.</param>
        /// <returns>The current instance of the registrator.</returns>
        public static IPipelineBehaviourRegistrator AddPipelineBehaviourBuilder(this IServiceCollection services)
        {
            return FluentPipelinePipelineBehaviourBuilder.CreateBuilder(services);
        }

        /// <summary>
        ///     Registers <see cref="UnhandledExceptionHandlingBehaviour{TRequest,TResponse}"/>,
        ///     <see cref="RequestLoggingBehaviour{TRequest,TResponse}"/>, and <see cref="RequestValidationBehaviour{TRequest,TResponse}"/>
        ///     as transient dependencies.
        /// </summary>
        /// <param name="services">The current service collection.</param>
        /// <returns>The current service collection.</returns>
        public static IServiceCollection AddFluentUtilsPipelineBehaviours(this IServiceCollection services)
        {
            FluentPipelinePipelineBehaviourBuilder.CreateBuilder(services)
                                                  .WithAllBehaviours();

            return services;
        }
    }
}
