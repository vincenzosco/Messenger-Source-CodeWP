using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Facebook.Messenger.WP8.AudioController;

namespace Facebook.Messenger.WP8.ViewModels.Internal;

public class AudioLevelsViewModel
{
	private readonly Service _audioService;

	public IEnumerable<Sounds.Sound> Sounds { get; private set; }

	public AudioLevelsViewModel(Service audioService)
	{
		_audioService = audioService;
		Sounds = Facebook.Messenger.WP8.AudioController.Sounds.GetAllSounds();
	}

	public void PlaySound(Sounds.Sound sound)
	{
		_audioService.PlaySound(sound);
	}

	public void CopySoundInformationToClipboard()
	{
		IEnumerable<string> values = Sounds.Select((Sounds.Sound s) => $"Name: {s.Name}, Volume: {s.Volume}");
		string text = "Audio Data\n\n" + string.Join("\n", values);
		Clipboard.SetText(text);
	}
}
