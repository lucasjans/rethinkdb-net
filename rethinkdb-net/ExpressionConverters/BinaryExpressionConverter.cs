using System;
using System.Linq.Expressions;
using RethinkDb.DatumConverters;
using RethinkDb.Spec;

namespace RethinkDb.ExpressionConverters
{
    public class BinaryExpressionConverter : IExpressionConverter
    {
        public static readonly BinaryExpressionConverter Instance = new BinaryExpressionConverter();

        protected BinaryExpressionConverter()
        {
        }

        private bool ConvertBinaryExpressionToTerm(IDatumConverterFactory datumConverterFactory, IExpressionConverter rootExpressionConverter, BinaryExpression expr, Term.TermType termType, out Term term)
        {
            Term subTerm;

            term = new Term() {
                type = termType,
            };

            if (!rootExpressionConverter.TryConvertExpression(datumConverterFactory, rootExpressionConverter, expr.Left, out subTerm))
                return false;
            term.args.Add(subTerm);

            if (!rootExpressionConverter.TryConvertExpression(datumConverterFactory, rootExpressionConverter, expr.Right, out subTerm))
                return false;
            term.args.Add(subTerm);

            return true;
        }

        public virtual bool TryConvertExpression(IDatumConverterFactory datumConverterFactory, IExpressionConverter rootExpressionConverter, Expression expr, out Term term)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.Add:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.ADD, out term);
                case ExpressionType.Modulo:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.MOD, out term);
                case ExpressionType.Divide:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.DIV, out term);
                case ExpressionType.Multiply:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.MUL, out term);
                case ExpressionType.Subtract:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.SUB, out term);
                case ExpressionType.Equal:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.EQ, out term);
                case ExpressionType.LessThan:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.LT, out term);
                case ExpressionType.LessThanOrEqual:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.LE, out term);
                case ExpressionType.GreaterThan:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.GT, out term);
                case ExpressionType.GreaterThanOrEqual:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.GE, out term);
                case ExpressionType.AndAlso:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.ALL, out term);
                case ExpressionType.OrElse:
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.ANY, out term);
                case ExpressionType.NotEqual:   
                    return ConvertBinaryExpressionToTerm(datumConverterFactory, rootExpressionConverter, (BinaryExpression)expr, Term.TermType.NE, out term);
            }

            term = null;
            return false;
        }
    }
}

