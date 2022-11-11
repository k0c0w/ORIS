using System.Net;
using System.Text;
using System.Text.Json;

namespace HTTPServer.Services;

public static class ActionResultFactory
{
    public static IActionResult SendHtml(string html) => new HtmlResult(html);
    public static IActionResult SendHtml(byte[] html) => new HtmlResult(html);

    public static IActionResult RedirectTo(string redirectTo) => new Redirect(redirectTo);

    public static IActionResult Json<T>(T model) => new Json<T>(model);

    public static IActionResult NotFound() => new NotFound();
}

public class NotFound : IActionResult
{
    public Task ExecuteResultAsync(HttpListenerContext context)
    {
        context.Response.SetStatusCode((int)HttpStatusCode.NotFound)
            .Write404PageToBody()
            .Close();
        return Task.CompletedTask;
    }
}

//TODO: переделать чтобы не висели массивы и не было утечки памяти
public class HtmlResult : IActionResult
{
    private readonly byte[] _htmlBytes;

    public HtmlResult(string html) : this(Encoding.UTF8.GetBytes(html)) {}
    public HtmlResult(byte[] file) => _htmlBytes = file;
    
    public Task ExecuteResultAsync(HttpListenerContext context)
    {
        return Task.Run(async () =>
        {
            var response = context.Response.SetStatusCode((int)HttpStatusCode.OK)
                .SetContentType(".html");
            await response.WriteToBodyAsync(_htmlBytes);
            response.Close();
        });
    }
}

public class Redirect : IActionResult
{
    private readonly string _route;
    public Redirect(string route) => _route = route;
    
    public Task ExecuteResultAsync(HttpListenerContext context)
    {
        var response = context.Response.SetStatusCode((int)HttpStatusCode.SeeOther);
        response.Headers.Set("Location", _route);
        response.Close(); 
        return Task.CompletedTask;
    }
}

public class Json<T> : IActionResult
{
    private readonly T _model;

    public Json(T model) => _model = model;

    public Task ExecuteResultAsync(HttpListenerContext context)
    {
        return Task.Run(async () =>
            {
                await JsonSerializer.SerializeAsync(context.Response.OutputStream, _model);
                context.Response.SetStatusCode((int)HttpStatusCode.OK)
                    .SetContentType(".json")
                    .Close();
            });
    }
}