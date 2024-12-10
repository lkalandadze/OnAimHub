using Microsoft.AspNetCore.Http;
using Shared.Application.Exceptions;
using Shared.Lib.Wrappers;
using System.Net;
using System.Text.Json;

namespace Shared.Application.Middlewares;

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

            var responseModel = new Response<int?>() { Data = null, Succeeded = false, Message = error?.Message };

            switch (error)
            {
                case ApiException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case CheckmateException e:
                    // Custom validation error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Message = "Validation failed.";
                    responseModel.ValidationErrors = e.FailedChecks
                        .GroupBy(fc => fc.Path)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(fc => fc.Message).ToArray()
                        );
                    break;

                //For future fluentvalidation

                //case ValidationException e:
                //    // custom application error
                //    response.StatusCode = (int)HttpStatusCode.BadRequest;
                //    break;
                //case ValidationException e:
                //    // custom application error
                //    response.StatusCode = (int)HttpStatusCode.BadRequest;
                //    responseModel.ValidationErrors = e.ValidationErrors;
                //    break;

                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel);

            await response.WriteAsync(result);
        }
        finally
        {
            // Logging here
        }
    }
}