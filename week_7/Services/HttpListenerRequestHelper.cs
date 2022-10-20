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

        public static bool TryParse(string parameters, out SteamAccount? account)
        {
            //TODO: сделать в регулярке только лат символы и цифра
            string pattern = @"\?login=\b(\w+?)\b&password=\b(\w+?)\b";
            Regex rg = new Regex(pattern);
            if (string.IsNullOrEmpty(parameters) || !rg.IsMatch(parameters))
            {
                account = null;
                return false;
            }
            var data = parameters.Split(new[] { "/", "?", "login", "=", "password", "&" }, StringSplitOptions.RemoveEmptyEntries);
            if(data.Length == 2)
            {
                account = new SteamAccount() { Login = data[0], Password = data[1] };
                return true;
            }
            account = null;
            return false;
        }
    }
}
