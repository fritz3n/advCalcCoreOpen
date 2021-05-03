using advCalcCore.Tokenizing.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Tokenizing.Tokens
{
    public class Token
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public TokenType Type { get; set; }
        public TextRegion Range { get; set; }
        public Token(TextRegion range, string text, string name = null, TokenType type = TokenType.None)
        {
            Range = range;
            Text = text;
            Name = name;
            Type = type;
        }

        public override string ToString() => Text;

        [Flags]
        public enum TokenType
        {
            NA = 0,
            None = 1,
            Operator = 2,
            Value = 4,
        }
    }
}