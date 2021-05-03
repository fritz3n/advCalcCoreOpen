using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Tokenizing.Tokenizers.Constructs
{
	class IfTokenizer : ConstructTokenizer
	{
		public override Func<char, bool> Selector => c => c == 'i';

		private readonly ITokenizer ifString = new StringTokenizer("if", Token.TokenType.Value, Token.TokenType.Operator | Token.TokenType.None);
		private readonly ITokenizer elseString = new StringTokenizer("else", Token.TokenType.Operator);
		private readonly ITokenizer roundBrackets = new BracketTokenizer('(', ')');
		private readonly ITokenizer curlyBrackets = new BracketTokenizer('{', '}');
		private readonly ITokenizer whitespaces = new WhitespaceTokenizer();

		protected override ConstructToken TokenizeConstruct(ITracker tracker)
		{
			int temp = tracker.Index;

			var tokens = new List<Token>();

			if (!TryTokenize(tracker, ifString, tokens))
				return null;

			whitespaces.Tokenize(tracker);

			if (!TryTokenize(tracker, roundBrackets, tokens, "condition"))
				return null;

			whitespaces.Tokenize(tracker);

			if (!TryTokenize(tracker, curlyBrackets, tokens, "ifBlock"))
				return null;

			using (var point = new ResetPoint(tracker))
			{
				whitespaces.Tokenize(tracker);

				if (TryTokenize(tracker, elseString, tokens))
				{
					whitespaces.Tokenize(tracker);
					if (!TryTokenize(tracker, curlyBrackets, tokens, "elseBlock"))
						return null;
					point.Disable();
				}
			}

			return new ConstructToken(new TextRegion(temp, tracker.Index), tokens, "if");
		}
	}
}
