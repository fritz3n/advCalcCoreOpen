using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Callstack
{
	public class CallStack
	{
		private Stack<StackFrame> callstack = new Stack<StackFrame>(100);

		public IReadOnlyList<StackFrame> StackTrace => callstack.ToList();

		public Value Result { get; set; } = null;
		public ReturnFlags Flags { get; set; } = ReturnFlags.None;

		public bool TryGetResult(out Value value)
		{
			value = Result;
			if (value is null)
				return false;
			return true;
		}
		public void PushStackFrame(StackFrame stackFrame) => callstack.Push(stackFrame);
		public void PushStackFrame(string name, string text, TextRegion textRegion, TextRegion? contextTextRegion, string originalText = null, Value @this = null)
		{
			if (callstack.Count > 1024)
				throw new ExpressionException(this, "Too many Stackframes.");
			callstack.Push(new StackFrame() { Name = name, Text = text, OriginalText = originalText, RegionInfo = new RegionInfo(textRegion, contextTextRegion), This = @this });
		}

		public StackFrame PopStackFrame() => callstack.Pop();
		public override string ToString() => string.Join("> ", StackTrace.Reverse());

		public Value GetThis()
		{
			return callstack.FirstOrDefault(f => f.This is not null).This; // returns null if not found because default(StackFrame).This == null
		}

		/// <summary>
		/// Resets the CallStacks Flags and Result Value
		/// </summary>
		public void ResetCallStackFlags()
		{
			Result = null;
			Flags = ReturnFlags.None;
		}

		public string GetLocalText()
		{
			string originalText = null;
			foreach (StackFrame stackFrame in callstack)
			{
				if (stackFrame.OriginalText != null)
				{
					originalText = stackFrame.OriginalText;
					break;
				}
			}
			return originalText;
		}

		public void OutputStackposition(string originalText = null)
		{
			originalText = originalText ?? GetLocalText();

			if (originalText == null)
			{
				Console.WriteLine("Can´t output Stackposition. No original Text found.");
			}

			StackFrame currentFrame = callstack.Peek();

			if (currentFrame.RegionInfo.ContextTextRegion is TextRegion contextRegion)
			{
				TextRegion region = currentFrame.RegionInfo.TextRegion;

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(originalText.Substring(0, contextRegion.Start));

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(originalText.Substring(contextRegion.Start, region.Start - contextRegion.Start));

				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write(region.Apply(originalText));

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(originalText.Substring(region.End, contextRegion.End - region.End));

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(originalText.Substring(contextRegion.End));
			}
			else
			{
				TextRegion region = currentFrame.RegionInfo.TextRegion;

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(originalText.Substring(0, region.Start));

				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write(region.Apply(originalText));

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(originalText.Substring(region.End));
			}
		}
		[Flags]
		public enum ReturnFlags
		{
			None = 0b0,
			Return = 0b1,
			Break = 0b10,
			Continue = 0b100,
		}
	}
}
