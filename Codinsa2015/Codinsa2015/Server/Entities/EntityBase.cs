using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Shapes;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.Entities
{
    
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
        #region Details

        #endregion
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Obtient la liste des altérations d'état affectées à cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("List<StateAlterationView>", "Obtient la liste des altérations d'état affectées à cette entité.")]
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
            set { m_direction = value; m_direction.Normalize(); }
        }

        /// <summary>
        /// Position de l'entité sur la map.
        /// </summary>
        [Clank.ViewCreator.Export("Vector2", "Position de l'entité sur la map.")]
        public Vector2 Position
        {
            get { return m_position; }
            set { m_position = value; m_shape.Position = m_position; }
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
        /// ainsi que sur sa catégorie (Héros, tour, idole etc...).
        /// </summary>
        [Clank.ViewCreator.Export("EntityType", "Retourne le type de cette entité.")]
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
            return BaseMagicResist;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retourne la valeur d'AP effective de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Retourne la valeur d'AP effective de cette entité.")]
        public virtual float GetAbilityPower()
        {
            return BaseAbilityPower;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retourne la valeur de CDR effective de cette entité.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Retourne la valeur de CDR effective de cette entité.")]
        public virtual float GetCooldownReduction()
        {
            return BaseCooldownReduction;
            throw new NotImplementedException();
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

            return Math.Max(0, totalMs);
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

            return Math.Max(0, totalMs);
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
                if(alteration.Source == this)
                    totalDamage += alteration.Model.SourcePercentADValue * totalDamage
                                + alteration.Model.DestPercentADValue * totalDamage;
                else
                    totalDamage += alteration.Model.DestPercentADValue * totalDamage;
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
                    totalArmor += alteration.Model.SourcePercentArmorValue * totalArmor
                                + alteration.Model.DestPercentArmorValue * totalArmor;
                else
                    totalArmor += alteration.Model.DestPercentArmorValue * totalArmor;
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
            float totalHP = BaseArmor;

            // Récupère tous les buffs d'attaque.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.MaxHP);
            // Les buffs sont ajoutés de celui au plus gros scaling vers le plus petit.
            alterations.Sort(new Comparison<StateAlteration>((StateAlteration a, StateAlteration b) =>
            {
                return (int)(a.Model.DestPercentArmorValue - b.Model.DestPercentArmorValue);
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
                    totalHP += alteration.Model.SourcePercentMaxHPValue * totalHP
                                + alteration.Model.DestPercentMaxHPValue * totalHP;
                else
                    totalHP += alteration.Model.DestPercentMaxHPValue * totalHP;
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
            BaseMaxHP = 50;
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
        public void MoveForward(GameTime time)
        {
            MoveTowards(Direction, (float)time.ElapsedGameTime.TotalSeconds, GetMoveSpeed());
        }
        /// <summary>
        /// Avance dans la direction donnée, à la vitesse du personnage,
        /// pendant la durée donnée (en secondes).
        /// </summary>
        public void MoveTowards(Vector2 direction, float duration, float speed)
        {
            // Si l'entité est rootée, le mouvement est impossible.
            if (IsRooted)
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
                int left = (int)m_position.X;
                int right = (int)(m_position.X) + 1;
                int top = (int)m_position.Y;
                int bottom = (int)(m_position.Y) + 1;

                // On limite le mouvement au bord de la case.
                newDst.X = Math.Min(right - 0.02f, Math.Max(left+0.02f, dst.X));
                newDst.Y = Math.Min(bottom - 0.02f, Math.Max(top+0.02f, dst.Y));

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

            // Apply state alterations
            ApplyStateAlterations(time);

            // Mise à jour du mouvement
            UpdateMoveTo(time);

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
            HP = GetMaxHP();
            m_stateAlterations.EndAlterations(StateAlterationSource.SpellActive);
            m_stateAlterations.EndAlterations(StateAlterationSource.SpellPassive);
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

                // Applique les dégâts de base du sort + dégâts bonus fonction de l'attaque de la source.
                ApplyTrueDamage(alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All));
            }

            // Applique les soins
            List<StateAlteration> healAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.Heal);
            foreach(StateAlteration alteration in healAlterations)
            {
                ApplyHeal(alteration.Model.GetValue(alteration.Source, this, ScalingRatios.All));
            }

            // Applique les dash
            List<StateAlteration> dashAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.Dash);
            foreach(StateAlteration alteration in dashAlterations)
            {
                ApplyDash(alteration.Model.GetDashDirection(alteration.Source, this, alteration.Parameters), 
                    alteration.Model.DashSpeed,
                    alteration.RemainingTime,
                    (float)time.ElapsedGameTime.TotalSeconds,
                    alteration.Model.DashGoThroughWall);
            }
        }

        /// <summary>
        /// Applique l'altération de dash passée en paramètre.
        /// </summary>
        public void ApplyDash(Vector2 direction, float speed, float remainingDuration, float stepDuration, bool goThroughWalls)
        {
            // Détermine si la direction finale est praticable.
            Vector2 finalPosition = Position + direction * speed * remainingDuration;

            if(float.IsNaN(finalPosition.X) || float.IsNaN(finalPosition.Y))
                return;
            

            // Dash : on ignore les murs tant que la destination est praticable, si elle ne l'est pas,
            // on cogne.
            if (goThroughWalls && GameServer.GetMap().GetPassabilityAt(finalPosition.X, finalPosition.Y))
            {
                Position += direction * speed * stepDuration;
            }
            else
            {
                MoveTowards(direction, stepDuration, speed);
            }

            // Arrête le déplacement en cours après le dash.
            EndMoveTo();
        }
        /// <summary>
        /// Ajoute une altération d'état à cette entité.
        /// </summary>
        public void AddAlteration(StateAlteration alteration)
        {
            // Notifications au système de récompenses.
            switch(alteration.Model.Type)
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
            m_stateAlterations.Add(alteration);
        }

        /// <summary>
        /// Applique le nombre de dégâts indiqué à cette entité.
        /// Cette fonction prend en compte l'armure de l'entité pour déterminer
        /// les dégâts réellement infligés.
        /// </summary>
        /// <returns>Retourne le nombre de dégâts bruts infligés.</returns>
        protected virtual float ApplyAttackDamage(float damage)
        {
            float trueDamages = (damage * 100 / (100 + GetArmor()));
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
        /// <summary>
        /// Dessine l'entitié.
        /// </summary>
        public void Draw(GameTime time, RemoteSpriteBatch batch)
        {
            Point scroll = GameServer.GetMap().Scrolling;
            Point drawPos = new Point((int)(m_position.X * GameServer.GetMap().UnitSize) - scroll.X, (int)(m_position.Y * GameServer.GetMap().UnitSize) - scroll.Y);

            if (drawPos.X > GameServer.GetMap().Viewport.Right || drawPos.Y > GameServer.GetMap().Viewport.Bottom 
                || drawPos.X < GameServer.GetMap().Viewport.Left - GameServer.GetMap().UnitSize || drawPos.Y < GameServer.GetMap().Viewport.Top - GameServer.GetMap().UnitSize)
                return;


            Draw(time, batch, drawPos);
        }

        protected float __angle;
        /// <summary>
        /// Dessine l'entité à la position donnée.
        /// 
        /// Cette méthode doit être réécrite pour chaque type d'entité.
        /// </summary>
        /// <param name="time">Temps de jeu.</param>
        /// <param name="batch">Batch sur lequel dessiner.</param>
        /// <param name="position">Position à laquelle dessiner l'unité.</param>
        public virtual void Draw(GameTime time, RemoteSpriteBatch batch, Point position)
        {
            Color col;
            if (Type.HasFlag(EntityType.Team1))
                col = Color.Blue;
            else if (Type.HasFlag(EntityType.Team2))
                col = Color.Red;
            else
                col = Color.White;

            RemoteTexture2D tex = Ressources.DummyTexture;
            if (Type.HasFlag(EntityType.Tower))
                tex = Ressources.SelectMark;
            else if (Type.HasFlag(EntityType.Spawner))
                tex = Ressources.TextBox;
            else if (Type.HasFlag(EntityType.WardPlacement))
                tex = Ressources.SelectMark;

            int s = GameServer.GetMap().UnitSize / 2;
            if (Type.HasFlag(EntityType.Checkpoint))
                s /= 4;

            col.A = (byte)((GameServer.GetMap().Vision.HasVision((Type & EntityType.Teams) ^ EntityType.Teams, Position)) ? 255 : 100);
            batch.Draw(tex, 
                new Rectangle(position.X, position.Y, s, s), null, col, __angle, new Vector2(s, s), SpriteEffects.None, 0.0f);
        }

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
        #endregion

    }
}
