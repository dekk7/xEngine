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

namespace TestGame
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //public static Game Instance;
        Map map;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;

        protected bool enableVSync = true;
        protected bool DebugConsoleOn = true;

        //test
        Location loct;

        Character _player;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            
        }
        protected override void Initialize()
        {
            base.Initialize();

            mapDisplayDevice = new XnaDisplayDevice(this.Content, graphics.GraphicsDevice);

            map.LoadTileSheets(mapDisplayDevice);

            viewport = new xTile.Dimensions.Rectangle(new Size(800,600));

            viewport.X = 0;
            viewport.Y = 280;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadMap("Map01");

            _player = new Character(Content.Load<Texture2D>("Sprite_Sheet"), 1, 32, 48, map);

            _player.Position = new Point(139, 309);//new Vector2(138, 308);

            base.LoadContent();
           
        }

        protected override void UnloadContent()
        {
            map.DisposeTileSheets(mapDisplayDevice);
            map = null;
            
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyb = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            /*if (keyb.IsKeyDown(Keys.A) && !(viewport.X <= 0))
            {
                viewport.X--;
                Console.WriteLine("viewport.X: {0}", viewport.X);
            }
            else if (keyb.IsKeyDown(Keys.D))
            {
                viewport.X++;
                Console.WriteLine("viewport.X: {0}", viewport.X);
            }
            else if (keyb.IsKeyDown(Keys.S) && !(viewport.Y >= 280))
            {
                viewport.Y++;
                Console.WriteLine("viewport.Y: {0}", viewport.Y);
            }
            else if (keyb.IsKeyDown(Keys.W) && !(viewport.Y <= 0))
            {
                viewport.Y--;
                Console.WriteLine("viewport.Y: {0}", viewport.Y);
            }*/

            if (keyb.IsKeyDown(Keys.B))
            {
                //LoadMap("Map01");
            }

            if (map != null)
                map.Update(gameTime.ElapsedGameTime.Milliseconds);


            if (_player != null)
                _player.HandleSpriteMovement(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (map != null)
                map.Draw(mapDisplayDevice, viewport, loct, false);


            if (_player != null)
            {
                spriteBatch.Begin();
                Vector2 correctedPosition = new Vector2(_player.Position.X + loct.X, _player.Position.Y + loct.Y);
                spriteBatch.Draw(_player.Texture, correctedPosition, _player.SourceRect, Color.White, 0f, _player.Origin, 1.0f, SpriteEffects.None, 0);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public void LoadMap(string _map)
        {
            map = Content.Load<Map>("Maps\\"+_map);
        }
    }
}
