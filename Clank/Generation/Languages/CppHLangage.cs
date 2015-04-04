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
    [LanguageGenerator("h")]
    public class CppHGenerator : ILanguageGenerator
    {
        public const string LANG_KEY = "cpp";

        #region Variables
        Clank.Core.Model.ProjectFile m_project;
        #endregion

        /// <summary>
        /// Crée une nouvelle instance de CSGenerator avec un fichier projet passé en paramètre.
        /// </summary>
        /// <param name="project"></param>
        public CppHGenerator(Clank.Core.Model.ProjectFile project)
        {
            SetProject(project);
        }
        /// <summary>
        /// Crée une nouvelle instance de CSGenerator.
        /// </summary>
        public CppHGenerator() { }
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
            foreach (Model.Language.Instruction inst in instructions)
            {
                if (inst is Model.Language.ClassDeclaration)
                {
                    Model.Language.ClassDeclaration decl = (Model.Language.ClassDeclaration)inst;

                    outputFiles.Add(new OutputFile(outputDirectory + "/" + decl.Name + ".h",
                        GenerateInstruction(inst)));
                }
                else if (inst is Model.Language.EnumDeclaration)
                {
                    enums.Add(inst);
                }
                else
                {
                    m_project.Log.AddWarning("Instruction de type " + inst.GetType().ToString() + " inattendue.", inst.Line, inst.Character, inst.Source);
                }
            }

            // Génère le fichier des enums
            outputFiles.Add(new OutputFile(outputDirectory + "/Common.h", GenerateCommonFile(enums)));

            return outputFiles;
        }

        /// <summary>
        /// Génère le code d'un fichier ne contenant que des enums.
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        string GenerateCommonFile(List<Instruction> enums)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("#pragma once");
            builder.AppendLine("#include \"inttypes.h\"");
            builder.AppendLine("#include \"TCPHelper.h\"");
            builder.AppendLine(@"#include <iostream>
#include <vector>
#include <list>");

            foreach (Instruction instruction in enums)
            {
                builder.AppendLine(GenerateInstruction(instruction));
            }

            return builder.ToString();
        }

        #region Includes
        /// <summary>
        /// Ajoute un type includable à la collection de types donnée.
        /// </summary>
        void AddToSet(HashSet<ClankType> types, ClankTypeInstance type) 
        {
            if(type.IsGeneric)
            {
                foreach(var arg in type.GenericArguments)
                {
                    AddToSet(types, arg);
                }
            }

            if (type.BaseType.IsBuiltIn | type.BaseType.IsEnum | type.BaseType.IsMacro)
                return;
            
            if (!types.Contains(type.BaseType)) types.Add(type.BaseType); 
        }
        /// <summary>
        /// Ajoute tous les types dont dépend l'instruction inst dans le set passé en paramètres.
        /// </summary>
        void AggregateDependencies(Instruction inst, HashSet<ClankType> types)
        {
            if(inst is ClassDeclaration)
            {
                ClassDeclaration decl = (ClassDeclaration)inst;
                foreach(var instruction in decl.Instructions)
                {
                    AggregateDependencies(instruction, types);
                }
            }
            else if (inst is Model.Language.Macros.RemoteFunctionWrapper)
            {
                var decl = (Model.Language.Macros.RemoteFunctionWrapper)inst;
                AggregateDependencies(decl.Func, types);
            }
            if(inst is VariableDeclarationInstruction)
            {
                VariableDeclarationInstruction decl = (VariableDeclarationInstruction)inst;
                AddToSet(types, decl.Var.Type);
            }
            else if(inst is VariableDeclarationAndAssignmentInstruction)
            {
                VariableDeclarationAndAssignmentInstruction decl = (VariableDeclarationAndAssignmentInstruction)inst;
                AddToSet(types, decl.Declaration.Var.Type);
            }
            else if(inst is FunctionDeclaration)
            {
                FunctionDeclaration decl = (FunctionDeclaration)inst;
                AddToSet(types, decl.Func.ReturnType);
                foreach(var arg in decl.Func.Arguments)
                {
                    AddToSet(types, arg.ArgType);
                }

                foreach(var instruction in decl.Code)
                {
                    AggregateDependencies(instruction, types);
                }
            }
            else if(inst is FunctionCallInstruction)
            {
                FunctionCallInstruction decl = (FunctionCallInstruction)inst;
                AddToSet(types, decl.Call.Func.ReturnType);
                foreach(var arg in decl.Call.Arguments)
                {
                    AddToSet(types, arg.Type);
                }
            }
        }
        /// <summary>
        /// Génère les déclaration d'inclusion de la classe.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        string GenerateIncludes(ClassDeclaration declaration)
        {
            StringBuilder builder = new StringBuilder();

            HashSet<ClankType> referencedTypes = new HashSet<ClankType>();
            AggregateDependencies(declaration, referencedTypes);
            foreach(ClankType inst in referencedTypes)
            {
                builder.AppendLine("#include \"" + inst.Name + ".h\"");
            }


            return builder.ToString();
        }
        #endregion
        /// <summary>
        /// Génére le code d'une classe.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        string GenerateClass(ClassDeclaration declaration)
        {
            StringBuilder builder = new StringBuilder();
            string inheritance = declaration.InheritsFrom == null ? "" : " : " + declaration.InheritsFrom;

            // Headers
            builder.AppendLine("#pragma once");
            builder.AppendLine("#include \"Common.h\"");
            builder.AppendLine(GenerateIncludes(declaration));


            // Paramètres génériques
            if (declaration.GenericParameters.Count > 0)
            {
                builder.Append("template");
                builder.Append("<");
                foreach (string tupe in declaration.GenericParameters)
                {
                    builder.Append("class " + tupe);
                    if (tupe != declaration.GenericParameters.Last())
                        builder.Append(",");
                }
                builder.Append(">");
            }
            builder.AppendLine();
            builder.Append("class " + declaration.Name);

            // Héritage
            builder.AppendLine(inheritance + "\n{\n");
            
            // Classe les instruction
            List<Instruction> publicInstructions = new List<Instruction>();
            List<Instruction> privateInstructions = new List<Instruction>();
            foreach(Instruction instruction in declaration.Instructions)
            {
                bool isPublic = false;
                if (instruction is VariableDeclarationInstruction)
                    isPublic = ((VariableDeclarationInstruction)instruction).IsPublic;
                else if (instruction is VariableDeclarationAndAssignmentInstruction)
                    isPublic = ((VariableDeclarationAndAssignmentInstruction)instruction).Declaration.IsPublic;
                else if (instruction is FunctionDeclaration)
                    isPublic = ((FunctionDeclaration)instruction).Func.IsPublic;
                else if (instruction is Model.Language.Macros.ProcessMessageMacro)
                    isPublic = true;
                else if (instruction is Model.Language.Macros.RemoteFunctionWrapper)
                    isPublic = true;

                if (isPublic)
                    publicInstructions.Add(instruction);
                else
                    privateInstructions.Add(instruction);
            }

            // Génère les instructions publiques.
            builder.AppendLine("public: ");
            foreach (Instruction instruction in publicInstructions)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\n");
            }
            builder.AppendLine(Tools.StringUtils.Indent(GenerateSerializer(declaration)));
            builder.AppendLine(Tools.StringUtils.Indent(GenerateDeserializer(declaration)));
            builder.AppendLine(Tools.StringUtils.Indent(GenerateConstructor(declaration)));
            // Génère les instructions privées.
            if(privateInstructions.Count > 0)
                builder.AppendLine("private: ");

            foreach (Instruction instruction in privateInstructions)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\n");
            }

            builder.Append("};");
            return builder.ToString();
        }

        string GenerateConstructor(ClassDeclaration declaration)
        {
            return declaration.Name + "();";
        }
        /// <summary>
        /// Génère le sérializer pour la classe donnée.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        string GenerateSerializer(ClassDeclaration declaration)
        {
            StringBuilder builder = new StringBuilder();
            // Nom de la fonction.
            builder.AppendLine("void serialize(std::ostream& output);");
            return builder.ToString();
        }

        public enum SerializerMode
        {
            Member,
            Array,
            None
        }

        /// <summary>
        /// Génère le désérializer pour la classe donnée.
        /// </summary>
        /// <param name="decl"></param>
        /// <returns></returns>
        string GenerateDeserializer(ClassDeclaration declaration)
        {
            StringBuilder builder = new StringBuilder();
            string typename = GenerateTypeName(m_project.Types.Types[declaration.GetFullName()]);
            builder.Append("static ");
            builder.Append(typename + " ");
            builder.Append("deserialize(std::istream& input);");
            return builder.ToString();
        }

        /// <summary>
        /// Génère le code d'une instruction.
        /// </summary>
        public string GenerateInstruction(Instruction instruction)
        {
            if (m_project == null)
                throw new InvalidOperationException("GenerateInstruction cannot be called if SetProject() has not been called before.");

            string comment = "";
            // Génération du commentaire
            if(instruction.Comment != null)
            {
                comment = GenerateComment(instruction);
            }

            // Génération du corps de l'instruction.
            if(instruction is FunctionDeclaration)
            {
                return comment + GenerateFunctionDeclarationInstruction((FunctionDeclaration)instruction);
            }
            else if(instruction is ConstructorDeclaration)
            {
                return comment + GenerateConstructorDeclarationInstruction((ConstructorDeclaration)instruction);
            }
            else if(instruction is ClassDeclaration)
            {
                return comment + GenerateClass((ClassDeclaration)instruction);
            }
            else if(instruction is VariableDeclarationInstruction)
            {
                return comment + GenerateDeclarationInstruction((VariableDeclarationInstruction)instruction);
            }
            else if(instruction is VariableDeclarationAndAssignmentInstruction)
            {
                return comment + GenerateDeclarationAndAssignmentInstruction((VariableDeclarationAndAssignmentInstruction)instruction);
            }
            else if(instruction is FunctionCallInstruction)
            {
                return comment + GenerateFunctionCall(((FunctionCallInstruction)instruction).Call) + ";";
            }
            else if(instruction is EnumDeclaration)
            {
                return comment + GenerateEnumInstruction((EnumDeclaration)instruction);
            }
            else if(instruction is Model.Language.Macros.ProcessMessageMacro)
            {
                return comment + GenerateProcessMessageMacro((Model.Language.Macros.ProcessMessageMacro)instruction);
            }
            else if(instruction is Model.Language.Macros.RemoteFunctionWrapper)
            {
                return comment + GenerateRemoteFunctionWrapper((Model.Language.Macros.RemoteFunctionWrapper)instruction);
            }
            else if(instruction is PlaceholderInstruction)
            {
                return comment;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Génère le code pour un wrapper de fonction à distance (pour les clients).
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateRemoteFunctionWrapper(Model.Language.Macros.RemoteFunctionWrapper instruction)
        {
            Function func = instruction.Func.Func;
            StringBuilder builder = new StringBuilder();
            string returnTypeName = GenerateTypeInstanceName(func.ReturnType);
            builder.Append(returnTypeName + " " + func.Name + "(");
            
            // Arg list
            foreach(var arg in func.Arguments)
            {
                builder.Append(GenerateTypeInstanceName(arg.ArgType) + " " + arg.ArgName + (arg == func.Arguments.Last() ? "" : ","));
            }
            builder.AppendLine(");");

            return builder.ToString();
        }
        /// <summary>
        /// Génère le commentaire attaché à l'instruction donné.
        /// </summary>
        string GenerateComment(Instruction inst)
        {
            StringBuilder builder = new StringBuilder();
            string[] lines = Tools.StringUtils.CutInLines(inst.Comment, 80);

            // Génère un commentaire approprié.
            if (inst is FunctionDeclaration || inst is ClassDeclaration || inst is Model.Language.Macros.ProcessMessageMacro)
            {
                builder.AppendLine("/** ");
                foreach (string line in lines)
                {
                    builder.AppendLine(" * " + line);
                }
                builder.AppendLine(" */");
            }
            else
            {
                // Commentaire en plusieurs lignes.
                foreach (string line in lines)
                {
                    builder.AppendLine("// " + line);
                }
            }
            return builder.ToString();
        }


        /// <summary>
        /// Génère le code pour la fonction de traitement des messages.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateProcessMessageMacro(Model.Language.Macros.ProcessMessageMacro instruction)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("string processRequest(string request, int " + Clank.Core.Model.Language.SemanticConstants.ClientID + ");");
            return builder.ToString();
        }

        #region Expressions
        /// <summary>
        /// Génère une expression évaluable.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        string GenerateEvaluable(Evaluable expr)
        {
            if(expr is BinaryExpressionGroup)
            {
                return GenerateBinaryExpressionGroup((BinaryExpressionGroup)expr);
            }
            else if(expr is UnaryExpressionGroup)
            {
                return GenerateUnaryExpressionGroup((UnaryExpressionGroup)expr);
            }
            else if(expr is VariableAccess)
            {
                return GenerateVariableAccess((VariableAccess)expr);
            }
            else if(expr is FunctionCall)
            {
                return GenerateFunctionCall((FunctionCall)expr);
            }
            else if(expr is Variable)
            {
                return GenerateVariable((Variable)expr);
            }
            else if(expr is BoolLiteral)
            {
                return ((BoolLiteral)expr).Value.ToString().ToLower();
            }
            else if(expr is StringLiteral)
            {
                return "\"" + ((StringLiteral)expr).Value + "\"";
            }
            else if(expr is FloatLiteral)
            {
                return ((FloatLiteral)expr).Value.ToString();
            }
            else if(expr is IntegerLiteral)
            {
                return ((IntegerLiteral)expr).Value.ToString();
            }
            else if(expr is EnumAccess)
            {
                return GenerateEnumAccess((EnumAccess)expr);
            }
            else if(expr is Typename)
            {
                return GenerateTypeInstanceName(((Typename)expr).Name);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Génère le code représentant l'instance de type passé en paramètre.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GenerateTypeInstanceName(ClankTypeInstance type)
        {
            if (type.IsGeneric || type.IsArray)
            {
                StringBuilder b = new StringBuilder();
                foreach (ClankTypeInstance tArg in type.GenericArguments)
                {
                    b.Append(GenerateTypeInstanceName(tArg));
                    if (tArg != type.GenericArguments.Last())
                        b.Append(",");
                }

                if(!type.BaseType.IsMacro)
                    return GenerateTypeName(type.BaseType) + "<" + b.ToString() + ">";
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

                    return nativeFuncName;
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
            if (type.IsMacro)
            {

                // Pour les types macro, on remplace le nom du type par le nom du type natif.
                Model.MacroContainer.MacroClass mcClass = m_project.Macros.FindClassByType(type);
                if (!mcClass.LanguageToTypeName.ContainsKey(LANG_KEY))
                    throw new InvalidOperationException("Le nom de la macro classe '" + type.GetFullNameAndGenericArgs() + "' n'est pas renseigné pour le langage '" + LANG_KEY + "'");

                string nativeClassName = mcClass.LanguageToTypeName[LANG_KEY];

                return mcClass.LanguageToTypeName[LANG_KEY];
            }
            else
                if (type.Name == "string")
                    return "std::string";
                else
                    return type.Name;
        }
        /// <summary>
        /// Génère le code d'un accès à un membre d'une énumération.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        string GenerateEnumAccess(EnumAccess access)
        {
            return access.Type.BaseType.GetFullName() + "." + access.Name;
        }
        /// <summary>
        /// Génère le code d'un appel de fonction.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        string GenerateFunctionCall(FunctionCall call)
        {
            if (call.Func.IsMacro)
                return GenerateMacroFunctionCall(call);
            if (call.Func.IsConstructor)
                return GenerateConstructorFunctionCall(call);
            string dot = call.Func.IsStatic ? "::" : ".";
            StringBuilder builder = new StringBuilder();
            if (call.Src != null)
                builder.Append(GenerateEvaluable(call.Src) + dot);
            else if (call.IsThisClassFunction)
                builder.Append("this->");

            builder.Append(call.Func.Name);
            builder.Append("(");

            foreach(Evaluable arg in call.Arguments)
            {
                builder.Append(GenerateEvaluable(arg));
                if (arg != call.Arguments.Last())
                    builder.Append(",");
            }

            builder.Append(")");
            return builder.ToString();
        }
        /// <summary>
        /// Génère le code d'un appel de "macro" fonction.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        string GenerateConstructorFunctionCall(FunctionCall call)
        {
            StringBuilder builder = new StringBuilder();
            // Type name
            builder.Append(GenerateTypeInstanceName(call.Func.ReturnType));

            // Arguments
            builder.Append("(");

            foreach (Evaluable arg in call.Arguments)
            {
                builder.Append(GenerateEvaluable(arg));
                if (arg != call.Arguments.Last())
                    builder.Append(",");
            }

            builder.Append(")");

            return builder.ToString();
        }
        /// <summary>
        /// Génère le code d'un appel de "macro" fonction.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        string GenerateMacroFunctionCall(FunctionCall call)
        {
            StringBuilder builder = new StringBuilder();

            Clank.Core.Model.MacroContainer macros = m_project.Macros;
            ClankTypeInstance owner;
            
            if (call.IsConstructor)
                owner = call.Func.ReturnType;
            else
                owner = call.Src.Type;

            Model.MacroContainer.MacroClass klass = macros.FindClassByType(owner.BaseType);
            Clank.Core.Model.MacroContainer.MacroFunction func = klass.Functions[call.Func.GetFullName()]; // TODO : GetFullName() ??

            // Nom de la fonctin native dont les paramètres sont entourés par des $
            if (!func.LanguageToFunctionName.ContainsKey(LANG_KEY))
                throw new Exception("La macro-fonction '" + call.Func.GetFullName() + "' n'est pas renseignée pour le langage '" + LANG_KEY + "'.");
            string nativeFuncName = func.LanguageToFunctionName[LANG_KEY];


            // Remplace les paramètres par leurs values.
            for(int i = 0; i < call.Func.Arguments.Count; i++)
            {
                nativeFuncName = nativeFuncName.Replace(SemanticConstants.ReplaceChr + "(" + call.Func.Arguments[i].ArgName + ")",
                    GenerateEvaluable(call.Arguments[i]));
            }


            // Remplace les params génériques par leurs valeurs.
            for(int i = 0; i < call.Func.Owner.GenericArgumentNames.Count; i++)
            {
                nativeFuncName = nativeFuncName.Replace(SemanticConstants.ReplaceChr + "(" + call.Func.Owner.GenericArgumentNames[i] + ")",
                    GenerateTypeInstanceName(owner.GenericArguments[i]));
            }

            // Remplace @self par le nom de la variable.
            nativeFuncName = nativeFuncName.Replace(SemanticConstants.SelfKW, GenerateEvaluable(call.Src));

            builder.Append(nativeFuncName);
            return builder.ToString().Replace(".[", "[");
        }
        /// <summary>
        /// Génère le code d'un accès à une variable.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        string GenerateVariableAccess(VariableAccess access)
        {
            if(access.Left.Type.GetFullName() == Core.Model.Language.SemanticConstants.StateClass)
            {
                return "this->" + access.VariableName;
            }

            return GenerateEvaluable(access.Left) + "." + access.VariableName;
        }
        /// <summary>
        /// Génère le code d'une variable.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        string GenerateVariable(Variable variable)
        {
            return variable.Name;
        }
        /// <summary>
        /// Génère le code d'un opérateur.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        string GenerateOperator(Operator op)
        {
            switch(op)
            {
                case Operator.Add: return "+";
                case Operator.Affectation: return "=";
                case Operator.And: return "&";
                case Operator.Div: return "/";
                case Operator.Dot: return ".";
                case Operator.Equals: return "==";
                case Operator.LazyAnd: return "&&";
                case Operator.LazyOr: return "||";
                case Operator.Minus: return "-";
                case Operator.Mod: return "%";
                case Operator.Mult: return "*";
                case Operator.New: return "new ";
                case Operator.Not: return "!";
                case Operator.NotEquals: return "!=";
                case Operator.Or: return "|";
                case Operator.Xor: return "^";
            }

            throw new NotImplementedException();
        }
        /// <summary>
        /// Génère une expression de groupe unaire.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        string GenerateUnaryExpressionGroup(UnaryExpressionGroup expr)
        {
            return GenerateOperator(expr.Operator) + " " + GenerateEvaluable(expr.Operand);
        }
        /// <summary>
        /// Génère une expression de groupe binaire.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        string GenerateBinaryExpressionGroup(BinaryExpressionGroup expr, bool parenthesis=true)
        {
            if(parenthesis)
                return "(" + GenerateEvaluable(expr.Operand1) + " " + GenerateOperator(expr.Operator) + " " + GenerateEvaluable(expr.Operand2) + ")";
            else
                return GenerateEvaluable(expr.Operand1) + " " + GenerateOperator(expr.Operator) + " " + GenerateEvaluable(expr.Operand2);
        }
        #endregion

        #region Instructions
        /// <summary>
        /// Génère le code d'une enum.
        /// </summary>
        /// <param name="decl"></param>
        /// <returns></returns>
        string GenerateEnumInstruction(EnumDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("enum class " + decl.Name + "\n{\n");
            foreach(var kvp in decl.Members)
            {
                string member = kvp.Key;
                int value = kvp.Value;
                builder.Append(Tools.StringUtils.Indent(member, 1));
                builder.Append(" = " + value.ToString());
                if (kvp.Key != decl.Members.Last().Key)
                    builder.Append(',');
                builder.Append("\n");
            }
            builder.Append("};\n");
            return builder.ToString();
        }
        /// <summary>
        /// Génère une instruction d'affectation.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateAffectationInstruction(AffectationInstruction instruction)
        {
            return GenerateBinaryExpressionGroup(instruction.Expression, false) + ";";
        }
        /// <summary>
        /// Génère le code de déclaration d'une instruction.
        /// </summary>
        /// <returns></returns>
        string GenerateDeclarationInstruction(VariableDeclarationInstruction instruction)
        {
            return GenerateTypeInstanceName(instruction.Var.Type) + " " + instruction.Var.Name + ";";
        }

        /// <summary>
        /// Génère le code d'une déclaration + affectation d'une variable.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateDeclarationAndAssignmentInstruction(VariableDeclarationAndAssignmentInstruction instruction)
        {
            return GenerateTypeInstanceName(instruction.Declaration.Var.Type) + " " + GenerateAffectationInstruction(instruction.Assignment);
        }
        /// <summary>
        /// Génère une instruction de déclaration de fonction.
        /// </summary>
        /// <returns></returns>
        string GenerateFunctionDeclarationInstruction(FunctionDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();
            if (decl.Func.IsStatic)
                builder.Append("static ");

            if (decl.Func.IsConstructor)
            {
                // Constructeur
                builder.Append(GenerateTypeName(decl.Func.ReturnType.BaseType));
            }
            else
            {
                // Fonction classique
                builder.Append(GenerateTypeInstanceName(decl.Func.ReturnType) + " ");
                builder.Append(decl.Func.Name);
            }
            builder.Append("(");

            // Ajout des arguments.
            foreach(FunctionArgument arg in decl.Func.Arguments)
            {
                bool hasRef = arg.ArgType.BaseType.JType == JSONType.Object ||
                                arg.ArgType.BaseType.JType == JSONType.Array;
                builder.Append(GenerateTypeInstanceName(arg.ArgType) + (hasRef ? "& " : " ") + arg.ArgName);
                if (arg != decl.Func.Arguments.Last())
                    builder.Append(", ");
            }
            builder.AppendLine(");");

            return builder.ToString();
        }

        /// <summary>
        /// Génère une instruction de déclaration de constructeur.
        /// </summary>
        /// <returns></returns>
        string GenerateConstructorDeclarationInstruction(ConstructorDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(decl.Func.Owner.GetFullName() + " ");
            builder.Append("(");

            // Ajout des arguments.
            foreach (FunctionArgument arg in decl.Func.Arguments)
            {
                builder.Append(GenerateTypeInstanceName(arg.ArgType) + " " + arg.ArgName);
                if (arg != decl.Func.Arguments.Last())
                    builder.Append(", ");
            }
            builder.AppendLine(");");

            return builder.ToString();
        }
        #endregion
    }
}
