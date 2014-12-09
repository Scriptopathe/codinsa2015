using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine
{
    /// <summary>
    /// Indique l'état de la vision que chaque équipe peut avoir sur une certaine zone.
    /// 2 types de vision sont possibles :
    /// - la vision simple : ne révèle que les unités qui ne sont pas actuellement en stealth
    /// - la vision pure (true vision) : révèle aussi les unités en stealth.
    /// </summary>
    public enum VisionFlags
    {
        None              = 0x0000,
        // NE PAS MODIFIER LES VALEURS
        Team1Vision       = 0x0002,
        Team2Vision       = 0x0004,

        // NE PAS MODIFIER LES VALEURS
        Team1TrueVision   = 0x0020 | Team1Vision,
        Team2TrueVision   = 0x0040 | Team2Vision

    }

    /// <summary>
    /// Classe contenant la carte de vision, et permettant de calculer les endroits où il y a la vision.
    /// </summary>
    public class VisionMap
    {
        struct Node
        {
            public Point P;
            public float Distance;
            public Node(Point p, float distance)
            {
                P = p;
                Distance = distance;
            }
        }
        #region Variables
        const float Sqrt2 = 1.41f;
        
        Map m_map;
        VisionFlags[,] m_vision;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la map associée à cette vision map.
        /// </summary>
        public Map TheMap { get { return m_map; } set { m_map = value; m_vision = new VisionFlags[m_map.Size.X, m_map.Size.Y]; } }

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de VisionMap associée à la map donnée.
        /// </summary>
        /// <param name="map"></param>
        public VisionMap(Map map)
        {
            TheMap = map;
        }

        /// <summary>
        /// Obtient une variable indiquant si la team donnée possède la vision à l'endroit donné.
        /// </summary>
        public bool HasVision(EntityType team, Vector2 position)
        {
            team &= (EntityType.Team1 | EntityType.Team2);
            return (m_vision[(int)position.X, (int)position.Y] & (VisionFlags)team) != 0;
        }
        /// <summary>
        /// Obtient une variable indiquant si la team donnée possède pure la vision à l'endroit donné.
        /// </summary>
        public bool HasTrueVision(EntityType team, Vector2 position)
        {
            team &= (EntityType.Team1 | EntityType.Team2);
            return (m_vision[(int)position.X, (int)position.Y] & (VisionFlags)((int)team << 2)) != 0;
        }


        float __debug2 = 0;
        /// <summary>
        /// Rempli la carte à la position donnée et avec le rayon donnée, avec les informations
        /// contenues dans flags.
        /// </summary>
        public void FloodWith0(Vector2 position, float radius, VisionFlags flags)
        {

            Queue<Node> positions = new Queue<Node>();
            Dictionary<Point, float> minDist = new Dictionary<Point, float>();
            positions.Enqueue(new Node(new Point((int)position.X, (int)position.Y), 0));

            int w = m_vision.GetLength(0);
            int h = m_vision.GetLength(1);

            Node n = new Node(new Point(0, 0), 0);
            Point p = new Point();
            float distance;
            while(positions.Count != 0)
            {

                Node current = positions.Dequeue();

                // Mets à jour la distance minimale vers le point courant.
                if (minDist.ContainsKey(current.P))
                    minDist[current.P] = current.Distance;
                else
                    minDist.Add(current.P, current.Distance);

                if (current.P.X < 0 || current.P.Y < 0 || current.P.X >= w || current.P.Y >= h)
                    continue;

                m_vision[current.P.X, current.P.Y] |= flags;

                for(int x = -1; x <= 1; x ++)
                    for(int y = -1; y <= 1; y++)
                    {
                        if(x == 0 && y == 0)
                            continue;

                        // On calcule un nouveau noeud dont la distance par rapport à l'origine est :
                        // 1 sur les côtés
                        // Sqrt 2 sur les diagonales.
                        p.X = current.P.X + x;
                        p.Y = current.P.Y + y;
                        n.Distance = current.Distance + ((x == 0 || y == 0) ? 1 : Sqrt2);
                        bool process = true;

                        // Si le point a déjà été parcouru, on le parcours si la distance pour arriver à ce point
                        // est inférieure à la précédente enregistrée.
                        // if (minDist.TryGetValue(p, out distance))
                        if(minDist.ContainsKey(p))
                        {
                            distance = minDist[p];
                            if (n.Distance < distance)
                            {
                                minDist[p] = n.Distance;
                            }
                            else
                                process = false;
                        }

                        // On ajoute le noeud à la pile si sa distance est inférieure au rayon de la vision,
                        // et s'il n'a pas déjà été process.
                        if (process && n.Distance <= radius && m_map.GetPassabilityAt(p.X, p.Y))
                        {
                            n.P = p;
                            positions.Enqueue(n);
                        }
                    }
            }

        }
        
        /// <summary>
        /// Utilise un algorithme de raycasting pour appliquer les [flags] de visions données sur une zone
        /// de [radius] unités autour de la [position] donnée.
        /// </summary>
        public void RaycastVision(Vector2 position, float radius, VisionFlags flags)
        {
            Vector2 startPos = new Vector2((int)position.X, (int)position.Y);
            float radiusSqr = radius * radius;
            for(float x = -radius - 1; x <= radius + 1; x+=0.5f)
            {   
                float y1 = -radius;
                float y2 = radius;
                DrawRay(startPos, new Vector2(x, y1), flags, radius, radiusSqr);
                DrawRay(startPos, new Vector2(x, y2), flags, radius, radiusSqr);

                DrawRay(startPos, new Vector2(y1, x), flags, radius, radiusSqr);
                DrawRay(startPos, new Vector2(y2, x), flags, radius, radiusSqr);
            }
        }

        /// <summary>
        /// Dessine un rayon de vision, appliquant les [flags] passés en paramètres, 
        /// de la position de départ [startPos] vers la direction [dir] donnée.
        /// Le rayon s'arrête à la rencontre d'un obstacle ou après avoir traversé [radius] unités.
        /// </summary>
        public void DrawRay(Vector2 startPos, Vector2 dir, VisionFlags flags, float radius, float radiusSqr)
        {
            dir.Normalize();
            for(int i = 0; i < radius; i++)
            {
                Vector2 currentStep = startPos + dir * i;
                currentStep.X = (int)Math.Round(currentStep.X);
                currentStep.Y = (int)Math.Round(currentStep.Y);
                bool distanceOK = Vector2.DistanceSquared(startPos, currentStep) < radiusSqr;
                if (m_map.GetPassabilityAt(currentStep) && distanceOK)
                {
                    m_vision[(int)currentStep.X, (int)currentStep.Y] |= flags;
                }
                else if (dir.Y < -0.1f && distanceOK)
                {
                    // Effet graphique pour éclairer les murs.
                    m_vision[(int)currentStep.X, (int)currentStep.Y] |= flags;
                    break;
                }
                else
                    break;
            }
        }

        /// <summary>
        /// Mets à jour la carte de vision.
        /// </summary>
        public void Update(GameTime time)
        {
            for (int x = 0; x < m_vision.GetLength(0); x++)
                for (int y = 0; y < m_vision.GetLength(1); y++)
                    m_vision[x, y] = VisionFlags.None;

            

            foreach (EntityBase entity in m_map.Entities.Values)
            {
                int team = (int)(entity.Type & (EntityType.Team1 | EntityType.Team2));

                // Si l'entité a la vision pure, on l'ajoute au flags.
                if (entity.HasTrueVision)
                    team += team << 4;

                VisionFlags flag = (VisionFlags)team;

                if (entity.VisionRange > 0.5f)
                    RaycastVision(entity.Position, entity.VisionRange, flag);
                        
            }


        }
        #endregion
    }
}
