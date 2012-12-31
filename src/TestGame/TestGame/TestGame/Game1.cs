using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using xTile.Display;
using xTile.Dimensions;
using xTile.Tiles;
using xTile.Layers;
using xTile.Format;

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
        private XnaDisplayDevice m_xnaDisplayDevice;
        xTile.Dimensions.Rectangle viewport;

        protected bool enableVSync = true;
        protected bool DebugConsoleOn = true;

        //test
        Location loct;

        Character _player;
        Physics physics;
        Sprite _character;
        ContentManager contentManager;

        xTile.Dimensions.Rectangle camera;

        readonly int charScreenX = 139;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            contentManager = Content;
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            
        }

        void FixTileLocationsBug(Map map)
        {
            foreach (TileSheet ts in map.TileSheets)
            {
                
                //int p = ts.ImageSource.LastIndexOf('\\') + 1;
                //ts.ImageSource = ts.ImageSource.Substring(p, ts.ImageSource.Length - p);
                //ts.ImageSource = ts.ImageSource.Substring(0, ts.ImageSource.Length - 4);
                ts.ImageSource = ts.ImageSource.Replace(".png", "");
                Console.WriteLine(ts.ImageSource);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            m_xnaDisplayDevice = new XnaDisplayDevice(this.Content, graphics.GraphicsDevice);
            map.LoadTileSheets(m_xnaDisplayDevice);
            viewport = new xTile.Dimensions.Rectangle(new Size(800,600));

            viewport.X = 0;
            viewport.Y = 280;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new xTile.Dimensions.Rectangle(new Size(charScreenX, 309));

            //System.IO.Stream stream = TitleContainer.OpenStream("Content\\Maps\\Map02.tbin");

            //map = FormatManager.Instance.BinaryFormat.Load(stream);
            //FixTileLocationsBug(map);
            map = Content.Load<Map>("Maps\\Map02");
            

            //LoadMap("Map01");
            /*map = Content.Load<Map>("Maps\\Map01");
            map.Layers[3].AfterDraw += OnBeforeLayerDraw;
            map.Layers[4].Visible = false;*/

            _character = new Sprite(contentManager.Load<Texture2D>("golfball"));
            _character.X = charScreenX;
            _character.Y = 128;

            physics = new Physics();
            physics.addCharacterSprite(_character);
            TileArray groundTiles = map.GetLayer("HitGround").Tiles;
            TileArray waterTiles = map.GetLayer("HitWater").Tiles;

            for (int y = 0; y < 48; y++)
            {
                for (int x = 0; x < 800; x++)
                {

                    Tile tgroup = groundTiles[x, y];
                    Tile twater = waterTiles[x, y];

                    if (tgroup != null)
                    {
                        physics.addRect(x * 16, y * 16, 16, 16, Physics.TYPE_GROUND);
                    }
                        

                    if (twater != null)
                    {
                        physics.addRect(x * 16, y * 16, 16, 16, Physics.TYPE_WATER);
                    }

                }
            }

            physics.flickCharacter(139, 309);
            
            //_player = new Character(Content.Load<Texture2D>("Sprite_Sheet"), 1, 32, 48, this.map);

            //_player.Position = new Point(139, 309);//new Vector2(138, 308);

            
            base.LoadContent();
           
        }

        protected override void UnloadContent()
        {
            map.DisposeTileSheets(m_xnaDisplayDevice);
            map = null;
            
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
            else if (keyb.IsKeyDown(Keys.S) && !(viewport.Y >= 280))
            {
                viewport.Y++;
                Console.WriteLine("viewport.Y: {0}", viewport.Y);
            }
            else if (keyb.IsKeyDown(Keys.W) && !(viewport.Y <= 0))
            {
                viewport.Y--;
                Console.WriteLine("viewport.Y: {0}", viewport.Y);
            }

            if (keyb.IsKeyDown(Keys.B))
            {
                //LoadMap("Map01");
            }

            if (map != null)
                map.Update(gameTime.ElapsedGameTime.Milliseconds);

            physics.update(gameTime.ElapsedGameTime.Milliseconds);

            camera.X = (int)_character.X - charScreenX;

            if (_player != null)
                _player.HandleSpriteMovement(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (map != null)
                map.Draw(m_xnaDisplayDevice, viewport, loct, false);


            spriteBatch.Begin();
            spriteBatch.Draw(_character.Texture2D, new Vector2(charScreenX, _character.Y), Color.White);
            spriteBatch.End();

            /*if (_player != null)
            {
                spriteBatch.Begin();
                Vector2 correctedPosition = new Vector2(_player.Position.X + loct.X, _player.Position.Y + loct.Y);
                spriteBatch.Draw(_player.Texture, correctedPosition, _player.SourceRect, Color.White, 0f, _player.Origin, 1.0f, SpriteEffects.None, 0);
                spriteBatch.End();
            }
            */
            base.Draw(gameTime);
        }

        private void OnBeforeLayerDraw(object sender, LayerEventArgs layerEventArgs)
        {
            SpriteBatch spriteBatch = m_xnaDisplayDevice.SpriteBatchAlpha;

            Vector2 vet = new Vector2(_player.BoundingBox.X,_player.BoundingBox.Y);
            spriteBatch.Draw(_player.Texture, vet, _player.SourceRect, Color.White, 0f, _player.Origin, 1.0f, SpriteEffects.None, 0.0f);

        }

        public void LoadMap(string _map)
        {
            map = Content.Load<Map>("Maps\\"+_map);
        }
    }
}
