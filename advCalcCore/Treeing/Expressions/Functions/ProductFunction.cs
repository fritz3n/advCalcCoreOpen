using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using advCalcCore.Values.Casting;
using System.Collections.Generic;


namespace advCalcCore.Treeing.Expressions
{
	class ProductFunction : BuiltinFunctionExpression
	{
		public override string Name => "Product";
		public static void Register()
		{
			NamedConstants.RegisterExpression("Product", () => new ProductFunction(), "Product(x1, x2, ...)");
		}

		public ProductFunction() : base(1, int.MaxValue) { }


		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstack) => CalculateProduct(values);

		private Value CalculateProduct(List<Value> value)
		{
			Value product = value[0];
			value.RemoveAt(0);
			foreach (Value t in value)
			{
				product = product * t;
			}
			return product;
		}
	}
}