using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Codinsa2015.EnhancedGui
{
    /// <summary>
    /// Représente un contrôle d'entrée de texte dans la gui.
    /// </summary>
    public class GuiMultilineTextDisplay : GuiWidget
    {
        const int Margins = 2; // marges en pixels.
        const int LineHeightMargin = 1;
        #region Delegate / Events / Classes

        #endregion

        #region Variables
        GuiScrollbar m_scrollbar;
        /// <summary>
        /// Contient le texte en cours d'édition.
        /// </summary>
        StringBuilder m_textBuilder;
        /// <summary>
        /// Représente les lignes de texte de la plus haute dans le contrôle à la plus basse.
        /// </summary>
        List<string> m_linesCache;
        #endregion

        #region Properties
        bool m_isVisible;

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le contrôle est visible.
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set
            {
                m_isVisible = value;
                m_scrollbar.IsVisible = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de GuiMenu.
        /// </summary>
        public GuiMultilineTextDisplay(GuiManager manager)
            : base(manager)
        {
            manager.AddWidget(this);

            m_scrollbar = new GuiScrollbar(manager);
            m_scrollbar.MaxValue = 100;
            m_scrollbar.Step = 1;
            m_scrollbar.GrabLen = 1;
            m_scrollbar.CurrentValue = 0;
            m_scrollbar.Parent = this;

            Area = new Rectangle(0, 0, 640, 460);
            IsVisible = true;
            m_textBuilder = new StringBuilder("");
            m_linesCache = new List<string>() { "" };
        }

        /// <summary>
        /// Mets à jour le menu.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            m_scrollbar.Size = new Point(20, this.Size.Y);
            m_scrollbar.Location = new Point(this.Size.X - 20, 0);
            m_scrollbar.MaxValue = Math.Max(1, m_linesCache.Count);
        }

        /// <summary>
        /// Envoie la scrollbar tout en bas.
        /// </summary>
        public void ScrollDown()
        {
            m_scrollbar.CurrentValue = m_scrollbar.MaxValue;
        }

        /// <summary>
        /// Nombre de lignes max que peut afficher ce contrôle.
        /// </summary>
        /// <returns></returns>
        int GetMaxLines()
        {
            var font = Ressources.CourrierFont;
            int lineHeight = (int)font.MeasureString(" ").Y + LineHeightMargin;
            int maxLines = (Size.Y - 2 * Margins) / lineHeight + 1;
            return maxLines;
        }

        /// <summary>
        /// Dessine les items de ce menu.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            if (!IsVisible)
                return;

            // Mesures de taille de lignes etc...
            var font = Ressources.CourrierFont;
            int lineHeight = (int)font.MeasureString(" ").Y + LineHeightMargin;
            int maxLines = (Size.Y - 2 * Margins) / lineHeight + 1;
            int lastLine = Math.Max(0, Math.Min(
                m_linesCache.Count - 1,
                ((int)m_scrollbar.CurrentValue))); // TODO : mettre en place le scrolling.
            int firstLine = Math.Max(0, lastLine - maxLines);

            Rectangle menuBox = new Rectangle(0, 0, Size.X, Size.Y);

            // Dessine la boite principale.
            DrawRectBox(batch, Ressources.TextBox,
                menuBox,
                Color.White,
                0);

            // Dessine les lignes affichées
            for (int line = lastLine; line >= firstLine; line--)
            {
                int y = (int)(Size.Y + (line - lastLine) * lineHeight - lineHeight);
                DrawString(batch, font, m_linesCache[line],
                    new Vector2(Margins, Margins + y),
                    Color.Black,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    8);
            }
        }
        #endregion

        #region Text Handling
        /// <summary>
        /// Sépare le string en plusieurs lignes en fonction des caractères de retour à la ligne et de la taille du contrôle.
        /// </summary>
        /// <param name="s"></param>
        public List<string> ComputeLines(string s)
        {
            List<string> finalLines = new List<string>();
            string[] lines = s.Split('\n');
            foreach (string line in lines)
            {
                finalLines.AddRange(ComputeLine(line));
            }
            return finalLines;
        }
        /// <summary>
        /// Sépare le string (considéré sur 1 ligne) en plusieurs lignes afin qu'il ne dépasse pas la taille du contrôle.
        /// </summary>
        /// <param name="s"></param>
        public List<string> ComputeLine(string s)
        {
            List<string> lines = new List<string>();
            string[] words = s.Split(' ');

            StringBuilder buff = new StringBuilder();

            int i = 0;
            foreach (string word in words)
            {
                Vector2 size = Ressources.CourrierFont.MeasureString(buff + word + " ");

                // Si on dépasse la taille max, on passe à la ligne suivante.
                if (size.X > Size.X)
                {
                    lines.Add(buff.ToString());
                    buff.Clear();
                }
                // Ajoute les espaces entre chaque mot
                buff.Append(word + ((i != words.Length - 1) ? " " : ""));

                i++;
            }

            lines.Add(buff.ToString());

            return lines;
        }

        /// <summary>
        /// Ajoute un string à la fin de ce contrôle.
        /// </summary>
        /// <param name="s"></param>
        public void Append(string s)
        {
            s = s.Replace("\t", "  ");
            List<string> lines = ComputeLines(s);
            int i = 0;
            foreach (string line in lines)
            {
                m_textBuilder.Append(s);
                m_linesCache[m_linesCache.Count - 1] += line;
                if (i != lines.Count - 1)
                    m_linesCache.Add("");
                i++;
            }
        }

        /// <summary>
        /// Ajoute un string à la fin de ce contrôle, suivi d'une fin de ligne.
        /// </summary>
        /// <param name="s"></param>
        public void AppendLine(string s)
        {
            Append(s + "\n");
            m_scrollbar.MaxValue = Math.Max(1, m_linesCache.Count);
            ScrollDown();
        }

        /// <summary>
        /// Supprime tout le texte du contrôle.
        /// </summary>
        public void Clear()
        {
            m_linesCache.Clear();
            m_linesCache.Add("");
            m_textBuilder.Clear();
            m_scrollbar.MaxValue = Math.Max(1, m_linesCache.Count);
        }
        #endregion
    }
}
