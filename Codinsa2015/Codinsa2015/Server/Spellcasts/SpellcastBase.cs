using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spellcasts
{
    /// <summary>
    /// Représent un sort de base, dont l'effet s'adapte à la description
    /// passée en paramètre.
    /// </summary>
    public class SpellcastBase : Spellcast
    {
        #region Variables
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
        #endregion

        #region Exports
        /// <summary>
        /// Obtient ou définit la shape utilisée par ce spell cast.
        /// </summary>
        [Clank.ViewCreator.Export("GenericShapeView", "Shape utilisée par ce spell cast.")]
        public Shapes.CircleShape Shape { get { return m_shape; } set { m_shape = value; } }
        /// <summary>
        /// Obtient ou définit le nom de ce spellcast (utilisé pour le texturing notamment).
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de SpellcastFireball.
        /// </summary>
        public SpellcastBase(Spells.Spell sourceSpell, Spells.SpellCastTargetInfo castInfo) : base()
        {
            SourceSpell = sourceSpell;
            Name = sourceSpell.Name;
            m_time = 0;
            m_shape = new Shapes.CircleShape(sourceSpell.SourceCaster.Position, sourceSpell.Description.TargetType.AoeRadius);
            m_castInfo = castInfo;
            m_canTouch = false;
            m_entityIgnoreList = new List<EntityBase>();

            if (castInfo.Type == Spells.TargettingType.Targetted)
            {
                EntityBase target = GameServer.GetMap().GetEntityById(castInfo.TargetId);
                if(target == null)
                    IsDisposing= true;
                else
                    m_initialTargetPos = target.Position;
            }
        }
        /// <summary>
        /// Mets à jour ce sort.
        /// </summary>
        public override void Update(GameTime time)
        {
            if (IsDisposing)
                return;
            
            // Supprime le spell une fois que sa durée est terminée.
            if (m_time > SourceSpell.Description.TargetType.Duration)
            {
                IsDisposing = true;
                m_canTouch = false;
                return;
            }

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
                    m_canTouch = true;
                    break;
                // Targetted : on avance vers la cible.
                case Spells.TargettingType.Targetted:
                    if(GameServer.GetMap().GetEntityById(m_castInfo.TargetId) == null)
                    {
                        IsDisposing = true;
                        return;
                    }

                    if(SourceSpell.Description.TargetType.Duration == 0)
                    {
                        // Duration 0 : sort instant.
                        Entities.EntityBase dst = GameServer.GetMap().GetEntityById(m_castInfo.TargetId);
                        m_shape.Position = dst.Position;
                        m_canTouch = true;
                        OnCollide(dst);
                    }
                    else
                    {
                        Vector2 dir = GameServer.GetMap().GetEntityById(m_castInfo.TargetId).Position - m_shape.Position;
                        dir.Normalize();
                        m_shape.Position += dir * speed;
                        m_canTouch = true;

                    }
                    
                    break;
            }


        }

        /// <summary>
        /// Applique les effets du sorts à l'entité touchée.
        /// </summary>
        public override void OnCollide(EntityBase entity)
        {
            // Si le sort n'est pas encore prêt à toucher l'entité.
            if (!m_canTouch)
                return;

            // On fait en sorte que cette méthode ne soit appelée
            // qu'une seule fois pour chaque entité.
            if (m_entityIgnoreList.Contains(entity))
                return;
            else
                m_entityIgnoreList.Add(entity);


            // Vérifie que le sort fonctionne sur l'entité
            if (!SourceSpell.HasEffectOn(entity, m_castInfo))
                return;


            // Détruit le sort si il doit être détruit.
            if (SourceSpell.Description.TargetType.DieOnCollision)
                IsDisposing = true;

            // Ajoute les altérations d'état au héros.
            foreach(StateAlterationModel alteration in SourceSpell.Description.OnHitEffects)
            {
                entity.AddAlteration(new StateAlteration("spell-active-" + SourceSpell.Name + "-" + entity.ID,
                    SourceSpell.SourceCaster,
                    alteration, 
                    m_castInfo.AlterationParameters, 
                    StateAlterationSource.SpellActive));
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
