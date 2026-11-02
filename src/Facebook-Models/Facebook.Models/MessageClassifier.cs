using System;
using System.Collections.Generic;
using System.Linq;

namespace Facebook.Models
{
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
			switch (message.MessageType)
			{
			case MailMessage.Type.Regular:
				return GetRegularMessageClassification(message);
			case MailMessage.Type.ParticipantAdded:
				return Classification.ThreadParticipantAdded;
			case MailMessage.Type.ParticipantLeft:
				return Classification.ThreadParticipantRemoved;
			case MailMessage.Type.VideoCall:
				return Classification.VideoCall;
			case MailMessage.Type.VoiceCall:
				return Classification.VoiceCall;
			case MailMessage.Type.VoiceCallMissed:
				return Classification.VoiceCallMissed;
			case MailMessage.Type.ThreadImageSet:
				return Classification.ThreadImageSet;
			case MailMessage.Type.ThreadImageRemoved:
				return Classification.ThreadImageRemoved;
			case MailMessage.Type.ThreadNameSet:
				return Classification.ThreadNameSet;
			case MailMessage.Type.ThreadNameRemoved:
				return Classification.ThreadNameRemoved;
			case MailMessage.Type.Unknown:
				return Classification.Unknown;
			default:
				throw new ArgumentOutOfRangeException("MessageType: " + message.MessageType);
			}
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
			bool flag = attachments != null && attachments.Any();
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
}
