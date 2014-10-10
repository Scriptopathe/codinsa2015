using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model.Language
{
    /// <summary>
    /// Enumération représentant les différents opérateurs.
    /// </summary>
    public enum Operator
    {
        Add,
        Minus,
        Mult,
        Div,
        Mod,
        And, LazyAnd,
        Or,  LazyOr,
        Xor,

        Not,
        Equals,
        NotEquals,

        New,

        // TODO : à supprimer ?
        Dot,
        Affectation,
    }
    /// <summary>
    /// Contient des méthodes permettant la manipulation des opérateurs.
    /// </summary>
    public class Operators
    {
        // [Operateur][operandType1][operandType2] => resultat
        public static Dictionary<Operator, Dictionary<string, Dictionary<string, string>>> OperatorTypingMapping = new Dictionary<Operator, Dictionary<string, Dictionary<string, string>>>()
        {
            #region Numeric
            {Operator.Add, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"string", new Dictionary<string, string>()
                        {
                            {"string", "string"}
                        }
                    },
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "int"},
                            {"float", "float"}
                        }
                    },
                    {"float", new Dictionary<string, string>()
                        {
                            {"int", "float"},
                            {"float", "float"}
                        }
                    },
                }
            },
            {Operator.Minus, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "int"},
                            {"float", "float"}
                        }
                    },
                    {"float", new Dictionary<string, string>()
                        {
                            {"int", "float"},
                            {"float", "float"}
                        }
                    },
                }
            },
            {Operator.Mult, new Dictionary<string, Dictionary<string, string>>() 
                 {
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "int"},
                            {"float", "float"}
                        }
                    },
                    {"float", new Dictionary<string, string>()
                        {
                            {"int", "float"},
                            {"float", "float"}
                        }
                    },
                }
            },
            {Operator.Div, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "int"},
                            {"float", "float"}
                        }
                    },
                    {"float", new Dictionary<string, string>()
                        {
                            {"int", "float"},
                            {"float", "float"}
                        }
                    },
                }
            },
            {Operator.Mod, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "int"},
                            {"float", "float"}
                        }
                    },
                    {"float", new Dictionary<string, string>()
                        {
                            {"int", "float"},
                            {"float", "float"}
                        }
                    },
                }
            },
            #endregion

            #region Boolean
            {Operator.And, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"bool", new Dictionary<string, string>()
                        {
                            {"bool", "bool"}
                        }
                    },
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "int"}
                        }
                    }
                }
            },
            {Operator.Or, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"bool", new Dictionary<string, string>()
                        {
                            {"bool", "bool"}
                        }
                    },
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "int"}
                        }
                    }
                }
            },
            {Operator.LazyAnd, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"bool", new Dictionary<string, string>()
                        {
                            {"bool", "bool"}
                        }
                    }
                }
            },
            {Operator.LazyOr, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"bool", new Dictionary<string, string>()
                        {
                            {"bool", "bool"}
                        }
                    }
                }
            },
            {Operator.Xor, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"bool", new Dictionary<string, string>()
                        {
                            {"bool", "bool"}
                        }
                    },
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "int"}
                        }
                    }
                }
            },
            {Operator.Not, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"bool", new Dictionary<string, string>()
                        {
                            {"bool", "bool"}
                        }
                    }
                }
            },
            #endregion

            #region Equality
            {Operator.Equals, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"bool", new Dictionary<string, string>()
                        {
                            {"bool", "bool"}
                        }
                    },
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "bool"}
                        }
                    },
                    {"float", new Dictionary<string, string>()
                        {
                            {"float", "bool"}
                        }
                    },
                    {"string", new Dictionary<string, string>()
                        {
                            {"string", "bool"}
                        }
                    }
                }
            },
            {Operator.NotEquals, new Dictionary<string, Dictionary<string, string>>() 
                {
                    {"bool", new Dictionary<string, string>()
                        {
                            {"bool", "bool"}
                        }
                    },
                    {"int", new Dictionary<string, string>()
                        {
                            {"int", "bool"}
                        }
                    },
                    {"float", new Dictionary<string, string>()
                        {
                            {"float", "bool"}
                        }
                    },
                    {"string", new Dictionary<string, string>()
                        {
                            {"string", "bool"}
                        }
                    }
                }
            }
            #endregion
        };
        /// <summary>
        /// Extrait un opérateur d'un jeton.
        /// </summary>
        public static Operator ParseOperator(Tokenizers.ExpressionToken token)
        {
            if (token.TkType != Tokenizers.ExpressionToken.ExpressionTokenType.Operator)
                throw new InvalidOperationException();

            switch(token.Content)
            {
                case "+":
                    return Operator.Add;
                case "-":
                    return Operator.Minus;
                case "*":
                    return Operator.Mult;
                case "/":
                    return Operator.Div;
                case "&":
                    return Operator.And;
                case "&&":
                    return Operator.LazyAnd;
                case "|":
                    return Operator.Or;
                case "||":
                    return Operator.LazyOr;
                case "^":
                    return Operator.Xor;
                case ".":
                    return Operator.Dot;
                case "==":
                    return Operator.Equals;
                case "!=":
                    return Operator.NotEquals;
                case "=":
                    return Operator.Affectation;
                case "!":
                    return Operator.Not;
            }

            throw new InvalidOperationException("Opérateur " + token.Content + " inexistant");
        }
    }
}
