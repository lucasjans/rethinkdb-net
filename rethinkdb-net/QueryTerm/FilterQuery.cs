using RethinkDb.Spec;
using System;
using System.Linq.Expressions;

namespace RethinkDb.QueryTerm
{
    public class FilterQuery<T> : ISequenceQuery<T>
    {
        private readonly ISequenceQuery<T> sequenceQuery;
        private readonly IQueryFunction<T, bool> filterExpression;

        public FilterQuery(ISequenceQuery<T> sequenceQuery, IQueryFunction<T, bool> filterExpression)
        {
            this.sequenceQuery = sequenceQuery;
            this.filterExpression = filterExpression;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory)
        {
            var filterTerm = new Term()
            {
                type = Term.TermType.FILTER,
            };
            filterTerm.args.Add(sequenceQuery.GenerateTerm(datumConverterFactory));
            filterTerm.args.Add(filterExpression.GenerateTerm(datumConverterFactory));
            return filterTerm;
        }
    }
}
