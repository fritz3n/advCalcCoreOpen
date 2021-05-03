using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using System;
using System.Collections.Generic;

namespace advCalcCore.Tokenizing.Tracker
{
	public interface ITracker
	{
		/// <summary>
		/// The string remaining in this Tracker instance
		/// </summary>
		public string Remaining { get; }

		/// <summary>
		/// The Last Token added to the tracker, or null if there are none
		/// </summary>
		public Token Last { get; }

		/// <summary>
		/// Returns the last Token to be added before the tracker was created or null, if there are none 
		/// </summary>
		public Token Before { get; }

		public int InitialIndex { get; }
		public int StopIndex { get; }

		public int Index { get; }

		public string Raw { get; }

		public bool HasErrors { get; }

		public IEnumerable<TokenizeError> Errors { get; }
		public IEnumerable<Token> Tokens { get; }
		public IEnumerable<Token> TokensFrom(Guid resetPoint);

		public Guid lastResetPoint { get; }

		public Guid AddToken(Token token, int consume = 0);

		/// <summary>
		/// Add an Error including the reason why the tokenization failed.
		/// </summary>
		/// <param name="message">The message of the error</param>
		/// <param name="specificity">How specific the current error is. Start at 1</param>
		/// <param name="region">If set the region in which the error occurred. Else the already consumed string.</param>
		public void AddError(string message, int specificity = 0, TextRegion? region = null);

		/// <summary>
		/// Peeks the next character of the remaining string but doesn´t consume it
		/// </summary>
		/// <returns>The next character of the remaining string, or null if the end is reached</returns>
		public char? Read();

		public bool Read(out char character);

		/// <summary>
		/// Peeks a string of length "length" from the remaining string but doesn´t peek it.
		/// Returns null if length exceeds the remaining string
		/// </summary>
		/// <param name="length">The length of the string to be peeked</param>
		/// <returns>The peeked string or null if length exceeds the remaining string</returns>
		public string Read(int length);

		/// <summary>
		/// Peeks a string of length "length" from the remaining string but doesn´t consume it.
		/// Returns false if length exceeds the remaining string, otherwise true
		/// </summary>
		/// <param name="length">The length of the string to be peeked</param>
		/// <param name="s">Contains the peeked string, or "" if length exceeds the remaining string</param>
		/// <returns>True, if it succeded, false if not</returns>
		public bool Read(int length, out string s);

		public bool ReadWithOffset(int offset, out char character);

		public string Consume(int length);
		string ConsumeUntil(int index);
		Guid GetResetPoint();
		bool Reset(Guid resetPoint);
		bool ResetErrorsToLastResetPoint(Guid resetPoint);
	}
}
