using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels;

public class MuteSelectionItemViewModel
{
	private MuteDuration _duration;

	private MuteSelectionViewModel _parent;

	private RelayCommand _command;

	public RelayCommand Command
	{
		get
		{
			if (_command == null)
			{
				_command = new RelayCommand(Execute);
			}
			return _command;
		}
	}

	public string Name => _duration switch
	{
		MuteDuration.Forever => StringLibrary.Mute_Conversation_Forever, 
		MuteDuration.Hour => StringLibrary.Mute_Conversation_For_An_Hour, 
		MuteDuration.EightAM => StringLibrary.Mute_Conversation_Until_8AM, 
		MuteDuration.Off => StringLibrary.UnMute_Conversation_Menu_Item, 
		_ => string.Empty, 
	};

	public MuteSelectionItemViewModel(MuteSelectionViewModel parent, MuteDuration duration)
	{
		_parent = parent;
		_duration = duration;
	}

	public async void Execute()
	{
		await _parent.TurnOnMute(_duration);
	}
}
