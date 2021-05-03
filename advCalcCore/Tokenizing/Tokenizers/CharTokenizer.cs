using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Tokenizing.Tracker;
using System;

namespace advCalcCore.Tokenizing.Tokenizers
{
    class CharTokenizer : GeneralTokenizer
    {
        public override Func<char, bool> Selector => c => c == Character;

        private readonly char Character;

        public CharTokenizer(char character, Token.TokenType type, Token.TokenType previousType = Token.TokenType.NA, params string[] previousNames) : this(character, type, character.ToString(), previousType, previousNames) { }

        public CharTokenizer(char character, Token.TokenType type, string name, Token.TokenType previousType = Token.TokenType.NA, params string[] previousNames) : base(type, name, previousType, previousNames)
        {
            Character = character;
        }

        protected override bool MatchesValue(ITracker tracker, out string consumed)
        {
            consumed = Character.ToString();
            return tracker.Read(out char c) ? c == Character : false;
        }
    }
}
