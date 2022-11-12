﻿using System.Collections.Specialized;
using HTTPServer.Services;
using HTTPServer.Services.ServerServices;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace HTTPServer
{
    public class HttpServer
    {
        public readonly string path;
        public readonly int _port;
        readonly ILogger logger;
        readonly HttpListener listener = new HttpListener();

        public static string Path { get; private set; }
        public bool IsRunning { get; private set; }

        public HttpServer(ILogger logger, ServerSettings settings)
        {
            this.logger = logger;
            listener.Prefixes.Add($"http://localhost:{settings.Port}/");
            path = settings.Path;
            Path = path;
            _port = settings.Port;
        }

        public async void Start()
        {
            try
            {
                if (IsRunning)
                {
                    logger.Log("Server is already  running!");
                    return;
                }

                logger.Log("Starting server...");
                listener.Start();
                IsRunning = true;
                logger.Log($"Server has started at port {_port}");

                while (true)
                    await HandleRequestAsync();
            }
            catch(Exception ex)
            {
                if (IsRunning)
                {
                    logger.ReportError($"Closing server since exception: {ex.Message}");
                }
            }
        }

        public void Stop()
        {
            listener.Stop();
            IsRunning = false;
            logger.Log($"Closed server at port {_port}...");
        }

        private async Task HandleRequestAsync()
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;
            var fileSent = false;
            
            if(request.HttpMethod == "GET")
                fileSent = await SendFileIfExistsAsync(context);
            
            if(fileSent)  return;

            try
            {
                //todo: проверять требуется ли авторизация и затем если нужна чекать наличичие сессиии если сессии нет, то кидать 401
                await GiveContextToControllerByRouteIfExists(context);
            }
            catch
            {
                logger.Log($"Can not handle request {context.Request.Url}");
                context.Response
                    .Write404PageToBody()
                    .SetStatusCode((int)HttpStatusCode.NotFound)
                    .SetContentType(".html")
                    .Close();
            }

            logger.Log($"\n{request.ProtocolVersion} {request.HttpMethod} {request.Url} {context.Response.StatusCode}\n");
        }

        private async Task GiveContextToControllerByRouteIfExists(HttpListenerContext context)
        {
            var request = context.Request;
            var httpMethod = request.HttpMethod.ToUpper();
            var segments = context!.Request!.Url!.Segments;

            if (segments.Length < 1) return;
            segments[segments.Length - 1] = segments[segments.Length - 1].Replace("/","");

            var tuple = GetControllerAndMethodRoute(segments);
            var controllerRoute = tuple.Item1;
            var methodRoute = tuple.Item2;

            var controllerType = typeof(ApiControllerAttribute);
            var controller = GetRequiredController(controllerType, controllerRoute, Assembly.GetExecutingAssembly());

            if (controller == null)
                return;
            if (string.IsNullOrEmpty(methodRoute))
                methodRoute = controller.Name.Replace("Controller", "");

            var markedMethods = controller.GetMethods().Where(x => x.GetCustomAttribute(typeof(ApiControllerMethodAttribute)) != null);
            var parameters = GetParametersFromQuery(context);
            var method = GetRequiredMethod(httpMethod, markedMethods, methodRoute, parameters);

            if (method == null)
                return;

            if (method.GetCustomAttribute<AuthorizeAttribute>() == null || CheckSessionCookie(context.Request.Cookies))
            {
                if(method.GetCustomAttribute<SessionCookieRequiredAttribute>() != null)
                    parameters.Add("sessionCookie", context.Request.Cookies["SessionId"].Value);
                var actionResult = await GetActionResultTaskFromMethod(
                    controller.GetConstructor(new Type[0]).Invoke(new object[0]), method, parameters);
                await actionResult.ExecuteResultAsync(context);
            }
            else
                context.Response
                    .SetStatusCode((int)HttpStatusCode.Unauthorized)
                    .Close();
        }

        private bool CheckSessionCookie(CookieCollection cookies)
        {
            if (cookies["SessionId"] != null)
            {
                var values = cookies["SessionId"].Value.Split();
                if (values.Length != 2)
                    return false;
                return  values[0] == "IsAuthorized=True";
            }

            return false;
        }


        private Task<IActionResult> GetActionResultTaskFromMethod(object controller, MethodInfo method, Dictionary<string, string> parameters)
        {

            var paramsIn = method.GetParameters()
                .Select(p => Convert.ChangeType(parameters[p.Name], p.ParameterType))
                .ToArray();

            return (Task<IActionResult>)method.Invoke(controller, paramsIn);
        }
        
        
        private Dictionary<string, string> GetParametersFromQuery(HttpListenerContext context)
        {
            var dict = new Dictionary<string, string>();
            var method = context.Request.HttpMethod;
            if(method=="GET") 
                foreach (var key in context.Request.QueryString.AllKeys)
                    dict.Add(key, context.Request.QueryString[key]);
            if (method == "POST")
            {
                IEnumerable<(string, string)> keyValues;
                using (Stream body = context.Request.InputStream)
                {
                    using (var reader = new StreamReader(body, context.Request.ContentEncoding))
                    {
                        var parameters = reader.ReadToEnd();
                        keyValues = parameters
                            .Split("&", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x =>
                            {
                                var keyAndValue = x.Split('=', StringSplitOptions.RemoveEmptyEntries);
                                return (keyAndValue.FirstOrDefault(), keyAndValue.LastOrDefault());
                            })
                            .Where(x => !string.IsNullOrEmpty(x.Item1) && !string.IsNullOrEmpty(x.Item2));
                    }
                }
                foreach (var pair in keyValues)
                    dict.Add(pair.Item1, pair.Item2);
            }
            //todo: кидать unsupporteMethodExcpetion 

            return dict;
        }


        private Type? GetRequiredController(Type controllerType, string controllerRoute, Assembly assembly)
        {
            return assembly.GetTypes()
                                     .Where(type => Attribute.IsDefined(type, controllerType))
                                     .Select(type => (type, type.GetCustomAttribute(controllerType) as ApiControllerAttribute))
                                     .Where(tuple =>
                                            tuple.Item2 != null
                                            && (string.IsNullOrEmpty(tuple.Item2.ControllerName)
                                            ? tuple.Item1.Name.Replace("Controller", "") == controllerRoute
                                            : tuple.Item2.ControllerName == controllerRoute))
                                     .Select(tuple => tuple.Item1)
                                     .FirstOrDefault();
        }

        private MethodInfo? GetRequiredMethod
            (string httpMethod, IEnumerable<MethodInfo?> markedMethods, string route, Dictionary<string, string> parameters)
        {
            //todo: убрать проверку fromquery
            Func<Type, MethodInfo?> selector = 
                (type) => markedMethods.Select(x => (x, x.GetCustomAttribute(type) as ApiControllerMethodAttribute))
                                       .Where(
                                            x => x.Item2 != null 
                                            && (string.IsNullOrEmpty(x.Item2.MethodURI) 
                                            ? x.Item1!.Name == route : x.Item2.MethodURI == route))
                                            .Select(x => x.Item1)
                                            .Where(x => x.GetParameters()
                                                        .Where(x => x.GetCustomAttribute<FromQueryAttribute>() != null)
                                                        .All(param => parameters.ContainsKey(param.Name)))
                                       .FirstOrDefault();
            switch (httpMethod)
            {
                case "GET":
                    return selector(typeof(HttpGetAttribute));
                case "POST":
                    return selector(typeof(HttpPostAttribute));
                default:
                    return null;
            }
        }

        private (string, string?) GetControllerAndMethodRoute(string[] segments)
        {
            Func<string, string> removeSlash = (word) => word.Replace("/", string.Empty); 
            string? method = null;
            string? controller = null;

            //TODO: пересмотреть еще раз
            if (segments.Length == 1 && segments[0] == String.Empty)
                controller = "/";
            else if (segments.Length == 3)
            {
                controller = removeSlash(segments[1]);
                method = segments[2];
            }
            else if(segments.Length == 2 && segments[0] == "/" && !string.IsNullOrEmpty(segments[1]))
            {
                controller = "/";
                method = string.Concat(segments[1].TakeWhile(x => x != '?'));
            }
            else
            {
                controller = removeSlash(segments[1]);
                method = string.Concat(segments.Skip(2).TakeWhile(x => x != "?"));
            }

            return (controller, method);
        }

        private async Task<bool> SendFileIfExistsAsync(HttpListenerContext context)
        {
            var response = context.Response;
            if (Directory.Exists(path))
            {
                var rawUrl = context.Request.RawUrl.Replace("%20", " ");
                var bufferExtentionTuple = FileProvider.GetFileAndFileExtension(path + rawUrl);
                var buffer = bufferExtentionTuple.Item1;
                var extention = bufferExtentionTuple.Item2;
                if (buffer == null)
                {
                    return false;
                }
                await HttpListenerResponseExtenssions.WriteToBodyAsync(response.OutputStream, buffer);
                response.SetContentType(extention)
                        .SetStatusCode(200)
                        .Close();
                return true;
            }
            return false;
        }
    }
}