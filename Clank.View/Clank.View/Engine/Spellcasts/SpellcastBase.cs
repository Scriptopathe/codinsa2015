using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine.Spellcasts
{
    /// <summary>
    /// Représent un sort de base, dont l'effet s'adapte à la description
    /// passée en paramètre.
    /// </summary>
    public class SpellcastBase : Spellcast
    {
        /// <summary>
        /// Forme du sort.
        /// </summary>
        Shapes.CircleShape m_shape;
        /// <summary>
        /// Temps écoulé depuis le cast du sort.
        /// </summary>
        float m_time;
        /// <summary>
        /// Informations de ciblage.
        /// </summary>
        Spells.SpellCastTargetInfo m_castInfo;
        /// <summary>
        /// Indique si le sort est prêt à appliquer les effets à l'impact.
        /// </summary>
        bool m_canTouch;
        /// <summary>
        /// Position initiale de la cible (si le ciblage est "targetted").
        /// </summary>
        Vector2 m_initialTargetPos;
        /// <summary>
        /// Liste des entités ayant déjà été touchées par ce sort, que le sort doit 
        /// désormais ignorer.
        /// </summary>
        List<EntityBase> m_entityIgnoreList;
        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de SpellcastFireball.
        /// </summary>
        public SpellcastBase(Spells.Spell sourceSpell, Spells.SpellCastTargetInfo castInfo) : base()
        {
            SourceSpell = sourceSpell;
            m_time = 0;
            m_shape = new Shapes.CircleShape(sourceSpell.SourceCaster.Position, sourceSpell.Description.TargetType.AoeRadius);
            m_castInfo = castInfo;
            m_canTouch = false;
            m_entityIgnoreList = new List<EntityBase>();

            if (castInfo.Type == Spells.TargettingType.Targetted)
                m_initialTargetPos = Mobattack.GetMap().GetEntityById(castInfo.TargetId).Position;
        }
        /// <summary>
        /// Mets à jour ce sort.
        /// </summary>
        public override void Update(GameTime time)
        {
            m_time += (float)time.ElapsedGameTime.TotalSeconds;
            float speed = (float)time.ElapsedGameTime.TotalSeconds * SourceSpell.Description.TargetType.Range / SourceSpell.Description.TargetType.Duration; // distance / temps
            // Mouvement du sort.
            switch(SourceSpell.Description.TargetType.Type)
            {
                // Direction : on avance dans la direction de cast.
                case Spells.TargettingType.Direction:
                    m_shape.Position += m_castInfo.TargetDirection * speed;
                    m_canTouch = true;
                    break;
                // Position : on reste à la position de cast :D
                case Spells.TargettingType.Position:
                    m_shape.Position = m_castInfo.TargetPosition;
                    break;
                // Targetted : on avance vers la cible.
                case Spells.TargettingType.Targetted:
                    Vector2 dir = Mobattack.GetMap().GetEntityById(m_castInfo.TargetId).Position - m_shape.Position;
                    dir.Normalize();
                    m_shape.Position += dir * speed;
                    m_canTouch = true;
                    break;
            }

            // Supprime le spell une fois que sa durée est terminée.
            if (m_time >= SourceSpell.Description.TargetType.Duration)
            {
                IsDisposing = true;
                m_canTouch = false;
            }
        }

        /// <summary>
        /// Dessine ce sort à l'écran.
        /// </summary>
        public override void Draw(GameTime time, SpriteBatch batch)
        {
            Point scroll = Mobattack.GetMap().Scrolling;
            batch.Draw(Ressources.DummyTexture,
                new Rectangle((int)(m_shape.Position.X * Map.UnitSize) - scroll.X, (int)(m_shape.Position.Y * Map.UnitSize) - scroll.Y, (int)(m_shape.Radius * 2 * Map.UnitSize), (int)(m_shape.Radius * 2 * Map.UnitSize)),
                null, 
                Color.Violet,
                0.0f,
                new Vector2(m_shape.Radius * Map.UnitSize, m_shape.Radius * Map.UnitSize),
                SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Applique les effets du sorts à l'entité touchée.
        /// </summary>
        public override void OnCollide(EntityBase entity)
        {
            // On fait en sorte que cette méthode ne soit appelée
            // qu'une seule fois pour chaque entité.
            if (m_entityIgnoreList.Contains(entity))
                return;
            else
                m_entityIgnoreList.Add(entity);

            // Si le sort n'est pas encore prêt à toucher l'entité.
            if (!m_canTouch)
                return;

            // Vérifie que le sort peut toucher cette entité.
            EntityTypeRelative flag = EntityTypeConverter.ToRelative(entity.Type, SourceSpell.SourceCaster.Type & (EntityType.Team1 | EntityType.Team2));
            if (!SourceSpell.Description.TargetType.AllowedTargetTypes.HasFlag(flag))
                return;

            // Détruit le sort si il doit être détruit.
            if (SourceSpell.Description.TargetType.DieOnCollision)
                IsDisposing = true;

            // Ajoute les altérations d'état au héros.
            foreach(StateAlterationModel alteration in SourceSpell.Description.OnHitEffects)
            {
                entity.AddAlteration(new StateAlteration(SourceSpell.SourceCaster, alteration, m_castInfo.AlterationParameters, StateAlterationSource.Spell));
            }
            
        }
        /// <summary>
        /// Retourne la forme de cette entité.
        /// </summary>
        /// <returns></returns>
        public override Shapes.Shape GetShape()
        {
            return m_shape;
        }
        
        /// <summary>
        /// Supprime ce spell de la map.
        /// A appeler lorsque l'effet du spell est terminé.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            
        }
        #endregion
    }
}
