using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1Pixor
{
    class Player : Tile
    {
        public int widthCase { get; private set; }
        public int heightCase { get; private set; }
        public int nbrblock { get; private set; }

        public Player(Rectangle rec, Color color, GraphicsDevice gd, int widthCase, int heightCase)
                      : base(rec, color, gd)
        {
            this.widthCase = widthCase;
            this.heightCase = heightCase;
            this.nbrblock = 0;
        }

        public void move(int flag, int speed, int nbrWidth, int nbrHeight, Terrain[,,] t)
        {
            if (flag == 0)
            {
                if (rec.Y - speed >= 0 && t[Game1.lvl, this.widthCase, this.heightCase - 1].getType != Terrain.Type.Rock)
                {
                    rec.Y -= speed;
                    heightCase--;
                }
            }
            if (flag == 1)
            {
                if (rec.X + speed < nbrWidth * speed && t[Game1.lvl, this.widthCase + 1, this.heightCase].getType != Terrain.Type.Rock)
                {
                    rec.X += speed;
                    widthCase++;
                }
            }
            if (flag == 2)
            {
                if (rec.Y + speed < nbrHeight * speed && t[Game1.lvl, this.widthCase, this.heightCase + 1].getType != Terrain.Type.Rock)
                {
                    rec.Y += speed;
                    heightCase++;
                }
            }
            if (flag == 3)
            {
                if (rec.X - speed >= 0 && t[Game1.lvl, this.widthCase - 1, this.heightCase].getType != Terrain.Type.Rock)
                {
                    rec.X -= speed;
                    widthCase--;
                }
            }
        }

        public int changeLvl(Terrain[,,] t)
        {
            if (t[Game1.lvl, this.widthCase, this.heightCase].getType == Terrain.Type.Hole_Bot)
            {
                return 1;
            }
            if (t[Game1.lvl, this.widthCase, this.heightCase].getType == Terrain.Type.Hole_Top)
            {
                return 0;
            }
            return -1;
        }

        public Terrain[,,] put(int direction, int X, int Y, int taille, int nbrWidth, int nbrHeight, Terrain[, ,] t, GraphicsDevice gd)
        {
            if (direction == 1 && this.heightCase > 0 && t[Game1.lvl, X, Y - 1].getType == Terrain.Type.Air)
            {
                t[Game1.lvl, X, Y - 1] = new Terrain(new Rectangle(X * taille, Y * taille - taille, taille, taille), gd, Terrain.Type.Rock);
                nbrblock--;
            }
            if (direction == 2 && this.widthCase < nbrWidth * taille && t[Game1.lvl, X + 1, Y].getType == Terrain.Type.Air)
            {
                t[Game1.lvl, X + 1, Y] = new Terrain(new Rectangle(X * taille + taille, Y * taille, taille, taille), gd, Terrain.Type.Rock);
                nbrblock--;
            }
            if (direction == 3 && this.heightCase < nbrHeight * taille && t[Game1.lvl, X, Y + 1].getType == Terrain.Type.Air)
            {
                t[Game1.lvl, X, Y + 1] = new Terrain(new Rectangle(X * taille, Y * taille + taille, taille, taille), gd, Terrain.Type.Rock);
                nbrblock--;
            }
            if (direction == 4 && this.widthCase > 0 && t[Game1.lvl, X - 1, Y].getType == Terrain.Type.Air)
            {
                t[Game1.lvl, X - 1, Y] = new Terrain(new Rectangle(X * taille - taille, Y * taille, taille, taille), gd, Terrain.Type.Rock);
                nbrblock--;
            }

            return t;
        }

        public Terrain[,,] dig(int direction, int X, int Y, int taille, int nbrWidth, int nbrHeight, Terrain[,,] t, GraphicsDevice gd)
        {
            if (direction == 0)
            {
                if (t[0, X, Y].getType != Terrain.Type.Hole_Bot)
                    nbrblock++;
                t[0, X, Y] = new Terrain(new Rectangle(X * taille, Y * taille, taille, taille), gd, Terrain.Type.Hole_Bot);
                t[1, X, Y] = new Terrain(new Rectangle(X * taille, Y * taille, taille, taille), gd, Terrain.Type.Hole_Top);
            }
            if (direction == 1 && this.heightCase > 0 && t[Game1.lvl, X, Y - 1].getType != Terrain.Type.Hole_Bot && t[Game1.lvl, X, Y - 1].getType != Terrain.Type.Hole_Top)
            {
                if (t[Game1.lvl, X, Y - 1].getType == Terrain.Type.Rock)
                    nbrblock++;
                t[Game1.lvl, X, Y - 1] = new Terrain(new Rectangle(X * taille, Y * taille - taille, taille, taille), gd, Terrain.Type.Air);
            }
            if (direction == 2 && this.widthCase < nbrWidth * taille && t[Game1.lvl, X + 1, Y].getType != Terrain.Type.Hole_Bot && t[Game1.lvl, X + 1, Y].getType != Terrain.Type.Hole_Top)
            {
                if (t[Game1.lvl, X + 1, Y].getType == Terrain.Type.Rock)
                    nbrblock++;
                t[Game1.lvl, X + 1, Y] = new Terrain(new Rectangle(X * taille + taille, Y * taille, taille, taille), gd, Terrain.Type.Air);
            }
            if (direction == 3 && this.heightCase < nbrHeight * taille && t[Game1.lvl, X, Y + 1].getType != Terrain.Type.Hole_Bot && t[Game1.lvl, X, Y + 1].getType != Terrain.Type.Hole_Top)
            {
                if (t[Game1.lvl, X, Y + 1].getType == Terrain.Type.Rock)
                    nbrblock++;
                t[Game1.lvl, X, Y + 1] = new Terrain(new Rectangle(X * taille, Y * taille + taille, taille, taille), gd, Terrain.Type.Air);
            }
            if (direction == 4 && this.widthCase > 0 && t[Game1.lvl, X - 1, Y].getType != Terrain.Type.Hole_Bot && t[Game1.lvl, X - 1, Y].getType != Terrain.Type.Hole_Top)
            {
                if (t[Game1.lvl, X - 1, Y].getType == Terrain.Type.Rock && t[Game1.lvl, X, Y - 1].getType != Terrain.Type.Hole_Bot && t[Game1.lvl, X, Y - 1].getType != Terrain.Type.Hole_Top)
                    nbrblock++;
                t[Game1.lvl, X - 1, Y] = new Terrain(new Rectangle(X * taille - taille, Y * taille, taille, taille), gd, Terrain.Type.Air);
            }

            return t;
        }
    }
}
