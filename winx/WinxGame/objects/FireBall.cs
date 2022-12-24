﻿using Timer = System.Threading.Timer;

namespace WinxGame.objects
{
    internal class FireBall : PictureBox, IDisposable
    {
        private TimerCallback _callback;
        private readonly Form _form;
        private readonly Fairy Target;
        public readonly int Damage = 100;

        public FireBall(Point location, Form form, Fairy target)
        {
            _form = form;
            Location = location;
            Image = Image.FromFile(Path.Join(Directory.GetCurrentDirectory(), @"images/Fireball_68x9.png"));
            Height = 100;
            Width = 100;
            SizeMode = PictureBoxSizeMode.StretchImage;

            _callback = new TimerCallback(Redraw);
            var timer = new Timer(_callback, null, 0, 200);
            Target = target;
        }

        public void Redraw(object obj)
        {
            _form.BeginInvoke(new Action(() =>
            {
                if ((Location.Y + Height) > _form.Height) {
                    _form.Controls.Remove(this);
                    Dispose();
                }
                else if (Target.IsCollision(this))
                {
                    _form.Controls.Remove(this);
                    Target.Hp -= Damage;
                    Dispose();
                }
                else 
                {
                    Location = new Point(Location.X, Location.Y + 10); 
                }
            }));
        }

    }
}
