using PaintLibrary;
using System.Net.Sockets;
using System.Text.Json;

namespace PaintOnlineClient
{
    public partial class Form1 : Form
    {
        string Host { get; } = "127.0.0.1";
        int Port { get; } = 8888;
        TcpClient Client { get; } = new();
        StreamReader? Reader { get; set; }
        StreamWriter? Writer { get; set; }

        public string UserName { get; set; }
        public Color Color { get; set; }

        Graphics g;
        Pen p = new Pen(Color.Black, 5);

        public Form1()
        {
            InitializeComponent();
            g = groupBox1.CreateGraphics();
        }

        ~Form1()
        { 
            Writer?.Close();
            Reader?.Close();
            Client.Dispose();
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            if (Client.Client.Connected)
                return;

            var button = sender as Button;
            button.Enabled = false;
            button.Visible = false;

            groupBox1.Click += SendPoint;

            UserName = playerName.Text;

            try
            {
                Client.Connect(Host, Port); //подключение клиента
                Reader = new StreamReader(Client.GetStream());
                Writer = new StreamWriter(Client.GetStream());
                if (Writer is null || Reader is null) return;
                // запускаем новый поток для получения данных
                Task.Run(() => ReceiveMessageAsync(Reader));
                // запускаем ввод сообщений
                await SendMessageAsync(Writer, playerName.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task SendMessageAsync(StreamWriter writer, string message)
        {
            // сначала отправляем имя
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();
        }

        async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    string? message = await reader.ReadLineAsync();
                    // если пустой ответ, ничего не выводим на консоль
                    if (string.IsNullOrEmpty(message)) continue;

                    if (message.StartsWith("user_connect"))
                    {
                        var user = JsonSerializer.Deserialize<User>(message.Replace("user_connect", ""));

                        playersList.Invoke(() =>
                        {
                            var label = new Label();
                            label.Text = user.UserName;
                            label.ForeColor = Color.FromArgb(user.Color);
                            playersList.Controls.Add(label);
                        });
                    } else if (message.StartsWith("user_action"))
                    {
                        var sendPoint = JsonSerializer.Deserialize<SendPoint>(message.Replace("user_action", ""));
                        Draw(sendPoint.Point, Color.FromArgb(sendPoint.Color));
                    }
                    //playersList.Items.Add(message);
                }
                catch
                {
                    break;
                }
            }
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            if (Writer is null) return;
            await SendMessageAsync(Writer, playerName.Text);
        }

        public void Draw(Point point, Color color)
        {
            var p = new Pen(color, 5);

            Rectangle r = new Rectangle();
            r.Width = 4;
            r.Height = 4;
            r.Location = new Point(point.X - r.Width / 2, point.Y - r.Height / 2);
            g.DrawEllipse(p, r);
        }

        private async void SendPoint(object sender, EventArgs e)
        {
            MouseEventArgs mouseArgs = e as MouseEventArgs;

            var json = JsonSerializer.Serialize(new SendPoint { UserName = UserName, Point = mouseArgs.Location });
            await SendMessageAsync(Writer, json);
        }
    }
}