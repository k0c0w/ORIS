namespace HTTPServer
{
    public class ApiControllerAttribute : Attribute
    {
        public readonly string ControllerName;

        public ApiControllerAttribute(string controllerName)
        {
            ControllerName = controllerName;
        }

        public ApiControllerAttribute() : this("") { }
    }
}