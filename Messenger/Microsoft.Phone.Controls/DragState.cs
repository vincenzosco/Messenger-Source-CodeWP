using System;

namespace Microsoft.Phone.Controls
{
	internal class DragState
	{
		public double MaxDraggingBoundary { get; set; }

		public double MinDraggingBoundary { get; set; }

		public bool GotDragDelta { get; set; }

		public bool IsDraggingFirstElement { get; set; }

		public bool IsDraggingLastElement { get; set; }

		public DateTime LastDragUpdateTime { get; set; }

		public double DragStartingMediaStripOffset { get; set; }

		public double NetDragDistanceSincleLastDragStagnation { get; set; }

		public double LastDragDistanceDelta { get; set; }

		public int NewDisplayedElementIndex { get; set; }

		public double UnsquishTranslationAnimationTarget { get; set; }

		public DragState(double maxDraggingBoundary)
		{
			MaxDraggingBoundary = maxDraggingBoundary;
		}
	}
}
