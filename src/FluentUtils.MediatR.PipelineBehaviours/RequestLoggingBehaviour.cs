using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FluentUtils.MediatR.PipelineBehaviours
{
    /// <summary>
    ///     Logs request and elapsed time processing request.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class RequestLoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TRequest> _logger;

        /// <summary>
        ///     Creates a new request logging behaviour.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="logger">The logger.</param>
        public RequestLoggingBehaviour(IHttpContextAccessor httpContextAccessor, ILogger<TRequest> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            HttpRequest? httpRequest = httpContext?.Request;
            string? httpRequestMethod = httpRequest?.Method;
            PathString? httpRequestPath = httpRequest?.Path;
            QueryString? httpRequestQuery = httpRequest?.QueryString;

            string origin = $"{httpRequestMethod} {httpRequestPath}{httpRequestQuery}";

            if (string.IsNullOrWhiteSpace(origin))
            {
                origin = "internal";
            }

            Stopwatch sw = new();
            TResponse result;

            using (_logger.BeginScope("Processing Request {@Request} from origin {Origin}", request, origin))
            {
                _logger.LogInformation("Started processing {@Request} from origin {Origin}", request, origin);

                sw.Start();

                result = await next();

                sw.Stop();

                _logger.LogInformation(
                    "Successfully processed {@Request} from origin {Origin} in {ElapsedMilliseconds}ms",
                    request,
                    origin,
                    sw.ElapsedMilliseconds
                );
            }

            return result;
        }
    }
}
