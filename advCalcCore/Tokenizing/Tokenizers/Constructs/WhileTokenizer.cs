using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Collections.Generic;

namespace advCalcCore.Tokenizing.Tokenizers.Constructs
{
	class WhileTokenizer : ConstructTokenizer
	{
		public override Func<char, bool> Selector => c => c == 'w';

		private readonly ITokenizer whileString = new StringTokenizer("while", Token.TokenType.Value, Token.TokenType.Operator | Token.TokenType.None);
		private readonly ITokenizer roundBrackets = new BracketTokenizer('(', ')');
		private readonly ITokenizer curlyBrackets = new BracketTokenizer('{', '}');
		private readonly ITokenizer whitespaces = new WhitespaceTokenizer();

		protected override ConstructToken TokenizeConstruct(ITracker tracker)
		{
			int temp = tracker.Index;
			var tokens = new List<Token>();

			if (!TryTokenize(tracker, whileString, tokens))
				return null;

			whitespaces.Tokenize(tracker);

			if (!TryTokenize(tracker, roundBrackets, tokens, "condition"))
				return null;

			whitespaces.Tokenize(tracker);

			if (!TryTokenize(tracker, curlyBrackets, tokens, "whileBlock"))
				return null;

			return new ConstructToken(new TextRegion(temp, tracker.Index), tokens, "while");
		}
	}
}
