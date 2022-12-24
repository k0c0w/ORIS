namespace WinxGame.objects
{
    internal class Fairy : PictureBox
    {
        public int Hp { get; set; }
        private readonly Form1 _form;

        public Fairy(Point location, Form1 form)
        {
            Location = location;
            Image = Image.FromFile(Path.Join(Directory.GetCurrentDirectory(), @"images/bloom.png"));
            Height = 100;
            Width = 100;
            SizeMode = PictureBoxSizeMode.StretchImage;
            Hp = 100;
            _form = form;
        }

        public bool IsCollision(FireBall fireBall)
        {
            var collision = Math.Abs(fireBall.Location.X - Location.X) < 100 && Math.Abs(fireBall.Location.Y - Location.Y) < 100;
            if (collision) 
            {
                Hp -= fireBall.Damage;
                if (Hp <= 0)
                {
                    _form.Controls.Remove(this);
                    _form.KillSpiders();
                    Dispose();
                }
            }

            return collision;
        }

        public void Move(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    Location = new Point(Location.X, Location.Y - 10);
                    break;
                case Keys.Down:
                    Location = new Point(Location.X, Location.Y + 10);
                    break;
                case Keys.Left:
                    Location = new Point(Location.X - 10, Location.Y);
                    break;
                case Keys.Right:
                    Location = new Point(Location.X + 10, Location.Y);
                    break;
            }
        }

       
    }
}
