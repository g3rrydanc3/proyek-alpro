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
using System.Media;

namespace Proyek_Alpro
{
    public partial class FormGame : Form
    {
        public FormGame()
        {
            InitializeComponent();
        }
        
        Image ImgTembok;
        Image ImgLantai;
        Image ImgPlayer;
        Point Player;
        Point PlayerInPx;

        int[,] peta = new int[15, 15];
        bool loaded, enableKeys;

        public int[,] solusi;
        public List<string> log = new List<string>();
        int count = 0;
        int map_height = 0;
        int map_width = 0;

        int gerakX, gerakY;

        int solve_step = 1;

        SoundPlayer tabrak = new SoundPlayer(Proyek_Alpro.Properties.Resources.tabrak);

        public void solveMaze(int[,] maze, int N) 
        {
            solusi = new int[N,N];
		    for (int i = 0; i < N; i++)
            {
			    for (int j = 0; j < N; j++)
                {
				    solusi[i,j] = 0;
			    }
		    }
		    if (cariJalan(maze, 0, 0, N, "down")) {
                for (int i = 0; i < N; i++)
                {
                    string temp = "";
                    for (int j = 0; j < N; j++)
                    {
                        temp += " " + solusi[j, i];
                    }
                    log.Add(temp);
                }
		    }else{
			    log.Add("Impossible Map");
		    }
	    }

        public bool cariJalan(int[,] maze, int x, int y, int N, String arah)
        {
            if (x == N - 1 && y == N - 1)
            {
                solusi[x,y] = count;
                return true;
            }
            else if (x >= 0 && y >= 0 && x < N && y < N && maze[x, y] != 0)
            {
                solusi[x,y] = count++;
                if (arah != "up" && cariJalan(maze, x + 1, y, N, "down"))
                {
                    return true;
                }
                if (arah != "left" && cariJalan(maze, x, y + 1, N, "right"))
                {
                    return true;
                }
                if (arah != "down" && cariJalan(maze, x - 1, y, N, "up"))
                {
                    return true;
                }
                if (arah != "right" && cariJalan(maze, x, y - 1, N, "left"))
                {
                    return true;
                }
                //if none of the options work out BACKTRACK undo the move
                solusi[x,y] = 0;
                count--;
                return false;
            }
            return false;
        }

        private void playerGerak(int x, int y)
        {
            int tempX = x - Player.X;
            int tempY = y - Player.Y;
            Player.X = x;
            Player.Y = y;
            gerakX = tempX * 30;
            gerakY = tempY * 30;
            //PlayerInPx.X = (Player.X * 30) + 5;
            //PlayerInPx.Y = (Player.Y * 30) + 29;
            t2gerak.Enabled = true;
            enableKeys = false;
            this.Invalidate();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ImgTembok = Proyek_Alpro.Properties.Resources.tembok;
            ImgLantai = Proyek_Alpro.Properties.Resources.lantai;
            ImgPlayer = Proyek_Alpro.Properties.Resources.player;
            playerGerak(0, 0);
            PlayerInPx.X = (Player.X * 30) + 35;
            PlayerInPx.Y = (Player.Y * 30) + 59;
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (loaded == true)
            {
                for (int i = 0; i < 17; i++)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        if (i == 0 || i == 16 || j == 0 || j == 16)
                        {
                            g.DrawImage(ImgTembok, (j * 30), (i * 30) + 24, 30, 30);
                        }
                        else if (peta[j - 1, i - 1] == 1)
                        {
                            g.DrawImage(ImgLantai, (j * 30), (i * 30) + 24, 30, 30);
                        }
                        else
                        {
                            g.DrawImage(ImgTembok, (j * 30), (i * 30) + 24, 30, 30);
                        }
                    }
                }
                g.DrawImage(ImgPlayer, PlayerInPx.X, PlayerInPx.Y, 20, 20);
            }
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            if (enableKeys)
            {
                e.SuppressKeyPress = true;
                if (e.KeyCode == Keys.Right)
                {
                    if (Player.X < 14 && peta[Player.X + 1, Player.Y] == 1)
                    {
                        playerGerak(Player.X + 1,Player.Y);
                    }
                    else
                    {
                        tabrak.Play();
                    }
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (Player.X > 0 && peta[Player.X - 1, Player.Y] == 1)
                    {
                        playerGerak(Player.X - 1, Player.Y);
                    }
                    else
                    {
                        tabrak.Play();
                    }
                }
                if (e.KeyCode == Keys.Up)
                {
                    if (Player.Y > 0 && peta[Player.X, Player.Y - 1] == 1)
                    {
                        playerGerak(Player.X, Player.Y - 1);
                    }
                    else
                    {
                        tabrak.Play();
                    }
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (Player.Y < 14 && peta[Player.X, Player.Y + 1] == 1)
                    {
                        playerGerak(Player.X, Player.Y + 1);
                    }
                    else
                    {
                        tabrak.Play();
                    }
                }
                
            }
        }

        private void loadMapToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath + @"\asset\map";
            openFileDialog1.Filter = "TXT File (*.txt) | *.txt";// + "|" + "XML File (*.xml) | *.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(openFileDialog1.FileName);
                bool error = false;
                while (!file.EndOfStream)
                {
                    String data = file.ReadLine();
                    if (map_height == 0)
                    {
                        map_width = data.Count();
                    }

                    if (map_width == data.Count())
                    {
                        for (int i = 0; i < map_width; i++)
                        {
                            if (data[i] == '1' || data[i] == '0')
                            {
                                peta[i, map_height] = (int)Char.GetNumericValue(data[i]);
                            }
                            else
                            {
                                error = true;
                            }
                        }
                    }
                    else
                    {
                        error = true;
                    }
                    map_height++;
                }
                file.Close();

                if (error)
                {
                    MessageBox.Show("File tidak sesuai");
                }
                else
                {
                    loaded = true;
                    enableKeys = true;
                    solveToolStripMenuItem.Enabled = true;
                    this.Invalidate();
                }
            }
        }

        private void solveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            solveMaze(peta, 15);
            for (int i = 0; i < log.Count(); i++)
            {
                listBox1.Items.Add(log[i]);
            }
            timer1.Enabled = true;
            enableKeys = false;
            solveToolStripMenuItem.Enabled = false;
        }

        private void howToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHowToPlay f = new FormHowToPlay();
            f.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout f = new FormAbout();
            f.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int w = 0; w < map_width; w++)
            {
                for (int h = 0; h < map_height; h++)
                {
                    if (solusi[h,w] == solve_step)
                    {
                        playerGerak(h, w);
                    }
                }
                if (w == map_width - 1)
                {
                    solve_step++;
                }
            }
            if (solusi[map_height - 1,map_width - 1] == solve_step - 1)
            {
                timer1.Enabled = false;
                enableKeys = true;
            }
        }

        private void t2gerak_Tick(object sender, EventArgs e)
        {
            int increment = 10;
            if (gerakX != 0 || gerakY != 0)
            {
                if (gerakX > 0)
                {
                    PlayerInPx.X += increment;
                    gerakX-= increment;
                }
                else if ( gerakX < 0)
                {
                    PlayerInPx.X-=increment;
                    gerakX+=increment;
                }
                if (gerakY > 0)
                {
                    PlayerInPx.Y+=increment;
                    gerakY-=increment;
                }
                else if (gerakY < 0)
                {
                    PlayerInPx.Y-= increment;
                    gerakY+=increment;
                }
                this.Invalidate();
            }
            else
            {
                t2gerak.Enabled = false;
                enableKeys = true;
            }
        }
    }
}
