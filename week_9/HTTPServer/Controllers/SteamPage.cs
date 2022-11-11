using HTTPServer.Models;
using HTTPServer.Services.ServerServices;
using HTTPServer.Services;

namespace HTTPServer.Controllers
{
    [ApiController("/")]
    public class SteamPageController
    {
        [HttpGet]
        public async Task<IActionResult> SteamPage()
        {
            if (Directory.Exists(HttpServer.Path))
            {
                var buffer = await FileProvider.ReadFileAsync(HttpServer.Path + "steam/index.html");
                if (buffer != null)
                    return ActionResultFactory.SendHtml(buffer);
            }

            return ActionResultFactory.NotFound();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var account = new SteamAccount() { Login = email, Password = password };
            var accountExists = DBProvider.GetSteamAccount(account);
            if(accountExists != null)
                return ActionResultFactory.SendHtml("true", 
                    new SessionInfo() {IsAuthorized = true, AccountId = (int)accountExists.Id});
            return ActionResultFactory.SendHtml(accountExists.ToString());
        }
    }
}