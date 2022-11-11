using System.Net;
using HTTPServer.Models;
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
            var accountExists = DBProvider.GetSteamAccount(account) != null;
            return ActionResultFactory.SendHtml(accountExists.ToString());
        }
    }
}