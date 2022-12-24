namespace PaintOnlineServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ServerObject server = new ServerObject();// создаем сервер
            await server.ListenAsync(); // запускаем сервер
        }
    }
}