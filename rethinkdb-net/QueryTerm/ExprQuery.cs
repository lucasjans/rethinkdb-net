using RethinkDb.Spec;
using System;
using System.Linq.Expressions;

namespace RethinkDb.QueryTerm
{
    public class ExprQuery<T> : ISingleObjectQuery<T>
    {
        private readonly T @object;
        private readonly IQueryFunction<T> objectExpr;

        public ExprQuery(T @object)
        {
            this.@object = @object;
        }

        public ExprQuery(IQueryFunction<T> objectExpr)
        {
            this.objectExpr = objectExpr;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory)
        {
            if (objectExpr != null)
            {
                return objectExpr.GenerateTerm(datumConverterFactory);
            }
            else
            {
                var datumTerm = new Term()
                {
                    type = Term.TermType.DATUM,
                    datum = datumConverterFactory.Get<T>().ConvertObject(@object)
                };
                return datumTerm;
            }
        }
    }
}

