using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.Gui
{
    /// <summary>
    /// Contient des routines pour le dessin d'éléments de la GUI.
    /// </summary>
    public static class Drawing
    {
        const int SideMarginPx = 4;

        /// <summary>
        /// Dessine un cadre rectangulaire à partir d'une texture et d'un rectangle de destination.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="dstRect"></param>
        /// <param name="color"></param>
        /// <param name="layerDepth"></param>
        public static void DrawRectBox(RemoteSpriteBatch batch, RemoteTexture2D texture, Rectangle dstRect, Color color, float layerDepth=0.1f)
        {
            if (texture == null || batch == null)
                throw new ArgumentNullException();

            // Top left
            Rectangle src = new Rectangle(0, 0, SideMarginPx, SideMarginPx);
            Rectangle dst = new Rectangle(dstRect.X, dstRect.Y, SideMarginPx, SideMarginPx);
            batch.Draw(texture, dst, src, color, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);

            // Top
            src = new Rectangle(SideMarginPx, 0, texture.Width - SideMarginPx*2, SideMarginPx);
            dst = new Rectangle(dstRect.X + SideMarginPx, dstRect.Y, dstRect.Width - SideMarginPx*2, SideMarginPx);
            batch.Draw(texture, dst, src, color, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);

            // Top Right
            src = new Rectangle(texture.Width - SideMarginPx, 0, SideMarginPx, SideMarginPx);
            dst = new Rectangle(dstRect.X + dstRect.Width - SideMarginPx, dstRect.Y, SideMarginPx, SideMarginPx);
            batch.Draw(texture, dst, src, color, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);

            // Center
            src = new Rectangle(SideMarginPx, SideMarginPx, texture.Width - SideMarginPx * 2, texture.Height - SideMarginPx * 2);
            dst = new Rectangle(dstRect.X + SideMarginPx, dstRect.Y + SideMarginPx, dstRect.Width - SideMarginPx*2, dstRect.Height-SideMarginPx*2);
            batch.Draw(texture, dst, src, color, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);

            // Bottom left
            int yBaseSrc = texture.Height - SideMarginPx;
            int yBaseDst = dstRect.Bottom - SideMarginPx;
            src = new Rectangle(0, yBaseSrc, SideMarginPx, SideMarginPx);
            dst = new Rectangle(dstRect.X, yBaseDst, SideMarginPx, SideMarginPx);
            batch.Draw(texture, dst, src, color, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);

            // Bottom
            src = new Rectangle(SideMarginPx, yBaseSrc, texture.Width - SideMarginPx * 2, SideMarginPx);
            dst = new Rectangle(dstRect.X + SideMarginPx, yBaseDst, dstRect.Width - SideMarginPx * 2, SideMarginPx);
            batch.Draw(texture, dst, src, color, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);

            // Bottom Right
            src = new Rectangle(texture.Width - SideMarginPx, yBaseSrc, SideMarginPx, SideMarginPx);
            dst = new Rectangle(dstRect.X + dstRect.Width - SideMarginPx, yBaseDst, SideMarginPx, SideMarginPx);
            batch.Draw(texture, dst, src, color, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);

            // Left
            src = new Rectangle(0, SideMarginPx, SideMarginPx, texture.Height - SideMarginPx * 2);
            dst = new Rectangle(dstRect.X, dstRect.Y + SideMarginPx, SideMarginPx, dstRect.Height - SideMarginPx * 2);
            batch.Draw(texture, dst, src, color, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);

            // Right
            src = new Rectangle(texture.Width - SideMarginPx, SideMarginPx, SideMarginPx, texture.Height - SideMarginPx * 2);
            dst = new Rectangle(dstRect.Right - SideMarginPx, dstRect.Y + SideMarginPx, SideMarginPx, dstRect.Height - SideMarginPx * 2);
            batch.Draw(texture, dst, src, color, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);

        }

        /// <summary>
        /// Dessine une jauge à partir des arguments fournis.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="gaugeEmpty"></param>
        /// <param name="gaugeFull"></param>
        /// <param name="percent">Pourcent de remplissage de la jauge.</param>
        /// <param name="color"></param>
        /// <param name="layerDepth"></param>
        /// <param name="position"></param>
        /// <param name="width"></param>
        public static void DrawGauge(RemoteSpriteBatch batch, RemoteTexture2D gaugeEmpty, RemoteTexture2D gaugeFull, int value, int maxValue, Color color, 
            float layerDepth, Vector2 position, int width, int height)
        {
            float percent = value / (float)maxValue;
            // Dessin de la barre de vie.
            int bar_height = height;
            int bar_width = width;
            Rectangle dstRect = new Rectangle((int)position.X, (int)position.Y, bar_width, bar_height);
            batch.Draw(gaugeEmpty,
                dstRect,
                null,
                color,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                layerDepth);

            // Dessin de la partie pleine de la barre de vie.
            batch.Draw(gaugeFull,
                    new Rectangle((int)(position.X), (int)position.Y,
                                    (int)(bar_width * percent),
                                    bar_height),
                    new Rectangle(0, 0, (int)(gaugeFull.Width * percent), gaugeFull.Height),
                    color,
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    layerDepth - 0.0001f);

            // Dessin du texte :
            string str = value.ToString() + "/" + maxValue.ToString();
            Vector2 textSize = Ressources.NumbersFont.MeasureString(str);
            batch.DrawString(Ressources.NumbersFont, str,
                new Vector2(position.X + (bar_width - textSize.X)/2, position.Y),
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                layerDepth - 0.0002f
                );
        }
    }
}
