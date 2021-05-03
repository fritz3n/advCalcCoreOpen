using advCalcCore.Treeing.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
    abstract class MultiparamExpression : Expression
    {
        /// <summary>
        /// Default Implementation returns true, if all Expressions in <see cref="Parameters"></see> are return true.
        /// </summary>
        public override bool IsStatic
        {
            get
            {
                foreach (Expression exp in Parameters)
                {
                    if (!exp.IsStatic)
                        return false;
                }

                return true;
            }
        }
        public override IdentifierStore Identifiers
        {
            get => base.Identifiers;
            set
            {
                foreach (Expression p in Parameters)
                    p.Identifiers = value;
                base.Identifiers = value;
            }
        }

        protected override IEnumerable<Expression> AllParameters => Parameters;
        public virtual List<Expression> Parameters { get; set; }

    }
}
