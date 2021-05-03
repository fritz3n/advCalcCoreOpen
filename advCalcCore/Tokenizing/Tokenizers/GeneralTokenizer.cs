using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Linq;

namespace advCalcCore.Tokenizing.Tokenizers
{
    abstract class GeneralTokenizer : ITokenizer
    {
        public abstract Func<char, bool> Selector { get; }

        private readonly Token.TokenType Type;
        private readonly Token.TokenType PreviousType = Token.TokenType.NA;
        private readonly string[] PreviousNames;

        private readonly string Name;

        public GeneralTokenizer(Token.TokenType type, string name, Token.TokenType previousType = Token.TokenType.NA, params string[] previousNames)
        {
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

            if (!MatchesValue(tracker, out string consumed))
                return TokenizerResult.Failure;

            Token t = new Token(new TextRegion(tracker.Index, tracker.Index + consumed.Length), consumed, Name, Type);

            tracker.AddToken(t, consumed.Length);

            return TokenizerResult.Success;
        }

        /// <summary>
        /// This Method should return, wether the tracker matches the value / Expression of the Tokenizer.
        /// ALL CONSUMED CHARS ARE ASSUMED TO BE THE RESULTING TOKEN
        /// </summary>
        /// <param name="tracker"></param>
        /// <param name="consumed">If the method returns true it contains the consumed string</param>
        /// <returns>wether the tracker matches</returns>
        protected abstract bool MatchesValue(ITracker tracker, out string consumed);
    }
}
