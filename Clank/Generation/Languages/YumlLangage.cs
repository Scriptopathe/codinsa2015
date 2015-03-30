using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.Core.Model.Language;
namespace Clank.Core.Generation.Languages
{
    /// <summary>
    /// Représente un langage de programmation.
    /// Cette classe prend en charge la traduction du code d'un arbre abstrait 
    /// vers le langage de programmation de destination.
    /// </summary>
    [LanguageGenerator("yuml")]
    public class YumlGenerator : ILanguageGenerator
    {
        public const string LANG_KEY = "cs";
        public const bool PRINT_DEBUG = true;
        #region Variables
        Clank.Core.Model.ProjectFile m_project;
        #endregion

        /// <summary>
        /// Crée une nouvelle instance de YumlGenerator avec un fichier projet passé en paramètre.
        /// </summary>
        /// <param name="project"></param>
        public YumlGenerator(Clank.Core.Model.ProjectFile project)
        {
            SetProject(project);
        }

        public string GenerateInstruction(Instruction inst) { return ""; }
        /// <summary>
        /// Crée une nouvelle instance de YumlGenerator.
        /// </summary>
        public YumlGenerator() { }
        /// <summary>
        /// Définit le projet contenant les informations nécessaires à la génération de code.
        /// </summary>
        public void SetProject(Model.ProjectFile project)
        {
            m_project = project;
        }

        /// <summary>
        /// Génère les fichiers du projet à partir de la liste des instructions.
        /// </summary>
        public List<OutputFile> GenerateProjectFiles(List<Instruction> instructions, string outputDirectory, bool isServer)
        {
            List<OutputFile> outputFiles = new List<OutputFile>();
            List<Instruction> enums = new List<Instruction>();
            StringBuilder b = new StringBuilder();
            foreach (Model.Language.Instruction inst in instructions)
            {
                if (inst is Model.Language.ClassDeclaration)
                {
                    Model.Language.ClassDeclaration decl = (Model.Language.ClassDeclaration)inst;
                    b.AppendLine(GenerateYumlClass(decl));
                }
                else if(inst is Model.Language.EnumDeclaration)
                {
                    b.AppendLine("[" + ((Model.Language.EnumDeclaration)inst).Name +"{bg:green}]");
                }
                else
                {
                    m_project.Log.AddWarning("Instruction de type " + inst.GetType().ToString() + " inattendue.", inst.Line, inst.Character, inst.Source);
                }
            }

            // Génère le fichier des enums
            outputFiles.Add(new OutputFile(outputDirectory + "/diag.txt", b.ToString()));
            return outputFiles;
        }

        /// <summary>
        /// Cherche le type des éléments d'un type de collection donné.
        /// </summary>
        /// <param name="collectionType"></param>
        /// <returns></returns>
        ClankTypeInstance SeekElementType(ClankTypeInstance collectionType)
        {
            if (collectionType.BaseType.JType != JSONType.Array)
                return collectionType;
            else
                return SeekElementType(collectionType.GenericArguments.First());
        }

        string GenerateYumlClass(ClassDeclaration decl)
        {
            StringBuilder associations = new StringBuilder();
            var vars = decl.Instructions.Where((Instruction inst) =>
            {
                return inst is VariableDeclarationInstruction;
            });
            var funcs = decl.Instructions.Where((Instruction inst) =>
            {
                return inst is FunctionDeclaration;
            });

            string bgcolor = "white";
            if (decl.Name == "State") bgcolor = "blue";
            if (decl.Name.Contains("Entity")) bgcolor = "red";
            bgcolor = "{bg:" + bgcolor + "}";
            StringBuilder b = new StringBuilder();
            b.Append("[" + decl.Name + "|");
            foreach(var inst in vars)
            {
                var varDecl = inst as VariableDeclarationInstruction;
                if(varDecl != null)
                {
                    if (varDecl.IsPublic)
                        b.Append("+");
                    else
                        b.Append("-");
                    b.Append(varDecl.Var.Name + ":" + GenerateTypeInstanceName(varDecl.Var.Type));
                    b.Append(";");

                    if (!varDecl.Var.Type.BaseType.IsBuiltIn)
                    {

                        if (varDecl.Var.Type.BaseType.JType == JSONType.Array)
                        {
                            var elementType = SeekElementType(varDecl.Var.Type);
                            associations.AppendLine("[" + decl.Name + bgcolor + "]1--*+[" + GenerateTypeInstanceName(elementType) + "]");
                        }
                        else
                            associations.AppendLine("[" + decl.Name + bgcolor +"]1--1+[" + GenerateTypeInstanceName(varDecl.Var.Type) + "]");
                    }
                }
            }
            b.Append("|");
            foreach (var inst in funcs)
            {
                var funcDecl = inst as FunctionDeclaration;
                if (funcDecl != null)
                {
                    if (funcDecl.Func.IsPublic)
                        b.Append("+");
                    else
                        b.Append("-");
                    b.Append(funcDecl.Func.Name + "(");
                    foreach(var arg in funcDecl.Func.Arguments)
                    {
                        b.Append(arg.ArgName + ":" + GenerateTypeInstanceName(arg.ArgType));
                        if (arg != funcDecl.Func.Arguments.Last())
                            b.Append(";");
                    }
                    b.Append(")");
                    b.Append(":" + GenerateTypeInstanceName(funcDecl.Func.ReturnType));

                    if(inst != funcs.Last())
                        b.Append(";");


                    if (!funcDecl.Func.ReturnType.BaseType.IsBuiltIn)
                    {

                        if (funcDecl.Func.ReturnType.BaseType.JType == JSONType.Array)
                        {
                            var elementType = SeekElementType(funcDecl.Func.ReturnType);
                            associations.AppendLine("[" + decl.Name + bgcolor + "]1--*+[" + GenerateTypeInstanceName(elementType) + "]");
                        }
                        else
                            associations.AppendLine("[" + decl.Name + bgcolor + "]1--1+[" + GenerateTypeInstanceName(funcDecl.Func.ReturnType) + "]");
                    }
                }
            }
            b.AppendLine("]");
            b.AppendLine(associations.ToString());
            return b.ToString();
        }

        

        #region Expressions

        /// <summary>
        /// Génère le code représentant l'instance de type passé en paramètre.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GenerateTypeInstanceName(ClankTypeInstance type)
        {
            if (type.IsGeneric)
            {
                StringBuilder b = new StringBuilder();
                foreach (ClankTypeInstance tArg in type.GenericArguments)
                {
                    b.Append(GenerateTypeInstanceName(tArg));
                    if (tArg != type.GenericArguments.Last())
                        b.Append(",");
                }

                if(!type.BaseType.IsMacro)
                    return GenerateTypeName(type.BaseType) + "\\<" + b.ToString() + "\\>";
                else
                {
                    // Si on a un type macro on va remplacer :
                    // $(T) => type concerné
                    string nativeFuncName = GenerateTypeName(type.BaseType);

                    // Remplace les params génériques par leurs valeurs.
                    for (int i = 0; i < type.BaseType.GenericArgumentNames.Count; i++)
                    {
                        nativeFuncName = nativeFuncName.Replace(SemanticConstants.ReplaceChr + "(" + type.BaseType.GenericArgumentNames[i] + ")",
                            GenerateTypeInstanceName(type.GenericArguments[i]));
                    }

                    return nativeFuncName.Replace("<", "\\<").Replace(">", "\\>");
                }
            }

            return GenerateTypeName(type.BaseType);
        }

        /// <summary>
        /// Génère le code représentant le type passé en paramètre.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GenerateTypeName(ClankType type)
        {
            if(type.IsMacro)
            {
                
                // Pour les types macro, on remplace le nom du type par le nom du type natif.
                Model.MacroContainer.MacroClass mcClass = m_project.Macros.FindClassByType(type);
                if (!mcClass.LanguageToTypeName.ContainsKey(LANG_KEY))
                    throw new InvalidOperationException("Le nom de la macro classe '" + type.GetFullNameAndGenericArgs() + "' n'est pas renseigné pour le langage '" + LANG_KEY + "'");

                string nativeClassName = mcClass.LanguageToTypeName[LANG_KEY];

                return mcClass.LanguageToTypeName[LANG_KEY];
            }
            else
                return type.Name;
        }
        #endregion

    }
}
