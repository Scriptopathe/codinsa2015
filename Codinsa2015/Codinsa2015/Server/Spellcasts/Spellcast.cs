using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spellcasts
{
    public abstract class Spellcast
    {
        #region Variables
        /// <summary>
        /// Obtient ou définit la source du sort.
        /// 
        /// Note : ceci est un wrapper pour SourceSpell.SourceCaster, il n'existe
        /// que pour des raisons de rétro-compatibilité.
        /// </summary>
        public EntityBase Source { get { return SourceSpell.SourceCaster; } set { SourceSpell.SourceCaster = value; } }

        /// <summary>
        /// Spell ayant créé ce spell cast.
        /// </summary>
        public Spells.Spell SourceSpell { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce SpellCast doit être supprimé.
        /// </summary>
        public bool IsDisposing { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce Spellcast a été supprimé.
        /// </summary>
        public bool IsDisposed { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Mets à jour ce sort.
        /// </summary>
        public abstract void Update(GameTime time);

        /// <summary>
        /// Dessine ce sort à l'écran.
        /// </summary>
        public abstract void Draw(GameTime time, SpriteBatch batch);
        
        /// <summary>
        /// Applique les effets du sorts à l'entité touchée.
        /// </summary>
        /// <param name="entity"></param>
        public abstract void OnCollide(EntityBase entity);

        /// <summary>
        /// Retourne la forme de ce spellcast.
        /// </summary>
        /// <returns></returns>
        public abstract Shapes.Shape GetShape();
        /// <summary>
        /// Supprime ce spell de la map.
        /// A appeler lorsque l'effet du spell est terminé.
        /// </summary>
        public virtual void Dispose()
        {
            IsDisposed = true;

            
        }
        #endregion
    }
}
