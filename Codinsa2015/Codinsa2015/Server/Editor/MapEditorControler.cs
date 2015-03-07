using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server;
using Codinsa2015.Server.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Graphics.Server;
using Codinsa2015.Server.Controlers.Components;
namespace Codinsa2015.Server.Editor
{
    /// <summary>
    /// Contrôleur de l'éditeur de map.
    /// </summary>
    public class MapEditorControler
    {
        
        #region Variables
        Map m_map;
        bool m_terraFormingMode = true;
        int m_rowId = 0;
        int m_checkpointId = 0;
        private Gui.GuiButton m_modeButton;
        int m_brushSize = 2;
        Controlers.ControlerBase m_baseControler;
        #region Graphics
        Minimap m_minimap;
        DeveloperConsole m_console;
        #endregion
        #endregion

        #region Events
        public delegate void MapLoadedDelegate(MapFile map);
        public event MapLoadedDelegate OnMapLoaded;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la map en cours.
        /// </summary>
        public Map CurrentMap
        {
            get
            {
                return m_map;
            }
            set
            {
                m_map = value;
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce contrôleur est actuellement activé.
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de MapEditorControler associé à la map donnée.
        /// </summary>
        public MapEditorControler(Controlers.ControlerBase baseControler)
        {
            m_baseControler = baseControler;
            m_minimap = new Minimap();
            m_console = new DeveloperConsole(baseControler.GuiManager);
            
        }


        /// <summary>
        /// Charge les ressources graphiques dont a besoin ce contrôleur.
        /// </summary>
        public void LoadContent()
        {
            m_minimap.LoadContent();
            m_console.LoadContent();
            EnhancedGui.GuiButton btn = new EnhancedGui.GuiButton(m_baseControler.EnhancedGuiManager);
            btn.Area = new Rectangle(100, 100, 200, 200);
            btn.Title = "Back";
            EnhancedGui.GuiButton btn2 = new EnhancedGui.GuiButton(m_baseControler.EnhancedGuiManager);
            btn2.Area = new Rectangle(50, 50, 300, 100);
            btn2.Parent = btn;
            btn2.Title = "Front";
            EnhancedGui.GuiMultilineTextDisplay disp = new EnhancedGui.GuiMultilineTextDisplay(m_baseControler.EnhancedGuiManager);
            disp.Parent = btn2;
            disp.Location = new Point(50, 50);
            disp.Size = new Point(200, 200);
            disp.AppendLine("hahah");
            EnhancedGui.GuiTextInput inp = new EnhancedGui.GuiTextInput(m_baseControler.EnhancedGuiManager);
            inp.Parent = btn2;
            inp.Location = new Point(50, 250);
            inp.Size = new Point(200, 20);
            EnhancedGui.GuiMenu menu = new EnhancedGui.GuiMenu(m_baseControler.EnhancedGuiManager);
            menu.Location = new Point(500, 100);
            menu.Items = new List<EnhancedGui.GuiMenu.GuiMenuItem>()
            {
                new EnhancedGui.GuiMenu.GuiMenuItem("hah"),
                new EnhancedGui.GuiMenu.GuiMenuItem("hihi")
            };
            menu.Focus();
            CreateGui();
        }

  


        /// <summary>
        /// Initialise les composants de l'interface graphique.
        /// </summary>
        void CreateGui()
        {
            m_modeButton = new Gui.GuiButton()
            {
                Position = new Vector2(0, 0),
                Title = "Land",
                Width = 150, 
                Height = 25
            };
            m_modeButton.Clicked += OnChangeMode;
            m_baseControler.GuiManager.AddWidget(m_modeButton);



        }

        /// <summary>
        /// Change le mode du contrôleur.
        /// </summary>
        void OnChangeMode()
        {
            m_terraFormingMode = !m_terraFormingMode;
            m_modeButton.Title = m_terraFormingMode ? "Land" : "Entities";
        }

        /// <summary>
        /// Mets à jour le contrôleur en prenant en compte les entrées utilisateurs.
        /// </summary>
        public void Update(GameTime time)
        {
            if (!IsEnabled)
                return;

            // Focus de la console.
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.OemComma))
                m_console.HasFocus = true;

            if (m_console.HasFocus)
                return;

            // Affichage de la minimap.
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.M))
                m_minimap.Visible = !m_minimap.Visible;

            // Zoom
            Vector2 mousePosPx = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            Vector2 mousePosUnits = GetMousePosUnits();

            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Add))
            {
                GameServer.GetMap().UnitSize *= 2;
                GameServer.GetMap().ScrollingVector2 *= 2;
            }
            else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Subtract))
            {
                GameServer.GetMap().UnitSize /= 2;
                GameServer.GetMap().ScrollingVector2 /= 2;
            }

            // Terraforming
            if (m_terraFormingMode && mousePosPx.Y > 25)
            { 
                // Ajout de matière
                if (Input.IsLeftClickPressed())
                {
                    for (int i = 0; i < m_brushSize; i++)
                        for (int j = 0; j < m_brushSize; j++)
                            CurrentMap.SetPassabilityAt(mousePosUnits + new Vector2(i, j), false);
                }
                else if (Input.IsRightClickPressed())
                {
                    for (int i = 0; i < m_brushSize; i++)
                        for (int j = 0; j < m_brushSize; j++ )
                            CurrentMap.SetPassabilityAt(mousePosUnits + new Vector2(i, j), true);
                }
                else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.O))
                    m_brushSize++;
                else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.I))
                    m_brushSize--;
            }
            // Edition d'entités.
            else if(!m_terraFormingMode)
            {
                if (Input.IsRightClickTrigger())
                {

                    Entities.EntityCollection entitiesInRange = CurrentMap.Entities.GetAliveEntitiesInRange(mousePosUnits, 1f);

                    Gui.GuiMenu menu = new Gui.GuiMenu();
                    menu.Position = mousePosPx;
                    menu.Title = "Menu";
                    foreach (EntityBase entity in entitiesInRange.Values)
                    {
                        Gui.GuiMenu.GuiMenuItem item = new Gui.GuiMenu.GuiMenuItem("Remove " + entity.Type.ToString());
                        item.ItemSelected += new Gui.GuiMenu.ItemSelectedDelegate(() =>
                        {
                            CurrentMap.Entities.Remove(entity.ID);
                            entity.Dispose();
                        });
                        item.IsEnabled = true;
                        menu.AddItem(item);
                        GameServer.GetScene().CurrentControler.GuiManager.AddWidget(menu);
                    }
                }

                // Ajout d'éléments de jeu.
                Entities.EntityType team = Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Q) ? Entities.EntityType.Team2 : Entities.EntityType.Team1;
                if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.T))
                {
                    AddTower(team, mousePosUnits);
                }
                else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.R))
                {
                    AddSpawner(team, mousePosUnits);
                }
                else if(Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    AddCheckpoint(team, mousePosUnits);
                }
                else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.U))
                {
                    AddWardPlace(mousePosUnits);
                }
                else if(Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    PutWard(team, mousePosUnits);

                }
                if(Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad1))
                {
                    m_checkpointId = 0;
                    m_rowId = 0;
                }
                else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2))
                {
                    m_checkpointId = 0;
                    m_rowId = 1;
                }
                else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad3))
                {
                    m_checkpointId = 0;
                    m_rowId = 2;
                }
                else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4))
                {
                    m_checkpointId = 0;
                    m_rowId = 3;
                }
            }

            // Sauvegarde
            if( Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.M))
            {
                Save();
            }

            // Chargement
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.L))
            {
                Load();
                
            }
        }

        #region API
        /// <summary>
        /// Retourne la position de la souris sur la map en unités métriques.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMousePosUnits()
        {
            Vector2 mousePosPx = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            Vector2 mousePosUnits = ((mousePosPx + CurrentMap.ScrollingVector2) - new Vector2(CurrentMap.Viewport.X, CurrentMap.Viewport.Y)) / GameServer.GetMap().UnitSize;
            return mousePosUnits;
        }

        /// <summary>
        /// Affiche un message d'aide.
        /// </summary>
        public void Help()
        {
            
        }

        public void SetCheckpointId(int id) { m_checkpointId = id; }
        public void SetRowId(int id) { m_rowId = id; m_checkpointId = 0; }
        /// <summary>
        /// Pose une ward sur l'emplacement à proximité de la placementPosition.
        /// </summary>
        /// <param name="team"></param>
        public void PutWard(EntityType team, Vector2 placementPosition)
        {
            EntityWardPlacement p = m_map.Entities.GetEntitiesByType(EntityType.WardPlacement).
                                    GetAliveEntitiesInRange(placementPosition, 1).
                                    NearestFrom(placementPosition) as EntityWardPlacement;
            if (p != null)
            {
                p.PutWard(new EntityHero() { Type = team });
            }
            else
                m_console.Output.AppendLine("Pas d'emplacement où poser la ward !");
        }
        
        /// <summary>
        /// Ajoute un tour pour la team donnée à la position donnée.
        /// </summary>
        /// <param name="team"></param>
        /// <param name="position"></param>
        public void AddTower(EntityType team, Vector2 position)
        {
            Entities.EntityBase entity = new Entities.EntityTower()
            {
                Position = position,
                Type = Entities.EntityType.Tower | team,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
        }
        /// <summary>
        /// Ajoute un emplacement de ward à la position donnée.
        /// </summary>
        /// <param name="position"></param>
        public void AddWardPlace(Vector2 position)
        {
            Entities.EntityBase entity = new EntityWardPlacement()
            {
                Position = position,
                Type = EntityType.WardPlacement,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
        }

        /// <summary>
        /// Ajoute un checkpoint à la position donnée et incrémente le compteur de checkpoint.
        /// </summary>
        public void AddCheckpoint(EntityType team, Vector2 position)
        {
            Entities.EntityBase entity = new Entities.EntityCheckpoint()
            {
                Position = position,
                Type = Entities.EntityType.Checkpoint | team,
                CheckpointID = m_checkpointId,
                CheckpointRow = m_rowId,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
            m_checkpointId++;
        }

        /// <summary>
        /// Ajoute un spawner pour la team donnée à la position donnée.
        /// </summary>
        public void AddSpawner(EntityType team, Vector2 position)
        {
            Entities.EntityBase entity = new Entities.EntitySpawner()
            {
                Position = position,
                SpawnPosition = position,
                Type = Entities.EntityType.Spawner | team,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
        }

        /// <summary>
        /// Ajoute une zone de spawn de héros à la position donnée.
        /// </summary>
        /// <param name="team"></param>
        /// <param name="position"></param>
        public void AddHeroSpawner(EntityType team, Vector2 position)
        {
            Entities.EntityBase entity = new EntityHeroSpawner()
            {
                Position = position,
                Type = EntityType.HeroSpawner | team,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
        }
        /// <summary>
        /// Sauvegarde la map.
        /// </summary>
        public void Save()
        {
            MapFile.Save(CurrentMap);
        }

        /// <summary>
        /// Charge la map.
        /// </summary>
        public void Load()
        {
            if (System.IO.File.Exists(Ressources.MapFilename))
            {
                try
                {
                    MapFile loaded = MapFile.FromFile(Ressources.MapFilename);
                    if (OnMapLoaded != null)
                        OnMapLoaded(loaded);
                }
                /*catch { }*/
                finally { }
            }
        }
        #endregion

        /// <summary>
        /// Dessine les éléments graphiques du contrôleur.
        /// </summary>
        public void Draw(RemoteSpriteBatch batch)
        {
            int ts = GameServer.GetMap().UnitSize;
            // Récupère la position de la souris
            Vector2 position = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            // Dessine le cursor
            batch.Draw(Ressources.Cursor, new Rectangle((int)position.X, (int)position.Y, 32, 32), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.Front);
            
            if (!IsEnabled)
            {
                m_modeButton.Visible = false;
                m_console.Visible = false;
                m_console.HasFocus = false;
            }
            else
            {
                m_modeButton.Visible = true;
                m_console.Visible = true;
            }
            
            // Dessine la case survolée
            var rawpos = GetMousePosUnits();
            rawpos.X -= rawpos.X % 1;
            rawpos.Y -= rawpos.Y % 1;
            var pos = GameServer.GetMap().ToScreenSpace(rawpos);
            batch.Draw(Ressources.HighlightMark, new Rectangle((int)pos.X, (int)pos.Y, ts, ts), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.Front);
            

            // Dessine le bandeau supérieur
            batch.Draw(Ressources.DummyTexture, new Rectangle(0, 0, (int)GameServer.GetScreenSize().X, 25),
                null, new Color(0, 0, 0, 200), 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.GUI + 5 * GraphicsHelpers.Z.BackStep);
            
            // Dessine la minimap
            m_minimap.Position = new Vector2(GameServer.GetScreenSize().X - 200, GameServer.GetScreenSize().Y - 100);
            m_minimap.Size = new Vector2(200, 100);
            m_minimap.Z = GraphicsHelpers.Z.GUI + 2 * GraphicsHelpers.Z.BackStep;
            m_minimap.CurrentMap = CurrentMap;
            m_minimap.Draw(batch);
            
            // Dessine des infos de debug.
            batch.DrawString(Ressources.Font, "RowId = " + m_rowId + " | CheckpointId = " + m_checkpointId + " | Mouse=" + rawpos.ToString(),
                new Vector2(150, 0), Color.White, 0.0f, Vector2.Zero, 1.0f, GraphicsHelpers.Z.GUI + 4 * GraphicsHelpers.Z.BackStep);
        }

        #region External event handlers
        #endregion
        #endregion
    }
}
