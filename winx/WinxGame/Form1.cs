using System;
using WinxGame.objects;

namespace WinxGame
{
    public partial class Form1 : Form
    {
        private readonly Fairy _fairy;
        internal Spider[] _spiders;

        public Form1()
        {
            InitializeComponent();
            _fairy = new Fairy(new Point(Width / 2, (Height * 2) / 3), this);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Hide();
            Focus();
            Controls.Add(_fairy);

            _spiders = SpawnSpiders(Width, Height / 2);

            Controls.AddRange(_spiders);
        }

        private void Render(object obj)
        {

            foreach (var spider in _spiders)
            {
                spider.Redraw(spider.CreateFireBallTime);
                
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // MessageBox.Show(e.Key);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            _fairy.Move(e.KeyCode);
        }

        private Spider[] SpawnSpiders(int maxWidth, int maxHeigth)
        {
            var spiders = new Spider[10];
            var stepX = maxWidth / 10;
            var current = 0;


            for (var i = 0; i < 10; i++)
            {
                var rnd = new Random();

                var point = new Point(rnd.Next(current, current + stepX), rnd.Next(0, maxHeigth));
                var spider = new Spider(point, this, _fairy);
                spiders[i]=spider;
                current += stepX;
            }

            return spiders;
        }
        
        public void KillSpiders()
        {
            foreach (var s in _spiders)
                s.Dispose();
        }
    }
}