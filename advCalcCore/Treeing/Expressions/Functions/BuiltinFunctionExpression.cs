using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
	abstract class BuiltinFunctionExpression : Expression
	{
		BuiltinFunctionValue functionValue;

		public int MinParameterCount { get; }
		public int MaxParameterCount { get; }

		public override bool IsStatic => true;
		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();

		private BuiltinFunctionExpression()
		{
			functionValue = new BuiltinFunctionValue(this);
		}

		protected BuiltinFunctionExpression(int parameterCount) : this()
		{
			MinParameterCount = parameterCount;
			MaxParameterCount = parameterCount;
		}

		protected BuiltinFunctionExpression(int minParameterCount, int maxParameterCount) : this()
		{
			MinParameterCount = minParameterCount;
			MaxParameterCount = maxParameterCount;
		}

		protected override Value GetValueInternal(bool execute = true) => functionValue;

		protected abstract Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstack);


		private class BuiltinFunctionValue : Value, ICallableValue
		{
			private BuiltinFunctionExpression parent;


			public BuiltinFunctionValue(BuiltinFunctionExpression parent)
			{
				this.parent = parent;
			}

			public Value GetValue(List<Value> values, IdentifierStore identifierStore = null, CallStack callstack = null)
			{

				if (values.Count < parent.MinParameterCount || values.Count > parent.MaxParameterCount)
					throw new InvalidOperationException("Value count doesn´t match parameter count");

				return parent.CalculateValue(values, identifierStore, callstack);
			}

			public override string ToString() => "BuiltIn-" + parent.Name;
		}

		public override string ToString() => Name;
	}
}
