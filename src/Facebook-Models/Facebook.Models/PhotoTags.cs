using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class PhotoTags
	{
		[DataMember]
		[JsonProperty("edges")]
		public ObservableCollection<PhotoTagEdge> PhotoTagEdges { get; set; }
	}
}
