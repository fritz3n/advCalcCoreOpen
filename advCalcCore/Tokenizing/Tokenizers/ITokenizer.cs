using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tracker;
using System;

namespace advCalcCore.Tokenizing.Tokenizers
{
    public interface ITokenizer
    {
        Func<char, bool> Selector { get; }
        TokenizerResult Tokenize(ITracker tracker);
    }
}
