using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using api.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly RequestDelegate _next;
        public ExceptionMiddleware (ILogger<ExceptionMiddleware> logger, IHostEnvironment env, RequestDelegate next) {
            _next = next;
            _env = env;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context){
            try
            {
                await _next(context);
            }
            catch (Exception xcp)
            {
                _logger.LogError(xcp.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, xcp.Message, xcp.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode);
        
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        
                var json = JsonSerializer.Serialize(response,options);
        
                await context.Response.WriteAsync(json);
            }
        }
    }
}