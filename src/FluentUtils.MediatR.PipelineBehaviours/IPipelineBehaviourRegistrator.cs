using Microsoft.Extensions.DependencyInjection;

namespace FluentUtils.MediatR.PipelineBehaviours
{
    /// <summary>
    ///     Contract for registering pipeline behaviours.
    /// </summary>
    public interface IPipelineBehaviourRegistrator
    {
        /// <summary>
        ///     Tries to register the <see cref="UnhandledExceptionHandlingBehaviour{TRequest,TResponse}"/>
        ///     as a transient dependency.
        /// </summary>
        /// <returns>The current instance of the registrator.</returns>
        IPipelineBehaviourRegistrator WithExceptionHandlingBehaviour();
        /// <summary>
        ///     Tries to register the <see cref="RequestLoggingBehaviour{TRequest,TResponse}"/>
        ///     as a transient dependency.
        /// </summary>
        /// <returns>The current instance of the registrator.</returns>
        IPipelineBehaviourRegistrator WithRequestLoggingBehaviour();
        /// <summary>
        ///     Tries to register the <see cref="RequestValidationBehaviour{TRequest,TResponse}"/>
        ///     as a transient dependency.
        /// </summary>
        /// <returns>The current instance of the registrator.</returns>
        IPipelineBehaviourRegistrator WithRequestValidationBehaviour();
        /// <summary>
        ///     Tries to register <see cref="UnhandledExceptionHandlingBehaviour{TRequest,TResponse}"/>,
        ///     <see cref="RequestLoggingBehaviour{TRequest,TResponse}"/>, and <see cref="RequestValidationBehaviour{TRequest,TResponse}"/>
        ///     as transient dependencies.
        /// </summary>
        /// <returns>The current collection of services.</returns>
        IServiceCollection WithAllBehaviours();
    }
}
