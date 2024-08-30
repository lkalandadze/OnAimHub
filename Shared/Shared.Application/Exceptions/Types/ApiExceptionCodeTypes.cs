namespace Shared.Application.Exceptions.Types;

public enum ApiExceptionCodeTypes
{
    // 1xxx: Custom General Success
    Unhandled = 1500,
    ResourceCreated = 1501,

    // 2xxx: Custom Client Errors
    InvalidInputData = 2500,
    AuthenticationFailed = 2501,
    PermissionDenied = 2502,
    UserLockedOut = 2503,
    TokenExpired = 2504,

    // 3xxx: Custom Server Errors
    DatabaseError = 3500,
    ServiceUnavailable = 3501,
    DependencyFailure = 3502,
    TimeoutOccurred = 3503,

    // 4xxx: Custom Validation Errors
    ValidationFailed = 4500,
    DuplicateEntry = 4501,

    // 5xxx: Custom Business Logic Errors
    BusinessRuleViolation = 5500,
    OperationNotAllowed = 5501,

    // 6xxx: Custom Integration Errors
    ExternalServiceError = 6500,
    IntegrationTimeout = 6501,

    // 7xxx: Custom Resource Management
    ResourceLimitExceeded = 7500,
    ResourceAllocationFailed = 7501,

    // 8xxx: Custom Infrastructure Errors
    InfrastructureFailure = 8500,
    ConfigurationError = 8501,

    // 9xxx: Custom Security Issues
    SecurityBreachDetected = 9500,
    UnauthorizedAccessAttempt = 9501
}