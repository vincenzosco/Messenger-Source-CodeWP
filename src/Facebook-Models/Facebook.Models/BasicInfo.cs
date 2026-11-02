namespace Facebook.Models
{
	public class BasicInfo : IAboutType
	{
		public string Birthdate { get; set; }

		public string InterestedIn { get; set; }

		public string Gender { get; set; }

		public string Language { get; set; }

		public AboutType Type
		{
			get
			{
				return AboutType.BasicInfo;
			}
		}

		public bool IsEmpty
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Birthdate) && string.IsNullOrWhiteSpace(InterestedIn) && string.IsNullOrWhiteSpace(Gender))
				{
					return string.IsNullOrWhiteSpace(Language);
				}
				return false;
			}
		}
	}
}
