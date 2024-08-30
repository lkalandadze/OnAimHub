﻿using Hub.Domain.Wrappers;
using Shared.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Hub.Api.Middlewares;

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