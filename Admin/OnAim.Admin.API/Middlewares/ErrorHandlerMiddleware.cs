using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Exceptions;
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

                ApplicationResult result;

                switch (error)
                {
                    case FluentValidation.ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = new ApplicationResult
                        {
                            Success = false,
                            Data = null,
                            Errors = e.Errors.Select(x => new Error
                            {
                                Code = (int)HttpStatusCode.BadRequest,
                                Message = x.ErrorMessage
                            })
                        };
                        break;
                    case BadRequestException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = new ApplicationResult
                        {
                            Success = false,
                            Data = null,
                            Errors = new[] { new Error
                            {
                                Code = (int)HttpStatusCode.BadRequest,
                                Message = e.Message
                            }}
                        };
                        break;
                    case ForbiddenException e:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        result = new ApplicationResult
                        {
                            Success = false,
                            Data = null,
                            Errors = new[] { new Error
                            {
                                Code = (int)HttpStatusCode.Forbidden,
                                Message = e.Message
                            }}
                        };
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        result = new ApplicationResult
                        {
                            Success = false,
                            Data = null,
                            Errors = new[] { new Error
                            {
                                Code = (int)HttpStatusCode.InternalServerError,
                                Message = "An unexpected error occurred."
                            }}
                        };
                        break;
                }

                var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                await response.WriteAsync(json);
            }
        }
    }
}
