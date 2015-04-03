using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente un modèle upgrade d'arme.
    /// </summary>
    public class WeaponUpgradeModel
    {
        /// <summary>
        /// Obtient la description de l'upgrade.
        /// </summary>
        [Clank.ViewCreator.Export("SpellLevelDescriptionView", "Obtient du sort que lance l'arme à ce niveau d'upgrade.")]
        public Spells.SpellLevelDescription Description { get; set; }

        /// <summary>
        /// Obtient les altérations d'état appliquées passivement par cette arme.
        /// </summary>
        [Clank.ViewCreator.Export("List<StateAlterationModelView>", "Obtient les altérations d'état appliquées passivement par l'arme à ce niveau d'upgrade.")]
        public List<Entities.StateAlterationModel> PassiveAlterations { get; set; }

        /// <summary>
        /// Obtient le coût de l'upgrade.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient le coût de cette upgrade.")]
        public float Cost { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de WeaponUpgradeModel.
        /// </summary>
        public WeaponUpgradeModel()
        {
            PassiveAlterations = new List<Entities.StateAlterationModel>();
            Description = new Spells.SpellLevelDescription();
            Cost = 0;
        }

        /// <summary>
        /// Crée une nouvelle instance de Weapon upgrade initialisée avec
        /// les valeurs données.
        /// </summary>
        public WeaponUpgradeModel(Spells.SpellLevelDescription description, List<Entities.StateAlterationModel> passiveAlterations, float cost)
        {
            PassiveAlterations = new List<Entities.StateAlterationModel>();
            Description = description;
            Cost = cost;
        }

        /// <summary>
        /// Retourne une copie complète de cet instance.
        /// </summary>
        /// <returns></returns>
        public WeaponUpgradeModel Copy()
        {
            WeaponUpgradeModel copy = new WeaponUpgradeModel();
            copy.Cost = Cost;
            copy.PassiveAlterations = new List<Entities.StateAlterationModel>();
            foreach (var alt in PassiveAlterations) { copy.PassiveAlterations.Add(alt.Copy()); }
            copy.Description = Description.Copy();
            return copy;
        }

        public Views.WeaponUpgradeModelView ToView()
        {
            Views.WeaponUpgradeModelView view = new Views.WeaponUpgradeModelView();
            view.Cost = Cost;
            view.Description = Description.ToView();
            view.PassiveAlterations = new List<Views.StateAlterationModelView>();
            foreach(var alt in PassiveAlterations)
            {
                view.PassiveAlterations.Add(alt.ToView());
            }

            return view;
        }
    }

    /// <summary>
    /// Représente un modèle enchantement d'arme.
    /// </summary>
    public class WeaponEnchantModel : EquipmentModel
    {
        /// <summary>
        /// Obtient les altértions d'état appliquées à l'impact de l'attaque sur la cible.
        /// </summary>
        [Clank.ViewCreator.Export("List<StateAlterationModelView>", "Obtient les altértions d'état appliquées à l'impact de l'attaque sur la cible.")]
        public List<Entities.StateAlterationModel> OnHitEffects { get; set; }
        /// <summary>
        /// Obtient les altérations d'état appliquées lors de l'attaque sur le caster.
        /// </summary>
        [Clank.ViewCreator.Export("List<StateAlterationModelView>", "Obtient les altérations d'état appliquées lors de l'attaque sur le caster.")]
        public List<Entities.StateAlterationModel> CastingEffects { get; set; }
        /// <summary>
        /// Obtient les effets passifs appliqués par l'enchantement.
        /// </summary>
        [Clank.ViewCreator.Export("List<StateAlterationModelView>", "Obtient les effets passifs appliqués par l'enchantement.")]
        public List<Entities.StateAlterationModel> PassiveEffects { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de WeaponEnchant.
        /// </summary>
        public WeaponEnchantModel() 
        {
            OnHitEffects = new List<Entities.StateAlterationModel>();
            PassiveEffects = new List<Entities.StateAlterationModel>();
            CastingEffects = new List<Entities.StateAlterationModel>(); 
        }

        /// <summary>
        /// Crée une nouvelle instance de WeaponEnchant avec les effets donnés.
        /// </summary>
        public WeaponEnchantModel(List<Entities.StateAlterationModel> onHit, List<Entities.StateAlterationModel> castingEffects,
                            List<Entities.StateAlterationModel> passives)
        {
            OnHitEffects = onHit;
            CastingEffects = castingEffects;
            PassiveEffects = passives;
        }

        /// <summary>
        /// Retourne une copie de cette instance.
        /// </summary>
        /// <returns></returns>
        public WeaponEnchantModel Copy()
        {
            WeaponEnchantModel copy = new WeaponEnchantModel();
            foreach (var i in OnHitEffects) { copy.OnHitEffects.Add(i.Copy()); }
            foreach (var i in PassiveEffects) { copy.PassiveEffects.Add(i.Copy()); }
            foreach (var i in CastingEffects) { copy.CastingEffects.Add(i.Copy()); }
            return copy;
        }

        public Views.WeaponEnchantModelView ToView()
        {
            Views.WeaponEnchantModelView view = new Views.WeaponEnchantModelView();
            view.OnHitEffects = new List<Views.StateAlterationModelView>();
            view.CastingEffects = new List<Views.StateAlterationModelView>();
            view.PassiveEffects = new List<Views.StateAlterationModelView>();
            foreach (var e in OnHitEffects) { view.OnHitEffects.Add(e.ToView()); }
            foreach (var e in PassiveEffects) { view.PassiveEffects.Add(e.ToView()); }
            foreach (var e in CastingEffects) { view.CastingEffects.Add(e.ToView()); }
            return view;
        }

        public override EquipmentType Type
        {
            get { return EquipmentType.WeaponEnchant; }
            set { } // permet au sérialiser XML de fonctionner correctement
        }
    }

    /// <summary>
    /// Représente une arme actuellement équipée et utilisable par le héros.
    /// Elle utilise un modèle d'arme, mais possède en plus un niveau (qui détermine l'upgrade
    /// du modèle utilisée), et un enchantement.
    /// </summary>
    public sealed class Weapon
    {
        #region Variables
        float m_cooldownSeconds;
        WeaponEnchantModel m_enchant;
        WeaponModel m_model;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient le niveau actuel de l'arme.
        /// </summary>
        public int Level { get; private set; }

        
        
        /// <summary>
        /// Obtient le modèle de l'arme.
        /// </summary>
        public WeaponModel Model 
        {
            get { return m_model; }
            set
            {
                m_model = value;
                if (Owner != null)
                    Owner.ApplyWeaponPassives();
            }
        }

        /// <summary>
        /// Obtient l'enchantement actuel de l'arme.
        /// </summary>
        public WeaponEnchantModel Enchant
        {
            get { return m_enchant; }
            set 
            { 
                m_enchant = value;

                // Notifie le propriétaire que des changements ont été
                // apportés à l'arme.
                if(Owner != null)
                    Owner.ApplyWeaponPassives();
            }
        }

        /// <summary>
        /// Obtient le propriétaire de l'arme.
        /// </summary>
        public Entities.EntityHero Owner { get; set; }

        /// <summary>
        /// Retourne un string utilisé pour le debug et représentant les caracts de l'arme.
        /// </summary>
        public string Debug_Stats
        {
            get
            {
                StringBuilder b = new StringBuilder();
                foreach(var alt in Model.Upgrades[Level].PassiveAlterations)
                {
                    b.AppendLine("passive: " + alt.Debug_Stats);
                }
                foreach(var alt in Model.Upgrades[Level].Description.OnHitEffects)
                {
                    b.AppendLine("onhit: " + alt.Debug_Stats);
                }
                return b.ToString();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creé une nouvelle instance de Weapon à partir du modèle donné.
        /// </summary>
        /// <param name="model"></param>
        public Weapon(Entities.EntityHero owner, WeaponModel model)
        {
            Model = model;
            Level = 0;
            Enchant = new WeaponEnchantModel();
            Owner = owner;
        }
        /// <summary>
        /// Obtient le spell d'attaque actuel de cette arme.
        /// </summary>
        public Spells.SpellLevelDescription GetAttackSpell() { return Model.Upgrades[Level].Description; }
        /// <summary>
        /// Obtient les passifs procurés par cette arme et ses enchantements.
        /// </summary>
        public List<Entities.StateAlterationModel> GetPassives()
        {
            List<Entities.StateAlterationModel> lst = new List<Entities.StateAlterationModel>();
            lst.AddRange(Model.Upgrades[Level].PassiveAlterations);
            if(Enchant != null)
                lst.AddRange(Enchant.PassiveEffects);
            return lst;
        }
        /// <summary>
        /// Mets à niveau l'arme et obtient une valeur indiquant si l'opération
        /// a réussi.
        /// </summary>
        /// <returns></returns>
        public bool Upgrade()
        {
            if (Level >= Model.Upgrades.Count - 1)
                return false;

            Level++;
            Owner.ApplyWeaponPassives();
            return true;
        }
        
        
        /// <summary>
        /// Demande une utilisation de l'arme.
        /// Retourne true si l'utilisation a réussi, false sinon.
        /// </summary>
        /// <param name="hero"></param>
        /// <returns></returns>
        public Spells.SpellUseResult Use(Entities.EntityHero hero, Entities.EntityBase entity)
        {
            // Vérifie que le cooldown de l'arme est à 0.
            if (m_cooldownSeconds > float.Epsilon)
                return Spells.SpellUseResult.OnCooldown;
            if (hero.IsBlind)
                return Spells.SpellUseResult.Blind;

            m_cooldownSeconds = 1.0f/Math.Max(hero.GetAttackSpeed(), 0.2f);
            
            Spells.SpellLevelDescription attackSpell = GetAttackSpell();
            Spells.WeaponAttackSpell spell = new Spells.WeaponAttackSpell(hero, attackSpell, Enchant);
            return spell.Use(new Spells.SpellCastTargetInfo() { Type = Spells.TargettingType.Targetted, TargetId = entity.ID }, true);
        }

        /// <summary>
        /// Mets à jour l'arme.
        /// </summary>
        public void Update(GameTime time)
        {
            m_cooldownSeconds = (float)Math.Max(0, m_cooldownSeconds - time.ElapsedGameTime.TotalSeconds);
        }
        #endregion


    }
    /// <summary>
    /// Représente un modèle d'arme.
    /// Le modèle contient la description des upgrades (ainsi que leur prix).
    /// </summary>
    public class WeaponModel : EquipmentModel
    {
        /// <summary>
        /// Représente les upgrades de l'arme.
        /// </summary>
        [Clank.ViewCreator.Export("List<WeaponUpgradeModelView>", "Liste des upgrades possibles de l'arme.")]
        public List<WeaponUpgradeModel> Upgrades { get; set; }

        /// <summary>
        /// Obtient le prix de l'arme.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Prix d'achat de l'arme")]
        public override float Price
        {
            get
            {
                return Upgrades[0].Cost;
            }
            set
            {
                throw new Codinsa2015.Exceptions.IdiotProgrammerException("fait pas ça.");
            }
        }

        /// <summary>
        /// Obtient une vue sur cette instance.
        /// </summary>
        /// <returns></returns>
        public Views.WeaponModelView ToView()
        {
            Views.WeaponModelView view = new Views.WeaponModelView();
            view.Upgrades = new List<Views.WeaponUpgradeModelView>();
            foreach (var upgrade in Upgrades) { view.Upgrades.Add(upgrade.ToView()); }
            view.Price = Price;
            return view;
        }
        /// <summary>
        /// Crée une nouvelle instance de WeaponModel.
        /// </summary>
        public WeaponModel()
        {
            Upgrades = new List<WeaponUpgradeModel>();
        }

        public override EquipmentType Type
        {
            get { return EquipmentType.Weapon; }
            set { } // permet au sérialiseur XML de fonctionner correctement.
        }
    }
}
