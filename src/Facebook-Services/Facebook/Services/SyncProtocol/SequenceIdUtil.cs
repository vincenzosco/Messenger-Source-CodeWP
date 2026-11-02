using System.Collections.Generic;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol
{
	public static class SequenceIdUtil
	{
		public class SequenceAndDeltaInformation
		{
			public DeltaWrapper Delta;

			public long SequenceId;

			public long ExtraSequenceCount;

			public SequenceAndDeltaInformation(DeltaWrapper delta, long sequenceId)
			{
				Delta = delta;
				SequenceId = sequenceId;
				ExtraSequenceCount = GetExtraSequenceIdCount(delta);
			}

			private static int GetExtraSequenceIdCount(DeltaWrapper delta)
			{
				if (delta.GetDeltaType() != DeltaType.NOOP)
				{
					return 0;
				}
				DeltaNoOp deltaNoOp = delta.DeltaNoOp;
				int num = ((!deltaNoOp.__isset.numNoOps || deltaNoOp.NumNoOps <= 0) ? 1 : deltaNoOp.NumNoOps);
				return num - 1;
			}
		}

		public static bool IsValidSequenceId(long sequenceId)
		{
			return sequenceId > 0;
		}

		public static IEnumerable<SequenceAndDeltaInformation> CalculateDeltasSequenceIds(IEnumerable<DeltaWrapper> deltas, long firstDeltaSequenceId)
		{
			List<SequenceAndDeltaInformation> list = new List<SequenceAndDeltaInformation>();
			long num = firstDeltaSequenceId;
			foreach (DeltaWrapper delta in deltas)
			{
				SequenceAndDeltaInformation sequenceAndDeltaInformation = new SequenceAndDeltaInformation(delta, num);
				list.Add(sequenceAndDeltaInformation);
				num += 1 + sequenceAndDeltaInformation.ExtraSequenceCount;
			}
			return list;
		}
	}
}
