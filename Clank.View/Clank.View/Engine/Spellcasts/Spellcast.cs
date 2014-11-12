using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine.Spellcasts
{
    public abstract class Spellcast
    {
        #region Variables
        /// <summary>
        /// Obtient ou définit la source du sort.
        /// </summary>
        public EntityBase Source { get; set; }

        /// <summary>
        /// Spell ayant créé ce spell cast.
        /// </summary>
        public Spells.Spell SourceSpell { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce SpellCast doit être supprimé.
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
