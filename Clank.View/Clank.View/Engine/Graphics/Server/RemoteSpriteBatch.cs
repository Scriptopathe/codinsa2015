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

        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencil, RasterizerState rasterizer, RemoteEffect effect)
        {
            Server.SendCommand(new CommandSpriteBatchBegin(
                this, sortMode, blendState, samplerState, depthStencil, rasterizer, effect));
        }
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencil, RasterizerState rasterizer)
        {
            Server.SendCommand(new CommandSpriteBatchBegin(
                this, sortMode, blendState, samplerState, depthStencil, rasterizer, null));
        }
        public void Begin(SpriteSortMode sortMode, BlendState blendState)
        {
            Server.SendCommand(new CommandSpriteBatchBegin(
                this, sortMode, blendState, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null));
        }
        public void Begin()
        {
            Server.SendCommand(new CommandSpriteBatchBegin(
                this, SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null));
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
        public void Draw(RemoteTexture texture, Vector2 position, Color color)
        {
            Server.SendCommand(new CommandSpriteBatchDraw(
                this,
                texture,
                null,
                null,
                color,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                1.0f
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
