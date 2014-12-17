using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Graphics.Engine
{
    /// <summary>
    /// Représente un appel à SpriteBatch.Draw
    /// </summary>
    public class CommandSpriteBatchDraw : Command
    {
        public RemoteSpriteBatch Batch { get; set; }
        public RemoteTexture Texture { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects Effect { get; set; }
        public float LayerDepth { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de CommandSpriteBatchDraw.
        /// </summary>
        public CommandSpriteBatchDraw(
            RemoteSpriteBatch batch,
            RemoteTexture texture,
            Rectangle destRect,
            Rectangle? srcRect,
            Color color,
            float rotation,
            Vector2 origin,
            SpriteEffects effects,
            float layerDepth)
        {
            Batch = batch;
            Texture = texture;
            DestinationRectangle = destRect;
            SourceRectangle = srcRect;
            Color = color;
            Rotation = rotation;
            Origin = origin;
            Effect = effects;
            LayerDepth = layerDepth;
        }
    }
}
