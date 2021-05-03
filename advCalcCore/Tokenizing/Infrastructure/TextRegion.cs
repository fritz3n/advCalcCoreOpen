using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Tokenizing.Infrastructure
{
    public struct TextRegion
    {
        public int Start { get; set; }
        public int End { get; set; }

        public TextRegion(int start, int end)
        {
            if (start > end)
                throw new ArgumentException();

            Start = start;
            End = end;
        }

        public TextRegion(int postition)
        {
            Start = postition;
            End = postition + 1;
        }

        public string Apply(string s) => s.Substring(Start, End - Start);

        public override string ToString() => Start + " - " + End;
    }
}
