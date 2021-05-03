using advCalcCore.Treeing.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
    abstract class UnaryExpression : Expression
    {
        public override bool IsStatic => Parameter.IsStatic;
        public override IdentifierStore Identifiers
        {
            get => base.Identifiers;
            set
            {
                Parameter.Identifiers = value;
                base.Identifiers = value;
            }
        }
        protected override IEnumerable<Expression> AllParameters => new Expression[] { Parameter };
        public virtual Expression Parameter { get; set; }

    }
}
