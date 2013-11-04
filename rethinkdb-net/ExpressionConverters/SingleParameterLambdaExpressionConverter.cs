using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RethinkDb.DatumConverters;
using RethinkDb.Spec;

namespace RethinkDb.Expressions
{
    public class SingleParameterLambdaExpressionConverter<TParameter1, TReturn> : IExpressionConverter
    {
        #region Public interface

        private readonly IDatumConverterFactory datumConverterFactory;
        private readonly IExpressionConverter innerExpressionConverter;

        public SingleParameterLambdaExpressionConverter(IDatumConverterFactory datumConverterFactory, IExpressionConverter innerExpressionConverter)
        {
            this.datumConverterFactory = datumConverterFactory;
            this.innerExpressionConverter = innerExpressionConverter;
        }

        public Term CreateFunctionTerm(Expression<Func<TParameter1, TReturn>> expression)
        {
            var funcTerm = new Term() {
                type = Term.TermType.FUNC
            };

            var parametersTerm = new Term() {
                type = Term.TermType.MAKE_ARRAY,
            };
            parametersTerm.args.Add(new Term() {
                type = Term.TermType.DATUM,
                datum = new Datum() {
                    type = Datum.DatumType.R_NUM,
                    r_num = 2
                }
            });
            funcTerm.args.Add(parametersTerm);

            var body = expression.Body;
            if (body.NodeType == ExpressionType.MemberInit)
            {
                var memberInit = (MemberInitExpression)body;
                if (!memberInit.Type.Equals(typeof(TReturn)))
                    throw new InvalidOperationException("Only expression types matching the table type are supported");
                else if (memberInit.NewExpression.Arguments.Count != 0)
                    throw new NotSupportedException("Constructors will not work here, only field member initialization");
                funcTerm.args.Add(MapMemberInitToTerm(memberInit));
            }
            else
            {
                Term term;
                if (!TryConvertExpression(datumConverterFactory, this, body, out term))
                    throw new NotSupportedException(String.Format("Unable to convert expression of type {0}: {1}", body.NodeType, body));
                funcTerm.args.Add(term);
            }

            return funcTerm;
        }

        private Term MapMemberInitToTerm(MemberInitExpression memberInit)
        {
            var makeObjTerm = new Term() {
                type = Term.TermType.MAKE_OBJ,
            };

            foreach (var binding in memberInit.Bindings)
            {
                switch (binding.BindingType)
                {
                    case MemberBindingType.Assignment:
                        makeObjTerm.optargs.Add(MapMemberAssignmentToMakeObjArg((MemberAssignment)binding));
                        break;
                    case MemberBindingType.ListBinding:
                    case MemberBindingType.MemberBinding:
                        throw new NotSupportedException("Binding type not currently supported");
                }
            }

            return makeObjTerm;
        }

        private Term.AssocPair MapMemberAssignmentToMakeObjArg(MemberAssignment memberAssignment)
        {
            var retval = new Term.AssocPair();

            var datumConverter = datumConverterFactory.Get<TReturn>();
            var fieldConverter = datumConverter as IObjectDatumConverter;
            if (fieldConverter == null)
                throw new NotSupportedException("Cannot map member assignments into ReQL without implementing IObjectDatumConverter");

            Term term;
            if (!TryConvertExpression(datumConverterFactory, this, memberAssignment.Expression, out term))
                throw new NotSupportedException(String.Format("Unable to convert expression of type {0}: {1}", memberAssignment.Expression.NodeType, memberAssignment.Expression));

            retval.key = fieldConverter.GetDatumFieldName(memberAssignment.Member);
            retval.val = term;


            return retval;
        }

        /*
        protected override Term RecursiveMap(Expression expression)
        {
            return MapExpressionToTerm(expression);
        }

        protected override Term RecursiveMapMemberInit<TInnerReturn>(Expression expression)
        {
            var newConverter = new SingleParameterLambda<TParameter1, TInnerReturn>(datumConverterFactory);
            return newConverter.MapMemberInitToTerm((MemberInitExpression)expression);
        }
        */

        public bool TryConvertExpression(IDatumConverterFactory datumConverterFactory, IExpressionConverter rootExpressionConverter, Expression expr, out Term term)
        {
            term = null;

            if (expr.NodeType == ExpressionType.Parameter)
            {
                term = new Term()
                {
                    type = Term.TermType.VAR,
                    args = {
                        new Term() {
                            type = Term.TermType.DATUM,
                            datum = new Datum() {
                                type = Datum.DatumType.R_NUM,
                                r_num = 2
                            },
                        }
                    }
                };
                return true;
            }

            if (expr.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = (MemberExpression)expr;

                if (memberExpr.Expression == null || memberExpr.Expression.NodeType != ExpressionType.Parameter)
                    return innerExpressionConverter.TryConvertExpression(datumConverterFactory, rootExpressionConverter, expr, out term);

                var getAttrTerm = new Term() {
                    type = Term.TermType.GET_FIELD
                };

                getAttrTerm.args.Add(new Term() {
                    type = Term.TermType.VAR,
                    args = {
                        new Term() {
                            type = Term.TermType.DATUM,
                            datum = new Datum() {
                                type = Datum.DatumType.R_NUM,
                                r_num = 2
                            },
                        }
                    }
                });

                var datumConverter = datumConverterFactory.Get<TParameter1>();
                var fieldConverter = datumConverter as IObjectDatumConverter;
                if (fieldConverter == null)
                    throw new NotSupportedException("Cannot map member access into ReQL without implementing IObjectDatumConverter");

                var datumFieldName = fieldConverter.GetDatumFieldName(memberExpr.Member);
                if (string.IsNullOrEmpty(datumFieldName))
                    throw new NotSupportedException(String.Format("Member {0} on type {1} could not be mapped to a datum field", memberExpr.Member.Name, memberExpr.Type));

                getAttrTerm.args.Add(new Term() {
                    type = Term.TermType.DATUM,
                    datum = new Datum() {
                        type = Datum.DatumType.R_STR,
                        r_str = datumFieldName
                    }
                });

                term = getAttrTerm;
                return true;
            }

            return innerExpressionConverter.TryConvertExpression(datumConverterFactory, rootExpressionConverter, expr, out term);
        }

        #endregion
    }
}
