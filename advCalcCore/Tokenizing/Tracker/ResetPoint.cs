using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Tokenizing.Tracker
{
	/// <summary>
	/// Accepts an <see cref="ITracker"/> and creates a resetpoint when constructed.
	/// If it is not disabled beforehand and Disposed, it Resets the tracker.
	/// Can only reset the tracker once.
	/// </summary>
	class ResetPoint : IDisposable
	{
		private readonly ITracker tracker;
		public Guid resetPoint { get; }
		private bool reset = false;

		public bool Disabled { get; set; } = false;

		public ResetPoint(ITracker tracker)
		{
			this.tracker = tracker;
			resetPoint = tracker.GetResetPoint();
		}

		public void Disable() => Disabled = true;

		public void Dispose()
		{
			if (!Disabled && !reset)
				tracker.Reset(resetPoint);
			reset = true;
		}
	}
}
