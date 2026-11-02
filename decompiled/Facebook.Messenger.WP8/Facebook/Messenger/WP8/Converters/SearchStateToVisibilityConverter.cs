using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Converters;

public class SearchStateToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		switch ((ContactSearchViewModel.SearchState)value)
		{
		case ContactSearchViewModel.SearchState.None:
		case ContactSearchViewModel.SearchState.Querying:
		case ContactSearchViewModel.SearchState.HasResults:
			return (object)(Visibility)1;
		case ContactSearchViewModel.SearchState.NoResults:
			return (object)(Visibility)0;
		default:
			return (object)(Visibility)1;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
