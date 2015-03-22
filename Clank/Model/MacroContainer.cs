using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model
{
    /// <summary>
    /// Regroupe un ensemble d'unités "macros".
    /// </summary>
    public class MacroContainer
    {
        /// <summary>
        /// Représente une classe macro, reliant le type en language Clank.Core aux types dans les langages cibles.
        /// </summary>
        public class MacroClass
        {

            /// <summary>
            /// Type en langage Clank.Core wrappé par cette macro.
            /// </summary>
            public Language.ClankType Type { get; set; }
            /// <summary>
            /// Dictionaire contenant :
            ///     - en clels les noms des langages cibles
            ///     - en valeurs les noms des types représentant ce type dans les langages cibles.
            /// </summary>
            public Dictionary<string, string> LanguageToTypeName { get; set; }
            /// <summary>
            /// Dictionaire mappant les noms des fonctions en langage Clank.Core aux macro fonctions
            /// représentatives de ces fonctions.
            /// </summary>
            public Dictionary<string, MacroFunction> Functions { get; set; }

            public MacroClass() { LanguageToTypeName = new Dictionary<string, string>(); Functions = new Dictionary<string, MacroFunction>(); }
        }

        /// <summary>
        /// Représente une fonction "macro", mappant reliant la fonction en language Clank.Core aux fonctions
        /// dans les langages cibles.
        /// </summary>
        public class MacroFunction
        {
            /// <summary>
            /// Function du langage Clank.Core wrappée par cette macro.
            /// </summary>
            public Language.Function Function { get; set; }
            /// <summary>
            /// Dictionnary contenant :
            ///     - en clefs le nom du langage cible
            ///     - en valeurs le string représentant l'appel à la fonction dans le langage cible.
            /// Les paramètres à la fonction sont wrappés par des $ $.
            /// </summary>
            public Dictionary<string, string> LanguageToFunctionName { get; set; }

            public MacroFunction() { Function = new Language.Function(); LanguageToFunctionName = new Dictionary<string, string>(); }
        }

        /// <summary>
        /// Obtient ou définit les déclarations de classes contenues dans cette unité de macro.
        /// </summary>
        public List<MacroClass> ClassDeclarations { get; set; }
        /// <summary>
        /// Obtient ou définit les déclarations de fonctions contenues dans cette unité de macro.
        /// </summary>
        public List<MacroFunction> FunctionDeclarations { get; set; }
        /// <summary>
        /// Journal d'erreurs/warnings.
        /// </summary>
        public Tools.EventLog ParsingLog { get; set; }
        /// <summary>
        /// Crée une nouvelle instance de MacroContainer.
        /// </summary>
        public MacroContainer()
        {
            ParsingLog = new Tools.EventLog();
            ClassDeclarations = new List<MacroClass>();
            FunctionDeclarations = new List<MacroFunction>();
        }

        /// <summary>
        /// Finds a class by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MacroClass FindClassByType(Language.ClankType type)
        {
            foreach(MacroClass klass in ClassDeclarations)
            {
                if (klass.Type.GetFullName() == type.GetFullName())
                    return klass;
            }
            throw new Exception("Class not found : " + type.GetFullName());
        }

        /// <summary>
        /// Finds a function by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MacroFunction FindFunctionByName(string name)
        {
            foreach (MacroFunction f in FunctionDeclarations)
            {
                if (f.Function.Name == name)
                    return f;
            }
            throw new Exception("Function not found : " + name);
        }
        /// <summary>
        /// Ajoute des déclarations à ce Container à partir d'un block.
        /// </summary>
        /// <param name="block"></param>
        public void AddDeclarationsFromScript(Language.NamedBlockDeclaration block, Semantic.TypeTable table)
        {
            foreach (Language.Instruction instruction in block.Instructions)
            {
                if (instruction is Language.NamedBlockDeclaration && ((Language.NamedBlockDeclaration)instruction).Name == Language.SemanticConstants.MacroBk)
                {
                    AddDeclarationsFromMacroBlock((Language.NamedBlockDeclaration)instruction, table);
                }
            }
        }

        /// <summary>
        /// Ajoute des déclarations à ce Container à partir d'un block Access.
        /// </summary>
        /// <param name="block"></param>
        void AddDeclarationsFromMacroBlock(Language.NamedBlockDeclaration block, Semantic.TypeTable table)
        {
            foreach (Language.Instruction instruction in block.Instructions)
            {
                if (instruction is Language.ClassDeclaration)
                {
                    Language.ClassDeclaration classDecl = (Language.ClassDeclaration)instruction;
                    MacroClass classMacro = new MacroClass();

                    // Récupération du type en language Clank.Core.
                    Language.ClankType type = table.Types[classDecl.GetFullName()];
                    classMacro.Type = type;


                    bool foundName = false;
                    // Parse les instructions à la recherche de :
                    //  - une fonction Name qui retourne un string et qui contient des variables de type string
                    //  - d'autres fonctions qui ne contiennent que des variables de type string.
                    foreach(Language.Instruction funcDeclInstruction in classDecl.Instructions)
                    {
                        Language.FunctionDeclaration funcDecl = funcDeclInstruction as Language.FunctionDeclaration;
                        if(funcDecl != null)
                        {
                            if(funcDecl.Func.Name == "name")
                            {
                                // Nom de la classe
                                foundName = true;
                                classMacro.LanguageToTypeName = ParseLanguageTranslations(funcDecl);
                            }
                            else
                            {
                                // Ajout de la fonction.
                                MacroFunction func = new MacroFunction();
                                func.Function = funcDecl.Func;
                                func.LanguageToFunctionName = ParseLanguageTranslations(funcDecl);
                                classMacro.Functions.Add(func.Function.GetFullName(), func);
                            }
                        }
                        else if (funcDeclInstruction is Language.PlaceholderInstruction)
                        { }
                        else
                        {
                            ParsingLog.AddWarning("Seules les déclarations de fonctions sont autorisées dans les classes de block macro. Obtenu : " +
                                                   funcDeclInstruction.GetType().Name + ".",
                                instruction.Line, instruction.Character, instruction.Source);
                        }
                    }
                    ClassDeclarations.Add(classMacro);
                    // Erreur si pas de nom de classe trouvé :
                    if(!foundName)
                    {
                        ParsingLog.AddError("Fonction 'string name()' attendue dans la déclaration de macro du type '" + type.Name + "'.", 
                            instruction.Line, instruction.Character, instruction.Source);
                    }
                }
                else if(instruction is Language.FunctionDeclaration)
                {
                    Language.FunctionDeclaration funcDecl = (Language.FunctionDeclaration)instruction;
                    MacroFunction func = new MacroFunction();
                    func.Function = funcDecl.Func;
                    func.LanguageToFunctionName = ParseLanguageTranslations(funcDecl);
                    FunctionDeclarations.Add(func);
                }
                else
                {
                    ParsingLog.AddWarning("Déclaration de classe attendue dans le block macro. Obtenu : " + instruction.GetType() + ".",
                        instruction.Line, instruction.Character, instruction.Source);
                }
            }
        }


        /// <summary>
        /// Crée le dictionnaire associant nom du langage et nom de la fonction à partir des variables contenues dans la fonction.
        /// </summary>
        Dictionary<string, string> ParseLanguageTranslations(Language.FunctionDeclaration decl)
        {
            Dictionary<string, string> langToFuncName = new Dictionary<string, string>();
            foreach(Language.Instruction instruction in decl.Code)
            {
                Language.VariableDeclarationAndAssignmentInstruction varDecl = instruction as Language.VariableDeclarationAndAssignmentInstruction;
                if(varDecl != null)
                {
                    // Vérification du type de la variable.
                    if(varDecl.Declaration.Var.Type.GetFullName() == "string")
                    {
                        // Vérification des doublons.
                        if (!langToFuncName.ContainsKey(varDecl.Declaration.Var.Name))
                        {
                            Language.StringLiteral literal = varDecl.Assignment.Expression.Operand2 as Language.StringLiteral;
                            // Vérification de la rvalue.
                            if(literal != null)
                            {
                                langToFuncName.Add(varDecl.Declaration.Var.Name, literal.Value);
                            }
                            else
                            {
                                ParsingLog.AddError("La valeur affectée à une variable dans une fonction d'un block macro doit être un litéral de string.", 
                                    varDecl.Line, varDecl.Character, varDecl.Source);
                            }
                        }
                        else
                        {
                            ParsingLog.AddWarning("Présente de doublon pour la déclaration de " + varDecl.Declaration.Var.Name + " dans la fonction " + decl.Func.Name + ".",
                                varDecl.Line, varDecl.Character, varDecl.Source);
                        }
                    }
                    else
                    {
                        ParsingLog.AddWarning("Les fonctions des blocs macro ne peuvent contenir que des variables de type string, obtenu : " + varDecl.Declaration.Var.Type.GetFullName(),
                            varDecl.Line, varDecl.Character, varDecl.Source);
                    }
                }
                else
                {
                    ParsingLog.AddWarning("Instruction de type " + instruction.GetType() + " inatendue dans une déclaration de fonction macro",
                        decl.Line, decl.Character, decl.Source);
                }
            }
            return langToFuncName;
        }
    }
}
