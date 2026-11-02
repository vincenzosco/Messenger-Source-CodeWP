using System;
using System.Collections.Generic;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ViewModelBuilder
	{
		private readonly UriMapper _mapper;

		public ViewModelBuilder(UriMapper mapper)
		{
			_mapper = mapper;
		}

		public TViewModel Build<TViewModel>(Uri uri, Dictionary<string, object> state = null) where TViewModel : FacebookMessengerViewModel
		{
			string mapping = _mapper.GetMapping(uri);
			TViewModel instance = ServiceLocator.Current.GetInstance<TViewModel>();
			if (mapping == null)
			{
				throw new Exception("Error parsing uri: " + uri);
			}
			if (instance == null)
			{
				throw new Exception("ViewModel was not registered in the injector: " + typeof(TViewModel));
			}
			instance.SetPath(new FacebookUri(mapping, UriKind.RelativeOrAbsolute));
			if (state != null)
			{
				instance.LoadState(state);
			}
			return instance;
		}
	}
}
