using System.Net;
using System.Text.RegularExpressions;
using HTTPServer.Models;

namespace HTTPServer.Services
{
    public static class HttpListenerRequestHelper
    {
        public static Task<string> GetBodyDataAsync(HttpListenerRequest request)
        {
            using (Stream body = request.InputStream)
            {
                using (var reader = new StreamReader(body, request.ContentEncoding))
                {
                    return reader.ReadToEndAsync();
                }
            }
        }
    }
}
