using System;
using System.Linq.Expressions;
using RethinkDb.DatumConverters;
using RethinkDb.Spec;

namespace RethinkDb.ExpressionConverters
{
    public class ConstantExpressionConverter : IExpressionConverter
    {
        public static readonly ConstantExpressionConverter Instance = new ConstantExpressionConverter();

        protected ConstantExpressionConverter()
        {
        }

        public virtual bool TryConvertExpression(IDatumConverterFactory datumConverterFactory, IExpressionConverter rootExpressionConverter, Expression expr, out Term term)
        {
            term = null;
            if (expr.NodeType != ExpressionType.Constant)
                return false;

            var constantExpression = (ConstantExpression)expr;
            var datumConverter = datumConverterFactory.Get(constantExpression.Type);

            term = new Term() {
                type = Term.TermType.DATUM,
                datum = datumConverter.ConvertObject(constantExpression.Value)
            };
            return true;
        }
    }
}

