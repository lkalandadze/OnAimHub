namespace OnAim.Admin.Contracts.Helpers.HtmlGenerator;

public interface IHtmlGenerator
{
    Task<string> GenerateAsync(string template, object model);
}
