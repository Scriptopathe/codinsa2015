using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server;
using Codinsa2015.Server.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Graphics.Server;
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
        bool m_displayMinimap = true;
        private Gui.GuiButton m_modeButton;
        int m_brushSize = 2;
        bool m_minimapDirty = true;
        Gui.GuiTextInput m_consoleInput;
        Gui.GuiMultilineTextDisplay m_consoleOutput;
        Controlers.ControlerBase m_baseControler;
        #region Graphics
        RemoteSpriteBatch m_minimapBatch;
        RemoteRenderTarget m_minimapTexture;
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

                if (m_minimapTexture != null)
                    m_minimapTexture.Dispose();

                m_minimapTexture = new RemoteRenderTarget(GameServer.GetScene().GraphicsServer, value.Size.X, value.Size.Y, RenderTargetUsage.PreserveContents);
                m_map.OnMapModified += m_map_OnMapModified;
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
        }


        /// <summary>
        /// Charge les ressources graphiques dont a besoin ce contrôleur.
        /// </summary>
        public void LoadContent()
        {
            m_minimapBatch = new RemoteSpriteBatch(GameServer.GetScene().GraphicsServer);
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


            // Console input
            m_consoleInput = new Gui.GuiTextInput();
            m_consoleInput.Position = new Vector2(0, GameServer.GetScreenSize().Y - 25);
            m_consoleInput.Size = new Point((int)GameServer.GetScreenSize().X - 200, 25);
            m_consoleInput.TextValidated += m_consoleInput_TextValidated;

            m_baseControler.GuiManager.AddWidget(m_consoleInput);

            // Console output
            m_consoleOutput = new Gui.GuiMultilineTextDisplay();
            m_consoleOutput.Position = new Vector2(0, GameServer.GetScreenSize().Y - 100);
            m_consoleOutput.Size = new Point((int)GameServer.GetScreenSize().X - 200, 75);
            m_baseControler.GuiManager.AddWidget(m_consoleOutput);
            GameServer.GetScene().GameInterpreter.OnPuts = new PonyCarpetExtractor.Interpreter.PutsDelegate((string s) => { m_consoleOutput.AppendLine(s); });
            GameServer.GetScene().GameInterpreter.OnError = new PonyCarpetExtractor.Interpreter.PutsDelegate((string s) => { m_consoleOutput.AppendLine("error: " + s); });
           
        }

        /// <summary>
        /// Se produit lorsqu'une commande est entrée dans la console.
        /// </summary>
        /// <param name="sender"></param>
        void m_consoleInput_TextValidated(Gui.GuiTextInput sender)
        {
            if(sender.Text == "")
                m_consoleInput.HasFocus = false;

            GameServer.GetScene().GameInterpreter.Eval(sender.Text);
            sender.Text = "";
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
                m_consoleInput.HasFocus = true;
            
            if (m_consoleInput.HasFocus)
                return;

            // Affichage de la minimap.
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.M))
                m_displayMinimap = !m_displayMinimap;

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
            if( Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.S))
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
        /// Affiche la position de la souris sur la console.
        /// </summary>
        public void PutsMousePos()
        {
            m_consoleOutput.AppendLine("Mouse position : " + GetMousePosUnits().ToString());
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
                m_consoleOutput.AppendLine("Pas d'emplacement où poser la ward !");
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
            // Récupère la position de la souris
            Vector2 position = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            // Dessine le cursor
            batch.Draw(Ressources.Cursor, new Rectangle((int)position.X, (int)position.Y, 32, 32), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.Front);
            
            if (!IsEnabled)
            {
                m_modeButton.Visible = false;
                m_consoleInput.IsVisible = false;
                m_consoleInput.HasFocus = false;
                m_consoleOutput.IsVisible = false;
            }
            else
            {
                m_modeButton.Visible = true;
                m_consoleInput.IsVisible = true;
                m_consoleOutput.IsVisible = true;
            }

            // Dessine le bandeau supérieur
            batch.Draw(Ressources.DummyTexture, new Rectangle(0, 0, (int)GameServer.GetScreenSize().X, 25),
                null, new Color(0, 0, 0, 200), 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.GUI + 5 * GraphicsHelpers.Z.BackStep);
            
            // Dessine la minimap
            DrawMinimap(batch, new Rectangle((int)GameServer.GetScreenSize().X - 200, (int)GameServer.GetScreenSize().Y - 100, 200, 100), GraphicsHelpers.Z.GUI + 2 * GraphicsHelpers.Z.BackStep);
            
            // Dessine des infos de debug.
            batch.DrawString(Ressources.Font, "RowId = " + m_rowId + " | CheckpointId = " + m_checkpointId, new Vector2(150, 0), Color.White);
        }

        /// <summary>
        /// Dessine la minimap à l'emplacement donné.
        /// </summary>
        void DrawMinimap(RemoteSpriteBatch batch, Rectangle rect, float z)
        {
            if (!m_displayMinimap)
                return;

            int w = CurrentMap.Passability.GetLength(0);
            int h = CurrentMap.Passability.GetLength(1);
            int unitX = Math.Max(1, rect.Width / w);
            int unitY = Math.Max(1, rect.Height / h);

            if(m_displayMinimap && m_minimapDirty)
            {
                // Si la taille de la map a a changé, on change la taille de la texture de la minimap.
                if(m_minimapTexture.Width != CurrentMap.Passability.GetLength(0) || m_minimapTexture.Height != CurrentMap.Passability.GetLength(1))
                {
                    m_minimapTexture.Dispose();
                    m_minimapTexture = new RemoteRenderTarget(GameServer.GetScene().GraphicsServer, CurrentMap.Size.X, CurrentMap.Size.Y, RenderTargetUsage.PreserveContents);
                    
                    
                }

                m_minimapBatch.GraphicsDevice.SetRenderTarget(m_minimapTexture);
                m_minimapBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                m_minimapBatch.GraphicsDevice.Clear(Color.White);
                // Dessine la minimap sur la texture temporaire.
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        Color col = CurrentMap.Passability[x, y] ? Color.White : Color.Red;
                        col = new Color(col.R / 2, col.G / 2, col.B / 2);
                        
                        m_minimapBatch.Draw(Ressources.DummyTexture,
                            new Rectangle(x, y, 1, 1),
                            null,
                            col,
                            0.0f,
                            Vector2.Zero, SpriteEffects.None,
                            z);
                    }
                }
                m_minimapBatch.End();
                m_minimapBatch.GraphicsDevice.SetRenderTarget(GameServer.GetScene().MainRenderTarget);
                // Supprime le dirty bit.
                m_minimapDirty = false;
            }




            // Dessine la minimap
            batch.Draw(m_minimapTexture, rect, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, z);
            
            // Dessine la vision sur la minimap
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (m_map.Vision.HasVision(EntityType.Team1, new Vector2(x, y)))
                    {
                        Color col = new Color(255, 255, 255, 255);
                        batch.Draw(Ressources.DummyTexture,
                            new Rectangle((int)(rect.X + (x / (float)w) * rect.Width),
                            (int)(rect.Y + (y / (float)h) * rect.Height),
                            unitX,
                            unitY), null,
                            col,
                            0.0f,
                            Vector2.Zero, SpriteEffects.None,
                            z + GraphicsHelpers.Z.FrontStep);
                    }

                }
            }

            // Dessine le rectangle indiquant quelle partie de la map est actuellement affichée à l'écran.
            batch.Draw(Ressources.DummyTexture,
                new Rectangle((int)(rect.X + (CurrentMap.ScrollingVector2.X / GameServer.GetMap().UnitSize / (float)w) * rect.Width),
                              (int)(rect.Y + (CurrentMap.ScrollingVector2.Y / GameServer.GetMap().UnitSize / (float)h) * rect.Height),
                              (int)((CurrentMap.Viewport.Width / (float)(w * GameServer.GetMap().UnitSize)) * rect.Width),
                              (int)((CurrentMap.Viewport.Height / (float)(h * GameServer.GetMap().UnitSize)) * rect.Height)), null,
                              new Color(255, 255, 255, 60),
                              0.0f,
                              Vector2.Zero, SpriteEffects.None,
                              z + GraphicsHelpers.Z.FrontStep * 2);
        }

        #region External event handlers
        /// <summary>
        /// Dessine 
        /// </summary>
        void m_map_OnMapModified()
        {
            m_minimapDirty = true;
        }
        #endregion
        #endregion
    }
}
