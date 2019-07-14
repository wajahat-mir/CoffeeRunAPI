using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeRunAPI.Filters
{
    public class CustomExceptionMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;


        public CustomExceptionMiddleware(ILogger logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception ...", ex);
            }
        }
    }
}
