using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Objects
{
	class AccessorExpression : UnaryExpression, IAssignable
	{
		public string AccessorName { get; set; }

		public override string Name => "Accessor";


		protected override Value GetValueInternal(bool execute = true)
		{
			if (Parameter.GetValue(Callstack, execute) is not IAccessibleValue accessible)
				throw new ExpressionException(Callstack, $"'{Parameter}' is not accessable");

			if (!accessible.Contains(AccessorName))
				throw new ExpressionException(Callstack, $"'{Parameter}' does not contain '.{AccessorName}'");

			return accessible.GetValueByName(AccessorName);
		}
		public void Assign(Value value)
		{
			Value param = Parameter.GetValue(Callstack, false);
			if (param is not IAccessibleSettableValue accessible)
				throw new ExpressionException(Callstack, $"'{Parameter}' is not accessable or readonly");

			if (value is IThisValue thisValue)
				thisValue.This = param;

			accessible.SetValueByName(AccessorName, value);
		}
		public override string ToString() => $"{Parameter}.{AccessorName}";
	}
}
