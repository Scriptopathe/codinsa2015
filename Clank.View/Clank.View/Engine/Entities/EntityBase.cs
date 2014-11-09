using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Clank.View.Engine.Shapes;
namespace Clank.View.Engine.Entities
{
    
    /// <summary>
    /// Classe de base pour les entités in-game.
    /// 
    /// </summary>
    public class EntityBase
    {
        #region Variables

        #region Battle variables
        /// <summary>
        /// Représente les points de vie actuels de l'entité.
        /// </summary>
        int m_hp;
        /// <summary>
        /// Points de bouclier de cette entité.
        /// Si l'entité subit des dégâts, ils sont répercutés en priorité sur les points de bouclier.
        /// </summary>
        int m_shieldPoints;
        /// <summary>
        /// Représente les points d'armure de cette entité.
        /// Les points d'armure réduisent les dégâts infligés à cette unité selon la formule :
        /// dégats infligés = dmg * 100 / (100+armor)
        /// </summary>
        int m_armor;
        /// <summary>
        /// Représente le nombre de points de vie maximum de cette entité.
        /// </summary>
        int m_maxHP;
        /// <summary>
        /// Position de l'entité sur la map.
        /// </summary>
        Vector2 m_position;
        /// <summary>
        /// Représente la direction de cette entité.
        /// </summary>
        Vector2 m_direction;
        /// <summary>
        /// Forme de cette entité.
        /// </summary>
        Shape m_shape;

        /// <summary>
        /// Représente la vitesse de cette entité, en unités de distance / seconde.
        /// </summary>
        float m_speed;


        /// <summary>
        /// Représente le type de l'entité.
        /// </summary>
        EntityType m_type;
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Représente la direction de cette entité.
        /// </summary>
        public Vector2 Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }
        /// <summary>
        /// Position de l'entité sur la map.
        /// </summary>
        public Vector2 Position
        {
            get { return m_position; }
            protected set { m_position = value; }
        }
        /// <summary>
        /// Forme de cette entité.
        /// </summary>
        public Shape Shape
        {
            get { return m_shape; }
            protected set { m_shape = value; }
        }
        /// <summary>
        /// Points de bouclier de cette entité.
        /// Si l'entité subit des dégâts, ils sont répercutés en priorité sur les points de bouclier.
        /// </summary>
        public int ShieldPoints
        {
            get { return m_shieldPoints; }
            protected set { m_shieldPoints = value; }
        }
        /// <summary>
        /// Obtient les points de vie actuels de l'entité.
        /// </summary>
        public int HP
        {
            get { return m_hp; }
            protected set { m_hp = value; }
        }

        /// <summary>
        /// Obtient le nombre de points de vie maximum de cette entité.
        /// </summary>
        public int MaxHP
        {
            get { return m_maxHP; }
            protected set { m_maxHP = value; }
        }

        /// <summary>
        /// Retourne une valeur indiquant si l'entité est morte.
        /// </summary>
        public bool IsDead
        {
            get { return m_hp <= 0; }
        }

        /// <summary>
        /// Retourne le type de cette entité.
        /// Le type inclut des informations sur l'équipe de l'entité (Team1, Team2 ou Neutral),
        /// ainsi que sur sa catégorie (Héros, tour, idole etc...).
        /// </summary>
        EntityType Type
        {
            get { return m_type; }
            protected set { m_type = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Avance dans la direction du personnage, à la vitesse du personnage,
        /// pendant le temps écoulé durant la frame précédente.
        /// </summary>
        public void MoveForward(GameTime time)
        {
            // Stratégie :
            // on regarde si à vitesse * time.elapsed * direction + Position, on est dans une case invalide
            // si c'est le cas, on se place sur une extrémité de la case.
        }
        #endregion
    }
}
