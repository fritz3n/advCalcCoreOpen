using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
    class SimpleBracketExpression : UnaryExpression
    {
        public override string Name => "Brackets";

        public SimpleBracketExpression(Expression content = null)
        {
            Parameter = content;
        }


        protected override Value GetValueInternal(bool execute = true) => Parameter.GetValue(Callstack, execute);

        public override string ToString() => $"({Parameter})";
    }
}
