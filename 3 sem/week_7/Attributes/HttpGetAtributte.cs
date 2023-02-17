namespace HTTPServer
{
    public class HttpGetAtributte : Attribute
    {
        public readonly string MethodURI;

        public HttpGetAtributte(string methodURI)
        {
            MethodURI = methodURI;
        }

        public HttpGetAtributte() : this(""){ }
    }
}