using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace advCalcCore.Tokenizing.Tokenizers
{
	class RegexTokenizer : ITokenizer
	{
		public Func<char, bool> Selector => c => First.Contains(c);

		private readonly Regex Regex;
		private readonly CharRange First;
		private readonly Token.TokenType Type;
		private readonly Token.TokenType PreviousType = Token.TokenType.NA;
		private readonly string[] PreviousNames;

		private readonly string Name;

		public RegexTokenizer(string regex, Token.TokenType type, string name, CharRange? first = null, Token.TokenType previousType = Token.TokenType.NA, params string[] previousNames)
		{
			Regex = new Regex("^" + regex);
			First = first ?? new CharRange { Min = char.MinValue, Max = char.MaxValue };
			Type = type;
			Name = name;
			PreviousType = previousType;
			PreviousNames = previousNames;
		}


		public TokenizerResult Tokenize(ITracker tracker)
		{
			// Fail if the previoustype is not NA and the type of the previous token doesn´t match
			if (PreviousType != Token.TokenType.NA && ((tracker.Before?.Type ?? Token.TokenType.None) & PreviousType) == Token.TokenType.NA)
			{
				tracker.AddError("'" + (tracker.Before?.ToString() ?? "") + "' Can´t appear in front of " + Type, 1);
				return TokenizerResult.Failure;
			}

			if (PreviousNames != null && PreviousNames.Length != 0 && !PreviousNames.Contains(tracker.Before?.Name))
			{
				tracker.AddError("'" + (tracker.Before?.ToString() ?? "") + "' Can´t appear in front of " + Type, 1);
				return TokenizerResult.Failure;
			}


			Match Match = Regex.Match(tracker.Remaining);

			if (!Match.Success)
				return TokenizerResult.Failure;

			if (Match.Index != 0)
				return TokenizerResult.Failure;

			if (Match.Length == 0)
				return TokenizerResult.Failure;

			string consumed = Match.Value;

			var t = new RegexToken(new TextRegion(tracker.Index, tracker.Index + consumed.Length), consumed, (Match.Groups as IEnumerable<Group>).Select(g => g.Value).ToArray(), Name, Type);

			tracker.AddToken(t, consumed.Length);

			return TokenizerResult.Success;
		}
	}

	struct CharRange
	{
		public char Min { get; set; }
		public char Max { get; set; }

		public bool Contains(char c)
		{
			return Min <= c && c <= Max;
		}
	}
}
