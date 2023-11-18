using Newtonsoft.Json;
using System.Net;

namespace TalkHubAPI.Middlewarres
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _Next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new { error = "An unexpected error occurred." };
            var jsonResponse = JsonConvert.SerializeObject(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
