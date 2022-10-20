namespace HTTPServer
{
    public  class HttpPostAttribute : Attribute
    {
        public readonly string MethodURI;

        public HttpPostAttribute(string methodURI)
        {
            MethodURI = methodURI;
        }

        public HttpPostAttribute() : this("") { }
    }
}