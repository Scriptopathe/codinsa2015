using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics
{
    /// <summary>
    /// Contient des informations sur le z-layer à utiliser pour différents types d'objets graphiques.
    /// </summary>
    public static class Z
    {
        #region Steps
        public const float Front = 0.0f;
        public const float Back = 1.0f;
        public const float BackStep = (Front - Back) * 0.001f;
        public const float FrontStep = (Back - Front) * 0.001f;
        #endregion

        #region Entities
        public const float Entities     = 0.5f;
        public const float Background   = 0.0f;
        public const float Map          = 0.01f;
        public const float GUI          = 0.8f;
        #endregion

    }
}
