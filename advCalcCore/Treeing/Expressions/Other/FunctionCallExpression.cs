using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Expressions.Other;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
	class FunctionCallExpression : UnaryExpression, IExpressionAssignable
	{
		public override string Name => "Function Call";

		protected override IEnumerable<Expression> AllParameters => Parameters.Append(Parameter);
		public override bool IsStatic
		{
			get
			{
				foreach (Expression exp in Parameters)
				{
					if (!exp.IsStatic)
						return false;
				}

				return true;
			}
		}
		public override IdentifierStore Identifiers
		{
			get => base.Identifiers;
			set
			{
				foreach (Expression p in Parameters)
					p.Identifiers = value;
				base.Identifiers = value;
			}
		}

		public virtual List<Expression> Parameters { get; set; }


		public Value Assign(Expression expression, CallStack callStack)
		{
			if (!(Parameter is IAssignable Identifier))
				throw new ExpressionException(Callstack, Parameter + " can´t be assigned to");

			var parameters = Parameters.Select(
				exp =>
				{
					if (exp is IdentifierExpression ident)
						return ident.Identifier;
					throw new ExpressionException(Callstack, "Functions can only take parameter names when assigned.");
				}).ToList();



			Expression.Rebase(expression, callStack, ToString());
			var function = new FunctionValue(expression, parameters);

			Identifier.Assign(function);

			return function;
		}

		protected override Value GetValueInternal(bool execute = true)
		{
			if (Parameter.GetValue(Callstack) is ICallableValue function)
			{
				Value result = function.GetValue(Parameters.Select(value => value.GetValue(Callstack)).ToList(), Identifiers, Callstack);
				if (function.ResetCallStack)
					Callstack.ResetCallStackFlags();
				return result;
			}
			throw new ExpressionException(Callstack, $"Can´t call '{Parameter.GetValue(Callstack)}'");
		}

		public override string ToString() => $"{Parameter}({string.Join(", ", Parameters)})";
	}
}
