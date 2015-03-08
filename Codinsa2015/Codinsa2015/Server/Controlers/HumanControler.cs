using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server;
using Codinsa2015.Server.Spellcasts;
using Codinsa2015.Server.Spells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Graphics.Server;
using Codinsa2015.Graphics.Client;
namespace Codinsa2015.Server.Controlers
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

        /// <summary>
        /// Indique si le contrôleur doit capturer la souris (l'empêcher de sortir des bords + scrolling).
        /// </summary>
        bool m_captureMouse = true;
        #endregion

        #region Properties
        public int ScrollSpeed = 16;

        /// <summary>
        /// Fenêtre invisible permettant de déterminer si le jeu a peut récupérer les entrées
        /// clavier / souris, ou si elles sont dédiées à un autre composant de la GUI.
        /// </summary>
        public EnhancedGui.GuiWindow GameWindow { get; set; }
        /// <summary>
        /// Obtient ou définit le héros contrôlé.
        /// </summary>
        public override EntityHero Hero
        {
            get { return m_hero; }
            set { m_hero = value; }
        }

        /// <summary>
        /// Obtient le point de vue de ce contrôleur sur la map.
        /// </summary>
        public Map.PointOfView Pov
        {
            get;
            set;
        }


        /// <summary>
        /// Obtient une référence vers la map contrôlée.
        /// </summary>
        public Map Map
        {
            get { return GameServer.GetScene().Map; }
        }

        /// <summary>
        /// Représente le contrôleur d'édition de la map.
        /// </summary>
        Editor.MapEditorControler MapEditControler
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient le client graphique que ce contrôleur utilise pour afficher la scène.
        /// </summary>
        public GraphicsClient GraphicsClient
        {
            get;
            set;
        }


        /// <summary>
        /// Obtient une valeur indiquant si le contrôleur est en mode édition de map.
        /// </summary>
        public bool EditMode
        {
            get;
            set;
        }
        #endregion

        #region Methods

        #region Init
        /// <summary>
        /// Crée un nouveau contrôleur ayant le contrôle sur le héros donné.
        /// </summary>
        /// <param name="hero"></param>
        public HumanControler(EntityHero hero) : base(hero)
        {
            EditMode = false;
            m_hero = hero;
            Pov = new Server.Map.PointOfView() { Teams = hero.Type, UnitSize = 32 };
            Particles = new Server.Particles.ParticleManager();
            EnhancedGuiManager = new EnhancedGui.GuiManager();
            MapEditControler = new Editor.MapEditorControler(this);
        }

        /// <summary>
        /// Lie le client graphique (si existant) du contrôleur au serveur donné.
        /// </summary>
        public override void BindGraphicsClient(GraphicsServer server)
        {
            GraphicsClient = new Codinsa2015.Graphics.Client.IntegratedClient(GameServer.Instance.GraphicsDevice, GameServer.Instance.Content);
            GameServer.GetScene().GraphicsServer.CommandIssued += GraphicsClient.ProcessCommand;
        }
        /// <summary>
        /// Charges les ressources (graphiques et autres) dont a besoin de contrôleur.
        /// </summary>
        public override void LoadContent()
        {
            GameWindow = new EnhancedGui.GuiWindow(EnhancedGuiManager);
            GameWindow.IsHiden = true;
            GameWindow.BackColor = new Color(0, 0, 0, 50);
            GameWindow.Layer = -1;
            GameWindow.IsMoveable = false;

            MapEditControler.LoadContent();
            MapEditControler.CurrentMap = GameServer.GetScene().Map;
            MapEditControler.OnMapLoaded += GameServer.GetScene().Map.Load;
        }
        #endregion

        #region Update
        int __oldScroll;
        Point __oldPos;
        /// <summary>
        /// Mets à jour l'état de ce contrôleur, et lui permet d'envoyer des commandes au héros.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(GameTime time)
        {
            if(GameServer.GetScene().Mode == SceneMode.Game)
            {
                GameWindow.IsVisible = true;
                GameWindow.Area = Map.Viewport;

                // Passage du mode d'édition au mode normal.
                if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.LeftControl) && !Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                    EditMode = !EditMode;

                // Toogle de la capture de la souris.
                if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.RightControl))
                    m_captureMouse = !m_captureMouse;


                // Capture de la souris + scrolling.
                if (m_captureMouse)
                    UpdateMouseScrolling();

                // Change le point de vue de la map.
                Map.Pov = Pov;

                // Mise à jour du contrôleur de la map.
                MapEditControler.IsEnabled = EditMode;
                MapEditControler.Update(time);

                // Particules / etc
                Particles.Update(time);

                // Gui manager
                EnhancedGuiManager.Update(time);

                if (GameWindow.HasFocus())
                {
                    GameWindow.IsHiden = true;
                    UpdateGameInput();
                }
                else
                    GameWindow.IsHiden = false;


                var ms = Input.GetMouseState();
                __oldScroll = ms.ScrollWheelValue;
            }
            else
            {
                GameWindow.IsVisible = false;
                // MODE : PICK
                var ms = Input.GetMouseState();
                GameServer.GetScene().PickControler.SetVirtualMousePos(new Vector2(ms.X, ms.Y));

                if (Input.IsLeftClickTrigger())
                    GameServer.GetScene().PickControler.VirtualMouseClick(Hero.ID);

            }
            
        }

        /// <summary>
        /// Mets à jour le scrolling en fonction de la position de la souris.
        /// </summary>
        void UpdateMouseScrolling()
        {
            // Récupère la position de la souris, et la garde sur le bord.
            Vector2 position = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            Vector2 position2 = Vector2.Max(Vector2.Zero, Vector2.Min(GameServer.GetScreenSize(), position));
            if(position != position2)
                Input.SetMousePosition((int)position2.X, (int)position2.Y);
            
            // Fait bouger l'écran quand on est au bord.
            if (position.X <= 10)
                MapEditControler.CurrentMap.ScrollingVector2 = new Vector2(MapEditControler.CurrentMap.ScrollingVector2.X - ScrollSpeed, MapEditControler.CurrentMap.ScrollingVector2.Y);
            else if (position.X >= GameServer.GetScreenSize().X - 10)
                MapEditControler.CurrentMap.ScrollingVector2 = new Vector2(MapEditControler.CurrentMap.ScrollingVector2.X + ScrollSpeed, MapEditControler.CurrentMap.ScrollingVector2.Y);
            if (position.Y <= 10)
                MapEditControler.CurrentMap.ScrollingVector2 = new Vector2(MapEditControler.CurrentMap.ScrollingVector2.X, MapEditControler.CurrentMap.ScrollingVector2.Y - ScrollSpeed);
            else if (position.Y >= GameServer.GetScreenSize().Y - 10)
                MapEditControler.CurrentMap.ScrollingVector2 = new Vector2(MapEditControler.CurrentMap.ScrollingVector2.X, MapEditControler.CurrentMap.ScrollingVector2.Y + ScrollSpeed);
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
        /// Mets à jour l'état du contrôleur en fonction des entrées du joueur humain.
        /// </summary>
        void UpdateGameInput()
        {
            UpdateHeroMovement();
            UpdateSpells();
            UpdateConsummable();
        }

        /// <summary>
        /// Mets à jour le déplacement du héros en fonction des entrées du joueur humain.
        /// </summary>
        void UpdateHeroMovement()
        {
            // Mouvement du héros.
            var ms = Input.GetMouseState();
            Vector2 pos = GameServer.GetMap().ToMapSpace(new Vector2(ms.X, ms.Y));
            if (Input.IsRightClickPressed() && GameServer.GetMap().GetPassabilityAt(pos))
            {

                float dst = Vector2.Distance(pos, Hero.Position);
                // Obtient les entités targettables par le héros à portée.
                var entities = GameServer.GetMap().Entities.GetEntitiesInSight(Hero.Type).
                    GetAliveEntitiesInRange(pos, 1).Where(delegate(KeyValuePair<int, EntityBase> kvp)
                    {
                        EntityType ennemyteam = (Hero.Type & EntityType.Teams) ^ EntityType.Teams;
                        return kvp.Value.Type.HasFlag(ennemyteam) || EntityType.AllTargettableNeutral.HasFlag(kvp.Value.Type);
                    }).ToList();

                // Utilise l'arme sur le héros.
                if (entities.Count != 0)
                {
                    if (Hero.Weapon.Use(Hero, entities.First().Value) == SpellUseResult.Success)
                    {
                        m_hero.EndMoveTo();
                        m_hero.Path = null;
                    }
                }

                // Si on effectue une auto attaque
                if (entities.Count == 0 ||
                    dst > Hero.Weapon.GetAttackSpell().TargetType.Range)
                    m_hero.Path = new Trajectory(new List<Vector2>() { pos });


            }

            if (m_hero.Path != null && m_hero.IsBlockedByWall)
            {
                m_hero.StartMoveTo(m_hero.Path.LastPosition());
            }

            // Vision range
            if (ms.ScrollWheelValue - __oldScroll < 0)
                m_hero.VisionRange--;
            else if (ms.ScrollWheelValue - __oldScroll > 0)
                m_hero.VisionRange++;
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
            Vector2 pos = GameServer.GetMap().ToMapSpace(new Vector2(ms.X, ms.Y));

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
                        EntityBase entityDst = GameServer.GetMap().Entities.GetAliveEntitiesInRange(pos, 1.0f).GetEntitiesInSight(m_hero.Type).NearestFrom(pos);
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
        #endregion

        #region Draw

        #region GUI and stuff
        const int spellIconSize = 64;
        const int padding = 4;
        /// <summary>
        /// Dessine les icones des spells.
        /// </summary>
        void DrawSpellIcons(RemoteSpriteBatch batch, GameTime time)
        {
            int spellCount = m_hero.Spells.Count;
            int y = (int)GameServer.GetScreenSize().Y - spellIconSize - 5;
            int xBase = ((int)GameServer.GetScreenSize().X - ((spellIconSize + padding) * spellCount)) / 2;
            for (int i = 0; i < spellCount; i++)
            {
                bool isOnCooldown = m_hero.Spells[i].CurrentCooldown > 0;
                int x = xBase + i * (spellIconSize + padding);

                // Dessine l'icone du sort
                Color col = isOnCooldown ? Color.Gray : Color.White;
                batch.Draw(Ressources.GetSpellTexture(m_hero.Spells[i].Name),
                           new Rectangle(x, y, spellIconSize, spellIconSize), null, col, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.HeroControler);

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

                    batch.DrawString(Ressources.Font, cooldown, new Vector2(x + offsetX, y + offsetY), Color.Black, 0.0f, Vector2.Zero, 1.0f, GraphicsHelpers.Z.HeroControler + GraphicsHelpers.Z.FrontStep);
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
            int y = (int)GameServer.GetScreenSize().Y - spellIconSize/2 - 5;
            int xBase = ((int)GameServer.GetScreenSize().X - ((spellIconSize + padding) * m_hero.Spells.Count)) / 2;
            int size = spellIconSize / 2;
            xBase -= (size + padding) * m_hero.Consummables.Length;
            for (int i = 0; i < m_hero.Consummables.Length; i++)
            {
                Color col = m_hero.Consummables[i].UsingStarted ? Color.Gray : Color.White;

                // Dessine le slot du consommable.
                batch.Draw(Ressources.GetSpellTexture(m_hero.Consummables[i].Model.Name),
                    new Rectangle(xBase, y, size, size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.HeroControler);

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
            int y = (int)GameServer.GetScreenSize().Y - spellIconSize - 5;
            int xBase = ((int)GameServer.GetScreenSize().X - ((spellIconSize + padding) * spellCount)) / 2;
            int size = spellIconSize / 2;
            xBase -= (size + padding) * m_hero.Consummables.Length;
            for (int i = 0; i < m_hero.Consummables.Length;i++)
            {
                Color col = m_hero.Consummables[i].UsingStarted ? Color.Gray : Color.White;

                // Dessine le slot du consommable.
                batch.Draw(Ressources.GetSpellTexture(m_hero.Consummables[i].Model.ConsummableType.ToString()),
                    new Rectangle(xBase, y, size, size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.HeroControler);

                // Affiche le nombre de consommables.
                if(m_hero.Consummables[i].Count >= 1)
                {
                    batch.DrawString(Ressources.Font, "x" + m_hero.Consummables[i].Count, new Vector2(xBase + size - 12, y + size - 12),
                        Color.Black, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, GraphicsHelpers.Z.HeroControler + GraphicsHelpers.Z.FrontStep);
                }

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

                    batch.DrawString(Ressources.Font, cooldown, new Vector2(xBase + offsetX, y + offsetY), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GraphicsHelpers.Z.HeroControler + GraphicsHelpers.Z.FrontStep);
                }


                xBase += size + padding;
            }



        }

        #endregion


        /// <summary>
        /// Dessine les éléments graphiques du contrôleur à l'écran.
        /// </summary>
        public override void Draw(RemoteSpriteBatch batch, GameTime time)
        {
            if(GameServer.GetScene().Mode == SceneMode.Game)
            {
                // Change le point de vue de la map.
                Map.Pov = Pov;

                // Dessine la map sur le main render target.
                MapEditControler.CurrentMap.Draw(time, batch);


                // Dessine les GUI, particules etc...
                batch.GraphicsDevice.SetRenderTarget(GameServer.GetScene().MainRenderTarget);
                batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                EnhancedGuiManager.Draw(batch);
                Particles.Draw(batch, new Vector2(Map.Viewport.X, Map.Viewport.Y), Map.ScrollingVector2);
                DrawControlerGUI(batch, time);
                batch.End();


                // Dessine le render target principal sur le back buffer.
                batch.GraphicsDevice.SetRenderTarget(null);
                batch.Begin();
                batch.Draw(GameServer.GetScene().MainRenderTarget, Vector2.Zero, Color.White);
                batch.End();
            }
            else
            {
                // PICKS
                var ms = Input.GetMouseState();
                batch.Begin();
                batch.Draw(Ressources.Cursor, new Rectangle((int)ms.X, (int)ms.Y, 32, 32), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.Front);
                batch.End();
            }

        }

        /// <summary>
        /// Dessine les éléments de GUI du contrôleur.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="time"></param>
        void DrawControlerGUI(RemoteSpriteBatch batch, GameTime time)
        {
            MapEditControler.Draw(batch);
            DrawSpellIcons(batch, time);
            DrawConsummableSlots(batch, time);
            DrawEquipmentSlots(batch, time);
        }
        #endregion

        
        #endregion
    }
}
