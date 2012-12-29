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
using System.Text;
using IceCream.Drawing;

namespace IceCream.Drawing
{
    public class DrawingLayer
    {
        #region Fields

        // array of "sub" layers
        private DrawingLayerComponent[] _components;
#if !XNATOUCH
        private List<PostProcessRequest> _postProcessRequestCollection;
#endif
        #endregion

        #region Properties

        public Vector3 Parallax { get; set; }

        public bool Visible { get; set; }

        public byte Opacity { get; set; }

        #endregion

        #region Constructor

        public DrawingLayer()
        {
            this.Parallax = Vector3.One;
            this.Visible = true;
            this.Opacity = 255;
            _components = new DrawingLayerComponent[6];
            _components[0] = new DrawingLayerComponent(DrawingLayerType.Additive);
            _components[1] = new DrawingLayerComponent(DrawingLayerType.Subtractive);            
            _components[2] = new DrawingLayerComponent(DrawingLayerType.Alpha);
            _components[3] = new DrawingLayerComponent(DrawingLayerType.Additive);
            _components[4] = new DrawingLayerComponent(DrawingLayerType.Subtractive);            
            _components[5] = new DrawingLayerComponent(DrawingLayerType.Additive);
            _components[5].UseRefraction = true;            
			#if !XNATOUCH
            _postProcessRequestCollection = new List<PostProcessRequest>();
#endif
        }

        #endregion

        #region Methods
        
        public void EndDrawing()
        {
            for (int i = 0; i < _components.Length; i++)
            {
                _components[i].EndDrawing();
            }
        }

        /// <summary>
        /// Render each sub layer (blending) of that layer
        /// </summary>
        /// <param name="graphicsDevice">The GraphicsDevice to render on</param>
        /// <param name="transformMatrix">The camera transform matrix </param>
        public void RenderLayer(GraphicsDevice graphicsDevice, Matrix transformMatrix)
        {
            bool requireRenderTarget = false;
            Texture2D bufferTexture = null;
            if (this.Opacity < 255)
            {
                requireRenderTarget = true;
            }
			#if !REACH
            if (_postProcessRequestCollection.Count > 0)
            {
                for (int i = 0; i < _postProcessRequestCollection.Count; i++)
                {
                    if (_postProcessRequestCollection[i].OwnLayerOnly == true)
                    {
                        requireRenderTarget = true;
                        break;
                    }
                }
            }
            if (requireRenderTarget == true)
            {
                // End the sprite batch if needed
                if (DrawingManager.DrawingLayerTypeInUse != null)
                {
                    DrawingManager.SpriteBatch.End();
                    DrawingManager.DrawingLayerTypeInUse = null;
                }
                DrawingManager.RenderTargetManager.SwitchTo(RenderTargetInstance.OwnLayerRendering);
                DrawingManager.CurrentRenderTarget = RenderTargetInstance.OwnLayerRendering;
                bufferTexture = DrawingManager.RenderTargetManager.GetRenderTarget2D(RenderTargetInstance.SceneRendering);
                graphicsDevice.Clear(new Color(0, 0, 0, 0));
            }
			#endif
            for (int i = 0; i < _components.Length; i++)
            {
                _components[i].RenderLayerComponent(graphicsDevice, transformMatrix);
            }
			#if !REACH
            // if there is postprocessing to be done on that layer
            if (_postProcessRequestCollection.Count > 0)
            {
                // End the sprite batch if needed
                if (DrawingManager.DrawingLayerTypeInUse != null)
                {
                    DrawingManager.SpriteBatch.End();    
                    DrawingManager.DrawingLayerTypeInUse = null;
                }
                for (int i = 0; i < _postProcessRequestCollection.Count; i++)
                {
                    _postProcessRequestCollection[i].IceEffect.
                        SetParameters(_postProcessRequestCollection[i].IceEffectParameters);
                    _postProcessRequestCollection[i].IceEffect.Draw(DrawingManager.SpriteBatch);         
                }
                // clear all the requests for the next call
                _postProcessRequestCollection.Clear();
            }
            if (requireRenderTarget == true)
            {               
                DrawingManager.DrawRenderTargetContent(RenderTargetInstance.OwnLayerRendering,
                    RenderTargetInstance.EffectA, bufferTexture, this.Opacity);
                //DrawingManager.RenderTargetManager.SaveTexture(RenderTargetInstance.OwnLayerRendering, "E:/own.png");
                DrawingManager.DrawRenderTargetContent(RenderTargetInstance.EffectA, 
                    RenderTargetInstance.SceneRendering, null, 255);
                //DrawingManager.RenderTargetManager.SaveTexture(RenderTargetInstance.EffectA, "E:/ownEffectA.png");
                DrawingManager.CurrentRenderTarget = RenderTargetInstance.SceneRendering;
            }
            #endif
        }

        /// <summary>
        /// Draw a DrawRequest on the alpha sublayer of that layer
        /// </summary>
        public void Draw(DrawRequest drawRequest)
        {
            _components[2].Draw(drawRequest);
        }

        /// <summary>
        /// Draw a DrawRequest on the specified blending sub layer
        /// </summary>
        /// <param name="drawRequest"></param>
        /// <param name="blendingType"></param>
        public void Draw(DrawRequest drawRequest, DrawingBlendingType blendingType)
        {
            // Forward the request to the correct layer
            _components[(int)blendingType].Draw(drawRequest);
        }

		#if !XNATOUCH
        /// <summary>
        /// Apply the specified Post Process effect on the layer after rendering it
        /// </summary>
        public void ApplyPostProcess(PostProcessRequest postProcessRequest)
        {
            _postProcessRequestCollection.Add(postProcessRequest);
        }
#endif
		
        #endregion
    }
}
