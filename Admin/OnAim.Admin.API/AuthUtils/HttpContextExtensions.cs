using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OnAim.Admin.API.AuthUtils
{
    public static class HttpContextExtensions
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true
        };

        public static async ValueTask WriteAccessDeniedResponse(
            this HttpContext context,
            string? title = null,
            int? statusCode = null,
            CancellationToken cancellationToken = default)
        {
            var problem = new ProblemDetails
            {
                Instance = context.Request.Path,
                Title = title ?? "Access denied",
                Status = statusCode ?? Status403Forbidden
            };
            context.Response.StatusCode = problem.Status.Value;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonSerializerOptions),
                cancellationToken);
        }
    }
}
