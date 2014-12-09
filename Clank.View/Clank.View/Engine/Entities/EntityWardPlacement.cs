using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine.Entities
{
    public class EntityWardPlacement : EntityBase
    {
        #region Variables
        EntityWard m_team1Ward;
        EntityHero m_team1WardOwner;

        EntityWard m_team2Ward;
        EntityHero m_team2WardOwner;
        #endregion
        /// <summary>
        /// Ne peut pas prendre de dégâts.
        /// </summary>
        protected override void ApplyTrueDamage(float damage)
        {
            return;
        }

        /// <summary>
        /// Crée une nouvelle instance de EntityWardPlacement.
        /// </summary>
        public EntityWardPlacement() : base()
        {
            VisionRange = 0;
            Type |= EntityType.WardPlacement;
            Shape = new Shapes.CircleShape(Vector2.Zero, 2);
        }

        #region API
        /// <summary>
        /// Détruit la ward de la team adverse au héros passé en paramètre sur cet emplacement.
        /// Retourne vrai si la ward a pu être détruite, false si aucune ward de la team
        /// n'a été trouvée.
        /// </summary>
        public bool DestroyWard(EntityHero owner)
        {
            EntityType team = owner.Type;
            if(team.HasFlag(EntityType.Team2))
            {
                if (m_team1Ward == null)
                    return false;
                m_team1WardOwner.WardCount--;
                m_team1Ward.Die();
                m_team1Ward = null;
            }
            else
            {
                if (m_team2Ward == null)
                    return false;
                m_team2WardOwner.WardCount--;
                m_team2Ward.Die();
                m_team2Ward = null;
            }

            return true;
        }
        /// <summary>
        /// Révèle la ward de la team adverse au héros passé en paramètre sur cet emplacement.
        /// Retourne vrai si la ward a pu être détruite, false si aucune ward de la team
        /// n'a été trouvée.
        /// </summary>
        public bool RevealWard(EntityHero owner)
        {
            EntityType team = owner.Type;
            if (team.HasFlag(EntityType.Team2))
            {
                if (m_team1Ward == null)
                    return false;
                m_team1Ward.Reveal(Mobattack.GetScene().Constants.Vision.WardRevealDuration);
            }
            else
            {
                if (m_team2Ward == null)
                    return false;
                m_team2Ward.Reveal(Mobattack.GetScene().Constants.Vision.WardRevealDuration);
            }

            return true;
        }

        /// <summary>
        /// Pose une ward sur cet emplacement.
        /// Retourne vrai si la ward a pu être posée.
        /// </summary>
        public bool PutWard(EntityHero owner)
        {
            // Vérifie que le nombre de ward max n'a pas été posé.
            if (owner.WardCount >= Mobattack.GetScene().Constants.Vision.MaxWardsPerHero)
                return false;

            // Pose la ward
            if(owner.Type.HasFlag(EntityType.Team1))
            {
                if (m_team1Ward != null)
                    return false;

                m_team1WardOwner = owner;
                m_team1Ward = new EntityWard() { Type = EntityType.Team1 | EntityType.Ward, Position = Position };
                Mobattack.GetMap().AddEntity(m_team1Ward);
            }
            else
            {
                if (m_team2Ward != null)
                    return false;

                m_team2WardOwner = owner;
                m_team2Ward = new EntityWard() { Type = EntityType.Team2 | EntityType.Ward, Position = Position };
                Mobattack.GetMap().AddEntity(m_team2Ward);
            }
            owner.WardCount++;
            return true;
        }
        #endregion

    }
}
