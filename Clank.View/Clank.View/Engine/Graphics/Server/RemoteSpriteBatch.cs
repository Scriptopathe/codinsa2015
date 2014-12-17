using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente un SpriteBatch distant.
    /// </summary>
    public class RemoteSpriteBatch : RemoteGraphicsObject
    {
        /// <summary>
        /// Création d'un sprite batch distant.
        /// </summary>
        public RemoteSpriteBatch(GraphicsServer server) : base(server)
        {
            
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, RemoteEffect effect)
        {
            Server.SendCommand(new CommandSpriteBatchBegin(
                this, sortMode, blendState, samplerState, effect));
        }

        public void Draw(RemoteTexture texture, Rectangle destRect,
            Rectangle? srcRect, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            Server.SendCommand(new CommandSpriteBatchDraw(
                this,
                texture,
                destRect,
                srcRect,
                color,
                rotation,
                origin,
                effects,
                layerDepth
                ));
        }


        public void DrawString(string str, RemoteSpriteFont font, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, float layerDepth)
        {
            Server.SendCommand(new CommandSpriteBatchDrawString(
                this,
                font,
                str,
                position,
                color,
                rotation,
                origin,
                scale,
                layerDepth));
        }

        public void End()
        {
            Server.SendCommand(new CommandSpriteBatchEnd(this));
        }
    }
}
