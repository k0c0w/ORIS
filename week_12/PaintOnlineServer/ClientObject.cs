using PaintLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaintOnlineServer
{
    internal class ClientObject
    {
        protected internal string Id { get; } = Guid.NewGuid().ToString();
        protected internal StreamWriter Writer { get; }
        protected internal StreamReader Reader { get; }

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
                var rnd = new Random();
                // получаем имя пользователя
                string? userName = await Reader.ReadLineAsync();
                if (string.IsNullOrEmpty(userName.Trim())) return;
                var color = System.Drawing.Color.FromArgb(rnd.Next(255), 
                                            rnd.Next(255), 
                                            rnd.Next(255));

                string? message = $"{userName} вошел в игру";
                // посылаем сообщение о входе в чат всем подключенным пользователям
                var json = JsonSerializer.Serialize(new User { UserName = userName, Color = color.ToArgb() });
                await server.BroadcastMessageAsync("user_connect" + json, Id);
                Console.WriteLine(message);

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message == null) continue;
                            var point = JsonSerializer.Deserialize<SendPoint>(message);
                            point.Color = color.ToArgb();
                            //Console.WriteLine(point);
                            await server.BroadcastMessageAsync("user_action" + JsonSerializer.Serialize(point), Id);
                    }
                    catch
                    {
                        message = $"{userName} покинул в игру";
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
}
