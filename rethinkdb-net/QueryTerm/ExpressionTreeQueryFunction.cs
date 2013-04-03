using RethinkDb.Spec;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace RethinkDb.QueryTerm
{
    public class ExpressionTreeQueryFunction<TReturn> : IQueryFunction<TReturn>
    {
        private readonly Expression<Func<TReturn>> expression;

        public ExpressionTreeQueryFunction(Expression<Func<TReturn>> expression)
        {
            this.expression = expression;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory)
        {
            return ExpressionUtils.CreateValueTerm(datumConverterFactory, expression);
        }
    }

    public class ExpressionTreeQueryFunction<TParameter1, TReturn> : IQueryFunction<TParameter1, TReturn>
    {
        private readonly Expression<Func<TParameter1, TReturn>> expression;

        public ExpressionTreeQueryFunction(Expression<Func<TParameter1, TReturn>> expression)
        {
            this.expression = expression;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory)
        {
            return ExpressionUtils.CreateFunctionTerm(datumConverterFactory, expression);
        }
    }

    public class ExpressionTreeQueryFunction<TParameter1, TParameter2, TReturn> : IQueryFunction<TParameter1, TParameter2, TReturn>
    {
        private readonly Expression<Func<TParameter1, TParameter2, TReturn>> expression;

        public ExpressionTreeQueryFunction(Expression<Func<TParameter1, TParameter2, TReturn>> expression)
        {
            this.expression = expression;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory)
        {
            return ExpressionUtils.CreateFunctionTerm(datumConverterFactory, expression);
        }
    }
}

