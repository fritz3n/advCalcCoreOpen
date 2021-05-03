using advCalcCore.Tokenizing.Tokenizers;
using advCalcCore.Tokenizing.Tokenizers.Constructs;
using advCalcCore.Tokenizing.Tokens;
using System.Collections.Generic;

namespace advCalcCore.Tokenizing
{
	static class StandardList
	{
		// Warning: Order of the list is important

		public static List<ITokenizer> List = new List<ITokenizer>
		{
            // Whitespace
            new WhitespaceWithoutNewlineTokenizer(),
            // new WhitespaceTokenizer() - The WithepaceTokenizer is intended to also remove \n in code constructs e.g. "if(bool)\n{code}"

            // Seperator
            new CharTokenizer('\n', Token.TokenType.None, "instructSeperator", Token.TokenType.None | Token.TokenType.Value), // WhitespaceTokenizer removes \r
            new CharTokenizer(';', Token.TokenType.None, "instructSeperator", Token.TokenType.None | Token.TokenType.Value),
			new CharTokenizer(Locale.ElementSeperator[0], Token.TokenType.Operator, "seperator", Token.TokenType.Value),

            // Accessor (e.g. .WriteLine)
            new RegexTokenizer($@"\.([a-zA-Z_]\w*)", Token.TokenType.Value | Token.TokenType.Operator, "Accessor", new CharRange{Min = '.', Max = '.' }, Token.TokenType.Value),

			new RegexTokenizer(@$"(-?(?:\d+\{Locale.DecimalSeperator}\d+|\{Locale.DecimalSeperator}\d+|\d+))([a-zA-Z_]\w*)", Token.TokenType.Value, "multIdent", new CharRange{Min = '-', Max = '9' }, Token.TokenType.None | Token.TokenType.Operator),

            // Value
            new RegexTokenizer($@"(-?(?:\d+\{Locale.DecimalSeperator}\d+|\{Locale.DecimalSeperator}\d+))", Token.TokenType.Value, "num", new CharRange{Min = '-', Max = '9' }, Token.TokenType.None | Token.TokenType.Operator),
			new RegexTokenizer($@"(-?(?:\d+))", Token.TokenType.Value, "int", new CharRange{Min = '-', Max = '9' }, Token.TokenType.None | Token.TokenType.Operator),
			new TextTokenizer('"', Token.TokenType.None | Token.TokenType.Operator, Token.TokenType.Value, "text"),

            // Operator
            new StringTokenizer("==", Token.TokenType.Operator, Token.TokenType.Value),
			new StringTokenizer("!=", Token.TokenType.Operator, Token.TokenType.Value),
			new StringTokenizer("+=", Token.TokenType.Operator, Token.TokenType.Value),
			new StringTokenizer("-=", Token.TokenType.Operator, Token.TokenType.Value),
			new StringTokenizer("*=", Token.TokenType.Operator, Token.TokenType.Value),
			new StringTokenizer("/=", Token.TokenType.Operator, Token.TokenType.Value),
			new StringTokenizer("^=", Token.TokenType.Operator, Token.TokenType.Value),
			new StringTokenizer("++", Token.TokenType.Value | Token.TokenType.Operator, Token.TokenType.Value),
			new StringTokenizer("--", Token.TokenType.Value | Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('!', Token.TokenType.Operator, "negate", Token.TokenType.Operator | Token.TokenType.None),
			new CharTokenizer('!', Token.TokenType.Value | Token.TokenType.Operator, "factorial", Token.TokenType.Value),
			new CharTokenizer('?', Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('=', Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('+', Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('-', Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('-', Token.TokenType.Operator, "negate", Token.TokenType.None),
			new CharTokenizer('*', Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('%', Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('/', Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('^', Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('>', Token.TokenType.Operator, Token.TokenType.Value),
			new CharTokenizer('<', Token.TokenType.Operator, Token.TokenType.Value),

            // Code Construct (g.e. if, for, while, ...)
            new IfTokenizer(),
			new WhileTokenizer(),
			new ForTokenizer(),
			new LambdaTokenizer(),

			new ObjectTokenizer(),

			// Regex (= Type ? functionName : Operator; else: variable)
			new RegexTokenizer(@"([a-zA-Z_]\w*)", Token.TokenType.Value, "ident", new CharRange{Min = 'A', Max = 'z' }, Token.TokenType.None | Token.TokenType.Operator),
			new RegexTokenizer(@"\$([a-zA-Z_]\w*)", Token.TokenType.Value, "globalIdent", new CharRange{Min = '$', Max = '$' }, Token.TokenType.None | Token.TokenType.Operator),
            
            // Bracket
            new BracketTokenizer('(', ')', Token.TokenType.Operator | Token.TokenType.Value, Token.TokenType.Value, "call"),
			new BracketTokenizer('(', ')', Token.TokenType.Value, Token.TokenType.Operator | Token.TokenType.None),
			new BracketTokenizer('{', '}'),
			new BracketTokenizer('[', ']', Token.TokenType.Operator | Token.TokenType.Value, Token.TokenType.Value, "index"),
			new BracketTokenizer('[', ']', Token.TokenType.Value, Token.TokenType.Operator | Token.TokenType.None),
		};
	}
}
