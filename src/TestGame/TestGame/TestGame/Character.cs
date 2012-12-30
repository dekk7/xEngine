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

using xTile;
using xTile.Dimensions;
using xTile.Display;
using xTile.Layers;
using xTile.Tiles;

namespace TestGame
{
    public class Character
    {
        Texture2D spriteTexture;
        float timer = 0f;
        float interval = 200f;
        int currentFrame = 0;
        int spriteWidth = 32;
        int spriteHeight = 48;
        int spriteSpeed = 2;
        Microsoft.Xna.Framework.Rectangle sourceRect;
        Microsoft.Xna.Framework.Rectangle collisionBox;
        //Vector2 position;
        Point position;
        Vector2 origin;
        KeyboardState currentKBState;
        KeyboardState previousKBState;

        Map _map;
        Layer collision;

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Texture2D Texture
        {
            get { return spriteTexture; }
            set { spriteTexture = value; }
        }

        public Microsoft.Xna.Framework.Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public Character(Texture2D texture,
            int currentFrame,
            int spriteWidth,
            int spriteHeight,
            Map map)
        {
            this.spriteTexture = texture;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this._map = map;
            this.collision = map.Layers[1];
        }

        /*public override void Initialize()
        {

            base.Initialize();
        }*/

        public void HandleSpriteMovement(GameTime gameTime)
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            sourceRect = new Microsoft.Xna.Framework.Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            //collisionBox = sourceRect;

            if (currentKBState.GetPressedKeys().Length == 0)
            {
                if (currentFrame > 0 && currentFrame < 4)
                    currentFrame = 0;
                if (currentFrame > 4 && currentFrame < 8)
                    currentFrame = 4;
                if (currentFrame > 8 && currentFrame < 12)
                    currentFrame = 8;
                if (currentFrame > 12 && currentFrame < 16)
                    currentFrame = 12;
            }

            



            if (currentKBState.IsKeyDown(Keys.Space))
            {
                spriteSpeed = 3;
                interval = 100;
            }
            else
            {
                spriteSpeed = 2;
                interval = 200;
            }

            if (currentKBState.IsKeyDown(Keys.Right) == true)
            {
                AnimateRight(gameTime);
                if (position.X < 780)
                    position.X += spriteSpeed;
            }

            if (currentKBState.IsKeyDown(Keys.Left) == true)
            {
                AnimateLeft(gameTime);
                if (position.X > 20)
                    position.X -= spriteSpeed;
            }

            /*if (currentKBState.IsKeyDown(Keys.Down) == true)
            {
                AnimateDown(gameTime);
                if (position.Y < 575)
                {
                    position.Y += spriteSpeed;
                }
            }

            if (currentKBState.IsKeyDown(Keys.Up) == true)
            {
                AnimateUp(gameTime);
                if (position.Y > 25)
                {
                    position.Y -= spriteSpeed;
                }
            }*/

            if (currentKBState.IsKeyDown(Keys.P))
            {
                Console.WriteLine("Position: X:{0} Y:{1}", position.X, position.Y);
            }

            origin = new Vector2(sourceRect.Width, sourceRect.Height / 2);
        }

        public void AnimateRight(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 9;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 11)
                {
                    currentFrame = 8;
                }
                timer = 0f;
            }
        }
        public void AnimateUp(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 13;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 15)
                {
                    currentFrame = 12;
                }
                timer = 0f;
            }
        }

        public void AnimateDown(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 1;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 3)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateLeft(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 5;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 7)
                {
                    currentFrame = 4;
                }
                timer = 0f;
            }
        }

        private bool calculateCollision(Point newPos)
        {
            Tile tile;
            Location tileLocation;

            Console.WriteLine("cx: " + collisionBox.X + " cy: " + collisionBox.Y + "\n");

            tileLocation = new Location((newPos.X - collisionBox.Width / 2) / 64,
                (newPos.Y - collisionBox.Height / 2) / 64);
            tile = collision.Tiles[tileLocation];
            if (tile.TileIndex == 0)
                return true;

            tileLocation = new Location((newPos.X + collisionBox.Width / 2) / 64,
                (newPos.Y - collisionBox.Height / 2) / 64);
            tile = collision.Tiles[tileLocation];
            if (tile.TileIndex == 0)
                return true;

            tileLocation = new Location((newPos.X + collisionBox.Width / 2) / 64,
                (newPos.Y + collisionBox.Height / 2) / 64);
            tile = collision.Tiles[tileLocation];
            if (tile.TileIndex == 0)
                return true;

            tileLocation = new Location((newPos.X - collisionBox.Width / 2) / 64,
                (newPos.Y + collisionBox.Height / 2) / 64);
            tile = collision.Tiles[tileLocation];
            if (tile.TileIndex == 0)
                return true;

            return false;
        }


    }
}
