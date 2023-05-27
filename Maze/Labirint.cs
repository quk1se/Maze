using System;
using System.Windows.Forms;
using System.Drawing;

namespace Maze
{
    public class Labirint
    {
        public int height; // высота лабиринта (количество строк)
        public int width; // ширина лабиринта (количество столбцов в каждой строке)

        private MazeObject[,] maze;
        public PictureBox[,] images;
        public static Labirint l;
        private Player player = new Player();
        public static Random r = new Random();
        public Form parent;
        public MazeObject[,] Maze => maze;

        public Labirint(Form parent, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.parent = parent;

            maze = new MazeObject[height, width];
            images = new PictureBox[height, width];

            Generate();
        }

        private void Generate()
        {

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    MazeObject.MazeObjectType current = MazeObject.MazeObjectType.HALL;

                    // в 1 случае из 5 - ставим стену
                    if (r.Next(5) == 0)
                    {
                        current = MazeObject.MazeObjectType.WALL;
                    }

                    // в 1 случае из 250 - кладём денежку
                    if (r.Next(250) == 0)
                    {
                        current = MazeObject.MazeObjectType.MEDAL;
                    }

                    // в 1 случае из 250 - размещаем врага
                    if (r.Next(250) == 0)
                    {
                        current = MazeObject.MazeObjectType.ENEMY;
                    }

                    // стены по периметру обязательны
                    if (y == 0 || x == 0 || y == height - 1 | x == width - 1)
                    {
                        current = MazeObject.MazeObjectType.WALL;
                    }

                    // наш персонажик
                    if (x == player.playerLoc.X && y == player.playerLoc.Y)
                    {
                        current = MazeObject.MazeObjectType.CHAR;
                    }

                    // есть выход, и соседняя ячейка справа всегда свободна
                    if (x == player.playerLoc.X + 1 && y == player.playerLoc.Y || x == width - 1 && y == height - 3)
                    {
                        current = MazeObject.MazeObjectType.HALL;
                    }
                    
                    maze[y, x] = new MazeObject(current);
                    images[y, x] = new PictureBox();
                    images[y, x].Location = new Point(x * maze[y, x].width, y * maze[y, x].height);
                    images[y, x].Parent = parent;
                    images[y, x].Width = maze[y, x].width;
                    images[y, x].Height = maze[y, x].height;
                    images[y, x].BackgroundImage = maze[y, x].texture;
                    images[y, x].Visible = false;
                }
            }
        }

        public void Show()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    images[y, x].Visible = true;
                }
            }
        }

        public void MovePlayer(KeyEventArgs e)
        {
            int newX = player.playerLoc.X;
            int newY = player.playerLoc.Y;

            switch (e.KeyCode)
            {
                case Keys.W:
                    if (player.CheckCollision(newX, newY - 1))
                    {
                        player.Move(newX, newY - 1);
                        player.playerLoc = new Point(newX, newY - 1);
                        parent.Text= player.playerLoc.ToString();
                    }
                    break;

                case Keys.S:
                    if (player.CheckCollision(newX, newY + 1))
                    {
                        player.Move(newX, newY + 1);
                        player.playerLoc = new Point(newX, newY + 1);
                        parent.Text = player.playerLoc.ToString();
                    }
                    break;

                case Keys.A:
                    if (player.CheckCollision(newX - 1, newY))
                    {
                        player.Move(newX - 1, newY);
                        player.playerLoc = new Point(newX - 1, newY);
                        parent.Text = player.playerLoc.ToString();
                    }
                    break;

                case Keys.D:
                    if (player.CheckCollision(newX + 1, newY))
                    {
                        player.Move(newX + 1, newY);
                        player.playerLoc = new Point(newX + 1, newY);
                        parent.Text = player.playerLoc.ToString();
                    }
                    break;
            }
        }
    }
}
