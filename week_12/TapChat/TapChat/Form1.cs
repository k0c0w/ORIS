using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TapChat
{
    public partial class Form1 : Form
    {
        private string host = "127.0.0.1";
        private int port = 8888;
        TcpClient client = new TcpClient();
        private Graphics graphics;
        StreamReader? Reader = null;
        StreamWriter? Writer = null;
        private Dictionary<Guid, (User, Control)> connectedUsers = new Dictionary<Guid, (User,Control)>();
        public Form1()
        {
            InitializeComponent();
            graphics = field.CreateGraphics();
        }

        private async void enterBtn_Click(object sender, EventArgs e)
        {
            var username = textBox1.Text;
            try
            {
                client.Connect(host, port);
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());
                await SendMessageAsync(Writer, username);
                ReceiveMessageAsync(Reader);
                entryBox.Enabled = false;
                entryBox.Visible = false;
                chatBox.Visible = true;
                chatBox.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async Task SendMessageAsync(StreamWriter writer, string message)
        {
            await writer?.WriteLineAsync(message);
            await writer?.FlushAsync();
        }

        async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    var message = await reader.ReadLineAsync();
                    // если пустой ответ, ничего не выводим на консоль
                    if (string.IsNullOrEmpty(message)) continue;
                   
                    HandleMessage(message);
                }
                catch
                {
                    MessageBox.Show("обрываю соединения");
                    break;
                }
            }
            client.Close();
        }

        private void HandleMessage(string message)
        {
            var m = message.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (m[0]=="ENTER")
            {
                m = m[1].Split();
                var guid = Guid.Parse(m[0]);
                var user = new User
                {
                    Id = guid,
                    Name = m[1],
                    Color = Color.FromArgb(int.Parse(m[2]))
                };
                var text = new TextBox { Text = user.Name, ForeColor = user.Color, Enabled = false };
                connectedUsers.Add(guid, (user, text));
                chatUsers.Controls.Add(text);

            }
            else if (m[0] == "EXIT")
            {
                var user = connectedUsers[Guid.Parse(m[1])];
                connectedUsers.Remove(user.Item1.Id);
                chatUsers.Controls.Remove(user.Item2);
            }
            else if (message.StartsWith("INFO"))
            {
                message = message.Remove(0, 5);

                var users = JsonSerializer.Deserialize<User[]>(message);
                foreach(var user in users)
                {
                    var text = new TextBox { Text = user.Name, ForeColor = user.Color, Enabled = false };
                    connectedUsers.Add(user.Id, (user, text));
                    chatUsers.Controls.Add(text);
                }
            }
            else
            {
                m = m[1].Split();
                var guid = Guid.Parse(m[0]);
                var user = connectedUsers[guid].Item1;
                m = m[1].Split(';');
                var point = new Point(int.Parse(m[0]), int.Parse(m[1]));
                Pen pn = new Pen(user.Color, 5);
                var g =graphics;
                g.DrawEllipse(pn, point.X - 5, point.Y -5, 10, 10);
            }
        }

        private void OnFieldClicked(object sender, EventArgs e)
        {
            var x = Cursor.Position.X;
            var y = Cursor.Position.Y;
            Pen pn = new Pen(Color.Black, 5);
            var g = graphics;
            g.DrawEllipse(pn, x - 5, y - 5, 10, 10);
            SendMessageAsync(Writer, $"{x};{y}");
        }
    }
}