using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Clank.View.Engine.Spells;
using Clank.View.Engine.Equip;
namespace Clank.View.Engine.Entities
{
    public enum EntityHeroRole
    {
        Fighter,
        Mage,
        Tank
    }
    /// <summary>
    /// Classe de base pour toutes les entités héros.
    /// </summary>
    public class EntityHero : EntityBase
    {
        #region Variables
        /// <summary>
        /// Liste de spells accessibles pour ce héros.
        /// </summary>
        List<Spell> m_spells;

        /// <summary>
        /// Points d'amélioration obtenus par ce héros.
        /// </summary>
        float m_pa;
        
        /// <summary>
        /// Représente l'armure possédée par ce héros.
        /// </summary>
        Armor m_armor;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit le rôle de ce héros.
        /// </summary>
        public EntityHeroRole Role
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient ou définit la liste des spells accessibles pour ce héros.
        /// </summary>
        public List<Spell> Spells
        {
            get { return m_spells; }
            set { m_spells = value; }
        }

        /// <summary>
        /// Obtient les points d'amélioration obtenus par ce héros.
        /// </summary>
        public float PA
        {
            get { return m_pa; }
            set { m_pa = value; }
        }

        /// <summary>
        /// Obtient l'armure équippée par ce héros.
        /// </summary>
        public Equip.Armor Armor
        {
            get { return m_armor; }
            set
            {
                // Si on remplace l'armure précédente : on termine toutes ses anciennes
                // intéractions.
                if(m_armor != null)
                {
                    StateAlterations.EndAlterations(StateAlterationSource.Armor);
                }

                m_armor = value;

                // Si la nouvelle valeur n'est pas nulle, on applique les intéractions.
                if(value != null)
                    foreach(StateAlterationModel model in m_armor.Alterations)
                    {
                        model.BaseDuration = StateAlteration.DURATION_INFINITY;
                        StateAlteration alt = new StateAlteration(this, model, new StateAlterationParameters(), StateAlterationSource.Armor);
                        AddAlteration(alt);
                    }
            }
        }

        /// <summary>
        /// Trajectoire du héros lorsqu'il doit se déplacer vers un point donné.
        /// </summary>
        Trajectory m_path;
        #endregion

        #region Methods

        /// <summary>
        /// Crée une nouvelle instance de EntityHero.
        /// </summary>
        public EntityHero()
        {
            Spells = new List<Spell>();
        }

        /// <summary>
        /// Mise à jour de l'entité.
        /// </summary>
        protected override void DoUpdate(GameTime time)
        {
            base.DoUpdate(time);
            __UpdateDebug(time);
            UpdateMoveTo(time);
        }
        #endregion

        #region API

        /// <summary>
        /// Mets à jour le suivi de la trajectoire créée par A*.
        /// </summary>
        void UpdateMoveTo(GameTime time)
        {
            if (m_path != null)
            {
                m_path.UpdateStep(Position, GetMoveSpeed(), time);
                Vector2 oldDir = Direction;
                Direction = m_path.CurrentStep - Position;

                // Si on se met subitement à changer de direction vers l'arrière, c'est
                // qu'on a fini.
                if (m_path.IsEnded(Position, GetMoveSpeed(), time))//Vector2.Dot(Direction, oldDir) <-0.9f)
                    m_path = null;
                else
                    MoveForward(time);


            }
        }

        /// <summary>
        /// Arrête le déplacement automatique du héros selon l'A*.
        /// </summary>
        public void EndMoveTo()
        {
            m_path = null;
        }

        /// <summary>
        /// Utilise l'A* pour calculer la trajectoire du héros jusqu'à la position
        /// donnée, et ordonne au héros de suivre cette trajectoire jusqu'à ce que
        /// EndMoveTo() soit appelé, ou que le héros atteigne la position désirée.
        /// </summary>
        /// <param name="position"></param>
        public void StartMoveTo(Vector2 position)
        {
            m_path = new Trajectory(PathFinder.Astar(Position, position));
            Direction = Vector2.Zero;
            if (m_path.TrajectoryUnits.Count <= 1)
                m_path = null;
        }


        #region Debug
        Spells.FireballSpell __spell = null;
        void __UpdateDebug(GameTime time)
        {
            if (!Type.HasFlag(EntityType.Team1Player))
                return;

            if (IsDead)
                IsDisposing = true;
            /*

            // DEBUG
            if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Q))
                __angle -= 0.1f;
            else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.D))
                __angle += 0.1f;

            Direction = new Vector2((float)Math.Cos(__angle), (float)Math.Sin(__angle));

            if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Z))
                MoveForward(time);*/

            var ms = Input.GetMouseState();
            Vector2 pos = (new Vector2(ms.X, ms.Y) - new Vector2(Mobattack.GetMap().Viewport.X, Mobattack.GetMap().Viewport.Y) + Mobattack.GetMap().ScrollingVector2) / Map.UnitSize;
            if(Input.IsRightClickTrigger())
            {
                if(Vector2.Distance(pos, Position) < 5)
                    m_path = new Trajectory(new List<Vector2>() { pos });
                else
                    StartMoveTo(pos);
            }

            // -----
            if (__spell == null)
                __spell = new Spells.FireballSpell(this);
            __spell.UpdateCooldown((float)time.ElapsedGameTime.TotalSeconds);
            if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                __spell.Use(new Spells.SpellCastTargetInfo()
                {
                    Type = TargettingType.Direction,
                    TargetDirection = Direction
                });
            }
        }
        #endregion
        #endregion

    }
}
