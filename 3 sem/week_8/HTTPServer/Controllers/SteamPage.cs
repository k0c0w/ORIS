using System.Net;
using HTTPServer.Services;

namespace HTTPServer.Controllers
{
    [ApiController("/")]
    public class SteamPageController
    {
        [HttpGet]
        public async Task<HttpListenerResponse> SteamPage(HttpListenerContext context)
        {
            var response = context.Response;
            if (Directory.Exists(HttpServer.Path))
            {
                var bufferExtentionTuple = FileProvider.GetFileAndFileExtension(HttpServer.Path + "steam/index.html");
                var buffer = bufferExtentionTuple.Item1;
                var extention = bufferExtentionTuple.Item2;
                if (buffer != null)
                {
                    await HttpListenerResponseExtenssions.WriteToBodyAsync(response.OutputStream, buffer);
                    response.SetContentType(extention)
                            .SetStatusCode(200);
                    return response;
                }
            }
            return response.Write404PageToBody()
                           .SetContentType(".html")
                           .SetStatusCode((int)HttpStatusCode.NotFound);
        }
    }
}