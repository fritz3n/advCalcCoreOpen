using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Constructs
{
	class ReturnFunctionExpression : Expression
	{
		public static void Register()
		{
			NamedConstants.RegisterExpression("return", () => new ReturnFunctionExpression(), "return(val1, val2, ...)");
		}

		ReturnFunctionValue functionValue = new ReturnFunctionValue();


		public int MinParameterCount { get; }
		public int MaxParameterCount { get; }

		public override bool IsStatic => true;

		public override string Name => "return";

		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();

		protected override Value GetValueInternal(bool execute = true) => functionValue;


		private class ReturnFunctionValue : Value, ICallableValue
		{
			public bool ResetCallStack => false;

			public Value GetValue(List<Value> values, IdentifierStore identifierStore = null, CallStack callstack = null)
			{
				Value returned;
				if (values.Count == 0)
					returned = NullValue.Null;
				else if (values.Count == 1)
					returned = values[0];
				else
					returned = new ListValue(values);
				callstack.Result = returned;
				callstack.Flags |= CallStack.ReturnFlags.Return;
				return returned;
			}
		}

		public override string ToString() => Name;
	}
}

