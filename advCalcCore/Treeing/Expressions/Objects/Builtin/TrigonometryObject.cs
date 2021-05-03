using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Objects.Builtin
{
	class TrigonometryObject : BuiltinObjectExpression
	{
		public static void Register()
		{
			NamedConstants.RegisterExpression("Trig", () => new ConsoleObject(), "Trig.sin, Trig.arcsin, Trig.degsin, Trig.degarcsin");

			Object = Builder.New
				.WithFunc("sin", (DecimalValue d) => (DecimalValue)Math.Sin((double)d), true)
				.WithFunc("cos", (DecimalValue d) => (DecimalValue)Math.Cos((double)d), true)
				.WithFunc("tan", (DecimalValue d) => (DecimalValue)Math.Tan((double)d), true)
				.WithFunc("degsin", (DecimalValue d) => (DecimalValue)(Math.Sin((double)d) / Math.PI * 180), true)
				.WithFunc("degcos", (DecimalValue d) => (DecimalValue)(Math.Cos((double)d) / Math.PI * 180), true)
				.WithFunc("degtan", (DecimalValue d) => (DecimalValue)(Math.Tan((double)d) / Math.PI * 180), true)
				.WithFunc("arcsin", (DecimalValue d) => (DecimalValue)Math.Sinh((double)d), true)
				.WithFunc("arccos", (DecimalValue d) => (DecimalValue)Math.Cosh((double)d), true)
				.WithFunc("arctan", (DecimalValue d) => (DecimalValue)Math.Tanh((double)d), true)
				.WithFunc("degarcsin", (DecimalValue d) => (DecimalValue)Math.Sinh((double)d / 180 * Math.PI), true)
				.WithFunc("degarccos", (DecimalValue d) => (DecimalValue)Math.Sinh((double)d / 180 * Math.PI), true)
				.WithFunc("degarctan", (DecimalValue d) => (DecimalValue)Math.Sinh((double)d / 180 * Math.PI), true)
				.Build();
		}

		public override string Name => "Trig";

	}
}
