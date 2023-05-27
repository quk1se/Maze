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
        
        public Player()
        {
            this.playerLoc = new Point(0,2);
        }
        public void Move(int newX, int newY)
        {
            l.Maze[playerLoc.Y, playerLoc.X].ChangeBackgroundImage(MazeObjectType.HALL);
            l.Maze[newY, newX].ChangeBackgroundImage(MazeObjectType.CHAR);
            

            //l.ShowInfo();
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
                    break;

                case MazeObjectType.ENEMY:
                    break;

            }
            return true;
        }
    }
}
