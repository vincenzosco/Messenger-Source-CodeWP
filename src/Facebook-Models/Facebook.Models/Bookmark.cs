using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class Bookmark
	{
		private sealed class IdEqualityComparer : IEqualityComparer<Bookmark>
		{
			public bool Equals(Bookmark x, Bookmark y)
			{
				if (object.ReferenceEquals(x, y))
				{
					return true;
				}
				if (object.ReferenceEquals(x, null))
				{
					return false;
				}
				if (object.ReferenceEquals(y, null))
				{
					return false;
				}
				if ((object)x.GetType() != y.GetType())
				{
					return false;
				}
				return string.Equals(x.Id, y.Id);
			}

			public int GetHashCode(Bookmark obj)
			{
				if (obj.Id == null)
				{
					return 0;
				}
				return obj.Id.GetHashCode();
			}
		}

		private static readonly IEqualityComparer<Bookmark> IdComparerInstance = new IdEqualityComparer();

		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "url")]
		public string Url { get; set; }

		[JsonProperty(PropertyName = "count")]
		[DataMember]
		public int Count { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "pic")]
		public string Picture { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "icon")]
		public string Icon { get; set; }

		[JsonProperty(PropertyName = "type")]
		[DataMember]
		public string Type { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "is_local")]
		public bool IsLocal { get; set; }

		public static IEqualityComparer<Bookmark> IdComparer
		{
			get
			{
				return IdComparerInstance;
			}
		}
	}
}
