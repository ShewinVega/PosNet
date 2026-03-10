
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace PosNet.Infrastructure.Middlewares
{
    public class GlobalExceptionMiddleware(
        ILogger<GlobalExceptionMiddleware> logger,
        RequestDelegate next
     )
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next( context );
            } catch ( Exception ex )
            {
                _logger.LogError(ex, "There was an unexpected error: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }


        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string messageDetail = "Something got wrong. We are working on it!!!";
            Type? exceptionType = exception.GetType();

            if (exceptionType == typeof(DirectoryNotFoundException) ||
            exceptionType == typeof(DllNotFoundException) ||
            exceptionType == typeof(EntryPointNotFoundException) ||
            exceptionType == typeof(FileNotFoundException) ||
            exceptionType == typeof(KeyNotFoundException)) 
            {
                messageDetail = exception.Message;
                statusCode = HttpStatusCode.NotFound;
            }

            if(exceptionType == typeof(NotImplementedException))
            {
                messageDetail = exception.Message;
                statusCode = HttpStatusCode.NotImplemented;
            }

            if(exceptionType == typeof(UnauthorizedAccessException) ||
                exceptionType == typeof(AuthenticationException))
            {
                messageDetail = exception.Message;
                statusCode= HttpStatusCode.Unauthorized;
            }

            var problemDetails = new ProblemDetails
            {
                Title = "Internal Server error",
                Status = (int)statusCode,
                Detail = messageDetail,
                Instance = context.Request.Path,
            };

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            
        }
    }
}
