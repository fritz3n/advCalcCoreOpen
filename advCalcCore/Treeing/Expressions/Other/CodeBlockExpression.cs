using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Other
{
    class CodeBlockExpression : Expression
    {
        public override bool IsStatic
        {
            get
            {
                foreach (Expression exp in Instructions)
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
                foreach (Expression p in Instructions)
                    p.Identifiers = value;
                base.Identifiers = value;
            }
        }
        public virtual List<Expression> Instructions { get; set; }
        protected override IEnumerable<Expression> AllParameters => Instructions;

        public override string Name => "Code Block";

        public CodeBlockExpression() { }

        public CodeBlockExpression(List<Expression> instructions)
        {
            Instructions = instructions;
        }

        protected override Value GetValueInternal(bool execute = true)
        {
            Callstack.ResetCallStackFlags();
            foreach (Expression instruction in Instructions)
            {
                instruction.GetValue(Callstack, execute);
                if ((Callstack.Flags & CallStack.ReturnFlags.Return) != 0)
                {
                    return Callstack.Result;
                }
                else if ((Callstack.Flags & CallStack.ReturnFlags.Break) != 0 ||
                        (Callstack.Flags & CallStack.ReturnFlags.Continue) != 0)
                {
                    return NullValue.Null;
                }
            }
            return NullValue.Null;
        }

        public override string ToString() => "{" + string.Join(";\n", Instructions) + "}";
    }
}
