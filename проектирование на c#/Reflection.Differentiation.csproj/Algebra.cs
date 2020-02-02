using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;


namespace Reflection.Differentiation
{
    public class Algebra
    {
        public class ParamModifier : ExpressionVisitor
        {
            public ParameterExpression param;
            public ConstantExpression eps = Expression.Constant(1e-7);
            public Expression Modify(Expression expression, ParameterExpression param)
            {
                this.param = param;
                return Visit(expression);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                bool change = false;

                foreach (var el in node.Arguments)
                    if (el.NodeType.Equals(ExpressionType.Parameter))
                     change = true; 

                if (change)
                    return Expression.Call(node.Method, Expression.Add(param, eps));

                return base.VisitMethodCall(node);
            }

            protected override Expression VisitBinary(BinaryExpression b)
            {
                bool change = false;

                Expression left = this.Visit(b.Left);
                Expression right = this.Visit(b.Right);
                if (left.NodeType == ExpressionType.Parameter)
                {
                    change = true;
                    left = Expression.Add(param, eps);
                }
                if (right.NodeType == ExpressionType.Parameter)
                {
                    change = true;
                    right = Expression.Add(param, eps);
                }
                if (change)
                    return Expression.MakeBinary(b.NodeType, left, right);

                return base.VisitBinary(b);
            }
        }

        public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> function)
        {
            var eps = Expression.Constant(1e-7);
            var param = function.Parameters[0];
            var body = function.Body;

            ParamModifier treeModifier = new ParamModifier();
            var modifiedExpr = body.NodeType.Equals(ExpressionType.Parameter) ? Expression.Add(param, eps) : treeModifier.Modify(body, param);

            var expression = Expression.Lambda<Func<double, double>>(
            Expression.Divide( Expression.Subtract( modifiedExpr, body ), eps), param);

            return expression;
        }
    }
}
