using System;

namespace Facebook.Services.Api
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class QueueAttribute : Attribute
	{
		public QueueType QueueType { get; set; }

		public QueueAttribute(QueueType type)
		{
			QueueType = type;
		}
	}
}
