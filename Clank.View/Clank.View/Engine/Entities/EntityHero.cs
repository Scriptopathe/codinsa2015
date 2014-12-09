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

        /// <summary>
        /// Représente les consommables possédés par le héros.
        /// </summary>
        Consummable[] m_consummables;
    
        #endregion

        #region Properties
        /// <summary>
        /// Obtient les consommables possédés par ce héros.
        /// </summary>
        public Consummable[] Consummables
        {
            get { return m_consummables; }
        }
        /// <summary>
        /// Obtient le consommable contenu dans le slot 1.
        /// </summary>
        public Consummable Consummable1
        {
            get { return m_consummables[0]; }
            protected set { m_consummables[0] = value; }
        }
        /// <summary>
        /// Obtient le consommable contenu dans le slot 1.
        /// </summary>
        public Consummable Consummable2
        {
            get { return m_consummables[1]; }
            protected set { m_consummables[1] = value; }
        }
        /// <summary>
        /// Obtient ou définit le nombre de wards que ce héros a posé sur la map.
        /// </summary>
        public int WardCount
        {
            get;
            set;
        }
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
        /// Différence de PA entre la frame précédente et cette frame.
        /// </summary>
        float __paDiff;
        /// <summary>
        /// Obtient les points d'amélioration obtenus par ce héros.
        /// </summary>
        public float PA
        {
            get { return m_pa; }
            set 
            {
                float diff = value - m_pa;
                m_pa = value;
                __paDiff += diff;
            }
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

        #endregion

        #region Methods

        /// <summary>
        /// Crée une nouvelle instance de EntityHero.
        /// </summary>
        public EntityHero()
        {
            Spells = new List<Spell>();
            Spells.Add(new Spells.FireballSpell(this, 4));
            Spells.Add(new Spells.DashForwardSpell(this));
            Spells.Add(new Spells.MovementSpeedBuffSpell(this));
            Spells.Add(new Spells.TargettedTowerSpell(this));
            VisionRange = 8;
            BaseMoveSpeed = 2;
            m_consummables = new Consummable[2] {
                new WardConsummable(),
                new UnwardConsummable()
            };
        }

        /// <summary>
        /// Mise à jour de l'entité.
        /// </summary>
        protected override void DoUpdate(GameTime time)
        {
            base.DoUpdate(time);
            UpdatePAParticle();
            UpdateConsummables(time);
            foreach (Spell spell in Spells) { spell.UpdateCooldown((float)time.ElapsedGameTime.TotalSeconds); }
        }

        /// <summary>
        /// Mets à jour les consommables.
        /// </summary>
        void UpdateConsummables(GameTime time)
        {
            for(int i = 0; i < m_consummables.Length; i++)
            {
                if (m_consummables[i].Update(time, this))
                    m_consummables[i] = new EmptyConsummable();
            }
        }
        #endregion

        #region API

        /// <summary>
        /// Utilise le consommable dans le slot donné.
        /// </summary>
        public void UseConsummable(int id)
        {
            if(m_consummables[id].Use(this))
                m_consummables[id] = new EmptyConsummable();
        }
        #endregion

        #region DEBUG
        /// <summary>
        /// Mets à jour les particules des PA.
        /// </summary>
        void UpdatePAParticle()
        {
            if (__paDiff > 2)
                Mobattack.GetScene().Particles.Add(new Particles.ParticleText()
                {
                    CurrentColor = Color.White,
                    MoveFunction = Particles.ParticleBase.MoveLine((this.Position + new Vector2(0, -1))),
                    DurationSeconds = 2f,
                    StartPosition = this.Position,
                    Text = (__paDiff < 0 ? "" : "+") + ((int)__paDiff).ToString()
                });
            __paDiff = 0;
        }
        #endregion

    }
}
