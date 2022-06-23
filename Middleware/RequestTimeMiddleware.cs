using System;
using System.Diagnostics;

namespace AutoMobileBackend.Middleware;

public class RequestTimeMiddleware:IMiddleware
{
    private readonly Stopwatch _stopwatch;
    private readonly ILogger<RequestTimeMiddleware> _logger;

    public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
    {
        _stopwatch = new Stopwatch();
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _stopwatch.Start();
        await next.Invoke(context);
        _stopwatch.Stop();

        if (_stopwatch.ElapsedMilliseconds / 1000 > 4)
        {
            var message =
                $"Request [{context.Request.Method}] at [{context.Request.Path}] took [{_stopwatch.ElapsedMilliseconds}] ms";
            _logger.LogInformation(message);
        }
    }
}

