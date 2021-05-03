using advCalcCore.Treeing.Expressions;
using advCalcCore.Treeing.Expressions.Constructs;
using advCalcCore.Treeing.Expressions.Functions;
using advCalcCore.Treeing.Expressions.Objects;
using advCalcCore.Treeing.Expressions.Objects.Builtin;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressionizer.Mapping
{
	static class ConstantList
	{
		public static void RegisterAll()
		{
			BreakExpression.Register();
			ContinueExpression.Register();
			ReturnFunctionExpression.Register();
			AbsFunction.Register();
			ExecuteFile.Register();
			PowFunction.Register();
			ProductFunction.Register();
			HelpFunction.Register();
			SqrtFunction.Register();
			SumDeepFunction.Register();
			SumFunction.Register();
			LengthFunction.Register();

			ConsoleObject.Register();
			TrigonometryObject.Register();
			ThisConstant.Register();

			BooleanValue.Register();
			ComplexValue.Register();
			DecimalValue.Register();
			NullValue.Register();
		}

	}
}
