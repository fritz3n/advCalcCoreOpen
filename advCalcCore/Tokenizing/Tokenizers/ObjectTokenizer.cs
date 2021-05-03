using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Tokenizing.Tokenizers
{
	class ObjectTokenizer : ITokenizer
	{
		public Func<char, bool> Selector => c => c == '{';

		private ITokenizer keyTokenizer = new RegexTokenizer("([a-zA-Z_]\\w*)[^\\S\\n]*:", Token.TokenType.Operator, "key", new CharRange { Min = 'A', Max = 'z' }, Token.TokenType.None | Token.TokenType.Operator);
		private ITokenizer bracketTokenizer = null;
		private List<ITokenizer> tokenizers = null;


		public TokenizerResult Tokenize(ITracker tracker)
		{
			if (tokenizers is null)
			{
				tokenizers = new();
				tokenizers.AddRange(StandardList.List);
				tokenizers.Insert(32, keyTokenizer);
				bracketTokenizer = new BracketTokenizer('{', '}', Token.TokenType.Value, Token.TokenType.Operator | Token.TokenType.None, "object", tokenizers);
			}

			return bracketTokenizer.Tokenize(tracker);
		}
	}
}