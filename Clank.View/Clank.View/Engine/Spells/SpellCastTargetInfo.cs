using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Clank.View.Engine.Spells
{

    /// <summary>
    /// Contient des informations de ciblage d'un sort qui a été lancé.
    /// 
    /// ---
    /// 
    /// Différence avec SpellTargetInfo : 
    /// 
    /// SpellCastTargetInfo ne sert juste qu'à indiquer la direction / position / cible
    /// que prend le spell une fois lancé.
    /// 
    /// SpellTargetInfo, lui, est une propriété des sorts qui détermine la manière dont il est lancé
    /// (si c'est un sort ciblé, sa range, sa zone d'effet).
    /// 
    /// -----
    /// 
    /// Différence avec SpellDescription :
    /// SpellDescription décrit la totalité du sort, et encapsule un objet de type SpellTargetInfo.
    /// 
    /// </summary>
    public class SpellCastTargetInfo
    {
        int m_targetId;
        Vector2 m_targetPosition;
        Vector2 m_targetDirection;

        /// <summary>
        /// Retourne le type de ciblage de cet objet TargetInfo.
        /// 
        /// Il existe 3 types de ciblages : Targetted (sur une cible précise), Position (à une
        /// position donnée), Direction (vers une direction donnée).
        /// </summary>
        public TargettingType Type
        {
            get;
            set;
        }
        /// <summary>
        /// Retourne la position de la cible, si le type de ciblage (Type) est TargettingType.Position.
        /// </summary>
        public Vector2 TargetPosition
        {
            get
            {
                if (Type != TargettingType.Position)
                    throw new InvalidOperationException();
                return m_targetPosition;
            }
            set
            {
                m_targetPosition = value;
            }
        }
        /// <summary>
        /// Retourne la direction de la cible, si le type de ciblage (Type) est TargettingType.Direction.
        /// </summary>
        public Vector2 TargetDirection
        {
            get
            {
                if (Type != TargettingType.Direction)
                    throw new InvalidOperationException();
                return m_targetDirection;
            }
            set
            {
                m_targetDirection = value;
            }
        }
        /// <summary>
        /// Retourne l'id de la cible, si le type de cibale (Type) est TargettingType.Targetted.
        /// </summary>
        public int TargetId
        {
            get
            {
                if (Type != TargettingType.Targetted)
                    throw new InvalidOperationException();
                return m_targetId;
            }
            set
            {
                m_targetId = value;
            }
        }
    }
}
