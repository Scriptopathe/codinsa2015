using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
namespace MapImporter
{
    class Program
    {
        static int s_campId = 1;
        static int s_minibossId = 1;
        static void Main(string[] args)
        {
            // Vérification des arguments.
            if(args.Count() != 2)
            {
                Console.WriteLine("usage: Codinsa2015.MapImporter.exe input.png output.txt");
                return;
            }
            else if(!System.IO.File.Exists(args[0]))
            {
                Console.WriteLine("erreur: fichier " + args[0] + " inexistant;");
                return;
            }

            StringBuilder entities = new StringBuilder();
            string infile = args[0];
            string outfile = args[1];

            var stream = System.IO.File.Open(outfile, System.IO.FileMode.Create);
            var streamWriter = new System.IO.StreamWriter(stream);
            Bitmap bmp = new Bitmap(infile);

            
            streamWriter.WriteLine("size " + bmp.Width + " " + bmp.Height);
            streamWriter.WriteLine("map");
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    Color col = bmp.GetPixel(x, y);
                    if (col.R == 0 && col.G == 0 && col.B == 0)
                        streamWriter.Write("0");
                    else
                        streamWriter.Write("1");

                    AddEntity(entities, col, x, y, bmp.Width);
                }
                streamWriter.WriteLine();
            }

            streamWriter.Write(entities.ToString());

            streamWriter.Close();

            Console.WriteLine("Opération terminée.");
            Console.Read();
        }

        static void AddEntity(StringBuilder b, Color color, int x, int y, int totalW)
        {
            int hexcode = ((int)color.R << 16) | ((int)color.G << 8) | (int)color.B;
            string posStr = x + " " + y;
            string teamStr = (x < totalW/2) ? "Team1" : "Team2";
            switch(hexcode)
            {
                case 0xFF0000: // rouge = inhibiteur / spawner
                    b.AppendLine(teamStr + "Spawner " + posStr);
                    break;
                case 0x0000FF: // bleu = idole
                    b.AppendLine(teamStr + "Idol " + posStr);
                    break;
                case 0x00FF00: // vert = hero spawner
                    b.AppendLine(teamStr + "HeroSpawner " + posStr);
                    break;
                case 0xFF00FF: // fushia = tour
                    b.AppendLine(teamStr + "Tower " + posStr);
                    break;
                case 0x00FFFF: // cyan = emplacement wardable
                    b.AppendLine("WardPlacement " + posStr);
                    break;
                case 0xFFFF00: // jaune = camp
                    b.AppendLine("Camp" + s_campId + " " + posStr);
                    s_campId++;
                    break;
                case 0x404040: // gris = miniboss
                    b.AppendLine("Miniboss" + s_minibossId + " " + posStr);
                    s_minibossId++;
                    break;
                default: // checkpoints
                    // Team dans le rouge (0x0A = 1, 0xA0 = 2, 0xAA = les 2)
                    // Id pour la team 1 : dans les 4 bits de poids faible du vert
                    //            team 2 : dans les 4 bits de poids fort du vert
                    // Row               : dans le bleu.
                    int row = hexcode & 0x0000FF;
                    int id = hexcode & 0x00FF00;
                    List<int> teams = new List<int>();
                    if((hexcode & 0xFF0000) == 0xAA) // 170
                    {
                        teams.Add(1);
                        teams.Add(2);
                    }
                    else if((hexcode & 0xFF0000) == 0x0A) // 10
                        teams.Add(1);
                    else if((hexcode & 0xFF0000) == 0xA0) // 160
                        teams.Add(2);
                    
                    foreach(int team in teams)
                    {
                        if(team == 1)
                            b.AppendLine("Team" + team + "Checkpoint " + posStr + " " + row + " " + (id & 0x000F00));
                        else
                            b.AppendLine("Team" + team + "Checkpoint " + posStr + " " + row + " " + (id & 0x00F000));
                    }

                    break;
            }
        }
    }
}
