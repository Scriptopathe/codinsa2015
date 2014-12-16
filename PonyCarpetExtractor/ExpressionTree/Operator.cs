using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree
{
    /// <summary>
    /// Delegate de fonction d'opérateur.
    /// </summary>
    public delegate dynamic OperatorDelegate(dynamic op1, dynamic op2);
    /// <summary>
    /// Représente un opérateur.
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// Opération exécutée par l'opérateur.
        /// </summary>
        public OperatorDelegate Operation
        {
            get;
            set;
        }
        public Operator(OperatorDelegate op)
        {
            Operation = op;
        }
    }
    public static class Operators
    {
        public static Dictionary<string, Operator> Mapping;
        /// <summary>
        /// Initialize les opérators.
        /// Si je fais une lazy initialization ils valent tous null au moment crutial.
        /// Donc je les initialise ici.
        /// </summary>
        static public void InitOperators()
        {
            // plus
            plus = delegate(dynamic op1, dynamic op2)
            {
                return op1 + op2;
            };
            Plus = new Operator(plus);
            // minus
            minus = delegate(dynamic op1, dynamic op2)
            {
                return op1 - op2;
            };
            Minus = new Operator(minus);
            // mult
            mult = delegate(dynamic op1, dynamic op2)
            {
                return op1 * op2;
            };
            Mult = new Operator(mult);
            // div
            div = delegate(dynamic op1, dynamic op2)
            {
                return op1 / op2;
            };
            Div = new Operator(div);
            // equals
            equals = delegate(dynamic op1, dynamic op2)
            {
                return op1 == op2;
            };
            Equals = new Operator(equals);
            // not equals
            notEquals = delegate(dynamic op1, dynamic op2)
            {
                return op1 != op2;
            };
            NotEquals = new Operator(notEquals);
            // and
            and = delegate(dynamic op1, dynamic op2)
            {
                return op1 & op2;
            };
            And = new Operator(and);
            // lazy and
            lazyAnd = delegate(dynamic op1, dynamic op2)
            {
                return op1 && op2;
            };
            LazyAnd = new Operator(lazyAnd);
            // or
            or = delegate(dynamic op1, dynamic op2)
            {
                return op1 | op2;
            };
            Or = new Operator(or);

            // lazy or
            lazyOr = delegate(dynamic op1, dynamic op2)
            {
                return op1 || op2;
            };
            LazyOr = new Operator(lazyOr);
            // xor
            xor = delegate(dynamic op1, dynamic op2)
            {
                return op1 ^ op2;
            };
            Xor = new Operator(xor);
            // Pow
            pow = delegate(dynamic op1, dynamic op2)
            {
                return Math.Pow(op1, op2);
            };
            Pow = new Operator(pow);
            // <
            smallerThan = delegate(dynamic op1, dynamic op2)
            {
                return op1 < op2;
            };
            SmallerThan = new Operator(smallerThan);
            // <= 
            smallerThanOrEqual = delegate(dynamic op1, dynamic op2)
            {
                return op1 <= op2;
            };
            SmallerThanOrEqual = new Operator(smallerThanOrEqual);
            // >
            greaterThan = delegate(dynamic op1, dynamic op2)
            {
                return op1 > op2;
            };
            GreaterThan = new Operator(greaterThan);
            // >=
            greaterThanOrEqual = delegate(dynamic op1, dynamic op2)
            {
                return op1 >= op2;
            };
            GreaterThanOrEqual = new Operator(greaterThanOrEqual);
            Mapping = new Dictionary<string, Operator>()
            {
                {"+", Plus},
                {"-", Minus},
                {"==", Equals},
                {"!=", NotEquals},
                {"/", Div},
                {"*", Mult},
                {">", GreaterThan},
                {">=", GreaterThanOrEqual},
                {"<", SmallerThan},
                {"<=", SmallerThanOrEqual},
                {"||", LazyOr},
                {"|", Or},
                {"&&", LazyAnd},
                {"&", And},
                {"^", Xor},
                {"**", Pow}
            };
        }
        public static Operator Pow = new Operator(pow);
        static OperatorDelegate pow;
        public static Operator LazyAnd = new Operator(lazyAnd);
        static OperatorDelegate lazyAnd;
        public static Operator And = new Operator(and);
        static OperatorDelegate and;
        public static Operator Or = new Operator(or);
        static OperatorDelegate or;
        public static Operator LazyOr = new Operator(lazyOr);
        static OperatorDelegate lazyOr;
        public static Operator Xor = new Operator(xor);
        static OperatorDelegate xor;

        public static Operator Plus = new Operator(plus);
        static OperatorDelegate plus;
        public static Operator Minus = new Operator(minus);
        static OperatorDelegate minus;
        public static Operator Mult = new Operator(mult);
        static OperatorDelegate mult;
        public static Operator Div = new Operator(div);
        static OperatorDelegate div;
        public static Operator Equals = new Operator(equals);
        static OperatorDelegate equals;
        public static Operator NotEquals = new Operator(notEquals);
        static OperatorDelegate notEquals;
        public static Operator SmallerThan = new Operator(smallerThan);
        static OperatorDelegate smallerThan;
        public static Operator SmallerThanOrEqual = new Operator(smallerThanOrEqual);
        static OperatorDelegate smallerThanOrEqual;
        public static Operator GreaterThan = new Operator(greaterThan);
        static OperatorDelegate greaterThan;
        public static Operator GreaterThanOrEqual = new Operator(greaterThanOrEqual);
        static OperatorDelegate greaterThanOrEqual;
    }
}
