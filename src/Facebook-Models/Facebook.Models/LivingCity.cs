using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class LivingCity : IAboutType
	{
		public enum LivingCityType
		{
			[EnumMember(Value = "Current City")]
			CurrentCity,
			[EnumMember(Value = "Hometown")]
			Hometown
		}

		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		public LivingCityType CityType { get; set; }

		public AboutType Type
		{
			get
			{
				return AboutType.Living;
			}
		}
	}
}
