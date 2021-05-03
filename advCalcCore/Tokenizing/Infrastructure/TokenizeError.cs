using advCalcCore.Tokenizing.Tokenizers;

namespace advCalcCore.Tokenizing.Infrastructure
{
    public struct TokenizeError
    {
        public int Specificity { get; set; }
        public string Message { get; set; }
        public TextRegion Region { get; set; }
        public ITokenizer Tokenizer { get; set; }

        public TokenizeError(int specificity, string message, TextRegion region, ITokenizer tokenizer)
        {
            Specificity = specificity;
            Message = message;
            Region = region;
            Tokenizer = tokenizer;
        }

        public override string ToString() => $"{Specificity}: '{Message}' from '{Tokenizer.GetType()}'";
    }
}
