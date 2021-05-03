using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
	public abstract class Expression
	{
		private Value Cache = null;

		public Value This { get; set; } = null;
		public abstract bool IsStatic { get; }
		protected abstract Value GetValueInternal(bool execute = true);
		public virtual IdentifierStore Identifiers { get; set; }
		public TextRegion TextRegion { get; set; }
		public TextRegion? ContextTextRegion { get; set; } = null;
		public abstract string Name { get; }
		protected CallStack Callstack { get; private set; }
		public string OriginalText { get; set; } = null;

		protected abstract IEnumerable<Expression> AllParameters { get; }

		public Value GetValue(CallStack callstack = null, bool execute = true)
		{

			Callstack = callstack ?? new CallStack();
			if (IsStatic)
			{
				if (execute)
				{
					PushStackFrame(Callstack);
					Cache = GetValueCatch(true);
					PopStackFrame(Callstack);
					return Cache;
				}
				else if (Cache is null)
				{
					PushStackFrame(Callstack);
					Cache = GetValueCatch(false);
					PopStackFrame(Callstack);
					return Cache;
				}
				else
				{
					return Cache;
				}

			}

			PushStackFrame(Callstack);
			Cache = GetValueCatch(execute);
			PopStackFrame(Callstack);
			return Cache;
		}

		private Value GetValueCatch(bool execute)
		{
			try
			{
				return GetValueInternal(execute);
			}
			catch (ExpressionException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new ExpressionException(Callstack, "Unspecified ExpressionException. See InnerException: " + e.Message, e);
			}
		}

		protected void PushStackFrame(CallStack callstack) => callstack.PushStackFrame(Name, ToString(), TextRegion, ContextTextRegion, OriginalText, This);
		protected void PopStackFrame(CallStack callstack) => callstack.PopStackFrame();

		/// <summary>
		/// Decouples the OriginalText and TextRegion parameters from it´s Perant and rebases all TextRegions
		/// </summary>
		/// <param name="expression">The expression to rebase</param>
		/// <param name="callstack">The current Callstack, for retrieving the originalText</param>
		public static void Rebase(Expression expression, CallStack callstack, string name = null)
		{
			string originalText = callstack.GetLocalText();
			Rebase(expression, originalText, name);
		}
		/// <summary>
		/// Decouples the OriginalText and TextRegion parameters from it´s Parent and rebases all TextRegions
		/// </summary>
		/// <param name="expression">The expression to rebase</param>
		/// <param name="originalText">The orignal text, on which the TextRegions are based</param>
		public static void Rebase(Expression expression, string originalText, string name = null)
		{
			if (originalText == null)
				return;
			int BaseOffset = (expression.ContextTextRegion ?? expression.TextRegion).Start;

			expression.OriginalText = (expression.ContextTextRegion ?? expression.TextRegion).Apply(originalText);

			if (!string.IsNullOrWhiteSpace(name))
				expression.OriginalText += " in " + name;

			void RebaseAll(Expression parent)
			{
				foreach (Expression child in parent.AllParameters)
				{
					if (child is null)
						continue;
					child.TextRegion = new TextRegion(child.TextRegion.Start - BaseOffset, child.TextRegion.End - BaseOffset);
					if (child.ContextTextRegion is TextRegion contextRegion)
						child.ContextTextRegion = new TextRegion(contextRegion.Start - BaseOffset, contextRegion.End - BaseOffset);
					RebaseAll(child);
				}
			}

			expression.TextRegion = new TextRegion(expression.TextRegion.Start - BaseOffset, expression.TextRegion.End - BaseOffset);
			if (expression.ContextTextRegion is TextRegion contextRegion)
				expression.ContextTextRegion = new TextRegion(contextRegion.Start - BaseOffset, contextRegion.End - BaseOffset);
			RebaseAll(expression);
		}
	}
}
