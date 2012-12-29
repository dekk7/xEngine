#if XNATOUCH
using XnaTouch.Framework;
using XnaTouch.Framework.Audio;
using XnaTouch.Framework.Content;
using XnaTouch.Framework.GamerServices;
using XnaTouch.Framework.Graphics;
using XnaTouch.Framework.Input;
using XnaTouch.Framework.Media;
using XnaTouch.Framework.Net;
using XnaTouch.Framework.Storage;
#else
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceCream
{
    public class LinkFuse
    {
        public bool Initialized = false;
        public LinkPoint linkOwner;
        public LinkPoint linkChild;
        float _rotation = 0;
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = IceVectorUtil.WrapAngle(value); }
        }
    }
}
