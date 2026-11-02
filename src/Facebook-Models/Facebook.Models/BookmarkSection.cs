using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class BookmarkSection
	{
		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "header")]
		[DataMember]
		public string Header { get; set; }

		[JsonProperty(PropertyName = "visible")]
		[DataMember]
		public virtual List<Bookmark> Visible { get; set; }

		[JsonProperty(PropertyName = "all")]
		[DataMember]
		public List<Bookmark> All { get; set; }

		public bool ShouldShow { get; set; }

		public BookmarkSection()
		{
			Visible = new List<Bookmark>();
			All = new List<Bookmark>();
			ShouldShow = true;
		}

		protected BookmarkSection(string id, string name, string header, List<Bookmark> visible, List<Bookmark> all, bool shouldShow)
		{
			Id = id;
			Name = name;
			Header = header;
			Visible = visible;
			All = all;
			ShouldShow = shouldShow;
		}
	}
}
