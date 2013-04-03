using System;
using RethinkDb.Spec;

namespace RethinkDb.QueryTerm
{
    public class JavaScriptQueryFunction<TReturn> : IQueryFunction<TReturn>
    {
        private readonly string js;

        public JavaScriptQueryFunction(string js)
        {
            this.js = js;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory)
        {
            var term = new Term() {
                type = Term.TermType.JAVASCRIPT,
            };
            term.args.Add(new Term() {
                type = Term.TermType.DATUM,
                datum = new Datum() {
                    type = Datum.DatumType.R_STR,
                    r_str = js,
                }
            });
            return term;
        }
    }

    public class JavaScriptQueryFunction<TParameter1, TReturn> : IQueryFunction<TParameter1, TReturn>
    {
        private readonly string js;

        public JavaScriptQueryFunction(string js)
        {
            this.js = js;
        }

        public Term GenerateTerm(IDatumConverterFactory datumConverterFactory)
        {
            var term = new Term() {
                type = Term.TermType.JAVASCRIPT,
            };
            term.args.Add(new Term() {
                type = Term.TermType.DATUM,
                datum = new Datum() {
                    type = Datum.DatumType.R_STR,
                    r_str = js,
                }
            });
            return term;
        }
    }
}

