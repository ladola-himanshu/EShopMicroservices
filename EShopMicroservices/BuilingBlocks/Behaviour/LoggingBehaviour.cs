using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuilingBlocks.Behaviour
{
    public class LoggingBehaviour<TRequest, TResponse>
        (ILogger<LoggingBehaviour<TRequest, TResponse>> logger):
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("START - Handling {RequestType} with content: {@Request} and Response {ResponseType}", typeof(TRequest).Name, request, typeof(TResponse));
            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();
            var timetaken = timer.Elapsed;
            if (timetaken.Seconds > 3)
            {
                logger.LogWarning("Long Running Request: " +
                    "{RequestType} took {ElapsedMilliseconds}ms " +
                    "with content: {@Request} and " +
                    "Response {ResponseType}",
                    typeof(TRequest).Name,
                    timetaken.TotalMilliseconds,
                    request,
                    typeof(TResponse));
            }
            logger.LogInformation("END - Handled {RequestType} in {ElapsedMilliseconds}ms with content: {@Request} and Response {ResponseType}", typeof(TRequest).Name, timetaken.TotalMilliseconds, request, typeof(TResponse));

            return response;
        }
    }
}
