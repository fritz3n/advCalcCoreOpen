using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{

	class AssignExpression : BinaryExpression
	{
		public override string Name => "Assignation";

		protected override Value GetValueInternal(bool execute = true)
		{
			if (execute)
			{
				if (Left is IAssignable assignable)
					assignable.Assign(Right.GetValue(Callstack));
				else if (Left is IExpressionAssignable expressionAssignable)
					return expressionAssignable.Assign(Right, Callstack);
				else
					throw new ExpressionException(Callstack, $"Cannot assign '{Left}'");
			}
			return Right.GetValue(Callstack, false);
		}
		public override string ToString() => Left + " = " + Right;
	}
}
