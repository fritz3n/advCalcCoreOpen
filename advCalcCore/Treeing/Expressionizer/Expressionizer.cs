using advCalcCore.Tokenizing;
using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions;
using advCalcCore.Treeing.Expressions.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressionizer
{
	public class Expressionizer
	{
		Stack<Operator> operators;
		Stack<Expression> expressions;
		bool unaryAwaiting = false;

		public IEnumerable<Expression> Expressionize(IEnumerable<Token> tokens, string originalText = null, string delimiter = "instructSeperator", bool keepNull = false)
		{
			var sectionTokens = new List<Token>();
			foreach (Token token in tokens)
			{
				if (token.Name == delimiter)
				{
					if (sectionTokens.Count > 0)
						yield return ExpressionizeSingle(sectionTokens, originalText);
					else if (keepNull)
						yield return null;
					sectionTokens.Clear();
				}
				else
				{
					sectionTokens.Add(token);
				}
			}
			if (sectionTokens.Count > 0)
				yield return ExpressionizeSingle(sectionTokens, originalText);
		}

		public Expression ExpressionizeSingle(IEnumerable<Token> tokens, string originalText = null)
		{
			operators = new Stack<Operator>();
			expressions = new Stack<Expression>();

			foreach (Token t in tokens)
			{
				if (t.Name == "()")
				{
					expressions.Push(new SimpleBracketExpression(new Expressionizer().ExpressionizeSingle((t as CompoundToken).Tokens))
					{
						TextRegion = t.Range
					});
				}
				else if (t.Name == "{}")
				{
					expressions.Push(new CodeBlockExpression()
					{
						Instructions = new Expressionizer().Expressionize((t as CompoundToken).Tokens).ToList(),
						TextRegion = t.Range
					});
				}
				else if (t.Name == "[]")
				{
					var compound = t as CompoundToken;

					List<Expression> parameters;

					if (compound.Tokens.Count == 0)
					{
						parameters = new List<Expression>();
					}
					else
					{
						parameters = new Expressionizer().Expressionize(compound.Tokens, null, "seperator").ToList();
					}

					expressions.Push(new ListExpression()
					{
						Parameters = parameters,
						TextRegion = t.Range
					});
				}
				else if (Mapper.IsOperator(t))
				{
					HandleOperatorToken(t);
				}
				else if (Mapper.IsValue(t))
				{

					if (unaryAwaiting)
					{
						unaryAwaiting = false;

						Expression previous = expressions.Peek();
						if (previous is UnaryExpression unary)
						{
							Expression right = Mapper.MapValue(t);
							unary.Parameter = right;

							int contextTextRegionEnd = (right.ContextTextRegion ?? right.TextRegion).End;
							unary.ContextTextRegion = new TextRegion(unary.TextRegion.Start, contextTextRegionEnd);
						}
						else
						{
							expressions.Push(Mapper.MapValue(t));
						}
					}
					else
					{
						expressions.Push(Mapper.MapValue(t));
					}
				}
			}

			while (operators.Count > 0)
				HandleOperator(operators.Pop());

			Expression result = expressions.Single();
			result.OriginalText = originalText;

			return result;

		}

		private void HandleOperatorToken(Token t)
		{
			Operator op = Mapper.MapOperator(t);
			op.Region = t.Range;

			if (op.IsUnary && op.Associativity == Associativity.LeftToRight)
			{
				HandleOperator(op);
			}
			else
			{
				while (operators.Count > 0 && (
					operators.Peek().Precedence > op.Precedence ||
					(operators.Peek().Precedence == op.Precedence && op.Associativity == Associativity.LeftToRight)
					))
				{
					Operator previous = operators.Pop();

					HandleOperator(previous);
				}

				operators.Push(op);
			}
		}

		private void HandleOperator(Operator previous)
		{
			Expression expression = previous.ExpressionFactory();
			expression.TextRegion = previous.Region;

			if (expression is MultiparamExpression multiparam)
			{
				Expression first = expressions.Pop();
				int contextTextRegionStart = (first.ContextTextRegion ?? first.TextRegion).Start;
				int contextTextRegionEnd = (expressions.Peek().ContextTextRegion ?? expressions.Peek().TextRegion).End;

				var parametes = new List<Expression>
							{
								first,
								expressions.Pop()
							};

				while (operators.Count > 0 && operators.Peek().Precedence == previous.Precedence)
				{
					operators.Pop();
					Expression exp = expressions.Pop();
					contextTextRegionEnd = (exp.ContextTextRegion ?? exp.TextRegion).End;
					parametes.Add(exp);
				}

				parametes.Reverse();
				multiparam.Parameters = parametes;
				multiparam.ContextTextRegion = new TextRegion(contextTextRegionStart, contextTextRegionEnd);
			}
			else if (expression is BinaryExpression binary)
			{
				Expression right = expressions.Pop();
				Expression left = expressions.Pop();

				binary.Left = left;
				binary.Right = right;

				int contextTextRegionStart = (left.ContextTextRegion ?? left.TextRegion).Start;
				int contextTextRegionEnd = (right.ContextTextRegion ?? right.TextRegion).End;

				binary.ContextTextRegion = new TextRegion(contextTextRegionStart, contextTextRegionEnd);
			}
			else if (expression is UnaryExpression unary)
			{
				if (previous.Associativity == Associativity.LeftToRight)
				{
					Expression left = expressions.Pop();
					unary.Parameter = left;

					int contextTextRegionStart = Math.Min((left.ContextTextRegion ?? left.TextRegion).Start, (unary.ContextTextRegion ?? unary.TextRegion).Start);
					int contextTextRegionEnd = Math.Max((left.ContextTextRegion ?? left.TextRegion).End, (unary.ContextTextRegion ?? unary.TextRegion).End);
					unary.ContextTextRegion = new TextRegion(contextTextRegionStart, contextTextRegionEnd);
				}
				else
				{
					unaryAwaiting = true;
				}
			}

			expressions.Push(expression);
		}
	}
}
