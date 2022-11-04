namespace HTTPServer
{
    public class HttpGetAttribute : ApiControllerMethodAttribute
    {
        public HttpGetAttribute(string methodURI) : base(methodURI) { }

        public HttpGetAttribute() : base(){ }
    }
}