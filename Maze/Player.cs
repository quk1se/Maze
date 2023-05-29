using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Maze.MazeObject;

namespace Maze
{
    internal class Player
    {
        public static Labirint l;
        public Point playerLoc;
        public int medalsClaim = 0;
        public int playerHealth = 100;
        Random rand = new Random();
        public Player()
        {
            this.playerLoc = new Point(0,2);
        }
        public void Move(int newX, int newY)
        {
            l.Maze[playerLoc.Y, playerLoc.X].ChangeBackgroundImage(MazeObjectType.HALL);
            l.images[playerLoc.Y, playerLoc.X].BackgroundImage = l.Maze[playerLoc.Y, playerLoc.X].texture;
            l.Maze[newY, newX].ChangeBackgroundImage(MazeObjectType.CHAR);
            l.showTitle();
        }
        public static void SetLabirint(Labirint labirint)
        {
            l = labirint;
        }
        public bool CheckCollision(int newX, int newY)
        {
            if (newX < 0) return false;

            switch (l.Maze[newY, newX].Type)
            {
                case MazeObjectType.WALL:
                    return false;

                case MazeObjectType.HALL:
                    break;

                case MazeObjectType.MEDAL:
                    medalsClaim++;
                    break;

                case MazeObjectType.ENEMY:
                    int random = rand.Next(20, 26);
                    if (playerHealth - random < 0) playerHealth = 0;
                    else playerHealth -= random;
                    break;
                case MazeObjectType.HEAL:
                    if (playerHealth + Labirint.heal > 100) playerHealth = 100;
                    if (playerHealth != 100) playerHealth += Labirint.heal;
                    break;
            }
            return true;
        }
    }
}
