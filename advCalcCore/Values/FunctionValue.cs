using advCalcCore.Treeing.Expressions;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values
{
	public class FunctionValue : Value, ICallableValue, IThisValue
	{
		private readonly List<string> parameters;

		private Expression Expression { get; }
		public IReadOnlyList<string> Parameters => parameters;

		public Value This { get; set; } = null;

		public FunctionValue(Expression expression, List<string> parameters)
		{
			Expression = expression ?? throw new ArgumentNullException(nameof(expression));
			this.parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
		}
		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(Value))
				return CastingType.Implicit;
			if (type == typeof(FunctionValue))
				return CastingType.Implicit;

			if (type == typeof(TextValue))
				return CastingType.Explicit;

			return CastingType.None;
		}

		public override Value CastTo(Type type, bool explicitCast = false)
		{
			if (type == typeof(Value))
				return this;
			if (type == typeof(FunctionValue))
				return this;

			if (explicitCast)
			{
				if (type == typeof(TextValue))
					return new TextValue(ToString());
			}

			return base.CastTo(type, explicitCast);
		}

		public Value GetValue(List<Value> values, IdentifierStore identifierStore = null, CallStack callstack = null)
		{
			identifierStore = identifierStore ?? IdentifierStore.Global;
			callstack = callstack ?? new CallStack();

			if (values.Count != parameters.Count)
				throw new InvalidOperationException("Value count doesn´t match parameter count");

			var dict = parameters.Zip(values, (k, v) => new { k, v })
			  .ToDictionary(x => x.k, x => x.v);

			identifierStore = identifierStore.NewScopeWith(dict);

			Expression.Identifiers = identifierStore;
			Expression.This = this;
			return Expression.GetValue(callstack);
		}

		public override string ToString() => $"({string.Join(", ", parameters)}) => " + Expression.ToString();
	}
}