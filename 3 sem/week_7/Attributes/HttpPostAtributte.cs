namespace HTTPServer
{
    public  class HttpPostAtributte : Attribute
    {
        public readonly string MethodURI;

        public HttpPostAtributte(string methodURI)
        {
            MethodURI = methodURI;
        }

        public HttpPostAtributte() : this("") { }
    }
}