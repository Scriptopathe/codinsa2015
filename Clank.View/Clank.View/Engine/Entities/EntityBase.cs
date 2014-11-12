using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine.Shapes;
namespace Clank.View.Engine.Entities
{
    
    /// <summary>
    /// Classe de base pour les entités in-game.
    /// 
    /// 
    /// </summary>
    public class EntityBase
    {
        #region Static 
        public static int EntityCount = 0;
        public static void ResetEntityCount()
        {
            EntityCount = 0;
        }
        #endregion
        #region Variables

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
        float m_speed;
        /// <summary>
        /// Représente le type de l'entité.
        /// </summary>
        EntityType m_type;
        /// <summary>
        /// Retourne toutes les altérations d'état
        /// </summary>
        StateAlterationCollection m_stateAlterations;
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Représente les points d'armure de base de cette entité.
        /// Les points d'armure réduisent les dégâts infligés à cette unité selon la formule :
        /// dégats infligés = dmg * 100 / (100+armor)
        /// </summary>
        public float BaseArmor
        {
            get { return m_baseArmor; }
            set { m_baseArmor = value; }
        }
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
        public float ShieldPoints
        {
            get { return m_shieldPoints; }
            set { m_shieldPoints = value; }
        }

        /// <summary>
        /// Obtient les points de vie actuels de l'entité.
        /// </summary>
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
        public float BaseMaxHP
        {
            get { return m_baseMaxHP; }
            set { m_baseMaxHP = value; }
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
        public EntityType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// Obtient l'id de cette entité.
        /// </summary>
        public int ID
        {
            get;
            protected set;
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si cette entité doit 
        /// être supprimée de la map.
        /// </summary>
        public bool IsDisposed
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit les points d'attaque de base de cette unité.
        /// </summary>
        public float BaseAttackDamage
        {
            get { return m_baseAttackDamage; }
            set { m_baseAttackDamage = value; }
        }

        /// <summary>
        /// Fonction utilisée pour obtenir les points d'attaque effectifs de cette entité.
        /// </summary>
        public virtual float GetAttackDamage()
        {
            float totalDamage = BaseAttackDamage;

            // Récupère tous les buffs d'attaque.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.DamageBuff);
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
                    totalDamage += alteration.Model.CalculateValue(alteration.Source, this, ScalingRatios.All ^ ScalingRatios.DstAd);
                else
                    totalDamage += alteration.Model.CalculateValue(alteration.Source, this, ScalingRatios.All ^ (ScalingRatios.DstAd | ScalingRatios.SrcAd));
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
        public virtual float GetArmor()
        {
            float totalArmor = BaseArmor;

            // Récupère tous les buffs d'armure.
            List<StateAlteration> alterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.Armor);
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
                    totalArmor += alteration.Model.CalculateValue(alteration.Source, this, ScalingRatios.All ^ ScalingRatios.DstArmor);
                else
                    totalArmor += alteration.Model.CalculateValue(alteration.Source, this, ScalingRatios.All ^ (ScalingRatios.SrcArmor | ScalingRatios.DstArmor));
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
        public virtual float GetHP()
        {
            return HP;
        }

        /// <summary>
        /// Obtient les HP max actuels de cette entité.
        /// </summary>
        /// <returns></returns>
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
                    totalHP += alteration.Model.CalculateValue(alteration.Source, this, ScalingRatios.All ^ ScalingRatios.DstMaxHP);
                else
                    totalHP += alteration.Model.CalculateValue(alteration.Source, this, ScalingRatios.All ^ (ScalingRatios.SrcMaxHP | ScalingRatios.DstMaxHP));
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
        #region State
        /// <summary>
        /// Obtient une valeur indiquant si cette entité est Rootée. (ne peut plus bouger).
        /// </summary>
        public bool IsRooted
        {
            get { return m_stateAlterations.GetInteractionsByType(StateAlterationType.Root).Count != 0; }
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

            // TODO : supprimer ces lignes, seulement utiles pour le debug.
            m_shape = new RectangleShape(Vector2.Zero, new Vector2(0.5f, 0.5f));
            m_speed = 8.0f;
        }

        /// <summary>
        /// Avance dans la direction du personnage, à la vitesse du personnage,
        /// pendant le temps écoulé durant la frame précédente.
        /// </summary>
        public void MoveForward(GameTime time)
        {
            // Si l'entité est rootée, le mouvement est impossible.
            if (IsRooted)
                return;

            // Stratégie :
            // on regarde si à vitesse * time.elapsed * direction + Position, on est dans une case invalide
            // si c'est le cas, on se place sur une extrémité de la case.
            float length = m_speed * (float)time.ElapsedGameTime.TotalSeconds; 
            Vector2 dst = length * Direction + Position;

            if (length < 1)
                MoveForwardStep(dst);
            else
            {
                // Ici, on gère les "grandes" vitesse, càd, celles supérieures à une unité métrique.
                float step = 1.0f;
                bool stop = false;
                while(true)
                {
                    // On avance d'un pas de taille max d'une unité métrique.
                    stop = !MoveForwardStep(step * Direction + Position);

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
        bool MoveForwardStep(Vector2 dst)
        {
            Vector2 newDst;
            bool dstOK = Mobattack.GetMap().GetPassabilityAt(dst.X, dst.Y);
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
            // Apply state alterations
            ApplyStateAlterations();

            // Mets à jour les altérations d'état.
            m_stateAlterations.UpdateStateAlterations(time);
        }

        #region Alterations
        /// <summary>
        /// Applique les altérations d'état en cours.
        /// </summary>
        protected virtual void ApplyStateAlterations()
        {
            // Applique les dégâts AD
            List<StateAlteration> attackDamageAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.AttackDamage);
            foreach(StateAlteration alteration in attackDamageAlterations)
            {
                // Applique les dégâts de base du sort + dégâts bonus fonction de l'attaque de la source.
                ApplyDamage(alteration.Model.CalculateValue(alteration.Source, this, ScalingRatios.All));
            }

            // Applique les dégâts bruts.
            List<StateAlteration> trueDamageAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.TrueDamage);
            foreach(StateAlteration alteration in trueDamageAlterations)
            {
                // Applique les dégâts de base du sort + dégâts bonus fonction de l'attaque de la source.
                ApplyTrueDamage(alteration.Model.CalculateValue(alteration.Source, this, ScalingRatios.All));
            }

            // Applique les soins
            List<StateAlteration> healAlterations = m_stateAlterations.GetInteractionsByType(StateAlterationType.Heal);
            foreach(StateAlteration alteration in healAlterations)
            {
                ApplyHeal(alteration.Model.CalculateValue(alteration.Source, this, ScalingRatios.All));
            }

            
        }

        /// <summary>
        /// Applique le nombre de dégâts indiqué à cette entité.
        /// Cette fonction prend en compte l'armure de l'entité pour déterminer
        /// les dégâts réellement infligés.
        /// </summary>
        /// <param name="damage"></param>
        protected virtual void ApplyDamage(float damage)
        {
            float trueDamages = (damage * 100 / (100 + GetArmor()));
            ApplyTrueDamage(trueDamages);
        }
        /// <summary>
        /// Applique le nombre de dégâts bruts indiqué à cette entité.
        /// </summary>
        protected virtual void ApplyTrueDamage(float damage)
        {
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
        #endregion
        /// <summary>
        /// Dessine l'entitié.
        /// </summary>
        public void Draw(GameTime time, SpriteBatch batch)
        {
            Point scroll = Mobattack.GetMap().Scrolling;
            Point drawPos = new Point((int)(m_position.X * Map.UnitSize) - scroll.X, (int)(m_position.Y * Map.UnitSize) - scroll.Y);

            Draw(time, batch, drawPos);
        }

        /// <summary>
        /// Dessine l'entité à la position donnée.
        /// 
        /// Cette méthode doit être réécrite pour chaque type d'entité.
        /// </summary>
        /// <param name="time">Temps de jeu.</param>
        /// <param name="batch">Batch sur lequel dessiner.</param>
        /// <param name="position">Position à laquelle dessiner l'unité.</param>
        public virtual void Draw(GameTime time, SpriteBatch batch, Point position)
        {
            batch.Draw(Ressources.DummyTexture, new Rectangle(position.X, position.Y, 16, 16), null, Color.White, __angle, new Vector2(16, 16), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Supprime cette entité de la map.
        /// </summary>
        public void Dispose()
        {
            IsDisposed = true;
        }
        #endregion

        #region Debug
        float __angle = 0; // DEBUG
        void __UpdateDebug(GameTime time)
        {
            // DEBUG
            if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Q))
                __angle -= 0.05f;
            else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.D))
                __angle += 0.05f;

            Direction = new Vector2((float)Math.Cos(__angle), (float)Math.Sin(__angle));

            if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Z))
                MoveForward(time);
            // -----
        }
        #endregion
    }
}
