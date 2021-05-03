using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Treeing.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressionizer.Mapping
{
	class Operator
	{
		public Operator(int precedence, Associativity associativity, Func<Expression> expression, bool isUnary = false)
		{
			Precedence = precedence;
			Associativity = associativity;
			ExpressionFactory = expression;
			IsUnary = isUnary;
		}

		public int Precedence { get; }
		public Associativity Associativity { get; }
		public Func<Expression> ExpressionFactory { get; }
		public TextRegion Region { get; set; }
		public bool IsUnary { get; }
	}

	public enum Associativity
	{
		LeftToRight,
		RightToLeft
	}
}
