using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
    class IntExpression : Expression
    {
        public override string Name => "Integer";
        public override bool IsStatic => true;
        protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();
        public int Number { get; set; }

        protected override Value GetValueInternal(bool execute = true) => new IntValue(Number);
        public override string ToString() => Number.ToString();
    }
}
