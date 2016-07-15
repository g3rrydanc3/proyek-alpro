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
        public class RatInMaze
        {
            public int[,] solution;
            public List<string> log = new List<string>();

            //initialize the solution matrix in constructor.
            public RatInMaze(int N) {
		        solution = new int[N,N];
		        for (int i = 0; i < N; i++) {
			        for (int j = 0; j < N; j++) {
				        solution[i,j] = 0;
			        }
		        }
	        }

            public void solveMaze(int[,] maze, int N) {
		        if (findPath(maze, 0, 0, N, "down")) {
			        print(solution, N);
		        }else{
			        log.Add("NO PATH FOUND");
		        }
		
	        }

            public bool findPath(int[,] maze, int x, int y, int N, String direction)
            {
                // check if maze[x,y] is feasible to move
                if (x == N - 1 && y == N - 1)
                {//we have reached
                    solution[x,y] = 1;
                    return true;
                }
                if (isSafeToGo(maze, x, y, N))
                {
                    // move to maze[x,y]
                    solution[x,y] = 1;
                    // now rat has four options, either go right OR go down or left or go up
                    if (direction != "up" && findPath(maze, x + 1, y, N, "down"))
                    { //go down
                        return true;
                    }
                    //else go down
                    if (direction != "left" && findPath(maze, x, y + 1, N, "right"))
                    { //go right
                        return true;
                    }
                    if (direction != "down" && findPath(maze, x - 1, y, N, "up"))
                    { //go up
                        return true;
                    }
                    if (direction != "right" && findPath(maze, x, y - 1, N, "left"))
                    { //go left
                        return true;
                    }
                    //if none of the options work out BACKTRACK undo the move
                    solution[x,y] = 0;
                    return false;
                }
                return false;
            }

            // this function will check if mouse can move to this cell
            public bool isSafeToGo(int[,] maze, int x, int y, int N)
            {
                // check if x and y are in limits and cell is not blocked
                if (x >= 0 && y >= 0 && x < N && y < N && maze[x,y] != 0)
                {
                    return true;
                }
                return false;
            }
            public void print(int[,] solution, int N){
		        for (int i = 0; i < N; i++) {
                    string temp = "";
			        for (int j = 0; j < N; j++) {
				        temp += " " + solution[j,i];
			        }
                    log.Add(temp);
		        }
            }
        }

        private void refreshGerak()
        {
            PlayerInPx.X = (Player.X * 30) + 5;
            PlayerInPx.Y = (Player.Y * 30) + 29;
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
            this.Invalidate();
        }

        private void loadMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath + @"\asset\map";
            openFileDialog1.Filter = "TXT File (*.txt) | *.txt" + "|" + "XML File (*.xml) | *.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                StreamReader file = new StreamReader(filename);
                int counterBaris = 0;
                while (!file.EndOfStream)
                {
                    String data = file.ReadLine();
                    for (int i = 0; i < 15; i++)
                    {
                        peta[i, counterBaris] = (int)Char.GetNumericValue(data[i]);
                    }
                    counterBaris++;
                }
                file.Close();
                loaded = true;
                this.Invalidate();
            }
        }

        private void solveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RatInMaze r = new RatInMaze(15);
            r.solveMaze(peta, 15);
            for (int i = 0; i < r.log.Count(); i++)
            {
                listBox1.Items.Add(r.log[i]);
            }
        }
    }
}
