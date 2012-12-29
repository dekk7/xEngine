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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

// xTile
using xTile;
using xTile.Dimensions;
using xTile.Display;

using IceCream;

namespace TestGame
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : IceCream.Game
    {
        //IceScene scene;
        public Game1()
        {
            map = Content.Load<Map>("Maps\\Map01");
        }
        protected override void Initialize()
        {
            base.Initialize();

            viewport = new xTile.Dimensions.Rectangle(0, 0,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);

            viewport.X = 0;
            viewport.Y = 166;
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            //scene = SceneManager.LoadScene("Content/TestScene.icescene");
            //LoadMap("Maps\\Map01");
        }

        protected override void UnloadContent()
        {

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyb = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (keyb.IsKeyDown(Keys.A) && !(viewport.X <= 0))
            {
                viewport.X--;
                Console.WriteLine("viewport.X: {0}", viewport.X);
            }
            else if (keyb.IsKeyDown(Keys.D))
            {
                viewport.X++;
                Console.WriteLine("viewport.X: {0}", viewport.X);
            }
            else if (keyb.IsKeyDown(Keys.S) && !(viewport.Y >= 166))
            {
                viewport.Y++;
                Console.WriteLine("viewport.Y: {0}", viewport.Y);
            }
            else if (keyb.IsKeyDown(Keys.W) && !(viewport.Y <= 0))
            {
                viewport.Y--;
                Console.WriteLine("viewport.Y: {0}", viewport.Y);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
