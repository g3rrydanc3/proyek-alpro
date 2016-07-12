using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyek_Alpro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int splashed = 0;
        int timer = 0;
        Image logo;
        Graphics g;

        private void Form1_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.AppStarting;
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            //logo = Image.FromFile("The Maze.gif");
            timer1.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            //g.DrawImage(logo, 250, 210, 200, 100);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer++;
            if (timer > 1 && timer < 4)
            {
                if (splashed == 0)
                {
                    splashed = 1;
                }
                else
                {
                    this.Controls.Remove(pictureBox1);
                    PictureBox pic = new PictureBox();
                    pic.Image = Image.FromFile("The Maze.gif");
                    pic.SizeMode = PictureBoxSizeMode.AutoSize;
                    pic.Location = new Point(160, 300);
                    this.Controls.Add(pic);
                }
            }
            else if (timer > 4)
            {
                Cursor = Cursors.Default;
                this.Invalidate();
                splashed = 2;
                timer1.Stop();
                Label label = new Label();
                label.Text = "Press any key to continue...";
                label.Size = new Size(500, 70);
                label.Font = new Font("Century gothic", 18, FontStyle.Bold);
                label.Location = new Point(0, 655);
                label.ForeColor = Color.White;
                label.BackColor = Color.Transparent;
                this.Controls.Add(label);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (splashed == 2)
            {
                Form3 f = new Form3();
                f.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.ShowDialog();
        }
    }
}
