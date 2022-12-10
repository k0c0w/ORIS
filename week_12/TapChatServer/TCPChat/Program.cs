using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using TCPChat;


ServerObject server = new ServerObject();// создаем сервер
await server.ListenAsync(); // запускаем сервер



class ServerObject
{
    TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888); // сервер для прослушивания
    Dictionary<Guid, ClientObject> clients = new Dictionary<Guid, ClientObject>(); // все подключения
    protected internal void RemoveConnection(Guid id)
    {
        if (clients.Remove(id, out var client))
        {
           client.Close();
        }
    }
    // прослушивание входящих подключений
    protected internal async Task ListenAsync()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");
 
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
 
                ClientObject clientObject = new ClientObject(tcpClient, this);
                clients.Add(clientObject.Id, clientObject);
                Task.Run(clientObject.ProcessAsync);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }
 
    // трансляция сообщения подключенным клиентам
    protected internal async Task BroadcastMessageAsync(string message, Guid id)
    {
        foreach (var (guid,client) in  clients)
        {
            if (guid != id) // если id клиента не равно id отправителя
            {
                await client.Writer.WriteLineAsync(message); //передача данных
                await client.Writer.FlushAsync();
            }
        }
    }
    
    protected internal async Task BroadcastConnectedClientsToAsync(ClientObject client)
    {
        var id = client.Id;
        var clients = this.clients.Where(x => x.Key != id)
            .Select(x => x.Value.User);
        await client.Writer.WriteLineAsync($"INFO:{JsonSerializer.Serialize(clients)}");
        await client.Writer.FlushAsync();
    }
    // отключение всех клиентов
    protected internal void Disconnect()
    {
        foreach (var (id,client) in clients)
        {
            client.Close(); //отключение клиента
        }
        tcpListener.Stop(); //остановка сервера
    }
}
class ClientObject
{
    Random random = new Random();

    protected internal User User { get; private set; }
    protected internal Guid Id { get;} = Guid.NewGuid();
    protected internal StreamWriter Writer { get;}
    protected internal StreamReader Reader { get;}
 
    TcpClient client;
    ServerObject server; // объект сервера
 
    public ClientObject(TcpClient tcpClient, ServerObject serverObject)
    {
        client = tcpClient;
        server = serverObject;
        // получаем NetworkStream для взаимодействия с сервером
        var stream = client.GetStream();
        // создаем StreamReader для чтения данных
        Reader = new StreamReader(stream);
        // создаем StreamWriter для отправки данных
        Writer = new StreamWriter(stream);
    }
 
    public async Task ProcessAsync()
    {
        try
        {
            // получаем имя пользователя
            string? userName = await Reader.ReadLineAsync();
            var user = new User { Color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)), 
                Name = userName, Id = Id};
            User = user;
            string? message = $"ENTER:{user.Id} {userName} {user.Color.ToArgb()}";
            // посылаем сообщение о входе в чат всем подключенным пользователям
            await server.BroadcastMessageAsync(message, Id);
            await server.BroadcastConnectedClientsToAsync(this);
            Console.WriteLine(message);
            // в бесконечном цикле получаем сообщения от клиента
            while (true)
            {
                try
                {
                    message = await Reader.ReadLineAsync();
                    if (message == null) continue;
                    message = $"ACTION:{user.Id} {message}";
                    Console.WriteLine(message);
                    await server.BroadcastMessageAsync(message, Id);
                }
                catch
                {
                    message = $"EXIT:{user.Id}";
                    Console.WriteLine(message);
                    await server.BroadcastMessageAsync(message, Id);
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            // в случае выхода из цикла закрываем ресурсы
            server.RemoveConnection(Id);
        }
    }
    // закрытие подключения
    protected internal void Close()
    {
        Writer.Close();
        Reader.Close();
        client.Close();
    }
}