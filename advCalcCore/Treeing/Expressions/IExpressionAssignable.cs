using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Values;

namespace advCalcCore.Treeing.Expressions
{
	interface IExpressionAssignable
	{
		Value Assign(Expression expression, CallStack callstack);
	}
}