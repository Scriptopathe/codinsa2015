using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Spells;
using Codinsa2015.Server.Equip;
namespace Codinsa2015.Server.Entities
{
    /// <summary>
    /// Enumère les différents rôles des héros.
    /// </summary>
    [Clank.ViewCreator.Enum("Enumère les différents rôles des héros.")]
    public enum EntityHeroRole
    {
        Fighter     = 0,
        Mage        = 1,
        Tank        = 2,
        Max         = Tank,
        
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
        /// Représente l'arme possédée par le héros.
        /// </summary>
        Weapon m_weapon;

        /// <summary>
        /// Représente la paire de bottes possédée par le héros.
        /// </summary>
        Boots m_boots;

        /// <summary>
        /// Représente l'amulette possédée par le héros.
        /// </summary>
        Amulet m_amulet;

        /// <summary>
        /// Représente les consommables possédés par le héros.
        /// </summary>
        ConsummableStack[] m_consummables;
    
        #endregion

        #region Properties
        /// <summary>
        /// Obtient les consommables possédés par ce héros.
        /// </summary>
        public ConsummableStack[] Consummables
        {
            get { return m_consummables; }
        }
        /// <summary>
        /// Obtient le consommable contenu dans le slot 1.
        /// </summary>
        public ConsummableStack Consummable1
        {
            get { return m_consummables[0]; }
            protected set { m_consummables[0] = value; }
        }
        /// <summary>
        /// Obtient le consommable contenu dans le slot 2.
        /// </summary>
        public ConsummableStack Consummable2
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
        /// Obtient l'arme équipée par le héros.
        /// </summary>
        public Equip.Weapon Weapon
        {
            get { return m_weapon; }
            set
            {
                // Si on remplace l'arme précédente.
                m_weapon = value;
                ApplyWeaponPassives();
            }
        }

        /// <summary>
        /// Obtient la paire de bottes équipée par ce héros.
        /// </summary>
        public Equip.Boots Boots
        {
            get { return m_boots; }
            set
            {
                m_boots = value;
                ApplyPassives(EquipmentType.Boots);
            }
        }
        /// <summary>
        /// Obtient l'amulette possédée par ce héros.
        /// </summary>
        public Equip.Amulet Amulet
        {
            get { return m_amulet; }
            set
            {
                m_amulet = value;
                ApplyPassives(EquipmentType.Amulet);
            }
        }
        /// <summary>
        /// Obtient l'armure équipée par ce héros.
        /// </summary>
        public Equip.Armor Armor
        {
            get { return m_armor; }
            set
            {
                m_armor = value;
                ApplyPassives(EquipmentType.Armor);
            }
        }

        
        /// <summary>
        /// Applique les passifs des armes.
        /// </summary>
        public void ApplyWeaponPassives()
        {
            StateAlterations.EndAlterations(StateAlterationSource.Weapon);
            if (m_weapon != null)
                foreach (StateAlterationModel model in m_weapon.GetPassives())
                {
                    model.BaseDuration = StateAlteration.DURATION_INFINITY;
                    AddAlteration(new StateAlteration("weapon-passive",
                        this,
                        model, 
                        new StateAlterationParameters(), 
                        StateAlterationSource.Weapon), false);
                }
        }

        /// <summary>
        /// Applique les passifs de l'équipement passif passé en paramètre.
        /// </summary>
        public void ApplyPassives(EquipmentType equip)
        {
            StateAlterationSource src;
            PassiveEquipment passiveEquip;
            switch(equip)
            {
                case EquipmentType.Armor:
                    src = StateAlterationSource.Armor; passiveEquip = m_armor; break;
                case EquipmentType.Boots:
                    src = StateAlterationSource.Boots; passiveEquip = m_boots; break;
                case EquipmentType.Amulet:
                    src = StateAlterationSource.Amulet; passiveEquip = m_amulet; break;
                default:
                    throw new NotImplementedException();
            }

            StateAlterations.EndAlterations(src);
            if (passiveEquip != null)
                foreach (StateAlterationModel model in passiveEquip.GetPassives())
                {
                    model.BaseDuration = StateAlteration.DURATION_INFINITY;
                    AddAlteration(new StateAlteration("equip-passive-" + equip.ToString(),
                        this, 
                        model,
                        new StateAlterationParameters(), src), false);
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
            Spells.Add(Server.Spells.SpellFactory.Meteor(this)); //new Spells.FireballSpell(this, 4));
            Spells.Add(new Spells.DashForwardSpell(this));
            Spells.Add(new Spells.MovementSpeedBuffSpell(this));
            Spells.Add(new Spells.TargettedTowerSpell(this));
            Type |= EntityType.Player;
            VisionRange = 8;
            BaseMoveSpeed = 2;
            m_consummables = new ConsummableStack[2] {
                new ConsummableStack(this, ConsummableType.Ward),
                new ConsummableStack(this, ConsummableType.Unward)
            };
        }

        /// <summary>
        /// Mise à jour de l'entité.
        /// </summary>
        protected override void DoUpdate(GameTime time)
        {
            base.DoUpdate(time);
            UpdateConsummables(time);

            if(Weapon != null)
                Weapon.Update(time);

            // Mets à jour les spells et applique les passifs.
            foreach (Spell spell in Spells) { spell.UpdateCooldown((float)time.ElapsedGameTime.TotalSeconds); spell.ApplyPassives(); }
        }

        /// <summary>
        /// Mets à jour les consommables.
        /// </summary>
        void UpdateConsummables(GameTime time)
        {
            for(int i = 0; i < m_consummables.Length; i++)
            {
                m_consummables[i].Update(time);
            }
        }
        #endregion

        #region API

        /// <summary>
        /// Utilise le consommable dans le slot donné.
        /// </summary>
        public void UseConsummable(int id)
        {
            m_consummables[id].Use();
        }
        #endregion

        #region DEBUG
        #endregion

    }
}
