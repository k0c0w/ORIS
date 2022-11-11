using System.Net;

namespace HTTPServer;

public interface IActionResult
{
    Task ExecuteResultAsync(HttpListenerContext context);
}