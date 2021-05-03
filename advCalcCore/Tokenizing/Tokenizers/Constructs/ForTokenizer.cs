using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Tokenizing.Tokenizers.Constructs
{
	class ForTokenizer : ConstructTokenizer
	{
		public override Func<char, bool> Selector => c => c == 'f';

		private readonly ITokenizer forString = new StringTokenizer("for", Token.TokenType.Value, Token.TokenType.Operator | Token.TokenType.None);
		private readonly ITokenizer roundBrackets = new BracketTokenizer('(', ')');
		private readonly ITokenizer curlyBrackets = new BracketTokenizer('{', '}');
		private readonly ITokenizer whitespaces = new WhitespaceTokenizer();

		protected override ConstructToken TokenizeConstruct(ITracker tracker)
		{
			int temp = tracker.Index;

			var tokens = new List<Token>();

			if (!TryTokenize(tracker, forString, tokens))
				return null;

			whitespaces.Tokenize(tracker);

			if (!TryTokenize(tracker, roundBrackets, tokens, "condition"))
				return null;

			whitespaces.Tokenize(tracker);

			if (!TryTokenize(tracker, curlyBrackets, tokens, "forBlock"))
				return null;

			return new ConstructToken(new TextRegion(temp, tracker.Index), tokens, "for");
		}
	}
}
