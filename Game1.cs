using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _1Pixor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        SpriteFont sf;
        public static int lvl { get; private set; }
        bool debug, help;
        Terrain[,,] t;
        const int tailleCase = 30;
        const int nbrCaseHeight = 12;
        const int nbrCaseWidth = 15;
        KeyboardState oldKs;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = tailleCase * nbrCaseHeight;
            graphics.PreferredBackBufferWidth = tailleCase * nbrCaseWidth;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player(new Rectangle(tailleCase * (int)(nbrCaseWidth / 2), tailleCase * (int)(nbrCaseHeight / 2), tailleCase, tailleCase), Color.Green * 0.5f, GraphicsDevice, (int)(nbrCaseWidth / 2), (int)(nbrCaseHeight / 2));
            lvl = 0;
            debug = false;
            help = true;
            
            t = new Terrain[2, nbrCaseWidth, nbrCaseHeight];
            for (int i = 0; i < nbrCaseWidth; i++)
            {
                for (int j = 0; j < nbrCaseHeight; j++)
                {
                    t[1, i, j] = new Terrain(new Rectangle(tailleCase * i, tailleCase * j, Window.ClientBounds.Width, Window.ClientBounds.Height), GraphicsDevice, Terrain.Type.Rock);
                    t[0, i, j] = new Terrain(new Rectangle(tailleCase * i, tailleCase * j, Window.ClientBounds.Width, Window.ClientBounds.Height), GraphicsDevice, Terrain.Type.Air);
                }
            }
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            sf = Content.Load<SpriteFont>("Font1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            oldKs = Key(oldKs);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            Terrain.terrainDraw(t, lvl, nbrCaseWidth, nbrCaseHeight, spriteBatch, GraphicsDevice);
            for (int i = 0; i < nbrCaseWidth; i++)
            {
                for (int j = 0; j < nbrCaseHeight; j++)
                {
                    t[lvl, i, j].Draw(spriteBatch);
                }
            }
            player.Draw(spriteBatch);
            spriteBatch.DrawString(sf, lvl.ToString(), new Vector2(5, 0), Color.Black);
            spriteBatch.DrawString(sf, player.nbrblock.ToString(), new Vector2(nbrCaseWidth * tailleCase - 25, 0), Color.Black);
            
            if (help)
                spriteBatch.DrawString(sf, "Fleche directionnelle: se deplacer\nPave numerique: creuser\nEspace: descendre/monter\nZ,D,X,Q: poser", new Vector2(5, nbrCaseHeight * tailleCase - 28 * 4), Color.Black);
            if (debug)
                spriteBatch.DrawString(sf, player.rec.ToString() + "\n" + t[lvl, player.widthCase, player.heightCase].getType, new Vector2(5, 25), Color.Black);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public KeyboardState Key(KeyboardState oldKs)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                Exit();
            if (ks.IsKeyDown(Keys.Up) && !oldKs.IsKeyDown(Keys.Up))
                player.move(0, tailleCase, nbrCaseWidth, nbrCaseHeight, t);
            if (ks.IsKeyDown(Keys.Right) && !oldKs.IsKeyDown(Keys.Right))
                player.move(1, tailleCase, nbrCaseWidth, nbrCaseHeight, t);
            if (ks.IsKeyDown(Keys.Down) && !oldKs.IsKeyDown(Keys.Down))
                player.move(2, tailleCase, nbrCaseWidth, nbrCaseHeight, t);
            if (ks.IsKeyDown(Keys.Left) && !oldKs.IsKeyDown(Keys.Left))
                player.move(3, tailleCase, nbrCaseWidth, nbrCaseHeight, t);
            if (ks.IsKeyDown(Keys.Space) && !oldKs.IsKeyDown(Keys.Space) && (t[Game1.lvl, player.widthCase, player.heightCase].getType == Terrain.Type.Hole_Bot || t[Game1.lvl, player.widthCase, player.heightCase].getType == Terrain.Type.Hole_Top))
            {
                lvl = player.changeLvl(t);
            }

            if (ks.IsKeyDown(Keys.NumPad5) && !oldKs.IsKeyDown(Keys.NumPad5))
                t = player.dig(0, player.widthCase, player.heightCase, tailleCase, nbrCaseWidth, nbrCaseHeight, t, GraphicsDevice);
            if (ks.IsKeyDown(Keys.NumPad8) && !oldKs.IsKeyDown(Keys.NumPad8))
                t = player.dig(1, player.widthCase, player.heightCase, tailleCase, nbrCaseWidth, nbrCaseHeight, t, GraphicsDevice);
            if (ks.IsKeyDown(Keys.NumPad6) && !oldKs.IsKeyDown(Keys.NumPad6))
                t = player.dig(2, player.widthCase, player.heightCase, tailleCase, nbrCaseWidth, nbrCaseHeight, t, GraphicsDevice);
            if (ks.IsKeyDown(Keys.NumPad2) && !oldKs.IsKeyDown(Keys.NumPad2))
                t = player.dig(3, player.widthCase, player.heightCase, tailleCase, nbrCaseWidth, nbrCaseHeight, t, GraphicsDevice);
            if (ks.IsKeyDown(Keys.NumPad4) && !oldKs.IsKeyDown(Keys.NumPad4))
                t = player.dig(4, player.widthCase, player.heightCase, tailleCase, nbrCaseWidth, nbrCaseHeight, t, GraphicsDevice);
            if (player.nbrblock > 0)
            {
                if (ks.IsKeyDown(Keys.Z) && !oldKs.IsKeyDown(Keys.Z))
                    t = player.put(1, player.widthCase, player.heightCase, tailleCase, nbrCaseWidth, nbrCaseHeight, t, GraphicsDevice);
                if (ks.IsKeyDown(Keys.D) && !oldKs.IsKeyDown(Keys.D))
                    t = player.put(2, player.widthCase, player.heightCase, tailleCase, nbrCaseWidth, nbrCaseHeight, t, GraphicsDevice);
                if (ks.IsKeyDown(Keys.X) && !oldKs.IsKeyDown(Keys.X))
                    t = player.put(3, player.widthCase, player.heightCase, tailleCase, nbrCaseWidth, nbrCaseHeight, t, GraphicsDevice);
                if (ks.IsKeyDown(Keys.Q) && !oldKs.IsKeyDown(Keys.Q))
                    t = player.put(4, player.widthCase, player.heightCase, tailleCase, nbrCaseWidth, nbrCaseHeight, t, GraphicsDevice);
            }
            if (ks.IsKeyDown(Keys.P) && !oldKs.IsKeyDown(Keys.P))
            {
                if (debug)
                    debug = false;
                else
                    debug = true;
            }
            if (ks.IsKeyDown(Keys.H) && !oldKs.IsKeyDown(Keys.H))
            {
                if (help)
                    help = false;
                else
                    help = true;
            }

            return ks;
        }
    }
}
