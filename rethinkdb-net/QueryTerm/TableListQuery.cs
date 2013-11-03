using RethinkDb.Spec;

namespace RethinkDb.QueryTerm
{
    public class TableListQuery : ISingleObjectQuery<string[]>
    {
        private readonly IDatabaseQuery dbTerm;

        public TableListQuery(IDatabaseQuery dbTerm)
        {
            this.dbTerm = dbTerm;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory, IExpressionConverter expressionConverter)
        {
            var tableTerm = new Term()
            {
                type = Term.TermType.TABLE_LIST,
            };
            tableTerm.args.Add(dbTerm.GenerateTerm(datumConverterFactory, expressionConverter));
            return tableTerm;
        }
    }
}