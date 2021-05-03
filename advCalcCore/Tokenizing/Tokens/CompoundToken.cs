using advCalcCore.Tokenizing.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Tokenizing.Tokens
{
	public class CompoundToken : Token
	{
		private readonly List<Token> tokens;
		public char Opening { get; }
		public char Closing { get; }

		public IReadOnlyCollection<Token> Tokens => tokens.AsReadOnly();

		public CompoundToken(TextRegion range, List<Token> tokens, char opening, char closing, TokenType type = TokenType.Value, string name = null) : base(range, string.Join("", tokens), name ?? (opening.ToString() + closing), type)
		{
			this.tokens = tokens;
			Opening = opening;
			Closing = closing;
		}

		public override string ToString() => Opening + Text + Closing;
	}
}
