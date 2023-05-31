using System;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace Maze
{
    public class Labirint
    {
        public static string miamiFilePath = @"C:\1\miamiTrack.wav";
        public static string parisFilePath = @"C:\1\Paris.wav";
        public static string gameOverFilePath = @"C:\1\gameOver.wav";
        public int height; // высота лабиринта (количество строк)
        public int width; // ширина лабиринта (количество столбцов в каждой строке)

        private MazeObject[,] maze;
        public PictureBox[,] images;
        public static Labirint l;
        private Player player = new Player();
        public static Random r = new Random();
        public Form parent;
        public MazeObject[,] Maze => maze;

        public int medalsCount = 0;
        public const int heal = 5;
        public const int energy = 25;
        public int enemiesCount = 0;
        public int stepCounts = 0;

        public string MiamiTrack
        {
            get { return miamiFilePath; }
        }
        public string WinTrack
        {
            get { return parisFilePath; }
        }
        public string GameOverTrack
        {
            get { return gameOverFilePath; }
        }
        public Labirint(Form parent, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.parent = parent;

            maze = new MazeObject[height, width];
            images = new PictureBox[height, width];

            Generate();
        }
        public void mediaPlayer(string filePath)
        {
            SoundPlayer mediaPlayer = new SoundPlayer(filePath);
            mediaPlayer.Play();
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
                        medalsCount++;
                    }

                    // в 1 случае из 250 - размещаем врага
                    if (r.Next(250) == 0)
                    {
                        int randEnemy = r.Next(1, 4);
                        if (randEnemy == 1)
                        {
                            current = MazeObject.MazeObjectType.ENEMY1;
                            enemiesCount++;
                        }
                        else if (randEnemy == 2)
                        {
                            current = MazeObject.MazeObjectType.ENEMY2;
                            enemiesCount++;
                        }
                        else if (randEnemy == 3)
                        {
                            current = MazeObject.MazeObjectType.ENEMY3;
                            enemiesCount++;
                        }
                    }

                    if (r.Next(250) == 0)
                    {
                        current = MazeObject.MazeObjectType.COFFEE;
                    }

                    if (r.Next(250) == 0)
                    {
                        current = MazeObject.MazeObjectType.HEAL;
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
                    images[y, x].Location = new Point(x * maze[y, x].width, y * maze[y, x].height);
                    images[y, x].Visible = true;
                }
            }
            mediaPlayer(MiamiTrack);
        }

        public void MovePlayer(KeyEventArgs e)
        {
            int newX = player.playerLoc.X;
            int newY = player.playerLoc.Y;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    newX = player.playerLoc.X;
                    newY = player.playerLoc.Y - 1;
                    if (player.CheckCollision(newX, newY))
                    {
                        player.Move(newX, newY);
                        player.playerLoc = new Point(newX, newY);
                        parent.Invalidate(); // Обновление отображения
                    }
                    break;

                case Keys.Down:
                    newX = player.playerLoc.X;
                    newY = player.playerLoc.Y + 1;
                    if (player.CheckCollision(newX, newY))
                    {
                        player.Move(newX, newY);
                        player.playerLoc = new Point(newX, newY);
                        parent.Invalidate(); // Обновление отображения
                    }
                    break;

                case Keys.Left:
                    newX = player.playerLoc.X - 1;
                    newY = player.playerLoc.Y;
                    if (player.CheckCollision(newX, newY))
                    {
                        player.Move(newX, newY);
                        player.playerLoc = new Point(newX, newY);
                        parent.Invalidate(); // Обновление отображения
                    }
                    break;

                case Keys.Right:
                    newX = player.playerLoc.X + 1;
                    newY = player.playerLoc.Y;
                    if (player.CheckCollision(newX, newY))
                    {
                        player.Move(newX, newY);
                        player.playerLoc = new Point(newX, newY);
                        parent.Invalidate(); // Обновление отображения
                    }
                    break;
            }
            player.PlayerEnergy -= 1;
            stepCounts++;
            if (newX >= 0)
            {
                images[newY, newX].BackgroundImage = maze[newY, newX].texture;
                images[newY, newX].Refresh();
            }
            EndLabirint();
        }
        public void EndLabirint()
        {
            if (player.playerLoc.X == 39 && player.playerLoc.Y == 17)
            {
                mediaPlayer(WinTrack);
                MessageBox.Show("Победа, вы прошли до конца лабиринта!");
                parent.Close();
            }
            if (player.MedalsClaim == medalsCount && medalsCount != 0)
            {
                mediaPlayer(WinTrack);
                MessageBox.Show("Победа, вы собрали все монетки!");
                parent.Close();
            }
            if (player.PlayerHealth == 0)
            {
                mediaPlayer(GameOverTrack);
                MessageBox.Show("Поражение, вас убили монстры!");
                parent.Close();
            }
            if (player.PlayerEnergy == 0)
            {
                mediaPlayer(GameOverTrack);
                MessageBox.Show("Поражение, у вас закончилась вся энергия!");
                parent.Close();
            }
            if (player.EnemiesKill == enemiesCount && enemiesCount != 0)
            {
                mediaPlayer(WinTrack);
                MessageBox.Show("Победа, все монстры повержены!");
                parent.Close();
            }
        }
        public void showTitle()
        {
            parent.Text = $"Hotline Miami (Maze)  Medals: {player.MedalsClaim} / {medalsCount} | Health: {player.PlayerHealth} | Energy: {player.PlayerEnergy}";
        }
    }
}
