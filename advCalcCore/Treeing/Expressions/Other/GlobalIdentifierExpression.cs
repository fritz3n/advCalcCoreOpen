using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Other
{
    class GlobalIdentifierExpression : Expression, IAssignable
    {
        public override string Name => "Global Identifier";
        public string Identifier { get; set; }
        public override bool IsStatic => false;
        protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();
        public void Assign(Value value) => (Identifiers ?? IdentifierStore.Global)[Identifier, true] = value;
        protected override Value GetValueInternal(bool execute = true)
        {
            IdentifierStore store = (Identifiers ?? IdentifierStore.Global);
            if (!store.Contains(Identifier))
                throw new ExpressionException(Callstack, $"Identifier '{Identifier}' not found");

            return (Identifiers ?? IdentifierStore.Global)[Identifier, true];
        }

        public override string ToString() => Identifier;
    }
}
