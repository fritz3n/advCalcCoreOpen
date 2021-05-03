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
    class StringTokenizer : GeneralTokenizer
    {
        public override Func<char, bool> Selector => c => c == String[0];

        private readonly string String;

        public StringTokenizer(string token, Token.TokenType type, Token.TokenType previousType = Token.TokenType.NA, params string[] previousNames) : this(token, type, token, previousType, previousNames) { }

        public StringTokenizer(string token, Token.TokenType type, string name, Token.TokenType previousType = Token.TokenType.NA, params string[] previousNames) : base(type, name, previousType, previousNames)
        {
            String = token;
        }

        protected override bool MatchesValue(ITracker tracker, out string consumed)
        {
            return tracker.Read(String.Length, out consumed) ? consumed == String : false;
        }
    }
}
