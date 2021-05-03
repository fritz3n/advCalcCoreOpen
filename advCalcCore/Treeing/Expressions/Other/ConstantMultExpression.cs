using advCalcCore.Treeing.Expressions.Exceptions;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Other
{
	class ConstantMultExpression : UnaryExpression, IAssignable
	{
		public override string Name => "ConstantMult";
		public Value Multiplier { get; set; }

		public override bool IsStatic => Parameter.IsStatic;

		public void Assign(Value value)
		{
			if (Parameter is not IAssignable assignable)
				throw new ExpressionException(Callstack, $"{Parameter} is not assignable");

			assignable.Assign(value / Multiplier);
		}


		protected override Value GetValueInternal(bool execute = true)
		{
			return Parameter.GetValue(Callstack, execute) * Multiplier;
		}

		public override string ToString() => $"{Multiplier}{Parameter}";
	}
}
