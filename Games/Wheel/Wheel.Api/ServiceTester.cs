namespace Wheel.Api;

public static class ServiceTester
{
    internal static async Task<object> ExecuteAsync<T>(this IServiceProvider service, Func<T, Task<object>> action)
    {
        using (var scope = service.CreateScope())
        {
            var i = action.Invoke(scope.ServiceProvider.GetRequiredService<T>());
            return await i;
        }
    }

    internal static async Task ExecuteAsync<T>(this IServiceProvider service, Func<T, Task> action)
    {
        using (var scope = service.CreateScope())
        {
            await action.Invoke(scope.ServiceProvider.GetRequiredService<T>());
        }
    }
}