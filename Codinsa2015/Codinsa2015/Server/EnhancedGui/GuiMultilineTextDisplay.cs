using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.EnhancedGui
{
    /// <summary>
    /// Représente un contrôle d'entrée de texte dans la gui.
    /// </summary>
    public class GuiMultilineTextDisplay : GuiWidget
    {
        #region Delegate / Events / Classes

        #endregion

        #region Variables
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


        /// <summary>
        /// Obtient ou définit une valeur indiquant si le contrôle est visible.
        /// </summary>
        public bool IsVisible
        {
            get;
            set;
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

        }



        /// <summary>
        /// Dessine les items de ce menu.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(RemoteSpriteBatch batch)
        {
            if (!IsVisible)
                return;
            // Constantes
            const int Margins = 2; // marges en pixels.
            const int LineHeightMargin = 1;

            // Mesures de taille de lignes etc...
            var font = Ressources.CourrierFont;
            int lineHeight = (int)font.MeasureString(" ").Y + LineHeightMargin;
            int maxLines = (Size.Y - 2 * Margins) / lineHeight + 1;
            int lastLine = m_linesCache.Count - 1; // TODO : mettre en place le scrolling.
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
                int y = (int)(Size.Y + (line - lastLine) * lineHeight);
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
        }

        /// <summary>
        /// Supprime tout le texte du contrôle.
        /// </summary>
        public void Clear()
        {
            m_linesCache.Clear();
            m_linesCache.Add("");
            m_textBuilder.Clear();
        }
        #endregion
    }
}
