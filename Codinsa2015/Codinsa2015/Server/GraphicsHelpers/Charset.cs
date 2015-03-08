using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.GraphicsHelpers
{
    /// <summary>
    /// Représente une ressource graphique permettant d'obtenir des frames d'animation de personnage
    /// contenues dans une texture.
    ///
    /// Format standard : 4 lignes, 8 colonnes.
    /// Ligne 1         : Animations de marche.
    /// </summary>
    public class Charset
    {
        const int MaxWalkFrames = 8;

        #region Properties
        /// <summary>
        /// Obtient ou définit la texture associée à ce charset.
        /// </summary>
        public Texture2D Texture
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit la taille en pixel d'une frame du charset.
        /// </summary>
        public Point FrameSize
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit le nombre de frames de marche pour ce charset.
        /// </summary>
        public int WalkFrames
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit la durée d'une frame de ce charset (en frames de jeu).
        /// </summary>
        public int FrameDuration
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Crée une nouvelle instance de charset avec les paramètres par défaut.
        /// </summary>
        public Charset()
        {
            WalkFrames = MaxWalkFrames;
            FrameDuration = 8;
        }

        /// <summary>
        /// Crée une nouvelle instance de charset avec la texture et la frame size donnée.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="frameSize"></param>
        public Charset(Texture2D texture, Point frameSize)
        {
            Texture = texture;
            FrameSize = frameSize;
            WalkFrames = MaxWalkFrames;
            FrameDuration = 8;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Retourne le rectangle source de la texture correspondant à la frame de marche d'id donné.
        /// </summary>
        /// <param name="walkStep"></param>
        /// <returns></returns>
        public Rectangle GetWalkSrcRect(int walkStep)
        {
            return new Rectangle(FrameSize.X * walkStep, 0, FrameSize.X, FrameSize.Y);
        }

        #endregion
    }
}