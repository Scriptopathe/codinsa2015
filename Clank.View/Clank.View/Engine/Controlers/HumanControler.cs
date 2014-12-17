using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.View.Engine.Entities;
using Clank.View.Engine;
using Clank.View.Engine.Spellcasts;
using Clank.View.Engine.Spells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine.Graphics.Server;
namespace Clank.View.Engine.Controlers
{
    /// <summary>
    /// Classe abstraite de contrôleur.
    /// 
    /// Un contrôleur permet de contrôler un seul héros.
    /// </summary>
    public class HumanControler : ControlerBase
    {
        #region Variables
        /// <summary>
        /// Héros contrôlé par cette instance de contrôleur.
        /// </summary>
        EntityHero m_hero;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit le héros contrôlé.
        /// </summary>
        public override EntityHero Hero
        {
            get { return m_hero; }
            set { m_hero = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée un nouveau contrôleur ayant le contrôle sur le héros donné.
        /// </summary>
        /// <param name="hero"></param>
        public HumanControler(EntityHero hero) : base(hero)
        {
            m_hero = hero;
        }

        int __oldScroll;
        Point __oldPos;
        /// <summary>
        /// Mets à jour l'état de ce contrôleur, et lui permet d'envoyer des commandes au héros.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(GameTime time)
        {
            // Mouvement du héros.
            var ms = Input.GetMouseState();
            Vector2 pos = Mobattack.GetMap().ToMapSpace(new Vector2(ms.X, ms.Y));
            if(Input.IsRightClickPressed() && Mobattack.GetMap().GetPassabilityAt(pos))
            {
                m_hero.Path = new Trajectory(new List<Vector2>() { pos });
            }

            if (m_hero.Path != null && m_hero.IsBlockedByWall)
            {
                m_hero.StartMoveTo(m_hero.Path.LastPosition());
            }

            if (ms.ScrollWheelValue - __oldScroll < 0)
                m_hero.VisionRange--;
            else if (ms.ScrollWheelValue - __oldScroll> 0)
                m_hero.VisionRange++;

            // Mise à jour des spells.
            UpdateSpells();

            // Utilisation des consommables.
            UpdateConsummable();

            __oldScroll = ms.ScrollWheelValue;
        }
        
        /// <summary>
        /// Détermine si le joueur a appuyé sur une touche pour warder, et effectue l'action
        /// dans ce cas.
        /// </summary>
        void UpdateConsummable()
        {
            if(Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.D1))
            {
                m_hero.UseConsummable(0);
            }
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.D2))
            {
                m_hero.UseConsummable(1);
            }
        }
        /// <summary>
        /// Détermine si le joueur a appuyé sur des touches pour lancer des sorts.
        /// </summary>
        void UpdateSpells()
        {
            int spellUsed = -1;
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.A))
                spellUsed = 0;
            else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Z))
                spellUsed = 1;
            else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.E))
                spellUsed = 2;
            else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.R))
                spellUsed = 3;

            var ms = Input.GetMouseState();
            Vector2 pos = Mobattack.GetMap().ToMapSpace(new Vector2(ms.X, ms.Y));

            // Utilise le sort correspondant.
            if(spellUsed != -1 && m_hero.Spells.Count > spellUsed)
            {
                Spell spell = m_hero.Spells[spellUsed];

                if (spell.Description.TargetType.Type == TargettingType.Targetted)
                {
                    if(spell.Description.TargetType.AllowedTargetTypes == EntityTypeRelative.Me)
                    {
                        EntityBase entityDst = m_hero;
                        Vector2 dir = pos - m_hero.Position; dir.Normalize();
                        spell.Use(new SpellCastTargetInfo()
                        {
                            Type = TargettingType.Targetted,
                            TargetId = m_hero.ID,
                            AlterationParameters = new StateAlterationParameters()
                            {
                                DashTargetDirection = dir,
                            }
                        });
                    }
                    else
                    {
                        EntityBase entityDst = Mobattack.GetMap().Entities.GetAliveEntitiesInRange(pos, 1.0f).GetEntitiesInSight(m_hero.Type).NearestFrom(pos);
                        if (entityDst != null)
                            spell.Use(new SpellCastTargetInfo() { TargetId = entityDst.ID, Type = TargettingType.Targetted });
                    }
                }
                else if(spell.Description.TargetType.Type == TargettingType.Direction)
                {
                    Vector2 dir = pos - m_hero.Position; dir.Normalize();
                    spell.Use(new SpellCastTargetInfo() { Type = TargettingType.Direction, TargetDirection = dir });
                }
                else if(spell.Description.TargetType.Type == TargettingType.Position)
                {
                    spell.Use(new SpellCastTargetInfo() { Type = TargettingType.Position, TargetPosition = pos });
                }
            }
        }

        #region Draw
        const int spellIconSize = 64;
        const int padding = 4;
        /// <summary>
        /// Dessine les icones des spells.
        /// </summary>
        void DrawSpellIcons(RemoteSpriteBatch batch, GameTime time)
        {
            int spellCount = m_hero.Spells.Count;
            int y = (int)Mobattack.GetScreenSize().Y - spellIconSize - 5;
            int xBase = ((int)Mobattack.GetScreenSize().X - ((spellIconSize + padding) * spellCount)) / 2;
            for (int i = 0; i < spellCount; i++)
            {
                bool isOnCooldown = m_hero.Spells[i].CurrentCooldown > 0;
                int x = xBase + i * (spellIconSize + padding);

                // Dessine l'icone du sort
                Color col = isOnCooldown ? Color.Gray : Color.White;
                batch.Draw(Ressources.GetSpellTexture(m_hero.Spells[i].Name),
                           new Rectangle(x, y, spellIconSize, spellIconSize), null, col, 0.0f, Vector2.Zero, SpriteEffects.None, Graphics.Z.HeroControler);

                // Dessine le cooldown du sort.
                if (isOnCooldown)
                {
                    string cooldown;
                    if (m_hero.Spells[i].CurrentCooldown <= 1)
                        cooldown = (m_hero.Spells[i].CurrentCooldown).ToString("f1");
                    else
                        cooldown = (m_hero.Spells[i].CurrentCooldown).ToString("f0");

                    Vector2 stringW = Ressources.Font.MeasureString(cooldown);

                    int offsetX = (spellIconSize - (int)stringW.X) / 2;
                    int offsetY = (spellIconSize - (int)stringW.Y) / 2;

                    batch.DrawString(Ressources.Font, cooldown, new Vector2(x + offsetX, y + offsetY), Color.Black, 0.0f, Vector2.Zero, 1.0f, Graphics.Z.HeroControler + Graphics.Z.FrontStep);
                }

            }
        }
        
        /// <summary>
        /// Dessine les slots des équipements (armures, armes, bottes).
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="time"></param>
        void DrawEquipmentSlots(RemoteSpriteBatch batch, GameTime time)
        {
            Equip.Equipment[] equip = new Equip.Equipment[] { m_hero.Weapon, m_hero.Armor };
            int y = (int)Mobattack.GetScreenSize().Y - spellIconSize/2 - 5;
            int xBase = ((int)Mobattack.GetScreenSize().X - ((spellIconSize + padding) * m_hero.Spells.Count)) / 2;
            int size = spellIconSize / 2;
            xBase -= (size + padding) * m_hero.Consummables.Length;
            for (int i = 0; i < m_hero.Consummables.Length; i++)
            {
                Color col = m_hero.Consummables[i].UsingStarted ? Color.Gray : Color.White;

                // Dessine le slot du consommable.
                batch.Draw(Ressources.GetSpellTexture(equip[i].Name),
                    new Rectangle(xBase, y, size, size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, Graphics.Z.HeroControler);

                xBase += size + padding;
            }


        }
        /// <summary>
        /// Dessine le slots de consommables.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="time"></param>
        void DrawConsummableSlots(RemoteSpriteBatch batch, GameTime time)
        {
            int spellCount = m_hero.Spells.Count;
            int y = (int)Mobattack.GetScreenSize().Y - spellIconSize - 5;
            int xBase = ((int)Mobattack.GetScreenSize().X - ((spellIconSize + padding) * spellCount)) / 2;
            int size = spellIconSize / 2;
            xBase -= (size + padding) * m_hero.Consummables.Length;
            for (int i = 0; i < m_hero.Consummables.Length;i++)
            {
                Color col = m_hero.Consummables[i].UsingStarted ? Color.Gray : Color.White;

                // Dessine le slot du consommable.
                batch.Draw(Ressources.GetSpellTexture(m_hero.Consummables[i].Type.ToString()),
                    new Rectangle(xBase, y, size, size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, Graphics.Z.HeroControler);

                // Dessine le cooldown.
                if(m_hero.Consummables[i].UsingStarted)
                {
                    string cooldown;
                    if (m_hero.Consummables[i].RemainingTime <= 1)
                        cooldown = (m_hero.Consummables[i].RemainingTime).ToString("f1");
                    else
                        cooldown = (m_hero.Consummables[i].RemainingTime).ToString("f0");

                    Vector2 stringW = Ressources.Font.MeasureString(cooldown);

                    int offsetX = (size - (int)stringW.X) / 2;
                    int offsetY = (size - (int)stringW.Y) / 2;

                    batch.DrawString(Ressources.Font, cooldown, new Vector2(xBase + offsetX, y + offsetY), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, Graphics.Z.HeroControler + Graphics.Z.FrontStep);
                }


                xBase += size + padding;
            }



        }
        /// <summary>
        /// Dessine les éléments graphiques du contrôleur à l'écran.
        /// </summary>
        public override void Draw(RemoteSpriteBatch batch, GameTime time)
        {
            DrawSpellIcons(batch, time);
            DrawConsummableSlots(batch, time);
            DrawEquipmentSlots(batch, time);
        }


        #endregion

        
        #endregion
    }
}
