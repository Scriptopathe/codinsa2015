using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Shapes;
namespace Codinsa2015.Server.Entities
{
    
    /// <summary>
    /// Champ de bits énumèrant les différents passifs uniques.
    /// </summary>
    [Clank.ViewCreator.Enum("Enumère les différents passifs uniques.")]
    public enum EntityUniquePassives
    {
        None        = 0x0000,
        Hunter      = 0x0001,
        Rugged      = 0x0002,
        Unshakable  = 0x0004,
        Strategist  = 0x0010,
        Soldier     = 0x0020,
        Altruistic  = 0x0040,

        All         = 0xFFFF
    }
    /// <summary>
    /// Classe de base pour les entités in-game.
    /// 
    /// 
    /// </summary>
    public class EntityBase
    {
        #region Events / Delegate
        public delegate void OnDieDelegate(EntityBase entity, EntityHero killer);
        /// <summary>
        /// Event lancé lorsque l'entité meurt.
        /// </summary>
        public event OnDieDelegate OnDie;
        #endregion
        #region Constants
        #endregion

        #region Static
        public static int EntityCount = 0;
        public static void ResetEntityCount()
        {
            EntityCount = 0;
        }
        #endregion

        #region Variables
        #region Misc
        /// <summary>
        /// Indique si le mouvement de l'entité a été bloqué pendant cette frame, si 
        /// ce compteur est supérieur à 0.
        /// </summary>
        int m_movementBlockedCounter;
        #endregion

        #region Battle variables
        /// <summary>
        /// Représente le passif unique de cette entité.
        /// </summary>
        EntityUniquePassives m_uniquePassive;

        /// <summary>
        /// Niveau du passif unique.
        /// </summary>
        int m_uniquePassiveLevel;

        /// <summary>
        /// Représente les points de vie actuels de l'entité.
        /// </summary>
        float m_hp;
        /// <summary>
        /// Points de bouclier de cette entité.
        /// Si l'entité subit des dégâts, ils sont répercutés en priorité sur les points de bouclier.
        /// </summary>
        float m_shieldPoints;
        /// <summary>
        /// Représente les points d'armure de base de cette entité.
        /// Les points d'armure réduisent les dégâts infligés à cette unité selon la formule :
        /// dégats infligés = dmg * 100 / (100+armor)
        /// </summary>
        float m_baseArmor;
        /// <summary>
        /// Points d'attaque de base de cette unité.
        /// </summary>
        float m_baseAttackDamage;
        /// <summary>
        /// Point de résistance magique de base de cette entité.
        /// </summary>
        float m_baseMagicResist;
        /// <summary>
        /// Points d'AP de base de cette entité.
        /// </summary>
        float m_baseAbilityPower;
        /// <summary>
        /// Attack speed de base de cette entité.
        /// </summary>
        float m_baseAttackSpeed;
        /// <summary>
        /// Cooldown reduction de base de cette unité.
        /// </summary>
        float m_baseCooldownReduction;
        /// <summary>
        /// Représente le nombre de points de vie maximum de cette entité.
        /// </summary>
        float m_baseMaxHP;
        /// <summary>
        /// Régénération de HP / s de base de cette unité.
        /// </summary>
        float m_baseHPRegen;

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
        float m_baseMoveSpeed;
        /// <summary>
        /// Représente le type de l'entité.
        /// </summary>
        EntityType m_type;
        /// <summary>
        /// Retourne toutes les altérations d'état
        /// </summary>
        StateAlterationCollection m_stateAlterations;
        /// <summary>
        /// Trajectoire de l'entité lorsqu'il doit se déplacer vers un point donné.
        /// </summary>
        Trajectory m_path;

        /// <summary>
        /// Temps pendant lequel l'entité mémorise les entités lui ayant fait des dégâts.
        /// </summary>
        public float DamageTimeMemory
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient ou définit la trajectoire de l'entité :
        /// Si non nulle : le héros se dirige automatiquement vers le prochain point de la trajectoire.
        /// Si nulle : le héros ne bouge pas, sauf si on lui en donne l'ordre.
        /// </summary>
        public Trajectory Path
        {
            get { return m_path; }
            set { m_path = value; }
        }
        /// <summary>
        /// Dictionnaire contenant les entités ayant encommagé cette unité récemment (limite de temps
        /// DamageTimeMemory), et le temps restant à avant que les dites entités disparaissent du dictionnaire.
        /// </summary>
        EntityCollection m_recentlyAggressiveEntities;
        Dictionary<int, float> m_recentlyAgressiveEntitiesMemoryTime;

        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit le niveau du passif unique.
        /// </summary>
        [Clank.ViewCreator.Export("int", "niveau du passif unique")]
        public int UniquePassiveLevel
        {
            get { return m_uniquePassiveLevel; }
            set { m_uniquePassiveLevel = value; }
        }

        /// <summary>
        /// Obtient ou définit le passif unique de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("EntityUniquePassives", "passif unique de cette entité.")]
        public EntityUniquePassives UniquePassive
        {
            get { return m_uniquePassive; }
            set { m_uniquePassive = value; }
        }

        /// <summary>
        /// Si cette entité est un héros, obtient ou définit le rôle de ce héros.
        /// 
        /// note : cette propriété est stockée dans EntityBase pour pouvoir être communiquée
        /// facilement par l'API clank, qui ne gère pas l'héritage.
        /// </summary>
        [Clank.ViewCreator.Export("EntityHeroRole", "Si cette entité est un héros, obtient le rôle de ce héros.")]
        public EntityHeroRole Role
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient la liste des altérations d'état affectées à cette entité.
        /// </summary>
        //[Clank.ViewCreator.Export("List<StateAlterationView>", "Obtient la liste des altérations d'état affectées à cette entité.")]
        public StateAlterationCollection StateAlterations
        {
            get { return m_stateAlterations; }
            protected set { m_stateAlterations = value; }
        }
        /// <summary>
        /// Représente les points d'armure de base de cette entité.
        /// Les points d'armure réduisent les dégâts infligés à cette unité selon la formule :
        /// dégats infligés = dmg * 100 / (100+armor)
        /// </summary>
        [Clank.ViewCreator.Export("float", "Représente les points d'armure de base de cette entité.")]
        public float BaseArmor
        {
            get { return m_baseArmor; }
            set { m_baseArmor = value; }
        }
        /// <summary>
        /// Représente la direction de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("Vector2", "Représente la direction de cette entité.")]
        public Vector2 Direction
        {
            get { return m_direction; }
            set
            { 
                    m_direction = value; 
                    if(m_direction.X != 0 || m_direction.Y != 0)
                        m_direction.Normalize();
                    System.Diagnostics.Debug.Assert(!float.IsNaN(m_direction.X) && !float.IsNaN(m_direction.Y));
            }
        }

        /// <summary>
        /// Position de l'entité sur la map.
        /// </summary>
        [Clank.ViewCreator.Export("Vector2", "Position de l'entité sur la map.")]
        public Vector2 Position
        {
            get { return m_shape.Position; }
            set { m_shape.Position = value; }
        }

        /// <summary>
        /// Forme de cette entité.
        /// </summary>
        public Shape Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }

        /// <summary>
        /// Points de bouclier de cette entité.
        /// Si l'entité subit des dégâts, ils sont répercutés en priorité sur les points de bouclier.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Points de bouclier de cette entité.")]
        public float ShieldPoints
        {
            get { return m_shieldPoints; }
            set { m_shieldPoints = value; }
        }

        /// <summary>
        /// Obtient les points de vie actuels de l'entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient les points de vie actuels de l'entité")]
        public float HP
        {
            get { return m_hp; }
            set 
            { 
                m_hp = value;
                m_hp = Math.Max(0, Math.Min(m_baseMaxHP, m_hp));
            }
        }
        /// <summary>
        /// Obtient ou définit la régénération de HP / s de base de cette unité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "régénération de HP / s de base de cette unité.")]
        public float BaseHPRegen
        {
            get { return m_baseHPRegen; }
            set { m_baseHPRegen = value; }
        }

        /// <summary>
        /// Obtient le nombre de points de vie maximum de base de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient le nombre de points de vie maximum de base de cette entité.")]
        public float BaseMaxHP
        {
            get { return m_baseMaxHP; }
            set { m_baseMaxHP = value; }
        }

        /// <summary>
        /// Obtient ou définit la vitesse de déplacement de base de l'entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient la vitesse de déplacement de base de l'entité.")]
        public float BaseMoveSpeed
        {
            get { return m_baseMoveSpeed; }
            set { m_baseMoveSpeed = value; }
        }
        /// <summary>
        /// Retourne une valeur indiquant si l'entité est morte.
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Retourne une valeur indiquant si l'entité est morte.")]
        public bool IsDead
        {
            get { return m_hp <= 0; }
        }

        /// <summary>
        /// Retourne le type de cette entité.
        /// Le type inclut des informations sur l'équipe de l'entité (Team1, Team2 ou Neutral),
        /// ainsi que sur sa catégorie (Héros, tour, Datacenter etc...).
        /// </summary>
        [Clank.ViewCreator.Export("EntityTypeRelative", "Obtient le type de cette entité.")]
        public EntityType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// Obtient l'id de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("int", "Obtient l'id de cette entité.")]
        public int ID
        {
            get;
            protected set;
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si cette entité doit 
        /// être supprimée de la map.
        /// </summary>
        public bool IsDisposing
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient une valeur indiquant si l'entité a été supprimée.
        /// </summary>
        public bool IsDisposed
        {
            get;
            protected set;
        }
        /// <summary>
        /// Obtient ou définit les points d'attaque de base de cette unité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient ou définit les points d'attaque de base de cette unité.")]
        public float BaseAttackDamage
        {
            get { return m_baseAttackDamage; }
            set { m_baseAttackDamage = value; }
        }


        /// <summary>
        /// Cooldown reduction de base de cette unité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Cooldown reduction de base de cette unité.")]
        public float BaseCooldownReduction
        {
            get { return m_baseCooldownReduction; }
            set { m_baseCooldownReduction = value; }
        }

        /// <summary>
        /// Attack speed de base de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Attack speed de base de cette entité.")]
        public float BaseAttackSpeed
        {
            get { return m_baseAttackSpeed; }
            set { m_baseAttackSpeed = value; }
        }
        /// <summary>
        /// Points d'AP de base de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Points d'AP de base de cette entité.")]
        public float BaseAbilityPower
        {
            get { return m_baseAbilityPower; }
            set { m_baseAbilityPower = value; }
        }
        /// <summary>
        /// Point de résistance magique de base de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Point de résistance magique de base de cette entité.")]
        public float BaseMagicResist
        {
            get { return m_baseMagicResist; }
            set { m_baseMagicResist = value; }
        }

        /// <summary>
        /// Retourne la résistance magique effective de cette entité.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Export("float", "Retourne la résistance magique effective de cette entité.")]
        public virtual float GetMagicResist()
        {
            float total = BaseMagicResist;
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.MagicResistBuff);
            // Les buffs sont ajoutés de celui au plus gros scaling vers le plus petit.
            alterations.Sort(new Comparison<StateAlteration>((StateAlteration a, StateAlteration b) =>
            {
                return (int)(a.Model.DestPercentRMValue - b.Model.DestPercentRMValue);
            }));

            // Applique les buffs / debuffs ne prenant pas en compte l'armure de
            // cette entité.
            foreach (StateAlteration alteration in alterations)
            {
                if (alteration.Source != this)
                    total += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ ScalingRatios.DstMr);
                else
                    total += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ (ScalingRatios.SrcMR | ScalingRatios.DstMr));
            }


            // Applique les buffs de mr dépendants de la mr de cette entité.
            // Ceux-là sont séparés des buffs précédents puisque s'ils ne l'étaient pas,
            // la fonction GetMagicResist() serait appelée de manière récursive à l'infini.
            foreach (StateAlteration alteration in alterations)
            {
                if (alteration.Source == this)
                    total += alteration.Model.SourcePercentRMValue * total;
            }

            return total;
        }

        /// <summary>
        /// Retourne la valeur d'AP effective de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Retourne la valeur d'AP effective de cette entité.")]
        public virtual float GetAbilityPower()
        {
            float totalDamage = BaseAbilityPower;

            // Récupère tous les buffs d'ap.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.MagicDamageBuff);
            // Les buffs sont ajoutés de celui au plus gros scaling vers le plus petit.
            alterations.Sort(new Comparison<StateAlteration>((StateAlteration a, StateAlteration b) =>
            {
                return (int)(a.Model.DestPercentAPValue - b.Model.DestPercentAPValue);
            }));

            // Applique les buffs / debuffs ne prenant pas en compte les dégâts d'attaque de
            // cette entité.
            foreach (StateAlteration alteration in alterations)
            {
                if (alteration.Source != this)
                    totalDamage += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ ScalingRatios.DstAP);
                else
                    totalDamage += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ (ScalingRatios.DstAP | ScalingRatios.SrcAP));
            }



            // Applique les buffs d'attaque dépendants de l'attaque de cette entité.
            // Ceux-là sont séparés des buffs précédents puisque s'ils ne l'étaient pas,
            // la fonction GetAttackDamage() serait appelée de manière récursive à l'infini.
            
            foreach (StateAlteration alteration in alterations)
            {
                if (alteration.Source == this)
                    totalDamage += alteration.Model.SourcePercentAPValue * totalDamage;
            }

            return Math.Max(0, totalDamage);
        }

        /// <summary>
        /// Retourne la valeur de CDR effective de cette entité.
        /// (valeur entre 0 et 0.40)
        /// </summary>
        [Clank.ViewCreator.Export("float", "Retourne la valeur de CDR effective de cette entité. (de 0 à 0.40)")]
        public virtual float GetCooldownReduction()
        {
            float total = BaseCooldownReduction;           
            // Récupère tous les buffs d'attaque.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.CDR);

            // Applique les buffs / debuffs
            foreach (StateAlteration alteration in alterations)
            {
                total += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All);
            }

            return Math.Max(0, total);
        }

        /// <summary>
        /// Obtient la vitesse de déplacement de l'entité.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Export("float", "Obtient la vitesse de déplacement de l'entité.")]
        public virtual float GetMoveSpeed()
        {
            float totalMs = BaseMoveSpeed;

            // Récupère tous les buffs d'attaque.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.MoveSpeed);
            

            // Applique les buffs / debuffs
            foreach (StateAlteration alteration in alterations)
            {
                totalMs += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All);
            }

            return Math.Max(0.1f, totalMs);
        }

        /// <summary>
        /// Obtient la vitesse d'attaque effective de l'entité.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Export("float", "Obtient la vitesse d'attaque effective de l'entité.")]
        public virtual float GetAttackSpeed()
        {
            float totalMs = BaseAttackSpeed;

            // Récupère tous les buffs d'attaque.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.AttackSpeed);


            // Applique les buffs / debuffs
            foreach (StateAlteration alteration in alterations)
            {
                totalMs += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All);
            }

            return totalMs;
        }
        /// <summary>
        /// Obtient la regen de HP / s effective de l'entité.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Export("float", "Obtient la vitesse d'attaque effective de l'entité.")]
        public virtual float GetHPRegen()
        {
            float totalMs = BaseHPRegen;

            // Récupère tous les buffs d'attaque.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.Regen);


            // Applique les buffs / debuffs
            foreach (StateAlteration alteration in alterations)
            {
                totalMs += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All);
            }

            return totalMs;
        }
        /// <summary>
        /// Obtient les points d'attaque effectifs de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient les points d'attaque effectifs de cette entité.")]
        public virtual float GetAttackDamage()
        {
            float totalDamage = BaseAttackDamage;

            // Récupère tous les buffs d'attaque.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.AttackDamageBuff);
            // Les buffs sont ajoutés de celui au plus gros scaling vers le plus petit.
            alterations.Sort(new Comparison<StateAlteration>((StateAlteration a, StateAlteration b) =>
            {
                return (int)(a.Model.DestPercentADValue - b.Model.DestPercentADValue);
            }));

            // Applique les buffs / debuffs ne prenant pas en compte les dégâts d'attaque de
            // cette entité.
            foreach(StateAlteration alteration in alterations)
            {
                if (alteration.Source != this)
                    totalDamage += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ ScalingRatios.DstAd);
                else
                    totalDamage += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ (ScalingRatios.DstAd | ScalingRatios.SrcAd));
            }



            // Applique les buffs d'attaque dépendants de l'attaque de cette entité.
            // Ceux-là sont séparés des buffs précédents puisque s'ils ne l'étaient pas,
            // la fonction GetAttackDamage() serait appelée de manière récursive à l'infini.
            foreach (StateAlteration alteration in alterations)
            {
                if (alteration.Source == this)
                    totalDamage += alteration.Model.SourcePercentADValue * totalDamage;
            }

            return Math.Max(0, totalDamage);
        }

        /// <summary>
        /// Fonction utilisée pour obtenir les points d'armure effectifs sur cette unité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Fonction utilisée pour obtenir les points d'armure effectifs sur cette unité.")]
        public virtual float GetArmor()
        {
            float totalArmor = BaseArmor;

            // Récupère tous les buffs d'armure.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.ArmorBuff);
            // Les buffs sont ajoutés de celui au plus gros scaling vers le plus petit.
            alterations.Sort(new Comparison<StateAlteration>((StateAlteration a, StateAlteration b) =>
            {
                return (int)(a.Model.DestPercentArmorValue - b.Model.DestPercentArmorValue);
            }));

            // Applique les buffs / debuffs ne prenant pas en compte l'armure de
            // cette entité.
            foreach (StateAlteration alteration in alterations)
            {
                if (alteration.Source != this)
                    totalArmor += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ ScalingRatios.DstArmor);
                else
                    totalArmor += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ (ScalingRatios.SrcArmor | ScalingRatios.DstArmor));
            }



            // Applique les buffs d'armure dépendants de l'armure de cette entité.
            // Ceux-là sont séparés des buffs précédents puisque s'ils ne l'étaient pas,
            // la fonction GetArmor() serait appelée de manière récursive à l'infini.
            foreach (StateAlteration alteration in alterations)
            {
                if (alteration.Source == this)
                    totalArmor += alteration.Model.SourcePercentArmorValue * totalArmor;
            }

            return totalArmor;
        }

        /// <summary>
        /// Obtient les HP actuels de cette entité.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Export("float", "Obtient les HP actuels de cette entité.")]
        public virtual float GetHP()
        {
            return HP;
        }

        /// <summary>
        /// Obtient les HP max actuels de cette entité.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Export("float", "Obtient les HP max actuels de cette entité.")]
        public virtual float GetMaxHP()
        {
            float totalHP = BaseMaxHP;

            // Récupère tous les buffs d'hp.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.MaxHP);
            // Les buffs sont ajoutés de celui au plus gros scaling vers le plus petit.
            alterations.Sort(new Comparison<StateAlteration>((StateAlteration a, StateAlteration b) =>
            {
                return (int)(a.Model.DestPercentMaxHPValue - b.Model.DestPercentMaxHPValue);
            }));

            // Applique les buffs / debuffs ne prenant pas en compte les dégâts d'attaque de
            // cette entité.
            foreach (StateAlteration alteration in alterations)
            {
                if (alteration.Source != this)
                    totalHP += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ ScalingRatios.DstMaxHP);
                else
                    totalHP += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All ^ (ScalingRatios.SrcMaxHP | ScalingRatios.DstMaxHP));
            }



            // Applique les buffs d'attaque dépendants de l'attaque de cette entité.
            // Ceux-là sont séparés des buffs précédents puisque s'ils ne l'étaient pas,
            // la fonction GetAttackDamage() serait appelée de manière récursive à l'infini.
            foreach (StateAlteration alteration in alterations)
            {
                if (alteration.Source == this)
                    totalHP += alteration.Model.SourcePercentMaxHPValue * totalHP;
            }

            return totalHP;
        }

        /// <summary>
        /// Retourne la liste des entités ayant récemment infligés des dégâts à cette entité.
        /// </summary>
        /// <returns></returns>
        public virtual EntityCollection GetRecentlyAgressiveEntities()
        {
            return m_recentlyAggressiveEntities;
        }

        /// <summary>
        /// Retourne la liste des entités ayant récemment infligés des dégâts à cette unité
        /// durant la période de temps en secondes indiquée par time.
        /// </summary>
        public virtual EntityCollection GetRecentlyAgressiveEntities(float time)
        {
            EntityCollection coll = new EntityCollection();
            foreach(var kvp in m_recentlyAgressiveEntitiesMemoryTime)
            {
                float elapsed = DamageTimeMemory - kvp.Value;
                if (elapsed < time)
                    coll.Add(kvp.Key, m_recentlyAggressiveEntities[kvp.Key]);
            }
            return coll;
        }

        /// <summary>
        /// Retourne vrai si cette unité a été bloquée sur un mur récemment.
        /// Ordre de grandeur de mémoire : 2 frames.
        /// </summary>
        public bool IsBlockedByWall
        {
            get { return m_movementBlockedCounter > 0; }
        }

        #region State
        /// <summary>
        /// Obtient une valeur indiquant si cette entité est Rootée. (ne peut plus bouger).
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si cette entité est Rootée. (ne peut plus bouger).")]
        public bool IsRooted
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.Root).Count != 0; }
        }

        /// <summary>
        /// Obtient une valeur indiquant si cette unité est Silenced (ne peut pas utiliser de sorts).
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si cette unité est Silenced (ne peut pas utiliser de sorts).")]
        public bool IsSilenced
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.Silence).Count != 0; }
        }

        /// <summary>
        /// Obtient une valeur indiquant si cette unité est Stuned (ne peut pas bouger ni utiliser de sorts).
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si cette unité est Stuned (ne peut pas bouger ni utiliser de sorts).")]
        public bool IsStuned
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.Stun).Count != 0; }
        }

        /// <summary>
        /// Obtient une valeur indiquant si cette unité possède une immunité temporaire aux dégâts.
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si cette unité possède une immunité temporaire aux dégâts.")]
        public virtual bool IsDamageImmune
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.DamageImmune).Count != 0; }
        }

        /// <summary>
        /// Obtient une valeur indiquant si les cc présents sur cette unité sont annulés.
        /// </summary>
        public bool HasCleanse
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.Cleanse).Count != 0; }
        }
        /// <summary>
        /// Obtient une valeur indiquant si cette unité possède une immunité temporaire aux dégâts.
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si cette unité possède une immunité temporaire aux contrôles.")]
        public bool IsControlImmune
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.ControlImmune).Count != 0; }
        }
        /// <summary>
        /// Obtient une valeur indiquant si cette unité est Blinded (ne peut pas lancer d'attaque avec son arme).
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si cette unité est aveuglé (ne peut pas lancer d'attaque avec son arme).")]
        public bool IsBlind
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.Blind).Count != 0; }
        }
        /// <summary>
        /// Obtient une valeur indiquant si cette unité est invisible.
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si cette unité est invisible.")]
        public bool IsStealthed
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.Stealth).Count != 0; }
        }

        /// <summary>
        /// Obtient une valeur indiquant si cette entité possède la vision pure.
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si cette entité possède la vision pure.")]
        public bool HasTrueVision
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.TrueSight).Count != 0; }
        }

        /// <summary>
        /// Obtient une valeur indiquant si cette unité peut voir les wards.
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si cette unité peut voir les wards.")]
        public bool HasWardVision
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.WardSight).Count != 0; }
        }

        /// <summary>
        /// Retourne la range à laquelle cette entité donne la vision.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Retourne la range à laquelle cette entité donne la vision.")]
        public float VisionRange
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityBase.
        /// </summary>
        public EntityBase()
        {
            ID = EntityCount;
            EntityCount++;

            // Code de debug
            BaseMaxHP = 400;
            HP = BaseMaxHP;
            BaseArmor = 50;
            VisionRange = 3.0f;
            DamageTimeMemory = 30f;
            BaseAttackSpeed = 0.5f;
            BaseAttackDamage = 10;

            // Initialisation
            m_stateAlterations = new StateAlterationCollection();
            m_recentlyAggressiveEntities = new EntityCollection();
            m_recentlyAgressiveEntitiesMemoryTime = new Dictionary<int, float>();

            // TODO : supprimer ces lignes, seulement utiles pour le debug.
            m_shape = new RectangleShape(Vector2.Zero, new Vector2(0.5f, 0.5f));
            m_baseMoveSpeed = 8.0f;
        }

        /// <summary>
        /// Chage les constantes d'entité passées en paramètres.
        /// </summary>
        public void LoadEntityConstants(EntityConstants constants)
        {
            BaseMaxHP = constants.HP;
            BaseHPRegen = constants.HPRegen;
            BaseAttackDamage = constants.AttackDamage;
            BaseAttackSpeed = constants.AttackSpeed;
            BaseAbilityPower = constants.AbilityPower;
            BaseMagicResist = constants.MagicResist;
            BaseCooldownReduction = constants.CooldownReduction;
            BaseArmor = constants.Armor;
            BaseMoveSpeed = constants.MoveSpeed;
            VisionRange = constants.VisionRange;
            HP = BaseMaxHP;
        }

        /// <summary>
        /// Mets à jour les infos concernant les agressions des unités ennemies.
        /// </summary>
        public void UpdateAgressionInfo(GameTime time)
        {
            float elapsedSeconds = (float)time.ElapsedGameTime.TotalSeconds;
            List<int> toDeleteIds = new List<int>();
            foreach(var kvp in m_recentlyAggressiveEntities)
            {
                m_recentlyAgressiveEntitiesMemoryTime[kvp.Key] -= elapsedSeconds;
                if (m_recentlyAgressiveEntitiesMemoryTime[kvp.Key] <= 0)
                    toDeleteIds.Add(kvp.Key);
            }

            // Supprime les entités dont on n'a plus besoin de se souvenir.
            foreach(int id in toDeleteIds)
            {
                m_recentlyAggressiveEntities.Remove(id);
                m_recentlyAgressiveEntitiesMemoryTime.Remove(id);
            }
        }

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
        /// <returns>Retourne true si le mouvement a réussi.</returns>
        public bool StartMoveTo(Vector2 position)
        {
            m_path = new Trajectory(PathFinder.Astar(Position, position));
            Direction = Vector2.Zero;
            if (m_path.TrajectoryUnits.Count <= 1)
                m_path = null;
            return m_path != null;
        }

        /// <summary>
        /// Obtient une valeur indiquant si le héros est entrain de se déplacer à l'aide du path finding.
        /// </summary>
        /// <returns></returns>
        public bool IsAutoMoving()
        {
            return m_path != null;
        }
        /// <summary>
        /// Avance dans la direction du personnage, à la vitesse du personnage,
        /// pendant le temps écoulé durant la frame précédente.
        /// </summary>
        public void MoveForward(GameTime time, bool ignoreRoot=false)
        {
            MoveTowards(Direction, (float)time.ElapsedGameTime.TotalSeconds, GetMoveSpeed(), ignoreRoot);
        }
        /// <summary>
        /// Avance dans la direction donnée, à la vitesse du personnage,
        /// pendant la durée donnée (en secondes).
        /// </summary>
        public void MoveTowards(Vector2 direction, float duration, float speed, bool ignoreRoot=false)
        {
            // Si l'entité est rootée, le mouvement est impossible.
            if (IsRooted && !ignoreRoot)
                return;

            // Stratégie :
            // on regarde si à vitesse * time.elapsed * direction + Position, on est dans une case invalide
            // si c'est le cas, on se place sur une extrémité de la case.
            float length = speed * duration; 
            Vector2 dst = length * direction + Position;

            if (length < 1)
                MoveTowardsStep(dst);
            else
            {
                // Ici, on gère les "grandes" vitesse, càd, celles supérieures à une unité métrique.
                float step = 1.0f;
                bool stop = false;
                while(true)
                {
                    // On avance d'un pas de taille max d'une unité métrique.
                    stop = !MoveTowardsStep(step * direction + Position);

                    if (stop)
                        break;

                    // On augmente de 1 unité métrique la distance, si elle dépasse la distance à parcourir, 
                    // on la restreint à cette distance. A la fin de la prochaine itération, la boucle se terminera.
                    step++;
                    if (step > length)
                    {
                        step = length;
                        stop = true;
                    }
                }
            }
            
        }

        /// <summary>
        /// Avance à la position considérée, en considérant que la destination dst et à moins d'une unité métrique
        /// de distance de la position initiale.
        /// </summary>
        /// <param name="dst">destination</param>
        /// <returns>True si le mouvement peut être continué, false si une limite a été atteinte (case non passable).</returns>
        bool MoveTowardsStep(Vector2 dst)
        {
            Vector2 newDst;
            bool dstOK = GameServer.GetMap().GetPassabilityAt(dst.X, dst.Y);
            if (dstOK)
                newDst = dst;
            else
            {
                
                // Coins de la case.
                int left = (int)Position.X;
                int right = (int)(Position.X) + 1;
                int top = (int)Position.Y;
                int bottom = (int)(Position.Y) + 1;

                // On limite le mouvement au bord de la case.
                const float margin = 0.02f;
                newDst.X = Math.Min(right - margin, Math.Max(left + margin, dst.X));
                newDst.Y = Math.Min(bottom - margin, Math.Max(top + margin, dst.Y));

                m_movementBlockedCounter = 5;
            }

            Position = newDst;
            return dstOK;
        }

        /// <summary>
        /// Mets à jour l'entité.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            if (IsDisposing)
                return;
            // Compteur de blocage
            m_movementBlockedCounter--;

            // Regen de l'entité.
            UpdateRegen(time);

            // Mise à jour du mouvement
            UpdateMoveTo(time);

            // Mise à jour des passifs.
            UpdateUniquePassives(time);

            // Apply state alterations
            ApplyStateAlterations(time);

            // Mets à jour les composantes spécifiques à l'entité
            DoUpdate(time);

            // Mets à jour les altérations d'état.
            m_stateAlterations.UpdateStateAlterations(time, this);
            UpdateAgressionInfo(time);

            if (IsDead)
            {
                Die();
            }
        }

        /// <summary>
        /// Mets à jour la regen de HP.
        /// </summary>
        void UpdateRegen(GameTime time)
        {
            float regen = GetHPRegen();
            HP += regen * (float)time.ElapsedGameTime.TotalSeconds;

            // Régression du shield éventuel
            ShieldPoints = Math.Max(ShieldPoints - (ShieldPoints * 0.10f + 5) * (float)time.ElapsedGameTime.TotalSeconds, 0);
        }

        
        /// <summary>
        /// Effectue la mise à jour de l'entité.
        /// Cette méthode doit être réécrite dans les entités filles.
        /// </summary>
        /// <param name="time"></param>
        protected virtual void DoUpdate(GameTime time)
        {

        }

        /// <summary>
        /// Fonction appelée lorsque l'entité meurt.
        /// </summary>
        public virtual void Die()
        {
            if (IsDisposing)
                return;
            // Recherche le tueur de l'entité.
            float maxTime = float.MinValue;
            EntityHero killer = null;
            foreach (var kvp in m_recentlyAgressiveEntitiesMemoryTime)
            {
                EntityHero entity = GameServer.GetMap().GetEntityById(kvp.Key) as EntityHero;
                if (entity != null)
                {
                    if (kvp.Value > maxTime)
                    {
                        maxTime = kvp.Value;
                        killer = entity;
                    }
                }
            }

            // Notifie la mort de l'entité au système de récompenses.
            if (this is EntityHero)
            {
                GameServer.GetScene().RewardSystem.NotityHeroDeath((EntityHero)this, killer);
            }
            else
            {
                GameServer.GetScene().RewardSystem.NotifyUnitDeath(this, killer);
            }
            
            // Broadcaste l'information de la mort de l'entité.
            if(OnDie != null)
                OnDie(this, killer);

            IsDisposing = true;
        }
        /// <summary>
        /// Resuscite l'unité actuelle.
        /// Pour qu'elle continue à fonctionner correctement, il faut cependant aussi l'ajouter
        /// aux entités de la map.
        /// </summary>
        public void Resurrect()
        {
            IsDisposed = false;
            IsDisposing = false;
            m_path = null;
            HP = GetMaxHP();
            m_stateAlterations.EndAlterations(StateAlterationSource.SpellActive);
            m_stateAlterations.EndAlterations(StateAlterationSource.UniquePassive);
        }
        #region Alterations
        /// <summary>
        /// Applique les altérations d'état en cours.
        /// </summary>
        protected virtual void ApplyStateAlterations(GameTime time)
        {
            // Applique les dégâts AD
            List<StateAlteration> attackDamageAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.AttackDamage);
            foreach(StateAlteration alteration in attackDamageAlterations)
            {
                // Ajoute l'entité à la liste des entités ayant récemment agressé cette entité.
                if (m_recentlyAggressiveEntities.ContainsKey(alteration.Source.ID))
                    m_recentlyAgressiveEntitiesMemoryTime[alteration.Source.ID] = DamageTimeMemory;
                else
                {
                    m_recentlyAggressiveEntities.Add(alteration.Source.ID, alteration.Source);
                    m_recentlyAgressiveEntitiesMemoryTime.Add(alteration.Source.ID, DamageTimeMemory);
                }
                // Applique les dégâts de base du sort + dégâts bonus fonction de l'attaque de la source.
                float trueDamageDealt = ApplyAttackDamage(alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All));

                // Stats
                EntityHero dst = this as EntityHero;
                EntityHero src = alteration.Source as EntityHero;
                if(dst != null && src != null)
                {
                    dst.Stats.TotalDamageTaken += trueDamageDealt;
                    src.Stats.TotalDamageDealtToHeroes += trueDamageDealt;
                    src.Stats.TotalDamageDealt += trueDamageDealt;
                }
                else if(src != null)
                {
                    src.Stats.TotalDamageDealt += trueDamageDealt;
                    if(EntityType.AllObjectives.HasFlag(this.Type & EntityType.Teams))
                        src.Stats.TotalDamageDealtToObjectives += trueDamageDealt;
                    if (this.Type.HasFlag(EntityType.Structure))
                        src.Stats.TotalDamageDealtToStructures += trueDamageDealt;
                }
                // Notifie le système de récompenses.
                if(this is EntityHero && alteration.Source is EntityHero)
                {
                    GameServer.GetScene().RewardSystem.NotifyDamageDealt(alteration.Source, this, trueDamageDealt);
                }
            }

            // Applique les dégâts AP
            List<StateAlteration> magicDamageAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.MagicDamage);
            foreach (StateAlteration alteration in magicDamageAlterations)
            {
                // Ajoute l'entité à la liste des entités ayant récemment agressé cette entité.
                if (m_recentlyAggressiveEntities.ContainsKey(alteration.Source.ID))
                    m_recentlyAgressiveEntitiesMemoryTime[alteration.Source.ID] = DamageTimeMemory;
                else
                {
                    m_recentlyAggressiveEntities.Add(alteration.Source.ID, alteration.Source);
                    m_recentlyAgressiveEntitiesMemoryTime.Add(alteration.Source.ID, DamageTimeMemory);
                }

                // Applique les dégâts de base du sort + dégâts bonus fonction de l'attaque de la source.
                float trueDamageDealt = ApplyMagicDamage(alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All));

                // Stats
                EntityHero dst = this as EntityHero;
                EntityHero src = alteration.Source as EntityHero;
                if (dst != null && src != null)
                {
                    dst.Stats.TotalDamageTaken += trueDamageDealt;
                    src.Stats.TotalDamageDealtToHeroes += trueDamageDealt;
                    src.Stats.TotalDamageDealt += trueDamageDealt;
                }
                else if (src != null)
                {
                    src.Stats.TotalDamageDealt += trueDamageDealt;
                    if (EntityType.AllObjectives.HasFlag(this.Type & EntityType.Teams))
                        src.Stats.TotalDamageDealtToObjectives += trueDamageDealt;
                    if (this.Type.HasFlag(EntityType.Structure))
                        src.Stats.TotalDamageDealtToStructures += trueDamageDealt;
                }

                // Notifie le système de récompenses.
                if (this is EntityHero && alteration.Source is EntityHero)
                {
                    GameServer.GetScene().RewardSystem.NotifyDamageDealt(alteration.Source, this, trueDamageDealt);
                }
            }

            // Applique les dégâts bruts.
            List<StateAlteration> trueDamageAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.TrueDamage);
            foreach(StateAlteration alteration in trueDamageAlterations)
            {
                // Ajoute l'entité à la liste des entités ayant récemment agressé cette entité.
                if (m_recentlyAggressiveEntities.ContainsKey(alteration.Source.ID))
                    m_recentlyAgressiveEntitiesMemoryTime[alteration.Source.ID] = DamageTimeMemory;
                else
                {
                    m_recentlyAggressiveEntities.Add(alteration.Source.ID, alteration.Source);
                    m_recentlyAgressiveEntitiesMemoryTime.Add(alteration.Source.ID, DamageTimeMemory);
                }

                float trueDamageDealt = alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All);
                // Stats
                EntityHero dst = this as EntityHero;
                EntityHero src = alteration.Source as EntityHero;
                if (dst != null && src != null)
                {
                    dst.Stats.TotalDamageTaken += trueDamageDealt;
                    src.Stats.TotalDamageDealtToHeroes += trueDamageDealt;
                    src.Stats.TotalDamageDealt += trueDamageDealt;
                }
                else if (src != null)
                {
                    src.Stats.TotalDamageDealt += trueDamageDealt;
                    if (EntityType.AllObjectives.HasFlag(this.Type & EntityType.Teams))
                        src.Stats.TotalDamageDealtToObjectives += trueDamageDealt;
                    if (this.Type.HasFlag(EntityType.Structure))
                        src.Stats.TotalDamageDealtToStructures += trueDamageDealt;
                }

                // Applique les dégâts de base du sort + dégâts bonus fonction de l'attaque de la source.
                ApplyTrueDamage(trueDamageDealt);
            }

            // Applique les soins
            List<StateAlteration> healAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.Heal);
            foreach(StateAlteration alteration in healAlterations)
            {
                ApplyHeal(alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All));
            }

            // Shields
            List<StateAlteration> shields = m_stateAlterations.GetInteractionsByType(StateAlterationType.Shield);
            foreach(StateAlteration alteration in shields)
            {
                this.ShieldPoints += alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All);
                alteration.EndNow();
            }


            // Applique les dash
            List<StateAlteration> dashAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.Dash);
            foreach(StateAlteration alteration in dashAlterations)
            {
                bool isKick = alteration.Source.ID != ID;
                ApplyDash(alteration.Model.GetDashDirection(alteration.Source, this, alteration.Parameters), 
                    alteration.Model.DashSpeed,
                    alteration.RemainingTime,
                    (float)time.ElapsedGameTime.TotalSeconds,
                    alteration.Model.DashGoThroughWall,
                    isKick);
            }

            // Cleanse les CC
            if(HasCleanse)
            {
                StateAlterations.EndAlterations(StateAlterationType.Root);
                StateAlterations.EndAlterations(StateAlterationType.Stun);
                StateAlterations.EndAlterations(StateAlterationType.Silence);
                StateAlterations.EndAlterations(StateAlterationType.Blind);
            }
        }

        /// <summary>
        /// Applique l'altération de dash passée en paramètre.
        /// </summary>
        public void ApplyDash(Vector2 direction, float speed, float remainingDuration, float stepDuration, bool goThroughWalls, bool isKick)
        {
            // Détermine si la direction finale est praticable.
            Vector2 finalPosition = Position + direction * speed * remainingDuration;
            bool ignoreRoot = isKick;
            if(float.IsNaN(finalPosition.X) || float.IsNaN(finalPosition.Y))
                return;
            

            // Dash : on ignore les murs tant que la destination est praticable, si elle ne l'est pas,
            // on cogne.
            if (goThroughWalls && GameServer.GetMap().GetPassabilityAt(finalPosition.X, finalPosition.Y))
            {
                if(!IsRooted || isKick)
                    Position += direction * speed * stepDuration;
            }
            else
            {
                MoveTowards(direction, stepDuration, speed, isKick);
            }

            // Arrête le déplacement en cours après le dash.
            EndMoveTo();
        }

        /// <summary>
        /// Ajoute une altération d'état à cette entité.
        /// </summary>
        public void AddAlteration(StateAlteration alteration, bool notify=true)
        {
            
            // Immunités
            #region Immunity
            if(IsDamageImmune & ((alteration.Model.Type  & StateAlterationType.AllDamage) != 0))
            {
                alteration = alteration.Copy();
                // On annule tous les dégâts.
                alteration.Model.Type &= (StateAlterationType.AllDamage ^ StateAlterationType.All);
                if (alteration.Model.Type == StateAlterationType.None)
                    return;
            }
            if (IsControlImmune & ((alteration.Model.Type & StateAlterationType.AllCC) != 0))
            {
                alteration = alteration.Copy();
                // On annule tous les cc
                alteration.Model.Type &= (StateAlterationType.AllCC ^ StateAlterationType.All);
                if (alteration.Model.Type == StateAlterationType.None)
                    return;
            }
            #endregion

            // Modification éventuelle par le passif 
            #region Passive
            if (alteration.Model.Type.HasFlag(StateAlterationType.Stun))
            {
                // Un shakable => on transforme le stun en silence.
                if (UniquePassive == EntityUniquePassives.Unshakable && UniquePassiveLevel >= 1)
                {
                    alteration = alteration.Copy();
                    alteration.Model.Type = StateAlterationType.Silence;
                }
            }
            else if(alteration.Model.Type.HasFlag(StateAlterationType.Root))
            {
                if (UniquePassive == EntityUniquePassives.Unshakable && UniquePassiveLevel >= 1)
                {
                    // unshakable Le root ne s'applique pas.
                    return;
                }
            }
            else if(alteration.Model.Type.HasFlag(StateAlterationType.MoveSpeed))
            {
                if (UniquePassive == EntityUniquePassives.Unshakable && UniquePassiveLevel >= 2)
                {
                    alteration = alteration.Copy();
                    // unshakable => durée des slows / 2
                    if (alteration.Model.FlatValue > 0)
                        alteration.Model.FlatValue *= GameServer.GetScene().Constants.UniquePassives.UnshakableSlowResistance;
                }
            }
            else if(alteration.Model.Type.HasFlag(StateAlterationType.Silence))
            {
                if (UniquePassive == EntityUniquePassives.Altruistic && UniquePassiveLevel >= 2)
                {
                    // sur un altruiste de lvl >= 2, le silence ne s'applique pas.
                    return;
                }
            }
            #endregion

            // Notifications au système de récompenses
            #region Rewards
            if (notify)
                switch (alteration.Model.Type)
                {
                    case StateAlterationType.AttackDamageBuff:
                    case StateAlterationType.MagicDamageBuff:
                    case StateAlterationType.AttackSpeed:
                    case StateAlterationType.ArmorBuff:
                    case StateAlterationType.CDR:
                    case StateAlterationType.MaxHP:
                    case StateAlterationType.Regen:
                    case StateAlterationType.MagicResistBuff:
                        GameServer.GetScene().RewardSystem.NotifyBuffOrDebuffReception(alteration.Source,
                            this, alteration.Model,
                            alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All));
                        break;
                }
            #endregion

            // Statistiques
            #region Stats
            EntityHero src = alteration.Source as EntityHero;
            if (notify)
                switch (alteration.Model.Type)
                {
                    case StateAlterationType.AttackDamageBuff:
                    case StateAlterationType.MagicDamageBuff:
                    case StateAlterationType.AttackSpeed:
                    case StateAlterationType.ArmorBuff:
                    case StateAlterationType.CDR:
                    case StateAlterationType.MaxHP:
                    case StateAlterationType.Regen:
                    case StateAlterationType.MagicResistBuff:
                        break;
                    case StateAlterationType.Heal:
                        if (src != null)
                            src.Stats.TotalHealings += alteration.Model.GetValue(src, this, ScalingRatios.All);
                        break;
                }
            #endregion
            m_stateAlterations.Add(alteration);
        }

        /// <summary>
        /// Mets à jour les passifs uniques.
        /// </summary>
        /// <param name="time"></param>
        void UpdateUniquePassives(GameTime time)
        {
            UniquePassiveConstants cst = GameServer.GetScene().Constants.UniquePassives;
            switch (UniquePassive)
            {
                case EntityUniquePassives.Hunter:
                    {
                        EntityCollection monsters = GameServer.GetMap().Entities.
                            GetEntitiesByType(EntityType.Monster).
                            GetAliveEntitiesInRange(Position, cst.HunterActivationRange);
                        int monsterCount = monsters.Count;
                        if (monsterCount > 0)
                        {
                            string id = "unique-passive-hunter-" + ID;
                            // Applique le debuff d'armure aux monstres.
                            foreach (var kvp in monsters)
                            {
                                kvp.Value.StateAlterations.EndAlterations(id);
                                kvp.Value.AddAlteration(new StateAlteration(id,
                                    this,
                                    new StateAlterationModel()
                                    {
                                        BaseDuration = 0.25f,
                                        FlatValue = -cst.HunterMonsterArmorDebuff,
                                        Type = StateAlterationType.ArmorBuff
                                    },
                                    new StateAlterationParameters(),
                                    StateAlterationSource.UniquePassive
                                    ));
                                kvp.Value.AddAlteration(new StateAlteration(id,
                                    this,
                                    new StateAlterationModel()
                                    {
                                        BaseDuration = 0.25f,
                                        FlatValue = -cst.HunterMonsterMRDebuff,
                                        Type = StateAlterationType.MagicResistBuff
                                    },
                                    new StateAlterationParameters(),
                                    StateAlterationSource.UniquePassive
                                    ));
                            }
                            // Applique le buff de régen
                            StateAlterations.EndAlterations(id);
                            AddAlteration(new StateAlteration(id,
                                this,
                                new StateAlterationModel()
                                {
                                    BaseDuration = 1f,
                                    FlatValue = cst.HunterBonusRegen,
                                    Type = StateAlterationType.Regen
                                }, new StateAlterationParameters(),
                                StateAlterationSource.UniquePassive), false);

                            if (UniquePassiveLevel >= 2)
                            {
                                AddAlteration(new StateAlteration(id,
                                    this,
                                    new StateAlterationModel()
                                    {
                                        BaseDuration = 0.25f,
                                        FlatValue = cst.HunterBonusArmor,
                                        Type = StateAlterationType.ArmorBuff
                                    },
                                    new StateAlterationParameters(),
                                    StateAlterationSource.UniquePassive
                                    ), false);
                                AddAlteration(new StateAlteration(id,
                                    this,
                                    new StateAlterationModel()
                                    {
                                        BaseDuration = 0.25f,
                                        FlatValue = cst.HunterBonusMR,
                                        Type = StateAlterationType.MagicResistBuff
                                    },
                                    new StateAlterationParameters(),
                                    StateAlterationSource.UniquePassive
                                    ), false);
                            }
                        }
                    }
                    break;

                case EntityUniquePassives.Rugged:
                    {
                        EntityCollection ennemies = GameServer.GetMap().Entities.
                            GetEntitiesByType(EntityType.Player | ((EntityType.Teams & Type) ^ EntityType.Teams)).
                            GetAliveEntitiesInRange(Position, cst.RuggedActivationRange);

                        if (ennemies.Count > 0)
                        {
                            // AD, AP, Vitesse d'attaque et CDR bonus
                            string id = "unique-passive-rugged-" + ID;
                            StateAlterations.EndAlterations(id);

                            // Bonus d'ad
                            AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                            {
                                Type = StateAlterationType.AttackDamageBuff,
                                BaseDuration = 0.25f,
                                FlatValue = 0.0f,
                                SourcePercentADValue = cst.RuggedADBonusScaling
                            }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                            // Bonus d'ap
                            AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                            {
                                Type = StateAlterationType.MagicDamageBuff,
                                BaseDuration = 0.25f,
                                FlatValue = 0.0f,
                                SourcePercentAPValue = cst.RuggedAPBonusScaling
                            }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                            // Bonus d'as
                            AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                            {
                                Type = StateAlterationType.AttackSpeed,
                                BaseDuration = 0.25f,
                                FlatValue = cst.RuggedASBonusFlat
                            }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                            // Bonus de cdr
                            AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                            {
                                Type = StateAlterationType.CDR,
                                BaseDuration = 0.25f,
                                FlatValue = cst.RuggedCDRBonusFlat,
                            }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);

                            if (UniquePassiveLevel >= 1)
                            {
                                // Bonus de move speed
                                AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.MoveSpeed,
                                    BaseDuration = 0.25f,
                                    FlatValue = cst.RuggedMSBonus,
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                            }
                        }
                    }
                    break;


                case EntityUniquePassives.Soldier:
                    // Lvl 1 : armure + 25%, RM + 25%
                    // Lvl 2 : regen +100% quand ennemy proche
                    // Lvl 3 : max HP + 10%
                    {
                        string id = "unique-passive-soldier-" + ID;
                        // Bonus d'armure
                        AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                        {
                            Type = StateAlterationType.ArmorBuff,
                            BaseDuration = 0.25f,
                            DestPercentArmorValue = cst.SoldierArmorBuff,
                        }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                        // Bonus de MR
                        AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                        {
                            Type = StateAlterationType.MagicResistBuff,
                            BaseDuration = 0.25f,
                            DestPercentRMValue = cst.SoldierMRBuff,
                        }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);

                        if (UniquePassiveLevel >= 1)
                        {
                            // Bonus de regen si ennemi proche
                            EntityCollection ennemies = GameServer.GetMap().Entities.
                                GetEntitiesByType(EntityType.Player | ((EntityType.Teams & Type) ^ EntityType.Teams)).
                                GetAliveEntitiesInRange(Position, cst.RuggedActivationRange);
                            if (ennemies.Count > 0)
                            {
                                AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.Regen,
                                    BaseDuration = 0.25f,
                                    FlatValue = cst.SoldierRegenBuff * GetHPRegen(),
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                            }
                        }

                        if(UniquePassiveLevel >= 2)
                        {
                            // Bonus de hp max
                            AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                            {
                                Type = StateAlterationType.Regen,
                                BaseDuration = 0.25f,
                                DestPercentMaxHPValue = cst.SoldierRegenBuff,
                            }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                        }

                    }
                    break;

                case EntityUniquePassives.Strategist:
                    // Bâtiments ennemis proches : pertes d'armure et RM.
                    // Virus alliés : +10 vit depl
                    // Lvl1 : Bâtiments alliés proches +50% armure / RM
                    // Lvl2 : Virus alliés proches +50% armure / RM
                    {
                        EntityCollection ennemyBuildings = GameServer.GetMap().Entities.
                            GetEntitiesByType(EntityType.Structure | ((EntityType.Teams & Type) ^ EntityType.Teams)).
                            GetAliveEntitiesInRange(Position, cst.StrategistActivationRange);
                        EntityCollection allyVirus = GameServer.GetMap().Entities.
                            GetEntitiesByType(EntityType.Virus | (EntityType.Teams & Type)).
                            GetAliveEntitiesInRange(Position, cst.StrategistActivationRange);
                        string id = "unique-passive-strategist-" + ID;
                        
                        // Debuffs des buildings ennemys.
                        if(ennemyBuildings.Count > 0)
                        {
                            foreach(var kvp in ennemyBuildings)
                            {
                                kvp.Value.StateAlterations.EndAlterations(id);
                                // Debuff d'armure.
                                kvp.Value.AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.ArmorBuff,
                                    BaseDuration = 0.25f,
                                    DestPercentArmorValue = cst.StrategistAllyStructureArmorBuff,
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                                // Debuff de RM.
                                kvp.Value.AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.MagicResistBuff,
                                    BaseDuration = 0.25f,
                                    DestPercentRMValue = cst.StrategistAllyStructureRMBuff,
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                            }
                        }

                        // Buff des Virus alliés.
                        if(allyVirus.Count > 0)
                        {
                            foreach(var kvp in allyVirus)
                            {
                                kvp.Value.StateAlterations.EndAlterations(id);

                                // Buff de move speed.
                                kvp.Value.AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.MoveSpeed,
                                    BaseDuration = 0.25f,
                                    FlatValue = cst.StrategistAllyVirusMSBuff,
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);

                                // Level 2 : Virus + tanky !
                                if(UniquePassiveLevel >= 2)
                                {
                                    // Buff d'armure.
                                    kvp.Value.AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                    {
                                        Type = StateAlterationType.ArmorBuff,
                                        BaseDuration = 0.25f,
                                        DestPercentArmorValue = cst.StrategistAllyStructureArmorBuff,
                                    }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                                    // Buff de RM.
                                    kvp.Value.AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                    {
                                        Type = StateAlterationType.MagicResistBuff,
                                        BaseDuration = 0.25f,
                                        DestPercentRMValue = cst.StrategistAllyStructureRMBuff,
                                    }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                                }
                            }
                        }

                        // Level 1 : 
                        if(UniquePassiveLevel >= 1)
                        {
                            // Bâtiments alliés proches +50% armure / RM
                            EntityCollection allyBuildings = GameServer.GetMap().Entities.
                                GetEntitiesByType(EntityType.Structure | (EntityType.Teams & Type)).
                                GetAliveEntitiesInRange(Position, cst.StrategistActivationRange);

                            foreach (var kvp in allyBuildings)
                            {
                                kvp.Value.StateAlterations.EndAlterations(id);
                                // Buff d'armure.
                                kvp.Value.AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.ArmorBuff,
                                    BaseDuration = 0.25f,
                                    DestPercentArmorValue = cst.StrategistAllyStructureArmorBuff,
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                                // Buff de RM.
                                kvp.Value.AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.MagicResistBuff,
                                    BaseDuration = 0.25f,
                                    DestPercentRMValue = cst.StrategistAllyStructureRMBuff,
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                            }
                            
                        }

                    }
                    break;

                case EntityUniquePassives.Unshakable:
                    {
                        // Max HP bonus
                        string id = "unique-passive-unshakable-" + ID;
                        StateAlterations.EndAlterations(id);
                        AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                        {
                            Type = StateAlterationType.MaxHP,
                            BaseDuration = 0.25f,
                            FlatValue = cst.UnshakableMaxHpFlatBonus,
                        }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);

                    }
                    break;

                case EntityUniquePassives.Altruistic:
                    {
                        // Alliés proches : bonus regen
                        // (soins donnés + 50% : cf StateAlterationModel.GetValue())
                        // Lvl1 : Alliés proches : + 25% RM et armor
                        // Lvl2 : +20% CDR, (immunité aux silences cf AddAlteration).
                        EntityCollection allies = GameServer.GetMap().Entities.
                             GetEntitiesByType(EntityType.Player | (EntityType.Teams & Type)).
                             GetAliveEntitiesInRange(Position, cst.AltruistActivationRange);

                        string id = "unique-passive-unshakable-" + ID;
                        StateAlterations.EndAlterations(id);

                        foreach (var ally in allies)
                        {
                            // Regen
                            AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                            {
                                Type = StateAlterationType.Regen,
                                BaseDuration = 0.25f,
                                FlatValue = cst.AltruistAllyRegenBonus,
                            }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);

                            if (UniquePassiveLevel >= 1)
                            {
                                // Armor
                                AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.ArmorBuff,
                                    BaseDuration = 0.25f,
                                    DestPercentArmorValue = cst.AltruistAllyArmorBonus,
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);

                                // RM
                                AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.MagicResistBuff,
                                    BaseDuration = 0.25f,
                                    DestPercentRMValue = cst.AltruistAllyMRBonus,
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                            }

                            if(UniquePassiveLevel >= 2)
                            {
                                // +20% CDR
                                AddAlteration(new StateAlteration(id, this, new StateAlterationModel()
                                {
                                    Type = StateAlterationType.CDR,
                                    BaseDuration = 0.25f,
                                    FlatValue = cst.AltruistBonusCDR,
                                }, new StateAlterationParameters(), StateAlterationSource.UniquePassive), false);
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Applique le nombre de dégâts indiqué à cette entité.
        /// Cette fonction prend en compte l'armure de l'entité pour déterminer
        /// les dégâts réellement infligés.
        /// </summary>
        /// <returns>Retourne le nombre de dégâts bruts infligés.</returns>
        protected virtual float ApplyAttackDamage(float damage)
        {
            float armor = GetArmor();
            float trueDamages = (damage * 100 / (100 + armor));
#if DEBUG
            DebugBattleLogMessage("Dégâts AD infligés : " + damage + " (" + trueDamages + " true damage) armure : " + armor + " hp : " + GetHP() );
#endif
            ApplyTrueDamage(trueDamages);
            return trueDamages;
        }
        /// <summary>
        /// Applique le nombre de dégâts magiques indiqué à cette entité.
        /// Cette fonction prend en compte la résistance magique de l'entité pour déterminer
        /// les dégâts réellement infligés.
        /// </summary>
        /// <returns>Retourne le nombre de dégâts bruts infligés.</returns>
        protected virtual float ApplyMagicDamage(float damage)
        {
            float trueDamages = (damage * 100 / (100 + GetMagicResist()));
            ApplyTrueDamage(trueDamages);
#if DEBUG
            DebugBattleLogMessage("Dégâts AP infligés : " + damage + " (" + trueDamages + " true damage) magic resist : " + GetMagicResist() + " hp : " + GetHP());
#endif
            return trueDamages;
        }
        /// <summary>
        /// Applique le nombre de dégâts bruts indiqué à cette entité.
        /// </summary>
        protected virtual void ApplyTrueDamage(float damage)
        {
            // Notification au système de récompenses pour les shields.
            if(this is EntityHero)
            {
                // Notif de la consommation de shields.
                StateAlterationCollection shields = m_stateAlterations.GetInteractionsByType(StateAlterationType.Shield);
                foreach (var shield in shields)
                {
                    if(shield.Source is EntityHero)
                        GameServer.GetScene().RewardSystem.NotifyShieldConsumption((EntityHero)this, (EntityHero)shield.Source, Math.Min(ShieldPoints, damage));
                }
            }

            float remainingDamage = Math.Max(0, damage - ShieldPoints);
            float remainingShield = Math.Max(0, ShieldPoints - damage);

            m_shieldPoints = remainingShield;
            HP -= remainingDamage;            
        }

        /// <summary>
        /// Applique le soin dont la valeur est passée en paramètre.
        /// </summary>
        /// <param name="heal"></param>
        protected virtual void ApplyHeal(float heal)
        {
            HP += heal;
        }

        /// <summary>
        /// Obtient une valeur indiquant si cette entité a atteint la position donnée.
        /// (la tolérance est calculée à partir du temps écoulé entre la dernière
        /// frame et de la vitesse de l'entité).
        /// </summary>
        public bool HasReachedPosition(Vector2 position, GameTime time, float speed)
        {
            float dst = 1;// (float)time.ElapsedGameTime.TotalSeconds * speed;
            float dstSquare = Vector2.DistanceSquared(position, Position);
            return dstSquare <= dst * dst;
        }
        #endregion
       

        protected float __angle;

        /// <summary>
        /// Supprime cette entité de la map.
        /// /!\ Cette fonction est à appeler par la map, pour supprimer cette entité, utiliser Die().
        /// </summary>
        public void Dispose()
        {
            IsDisposed = true;
        }
        #endregion

        #region Misc
        /// <summary>
        /// Ajoute un message de debug dans le battle log.
        /// </summary>
        /// <param name="message"></param>
        protected void DebugBattleLogMessage(string message)
        {
            GameServer.GetBattleLog().AddMessage("[from: " + this.ToString() + "] " + message);
        }

        /// <summary>
        /// Affiche des stats de debug.
        /// </summary>
        public virtual string Debug_Stats
        {
            get
            {
                StringBuilder b = new StringBuilder();
                b.AppendLine("-- " + ToString() + " (" + ID + ")");
                b.AppendLine(String.Format("Max HP = {0}", GetMaxHP()));
                b.AppendLine(String.Format("Shield = {0}", ShieldPoints));
                b.AppendLine(String.Format("HP     = {0}", GetHP()));
                b.AppendLine(String.Format("Regen  = {0} HP/s", GetHPRegen()));
                b.AppendLine(String.Format("Armor  = {0}", GetArmor()));
                b.AppendLine(String.Format("MR     = {0}", GetMagicResist()));
                b.AppendLine(String.Format("AD     = {0}", GetAttackDamage()));
                b.AppendLine(String.Format("AP     = {0}", GetAbilityPower()));
                b.AppendLine(String.Format("MS     = {0}", GetMoveSpeed()));
                b.AppendLine(String.Format("AS     = {0}", GetAttackSpeed()));
                return b.ToString();
            }
        }
        /// <summary>
        /// Indique si oui ou non l'entité est à la position donné (avec une erreur convenablement
        /// calculée).
        /// </summary>
        public bool IsAt(Vector2 position)
        {
            float error = GetMoveSpeed() * (float)GameServer.GetTime().ElapsedGameTime.TotalSeconds;
            return Vector2.DistanceSquared(position, Position) <= error * error;
        }
        #endregion

        #region Vision
        /// <summary>
        /// Indique si cette entité voit l'autre entité.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool HasSightOn(EntityBase other)
        {
            return GameServer.GetMap().Vision.HasSightOn(Type, other);
        }

        /// <summary>
        /// Indique si l'entité 'other' est dans la RANGE de vision de cette entité.
        /// /!\ Cela n'indique pas si cette entité a la vision sur 'other'. Utiliser HasSightOn pour ça.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsInVisionRange(EntityBase other)
        {
            return Vector2.DistanceSquared(other.Position, Position) <= this.VisionRange * this.VisionRange;
        }

        public override string ToString()
        {
            return Type.ToString() + "(" + ID + ")";
        }
        #endregion

    }
}
