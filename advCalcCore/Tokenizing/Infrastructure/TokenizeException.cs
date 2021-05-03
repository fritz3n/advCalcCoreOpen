using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using System;

namespace advCalcCore.Tokenizing
{
    public class TokenizeException : Exception
    {
        private readonly string String;
        private readonly TextRegion Region;
        private readonly Token LastToken;
        private readonly string ExceptionMessage;

        public TokenizeException(string s, TextRegion region, Token lastToken, string message) : base("Error after: '" + lastToken + "' and near:\n" + new string(' ', region.Start) + new string('v', region.End - region.Start) + "\n" + s + "\n\n" + message)
        {
            String = s;
            Region = region;
            LastToken = lastToken;
            ExceptionMessage = message;
        }

        public override string ToString() => "Error after: '" + LastToken + "' and near:\n" + new string(' ', Region.Start) + new string('v', Region.End - Region.Start) + "\n" + String + "\n\n" + ExceptionMessage;
    }
}
