using FluentValidation;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Net;
using System.Text.Json;

namespace OnAim.Admin.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.OK;
                        var result = JsonSerializer.Serialize(new ApplicationResult
                        {
                            Success = false,
                            Data = null,
                            Errors = e.Errors.Select(x => new Error
                            {
                                Code = (int)HttpStatusCode.BadRequest,
                                Message = x.ErrorMessage
                            })
                        }, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        });
                        await response.WriteAsync(result);
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
        }
    }
}
