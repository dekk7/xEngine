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
        //Vector2 position;
        Point position;
        Vector2 origin;
        KeyboardState currentKBState;
        KeyboardState previousKBState;
        KeyboardState keyState;

        Map _map;
        Layer collision;

        private Vector2[] m_vecLeafPositions;
        private Vector2[] m_vecLeafVelocities;


        public Vector2[] vecLeafPositions
        {
            get { return m_vecLeafPositions; }
        }

        public Vector2[] vecLeafVelocities
        {
            get { return m_vecLeafVelocities; }
        }

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
        public Microsoft.Xna.Framework.Rectangle BoundingBox
        {
            get { return new Microsoft.Xna.Framework.Rectangle((int)Position.X, (int)Position.Y, spriteWidth, spriteHeight); }
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
            if (!(_map == null))
            {
                Console.WriteLine("_map != null");
            }
            this.collision = map.GetLayer("Colision");
            //this.collision = _map.Layers[3];
            m_vecLeafPositions = new Vector2[20];
            m_vecLeafVelocities = new Vector2[m_vecLeafPositions.Length];
        }

        public void HandleSpriteMovement(GameTime gameTime)
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();
            keyState = currentKBState;

            sourceRect = new Microsoft.Xna.Framework.Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);

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

            Point newPos = position;

            if (keyState.IsKeyDown(Keys.W))
            {
                newPos.Y -= 2;
            if (!calculateCollision(newPos))
                position.Y = newPos.Y;
            else
                newPos.Y = position.Y;
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                newPos.Y += 2;
                if (!calculateCollision(newPos))
                    position.Y = newPos.Y;
                else
                    newPos.Y = position.Y;
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                newPos.X -= 2;
                if (!calculateCollision(newPos))
                    position.X = newPos.X;
                else
                    newPos.X = position.X;
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                newPos.X += 2;
                if (!calculateCollision(newPos))
                    position.X = newPos.X;
                else
                    newPos.X = position.X;
            }

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
            try
            {
                Tile tile;
                Location tileLocation;

                tileLocation = new Location((newPos.X - BoundingBox.Width / 2) / 48,
                (newPos.Y - BoundingBox.Height / 2) / 48);
                tile = collision.Tiles[tileLocation];
                if (tile.TileIndex >= 1)
                {
                    Console.WriteLine("Collicion!!");
                    return true;
                }

                tileLocation = new Location((newPos.X + BoundingBox.Width / 2) / 48,
                    (newPos.Y - BoundingBox.Height / 2) / 48);
                tile = collision.Tiles[tileLocation];
                if (tile.TileIndex >= 1)
                {
                    Console.WriteLine("Collicion!!");
                    return true;
                }

                tileLocation = new Location((newPos.X + BoundingBox.Width / 2) / 48,
                    (newPos.Y + BoundingBox.Height / 2) / 48);
                tile = collision.Tiles[tileLocation];
                if (tile.TileIndex >= 1)
                {
                    Console.WriteLine("Collicion!!");
                    return true;
                }

                tileLocation = new Location((newPos.X - BoundingBox.Width / 2) / 48,
                    (newPos.Y + BoundingBox.Height / 2) / 48);
                tile = collision.Tiles[tileLocation];

                if (tile.TileIndex >= 1)
                {
                    Console.WriteLine("Collicion!!");
                    return true;
                }

                return false;
            }
            catch (Exception epx) {
                
                //Console.WriteLine("Error..! {0}",epx.Message);
            }

            return false;
        }


    }
}
