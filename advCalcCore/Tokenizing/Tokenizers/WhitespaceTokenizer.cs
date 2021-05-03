using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tracker;
using System;

namespace advCalcCore.Tokenizing.Tokenizers
{
	/// <summary>
	/// Despite the name, this just throws away whitespaces
	/// </summary>
	class WhitespaceTokenizer : ITokenizer
	{
		static readonly CharRange range = new CharRange() { Min = (char)1, Max = ' ' };
		public Func<char, bool> Selector => c => range.Contains(c);
		private char c;

		public TokenizerResult Tokenize(ITracker tracker)
		{
			int count = 0;
			while (tracker.ReadWithOffset(count, out c) && range.Contains(c))
			{
				count++;
			}
			tracker.AddToken(null, count);
			return TokenizerResult.Success;
		}
	}
}
