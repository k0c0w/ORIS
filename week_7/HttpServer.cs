using HTTPServer.Controllers;
using HTTPServer.Services;
using System.Net;
using System.Net.Mime;
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
            var response = context.Response;
            byte[] buffer;
            if(!Directory.Exists(path))
            {
                var rawUrl = context.Request.RawUrl.Replace("%20", " ");
                var bufferExtentionTuple = FileProvider.GetFileAndFileExtension(path + rawUrl);
                buffer = bufferExtentionTuple.Item1;
                var extention = bufferExtentionTuple.Item2;
                if(buffer == null)
                {
                    response.SetStatusCode((int)HttpStatusCode.NotFound)
                            .SetContentType(".txt")
                            .Write404PageToBody();
                    logger.Log($"\n{request.ProtocolVersion} {request.HttpMethod} {request.Url} {response.StatusCode}\n");
                    response.Close();
                    return;
                }
                else
                response.SetContentType(extention);
            }
            else if (true)
            {
                new ApiController().SaveAccount(context);
                return;
            }
            else
            {
                logger.ReportError($"Directory '{path}' not found");
                var err = "500 - server error.";
                buffer = Encoding.UTF8.GetBytes(err);
            }
            using(Stream output = response.OutputStream)
            {
                await output.WriteAsync(buffer, 0, buffer.Length);
                output.Close();
            }

            logger.Log($"\n{request.ProtocolVersion} {request.HttpMethod} {request.Url} {response.StatusCode}\n");
            response.Close();
        }
    }
}
