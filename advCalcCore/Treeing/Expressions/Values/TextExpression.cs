using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
	class TextExpression : Expression
	{
		// TODO just copy pasta -> check & test it
		public override string Name => "Text";
		public override bool IsStatic => true;
		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();
		public string Text { get; set; }

		protected override Value GetValueInternal(bool execute = true) => new TextValue(Text);
		public override string ToString() => '"' + Text + '"';
	}
}
