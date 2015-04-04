using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server
{
    /// <summary>
    /// Représente une série de statistiques liées à un héros.
    /// </summary>
    public class HeroStatistics
    {
        /// <summary>
        /// Obtient ou définit le nombre total de soins accordés par le héros
        /// à lui même ou d'autres joueurs.
        /// </summary>
        public float TotalHealings { get; set; } // done
        /// <summary>
        /// Obtient le nombre de dégâts infligés par le héros.
        /// </summary>
        public float TotalDamageDealt { get; set; } // done
        /// <summary>
        /// Obtient le nombre de dégâts infligés par le héros
        /// à d'autres héros.
        /// </summary>
        public float TotalDamageDealtToHeroes { get; set; } // done
        /// <summary>
        /// Obtient le nombre total de dégâts infligés par le héros aux objectifs.
        /// </summary>
        public float TotalDamageDealtToObjectives { get; set; } // done
        /// <summary>
        /// Obtient le nombre total de dégâts infligés aux structures.
        /// </summary>
        public float TotalDamageDealtToStructures { get; set; } // done
        /// <summary>
        /// Obtient le nombre total de dégâts reçus.
        /// </summary>
        public float TotalDamageTaken { get; set; } // done
        /// <summary>
        /// Obtient le nombre total de héros tués par le héros.
        /// </summary>
        public int TotalKills { get; set; } // done
        /// <summary>
        /// Obtient le nombre d'assistances du héros.
        /// </summary>
        public int TotalAssists { get; set; } // done
        /// <summary>
        /// Obtient le nombre de mort du héros.
        /// </summary>
        public int TotalDeaths { get; set; } // done
        /// <summary>
        /// Obtient le nombre de Virus tués par le héros.
        /// </summary>
        public int TotalVirusSlain { get; set; } // done
        /// <summary>
        /// Obtient le nombre de monstres neutres tués par le héros.
        /// </summary>
        public int TotalNeutralMonstersSlain { get; set; } // done
        /// <summary>
        /// Obtient le total du nombre de wards posées par le héros.
        /// </summary>
        public int TotalWardsUsed { get; set; } // done
        /// <summary>
        /// Obtient le total du nombre de wards détruites par ce joueur.
        /// </summary>
        public int TotalWardsDestroyed { get; set; } // done


        public HeroStatistics()
        {

        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine("----- Résumé ----");
            b.AppendLine("Kills : " + this.TotalKills + " | Deaths " + this.TotalDeaths + " | Assists " + this.TotalAssists);
            b.AppendLine("----- Dégâts ----");
            b.AppendLine("Infligés aux héros : " + this.TotalDamageDealtToHeroes);
            b.AppendLine("Infligés aux structures : " + this.TotalDamageDealtToStructures);
            b.AppendLine("Infligés aux objectifs : " + this.TotalDamageDealtToObjectives);
            b.AppendLine("Total : " + this.TotalDamageDealt);
            b.AppendLine("----- Autres ----");
            b.AppendLine("Soins donnés : " + this.TotalHealings);
            b.AppendLine("Monstres neutres tués : " + this.TotalNeutralMonstersSlain);
            b.AppendLine("Virusts tués : " + this.TotalVirusSlain);
            b.AppendLine("----- Vision ----");
            b.AppendLine("Wards posées : " + this.TotalWardsUsed);
            b.AppendLine("Wards détruites : " + this.TotalWardsDestroyed);

            return b.ToString();
        }
        /// <summary>
        /// Sauvegarde les statistiques de d'un héros dans le fichier donné.
        /// </summary>
        public void Save(string filename)
        {
            System.IO.File.WriteAllText(filename, Tools.Serializer.Serialize<HeroStatistics>(this));
        }
    }
}
