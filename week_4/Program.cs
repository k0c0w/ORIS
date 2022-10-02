using HTTPServer;

var paths = new Dictionary<string, string>();

Console.WriteLine("Enter path to GoogleMainPage.html");
var input = Console.ReadLine();
paths.Add( "/google", input);

HttpServer server = null;
Thread thread = null;


Console.WriteLine("ServerShell. To start server write: start <on wich port>\n");
while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    var command = Console.ReadLine().Split();
    Console.ForegroundColor= ConsoleColor.White;
    switch (command[0])
    {
        case "start":
            var port = int.Parse(command[1]);
            server = new HttpServer(port, paths, new ConsoleLogger());
            thread = new Thread(server.Start);
            thread.IsBackground = true;
            thread.Start();
            break;
        case "restart":
            if(thread != null)
                AbortThread(thread);
            if (server == null)
            {
                Console.WriteLine("You have not started server!");
                continue;
            }
            thread = new Thread(server.Start);
            thread.IsBackground = true;
            thread.Start();
            break;
        case "stop":
            if(thread != null)
                AbortThread(thread);
            thread = null;
            break;
        
        default:
            Console.WriteLine("Unknown command");
            Console.WriteLine("Commands: start <port>, restart, stop");
            break;
    }
}
 void AbortThread(Thread t)
{
    try
    {
        t.Abort();
    }
    catch
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Server process was interrupted! Server was closed.");
        Console.ForegroundColor = ConsoleColor.White;
        return;
    }
}