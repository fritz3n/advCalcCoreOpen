using advCalcCore.Tokenizing.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Callstack
{
    public struct RegionInfo
    {
        public TextRegion TextRegion { get; set; }
        public TextRegion? ContextTextRegion { get; set; }

        public RegionInfo(TextRegion textRegion, TextRegion? contextTextRegion)
        {
            TextRegion = textRegion;
            ContextTextRegion = contextTextRegion;
        }
    }
}
