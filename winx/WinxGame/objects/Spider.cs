using Timer = System.Threading.Timer;

namespace WinxGame.objects
{
    internal class Spider : PictureBox
    {        
        private TimerCallback _callback;

        public int Hp { get; set; }

        private readonly List<FireBall> _fireBalls;
        private readonly Form _form;
        private readonly Random _random = new();
        private readonly Fairy Target;
        private readonly Timer timer;

        public int CreateFireBallTime { get; }

        public Spider(Point location, Form form, Fairy target)
        {
            Location = location;
            Image = Image.FromFile(Path.Join(Directory.GetCurrentDirectory(), @"images/Skullback Spider.png"));
            Height = 50;
            Width = 50;
            SizeMode = PictureBoxSizeMode.StretchImage;
            Hp = 10;

            _form = form;
            _fireBalls = new List<FireBall>();

            CreateFireBallTime = _random.Next(5000, 15000);

            _callback = new TimerCallback(Redraw);
            timer = new Timer(_callback, null, CreateFireBallTime, CreateFireBallTime);
            Target = target;
        }

        public void CreateFireBall()
        {
            var fireBall = new FireBall(Location, _form, Target);
            _form.BeginInvoke(() =>
            {
                _form.Controls.Add(fireBall);
            });

            _fireBalls.Add(fireBall);
        }

        public void Redraw(object obj)
        {  // if (time % CreateFireBallTime == 0)
           
            CreateFireBall();
        }

        public void Dispose()
        {
            _form.Controls.Remove(this);
            timer.Dispose();
            foreach(var f in _fireBalls)
                f.Dispose();
        }
    }
}
