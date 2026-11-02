using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class ActionCount : Notifiable
	{
		private int? _count = 0;

		[JsonProperty(PropertyName = "count")]
		[DataMember]
		public int? Count
		{
			get
			{
				return _count;
			}
			set
			{
				_count = value;
				OnPropertyChanged("Count");
				OnPropertyChanged("DisplayCount");
			}
		}

		public string DisplayCount
		{
			get
			{
				int num = Count ?? 0;
				if (num >= 1000000)
				{
					double num2 = (double)num / 1000000.0;
					return string.Format("{0}M", new object[1] { num2.ToString("0.#") });
				}
				if (num >= 1000)
				{
					double num3 = (double)num / 1000.0;
					return string.Format("{0}K", new object[1] { num3.ToString("0.#") });
				}
				return num.ToString();
			}
		}
	}
}
