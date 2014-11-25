using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine.Entities
{
    /// <summary>
    /// Représente une collection d'entités.
    /// </summary>
    public class EntityCollection : Dictionary<int, EntityBase>
    {
        

        /// <summary>
        /// Retourne les entités de la collection contenant le flag de type donné.
        /// </summary>
        /// <returns></returns>
        public EntityCollection GetEntitiesByType(EntityType type)
        {
            EntityCollection entities = new EntityCollection();
            foreach(var kvp in this)
            {
                EntityBase entity = kvp.Value;
                if (entity.Type.HasFlag(type))
                    entities.Add(kvp.Key, entity);
            }
            return entities;
        }

        /// <summary>
        /// Retourne l'entité la plus proche de la position donnée.
        /// </summary>
        public EntityBase NearestFrom(Vector2 position)
        {
            float distance = float.MaxValue;
            EntityBase entity = null;
            foreach(var kvp in this)
            {
                float dst = Vector2.DistanceSquared(position, kvp.Value.Position);
                if(dst < distance)
                {
                    entity = kvp.Value;
                    distance = dst;
                }
            }
            return entity;
        }

        /// <summary>
        /// Obtient les entités vivantes se trouvant dans la forme passée en paramètre.
        /// </summary>
        public EntityCollection GetAliveEntitiesIn(Shapes.Shape shape)
        {
            EntityCollection entitiesIn = new EntityCollection();
            foreach (var kvp in this)
            {
                if (shape.Intersects(kvp.Value.Shape) && !kvp.Value.IsDead)
                    entitiesIn.Add(kvp.Key, kvp.Value);
            }
            return entitiesIn;
        }
        /// <summary>
        /// Obtient les entités vivantes se trouvant dans la forme passée en paramètre.
        /// </summary>
        public EntityCollection GetAliveEntitiesIn(Shapes.Shape shape, EntityType type)
        {
            EntityCollection entitiesIn = new EntityCollection();
            foreach (var kvp in this)
            {
                if (shape.Intersects(kvp.Value.Shape) && !kvp.Value.IsDead && kvp.Value.Type.HasFlag(type))
                    entitiesIn.Add(kvp.Key, kvp.Value);
            }
            return entitiesIn;
        }
        /// <summary>
        /// Retourne les entités vivantes se trouvant dans le rayon donné autour de
        /// la position donnée.
        /// </summary>
        public EntityCollection GetAliveEntitiesInRange(Vector2 position, float radius)
        {
            return GetAliveEntitiesIn(new Shapes.CircleShape(position, radius));
        }

        /// <summary>
        /// Retourne les entités vivantes se trouvant dans le rayon donné autour de
        /// la position donnée.
        /// </summary>
        public EntityCollection GetAliveEntitiesInRange(Vector2 position, float radius, EntityType type)
        {
            return GetAliveEntitiesIn(new Shapes.CircleShape(position, radius), type);
        }
        /// <summary>
        /// Retourne les entités en à portée de vue de la team donnée.
        /// </summary>
        public EntityCollection GetEntitiesInSight(EntityType team)
        {
            team = team & (EntityType.Team1 | EntityType.Team2);
            int teamId = (int)team;
            EntityCollection entitiesIn = new EntityCollection();
            foreach(var kvp in this)
            {
                // Si l'unité est masquée, il faut la vision pure
                if(kvp.Value.IsStealthed)
                {
                    if (Mobattack.GetMap().Vision.HasTrueVision(team, kvp.Value.Position))
                    {
                        entitiesIn.Add(kvp.Key, kvp.Value);
                    }
                }
                else
                {
                    if (Mobattack.GetMap().Vision.HasVision(team, kvp.Value.Position))
                    {
                        entitiesIn.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            return entitiesIn;
        }
    }
}
