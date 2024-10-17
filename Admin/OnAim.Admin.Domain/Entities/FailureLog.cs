using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Domain.Entities;

public abstract class FailureLog
{
    protected FailureLog()
    {
        
    }
    protected FailureLog(string action, int userId, int? objectId, string? objectt, string log, string errorMessage, int retryCount)
    {
        Action = action;
        UserId = userId;
        ObjectId = objectId;
        Object = objectt;
        Log = log;
        ErrorMessage = errorMessage;
        RetryCount = retryCount;
        Timestamp = SystemDate.Now;
    }

    [BsonId]
    public ObjectId Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Action { get; set; }
    public int UserId { get; set; }
    public int? ObjectId { get; set; }
    public string Object { get; set; }
    public string Log { get; set; }
    public string ErrorMessage { get; set; }
    public int RetryCount { get; set; }
}

public class AccessDeniedLog : FailureLog
{
    public AccessDeniedLog(string action, int userId, int? objectId, string? objectt, string log, string errorMessage, int retryCount)
        : base(action, userId, objectId, objectt, log, errorMessage, retryCount)
    {
    }
}

public class OperationFailedLog : FailureLog
{
    public OperationFailedLog(string action, int userId, int? objectId, string? objectt, string log, string errorMessage, int retryCount)
        : base(action, userId, objectId, objectt, log, errorMessage, retryCount)
    {
    }
}

