using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.EnhancedGui;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.DebugHumanControler.Components
{
    /// <summary>
    /// Représente un élément de GUI permettant de monitorer les stats d'une entité.
    /// </summary>
    public class StatsViewer : GuiWindow
    {
        const int TITLE_BAR_H = 20;
        #region Variables
        private EntityBase m_entity;
        private GuiMultilineTextDisplay m_output;
        #endregion

        /// <summary>
        /// Obtient ou définit l'entité monitorée par ce StatsViewer.
        /// </summary>
        public EntityBase Entity { get { return m_entity; } set { m_entity = value; } }

        /// <summary>
        /// Crée une nouvelle instance de StatsViewer.
        /// </summary>
        /// <param name="m_manager"></param>
        /// <param name="entity"></param>
        public StatsViewer(GuiManager manager, EntityBase entity) : base(manager)
        {
            m_entity = entity;
            m_output = new GuiMultilineTextDisplay(manager);
            m_output.Parent = this;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            m_output.Location = new Point(0, TITLE_BAR_H);
            m_output.Size = new Point(this.Size.X, this.Size.Y - m_output.Location.Y);

            m_output.Clear();
            m_output.AppendLine(String.Format("Max HP = {0}", m_entity.GetMaxHP()));
            m_output.AppendLine(String.Format("HP     = {0}", m_entity.GetHP()));
            m_output.AppendLine(String.Format("Regen  = {0} HP/s", m_entity.GetHPRegen()));
            m_output.AppendLine(String.Format("Armor  = {0}", m_entity.GetArmor()));
            m_output.AppendLine(String.Format("MR     = {0}", m_entity.GetMagicResist()));
            m_output.AppendLine(String.Format("AD     = {0}", m_entity.GetAttackDamage()));
            m_output.AppendLine(String.Format("AP     = {0}", m_entity.GetAbilityPower()));
            m_output.AppendLine(String.Format("MS     = {0}", m_entity.GetMoveSpeed()));
            m_output.AppendLine(String.Format("AS     = {0}", m_entity.GetAttackSpeed()));
        }
        

    }
}
