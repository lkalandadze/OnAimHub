using Microsoft.Extensions.DependencyInjection;
using RazorLight;

namespace OnAim.Admin.Shared.HtmlGenerators
{
    public static class HtmlGeneratorCollectionExtensions
    {
        public static IServiceCollection AddHtmlGenerator(this IServiceCollection services)
        {
            var engine = new RazorLightEngineBuilder()
                   .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates"))
                   .UseMemoryCachingProvider()
                   .Build();

            services.AddSingleton<IRazorLightEngine>(engine);
            services.AddSingleton<IHtmlGenerator, HtmlGenerator>();

            return services;
        }
    }
}
