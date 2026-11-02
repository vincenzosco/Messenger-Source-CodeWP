using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Threading;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Windows.Phone.Devices.Notification;

namespace Facebook.Messenger.WP8.AudioController;

public class Service : BackgroundService
{
	private const double MinimumMillisecondsBetweenSounds = 300.0;

	private static readonly TimeSpan VibrationPeriod = TimeSpan.FromSeconds(0.5);

	private readonly DispatcherTimer _updateTimer = new DispatcherTimer();

	private readonly FacebookContext _context;

	private DateTime _lastPlayed;

	public Service(IDispatcher ui, FacebookContext context)
		: base(ui)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected Obj, but got Unknown
		_context = context;
	}

	public override void StartService(FacebookContext context)
	{
		UiDispatcher.RunAsync(() =>
		{
			FrameworkDispatcher.Update();
			_updateTimer.Interval = TimeSpan.FromMilliseconds(50.0);
			_updateTimer.Tick += UpdateTimerTick;
			_updateTimer.Start();
		});
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheMessagesUpdatedSignal>(this, OnCacheMessageUpdatedSignal);
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MessageSentSignal>(this, "inbox", OnMessageSent);
	}

	public override void PrepareStopServiceUiThreadMustNotUseDispatcher()
	{
		_updateTimer.Tick -= UpdateTimerTick;
		_updateTimer.Stop();
	}

	public override void StopService()
	{
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
	}

	private void UpdateTimerTick(object sender, EventArgs e)
	{
		FrameworkDispatcher.Update();
	}

	private void OnMessageSent(MessageSentSignal signal)
	{
		if (!signal.Message.HasSticker())
		{
			PlaySound(Sounds.Sent);
		}
	}

	private void OnCacheMessageUpdatedSignal(CacheMessagesUpdatedSignal signal)
	{
		bool shouldNotify = signal.ShouldNotify;
		MailThread thread = signal.Thread;
		bool flag = MuteHelper.IsThreadMuted(thread.MuteUntil) || MuteHelper.IsMuted(_context);
		foreach (MailMessage message in signal.Messages)
		{
			bool flag2 = UserUtilities.IsCurrentUser(message.Sender, _context);
			if (!shouldNotify || !_context.IsAuthenticated() || flag2 || flag)
			{
				break;
			}
			if (message.HasSticker())
			{
				bool flag3 = MailShare.LikeStickerIds.Contains(message.StickerId());
				PlaySound(flag3 ? Sounds.ReceiveLike : Sounds.ReceiveSticker);
			}
			else
			{
				PlaySound(Sounds.ReceiveMessage);
			}
			if (_context.IsVibrationEnabled())
			{
				VibrationDevice val = VibrationDevice.GetDefault();
				val.Vibrate(VibrationPeriod);
			}
		}
	}

	public void PlaySeen()
	{
		PlaySound(Sounds.Seen);
	}

	public void PlayTypingIndicator()
	{
		PlaySound(Sounds.Typing);
	}

	public void PlayLocationOn()
	{
		PlaySound(Sounds.LocationOn);
	}

	public void PlayDrawerSelected()
	{
		PlaySound(Sounds.DrawerSelected);
	}

	public void PlayTextDrawerSelected()
	{
		PlaySound(Sounds.TextDrawerSelected);
	}

	public void PlaySendingMessage()
	{
		PlaySound(Sounds.Send);
	}

	public void PlaySendingSticker()
	{
		PlaySound(Sounds.SendSticker);
	}

	public void PlaySendingLike()
	{
		PlaySound(Sounds.SendLike);
	}

	public void PlayRefreshStart()
	{
		PlaySound(Sounds.PullToRefreshStart);
	}

	public void PlayRefreshEnd()
	{
		PlaySound(Sounds.PullToRefreshEnd);
	}

	public void PlayNewGroupCreatedTap()
	{
		PlaySound(Sounds.NewGroupCreateTapped);
	}

	public void PlayNewGroupAppearing()
	{
		PlaySound(Sounds.NewGroupAppearing);
	}

	public void PlayNewGroupBounce()
	{
		PlaySound(Sounds.NewGroupBounce);
	}

	public void PlayCardLanded()
	{
		PlaySound(Sounds.CardLanded);
	}

	public void PlayCardPickedUp()
	{
		PlaySound(Sounds.CardPickedUp);
	}

	public void PlayCardSlide1()
	{
		PlaySound(Sounds.CardSlide1);
	}

	public void PlayCardSlide2()
	{
		PlaySound(Sounds.CardSlide2);
	}

	public void PlayCardTouched()
	{
		PlaySound(Sounds.CardTouched);
	}

	public void PlayRemovingPin()
	{
		PlaySound(Sounds.CardRemoved);
	}

	public void PlayCameraDownUp()
	{
		PlaySound(Sounds.CameraButtonDownUp);
	}

	public void PlayCameraDown()
	{
		PlaySound(Sounds.CameraButtonDown);
	}

	public void PlayCameraUp()
	{
		PlaySound(Sounds.CameraButtonUp);
	}

	public void PlayCancelRecord()
	{
		PlaySound(Sounds.CancelRecord);
	}

	public void PlayVoiceClipSend()
	{
		PlaySound(Sounds.VoiceClipSend);
	}

	public void PlayVoiceClipCancel()
	{
		PlaySound(Sounds.VoiceClipCancel);
	}

	public void PlaySound(Sounds.Sound sound)
	{
		if (!_context.IsAudioEnabled())
		{
			return;
		}
		double totalMilliseconds = (DateTime.Now - _lastPlayed).TotalMilliseconds;
		if (totalMilliseconds < 300.0)
		{
			return;
		}
		_lastPlayed = DateTime.Now;
		try
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(sound.Uri);
			using Stream stream = resourceStream.Stream;
			SoundEffect val = SoundEffect.FromStream(stream);
			val.Play(sound.Volume, 0f, 0f);
		}
		catch (Exception arg)
		{
			Logger.Error(this, $"Unable to play sound {sound.Name}: {arg}", "PlaySound", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\AudioController\\Service.cs", 252);
		}
	}
}
