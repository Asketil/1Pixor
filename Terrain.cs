using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1Pixor
{
    class Terrain : Tile
    {
        public enum Type { Rock, Hole_Top, Hole_Bot, Air };
        public Type getType { get; private set; }

        public Terrain(Rectangle rec, GraphicsDevice gd, Type t) : base(rec, gd)
        {
            this.getType = t;
            if (t == Type.Air)
            {
                base.color = Color.White;
            }
            if (t == Type.Rock)
            {
                base.color = Color.Gray;
            }
            if (t == Type.Hole_Bot)
            {
                base.color = Color.Purple;
            }
            if (t == Type.Hole_Top)
            {
                base.color = Color.Blue;
            }
        }

        public static void terrainDraw(Terrain[,,] t, int lvl, int nbrWidth, int nbrHeight, SpriteBatch sp, GraphicsDevice gd)
        {
            Texture2D p = new Texture2D(gd, 1, 1, true, SurfaceFormat.Color);
            p.SetData(new[] { Color.White });

            for (int i = 0; i < nbrWidth; i++)
            {
                for (int j = 0; j < nbrHeight; j++)
                {
                    sp.Draw(p, t[lvl, i, j].rec, t[lvl, i, j].color);
                }
            }
        }
    }
}
