using advCalcCore.Treeing.Expressions;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace advCalcCore.Treeing.Expressionizer.Mapping
{
	static class NamedConstants
	{
		private static Dictionary<string, ExpressionConstant> expressions = new Dictionary<string, ExpressionConstant>();
		private static Dictionary<string, ValueConstant> values = new Dictionary<string, ValueConstant>();

		public static IReadOnlyDictionary<string, ExpressionConstant> Expressions => expressions;
		public static IReadOnlyDictionary<string, ValueConstant> Values => values;

		static NamedConstants()
		{
			ConstantList.RegisterAll();
		}

		public static void RegisterExpression(string name, Func<Expression> factory, string description = null) => expressions.Add(name.ToLower(), new ExpressionConstant() { Factory = factory, Description = description });

		public static void RegisterValue(string name, Value value, string description = null) => values.Add(name.ToLower(), new ValueConstant() { Value = value, Description = description });

		/// <summary>
		/// Tries to get an expression corresponding to a named constant
		/// </summary>
		/// <param name="name">The name of the named constant</param>
		/// <param name="expression">If the constant exists the corresponding expression, otherwise null</param>
		/// <returns>True if the constant exists</returns>
		public static bool TryGetExpression(string name, out Expression expression)
		{
			name = name.ToLower();

			if (expressions.ContainsKey(name))
			{
				expression = expressions[name].Factory();
				return true;
			}

			if (values.ContainsKey(name))
			{
				expression = new ConstantExpression(values[name].Value, name);
				return true;
			}

			expression = null;
			return false;
		}
	}

	struct ExpressionConstant
	{
		public Func<Expression> Factory { get; set; }
		public string Description { get; set; }
	}

	struct ValueConstant
	{
		public Value Value { get; set; }
		public string Description { get; set; }
	}
}
