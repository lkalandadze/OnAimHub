using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using SendGrid.Helpers.Errors.Model;
using System.Net;
using System.Text.Json;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.Infrasturcture.Interfaces;

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
        var entityName = path.Value?.Split('/').ElementAtOrDefault(2) ?? "Unknown";
        Exception? caughtError = null;

        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            caughtError = error;
            await HandleExceptionAsync(context, error);
        }
        finally
        {
            _ = caughtError != null
                ? auditLogService.AddOperationFailedLogAsync(new OperationFailedLog(
                    method,
                    securityContextAccessor.UserId,
                    null,
                    string.Empty,
                    $"{method} {path} failed",
                    caughtError.Message,
                    0))
                : auditLogService.AddAuditLogAsync(new AuditLog(
                    method,
                    null,
                    string.Empty,
                    securityContextAccessor.UserId,
                    $"{method} {path} succeeded",
                    entityName));
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        context.Response.ContentType = "application/json";
        var errorsList = new List<OnAim.Admin.Contracts.ApplicationInfrastructure.Error>();
        context.Response.StatusCode = error switch
        {
            OnAim.Admin.CrossCuttingConcerns.Exceptions.BadRequestException => (int)HttpStatusCode.BadRequest,
            OnAim.Admin.CrossCuttingConcerns.Exceptions.ForbiddenException => (int)HttpStatusCode.Forbidden,
            UnauthorizedException => (int)HttpStatusCode.Unauthorized,
            OnAim.Admin.CrossCuttingConcerns.Exceptions.NotFoundException => (int)HttpStatusCode.NotFound,
            FluentValidation.ValidationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        errorsList.Add(new OnAim.Admin.Contracts.ApplicationInfrastructure.Error
        {
            Code = context.Response.StatusCode,
            Message = error switch
            {
                FluentValidation.ValidationException e => string.Join(", ", e.Errors.Select(x => x.ErrorMessage)),
                _ => error.Message
            }
        });

        var result = new ApplicationResult
        {
            Success = false,
            Data = null,
            Errors = errorsList
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        }));
    }
}
