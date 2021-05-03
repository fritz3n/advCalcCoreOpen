using advCalcCore.Tokenizing.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Tokenizing.Tokens
{
	class RegexToken : Token
	{
		public RegexToken(TextRegion range, string text, string[] groups, string name = null, TokenType type = TokenType.None) : base(range, text, name, type)
		{
			Groups = groups;
		}

		public string[] Groups { get; }
	}
}
