using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Collections.Generic;

namespace advCalcCore.Tokenizing.Tokenizers
{

	class BracketTokenizer : ITokenizer
	{
		public Func<char, bool> Selector => c => c == OpeningBracket;

		public Token.TokenType PreviousType { get; }
		public Token.TokenType Type { get; }
		public string Name { get; }

		private readonly char OpeningBracket;
		private readonly char ClosingBracket;
		private List<ITokenizer> tokenizers;

		public BracketTokenizer(char openingBracket, char closingBracket, Token.TokenType type = Token.TokenType.Value, Token.TokenType previousType = Token.TokenType.NA, string name = null, List<ITokenizer> tokenizers = null)
		{
			OpeningBracket = openingBracket;
			ClosingBracket = closingBracket;
			PreviousType = previousType;
			Type = type;
			Name = name;
			this.tokenizers = tokenizers;
		}

		public TokenizerResult Tokenize(ITracker tracker)
		{
			tokenizers ??= StandardList.List;

			if (PreviousType != Token.TokenType.NA && ((tracker.Before?.Type ?? Token.TokenType.None) & PreviousType) == Token.TokenType.NA)
			{
				return TokenizerResult.Failure;
			}

			int level = 0;

			int textRegionStart = tracker.Index;

			string readString = "";
			while (true)
			{
				if (!tracker.ReadWithOffset(readString.Length, out char c))
				{
					tracker.AddError("No closing bracket in level: " + level, 2, new TextRegion(tracker.InitialIndex));
					return TokenizerResult.Failure;
				}
				readString += c;

				if (c == OpeningBracket)
				{
					level++;
				}
				else if (c == ClosingBracket)
				{
					level--;

					if (level == 0)
					{
						List<Token> tokens;
						using (var point = new ResetPoint(tracker))
						{
							// Usage of PseudoToken to ensure that tracker.Last returns TokenType.None for further checks
							var pseudoToken = new Token(new TextRegion(tracker.Index, tracker.Index + 1), OpeningBracket.ToString(), "PseudoToken", Token.TokenType.None);
							tracker.AddToken(pseudoToken, 1);
							tokens = new Tokenizer(tokenizers).TokenizeFrom(tracker, point.resetPoint, tracker.Index - 1 + readString.Length - 1);
							tokens.Remove(pseudoToken); // should equal tokens.RemoveAt(0)
						}

						tracker.AddToken(new CompoundToken(new TextRegion(textRegionStart, textRegionStart + readString.Length), tokens, OpeningBracket, ClosingBracket, Type, Name), readString.Length);
						return TokenizerResult.Success;
					}
				}
			}
		}
	}
}
