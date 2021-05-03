using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Tokenizing.Tokenizers.Constructs
{
	abstract class ConstructTokenizer : ITokenizer
	{

		public abstract Func<char, bool> Selector { get; }

		public TokenizerResult Tokenize(ITracker tracker)
		{
			ConstructToken token;
			using (var point = new ResetPoint(tracker))
				token = TokenizeConstruct(tracker);

			if (token is null)
			{
				return TokenizerResult.Failure;
			}
			else
			{
				tracker.AddToken(token, token.Range.End - tracker.Index);
				return TokenizerResult.Success;
			}

		}


		protected static bool TryTokenize(ITracker tracker, ITokenizer tokenizer, out Token output, string name = null)
		{
			if (tracker.Read() is char character && tokenizer.Selector(character))
			{
				Guid reset = tracker.GetResetPoint();
				if (tokenizer.Tokenize(tracker) == TokenizerResult.Success)
				{
					Token token = tracker.Last;
					if (token != null)
					{
						if (name != null)
							token.Name = name;
						output = token;
						return true;
					}
				}
				tracker.Reset(reset);
			}
			output = null;
			return false;
		}

		protected static bool TryTokenize(ITracker tracker, ITokenizer tokenizer, IList<Token> outputList, string name = null)
		{
			if (TryTokenize(tracker, tokenizer, out Token token, name))
			{
				outputList.Add(token);
				return true;
			}
			return false;
		}

		protected abstract ConstructToken TokenizeConstruct(ITracker tracker);
	}
}
