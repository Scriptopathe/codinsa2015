using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Tokenizers
{
    /// <summary>
    /// Représente un jeton pouvant englober des expressions.
    /// </summary>
    public class ExpressionToken
    {
        /// <summary>
        /// Enumère les types de jetons que l'on peut trouver dans une expression.
        /// </summary>
        public enum ExpressionTokenType
        {
            Comment,
            NumberLiteral,         // 56
            StringLiteral,         // "jiji"
            BoolLiteral,           // true, false
            Modifier,              // @, $, etc...
            Access,                // arg1.thing
            Name,                  // thing
            List,                  // (Bidule, machin, ;)
            ArgList,               // (arg1, arg2, arg3)
            ExpressionGroup,       // (arg1+arg2.potato)
            EndOfInstruction,      // ;
            InstructionList,       // 
            Separator,             // ,
            CodeBlock,             // { }
            NamedCodeBlock,        // haha{  }
            NamedGenericCodeBlock, // haha<T, T1> { }
            GenericParametersList, // <Param1, Param2>
            GenericType,           // Type<Param1, Param2>
            BracketList,           // [param1, param2]
            ArrayType,             // arr[param1, param2]
            FunctionCall,          // 
            FunctionDeclaration,   // FunctionCall + CodeBlock
            Operator,              // +, *, <, <=, ! etc...
            New,                   // new
            ConditionalStatement,  // if, else, elsif, while
        }
        ExpressionTokenType m_tkType;
        /// <summary>
        /// Obtient ou définit le type de ce jeton.
        /// </summary>
        public ExpressionTokenType TkType
        {
            get
            {
                return m_tkType;
            }
            set
            {
                m_tkType = value;
                switch(m_tkType)
                {
                    case ExpressionTokenType.ExpressionGroup:
                        SubTokens = new List<ExpressionToken>() { null, null, null };
                        break;
                }
            }
        }

        /// <summary>
        /// Obtient ou définit la "valeur" du jeton.
        /// </summary>
        public string Content
        {
            get;
            set;
        }


        /// <summary>
        /// Crée une nouvelle instance d'expressionToken.
        /// </summary>
        public ExpressionToken()
        {
            Content = "";
            SubTokens = new List<ExpressionToken>();
        }
        #region SubTokens
        /// <summary>
        /// Obtient ou définit les sous jetons associés à ce jeton.
        /// </summary>
        List<ExpressionToken> SubTokens
        {
            get;
            set;
        }

        /// <summary>
        /// Retourne le jeton enfant pour les types suivant :
        ///     - Access (arg1.arg2) => arg1
        ///     - 
        /// </summary>
        public ExpressionToken ChildToken
        {
            get { return SubTokens[0]; }
            set { SubTokens[0] = value; }
        }

        /// <summary>
        /// Obtient ou définit la liste de jetons associé à un jeton pouvant en contenir d'autres.
        /// </summary>
        public List<ExpressionToken> ListTokens
        {
            get 
            {
                return SubTokens;
            }
            set 
            {
                SubTokens = value;
            }
        }

        /// <summary>
        /// Obtient le jeton opérateur si ce jeton est de type ExpressionGroup.
        /// </summary>
        public ExpressionToken Operator
        {
            get
            {
                if (TkType == ExpressionTokenType.ExpressionGroup)
                    return SubTokens[0];
                else
                    throw new InvalidOperationException();
            }
            set
            {
                if (TkType == ExpressionTokenType.ExpressionGroup)
                    SubTokens[0] = value;
                else
                    throw new InvalidOperationException();
            }
        }


        /// <summary>
        /// Obtient le premier jeton opérande si ce jeton est de type expression group.
        /// </summary>
        public ExpressionToken Operands1
        {
            get
            {
                if (TkType == ExpressionTokenType.ExpressionGroup)
                    return SubTokens[1];
                else
                    throw new InvalidOperationException();
            }
            set
            {
                if (TkType == ExpressionTokenType.ExpressionGroup)
                    SubTokens[1] = value;
                else
                    throw new InvalidOperationException();
            }
        }
        /// <summary>
        /// Obtient le premier jeton opérande si ce jeton est de type expression group et que l'opérateur
        /// associé est binaire.
        /// </summary>
        public ExpressionToken Operands2
        {
            get
            {
                if (TkType == ExpressionTokenType.ExpressionGroup && Operator.IsBinaryOperator)
                    return SubTokens[2];
                else
                    throw new InvalidOperationException();
            }
            set
            {
                if (TkType == ExpressionTokenType.ExpressionGroup && Operator.IsBinaryOperator)
                    SubTokens[2] = value;
                else
                    throw new InvalidOperationException();
            }
        }
        /// <summary>
        /// Mappe les opérateurs avec leur priorité.
        /// </summary>
        static Dictionary<string, int> s_priorities = new Dictionary<string, int>()
        {
            {"=", 0}, {"*=", 0}, {"/=", 0}, {"-=", 0}, {"+=", 0},
            {"&", 1}, {"&&", 1}, {"|", 1}, {"||", 1},
            {"<", 2}, {"<=", 2}, {">", 2}, {">=", 2}, {"==", 2}, {"!=", 2},
            {"+", 3}, {"-", 3},
            {"*", 4}, {"/", 4},
            {"!", 6}, // {"new", 6},  // unaire
            {".", 7}
        };
        
        /// <summary>
        /// Pour un jeton de type opérateur, retourne sa priorité.
        /// </summary>
        public int Priority
        {
            get 
            {
                if (TkType != ExpressionTokenType.Operator)
                    throw new InvalidOperationException();
                return s_priorities[Content]; 
            }
        }
        /// <summary>
        /// Pour un jeton de type opérateur, retourne vrai s'il est binaire.
        /// </summary>
        public bool IsBinaryOperator
        {
            get
            {
                return TkType == ExpressionTokenType.Operator && Priority != 6;
            }
        }
        /// <summary>
        /// Pour un jeton de type opérateur, retourne vrai s'il est unaire.
        /// </summary>
        public bool IsUnaryOperator
        {
            get
            {
                return TkType == ExpressionTokenType.Operator && Priority == 6;
            }
        }

        #region Function call
        /// <summary>
        /// Obtient, pour un jeton de type FunctionCall, l'identifier de la fonction appelée.
        /// </summary>
        public ExpressionToken FunctionCallIdentifier
        {
            get
            {
                AssertType(this, ExpressionTokenType.FunctionCall);
                return SubTokens[0];
            }
        }

        /// <summary>
        /// Retourne la liste d'arguments de cette fonction.
        /// </summary>
        public ExpressionToken FunctionCallArgs
        {
            get
            {
                AssertType(this, ExpressionTokenType.FunctionCall);
                AssertType(SubTokens[1], ExpressionTokenType.ArgList);
                return SubTokens[1];
            }
        }
        #endregion

        #region Debug
        /// <summary>
        /// Fichier source contenant ce jeton.
        /// </summary>
        public string Source
        {
            get;
            set;
        }
        /// <summary>
        /// Ligne du fichier source contenant ce jeton.
        /// </summary>
        public int Line
        {
            get;
            set;
        }

        /// <summary>
        /// Caractère contenant ce jeton.
        /// </summary>
        public int Character
        {
            get;
            set;
        }
        #endregion

        #region Function Declaration
        /// <summary>
        /// Obtient, pour un jeton de type FunctionDeclaration, l'identifier de la fonction appelée.
        /// </summary>
        public ExpressionToken FunctionDeclIdentifier
        {
            get
            {
                AssertType(this, ExpressionTokenType.FunctionDeclaration);
                return SubTokens[0].FunctionCallIdentifier;
            }
        }
        /// <summary>
        /// Obtient les arguments de la déclaration de fonction.
        /// </summary>
        public ExpressionToken FunctionDeclArgs
        {
            get
            {
                AssertType(this, ExpressionTokenType.FunctionDeclaration);
                return SubTokens[0].FunctionCallArgs;
            }
        }
        /// <summary>
        /// Obtient le code contenu dans cette fonction.
        /// </summary>
        public ExpressionToken FunctionDeclCode
        {
            get
            {
                AssertType(this, ExpressionTokenType.FunctionDeclaration);
                AssertType(SubTokens[1], ExpressionTokenType.InstructionList);
                return SubTokens[1];
            }
        }
        #endregion

        #region Array Type
        /// <summary>
        /// Obtient, pour un jeton de type ArrayType, l'identifier du type.
        /// </summary>
        public ExpressionToken ArrayTypeIdentifier
        {
            get
            {
                AssertType(this, ExpressionTokenType.ArrayType);
                return SubTokens[0];
            }
        }

        /// <summary>
        /// Obtient la liste de paramètres de ce jeton de type ArrayType.
        /// </summary>
        public ExpressionToken ArrayTypeArgs
        {
            get
            {
                AssertType(this, ExpressionTokenType.ArrayType);
                AssertType(SubTokens[1], ExpressionTokenType.ArgList);
                return SubTokens[1];
            }
        }
        #endregion

        #region Generic Type
        /// <summary>
        /// Obtient, pour un jeton de type GenericType, l'identifier du type.
        /// </summary>
        public ExpressionToken GenericTypeIdentifier
        {
            get
            {
                AssertType(this, ExpressionTokenType.GenericType);
                return SubTokens[0];
            }
        }

        /// <summary>
        /// Obtient la liste de paramètres de ce jeton de type générique.
        /// Type : ArgList.
        /// </summary>
        public ExpressionToken GenericTypeArgs
        {
            get
            {
                AssertType(this, ExpressionTokenType.GenericType);
                AssertType(SubTokens[1], ExpressionTokenType.ArgList);
                return SubTokens[1];
            }
        }
        #endregion

        #region Code Block
        /// <summary>
        /// Obtient l'identifier associé à ce block de code nommé.
        /// </summary>
        public ExpressionToken NamedCodeBlockIdentifier
        {
            get
            {
                AssertType(this, ExpressionTokenType.NamedCodeBlock);
                return SubTokens[0];
            }
        }

        /// <summary>
        /// Obtient les instructions associées à ce block de code nommé.
        /// Le jeton retourné est un InstructionList.
        /// </summary>
        public ExpressionToken NamedCodeBlockInstructions
        {
            get
            {
                AssertType(this, ExpressionTokenType.NamedCodeBlock);
                AssertType(SubTokens[1], ExpressionTokenType.InstructionList);
                return SubTokens[1];
            }
        }
        #endregion

        #region Named generic Code Block
        /// <summary>
        /// Obtient l'identifier associé à ce block de code générique nommé.
        /// </summary>
        public ExpressionToken NamedGenericCodeBlockIdentifier
        {
            get
            {
                AssertType(this, ExpressionTokenType.NamedGenericCodeBlock);
                return SubTokens[0];
            }
        }
        /// <summary>
        /// Obtient le nom associé à ce block de code nommé.
        /// </summary>
        public ExpressionToken NamedGenericCodeBlockNameIdentifier
        {
            get
            {
                AssertType(this, ExpressionTokenType.NamedGenericCodeBlock);
                return SubTokens[0].GenericTypeIdentifier;
            }
        }
        /// <summary>
        /// Obtient la liste des arguments génériques associée à ce block de code nommé.
        /// Type : ArgList.
        /// </summary>
        public ExpressionToken NamedGenericCodeBlockArgs
        {
            get
            {
                AssertType(this, ExpressionTokenType.NamedGenericCodeBlock);
                return SubTokens[0].GenericTypeArgs;
            }
        }
        /// <summary>
        /// Obtient les instructions associées à ce block de code générique nommé.
        /// Le jeton retourné est un InstructionList.
        /// </summary>
        public ExpressionToken NamedGenericCodeBlockInstructions
        {
            get
            {
                AssertType(this, ExpressionTokenType.NamedGenericCodeBlock);
                AssertType(SubTokens[1], ExpressionTokenType.InstructionList);
                return SubTokens[1];
            }
        }
        #endregion

        #endregion
        /// <summary>
        /// Retourne vrai si le jeton d'expression est évaluable.
        /// </summary>
        public bool IsEvaluable
        {
            get
            {
                return (TkType == ExpressionTokenType.ExpressionGroup) | (TkType == ExpressionTokenType.NumberLiteral) |
                    (TkType == ExpressionTokenType.Access) | (TkType == ExpressionTokenType.Name);
            }
        }

        /// <summary>
        /// Retourne le nom complet du type potentiel encapsulé par ce jeton.
        /// </summary>
        /// <returns></returns>
        public string GetTypeFullName()
        {
            if (TkType == ExpressionTokenType.GenericType)
            {
                StringBuilder b = new StringBuilder();
                foreach(var arg in GenericTypeArgs.ListTokens)
                {
                    b.Append(arg.GetTypeFullName());
                    if (arg != GenericTypeArgs.ListTokens.Last())
                        b.Append(",");
                }
                return GenericTypeIdentifier.GetTypeFullName() + "<" + b.ToString() + ">";
            }
            else if(TkType == ExpressionTokenType.List)
            {
                StringBuilder b = new StringBuilder();
                foreach (var arg in ListTokens)
                {
                    b.Append(arg.GetTypeFullName());
                    if (arg != ListTokens.Last())
                        b.Append(",");
                }
                return b.ToString();
            }
            else if(TkType == ExpressionTokenType.Name)
            {
                return Content;
            }
            else if(TkType == ExpressionTokenType.ArrayType)
            {
                return ArrayTypeIdentifier.GetTypeFullName() + "[]";
            }
            else
                throw new InvalidOperationException();
        }

        #region To String etc..
        /// <summary>
        /// DEBUG.
        /// </summary>
        public string Debug_Code
        {
            get { return ToDebugCode(); }
        }
        /// <summary>
        /// Traduit le jeton en "code" lisible.
        /// </summary>
        /// <returns></returns>
        public string ToDebugCode()
        {
            Func<string, string> getChildrenCode = delegate(string separator)
            {
                StringBuilder b = new StringBuilder();
                for(int i = 0; i < SubTokens.Count; i++)
                {
                    b.Append(SubTokens[i].ToDebugCode());
                    if (i != SubTokens.Count - 1)
                        b.Append(separator);
                }
                return b.ToString();
            };

            switch(TkType)
            {
                case ExpressionTokenType.BracketList:
                    return "l[" + getChildrenCode(",") + "]";
                case ExpressionTokenType.FunctionCall:
                    return FunctionCallIdentifier.ToDebugCode() + ".call(" + FunctionCallArgs.ToDebugCode() + ")";
                case ExpressionTokenType.GenericType:
                    return GenericTypeIdentifier.ToDebugCode() + ".gen<" + GenericTypeArgs.ToDebugCode() + ">";
                case ExpressionTokenType.ArrayType:
                    return ArrayTypeIdentifier.ToDebugCode() + ".arr[" + ArrayTypeArgs.ToDebugCode() + "]";
                case ExpressionTokenType.List:
                    return "l:(" + getChildrenCode(" _ ") + ")";
                case ExpressionTokenType.ArgList:
                    return "args:(" + getChildrenCode(",") + ")";
                case ExpressionTokenType.GenericParametersList:
                    return "l<" + getChildrenCode(",") + ">";
                case ExpressionTokenType.Name:
                    return this.Content;
                case ExpressionTokenType.CodeBlock:
                    return "{ " + getChildrenCode(",") + "}";
                case ExpressionTokenType.FunctionDeclaration:
                    return "decl:" + SubTokens[0].ToDebugCode() + "{\n" + Tools.StringUtils.Indent(SubTokens[1].ToDebugCode(), 1) + "\n}";
                case ExpressionTokenType.EndOfInstruction:
                    return "/" + this.Content;
                case ExpressionTokenType.InstructionList:
                    return "\ninstruction_list:(\n" + getChildrenCode("?;\n") + ")\n";
                case ExpressionTokenType.ExpressionGroup:
                    if (Operator.IsBinaryOperator)
                        return "(" + Operands1.ToDebugCode() + Operator.ToDebugCode() + Operands2.ToDebugCode() + ")";
                    else
                        return Operator.ToDebugCode() + Operands1.ToDebugCode();
                case ExpressionTokenType.NamedCodeBlock:
                    return "block:" + SubTokens[0].ToDebugCode() + "{\n" + Tools.StringUtils.Indent(SubTokens[1].ToDebugCode(), 1) + "}\n";
                case ExpressionTokenType.NamedGenericCodeBlock:
                    return "gen_block:" + NamedGenericCodeBlockIdentifier.ToDebugCode() +
                        "{\n" + Tools.StringUtils.Indent(NamedGenericCodeBlockInstructions.ToDebugCode(), 1) + "}\n";
                case ExpressionTokenType.NumberLiteral:
                    return this.Content;
                case ExpressionTokenType.Operator:
                    return this.Content;
                case ExpressionTokenType.Separator:
                    return "/" + this.Content;
                case ExpressionTokenType.StringLiteral:
                    return "\"" + this.Content + "\"";
                case ExpressionTokenType.Modifier:
                    return this.Content;
                case ExpressionTokenType.ConditionalStatement:
                    return this.Content;
                default:
                    return "??";
            }
        }

        /// <summary>
        /// Traduit le jeton en code lisible.
        /// </summary>
        /// <returns></returns>
        public string ToReadableCode()
        {
            Func<string, string> getChildrenCode = delegate(string separator)
            {
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < SubTokens.Count; i++)
                {
                    b.Append(SubTokens[i].ToReadableCode());
                    if (i != SubTokens.Count - 1)
                        b.Append(separator);
                }
                return b.ToString();
            };

            Func<string, string> getChildrenCode2 = delegate(string ending)
            {
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < SubTokens.Count; i++)
                {
                    b.Append(SubTokens[i].ToReadableCode());
                    b.Append(ending);
                }
                return b.ToString();
            };

            switch (TkType)
            {
                case ExpressionTokenType.BracketList:
                    return "[" + getChildrenCode(",") + "]";
                case ExpressionTokenType.FunctionCall:
                    return FunctionCallIdentifier.ToReadableCode() + "(" + FunctionCallArgs.ToReadableCode() + ")";
                case ExpressionTokenType.GenericType:
                    return GenericTypeIdentifier.ToReadableCode() + "<" + GenericTypeArgs.ToReadableCode() + ">";
                case ExpressionTokenType.ArrayType:
                    return ArrayTypeIdentifier.ToReadableCode() + "[" + ArrayTypeArgs.ToReadableCode() + "]";
                case ExpressionTokenType.List:
                    return getChildrenCode(" ");
                case ExpressionTokenType.ArgList:
                    return getChildrenCode(",");
                case ExpressionTokenType.GenericParametersList:
                    return "<" + getChildrenCode(",") + ">";
                case ExpressionTokenType.Name:
                    return this.Content;
                case ExpressionTokenType.CodeBlock:
                    return "{ " + getChildrenCode(",") + " }";
                case ExpressionTokenType.NamedCodeBlock:
                    return SubTokens[0].ToReadableCode()  + "\n{\n" + Tools.StringUtils.Indent(SubTokens[1].ToReadableCode(), 1) + "\n}";
                case ExpressionTokenType.FunctionDeclaration:
                    return SubTokens[0].ToReadableCode() + "\n{\n" + Tools.StringUtils.Indent(SubTokens[1].ToReadableCode(), 1) + "\n}";
                case ExpressionTokenType.NamedGenericCodeBlock:
                    return NamedGenericCodeBlockIdentifier.ToReadableCode() +
                        "{\n" + Tools.StringUtils.Indent(NamedGenericCodeBlockInstructions.ToReadableCode(), 1) + "\n}";
                case ExpressionTokenType.EndOfInstruction:
                    return this.Content + "\n";
                case ExpressionTokenType.InstructionList:
                    return getChildrenCode2(";\n");
                case ExpressionTokenType.ExpressionGroup:
                    if (Operator.IsBinaryOperator)
                        if(Operator.Content == "=")
                            return Operands1.ToReadableCode() + Operator.ToReadableCode() + Operands2.ToReadableCode();
                        else
                            return "(" + Operands1.ToReadableCode() + Operator.ToReadableCode() + Operands2.ToReadableCode() + ")";
                    else
                        return Operator.ToReadableCode() + Operands1.ToReadableCode();
                case ExpressionTokenType.NumberLiteral:
                    return this.Content;
                case ExpressionTokenType.Operator:
                    return this.Content;
                case ExpressionTokenType.Separator:
                    return this.Content;
                case ExpressionTokenType.BoolLiteral:
                    return this.Content;
                case ExpressionTokenType.StringLiteral:
                    return "\"" + this.Content + "\"";
                case ExpressionTokenType.Modifier:
                    return this.Content;
                default:
                    return "??";
            }
        }
        /// <summary>
        /// Transforme cet ExpressionToken en string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (TkType == ExpressionTokenType.List || TkType == ExpressionTokenType.GenericParametersList || TkType == ExpressionTokenType.BracketList)
            {
                StringBuilder children = new StringBuilder();
                foreach (ExpressionToken token in ListTokens)
                {
                    children.Append(token.ToString() + ",");
                }
                return "<type=" + TkType.ToString() + ";content='" + Content.ToString() + "';children={" + children.ToString() + "}>";
            }
            else
            {
                return "<type=" + TkType.ToString() + ";content='" + Content.ToString() + "'>";
            }
        }

        /// <summary>
        /// Traduit le jeton en code lisible.
        /// </summary>
        /// <returns></returns>
        public string ToXML()
        {
            Func<string, string> i = delegate(string str)
            {
                return Tools.StringUtils.Indent(str, 1);
            };
            Func<string, string, string> inlineencapsulate = delegate(string str, string delim)
            {
                return "<" + delim + ">" + str + "</" + delim + ">\n";
            };
            Func<string, string, string> encapsulate = delegate(string str, string delim)
            {
                return "<" + delim + ">\n" + i(str) + "\n</" + delim + ">\n";
            };
            Func<string, string> getChildrenCode = delegate(string name)
            {
                StringBuilder b = new StringBuilder();
                b.Append("<" + name + ">\n");
                for (int j = 0; j < SubTokens.Count; j++)
                {
                    b.AppendLine(i(SubTokens[j].ToXML()));
                }
                b.Append("</" + name + ">\n");
                return b.ToString();
            };

            switch (TkType)
            {
                case ExpressionTokenType.BracketList:
                    return getChildrenCode("BracketList");
                case ExpressionTokenType.FunctionCall:
                    return "<FunctionCall>\n" +
                        i("<Identifier>\n" + i(FunctionCallIdentifier.ToXML()) + "</Identifier>\n") +
                        i(FunctionCallArgs.ToXML()) +
                        "</FunctionCall>";
                case ExpressionTokenType.GenericType:
                    return "<GenericType>\n" +
                        i("<Identifier>\n" + i(GenericTypeIdentifier.ToXML()) + "</Identifier>\n") +
                        i(GenericTypeArgs.ToXML()) +
                        "</GenericType>";
                case ExpressionTokenType.ArrayType:
                    return "<ArrayType>\n" +
                        i("<Identifier>\n" + i(ArrayTypeIdentifier.ToXML()) + "</Identifier>\n") +
                        i(ArrayTypeArgs.ToXML()) + 
                        "</ArrayType>";
                case ExpressionTokenType.NamedCodeBlock:
                    return "<NamedCodeBlock>\n" +
                        i("<Identifier>\n" + i(SubTokens[0].ToXML()) + "</Identifier>\n") +
                        i(SubTokens[1].ToXML()) +
                        "</NamedCodeBlock>";
                case ExpressionTokenType.NamedGenericCodeBlock:
                    return "<NamedGenericCodeBlock>\n" +
                        i("<Identifier>\n" + i(NamedGenericCodeBlockIdentifier.ToXML()) + "</Identifier>\n") +
                        i(NamedGenericCodeBlockInstructions.ToXML()) + 
                        "</NamedGenericCodeBlock>";
                case ExpressionTokenType.FunctionDeclaration:
                    return "<FunctionDeclaration>\n" +
                        i("<FuncName>\n" + i(SubTokens[0].ToXML()) + "</FuncName>\n") +
                        i(SubTokens[1].ToXML()) +
                        "</FunctionDeclaration>";
                case ExpressionTokenType.List:
                    return getChildrenCode("List");
                case ExpressionTokenType.ArgList:
                    return getChildrenCode("ArgList");
                case ExpressionTokenType.GenericParametersList:
                    return getChildrenCode("GenList");
                case ExpressionTokenType.Name:
                    return inlineencapsulate(this.Content, "Name");
                case ExpressionTokenType.CodeBlock:
                    return getChildrenCode("CodeBlock");
                case ExpressionTokenType.EndOfInstruction:
                    return inlineencapsulate(this.Content, "EndOfInstruction");
                case ExpressionTokenType.InstructionList:
                    return getChildrenCode("InstructionList");
                case ExpressionTokenType.ExpressionGroup:
                    if (Operator.IsBinaryOperator)
                            return encapsulate(
                                encapsulate(Operands1.ToXML(), "Operand1") +
                                Operator.ToXML() + 
                                encapsulate(Operands2.ToXML(), "Operand2"),
                                "ExpressionGroup");

                    else
                        return encapsulate(
                            encapsulate(Operands1.ToXML(), "Operand1") +
                            Operator.ToXML(),
                            "ExpressionGroup");

                case ExpressionTokenType.NumberLiteral:
                    return inlineencapsulate(this.Content, "NumberLiteral");
                case ExpressionTokenType.Operator:
                    return inlineencapsulate(this.Content, "Operator");
                case ExpressionTokenType.Separator:
                    return inlineencapsulate(this.Content, "Separator");
                case ExpressionTokenType.StringLiteral:
                    return inlineencapsulate(this.Content, "StringLiteral");
                case ExpressionTokenType.BoolLiteral:
                    return inlineencapsulate(this.Content, "BoolLiteral");
                case ExpressionTokenType.Modifier:
                    return inlineencapsulate(this.Content, "Modifier");
                default:
                    return "??";
            }
        }
        #endregion

        /// <summary>
        /// Lève une exception si le jeton e n'est pas du type type.
        /// </summary>
        void AssertType(ExpressionToken e, ExpressionTokenType type)
        {
            if(e.TkType != type)
            {
                throw new InvalidOperationException();
            }
        }

        #region Tests
        /// <summary>
        /// Traduit le jeton en code lisible.
        /// </summary>
        /// <returns></returns>
        public string ToPython()
        {
            Func<string, string> getChildrenCode = delegate(string separator)
            {
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < SubTokens.Count; i++)
                {
                    b.Append(SubTokens[i].ToPython());
                    if (i != SubTokens.Count - 1)
                        b.Append(separator);
                }
                return b.ToString();
            };

            Func<string, string> getChildrenCode2 = delegate(string ending)
            {
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < SubTokens.Count; i++)
                {
                    b.Append(SubTokens[i].ToPython());
                    b.Append(ending);
                }
                return b.ToString();
            };

            switch (TkType)
            {
                case ExpressionTokenType.BracketList:
                    return "[" + getChildrenCode(",") + "]";
                case ExpressionTokenType.FunctionCall:
                    return FunctionCallIdentifier.ToPython() + "(" + FunctionCallArgs.ToPython() + ")";
                case ExpressionTokenType.GenericType:
                    return GenericTypeIdentifier.ToPython();
                case ExpressionTokenType.ArrayType:
                    return ArrayTypeIdentifier.ToPython() + "[" + ArrayTypeArgs.ToPython() + "]";
                case ExpressionTokenType.List:
                    return getChildrenCode(" ");
                case ExpressionTokenType.ArgList:
                    return getChildrenCode(",");
                case ExpressionTokenType.GenericParametersList:
                    return "";
                case ExpressionTokenType.Name:
                    return this.Content;
                case ExpressionTokenType.CodeBlock:
                    return "{ " + getChildrenCode(",") + " }";
                case ExpressionTokenType.NamedCodeBlock:
                    return SubTokens[0].ToPython() + ":\n" + Tools.StringUtils.Indent(SubTokens[1].ToPython(), 1) + "\n";
                case ExpressionTokenType.FunctionDeclaration:
                    return SubTokens[0].ToPython() + ":\n" + Tools.StringUtils.Indent(SubTokens[1].ToPython(), 1) + "\n";
                case ExpressionTokenType.NamedGenericCodeBlock:
                    return NamedGenericCodeBlockIdentifier.ToPython() +
                        ":\n" + Tools.StringUtils.Indent(NamedGenericCodeBlockInstructions.ToPython(), 1) + "\n";
                case ExpressionTokenType.EndOfInstruction:
                    return this.Content + "\n";
                case ExpressionTokenType.InstructionList:
                    if (this.ListTokens.Count == 0)
                        return "pass;";
                    return getChildrenCode2("\n");
                case ExpressionTokenType.ExpressionGroup:
                    if (Operator.IsBinaryOperator)
                        if (Operator.Content == "=" || Operator.Content == ".")
                            return Operands1.ToPython() + Operator.ToPython() + Operands2.ToPython();
                        else
                            return "(" + Operands1.ToPython() + Operator.ToPython() + Operands2.ToPython() + ")";
                    else
                        return Operator.ToPython() + Operands1.ToPython();
                case ExpressionTokenType.NumberLiteral:
                    return this.Content;
                case ExpressionTokenType.Operator:
                    return this.Content;
                case ExpressionTokenType.Separator:
                    return this.Content;
                case ExpressionTokenType.StringLiteral:
                    return "\"" + this.Content + "\"";
                case ExpressionTokenType.BoolLiteral:
                    return this.Content;
                case ExpressionTokenType.Modifier:
                    return this.Content;
                default:
                    return "??";
            }
        }
        #endregion
    }

}
