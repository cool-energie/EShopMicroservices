using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Start] Handling {RequestName} with content: {@Request} -- Response: {ResponseName}", typeof(TRequest).Name, request, typeof(TResponse).Name);
            var timer = new Stopwatch();
            timer.Start();
            var response = await next();
            timer.Stop();
            var timeTaken = timer.Elapsed.TotalSeconds;
            if (timeTaken > 3) { 
                logger.LogWarning("[Warning] Handling {RequestName} took longer than expected: {ElapsedSeconds} s", typeof(TRequest).Name, timeTaken);
            }
            logger.LogInformation("[End] Handled {RequestName} and returned response: {ResponseName} in {ElapsedSeconds} s", typeof(TRequest).Name, typeof(TResponse).Name, timeTaken);
            return response;
        }
    }
}
