using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Facebook.Models.PhoneContacts;
using Microsoft.Phone.UserData;

namespace Facebook.Messenger.WP8.Utilities.PhoneContacts
{
	public class PhoneContactUtils
	{
		private const int MinServerAcceptedBirthYear = 1801;

		public static FBPhoneContact CreatePhoneContact(Contact phoneContact)
		{
			FBPhoneContact fBPhoneContact = new FBPhoneContact();
			fBPhoneContact.Name = FBContactNameFromPhoneContact(phoneContact);
			fBPhoneContact.Birthday = FBContactBirthdayFromPhoneContact(phoneContact);
			fBPhoneContact.Phones = FBPhoneNumbersFromPhoneContact(phoneContact);
			fBPhoneContact.Emails = FBEmailAddressesFromPhoneContact(phoneContact);
			fBPhoneContact.ContactId = ((object)phoneContact).GetHashCode().ToString(CultureInfo.InvariantCulture);
			return fBPhoneContact;
		}

		private static FBContactName FBContactNameFromPhoneContact(Contact contact)
		{
			FBContactName fBContactName = new FBContactName();
			if (contact.CompleteName != null)
			{
				fBContactName.FirstName = contact.CompleteName.FirstName;
				fBContactName.MiddleName = contact.CompleteName.MiddleName;
				fBContactName.LastName = contact.CompleteName.LastName;
			}
			fBContactName.FormattedName = contact.DisplayName;
			return fBContactName;
		}

		private static FBContactBirthday FBContactBirthdayFromPhoneContact(Contact phoneContact)
		{
			if (phoneContact.Birthdays == null || !phoneContact.Birthdays.Any())
			{
				return null;
			}
			DateTime dateTime = phoneContact.Birthdays.FirstOrDefault();
			FBContactBirthday fBContactBirthday = new FBContactBirthday();
			fBContactBirthday.Year = ((dateTime.Year >= 1801) ? dateTime.ToString("yyyy", CultureInfo.InvariantCulture) : null);
			fBContactBirthday.Month = dateTime.ToString("MM", CultureInfo.InvariantCulture);
			fBContactBirthday.Day = dateTime.ToString("dd", CultureInfo.InvariantCulture);
			return fBContactBirthday;
		}

		private static List<FBContactEmail> FBEmailAddressesFromPhoneContact(Contact phoneContact)
		{
			return phoneContact.EmailAddresses.Select(FBContactEmailFromEmailAddress).ToList();
		}

		private static FBContactEmail FBContactEmailFromEmailAddress(ContactEmailAddress phoneEmailAddress)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			FBContactEmail fBContactEmail = new FBContactEmail();
			fBContactEmail.Email = phoneEmailAddress.EmailAddress;
			fBContactEmail.Type = GetFBEmailAddressType(phoneEmailAddress.Kind);
			FBContactEmail fBContactEmail2 = fBContactEmail;
			Account val = phoneEmailAddress.Accounts.FirstOrDefault();
			if (val != null)
			{
				fBContactEmail2.Label = val.Name;
			}
			return fBContactEmail2;
		}

		private static string GetFBEmailAddressType(EmailAddressKind kind)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected I4, but got Unknown
			switch ((int)kind)
			{
			case 0:
				return "home";
			case 1:
				return "work";
			case 2:
				return "other";
			default:
				return "null";
			}
		}

		private static List<FBContactPhoneNumber> FBPhoneNumbersFromPhoneContact(Contact phoneContact)
		{
			return phoneContact.PhoneNumbers.Select(FBContactPhoneNumberFromPhoneNumber).ToList();
		}

		private static FBContactPhoneNumber FBContactPhoneNumberFromPhoneNumber(ContactPhoneNumber phoneNumber)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			FBContactPhoneNumber fBContactPhoneNumber = new FBContactPhoneNumber();
			fBContactPhoneNumber.PhoneNumber = phoneNumber.PhoneNumber;
			fBContactPhoneNumber.Type = GetFBPhoneNumberType(phoneNumber.Kind);
			FBContactPhoneNumber fBContactPhoneNumber2 = fBContactPhoneNumber;
			Account val = phoneNumber.Accounts.FirstOrDefault();
			if (val != null)
			{
				fBContactPhoneNumber2.Label = val.Name;
			}
			return fBContactPhoneNumber2;
		}

		private static string GetFBPhoneNumberType(PhoneNumberKind kind)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected I4, but got Unknown
			switch ((int)kind)
			{
			case 1:
				return "home";
			case 5:
				return "fax_home";
			case 0:
				return "mobile";
			case 4:
				return "pager";
			case 6:
				return "fax_work";
			case 2:
			case 3:
				return "work";
			default:
				return "null";
			}
		}
	}
}
