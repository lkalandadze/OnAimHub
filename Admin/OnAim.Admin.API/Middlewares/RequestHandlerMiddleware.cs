using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.ApplicationInfrastructure.Validation;
using OnAim.Admin.Shared.Models;
using SendGrid.Helpers.Errors.Model;
using System.Net;
using System.Text;
using System.Text.Json;

namespace OnAim.Admin.API.Middleware;

public class RequestHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public RequestHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogRepository auditLogService, ISecurityContextAccessor securityContextAccessor)
    {
        var method = context.Request.Method;
        var path = context.Request.Path;
        var segments = path.Value.Split('/');
        var entityName = segments.Length > 2 ? segments[2] : "Unknown";


        string requestBody;
        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        Exception caughtError = null;

        try
        {
            await _next(context);
        }

        catch (Exception error)
        {
            caughtError = error;
            var response = context.Response;
            response.ContentType = "application/json";

            var errorsList = new List<Error>();

            switch (error)
            {
                case Domain.Exceptions.BadRequestException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorsList.Add(new Error { Code = (int)HttpStatusCode.BadRequest, Message = e.Message });
                    break;

                case Domain.Exceptions.ForbiddenException e:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorsList.Add(new Error { Code = (int)HttpStatusCode.Forbidden, Message = e.Message });
                    break;

                case UnauthorizedException e:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorsList.Add(new Error { Code = (int)HttpStatusCode.Unauthorized, Message = e.Message });
                    break;

                case Domain.Exceptions.NotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorsList.Add(new Error { Code = (int)HttpStatusCode.NotFound, Message = e.Message });
                    break;

                case FluentValidation.ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorsList.AddRange(e.Errors.Select(x => new Error
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = x.ErrorMessage
                    }));
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorsList.Add(new Error { Code = (int)HttpStatusCode.InternalServerError, Message = "An unexpected error occurred." });
                    break;
            }

            var result = new ApplicationResult
            {
                Success = false,
                Data = null,
                Errors = errorsList
            };

            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await response.WriteAsync(json);
        }
        finally
        {
            var rejectedLog = new RejectedLog(method, securityContextAccessor.UserId, null, requestBody, $"{method} {path} failed", caughtError?.Message, 0);

            if (caughtError != null)
            {
                await auditLogService.AddRejectedLogAsync(rejectedLog);
            }
            else
            {
                var successfulLog = new AuditLog(method, null, requestBody, securityContextAccessor.UserId, $"{method} {path} succeeded", entityName);

                await auditLogService.AddAuditLogAsync(successfulLog);
            }
        }
    }
}
