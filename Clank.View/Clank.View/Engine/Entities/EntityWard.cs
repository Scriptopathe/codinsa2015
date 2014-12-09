using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine.Entities
{
    public class EntityWard : EntityBase
    {
        #region Variables
        bool m_revealed;
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
        public EntityWard()
            : base()
        {
            VisionRange = Mobattack.GetScene().Constants.Vision.WardRange;
            Type |= EntityType.Ward;
            Shape = new Shapes.CircleShape(Vector2.Zero, 1);

            // Add the stealth property to the ward.
            AddAlteration(new StateAlteration(this, new StateAlterationModel()
                {
                    BaseDuration = StateAlteration.DURATION_INFINITY,
                    Type = StateAlterationType.Stealth
                },
                new StateAlterationParameters()
                {
                    
                }, StateAlterationSource.Self));
        }

        
        #region API
        /// <summary>
        /// Révèle la ward.
        /// </summary>
        public void Reveal(float duration)
        {
            if(m_revealed)
                return;

            m_revealed = true;
            StateAlterations.EndAlterations(StateAlterationSource.Self);
            Mobattack.GetScene().EventSheduler.Schedule(new Scheduler.ActionDelegate(() =>
            {
                // Add the stealth property to the ward.
                AddAlteration(new StateAlteration(this, new StateAlterationModel()
                {
                    BaseDuration = StateAlteration.DURATION_INFINITY,
                    Type = StateAlterationType.Stealth
                },
                new StateAlterationParameters(){}, 
                StateAlterationSource.Self));
                m_revealed = false;
            }), duration);
        }
        #endregion

    }
}
