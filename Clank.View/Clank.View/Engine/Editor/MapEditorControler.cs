﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }
        /// <summary>
        /// Mets à jour le contrôleur en prenant en compte les entrées utilisateurs.
        /// </summary>
        public void Update(GameTime time)
        {
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                IsEnabled = !IsEnabled;

            if (!IsEnabled)
                return;
            Vector2 mousePosUnits = (new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y) + m_map.ScrollingVector2) / Map.UnitSize;
            // Ajout de matière
            if (Input.IsLeftClickPressed())
            {
                
                m_map.SetPassabilityAt(mousePosUnits, false);
            }
            else if(Input.IsRightClickPressed())
            {
                m_map.SetPassabilityAt(mousePosUnits, true);
            }

            // Ajout d'éléments de jeu.
            Entities.EntityType team = Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Q) ? Entities.EntityType.Team2 : Entities.EntityType.Team1;
            if(Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.T))
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

            // Sauvegarde
            if( Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.S))
            {
                m_map.Save();
            }

            // Chargement
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.L))
            {
                if(System.IO.File.Exists("Content/map.txt"))
                {
                    try
                    {
                        Map loaded = Map.FromFile("Content/map.txt");
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
        void UpdateScrolling()
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
            if (!IsEnabled)
                return;

            // Récupère la position de la souris, et la garde sur le bord.
            Vector2 position = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            batch.Draw(Ressources.Cursor, new Rectangle((int)position.X, (int)position.Y, 32, 32), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, Graphics.Z.GUI);

            UpdateScrolling();
        }
        #endregion
    }
}