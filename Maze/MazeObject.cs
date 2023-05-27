﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Maze
{
    public class MazeObject
    {
        public enum MazeObjectType { HALL, WALL, MEDAL, ENEMY, CHAR };

        public Bitmap[] images = {new Bitmap(@"C:\1\hall.png"),
            new Bitmap(@"C:\1\wall.png"),
            new Bitmap(@"C:\1\medal.png"),
            new Bitmap(@"C:\1\enemy.png"),
            new Bitmap(@"C:\1\player.png")};

        public MazeObjectType type;
        public int width;
        public int height;
        public Image texture;
        public PictureBox pictureBox;

        public MazeObject(MazeObjectType type)
        {
            Type = type;
            Width = 16;
            Height = 16;
            pictureBox = new PictureBox();
        }

        public MazeObjectType Type
        {
            get => type;
            set
            {
                type = value;
                Texture = images[(int)type];
            }
        }

        public int Width
        {
            get => width;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Error value (width)!");
                width = value;
            }
        }

        public int Height
        {
            get => height;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Error value (height)!");
                height = value;
            }
        }

        public Image Texture
        {
            get => texture;
            set => texture = value;
        }

        public PictureBox PictureBox => pictureBox;
        public void ChangeBackgroundImage(MazeObjectType type)
        {
            Type = type;
            pictureBox.BackgroundImage = Texture;
        }
    }
}