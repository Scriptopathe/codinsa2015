using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine
{
    /// <summary>
    /// Contient tous les algorithmes permettant la recherche de chemins.
    /// </summary>
    public class PathFinder
    {
        /// <summary>
        /// Exception levée lorsque le point de destination ne peut pas être atteint.
        /// </summary>
        public class AStarFailureException : Exception { }

        /// <summary>
        /// Contient tous les algorythmes permettant de gérer la structure de donnée "node".
        /// </summary>
        public class Node
        {
            #region Properties
            /// <summary>
            /// Obtient ou définit la position d'une node.
            /// </summary>
            public Point Position
            {
                get;
                set;
            }

            /// <summary>
            /// Obtient ou définit le Gscore d'une node vers une position donnée
            /// </summary>
            public float GScore
            {
                get;
                set;
            }

            /// <summary>
            /// Obtient ou définit le FScore d'une node vers une posistion précise.
            /// </summary>
            public float FScore
            {
                get;
                set;
            }

            /// <summary>
            /// Créé une nouvelle instance de node.
            /// </summary>
            public Node(Point position, float gScore, float fScore)
            {
                Position = position;
                GScore = gScore;
                FScore = fScore;
            }

            /// <summary>
            /// Créé une nouvelle instance de node.
            /// </summary>
            public Node()
            {

            }

            public override string ToString()
            {
                return "(X: " + Position.X.ToString() + ", Y: " + Position.Y.ToString() + ")";
            }
            #endregion
        }


        /// <summary>
        /// obtient et retournes les cases adjacentes à une case en prenant en compte si elles sont traversables ou non
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static List<Point> GetNeighbours(Point position)
        {
            List<Point> voisins = new List<Point>();
            Point mapSize = new Point(Mobattack.GetMap().Passability.GetLength(0), Mobattack.GetMap().Passability.GetLength(1));

            Vector2 p = new Vector2(position.X + 1, position.Y);
            if (position.X < mapSize.X - 1 && Mobattack.GetMap().GetPassabilityAt(p))
                voisins.Add(new Point((int)p.X, (int)p.Y));

            p = new Vector2(position.X, position.Y + 1);
            if (position.Y < mapSize.Y - 1 && Mobattack.GetMap().GetPassabilityAt(p))
                voisins.Add(new Point((int)p.X, (int)p.Y));

            p = new Vector2(position.X - 1, position.Y);
            if (position.X >= 1 && Mobattack.GetMap().GetPassabilityAt(p))
                voisins.Add(new Point((int)p.X, (int)p.Y));

            p = new Vector2(position.X, position.Y - 1);
            if (position.Y >= 1 && Mobattack.GetMap().GetPassabilityAt(p))
                voisins.Add(new Point((int)p.X, (int)p.Y));

            return voisins;
        }




        /// <summary>
        /// Retourne la distance en pixels entre deux cases, à vol d'oiseau.
        ///
        /// </summary>
        /// <param name="start">Noeud départ.</param>
        /// <param name="end">Noeud d'arrivée.</param>
        /// <returns></returns>
        public static float EstimateHeuristicCost(Point start, Point end)
        {
            return (float)(Math.Sqrt(Math.Pow(end.X - start.X, 2) +
                                     Math.Pow(end.Y - start.Y, 2)));
        }

        /// <summary>
        /// Retourne la position de la case adjacente à la position courante
        /// dont le coût heuristique vers est l'objectif est le plus faible.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public static Point BestNeighbour(Point current, Point goal)
        {
            float min = float.MaxValue;
            Point closest = new Point(0, 0);
            foreach (Point voisin in GetNeighbours(current))
            {
                if (EstimateHeuristicCost(voisin, goal) < min && Mobattack.GetMap().GetPassabilityAt(voisin.X, voisin.Y))
                {
                    min = EstimateHeuristicCost(voisin, goal);
                    closest = voisin;
                }
            }
            return closest;
        }

        /// <summary>
        /// Retourne la trajectoire la plus optimisée entre deux positions,
        /// à partir d'un graphe précis de cases, sous la forme d'une liste de position.
        /// </summary>
        /// <param name="start">Position initiale</param>
        /// <param name="goal">Position finale</param>z
        /// <param name="cameFrom">Liste des cases parrcourues par l'astar</param>
        /// <returns>Chemin optimisé entre start et goal</returns>

        public static List<Vector2> ReconstructPath(Node current, Dictionary<Node, Node> cameFrom)
        {
            List<Vector2> trajectory;


            if (cameFrom.ContainsKey(current))
            {
                trajectory = ReconstructPath(cameFrom[current], cameFrom);
            }
            else
            {
                trajectory = new List<Vector2>();
            }

            trajectory.Add(new Vector2(current.Position.X+0.5f, current.Position.Y+0.5f));
            return trajectory;
        }

        /// <summary>
        /// Retourne la node avec le fscore le plus faible dans une liste de node.
        /// </summary>
        /// <param name="openset"></param>
        /// <returns></returns>
        public static Node LowestFScore(List<Node> openset)
        {
            if (openset.Count != 0)
            {
                float min = float.MaxValue;
                Node bestNode = null;
                foreach (Node node in openset)
                {
                    if (node.FScore < min)
                    {
                        min = node.FScore;
                        bestNode = node;
                    }
                }
                return bestNode;
            }
            else
            {
                throw new Exception();
            }

        }

        /// <summary>
        /// Cache de la nodemap permettant d'éviter de grosses allocs de mémoires à chaque calcul d'AStar.
        /// </summary>
        static Node[,] s_nodemapCache;

        /// <summary>
        /// Retourne le plus court chemin entre deux positions, en prenant en compt les obstacles
        /// 
        /// AMELIORATION : trier l'open set => lowest F score : top du set
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<Vector2> Astar(Vector2 debut, Vector2 fin, int maxAllowedMoves = 1000)
        {
            if (!Mobattack.GetMap().GetPassabilityAt(fin))
                return new List<Vector2>();

            Point start = new Point((int)debut.X, (int)debut.Y);
            Point end = new Point((int)fin.X, (int)fin.Y);

            // Création du graphe utilisé pour l'astar
            Node[,] nodemap;
            nodemap = CreerGraphe();

            // Contient les noeuds qui ont déjà été évalués.
            List<Node> closedset = new List<Node>();

            // Contient les noeuds qui doivent être évalués.
            List<Node> openset = new List<Node>();
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

            // Initialisation du premier noeud.
            Node current = new Node(start, 0, EstimateHeuristicCost(start, end));
            nodemap[start.X, start.Y] = current;

            // Ajout du noeud dans l'openset.
            openset.Add(nodemap[start.X, start.Y]);

            float tentativeGScore = 0;
            while (openset.Count != 0)
            {
                current = LowestFScore(openset);
                // Si on a trouvé la fin, on s'arrête.
                if (Vector2.DistanceSquared(new Vector2(current.Position.X, current.Position.Y), new Vector2(end.X, end.Y)) < 0.1f)
                {
                    var lst =  ReconstructPath(nodemap[end.X, end.Y], cameFrom);
                    return lst;
                }

                // On marque le noeud actuel comme évalué.
                openset.Remove(current);
                closedset.Add(current);

                // Récupération des positions des noeuds dans l'open set.
                IEnumerable<Point> closedsetPosition = closedset.Select(delegate(Node node) { return node.Position; });
                IEnumerable<Point> opensetPosition = openset.Select(delegate(Node node) { return node.Position; });

                // Récupère les voisins du "graphe"
                List<Point> neighbours = GetNeighbours(current.Position);
                foreach (Point neighbour in neighbours)
                {
                    // Si le voisin en question a déja été évalué, on passe au suivant.
                    if (closedsetPosition.Contains(neighbour))
                    {
                        continue;
                    }
                        
                    // On calcule le cout de ce voisin par rapport au noeud du début.
                    float cost = GetCost(neighbour);
                    tentativeGScore = current.GScore + cost;

                    // Si ce voisin n'est pas dans l'openset, ou qu'on a trouvé un chemin plus court pour aller à ce voisin qu'un éventuel
                    // précédent chemin.
                    if (!opensetPosition.Contains(neighbour) | tentativeGScore < nodemap[neighbour.X, neighbour.Y].GScore)
                    {
                        // On indique d'où on est venu à ce voisin.
                        cameFrom[nodemap[neighbour.X, neighbour.Y]] = current;

                        // On met à jour le GScore (coût réel pour aller à ce voisin) et le FScore (coût heuristique pour atteindre la fin).
                        nodemap[neighbour.X, neighbour.Y].GScore = tentativeGScore;
                        nodemap[neighbour.X, neighbour.Y].FScore = tentativeGScore + EstimateHeuristicCost(neighbour, end);

                        // Si l'openset ne contient pas notre noeud, on le rajoute.
                        if (!openset.Contains(nodemap[neighbour.X, neighbour.Y]))
                        {
                            openset.Add(nodemap[neighbour.X, neighbour.Y]);
                        }

                    }

                }

            }

            return new List<Vector2>();
        }

        /// <summary>
        /// Obtient le coût d'un noeud.
        /// </summary>
        /// <returns></returns>
        static float GetCost(Point node)
        {
            /*if(Mobattack.GetMap().GetAliveEntitiesInRange(new Vector2(node.X+0.5f, node.Y+0.5f), 0.6f).Count != 0)
            {
                return 4.0f;
            }*/
            return 1.0f;
        }
        /// <summary>
        /// Retourne un graphe de noeud, permettant d'effectuer efficacement l'astar, ainsi que la recherche de cases disponibles pour l'unité.
        /// </summary>
        /// <returns></returns>
        public static Node[,] CreerGraphe()
        {
            Point mapSize = new Point(Mobattack.GetMap().Passability.GetLength(0), Mobattack.GetMap().Passability.GetLength(1));

            // Création du graphe.
            Node[,] nodemap;
            if (s_nodemapCache == null || s_nodemapCache.GetLength(0) != mapSize.X || s_nodemapCache.GetLength(1) != mapSize.Y)
            {
                nodemap = new Node[mapSize.X, mapSize.Y];
                for (int x = 0; x < mapSize.X; x++)
                {
                    for (int y = 0; y < mapSize.Y; y++)
                    {
                        var position = new Point(x, y);
                        nodemap[x, y] = new Node(position, 0, 0);
                    }
                }
                s_nodemapCache = nodemap;
            }
            else
            {
                // Reset des GScore seulement.
                nodemap = s_nodemapCache;
                for (int x = 0; x < mapSize.X; x++)
                {
                    for (int y = 0; y < mapSize.Y; y++)
                    {
                        nodemap[x, y].GScore = 0.0f;
                        // nodemap[x, y].FScore = 0.0f;
                    }
                }
            }
            return nodemap;
        }

    }
}