using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clank.Core;
using Clank.Core.Generation;
using Clank.Core.Generation.Languages;
using Clank.Core.Generation.Preprocessor;
namespace Clank.Compiler
{
    class Program
    {
        /// <summary>
        /// Affiche la manière dont doit être utilisé le programme.
        /// </summary>
        static void PrintUsage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("compiler.exe -server=CS:server.cs -clients=CS:client.cs|JAVA:client.java -src=model.clank");
        }
        static void Main(string[] args)
        {
            // Get the arguments
            GenerationTarget serverTarget = null;
            List<GenerationTarget> clientTargets = new List<GenerationTarget>();
            string srcFile = null;
            foreach(string arg in args)
            {
                if (arg.Contains("-server="))
                {
                    string servTargetStr = arg.Split('=')[1];
                    string[] parts = servTargetStr.Split(':');
                    string tag = parts[0];
                    string outputFile = parts[1];
                    serverTarget = new GenerationTarget(tag, outputFile);
                }
                else if (arg.Contains("-clients="))
                {
                    string[] clientTargetStrs = arg.Split('=')[1].Split('|');
                    foreach (string clientTargetStr in clientTargetStrs)
                    {
                        string[] parts = clientTargetStr.Split(':');
                        string tag = parts[0];
                        string outputFile = parts[1];
                        clientTargets.Add(new GenerationTarget(tag, outputFile));
                    }
                }
                else if (arg.Contains("-src="))
                {
                    srcFile = arg.Split('=')[1];
                }
                else
                {
                    PrintUsage();
                    throw new ArgumentException("Argument '" + arg + "' invalide.");
                }
            }

            // Arguments vides.
            if(serverTarget == null || clientTargets.Count == 0 || srcFile == null)
            {
                PrintUsage();
                throw new ArgumentException("Nombre d'arguments invalide.");
            }

            // Fichier inexistant.
            if(!System.IO.File.Exists(srcFile))
            {
                throw new System.IO.FileNotFoundException("Le fichier " + srcFile + " est introuvable.");
            }

            // Génère le projet et redirige la sortie vers la console.
            ProjectGenerator generator = new ProjectGenerator();
            int errorCount = 0;
            int warningCount = 0;
            Clank.Core.Tools.EventLog.EventLogHandler handler = new Core.Tools.EventLog.EventLogHandler((Core.Tools.EventLog.Entry e) =>
            {
                Console.WriteLine(e.Type.ToString() + " (" + e.Source + "). Line " + e.Line + "-" + e.Character + ": " + e.Message + "\r\n\r\n");
                if (e.Type == Core.Tools.EventLog.EntryType.Error)
                    errorCount++;
                else if (e.Type == Core.Tools.EventLog.EntryType.Warning)
                    warningCount++;
            });

            generator.Generate("#include " + srcFile.Trim('"'), serverTarget, clientTargets, handler);

            Console.WriteLine("==================================");
            Console.WriteLine("== Compilation " + (errorCount > 0 ? "échouée." : "réussie") + ".");
            Console.WriteLine("== " + errorCount + " erreurs, " + warningCount + " warnings.");
            Console.WriteLine("Appuyez sur une touche pour terminer l'exécution du programme.");
            Console.Read();

        }
    }
}
