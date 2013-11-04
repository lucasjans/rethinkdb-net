using System;
using System.Linq.Expressions;
using RethinkDb.DatumConverters;
using RethinkDb.Spec;
using System.Collections.Generic;

namespace RethinkDb.ExpressionConverters
{
    public class AggregateExpressionConverter : IExpressionConverter
    {
        private readonly IList<IExpressionConverter> expressionConverters;

        public AggregateExpressionConverter(params IExpressionConverter[] expressionConverters)
        {
            this.expressionConverters = new List<IExpressionConverter>(expressionConverters);
        }

        public virtual bool TryConvertExpression(IDatumConverterFactory datumConverterFactory, IExpressionConverter rootExpressionConverter, Expression expr, out Term term)
        {
            term = null;
            foreach (var converter in expressionConverters)
            {
                if (converter.TryConvertExpression(datumConverterFactory, rootExpressionConverter, expr, out term))
                    return true;
            }

            return false;
        }
    }
}

