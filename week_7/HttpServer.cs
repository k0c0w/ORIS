using HTTPServer.Controllers;
using HTTPServer.Services;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text;

namespace HTTPServer
{
    public class HttpServer
    {
        readonly ILogger logger;
        readonly HttpListener listener = new HttpListener();
        readonly string path;
        readonly int _port;

        public bool IsRunning { get; private set; }

        public HttpServer(ILogger logger, ServerSettings settings)
        {
            this.logger = logger;
            listener.Prefixes.Add($"http://localhost:{settings.Port}/");
            path = settings.Path;
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
                    Stop();
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
            HttpListenerResponse response = null;

            if(request.HttpMethod.ToUpper() == "GET")
                response = await WriteFileIfExists(context);

            if (response == null)
                response = await DelegeteRequestToApiIfExists(context);

            if (response == null)
            {
                logger.Log($"Can not handle request {context.Request.Url}");
                response = context.Response
                                  .Write404PageToBody()
                                  .SetStatusCode((int)HttpStatusCode.NotFound)
                                  .SetContentType(".html");
            }

            logger.Log($"\n{request.ProtocolVersion} {request.HttpMethod} {request.Url} {response.StatusCode}\n");
            response.Close();
        }

        private async Task<HttpListenerResponse?> DelegeteRequestToApiIfExists(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            if (context.Request.Url.Segments.Length < 2) return null;

            string controllerName = context.Request.Url.Segments[1].Replace("/", "");

            string[] strParams = context.Request.Url
                                    .Segments
                                    .Skip(2)
                                    .Select(s => s.Replace("/", ""))
                                    .ToArray();

            var assembly = Assembly.GetExecutingAssembly();

            var controller = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(ApiControllerAttribute))).FirstOrDefault(c => c.Name.ToLower() == controllerName.ToLower());

            if (controller == null) return null;

            var test = typeof(ApiControllerAttribute).Name;
            var method = controller.GetMethods().Where(t => t.GetCustomAttributes(true)
                                                              .Any(attr => attr.GetType().Name == $"Http{context.Request.HttpMethod}"))
                                                 .FirstOrDefault();

            if (method == null) return null;

            object[] queryParams = method.GetParameters()
                                .Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType))
                                .ToArray();

            var ret = await (Task<HttpListenerResponse>)method.Invoke(Activator.CreateInstance(controller), queryParams);

            return ret;
        }

        private async Task<HttpListenerResponse?> WriteFileIfExists(HttpListenerContext context)
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
                    return null;
                }
                await HttpListenerResponseExtenssions.WriteToBodyAsync(response.OutputStream, buffer);
                response.SetContentType(extention)
                        .SetStatusCode(200);
                return response;
            }
            return null;
        }
    }
}
