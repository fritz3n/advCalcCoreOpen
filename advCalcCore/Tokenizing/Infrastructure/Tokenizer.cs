using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokenizers;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace advCalcCore.Tokenizing
{
	public class Tokenizer
	{
		private readonly List<ITokenizer> Tokenizers;
		private int index = 0;

		List<TokenizeError> Errors = new List<TokenizeError>();

		private int Level = 0;

		public Tokenizer()
		{
			Tokenizers = StandardList.List;
		}

		public Tokenizer(IEnumerable<ITokenizer> tokenizers)
		{
			Tokenizers = tokenizers.ToList();
		}

		public List<Token> Tokenize(string expression, int indexOffset = 0, int stopBeforeIndex = 0)
		{
			return Tokenize(new ProxyTracker(expression, indexOffset, stopBeforeIndex));
		}

		public List<Token> TokenizeFrom(ITracker tracker, Guid resetPoint, int untilIndex = 0)
		{
			Tokenize(tracker, untilIndex);

			return tracker.TokensFrom(resetPoint).ToList();
		}

		public List<Token> Tokenize(ITracker tracker, int untilIndex = 0)
		{
			Guid newestValidResetPoint = tracker.GetResetPoint();

			index = tracker.Index;
			Level = 0;

			if (0 == untilIndex || tracker.StopIndex < untilIndex)
				untilIndex = tracker.StopIndex;

			while (index < untilIndex)
			{
				if (Level >= Tokenizers.Count)
				{
					Errors.Reverse();
					TokenizeError specific = (from e in Errors
											  orderby e.Specificity descending
											  select e).First();

					throw new TokenizeException(tracker.Raw, specific.Region, tracker.Last, specific.Message);
				}

				if (!Tokenizers[Level].Selector(tracker.Raw[index]))
				{
					AddError("unknown char: '" + tracker.Raw[index] + "'", 0, new TextRegion(index), Tokenizers[Level]);
					Level++;
					continue;
				}

				// Reset errors from last iteration in the tracker
				if (!tracker.ResetErrorsToLastResetPoint(newestValidResetPoint))
				{
					throw new TokenizeException(tracker.Raw, new TextRegion(0, 0), tracker.Last, "Could not reset to last resetpoint - that should not be possible.");
				}

				// Try next Tokenizer
				TokenizerResult result = Tokenizers[Level].Tokenize(tracker);

				if (result == TokenizerResult.Success)
				{
					newestValidResetPoint = tracker.GetResetPoint();
					index = tracker.Index;

					Level = 0;
					Errors.Clear();
				}
				else
				{
					if (!tracker.HasErrors)
						AddError(Tokenizers[Level] + ": Error!", 0, new TextRegion(index), Tokenizers[Level]);
					else
						Errors.AddRange(tracker.Errors);
					tracker.Reset(newestValidResetPoint);
					Level++;
				}
			}

			return tracker.Tokens.ToList();
		}

		private void AddError(string message, int specificity, TextRegion region, ITokenizer tokenizer) => Errors.Add(new TokenizeError(specificity, message, region, tokenizer));
	}
}