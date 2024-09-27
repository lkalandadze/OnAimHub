namespace OnAim.Admin.Shared.Helpers.HtmlGenerators;

public interface IHtmlGenerator
{
    Task<string> GenerateAsync(string template, object model);
}
