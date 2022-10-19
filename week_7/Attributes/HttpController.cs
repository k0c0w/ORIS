namespace HTTPServer
{
    public class HttpController : Attribute
    {
        public readonly string ControllerName;

        public HttpController(string controllerName)
        {
            ControllerName = controllerName;
        }

        public HttpController() : this("") { }
    }
}