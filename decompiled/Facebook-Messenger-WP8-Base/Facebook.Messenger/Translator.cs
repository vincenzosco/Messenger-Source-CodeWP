using System.Collections.Generic;
using System.Globalization;
using Facebook.Base;

namespace Facebook.Messenger
{
	public class Translator : ITranslator
	{
		private const string DefaultFBLocale = "en_US";

		private static readonly Dictionary<string, string> RegionMapper = new Dictionary<string, string>
		{
			{ "es_ES", "es_ES" },
			{ "fr_CA", "fr_CA" },
			{ "pt_PT", "pt_PT" },
			{ "zh_TW", "zh_TW" },
			{ "zh_HK", "zh_HK" }
		};

		private static readonly Dictionary<string, string> LanguageMapper = new Dictionary<string, string>
		{
			{ "af", "af_ZA" },
			{ "ar", "ar_AR" },
			{ "az", "az_AZ" },
			{ "be", "be_BY" },
			{ "bg", "bg_BG" },
			{ "bn", "bn_IN" },
			{ "bs", "bs_BA" },
			{ "ca", "ca_ES" },
			{ "ck", "ck_US" },
			{ "cs", "cs_CZ" },
			{ "cy", "cy_GB" },
			{ "da", "da_DK" },
			{ "de", "de_DE" },
			{ "el", "el_GR" },
			{ "eo", "eo_EO" },
			{ "et", "et_EE" },
			{ "es", "es_LA" },
			{ "eu", "eu_ES" },
			{ "fa", "fa_IR" },
			{ "fi", "fi_FI" },
			{ "fo", "fo_FO" },
			{ "fr", "fr_FR" },
			{ "fy", "fy_NL" },
			{ "ga", "ga_IE" },
			{ "gl", "gl_ES" },
			{ "he", "he_IL" },
			{ "hi", "hi_IN" },
			{ "hr", "hr_HR" },
			{ "hu", "hu_HU" },
			{ "hy", "hy_AM" },
			{ "id", "id_ID" },
			{ "in", "id_ID" },
			{ "is", "is_IS" },
			{ "it", "it_IT" },
			{ "iw", "he_IL" },
			{ "ja", "ja_JP" },
			{ "ka", "ka_GE" },
			{ "km", "km_KH" },
			{ "ko", "ko_KR" },
			{ "ku", "ku_TR" },
			{ "la", "la_VA" },
			{ "lv", "lv_LV" },
			{ "mk", "mk_MK" },
			{ "ml", "ml_IN" },
			{ "ms", "ms_MY" },
			{ "nb", "nb_NO" },
			{ "ne", "ne_NP" },
			{ "nl", "nl_NL" },
			{ "nn", "nn_NO" },
			{ "pa", "pa_IN" },
			{ "pl", "pl_PL" },
			{ "ps", "ps_AF" },
			{ "pt", "pt_BR" },
			{ "ro", "ro_RO" },
			{ "ru", "ru_RU" },
			{ "sk", "sk_SK" },
			{ "sl", "sl_SI" },
			{ "sq", "sq_AL" },
			{ "sr", "sr_RS" },
			{ "sv", "sv_SE" },
			{ "sw", "sw_KE" },
			{ "ta", "ta_IN" },
			{ "th", "th_TH" },
			{ "tl", "tl_PH" },
			{ "tr", "tr_TR" },
			{ "uk", "uk_UA" },
			{ "vi", "vi_VN" },
			{ "zh", "zh_CN" }
		};

		public string GetCurrentFBLocale()
		{
			string key = CultureInfo.CurrentUICulture.Name.Replace('-', '_');
			string twoLetterISOLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
			string result = "en_US";
			if (RegionMapper.ContainsKey(key))
			{
				result = RegionMapper[key];
			}
			else if (LanguageMapper.ContainsKey(twoLetterISOLanguageName))
			{
				result = LanguageMapper[twoLetterISOLanguageName];
			}
			return result;
		}
	}
}
