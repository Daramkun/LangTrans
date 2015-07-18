using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace LangTransRT
{
	/// <summary>
	/// Language Enumeration
	/// (Available on Google Translator)
	/// </summary>
	public enum Language
	{
		Unknown = 0x00,
		Galatia = 0x01,
		Gujarati = 0x02,
		Georgian = 0x03,
		Greek = 0x04,
		Dutch = 0x05,
		Norwegian = 0x06,
		Danish = 0x07,
		German = 0x08,
		Latvian = 0x09,
		Latin = 0x0A,
		Russian = 0x0B,
		Romanian = 0x0C,
		Lithuanian = 0x0D,
		Macedonian = 0x0E,
		Malay = 0x0F,
		Maltese = 0x10,
		Basque = 0x11,
		Vietnamese = 0x12,
		Belarusian = 0x13,
		Bengali = 0x14,
		Bulgarian = 0x15,
		Serbian = 0x16,
		Swahili = 0x17,
		Swedish = 0x18,
		Spanish = 0x19,
		Slovak = 0x1A,
		Slovenian = 0x1B,
		Arabic = 0x1C,
		Armenian = 0x1D,
		Icelandic = 0x1E,
		HaitianFrench = 0x1F,
		Irish = 0x20,
		Azerbaijani = 0x21,
		Afrikaans = 0x22,
		Albanian = 0x23,
		Estonian = 0x24,
		Esperanto = 0x25,
		English = 0x26,
		Urdu = 0x27,
		Ukrainian = 0x28,
		Weiljeueo = 0x29,
		Yiddish = 0x2A,
		Italian = 0x2B,
		Indonesian = 0x2C,
		Japanese = 0x2D,
		Chinese = 0x2E,
		Czech = 0x2F,
		Catalan = 0x30,
		Kannada = 0x31,
		Croatian = 0x32,
		Tagalog = 0x33,
		Tamil = 0x34,
		Thai = 0x35,
		Turkish = 0x36,
		Telugu = 0x37,
		Persian = 0x38,
		Portuguese = 0x39,
		Polish = 0x3A,
		French = 0x3B,
		Finnish = 0x3C,
		Korean = 0x3D,
		Hungarian = 0x3E,
		Hebrew = 0x3F,
		Hindi = 0x40,
	}

	/// <summary>
	/// Translator using Google Translator
	/// </summary>
	public static class Translator
	{
		static Action m_translateEnd;
		static string m_translated = "";
		static Dictionary<Language, string> m_langDics = new Dictionary<Language, string> ();

		public static Action TranslateEnded
		{
			get { return m_translateEnd; }
			set { m_translateEnd = value; }
		}

		/// <summary>
		/// Translated text
		/// </summary>
		public static string Translated { get { return m_translated; } }

		/// <summary>
		/// Translate using Google Translator
		/// (Asynchronized method)
		/// </summary>
		/// <param name="sourceLanguage">Source Language</param>
		/// <param name="destinationLanguage">Destination Language</param>
		/// <param name="text">Text for Translate</param>
		/// <returns>Return of success</returns>
		public static async void Translate ( Language sourceLanguage, Language destinationLanguage, string text )
		{
			if ( sourceLanguage == Language.Unknown || destinationLanguage == Language.Unknown )
			{
				Task<MessageBoxResult> mr = MessageBox.ShowAsync ( "언어 설정이 잘못되었습니다!",
					"랑트랜스", MessageBoxButton.OK );
				MainPage.Current.Retrans ();
				return;
			}
			if ( text.Trim ().Length == 0 )
			{
				Task<MessageBoxResult> mr = MessageBox.ShowAsync ( "번역할 문장을 입력해주세요!",
					"랑트랜스", MessageBoxButton.OK );
				MainPage.Current.Retrans ();
				return;
			}

			try
			{
				HttpClient httpClient = new HttpClient ();
				HttpRequestMessage message = new HttpRequestMessage ();
				message.RequestUri = new Uri ( String.Format (
					"http://translate.google.com/m?hl={1}&sl={0}&tl={1}&ie=UTF-8&prev=_m&q={2}",
					m_langDics [ sourceLanguage ], m_langDics [ destinationLanguage ], text ),
					UriKind.Absolute );
				message.Headers.Add ( "User-Agent",
					"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1;.NET CLR1.1.4322; InfoPath.1; .NET CLR 2.0.50727)" );
				var response = await httpClient.SendAsync ( message );
				if ( response.StatusCode == HttpStatusCode.OK )
				{
					var responsedString = await response.Content.ReadAsStringAsync ();
					string responsed = responsedString;
					int start, end;
					start = responsed.IndexOf ( "<div dir=\"ltr\" class=\"t0\">" ) + 26;
					end = responsed.IndexOf ( "</div>", start );

					m_translated = responsed.Substring ( start, end - start );

					if ( m_translateEnd != null ) m_translateEnd ();
				}
			}
			catch
			{
				Task<MessageBoxResult> mr = MessageBox.ShowAsync ( "인터넷 연결에 실패했습니다!",
					"랑트랜스", MessageBoxButton.OK );
				MainPage.Current.Retrans ();
			}
		}

		/// <summary>
		/// Language enumeration from language code
		/// </summary>
		/// <param name="code">Language code for Google</param>
		/// <returns>Language enumeration</returns>
		public static Language GetLanguageFromCode ( string code )
		{
			foreach ( KeyValuePair<Language, string> lang in m_langDics )
				if ( lang.Value == code ) return lang.Key;
			return Language.Unknown;
		}

		static Translator ()
		{
			#region Initialize
			m_langDics.Add ( Language.Galatia, "gl" );
			m_langDics.Add ( Language.Gujarati, "gu" );
			m_langDics.Add ( Language.Georgian, "ka" );
			m_langDics.Add ( Language.Greek, "el" );
			m_langDics.Add ( Language.Dutch, "nl" );
			m_langDics.Add ( Language.Norwegian, "no" );
			m_langDics.Add ( Language.Danish, "da" );
			m_langDics.Add ( Language.German, "de" );
			m_langDics.Add ( Language.Latvian, "lv" );
			m_langDics.Add ( Language.Latin, "la" );
			m_langDics.Add ( Language.Russian, "ru" );
			m_langDics.Add ( Language.Romanian, "ro" );
			m_langDics.Add ( Language.Lithuanian, "lt" );
			m_langDics.Add ( Language.Macedonian, "mk" );
			m_langDics.Add ( Language.Malay, "ms" );
			m_langDics.Add ( Language.Maltese, "mt" );
			m_langDics.Add ( Language.Basque, "eu" );
			m_langDics.Add ( Language.Vietnamese, "vi" );
			m_langDics.Add ( Language.Belarusian, "be" );
			m_langDics.Add ( Language.Bengali, "bn" );
			m_langDics.Add ( Language.Bulgarian, "bg" );
			m_langDics.Add ( Language.Serbian, "sr" );
			m_langDics.Add ( Language.Swahili, "sw" );
			m_langDics.Add ( Language.Swedish, "sv" );
			m_langDics.Add ( Language.Spanish, "es" );
			m_langDics.Add ( Language.Slovak, "sk" );
			m_langDics.Add ( Language.Slovenian, "sl" );
			m_langDics.Add ( Language.Arabic, "ar" );
			m_langDics.Add ( Language.Armenian, "hy" );
			m_langDics.Add ( Language.Icelandic, "is" );
			m_langDics.Add ( Language.HaitianFrench, "ht" );
			m_langDics.Add ( Language.Irish, "ga" );
			m_langDics.Add ( Language.Azerbaijani, "az" );
			m_langDics.Add ( Language.Afrikaans, "af" );
			m_langDics.Add ( Language.Albanian, "sq" );
			m_langDics.Add ( Language.Estonian, "et" );
			m_langDics.Add ( Language.Esperanto, "eo" );
			m_langDics.Add ( Language.English, "en" );
			m_langDics.Add ( Language.Urdu, "ur" );
			m_langDics.Add ( Language.Ukrainian, "uk" );
			m_langDics.Add ( Language.Weiljeueo, "cy" );
			m_langDics.Add ( Language.Yiddish, "yi" );
			m_langDics.Add ( Language.Italian, "it" );
			m_langDics.Add ( Language.Indonesian, "id" );
			m_langDics.Add ( Language.Japanese, "ja" );
			m_langDics.Add ( Language.Chinese, "zh-CN" );
			m_langDics.Add ( Language.Czech, "cs" );
			m_langDics.Add ( Language.Catalan, "ca" );
			m_langDics.Add ( Language.Kannada, "kn" );
			m_langDics.Add ( Language.Croatian, "hr" );
			m_langDics.Add ( Language.Tagalog, "tl" );
			m_langDics.Add ( Language.Tamil, "ta" );
			m_langDics.Add ( Language.Thai, "th" );
			m_langDics.Add ( Language.Turkish, "tr" );
			m_langDics.Add ( Language.Telugu, "te" );
			m_langDics.Add ( Language.Persian, "fa" );
			m_langDics.Add ( Language.Portuguese, "pt" );
			m_langDics.Add ( Language.Polish, "pl" );
			m_langDics.Add ( Language.French, "fr" );
			m_langDics.Add ( Language.Finnish, "fi" );
			m_langDics.Add ( Language.Korean, "ko" );
			m_langDics.Add ( Language.Hungarian, "hu" );
			m_langDics.Add ( Language.Hebrew, "iw" );
			m_langDics.Add ( Language.Hindi, "hl" );
			#endregion
		}
	}
}
