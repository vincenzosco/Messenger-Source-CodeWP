using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Facebook.Messenger.WP8.AudioController;

public class Sounds
{
	public class Sound
	{
		public Uri Uri { get; private set; }

		public string Name { get; private set; }

		public float Volume { get; set; }

		public Sound(string url, float volume)
		{
			Uri = new Uri(url, UriKind.Relative);
			Name = Path.GetFileNameWithoutExtension(url);
			Volume = volume;
		}
	}

	public static readonly Sound Send = new Sound("Assets/Sounds/sendNeue.wav", 0.8f);

	public static readonly Sound Sent = new Sound("Assets/Sounds/sentNeue.wav", 0.8f);

	public static readonly Sound SendSticker = new Sound("Assets/Sounds/stickerOutgoingNeue.wav", 0.7f);

	public static readonly Sound SendLike = new Sound("Assets/Sounds/outgoing_likeNeue.wav", 0.55f);

	public static readonly Sound ReceiveLike = new Sound("Assets/Sounds/incoming_likeNeue.wav", 0.55f);

	public static readonly Sound ReceiveSticker = new Sound("Assets/Sounds/stickerIncomingNeue.wav", 0.6f);

	public static readonly Sound ReceiveMessage = new Sound("Assets/Sounds/incomingMsgNeue.wav", 0.8f);

	public static readonly Sound Seen = new Sound("Assets/Sounds/seenNeue.wav", 0.85f);

	public static readonly Sound Typing = new Sound("Assets/Sounds/typingNeue.wav", 1f);

	public static readonly Sound LocationOn = new Sound("Assets/Sounds/locationOn.wav", 0.35f);

	public static readonly Sound DrawerSelected = new Sound("Assets/Sounds/tabSelected.wav", 0.3f);

	public static readonly Sound TextDrawerSelected = new Sound("Assets/Sounds/textSelected.wav", 0.3f);

	public static readonly Sound PullToRefreshStart = new Sound("Assets/Sounds/pulltorefresh1.wav", 0.7f);

	public static readonly Sound PullToRefreshEnd = new Sound("Assets/Sounds/pulltorefresh2.wav", 0.7f);

	public static readonly Sound NewGroupCreateTapped = new Sound("Assets/Sounds/newGroupCreateTapped.wav", 0.3f);

	public static readonly Sound NewGroupAppearing = new Sound("Assets/Sounds/newGroupAppearing.wav", 0.3f);

	public static readonly Sound NewGroupBounce = new Sound("Assets/Sounds/newGroupBounce.wav", 0.3f);

	public static readonly Sound CardLanded = new Sound("Assets/Sounds/cardLanded.wav", 0.3f);

	public static readonly Sound CardPickedUp = new Sound("Assets/Sounds/cardPickedUp.wav", 0.3f);

	public static readonly Sound CardSlide1 = new Sound("Assets/Sounds/cardSlide1.wav", 1f);

	public static readonly Sound CardSlide2 = new Sound("Assets/Sounds/cardSlide2.wav", 1f);

	public static readonly Sound CardTouched = new Sound("Assets/Sounds/cardTouched.wav", 0.3f);

	public static readonly Sound CardRemoved = new Sound("Assets/Sounds/removeGroup.wav", 0.3f);

	public static readonly Sound CameraButtonDownUp = new Sound("Assets/Sounds/cameraButtonDownUp.wav", 0.65f);

	public static readonly Sound CameraButtonDown = new Sound("Assets/Sounds/cameraButtonDown.wav", 0.65f);

	public static readonly Sound CameraButtonUp = new Sound("Assets/Sounds/cameraButtonUp.wav", 0.65f);

	public static readonly Sound CancelRecord = new Sound("Assets/Sounds/cancelRecord.wav", 0.65f);

	public static readonly Sound VoiceClipCancel = new Sound("Assets/Sounds/cancelRecord.wav", 0.4f);

	public static readonly Sound VoiceClipSend = new Sound("Assets/Sounds/voiceClipStop.wav", 0.6f);

	public static IEnumerable<Sound> GetAllSounds()
	{
		return from f in typeof(Sounds).GetFields()
			where (object)f.FieldType == typeof(Sound) && f.IsStatic
			select (Sound)f.GetValue(null);
	}
}
