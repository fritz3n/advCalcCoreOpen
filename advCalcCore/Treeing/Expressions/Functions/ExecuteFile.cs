using advCalcCore.FileHandle;
using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using advCalcCore.Values.Casting;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Functions
{
	class ExecuteFile : BuiltinFunctionExpression
	{

		public static void Register()
		{
			NamedConstants.RegisterExpression("ExecuteFile", () => new ExecuteFile(), "ExecuteFile(filename)");
		}

		public ExecuteFile() : base(1) { }

		public override string Name => "ExecuteFile";

		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstack) => values.CastingRequest()
			.With((TextValue v) =>
			{
				try
				{
					return LoadFile.ExecuteFile(((TextValue)values[0]).Text);
				}
				catch (Exception e)
				{
					throw new ExpressionException(callstack, "ExecuteFile(" + values[0].ToString() + ") failed", e);
				}
			})
			.GetResult();
	}
}
