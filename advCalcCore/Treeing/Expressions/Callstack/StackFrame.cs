using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Callstack
{
	public struct StackFrame
	{
		public string Name { get; set; }
		public string Text { get; set; }
		public string OriginalText { get; set; }
		public RegionInfo RegionInfo { get; set; }
		/// <summary>
		/// If set, a reference to a value representing the 'this' keyword in this CallStack.
		/// </summary>
		public Value This { get; set; }
		public override string ToString() => Name + $" {{{Text}}}";
	}
}
