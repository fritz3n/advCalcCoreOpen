using advCalcCore.Tokenizing;
using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Treeing.Expressions;
using advCalcCore.Treeing.Expressions.Basic;
using advCalcCore.Treeing.Expressions.Constructs;
using advCalcCore.Treeing.Expressions.Objects;
using advCalcCore.Treeing.Expressions.Other;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressionizer.Mapping
{
	static class Mapper
	{
		public static bool IsOperator(Token token) => (token.Type & Token.TokenType.Operator) == Token.TokenType.Operator;
		public static bool IsValue(Token token) => (token.Type & Token.TokenType.Value) == Token.TokenType.Value;

		public static Operator MapOperator(Token token) => token.Name switch
		{
			"call" => new Operator(20, Associativity.LeftToRight, () =>
			{
				var compound = token as CompoundToken;

				List<Expression> parameters;

				if (compound.Tokens.Count == 0)
				{
					parameters = new List<Expression>();
				}
				else
				{
					parameters = new Expressionizer().Expressionize((token as CompoundToken).Tokens, null, "seperator").ToList();
				}

				return new FunctionCallExpression()
				{
					Parameters = parameters
				};
			}),

			"index" => new Operator(20, Associativity.LeftToRight, () =>
			{
				var compound = token as CompoundToken;

				Expression index = null;

				if (compound.Tokens.Count > 0)
					index = new Expressionizer().ExpressionizeSingle((token as CompoundToken).Tokens);

				return new IndexExpression()
				{
					Index = index
				};
			}),

			"Accessor" => new Operator(18, Associativity.LeftToRight, () =>
			{
				return new AccessorExpression()
				{
					AccessorName = token.Text[1..]
				};
			}, true),
			"++" => new Operator(17, Associativity.LeftToRight, () => new IncrementExpression()),
			"--" => new Operator(17, Associativity.LeftToRight, () => new DecrementExpresseion()),
			"^" => new Operator(16, Associativity.LeftToRight, () => new PowExpression()),
			"*" => new Operator(15, Associativity.LeftToRight, () => new MultiplyExpression()),
			"/" => new Operator(15, Associativity.LeftToRight, () => new DivideExpression()),
			"%" => new Operator(15, Associativity.LeftToRight, () => new ModuloExpression()),
			"+" => new Operator(14, Associativity.LeftToRight, () => new PlusExpression()),
			"-" => new Operator(14, Associativity.LeftToRight, () => new MinusExpression()),
			">" => new Operator(13, Associativity.LeftToRight, () => new GreaterExpression()),
			"<" => new Operator(13, Associativity.LeftToRight, () => new SmallerExpression()),
			"==" => new Operator(12, Associativity.LeftToRight, () => new EqualExpression()),
			"!=" => new Operator(12, Associativity.LeftToRight, () => new NotEqualExpression()),
			"=" => new Operator(2, Associativity.RightToLeft, () => new AssignExpression()),
			"*=" => new Operator(2, Associativity.RightToLeft, () => new MultiplyAssignExpression()),
			"/=" => new Operator(2, Associativity.RightToLeft, () => new DivideAssignExpression()),
			"-=" => new Operator(2, Associativity.RightToLeft, () => new MinusAssignExpression()),
			"+=" => new Operator(2, Associativity.RightToLeft, () => new PlusAssignExpression()),
			"key" => new Operator(1, Associativity.LeftToRight, () => new KeyValuePairExpression() { Key = (token as RegexToken).Groups[1] }),
			_ => throw new KeyNotFoundException()
		};

		public static Expression MapValue(Token token)
		{
			switch (token.Name)
			{
				case "if":
					var ifToken = token as ConstructToken;
					Expression condition = new Expressionizer().ExpressionizeSingle((ifToken["condition"] as CompoundToken).Tokens);
					Expression ifBlock = new CodeBlockExpression(new Expressionizer().Expressionize((ifToken["ifBlock"] as CompoundToken).Tokens).ToList())
					{
						TextRegion = ifToken["ifBlock"].Range
					};

					if (ifToken["elseBlock"] is CompoundToken elseToken)
					{
						Expression elseBlock = new CodeBlockExpression(new Expressionizer().Expressionize(elseToken.Tokens).ToList())
						{
							TextRegion = ifToken["elseBlock"].Range
						};
						return new IfElseExpression()
						{
							Condition = condition,
							IfBlock = ifBlock,
							ElseBlock = elseBlock,
							TextRegion = token.Range
						};
					}
					else
					{
						return new IfExpression()
						{
							Condition = condition,
							IfBlock = ifBlock,
							TextRegion = token.Range
						};
					}

				case "while":
					var whileToken = token as ConstructToken;
					condition = new Expressionizer().ExpressionizeSingle((whileToken["condition"] as CompoundToken).Tokens);
					Expression whileBlock = new CodeBlockExpression(new Expressionizer().Expressionize((whileToken["whileBlock"] as CompoundToken).Tokens).ToList())
					{
						TextRegion = whileToken["whileBlock"].Range
					};

					return new WhileExpression()
					{
						Condition = condition,
						WhileBlock = whileBlock,
						TextRegion = token.Range
					};

				case "for":
					var forToken = token as ConstructToken;
					var list = new Expressionizer().Expressionize((forToken["condition"] as CompoundToken).Tokens, null, "instructSeperator", true).ToList();

					Expression initial = null;
					condition = null;
					Expression incremental = null;

					if (list.Count > 3)
						throw new ArgumentException("for block has to many arguments");

					if (list.Count > 0)
						initial = list[0];
					if (list.Count > 1)
						condition = list[1];
					if (list.Count > 2)
						incremental = list[2];

					Expression forBlock = new CodeBlockExpression(new Expressionizer().Expressionize((forToken["forBlock"] as CompoundToken).Tokens).ToList())
					{
						TextRegion = forToken["forBlock"].Range
					};

					return new ForExpression()
					{
						Initial = initial,
						Condition = condition,
						Incremental = incremental,
						ForBlock = forBlock,
						TextRegion = token.Range
					};

				case "lambda":
					var lambdaToken = token as ConstructToken;

					List<Expression> parameters;

					var brackets = lambdaToken["parameters"] as CompoundToken;

					if (brackets.Tokens.Count == 0)
					{
						parameters = new List<Expression>();
					}
					else
					{
						parameters = new Expressionizer().Expressionize(brackets.Tokens, null, "seperator").ToList();
						if (parameters.Any(p => p is not IdentifierExpression))
							throw new ArgumentException("Lambda expression parameters can only contain identifiers.");
					}

					Expression codeBlock = new CodeBlockExpression(new Expressionizer().Expressionize((lambdaToken["codeBlock"] as CompoundToken).Tokens).ToList())
					{
						TextRegion = lambdaToken["codeBlock"].Range
					};

					return new LambdaExpression()
					{
						Parameters = parameters,
						CodeBlock = codeBlock
					};

				case "object":

					var objectToken = token as CompoundToken;
					Dictionary<string, Expression> pairs;

					if (objectToken.Tokens.Count == 0)
					{
						pairs = new();
					}
					else
					{
						parameters = new Expressionizer().Expressionize(objectToken.Tokens, null, "seperator").ToList();
						if (parameters.Any(p => p is not KeyValuePairExpression))
							throw new ArgumentException("Object expression parameters can only contain Key Value Pairs.");
						pairs = parameters.ToDictionary(p => (p as KeyValuePairExpression).Key, p => (p as KeyValuePairExpression).Parameter);
					}

					return new ObjectExpression()
					{
						Pairs = pairs
					};


				case "ident":
					if (NamedConstants.TryGetExpression(token.Text, out Expression constant))
					{
						constant.TextRegion = token.Range;
						return constant;
					}

					return new IdentifierExpression() { Identifier = token.Text, TextRegion = token.Range };

				case "multIdent":
					var multToken = token as RegexToken;

					Value value = new DecimalValue(decimal.Parse(multToken.Groups[1]));

					if (NamedConstants.TryGetExpression(multToken.Groups[2], out constant))
					{
						constant.TextRegion = new TextRegion(token.Range.Start + multToken.Groups[1].Length, token.Range.End);

						return new ConstantMultExpression()
						{
							Parameter = constant,
							Multiplier = value,
							TextRegion = new TextRegion(token.Range.Start, token.Range.End - multToken.Groups[1].Length)
						};
					}

					return new MultIdentifierExpression() { Identifier = multToken.Groups[2], TextRegion = token.Range, Multiplier = value };

				case "globalIdent":
					return new GlobalIdentifierExpression() { Identifier = token.Text[1..], TextRegion = token.Range };

				case "num":
					return new NumberExpression() { Number = decimal.Parse(token.Text), TextRegion = token.Range };

				case "int":
					return new IntExpression() { Number = int.Parse(token.Text), TextRegion = token.Range };

				case "text":
					return new TextExpression() { Text = token.Text, TextRegion = token.Range };

				default:
					throw new KeyNotFoundException();
			}
		}
	}
}
