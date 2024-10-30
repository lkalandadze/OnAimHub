namespace OnAim.Admin.Contracts.Helpers.HtmlGenerator;

public class HtmlGenerator : IHtmlGenerator
{
    //private readonly IRazorLightEngine _razorLightEngine;

    //public HtmlGenerator(IRazorLightEngine razorLightEngine)
    //{
    //    _razorLightEngine = razorLightEngine;
    //}

    //public Task<string> GenerateAsync(string template, object model)
    //{
    //    return _razorLightEngine.CompileRenderAsync(template, model);
    //}
    public Task<string> GenerateAsync(string template, object model)
    {
        throw new NotImplementedException();
    }
}
