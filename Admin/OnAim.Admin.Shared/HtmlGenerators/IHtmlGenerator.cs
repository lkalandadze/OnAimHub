namespace OnAim.Admin.Shared.HtmlGenerators
{
    public interface IHtmlGenerator
    {
        Task<string> GenerateAsync(string template, object model);
    }
}
