using advCalcCore.Tokenizing.Tokens;
using System;

namespace advCalcCore.Tokenizing.Tracker
{
	struct TrackerItem
	{
		public Token Token { get; }
		public int StartIndex { get; }
		public int NextIndex { get; }
		public Guid ResetPoint { get; }

		public TrackerItem(Token token, int startIndex, int nextIndex, Guid resetPoint)
		{
			Token = token;
			StartIndex = startIndex;
			NextIndex = nextIndex;
			ResetPoint = resetPoint;
		}
	}
}
