using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine
{
    /// <summary>
    /// Représente une trajectoire.
    /// Contient également des moyens de stockage de la position actuelle par rapport à la trajectoire.
    /// </summary>
    public class Trajectory
    {
        /// <summary>
        /// Liste des points de passage de la trajectoire.
        /// Deux points consécutifs doivent être séparés d'une distance de 1 !
        /// </summary>
        private List<Vector2> m_trajectoryUnits;

        /// <summary>
        /// Indique la distance parcourue depuis le début de la trajectoire en pourcentage de case.
        /// 0 : case de départ
        /// [taille de TrajectoryUnit] : case d'arrivée.
        /// </summary>
        private int m_currentPosition;

        #region Properties
        /// <summary>
        /// Obtient ou définit la liste des points de passage de la trajectoire.
        /// Deux points consécutifs doivent être séparés d'une distance de 1 !
        /// </summary>
        public List<Vector2> TrajectoryUnits
        {
            get
            {
                return m_trajectoryUnits;
            }
            set
            {
                // Vérifie que la distance entre 2 points consécutifs est de 1.
                bool ok = true;
                if (value.Count <= 1)
                    m_trajectoryUnits = value;
                else
                {
                    int i = 0;
                    while ((i < value.Count - 1) && ok)
                    {
                        ok = ((Math.Abs(value[i].X - value[i + 1].X) + Math.Abs(value[i].Y - value[i + 1].Y)) - 1) <= 0.0001f;
                        i++;
                    }
                    if (ok)
                        m_trajectoryUnits = value;
                    else
                        throw new Exception("La distance entre les points de la trajectoire n'est pas toujours inférieure ou égale à 1");
                }
            }
        }
        #endregion

        #region Public
        /// <summary>
        /// Crée une nouvelle instance de Trajectory avec une trajectoire vide.
        /// </summary>
        public Trajectory()
        {
            TrajectoryUnits = new List<Vector2>();
            m_currentPosition = 0;
        }

        /// <summary>
        /// Retourne la position finale de la trajectoire.
        /// </summary>
        /// <returns></returns>
        public Vector2 LastPosition()
        {
            return TrajectoryUnits.Last();
        }
        /// <summary>
        /// Crée une nouvelle instance de Trajectory avec la trajectoire passée en paramètre.
        /// /!\ Chaque point de la trajectoire doit être séparé d'une seule unité par rapport à son voisin.
        /// </summary>
        /// <param name="points"></param>
        public Trajectory(List<Vector2> points)
        {
            TrajectoryUnits = points;
        }
        /// <summary>
        /// Choisis et retourne la prochaine étape de la trajectoire.
        /// </summary>
        public Vector2 UpdateStep(Vector2 position, float speed, GameTime time)
        {
            // Si on est pas à la fin   
            if(m_currentPosition != TrajectoryUnits.Count - 1)
            {
                // On choisit la prochaine étape 
                float dst = Vector2.DistanceSquared(position, CurrentStep);
                if (dst <= speed * (float)time.ElapsedGameTime.TotalSeconds)
                    m_currentPosition++;
            }


            return CurrentStep;
        }
        /// <summary>
        /// Retourne vrai si la position du dernier élément de la trajectoire est atteint.
        /// </summary>
        /// <returns></returns>
        public bool IsEnded(Vector2 position, float speed, GameTime time)
        {
            return Vector2.DistanceSquared(position, TrajectoryUnits.Last()) <= speed * (float)time.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Obtient l'étape actuelle de la trajectoire.
        /// </summary>
        public Vector2 CurrentStep
        {
            get
            {
                return TrajectoryUnits[m_currentPosition] + new Vector2(0.5f, 0.5f);
            }
        }
        #endregion


        #region Utils
        /// <summary>
        /// Interpolation linéaire de from à to, avec l'avancement a.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        float lerp(float from, float to, float a)
        {
            return from * (1 - a) + to * a;
        }
        /// <summary>
        /// Interpolation linéaire entre deux vecteurs.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        Vector2 lerp(Vector2 from, Vector2 to, float a)
        {
            return new Vector2(lerp(from.X, to.X, a), lerp(from.Y, to.Y, a));
        }
        #endregion
    }
}
