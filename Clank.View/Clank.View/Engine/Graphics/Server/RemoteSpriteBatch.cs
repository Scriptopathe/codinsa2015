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
        /// Alias pour la propriété serveur. (utilisé pour conserver la syntaxe originale
        /// de SpriteBatch).
        /// </summary>
        public GraphicsServer GraphicsDevice { get { return Server; } }
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
                new Rectangle((int)position.X, (int)position.Y, -1, -1),
                null,
                color,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                1.0f
                ));
        }
        public void Draw(RemoteTexture texture, Rectangle dstRect, Color color)
        {
            Server.SendCommand(new CommandSpriteBatchDraw(
                this,
                texture,
                dstRect,
                null,
                color,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                1.0f
                ));
        }
        public void DrawString(RemoteSpriteFont font, string str, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, float layerDepth)
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
                SpriteEffects.None,
                layerDepth));
        }
        public void DrawString(RemoteSpriteFont font, string str, Vector2 position, Color color, float rotation, Vector2 origin, float scale, float layerDepth)
        {
            Server.SendCommand(new CommandSpriteBatchDrawString(
                this,
                font,
                str,
                position,
                color,
                rotation,
                origin,
                new Vector2(scale, scale),
                SpriteEffects.None,
                layerDepth));
        }
        public void DrawString(RemoteSpriteFont font, string str, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            Server.SendCommand(new CommandSpriteBatchDrawString(
                this,
                font,
                str,
                position,
                color,
                rotation,
                origin,
                new Vector2(scale, scale),
                spriteEffects,
                layerDepth));
        }

        public void DrawString(RemoteSpriteFont font, string str, Vector2 position, Color color)
        {
            Server.SendCommand(new CommandSpriteBatchDrawString(
                this,
                font,
                str,
                position,
                color,
                0.0f,
                Vector2.Zero,
                new Vector2(1.0f, 1.0f),
                SpriteEffects.None,
                1.0f));
        }
        public void End()
        {
            Server.SendCommand(new CommandSpriteBatchEnd(this));
        }
    }
}
