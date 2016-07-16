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
        bool loaded;

        public int[,] solution;
        public List<string> log = new List<string>();
        int count = 0;
        int map_height = 0;
        int map_width = 0;

        int solve_step = 1;

        public void solveMaze(int[,] maze, int N) 
        {
            solution = new int[N,N];
		    for (int i = 0; i < N; i++)
            {
			    for (int j = 0; j < N; j++)
                {
				    solution[i,j] = 0;
			    }
		    }
		    if (findPath(maze, 0, 0, N, "down")) {
                for (int i = 0; i < N; i++)
                {
                    string temp = "";
                    for (int j = 0; j < N; j++)
                    {
                        temp += " " + solution[j, i];
                    }
                    log.Add(temp);
                }
		    }else{
			    log.Add("Impossible Map");
		    }
	    }

        public bool findPath(int[,] maze, int x, int y, int N, String direction)
        {
            if (x == N - 1 && y == N - 1)
            {
                solution[x,y] = count;
                return true;
            }
            else if (x >= 0 && y >= 0 && x < N && y < N && maze[x, y] != 0)
            {
                solution[x,y] = count++;
                if (direction != "up" && findPath(maze, x + 1, y, N, "down"))
                {
                    return true;
                }
                if (direction != "left" && findPath(maze, x, y + 1, N, "right"))
                {
                    return true;
                }
                if (direction != "down" && findPath(maze, x - 1, y, N, "up"))
                {
                    return true;
                }
                if (direction != "right" && findPath(maze, x, y - 1, N, "left"))
                {
                    return true;
                }
                //if none of the options work out BACKTRACK undo the move
                solution[x,y] = 0;
                count--;
                return false;
            }
            return false;
        }

        private void refreshGerak()
        {
            PlayerInPx.X = (Player.X * 30) + 5;
            PlayerInPx.Y = (Player.Y * 30) + 29;
            this.Invalidate();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ImgTembok = Proyek_Alpro.Properties.Resources.tembok;
            ImgLantai = Proyek_Alpro.Properties.Resources.lantai;
            ImgPlayer = Proyek_Alpro.Properties.Resources.player;
            Player.X = 0;
            Player.Y = 0;
            refreshGerak();
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (loaded == true)
            {
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        if (peta[j, i] == 1)
                        {
                            g.DrawImage(ImgLantai, j * 30, (i * 30) + 24, 30, 30);
                        }
                        else
                        {
                            g.DrawImage(ImgTembok, j * 30, (i * 30) + 24, 30, 30);
                        }
                    }
                }
                g.DrawImage(ImgPlayer, PlayerInPx.X, PlayerInPx.Y, 20, 20);
            }
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (e.KeyCode == Keys.Right)
            {
                if (peta[Player.X + 1,Player.Y] == 1)
                {
                    Player.X++;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                if (peta[Player.X - 1, Player.Y] == 1)
                {
                    Player.X--;
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                if (peta[Player.X, Player.Y - 1] == 1)
                {
                    Player.Y--;
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                if (Player.Y == 14)
                {
                    Player.Y = 14;
                    this.Invalidate();
                }
                else if (peta[Player.X, Player.Y + 1] == 1)
                {
                    Player.Y++;
                }
            }
            refreshGerak();
        }

        private void loadMapToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath + @"\asset\map";
            openFileDialog1.Filter = "TXT File (*.txt) | *.txt" + "|" + "XML File (*.xml) | *.xml";
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
                    this.Invalidate();
                }
            }
        }

        private void solveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //RatInMaze r = new RatInMaze(15);
            solveMaze(peta, 15);
            for (int i = 0; i < log.Count(); i++)
            {
                listBox1.Items.Add(log[i]);
            }
            timer1.Enabled = true;
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
                    if (solution[h,w] == solve_step)
                    {
                        Player.X = h;
                        Player.Y = w;
                        refreshGerak();
                        break;
                    }
                }
                if (w == map_width - 2)
                {
                    solve_step++;
                }
            }
            if (solution[map_height - 1,map_width - 1] == solve_step)
            {
                timer1.Enabled = false;
            }
        }
    }
}
