using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1Pixor
{
    abstract class Tile
    {
        protected Color color;
        public Rectangle rec;
        public Texture2D pixel { get; private set; }

        public Tile(Rectangle rec, Color color, GraphicsDevice gd)
        {
            this.rec = rec;
            this.color = color;
            this.pixel = new Texture2D(gd, 1, 1, true, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        public Tile(Rectangle rec, GraphicsDevice gd)
        {
            this.rec = rec;
            this.pixel = new Texture2D(gd, 1, 1, true, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        public void Draw (SpriteBatch sp)
        {
            sp.Draw(pixel, rec, color);
        }
    }
}
