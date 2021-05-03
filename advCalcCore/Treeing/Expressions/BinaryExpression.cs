using advCalcCore.Treeing.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
    abstract class BinaryExpression : Expression
    {
        /// <summary>
        /// Default Implementation returns true if <code><see cref="Left"/> && <see cref="Right"/></code> returns true.
        /// </summary>
        public override bool IsStatic => Left.IsStatic && Right.IsStatic;
        public override IdentifierStore Identifiers
        {
            get => base.Identifiers;
            set
            {
                Left.Identifiers = value;
                Right.Identifiers = value;
                base.Identifiers = value;
            }
        }

        protected override IEnumerable<Expression> AllParameters => new Expression[] { Left, Right };

        public virtual Expression Left { get; set; }
        public virtual Expression Right { get; set; }
    }
}
