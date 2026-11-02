using System.Collections.Generic;
using System.Linq;

namespace Facebook.Models
{
	public class Contact : IAboutType
	{
		public List<string> Emails { get; set; }

		public List<string> Websites { get; set; }

		public List<Phone> PhoneNumbers { get; set; }

		public Address Address { get; set; }

		public AboutType Type
		{
			get
			{
				return AboutType.Contact;
			}
		}

		public bool IsEmpty
		{
			get
			{
				if (!Emails.Any() && !Websites.Any() && !PhoneNumbers.Any())
				{
					return Address == null;
				}
				return false;
			}
		}
	}
}
