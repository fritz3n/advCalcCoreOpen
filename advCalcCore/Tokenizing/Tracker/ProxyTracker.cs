using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace advCalcCore.Tokenizing.Tracker
{
	class ProxyTracker : ITracker
	{
		public static Guid ZERO_RESETPOINT = new Guid("00000000000000000000000000000000");

		public int InitialIndex { get; } = 0;
		public int StopIndex { get; } = 0;
		public int Index { get; private set; } = 0;
		public string Raw { get; }

		List<TrackerItem> tokens = new List<TrackerItem>();
		List<TrackerErrorItem> errors = new List<TrackerErrorItem>();

		public bool HasErrors => errors.Count > 0;

		public IEnumerable<TokenizeError> Errors => errors.Where(e => !e.IsMarker).Select(e => e.Error);
		public IEnumerable<Token> Tokens => tokens.Where(t => t.Token != null).Select(t => t.Token);
		public IEnumerable<Token> TokensFrom(Guid resetPoint)
		{
			IEnumerable<TrackerItem> selected;

			if (resetPoint == ZERO_RESETPOINT)
				selected = tokens;
			else
			{
				selected = tokens.SkipWhile(t => t.ResetPoint != resetPoint)
				.Skip(1);// We only want tokens after the reset point; skip the reset point
			}

			return selected.Where(t => t.Token != null)
				.Select(t => t.Token).ToList();
		}

		public Guid lastResetPoint { get; private set; } = ZERO_RESETPOINT;

		public ProxyTracker(string raw, int indexOffset = 0, int stopBeforeIndex = 0)
		{
			Raw = raw;
			Index = indexOffset;
			InitialIndex = indexOffset;

			lastResetPoint = ZERO_RESETPOINT;

			if (stopBeforeIndex > indexOffset)
				StopIndex = stopBeforeIndex;
			else
				StopIndex = raw.Length;
		}

		public string Remaining => Raw.Substring(Index, StopIndex - Index);

		public Token Last => tokens.Count > 0 ? tokens.LastOrDefault(t => t.Token != null).Token : null;
		public Token Before => tokens.Count > 0 ? tokens.LastOrDefault(t => t.Token != null).Token : null;

		public void AddError(string message, int specificity = 0, TextRegion? region = null) => errors.Add(new TrackerErrorItem(new TokenizeError(specificity, message, region ?? new TextRegion(Index), null), lastResetPoint));

		public Guid AddToken(Token token, int consume = 0)
		{
			lastResetPoint = Guid.NewGuid();
			tokens.Add(new TrackerItem(token, Index, Index + consume, lastResetPoint));
			errors.Add(new TrackerErrorItem(lastResetPoint, true));
			Consume(consume);

			return lastResetPoint;
		}

		public Guid GetResetPoint()
		{
			return lastResetPoint;
		}

		/// <summary>
		/// Reset the Tracker to the given ResetPoint. All changes made after this point will be discarded.
		/// This means that any Tokens and Errors added after this point will be removed.
		/// </summary>
		/// <param name="resetPoint">The resetpoint. It is obtained using <see cref="GetResetPoint"/></param>
		/// <returns>Returns true if the operation completed succesfully.</returns>
		public bool Reset(Guid resetPoint)
		{
			// No action needed
			if (resetPoint == lastResetPoint)
				return true;

			// Resetpoint is BEFORE first token = clear everything.
			if (resetPoint == ZERO_RESETPOINT)
			{
				errors.Clear();
				tokens.Clear();

				lastResetPoint = ZERO_RESETPOINT;
				Index = InitialIndex;

				return true;
			}

			// Test if resetPoint exists
			int resetPointPosition = -1;
			for (int i = 0; i < tokens.Count; i++)
			{
				if (tokens[i].ResetPoint == resetPoint)
				{
					resetPointPosition = i;
					break;
				}
			}

			// The given resetPoint does not exist
			if (resetPointPosition == -1)
				return false;

			// ResetPoint is one of the other elements (between the first and last one)

			// Remove newer errors
			int errorIndex = errors.FindIndex(m => m.ResetPoint == resetPoint);
			if (errorIndex != -1)
				errors.RemoveRange(errorIndex + 1, errors.Count - errorIndex - 1);
			else // The errors have been cleared since resetPoint and all esisting errors were added after it. Clear all errors.
				errors.Clear();

			// Reset counter (for index and resetPoint)
			Index = tokens[resetPointPosition].NextIndex;
			lastResetPoint = tokens[resetPointPosition].ResetPoint;

			// Remove newer tokens
			tokens.RemoveRange(resetPointPosition + 1, tokens.Count - (resetPointPosition + 1));

			return true;
		}

		public bool ResetErrorsToLastResetPoint(Guid resetPoint)
		{
			// Verify that the caller has the valid resetPoint
			if (resetPoint != lastResetPoint)
				return false;

			// Remove newer errors
			int errorIndex = errors.FindIndex(m => m.ResetPoint == resetPoint);
			if (errorIndex != -1)
				errors.RemoveRange(errorIndex + 1, errors.Count - errorIndex - 1);
			else // The errors have been cleared since resetPoint and all esisting errors were added after it. Clear all errors.
				errors.Clear();

			return true;
		}

		#region Peek/Pop
		/// <summary>
		/// Peeks the next character of the remaining string but doesn´t consume it
		/// </summary>
		/// <returns>The next character of the remaining string, or null if the end is reached</returns>
		public char? Read()
		{
			if (Index + 1 > StopIndex)
				return null;

			return Raw[Index];
		}

		public bool Read(out char character)
		{
			if (Index >= StopIndex)
			{
				character = (char)0;
				return false;
			}

			character = Raw[Index];

			return true;
		}

		/// <summary>
		/// Peeks a string of length "length" from the remaining string but doesn´t peek it.
		/// Returns null if length exceeds the remaining string
		/// </summary>
		/// <param name="length">The length of the string to be peeked</param>
		/// <returns>The peeked string or null if length exceeds the remaining string</returns>
		public string Read(int length)
		{
			if (Index + length > StopIndex)
				return null;

			return Raw.Substring(Index, length);
		}

		/// <summary>
		/// Peeks a string of length "length" from the remaining string but doesn´t consume it.
		/// Returns false if length exceeds the remaining string, otherwise true
		/// </summary>
		/// <param name="length">The length of the string to be peeked</param>
		/// <param name="s">Contains the peeked string, or "" if length exceeds the remaining string</param>
		/// <returns>True, if it succeded, false if not</returns>
		public bool Read(int length, out string s)
		{
			if (Index + length > StopIndex)
			{
				s = "";
				return false;
			}

			s = Raw.Substring(Index, length);

			return true;
		}

		public bool ReadWithOffset(int offset, out char character)
		{
			if (Index + offset >= StopIndex)
			{
				character = (char)0;
				return false;
			}

			character = Raw[Index + offset];

			return true;
		}

		/// <summary>
		/// Peeks a string of length "length" from the remaining string and consumes it.
		/// Returns null if length exceeds the remaining string
		/// </summary>
		/// <param name="length">The length of the string to be peeked</param>
		/// <returns>The peeked string or null if length exceeds the remaining string</returns>
		public string Consume(int length)
		{
			if (length <= 0 || Index + length > StopIndex)
				return null;

			string s = Raw.Substring(Index, length);

			Index += length;

			return s;
		}

		/// <summary>
		/// Consume until an index is reached.
		/// </summary>
		/// <param name="index">The intende index. Must be >=<see cref="ITracker.Index"/> and smaller than <see cref="ITracker.StopIndex"/>.</param>
		/// <returns>The Consumned string.</returns>
		public string ConsumeUntil(int index)
		{
			if (index < Index || index > StopIndex)
				throw new ArgumentOutOfRangeException(nameof(index));
			return Consume(index - Index);
		}
		#endregion
	}
}
