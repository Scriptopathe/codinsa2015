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
    [LanguageGenerator("python")]
    public class PythonGenerator : ILanguageGenerator
    {
        public const string LANG_KEY = "python";
        #region Variables
        Clank.Core.Model.ProjectFile m_project;
        #endregion

        /// <summary>
        /// Crée une nouvelle instance de CSGenerator avec un fichier projet passé en paramètre.
        /// </summary>
        /// <param name="project"></param>
        public PythonGenerator(Clank.Core.Model.ProjectFile project)
        {
            SetProject(project);
        }
        /// <summary>
        /// Crée une nouvelle instance de CSGenerator.
        /// </summary>
        public PythonGenerator() { }
        /// <summary>
        /// Définit le projet contenant les informations nécessaires à la génération de code.
        /// </summary>
        public void SetProject(Model.ProjectFile project)
        {
            m_project = project;
        }
        /// <summary>
        /// Génére le code d'une classe.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        string GenerateClass(ClassDeclaration declaration)
        {
            StringBuilder builder = new StringBuilder();
            string inheritance = declaration.InheritsFrom == null ? "" : "(" + declaration.InheritsFrom + ")";

            builder.Append("class " + declaration.Name);
            
            // Héritage
            builder.AppendLine(inheritance + ":\n");
            
            // Instructions
            foreach(Instruction instruction in declaration.Instructions)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\n");
            }

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
            else if(instruction is AffectationInstruction)
            {
                return comment + GenerateAffectationInstruction((AffectationInstruction)instruction);
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
            else if(instruction is ReturnInstruction)
            {
                return comment + GenerateReturnInstruction((ReturnInstruction)instruction);
            }
            else if(instruction is Model.Language.Macros.RemoteFunctionWrapper)
            {
                return comment + GenerateRemoteFunctionWrapper((Model.Language.Macros.RemoteFunctionWrapper)instruction);
            }
            else if(instruction is ConditionalStatement)
            {
                return comment + GenerateConditionalStatement((ConditionalStatement)instruction);
            }
            else if(instruction is PlaceholderInstruction)
            {
                return comment;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Génère le code pour des statements conditionnels.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        string GenerateConditionalStatement(ConditionalStatement statement)
        {
            StringBuilder builder = new StringBuilder();
            switch(statement.StatementType)
            {
                case ConditionalStatement.Type.If:
                    builder.Append("if");
                    break;
                case ConditionalStatement.Type.Elsif:
                    builder.Append("elsif");
                    break;
                case ConditionalStatement.Type.Else:
                    builder.Append("else");
                    break;
                case ConditionalStatement.Type.While:
                    builder.Append("while");
                    break;
                default:
                    throw new NotImplementedException();
            }

            // Ajout de la condition
            if(statement.StatementType != ConditionalStatement.Type.Else)
            {
                builder.Append("(");
                builder.Append(GenerateEvaluable(statement.Condition));
                builder.Append(")");
            }

            // Ajout du code
            builder.Append(":\r\n");
            foreach(Instruction inst in statement.Code)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(inst)));
            }
            builder.Append("\r\n");

            return builder.ToString();
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
            builder.Append("def " + func.Name + "(self, ");
            
            // Arg list
            foreach(var arg in func.Arguments)
            {
                builder.Append(arg.ArgName + (arg == func.Arguments.Last() ? "" : ","));
            }
            builder.Append("):\r\n");

            // Corps de la fonction.
            builder.Append("\t#TODO !!!\r\n\r\n");
            /*
            // --- Send
            builder.AppendLine("\t// Send");
            builder.Append("\tList<object> args = new List<object>() { ");
            // Arg list
            foreach (var arg in func.Arguments)
            {
                builder.Append(arg.ArgName + (arg == func.Arguments.Last() ? "" : ","));
            }
            builder.AppendLine("};");
            // Func id
            builder.AppendLine("\tint funcId = " + instruction.Id + ";");
            builder.AppendLine("\tList<object> obj = new List<object>() { funcId, args };");
            // Sending
            builder.AppendLine("\tTCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));");

            // --- Receive
            builder.AppendLine("\t// Receive");
            builder.AppendLine("\tstring str = TCPHelper.Receive();");
            builder.AppendLine("\tNewtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);");


            // Object
            if (func.ReturnType.BaseType.JType == JSONType.Object || func.ReturnType.BaseType.JType == JSONType.Array)
                builder.Append("\treturn (" + returnTypeName + ")o[0].ToObject(typeof(" + returnTypeName + "));\r\n");
            else // Value
                builder.Append("\treturn o.Value<" + returnTypeName + ">(0);\r\n");

            builder.AppendLine("}\r\n\r\n");*/
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
                builder.AppendLine("\"\"\"");
                foreach (string line in lines)
                {
                    builder.AppendLine(line);
                }
                builder.AppendLine("\"\"\"");
            }
            else
            {
                // Commentaire en plusieurs lignes.
                foreach (string line in lines)
                {
                    builder.AppendLine("# " + line);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Génère le code pour une instruction return.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateReturnInstruction(ReturnInstruction instruction)
        {
            return "return " + GenerateEvaluable(instruction.Value) + ";";
        }

        /// <summary>
        /// Génère le code pour la fonction de traitement des messages.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateProcessMessageMacro(Model.Language.Macros.ProcessMessageMacro instruction)
        {
            StringBuilder builder = new StringBuilder();
            return "#TODO !!!";
            builder.AppendLine("public string ProcessRequest(string request, int " + Clank.Core.Model.Language.SemanticConstants.ClientID + ")\r\n{");
            builder.AppendLine("\tNewtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(request);");
            builder.AppendLine("\tint functionId = o.Value<int>(0);");
            builder.AppendLine("\tswitch(functionId)\r\n\t{");
            int id = 0;
            List<FunctionDeclaration> decls = new List<FunctionDeclaration>();
            decls.AddRange(instruction.Access.Declarations);
            decls.AddRange(instruction.Write.Declarations);

            // Switch dans lequel on traite les différents messages.
            foreach(FunctionDeclaration func in decls)
            {
                StringBuilder argList = new StringBuilder();
                builder.AppendLine("\t\tcase " + id + ":");
                int argId = 0;
                foreach(FunctionArgument arg in func.Func.Arguments)
                {
                    // Variable name and type.
                    string varName = "arg" + id + "_" + argId;
                    string typeName = GenerateTypeInstanceName(arg.ArgType);
                    builder.Append("\t\t\t" + typeName + " " + varName + " = ");

                    // Object
                    if(arg.ArgType.BaseType.JType == JSONType.Object || arg.ArgType.BaseType.JType == JSONType.Array)
                        builder.Append("(" + typeName + ")o[1][" + argId + "].ToObject(typeof(" + typeName + "));\r\n");
                    else // Value
                        builder.Append("o[1].Value<" + typeName + ">(" + argId + ");\r\n");


                    argList.Append(varName + ", ");
                    argId++;
                }
                argList.Append(Model.Language.SemanticConstants.ClientID);
                // Function call and return
                builder.Append("\t\t\treturn Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { " + func.Func.Name + "(" + argList + ")" + " });\r\n");
                
                id++;
            }

            builder.Append("\t}\r\n");
            builder.Append("\treturn \"\";\r\n}\r\n");

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
            if (type.IsGeneric)
            {
                return GenerateTypeName(type.BaseType);
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
                return mcClass.LanguageToTypeName[LANG_KEY];
            }
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

            StringBuilder builder = new StringBuilder();
            if (call.Src != null)
                builder.Append(GenerateEvaluable(call.Src) + ".");

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
            if (call.Src != null)
                builder.Append(GenerateEvaluable(call.Src) + ".");

            Clank.Core.Model.MacroContainer macros = m_project.Macros;
            var klass = macros.FindClassByType(call.Src.Type.BaseType);
            Clank.Core.Model.MacroContainer.MacroFunction func = klass.Functions[call.Func.Name]; // TODO : GetFullName() ??

            // Nom de la fonctin native dont les paramètres sont entourés par des $
            string nativeFuncName = func.LanguageToFunctionName[LANG_KEY];

            // Remplace les paramètres par leurs values.
            for(int i = 0; i < call.Func.Arguments.Count; i++)
            {
                nativeFuncName = nativeFuncName.Replace("$" + call.Func.Arguments[i].ArgName + "$",
                    GenerateEvaluable(call.Arguments[i]));
            }

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
                return "self." + access.VariableName;
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
                case Operator.LazyAnd: return "and";
                case Operator.LazyOr: return "or";
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
            builder.Append("public enum " + decl.Name + "\n{\n");
            foreach (var kvp in decl.Members)
            {
                string member = kvp.Key;
                int value = kvp.Value;
                builder.Append(Tools.StringUtils.Indent(member, 1));
                builder.Append(" = " + value.ToString());
                if (kvp.Key != decl.Members.Last().Key)
                    builder.Append(',');
                builder.Append("\n");
            }
            builder.Append("}\n");
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
        /// Génère le code de déclaration d'une variable.
        /// </summary>
        /// <returns></returns>
        string GenerateDeclarationInstruction(VariableDeclarationInstruction instruction)
        {
            return "#Decl ?";
            StringBuilder builder = new StringBuilder();
            foreach(string modifier in instruction.Modifiers)
            {
                builder.Append(modifier + " ");
            }

            return builder.ToString() + GenerateTypeInstanceName(instruction.Var.Type) + " " + instruction.Var.Name + ";";
        }

        /// <summary>
        /// Génère le code d'une déclaration + affectation d'une variable.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateDeclarationAndAssignmentInstruction(VariableDeclarationAndAssignmentInstruction instruction)
        {
            return GenerateAffectationInstruction(instruction.Assignment);
        }
        /// <summary>
        /// Génère une instruction de déclaration de fonction.
        /// </summary>
        /// <returns></returns>
        string GenerateFunctionDeclarationInstruction(FunctionDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();

            if (decl.Func.IsConstructor)
            {
                // Constructeur
                builder.Append(GenerateTypeName(decl.Func.ReturnType.BaseType));
            }
            else
            {
                // Fonction classique
                builder.Append("def ");
                builder.Append(decl.Func.Name);
            }
            builder.Append("(");

            // Ajout des arguments.
            foreach(FunctionArgument arg in decl.Func.Arguments)
            {
                builder.Append(GenerateTypeInstanceName(arg.ArgType) + " " + arg.ArgName);
                if (arg != decl.Func.Arguments.Last())
                    builder.Append(", ");
            }
            builder.Append("):\r\n");

            // Ajout des instructions.
            foreach(Instruction instruction in decl.Code)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\r\n");
            }

            builder.Append("\r\n");
            return builder.ToString();
        }

        /// <summary>
        /// Génère une instruction de déclaration de constructeur.
        /// </summary>
        /// <returns></returns>
        string GenerateConstructorDeclarationInstruction(ConstructorDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("def __init__(self, ");

            // Ajout des arguments.
            foreach (FunctionArgument arg in decl.Func.Arguments)
            {
                builder.Append(arg.ArgName);
                if (arg != decl.Func.Arguments.Last())
                    builder.Append(", ");
            }
            builder.Append("):\r\n");

            // Ajout des instructions.
            foreach (Instruction instruction in decl.Func.Code)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\r\n");
            }

            builder.Append("\r\n");
            return builder.ToString();
        }
        #endregion
    }
}
