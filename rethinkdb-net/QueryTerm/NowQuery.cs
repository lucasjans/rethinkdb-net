using RethinkDb.Spec;
using System;

namespace RethinkDb.QueryTerm
{
    public class NowQuery : ISingleObjectQuery<DateTimeOffset>
    {
        public NowQuery()
        {
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory, IExpressionConverter expressionConverter)
        {
            var term = new Term()
            {
                type = Term.TermType.NOW,
            };
            return term;
        }
    }
}