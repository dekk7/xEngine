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


namespace TestGame
{
    public class Sprite
    {
        float x, y;
        Texture2D texture;
        bool drown;

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public float X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public bool Drown
        {
            get
            {
                return drown;
            }

            set
            {
                drown = value;
            }
        }

        public float Y
        {
            get { return y; }

            set
            {
                y = value;
            }
        }

        public Texture2D Texture2D
        {
            get { return texture; }
            set { texture = value; }
        }
    }
}
