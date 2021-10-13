using System;
using System.Net;
using System.Threading.Tasks;
using MetricsApp.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MetricsApp.Middlewares
{
    public class ExceptionHandler : IMiddleware
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                _logger.LogDebug("Log Initilized");

                await next(context);

            }
            catch (DomainNotFoundException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                //To get just message because it is a known domain exception
                _logger.LogError(e.Message);
            }
            catch (Exception e)
            {
                //Since this is an unknown error, we need complete details
                _logger.LogError(e, e.Message);
            }
        }
    }
}
