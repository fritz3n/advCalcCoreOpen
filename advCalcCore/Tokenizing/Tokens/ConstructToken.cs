using advCalcCore.Tokenizing.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Tokenizing.Tokens
{
	public class ConstructToken : Token
	{
		private readonly List<Token> tokens;
		public int Length { get; } = 0;

		public IReadOnlyCollection<Token> Tokens => tokens.AsReadOnly();

		public Token this[string name] => tokens.SingleOrDefault(t => t.Name == name);

		public ConstructToken(TextRegion range, List<Token> tokens, string name, TokenType type = TokenType.Value) : base(range, string.Join("", tokens), name, type)
		{
			this.tokens = tokens;
		}

		public override string ToString() => Text;
	}
}
