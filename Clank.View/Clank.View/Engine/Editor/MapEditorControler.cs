using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.View.Engine;
using Clank.View.Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Editor
{
    /// <summary>
    /// Contrôleur de l'éditeur de map.
    /// </summary>
    public class MapEditorControler
    {
        public int ScrollSpeed = 16;
        #region Variables
        bool m_isEnabled = false;
        Map m_map;
        bool m_terraFormingMode = true;
        int m_rowId = 0;
        int m_checkpointId = 0;
        bool m_captureMouse = true;
        private Gui.GuiButton m_modeButton;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce contrôleur est activé / désactivé.
        /// Si désactivé, tous les composants de l'interface seront masqués.
        /// </summary>
        public bool IsEnabled
        {
            get { return m_isEnabled; }
            set { m_isEnabled = value; }
        }

        /// <summary>
        /// Map en cours d'édition.
        /// </summary>
        public Map CurrentMap
        {
            get;
            set;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de MapEditorControler associé à la map donnée.
        /// </summary>
        public MapEditorControler(Map map)
        {
            m_map = map;
            m_isEnabled = true;

            CreateGui();
        }

        /// <summary>
        /// Initialise les composants de l'interface graphique.
        /// </summary>
        void CreateGui()
        {
            m_modeButton = new Gui.GuiButton()
            {
                Position = new Vector2(5, 5),
                Title = "Land",
                Width = 150, 
                Height = 15
            };
            m_modeButton.Clicked += OnChangeMode;
            Mobattack.GetScene().GuiManager.AddWidget(m_modeButton);
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
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                IsEnabled = !IsEnabled;

            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.RightControl))
                m_captureMouse = !m_captureMouse;

            if (!IsEnabled)
                return;

            Vector2 mousePosPx = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            Vector2 mousePosUnits = ((mousePosPx + m_map.ScrollingVector2) - new Vector2(m_map.Viewport.X, m_map.Viewport.Y)) / Map.UnitSize;

            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Add))
                Map.UnitSize *= 2;
            else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Subtract))
                Map.UnitSize /= 2;


            if (m_terraFormingMode && mousePosPx.Y > 25)
            { 
                // Ajout de matière
                if (Input.IsLeftClickPressed())
                {
                    m_map.SetPassabilityAt(mousePosUnits, false);
                }
                else if (Input.IsRightClickPressed())
                {
                    m_map.SetPassabilityAt(mousePosUnits, true);
                }
            }
            else if(!m_terraFormingMode)
            {
                if (Input.IsRightClickTrigger())
                {

                    Entities.EntityCollection entitiesInRange = m_map.Entities.GetAliveEntitiesInRange(mousePosUnits, 1f);

                    Gui.GuiMenu menu = new Gui.GuiMenu();
                    menu.Position = mousePosPx;
                    menu.Title = "Menu";
                    foreach (EntityBase entity in entitiesInRange.Values)
                    {
                        Gui.GuiMenu.GuiMenuItem item = new Gui.GuiMenu.GuiMenuItem("Remove " + entity.Type.ToString());
                        item.ItemSelected += new Gui.GuiMenu.ItemSelectedDelegate(() =>
                        {
                            m_map.Entities.Remove(entity.ID);
                            entity.Dispose();
                        });
                        item.IsEnabled = true;
                        menu.AddItem(item);
                        Mobattack.GetScene().GuiManager.AddWidget(menu);
                    }
                }

                // Ajout d'éléments de jeu.
                Entities.EntityType team = Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Q) ? Entities.EntityType.Team2 : Entities.EntityType.Team1;
                if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.T))
                {
                    Entities.EntityBase entity = new Entities.EntityTower()
                    {
                        Position = mousePosUnits,
                        Type = Entities.EntityType.Tower | team,
                    };
                    m_map.Entities.Add(entity.ID, entity);
                }
                else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.R))
                {
                    Entities.EntityBase entity = new Entities.EntitySpawner()
                    {
                        Position = mousePosUnits,
                        SpawnPosition = mousePosUnits,
                        Type = Entities.EntityType.Spawner | team,
                    };
                    m_map.Entities.Add(entity.ID, entity);
                }
                else if(Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    Entities.EntityBase entity = new Entities.EntityCheckpoint()
                    {
                        Position = mousePosUnits,
                        Type = Entities.EntityType.Checkpoint | team,
                        CheckpointID = m_checkpointId,
                        CheckpointRow = m_rowId,
                    };
                    m_map.Entities.Add(entity.ID, entity);
                    m_checkpointId++;
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
                m_map.Save();
            }

            // Chargement
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.L))
            {
                if(System.IO.File.Exists(Ressources.MapFilename))
                {
                    try
                    {
                        Map loaded = Map.FromFile(Ressources.MapFilename);
                        m_map = loaded;

                        Mobattack.GetScene().Map = m_map;
                    }
                    /*catch { }*/
                    finally { }
                    
                }
                
            }
        }

        /// <summary>
        /// Mets à jour le scrolling en fonction de la position de la souris.
        /// </summary>
        void UpdateMouseScrolling()
        {
            // Récupère la position de la souris, et la garde sur le bord.
            Vector2 position = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            position = Vector2.Max(Vector2.Zero, Vector2.Min(Mobattack.GetScreenSize(), position));
            Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)position.X, (int)position.Y);

            // Fait bouger l'écran quand on est au bord.
            if (position.X <= 5)
                m_map.ScrollingVector2 = new Vector2(m_map.ScrollingVector2.X - ScrollSpeed, m_map.ScrollingVector2.Y);
            else if (position.X >= Mobattack.GetScreenSize().X - 5)
                m_map.ScrollingVector2 = new Vector2(m_map.ScrollingVector2.X + ScrollSpeed, m_map.ScrollingVector2.Y);
            if (position.Y <= 5)
                m_map.ScrollingVector2 = new Vector2(m_map.ScrollingVector2.X, m_map.ScrollingVector2.Y - ScrollSpeed);
            else if (position.Y >= Mobattack.GetScreenSize().Y - 5)
                m_map.ScrollingVector2 = new Vector2(m_map.ScrollingVector2.X, m_map.ScrollingVector2.Y + ScrollSpeed);
        }

        /// <summary>
        /// Dessine les éléments graphiques du contrôleur.
        /// </summary>
        public void Draw(SpriteBatch batch)
        {            
            // Récupère la position de la souris
            Vector2 position = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            // Dessine le cursor
            batch.Draw(Ressources.Cursor, new Rectangle((int)position.X, (int)position.Y, 32, 32), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, Graphics.Z.Front);

            if(m_captureMouse)
                UpdateMouseScrolling();

            
            if (!IsEnabled)
            {
                m_modeButton.Visible = false;
                return;
            }
            
            m_modeButton.Visible = true;

            // Dessine le bandeau supérieur
            batch.Draw(Ressources.DummyTexture, new Rectangle(0, 0, (int)Mobattack.GetScreenSize().X, 25),
                null, new Color(255, 255, 255, 200), 0.0f, Vector2.Zero, SpriteEffects.None, Graphics.Z.GUI + 5 * Graphics.Z.BackStep);
            
            // Dessine la minimap
            DrawMinimap(batch, new Rectangle((int)Mobattack.GetScreenSize().X - 200, (int)Mobattack.GetScreenSize().Y - 100, 200, 100), Graphics.Z.GUI + 2 * Graphics.Z.BackStep);
            
            // Dessine des infos de debug.
            batch.DrawString(Ressources.Font, "RowId = " + m_rowId + " | CheckpointId = " + m_checkpointId, new Vector2(5, Mobattack.GetScreenSize().Y - 50), Color.Black);


        }

        /// <summary>
        /// Dessine la minimap à l'emplacement donné.
        /// </summary>
        void DrawMinimap(SpriteBatch batch, Rectangle rect, float z)
        {
            int w = m_map.Passability.GetLength(0);
            int h = m_map.Passability.GetLength(1);
            int unitX = Math.Max(1, rect.Width / w);
            int unitY = Math.Max(1, rect.Height / h);
            for(int x = 0; x < w; x++)
            {
                for(int y = 0; y < h; y++)
                {
                    Color col = m_map.Passability[x, y] ? Color.White : Color.Red;
                    if (!m_map.Vision.HasVision(EntityType.Team1, new Vector2(x, y)))
                    {
                        col = new Color(col.R / 2, col.G / 2, col.B / 2);
                    }
                    batch.Draw(Ressources.DummyTexture,
                        new Rectangle((int)(rect.X + (x / (float)w) * rect.Width),
                                      (int)(rect.Y + (y / (float)h) * rect.Height),
                                      unitX,
                                      unitY), null,
                                      col,
                                      0.0f,
                                      Vector2.Zero, SpriteEffects.None,
                                      z);
                }
            }

            batch.Draw(Ressources.DummyTexture,
                new Rectangle((int)(rect.X + (m_map.ScrollingVector2.X / Map.UnitSize / (float)w) * rect.Width),
                              (int)(rect.Y + (m_map.ScrollingVector2.Y / Map.UnitSize / (float)h) * rect.Height),
                              (int)((m_map.Viewport.Width / (float)(w * Map.UnitSize)) * rect.Width),
                              (int)((m_map.Viewport.Height / (float)(h * Map.UnitSize)) * rect.Height)), null,
                              new Color(255, 255, 255, 60),
                              0.0f,
                              Vector2.Zero, SpriteEffects.None,
                              z + Graphics.Z.FrontStep);
        }
        #endregion
    }
}
