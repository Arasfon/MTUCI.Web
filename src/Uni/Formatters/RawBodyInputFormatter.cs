using Microsoft.AspNetCore.Mvc.Formatters;

namespace Uni.Formatters;

public class RawBodyInputFormatter : InputFormatter
{
    public RawBodyInputFormatter() =>
        SupportedMediaTypes.Add("text/plain");

    public override bool CanRead(InputFormatterContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        string? contentType = context.HttpContext.Request.ContentType;

        return String.IsNullOrEmpty(contentType) || contentType == "text/plain";
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        context.HttpContext.Request.EnableBuffering();

        using StreamReader sr = new(context.HttpContext.Request.Body);

        return await InputFormatterResult.SuccessAsync(await sr.ReadToEndAsync());
    }
}
