using System.Net;
using HTTPServer.Models;
using HTTPServer.Services.ServerServices;
using HTTPServer.Services;

namespace HTTPServer.Controllers
{
    [ApiController("/")]
    public class SteamPageController
    {
        private static SessionManager _sessionManager = SessionManager.Instance;
        
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
        [CookieRequired]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password, CookieCollection cookies)
        {
            var sessionCookie = cookies["SessionId"]?.Value;
            var account = new SteamAccount() { Login = email, Password = password };
            var accountExists = DBProvider.GetSteamAccount(account);
            if (accountExists != null)
            {
                var guid = Guid.Empty;
                var correctCookie = string.IsNullOrEmpty(sessionCookie) || Guid.TryParse(sessionCookie, out guid);
                //todo: добавить проверку email и password из session с теми что пришли
                if (correctCookie && !_sessionManager.TryGetSession(guid, out var session))
                {
                    guid = Guid.NewGuid();
                    _sessionManager.CreateSession(guid, 
                        () => new Session() { Id = guid, AccountId = (int)accountExists.Id, 
                            Email = accountExists.Login, CreateDateTime = DateTime.Now});
                }
                
                return ActionResultFactory.SendHtml("true", 
                    new SessionInfo() { Guid = guid });
            }
            
            return ActionResultFactory.NotFound();
        }
    }
}