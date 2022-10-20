namespace HTTPServer
{
    public class HttpGetAttribute : Attribute
    {
        public readonly string MethodURI;

        public HttpGetAttribute(string methodURI)
        {
            MethodURI = methodURI;
        }

        public HttpGetAttribute() : this(""){ }
    }
}