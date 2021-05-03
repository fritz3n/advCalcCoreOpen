using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Objects.Builtin
{
	class FuncFunction : BuiltinFunctionExpression
	{
		private readonly Func<List<Value>, IdentifierStore, CallStack, Value> func;

		public FuncFunction(string name, Func<List<Value>, IdentifierStore, CallStack, Value> func, int parameterCount) : base(parameterCount)
		{
			Name = name;
			this.func = func;
		}

		public FuncFunction(string name, Func<List<Value>, IdentifierStore, CallStack, Value> func, int minParameterCount, int maxParameterCount) : base(minParameterCount, maxParameterCount)
		{
			Name = name;
			this.func = func;
		}

		public override string Name { get; }

		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstack)
		{
			return func(values, identifierStore, callstack);
		}
	}
}
