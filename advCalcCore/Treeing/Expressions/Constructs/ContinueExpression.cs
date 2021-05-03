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
	class ContinueExpression : Expression
	{
		public static void Register()
		{
			NamedConstants.RegisterExpression("continue", () => new ContinueExpression(), "continue");
		}

		public int MinParameterCount { get; }
		public int MaxParameterCount { get; }

		public override bool IsStatic => false;

		public override string Name => "continue";

		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();

		protected override Value GetValueInternal(bool execute = true)
		{
			Callstack.Flags |= CallStack.ReturnFlags.Continue;
			return NullValue.Null;
		}
		public override string ToString() => Name;
	}
}
