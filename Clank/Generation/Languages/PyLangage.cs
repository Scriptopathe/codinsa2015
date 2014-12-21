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
    public class PyGenerator_DO_NOT_USE_PLEASE_FOR_GOD_SAKE
    {
        /// <summary>
        /// Génére le code d'une classe.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static string GenerateClass(ClassDeclaration declaration)
        {
            StringBuilder builder = new StringBuilder();
            string inheritance = declaration.InheritsFrom == null ? "" : "(" + declaration.InheritsFrom + ")";

            builder.Append("class " + declaration.Name + inheritance  + ":\n");
            
            
            // Instructions
            List<Instruction> instanceVariableDeclarations = declaration.Instructions.FindAll(new Predicate<Instruction>(
                delegate(Instruction ins)
                {
                    return ins is VariableDeclarationInstruction || ins is VariableDeclarationAndAssignmentInstruction;
                }
                ));
            List<Instruction> instanceMethodsDeclaration = declaration.Instructions.FindAll(new Predicate<Instruction>(
                delegate(Instruction ins)
                {
                    return ins is FunctionDeclaration || ins is ConstructorDeclaration;
                }
                ));


            // Variables d'instance
            builder.Append(Tools.StringUtils.Indent("def __init_locals(self):\n"));
            if (instanceVariableDeclarations.Count == 0)
                builder.Append(Tools.StringUtils.Indent("pass\n", 2));

            foreach(Instruction ins in instanceVariableDeclarations)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(ins), 2));
                builder.Append("\n");
            }
            builder.Append("\n");

            // Méthodes d'instance.
            foreach(Instruction instruction in instanceMethodsDeclaration)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\n");
            }

            builder.Append("\n");

            return builder.ToString();
        }

        /// <summary>
        /// Génère le code d'une instruction.
        /// </summary>
        public static string GenerateInstruction(Instruction instruction)
        {
            if(instruction is FunctionDeclaration)
            {
                return GenerateFunctionDeclarationInstruction((FunctionDeclaration)instruction);
            }
            else if(instruction is ConstructorDeclaration)
            {
                return GenerateConstructorDeclarationInstruction((ConstructorDeclaration)instruction);
            }
            else if(instruction is ClassDeclaration)
            {
                return GenerateClass((ClassDeclaration)instruction);
            }
            else if(instruction is AffectationInstruction)
            {
                return GenerateAffectationInstruction((AffectationInstruction)instruction);
            }
            else if(instruction is VariableDeclarationInstruction)
            {
                return GenerateDeclarationInstruction((VariableDeclarationInstruction)instruction);
            }
            else if(instruction is VariableDeclarationAndAssignmentInstruction)
            {
                return GenerateDeclarationAndAssignmentInstruction((VariableDeclarationAndAssignmentInstruction)instruction);
            }
            else if(instruction is FunctionCallInstruction)
            {
                return GenerateFunctionCall(((FunctionCallInstruction)instruction).Call) + ";";
            }
            else if(instruction is EnumDeclaration)
            {
                return GenerateEnumInstruction((EnumDeclaration)instruction);
            }

            // throw new NotImplementedException();
            return "# NotImplemented Token : " + instruction.GetType().ToString();
        }
        #region Expressions
        /// <summary>
        /// Génère une expression évaluable.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        static string GenerateEvaluable(Evaluable expr)
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
                return ((BoolLiteral)expr).Value.ToString();
            }
            else if(expr is StringLiteral)
            {
                return '"' + ((StringLiteral)expr).Value + '"';
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Génère le code représentant l'instance de type passé en paramètre.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string GenerateTypeInstanceName(ClankTypeInstance type)
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
        static string GenerateTypeName(ClankType type)
        {
            return type.Name;
        }
        /// <summary>
        /// Génère le code d'un accès à un membre d'une énumération.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        static string GenerateEnumAccess(EnumAccess access)
        {
            return access.Type.BaseType.GetFullName() + "." + access.Name;
        }
        /// <summary>
        /// Génère le code d'un appel de fonction.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        static string GenerateFunctionCall(FunctionCall call)
        {
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
        /// Génère le code d'un accès à une variable.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        static string GenerateVariableAccess(VariableAccess access)
        {
            // TODO : gérer state etc...s
            return GenerateEvaluable(access.Left) + "." + access.VariableName;
        }
        /// <summary>
        /// Génère le code d'une variable.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        static string GenerateVariable(Variable variable)
        {
            return variable.Name;
        }
        /// <summary>
        /// Génère le code d'un opérateur.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        static string GenerateOperator(Operator op)
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
        static string GenerateUnaryExpressionGroup(UnaryExpressionGroup expr)
        {
            return GenerateOperator(expr.Operator) + " " + GenerateEvaluable(expr.Operand);
        }
        /// <summary>
        /// Génère une expression de groupe binaire.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        static string GenerateBinaryExpressionGroup(BinaryExpressionGroup expr, bool parenthesis=true)
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
        static string GenerateEnumInstruction(EnumDeclaration decl)
        {

            throw new NotImplementedException();
        }
        /// <summary>
        /// Génère une instruction d'affectation.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        static string GenerateAffectationInstruction(AffectationInstruction instruction)
        {
            return GenerateBinaryExpressionGroup(instruction.Expression, false);
        }
        /// <summary>
        /// Génère le code de déclaration d'une instruction.
        /// </summary>
        /// <returns></returns>
        static string GenerateDeclarationInstruction(VariableDeclarationInstruction instruction)
        {
            return instruction.Var.Name + " = Nil";
        }

        /// <summary>
        /// Génère le code d'une déclaration + affectation d'une variable.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        static string GenerateDeclarationAndAssignmentInstruction(VariableDeclarationAndAssignmentInstruction instruction)
        {
            return GenerateAffectationInstruction(instruction.Assignment);
        }
        /// <summary>
        /// Génère une instruction de déclaration de fonction.
        /// </summary>
        /// <returns></returns>
        static string GenerateFunctionDeclarationInstruction(FunctionDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("def ");
            builder.Append(decl.Func.Name);
            builder.Append("(");

            // Ajout des arguments.
            if (decl.Func.Owner != null)
                builder.Append("self" + (decl.Func.Arguments.Count != 0 ? "," : ""));
            
            foreach(FunctionArgument arg in decl.Func.Arguments)
            {
                builder.Append(arg.ArgName);
                if (arg != decl.Func.Arguments.Last())
                    builder.Append(", ");
            }
            builder.Append("):\n");

            // Ajout des instructions.
            if (decl.Code.Count == 0)
                builder.AppendLine(Tools.StringUtils.Indent("pass"));
            foreach(Instruction instruction in decl.Code)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\n");
            }

            builder.Append("\n");
            return builder.ToString();
        }

        /// <summary>
        /// Génère une instruction de déclaration de constructeur.
        /// </summary>
        /// <returns></returns>
        static string GenerateConstructorDeclarationInstruction(ConstructorDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("def __init__ ");
            builder.Append("(");

            // Ajout de self
            if (decl.Func.Owner != null)
                builder.Append("self" + (decl.Func.Arguments.Count != 0 ? "," : ""));
            // Ajout des arguments.
            foreach (FunctionArgument arg in decl.Func.Arguments)
            {
                builder.Append(arg.ArgName);
                if (arg != decl.Func.Arguments.Last())
                    builder.Append(", ");
            }
            builder.Append("):\n");

            // Ajout des instructions.
            if (decl.Func.Code.Count == 0)
                builder.AppendLine(Tools.StringUtils.Indent("pass"));
            foreach (Instruction instruction in decl.Func.Code)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\n");
            }

            builder.Append("\n");
            return builder.ToString();
        }
        #endregion
    }
}
