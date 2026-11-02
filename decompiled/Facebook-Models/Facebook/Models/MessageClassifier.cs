using System;
using System.Collections.Generic;
using System.Linq;

namespace Facebook.Models;

public static class MessageClassifier
{
	public enum Classification
	{
		Normal,
		LikeSticker,
		Sticker,
		Audio,
		Image,
		Video,
		ThreadParticipantAdded,
		ThreadParticipantRemoved,
		ThreadNameSet,
		ThreadNameRemoved,
		ThreadImageSet,
		ThreadImageRemoved,
		VideoCall,
		VoiceCall,
		VoiceCallMissed,
		Unknown
	}

	public static Classification Classify(MailMessage message)
	{
		return message.MessageType switch
		{
			MailMessage.Type.Regular => GetRegularMessageClassification(message), 
			MailMessage.Type.ParticipantAdded => Classification.ThreadParticipantAdded, 
			MailMessage.Type.ParticipantLeft => Classification.ThreadParticipantRemoved, 
			MailMessage.Type.VideoCall => Classification.VideoCall, 
			MailMessage.Type.VoiceCall => Classification.VoiceCall, 
			MailMessage.Type.VoiceCallMissed => Classification.VoiceCallMissed, 
			MailMessage.Type.ThreadImageSet => Classification.ThreadImageSet, 
			MailMessage.Type.ThreadImageRemoved => Classification.ThreadImageRemoved, 
			MailMessage.Type.ThreadNameSet => Classification.ThreadNameSet, 
			MailMessage.Type.ThreadNameRemoved => Classification.ThreadNameRemoved, 
			MailMessage.Type.Unknown => Classification.Unknown, 
			_ => throw new ArgumentOutOfRangeException("MessageType: " + message.MessageType), 
		};
	}

	private static Classification GetRegularMessageClassification(MailMessage message)
	{
		if (message.HasSticker())
		{
			if (!MailShare.LikeStickerIds.Contains(message.StickerId()))
			{
				return Classification.Sticker;
			}
			return Classification.LikeSticker;
		}
		List<MailAttachment> attachments = message.Attachments;
		bool flag = attachments?.Any() ?? false;
		bool flag2 = attachments != null && attachments.Count == 1;
		if (flag && attachments.All((MailAttachment a) => a.IsImage()))
		{
			return Classification.Image;
		}
		if (flag2 && attachments.All((MailAttachment s) => s.IsVideo()))
		{
			return Classification.Video;
		}
		if (flag2 && attachments.All((MailAttachment a) => a.IsAudioClip()))
		{
			return Classification.Audio;
		}
		return Classification.Normal;
	}
}
