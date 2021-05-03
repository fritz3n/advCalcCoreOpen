using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tracker;
using System;

namespace advCalcCore.Tokenizing.Tokenizers
{
	/// <summary>
	/// Despite the name, this just throws away whitespaces
	/// </summary>
	class WhitespaceWithoutNewlineTokenizer : ITokenizer
	{
		static readonly CharRange range = new CharRange() { Min = (char)1, Max = ' ' };
		public Func<char, bool> Selector => c => c != '\n' && range.Contains(c);
		private char c;

		public TokenizerResult Tokenize(ITracker tracker)
		{
			int count = 0;

			while (tracker.ReadWithOffset(count, out c) && range.Contains(c))
			{
				if (c == '\n')  // Newline is used as Seperator
					break;

				count++;
			}

			tracker.AddToken(null, count);

			return TokenizerResult.Success;
		}
	}
}
