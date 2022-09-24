using System.Net;
using System.Threading;

namespace HTTPServer
{
    internal class HttpServer
    {
        Dictionary<string, string> paths { get; init; }
        HttpListener listener = new HttpListener();
        ILogger logger;
        int _port { get; init; }

        public HttpServer(int port, Dictionary<string,string> paths, ILogger logger)
        {
            listener.Prefixes.Add($"http://*:{port}/");
            this.logger = logger;
            _port = port;
            this.paths = paths;
        }


        public void Start()
        {
            listener.Start();
            logger.Log($"Started server at port {_port}...");

            while (true)
            {
                try
                {
                    HandleRequest();
                }
                catch(FileNotFoundException ex)
                {
                    logger.ReportError(ex.Message);
                    this.Stop();
                    return;
                }
            }
        }

        public void Stop()
        {
            listener.Stop();
            logger.Log($"Closed server ar port {_port}...");
        }

        private void HandleRequest()
        {
            var context = listener.GetContext();
            var request = context.Request;
            var response = context.Response;

            if (request.Url.Host != "localhost")
                response.StatusCode = 404;
            else if (paths.ContainsKey(request.Url.AbsolutePath))
            {
                var buffer = File.ReadAllBytes(paths[request.Url.AbsolutePath]);
                response.ContentLength64 = buffer.Length;
                var output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                response.StatusCode = 200;
            }
            else
                response.StatusCode = 404;


            logger.Log($"\n{request.ProtocolVersion} {request.HttpMethod} {request.Url} {response.StatusCode}\n");
            response.Close();
        }
    }
}
