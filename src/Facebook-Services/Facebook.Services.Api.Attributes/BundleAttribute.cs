using System;

namespace Facebook.Services.Api.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class BundleAttribute : Attribute
	{
		public BundleType BundleType { get; private set; }

		public Type RequestType { get; private set; }

		public Type ResponseType { get; private set; }

		public BundleAttribute(BundleType bundleType, Type request, Type response)
		{
			BundleType = bundleType;
			RequestType = request;
			ResponseType = response;
		}
	}
}
