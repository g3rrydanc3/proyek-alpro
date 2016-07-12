using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Proyek_Alpro
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        Graphics g;
        Image tembok;
        Image lantai;
        Image player;
        int x;
        int y;
        int PlayerX, PlayerY;

        
        int[,] peta = new int[15, 15];
        bool cek;

        private void Form3_Load(object sender, EventArgs e)
        {
            PlayerX = 0;
            PlayerY = 0;
            tembok = Image.FromFile("tembok.jpg");
            lantai = Image.FromFile("lantai.jpg");
            player = Image.FromFile("1.png");
            //StreamReader map = new StreamReader(Application.StartupPath + "/1.txt");
            //int counterBaris = 0;
            //while (!map.EndOfStream)
            //{
            //    String data = map.ReadLine();
            //    for (int i = 0; i < 15; i++)
            //    {
            //        peta[i, counterBaris] = (int)Char.GetNumericValue(data[i]);
            //        //MessageBox.Show(data[i].ToString());
            //    }
            //    counterBaris++;
            //}
            //map.Close();
            //refreshGerak();
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            if (cek == true)
            {
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        if (peta[j, i] == 1)
                        {
                            g.DrawImage(lantai, j * 45, (i * 45) + 24, 45, 45);
                        }
                        else
                        {
                            g.DrawImage(tembok, j * 45, (i * 45) + 24, 45, 45);
                        }
                    }
                }
                g.DrawImage(player, x, y, 35, 35);
            }
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (peta[PlayerX + 1,PlayerY] == 1)
                {
                    PlayerX++;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                if (peta[PlayerX - 1, PlayerY] == 1)
                {
                    PlayerX--;
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                if (peta[PlayerX, PlayerY - 1] == 1)
                {
                    PlayerY--;
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                if (PlayerY == 14)
                {
                    PlayerY = 14;
                    this.Invalidate();
                }
                else if (peta[PlayerX, PlayerY + 1] == 1)
                {
                    PlayerY++;
                }
            }
            refreshGerak();
            this.Invalidate();
        }

        private void drawMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cek = false;
        }

        private void loadMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Title = "Open File Txt";
            openFileDialog1.Filter = "File text (*.txt.) | *.txt" + "|" + "All Files| *.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                StreamReader map = new StreamReader(filename);
                int counterBaris = 0;
                while (!map.EndOfStream)
                {
                    String data = map.ReadLine();
                    for (int i = 0; i < 15; i++)
                    {
                        peta[i, counterBaris] = (int)Char.GetNumericValue(data[i]);
                    }
                    counterBaris++;
                }
                map.Close();
                cek = true;
            }
            for (int i = 0; i < 15; i++)
            {
                if (peta[0,i] == 1)
                {
                    PlayerY = i;
                    refreshGerak();
                }
            }
            this.Invalidate();
        }

        private void refreshGerak()
        {
            x = (PlayerX * 45) + 5;
            y = (PlayerY * 45) + 29;
        }
    }
}
