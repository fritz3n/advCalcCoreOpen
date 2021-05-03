using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Tokenizing.Tokenizers
{
    class TextTokenizer : ITokenizer
    {
        public Func<char, bool> Selector => c => c == Symbol;

        public Token.TokenType PreviousType { get; }
        public Token.TokenType Type { get; }
        public string Name { get; }
        public char Symbol { get; }

        private const char escapeChar = '\\';

        public TextTokenizer(char symbol, Token.TokenType previousType = Token.TokenType.NA, Token.TokenType type = Token.TokenType.Value, string name = null)
        {
            Symbol = symbol;
            PreviousType = previousType;
            Type = type;
            Name = name;
        }

        public TokenizerResult Tokenize(ITracker tracker)
        {
            if (PreviousType != Token.TokenType.NA && ((tracker.Before?.Type ?? Token.TokenType.None) & PreviousType) == Token.TokenType.NA)
            {
                return TokenizerResult.Failure;
            }

            // TODO don't consume it here
            tracker.Consume(1); // First Char is guaranteed to be Symbol by Selector

            string text = "";
            bool escaped = false;

            while (true)
            {
                if (!tracker.Read(out char c))
                {
                    tracker.AddError("String not closed", 2, new TextRegion(tracker.InitialIndex));
                    tracker.Consume(1); // TODO don't consume it here
                    return TokenizerResult.Failure;
                }
                tracker.Consume(1); // TODO don't consume it here

                if (escaped)
                {
                    text += c;
                    escaped = false;
                    continue;
                }
                else if (c == Symbol)
                {
                    Token t = new Token(new TextRegion(tracker.InitialIndex, tracker.Index), text, Name, Type);

                    tracker.AddToken(t);

                    return TokenizerResult.Success;
                }
                else if (c == escapeChar)
                {
                    escaped = true;
                }
                else
                {
                    text += c;
                }
            }
        }
    }
}
