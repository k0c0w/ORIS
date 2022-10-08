using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading;

namespace HTTPServer
{
    internal class HttpServer
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

        public void Start()
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
                    HandleRequest();
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

        private void HandleRequest()
        {
            var context = listener.GetContext();

            var request = context.Request;
            var response = context.Response;
            byte[] buffer;
            if (Directory.Exists(path))
            {
                var rawUrl = context.Request.RawUrl.Replace("%20", " ");
                var bufferExtentionTuple = FileProvider.GetFileAndFileExtension(path + rawUrl);
                buffer = bufferExtentionTuple.Item1;
                var extention = bufferExtentionTuple.Item2;
                if(buffer == null)
                {
                    response.Headers.Set("Content-Type", "text/plain");
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    var err = "404 - not found";
                    buffer = Encoding.UTF8.GetBytes(err);
                    return;
                }
                response.Headers.Set("Content-Type", GetContentType(extention));
            }
            else
            {
                logger.ReportError($"Directory '{path}' not found");
                var err = "500 - server error.";
                buffer = Encoding.UTF8.GetBytes(err);
            }

            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            logger.Log($"\n{request.ProtocolVersion} {request.HttpMethod} {request.Url} {response.StatusCode}\n");
            response.Close();
        }

        private string GetContentType(string extension)
        {
            switch (extension)
            {
                case ".htm":
                case ".html":
                    return  "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return  "text/javascript";
                case ".jpg":
                    return "image/jpeg";
                case ".jpeg":
                case ".png":
                case ".gif":
                    return "image/" + extension.Substring(1);
                case ".ico":
                    return "image/x-icon";
                case ".svg":
                    return "image/svg+xml";

                default:
                    if (extension?.Length > 1)
                        return "application/" + extension.Substring(1);
                    else
                        return  "application/unknown";
            }
        }
    }
}
