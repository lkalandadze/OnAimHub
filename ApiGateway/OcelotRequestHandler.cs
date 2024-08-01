public class OcelotRequestHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Custom logic before sending the request
        // For example, logging the request
        Console.WriteLine($"Request: {request}");

        // Call the base class implementation to send the request
        var response = await base.SendAsync(request, cancellationToken);

        // Custom logic after receiving the response
        // For example, logging the response
        Console.WriteLine($"Response: {response}");

        return response;
    }
}