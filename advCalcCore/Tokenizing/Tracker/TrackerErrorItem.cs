using advCalcCore.Tokenizing.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Tokenizing.Tracker
{
	struct TrackerErrorItem
	{
		/// <summary>
		/// Means That this is a symbolic marker for the start of another ResetPoint. 
		/// If this is true, Error is expected to be <see cref="default(TokenizeError)"/> and should be disregarded.
		/// </summary>
		public bool IsMarker { get; }
		public TokenizeError Error { get; }
		public Guid ResetPoint { get; }

		public TrackerErrorItem(TokenizeError error, Guid resetPoint)
		{
			Error = error;
			ResetPoint = resetPoint;
			IsMarker = false;
		}

		public TrackerErrorItem(Guid resetPoint, bool isMarker)
		{
			ResetPoint = resetPoint;
			IsMarker = isMarker;
			Error = default;
		}
	}
}
