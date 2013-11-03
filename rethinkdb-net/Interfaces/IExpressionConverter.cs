using System.ComponentModel;
using System.Linq.Expressions;
using RethinkDb.Spec;

namespace RethinkDb
{
    [ImmutableObject(true)]
    public interface IExpressionConverter
    {
        bool TryConvertExpression(IDatumConverterFactory datumConverterFactory, IExpressionConverter rootExpressionConverter, Expression expr, out Term term);
    }
}
