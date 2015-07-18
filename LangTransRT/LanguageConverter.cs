using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace LangTransRT
{
	public class LanguageConverter : IValueConverter
	{
		public object Convert ( object value, Type targetType, object parameter, string language )
		{
			return MainPage.langs [ ( int ) value ];
		}

		public object ConvertBack ( object value, Type targetType, object parameter, string language )
		{
			throw new NotImplementedException ();
		}
	}
}
