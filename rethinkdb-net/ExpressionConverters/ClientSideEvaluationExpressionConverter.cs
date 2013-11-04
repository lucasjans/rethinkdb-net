using System;
using System.Linq.Expressions;
using RethinkDb.DatumConverters;
using RethinkDb.Spec;

namespace RethinkDb.ExpressionConverters
{
    public class ClientSideEvaluationExpressionConverter : IExpressionConverter
    {
        public static readonly ClientSideEvaluationExpressionConverter Instance = new ClientSideEvaluationExpressionConverter();

        protected ClientSideEvaluationExpressionConverter()
        {
        }

        public virtual bool TryConvertExpression(IDatumConverterFactory datumConverterFactory, IExpressionConverter rootExpressionConverter, Expression expr, out Term term)
        {
            term = null;
            try
            {
                var converter = datumConverterFactory.Get(expr.Type);
                var clientSideFunc = Expression.Lambda(expr).Compile();
                term = new Term() {
                    type = Term.TermType.DATUM,
                    datum = converter.ConvertObject(clientSideFunc.DynamicInvoke())
                };
                return true;
            }
            catch (InvalidOperationException)
            {
                // Failed to perform client-side evaluation of expression tree node; often this is caused by refering
                // to a server-side variable in a node that is only supported w/ client-side evaluation.
                return false;
            }
        }
    }
}
