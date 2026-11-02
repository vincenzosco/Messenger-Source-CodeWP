using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.SyncProtocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Api.Methods.ThreadQuery
{
	public class ThreadQueryConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			MailThread mailThread = new MailThread();
			List<MailMessage> messages = new List<MailMessage>();
			if (reader.TokenType == JsonToken.StartObject)
			{
				JObject source = JObject.Load(reader);
				foreach (JToken item in source.Values())
				{
					JToken jToken = item.SelectToken("thread_key");
					JToken jToken2 = jToken.SelectToken("other_user_id");
					if (jToken2 != null)
					{
						mailThread.SingleRecipient = jToken2.ToObject<string>();
					}
					string text = jToken.SelectToken("thread_fbid").ToObject<string>();
					if (text != null)
					{
						mailThread.Fbid = new ThreadFbid(long.Parse(text));
					}
					mailThread.Name = item.SelectToken("name").ToObject<string>();
					mailThread.Folder = item.SelectToken("folder").ToObject<string>();
					string text2 = item.SelectToken("mute_until").ToObject<string>();
					if (text2 != null)
					{
						mailThread.MuteUntil = long.Parse(text2);
					}
					JToken jToken3 = item.SelectToken("last_message");
					if (jToken3 != null)
					{
						IEnumerable<JToken> tokenListFromNodes = GetTokenListFromNodes(jToken3);
						if (tokenListFromNodes != null)
						{
							JToken jToken4 = tokenListFromNodes.Last();
							JToken jToken5 = jToken4.SelectToken("__type__");
							string text3 = jToken5.SelectToken("name").ToObject<string>();
							if (text3.Equals("UserMessage"))
							{
								mailThread.Snippet = jToken4.SelectToken("snippet").ToObject<string>();
							}
							else
							{
								mailThread.AdminSnippet = jToken4.SelectToken("snippet").ToObject<string>();
							}
							mailThread.SnippetMessageId = jToken4.SelectToken("message_id").ToObject<string>();
							JToken messageSender = jToken4.SelectToken("message_sender");
							mailThread.SnippetSender = ParseSnippetSender(messageSender);
						}
					}
					JToken participantsNodes = item.SelectToken("all_participants");
					mailThread.Participants = ParseParticipants(participantsNodes);
					JToken participantsNodes2 = item.SelectToken("former_participants");
					mailThread.FormerParticipants = ParseParticipants(participantsNodes2);
					long timestamp = (mailThread.Timestamp = long.Parse(item.SelectToken("updated_time_precise").ToObject<string>()));
					mailThread.ActionId = ActionIdUtil.CreateActionIdFromTimestamp(timestamp);
					long num2 = item.SelectToken("unread_count").ToObject<long>();
					mailThread.Unread = num2 > 0;
					mailThread.CanReply = item.SelectToken("can_viewer_reply").ToObject<bool>();
					mailThread.IsSubscribed = item.SelectToken("is_viewer_subscribed").ToObject<bool>();
					mailThread.Archived = item.SelectToken("has_viewer_archived").ToObject<bool>();
					JToken readReceiptNodes = item.SelectToken("read_receipts");
					mailThread.ReadReceipts = ParseReadReceipts(readReceiptNodes);
					JToken deliveryReceiptsNodes = item.SelectToken("delivery_receipts");
					mailThread.DeliveryReceipts = ParseDeliveryReceipts(deliveryReceiptsNodes);
					JToken jToken6 = item.SelectToken("image");
					if (jToken6.HasValues)
					{
						mailThread.PictureUrl = jToken6.SelectToken("uri").ToObject<string>();
					}
					messages = ParseMessages(item.SelectToken("messages"), mailThread);
				}
			}
			Response response = new Response();
			response.Thread = mailThread;
			response.Messages = messages;
			return response;
		}

		private List<MailMessage> ParseMessages(JToken messageNodes, MailThread thread)
		{
			List<MailMessage> list = new List<MailMessage>();
			IEnumerable<JToken> tokenListFromNodes = GetTokenListFromNodes(messageNodes);
			foreach (JToken item in tokenListFromNodes)
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.ThreadId = thread.SingleRecipient ?? thread.Fbid.ToString();
				mailMessage.Id = item.SelectToken("message_id").ToObject<string>();
				mailMessage.MessageType = GetAdminMessageType(item);
				MailMessage mailMessage2 = mailMessage;
				JToken jToken = item.SelectToken("message_sender");
				JToken jToken2 = jToken.SelectToken("messaging_actor");
				string messageSenderId = jToken2.SelectToken("id").ToObject<string>();
				MailUser mailUser = thread.Participants.FirstOrDefault((MailUser p) => p.Id == messageSenderId);
				mailMessage2.Sender = mailUser ?? thread.FormerParticipants.FirstOrDefault((MailUser p) => p.Id == messageSenderId);
				JToken jToken3 = item.SelectToken("message").SelectToken("text");
				if (jToken3 != null)
				{
					mailMessage2.Body = jToken3.ToObject<string>();
				}
				long timestamp = (mailMessage2.ServerTimestamp = long.Parse(item.SelectToken("timestamp_precise").ToObject<string>()));
				mailMessage2.ActionId = ActionIdUtil.CreateActionIdFromTimestamp(timestamp);
				mailMessage2.OfflineThreadingId = item.SelectToken("offline_threading_id").ToObject<string>();
				JToken jToken4 = item.SelectToken("blob_attachments");
				if (jToken4.HasValues)
				{
					mailMessage2.Attachments = ParseAttachments(jToken4.ToObject<JArray>());
				}
				MailCoordinates mailCoordinates = new MailCoordinates();
				JToken jToken5 = item.SelectToken("location");
				if (jToken5.HasValues)
				{
					mailCoordinates.Latitude = jToken5.SelectToken("latitude").ToObject<double>();
					mailCoordinates.Longitude = jToken5.SelectToken("longitude").ToObject<double>();
				}
				mailMessage2.Coordinates = mailCoordinates;
				mailMessage2.Tags = new List<string>();
				JToken jToken6 = item.SelectToken("tags_list");
				foreach (JToken item2 in (IEnumerable<JToken>)jToken6)
				{
					mailMessage2.Tags.Add(item2.ToObject<string>());
				}
				JToken jToken7 = item.SelectToken("sticker");
				if (jToken7.HasValues)
				{
					string stickerId = jToken7.SelectToken("id").ToObject<string>();
					mailMessage2.Shares = new List<MailShare>
					{
						new MailShare
						{
							StickerId = stickerId
						}
					};
				}
				list.Add(mailMessage2);
			}
			return list;
		}

		private static MailMessage.Type GetAdminMessageType(JToken messageToken)
		{
			JToken jToken = messageToken.SelectToken("__type__");
			switch (jToken.SelectToken("name").ToObject<string>())
			{
			case "UserMessage":
				return MailMessage.Type.Regular;
			case "ParticipantLeftMessage":
				return MailMessage.Type.ParticipantLeft;
			case "ParticipantsAddedMessage":
				return MailMessage.Type.ParticipantAdded;
			case "VideoCallMessage":
				if (!messageToken.SelectToken("answered").ToObject<bool>())
				{
					return MailMessage.Type.VideoCallMissed;
				}
				return MailMessage.Type.VideoCall;
			case "VoiceCallMessage":
				if (!messageToken.SelectToken("answered").ToObject<bool>())
				{
					return MailMessage.Type.VoiceCallMissed;
				}
				return MailMessage.Type.VoiceCall;
			case "ThreadImageMessage":
			{
				JToken jToken2 = messageToken.SelectToken("thread_image");
				if (!jToken2.HasValues)
				{
					return MailMessage.Type.ThreadImageRemoved;
				}
				return MailMessage.Type.ThreadImageSet;
			}
			case "ThreadNameMessage":
			{
				string value = messageToken.SelectToken("thread_name").ToObject<string>();
				if (!string.IsNullOrEmpty(value))
				{
					return MailMessage.Type.ThreadNameSet;
				}
				return MailMessage.Type.ThreadNameRemoved;
			}
			default:
				return MailMessage.Type.Unknown;
			}
		}

		private List<MailAttachment> ParseAttachments(IEnumerable<JToken> attachmentsArray)
		{
			List<MailAttachment> list = new List<MailAttachment>();
			foreach (JToken item in attachmentsArray)
			{
				MailAttachment mailAttachment = new MailAttachment();
				JToken jToken = item.SelectToken("__type__");
				string text = jToken.SelectToken("name").ToObject<string>();
				if (text.StartsWith("Message"))
				{
					FileUtil.FileType result;
					Enum.TryParse<FileUtil.FileType>(text.Substring("Message".Length), out result);
					mailAttachment.Type = result;
				}
				mailAttachment.Id = item.SelectToken("attachment_fbid").ToObject<string>();
				mailAttachment.MimeType = item.SelectToken("mimetype").ToObject<string>();
				mailAttachment.FileName = item.SelectToken("filename").ToObject<string>();
				string text2 = item.SelectToken("filesize").ToObject<string>();
				mailAttachment.FileSize = (string.IsNullOrEmpty(text2) ? 0 : long.Parse(text2));
				switch (mailAttachment.Type)
				{
				case FileUtil.FileType.Image:
				case FileUtil.FileType.AnimatedImage:
					mailAttachment.Image = ParseImage(item);
					break;
				case FileUtil.FileType.Video:
					mailAttachment.Video = ParseVideo(item);
					break;
				}
				list.Add(mailAttachment);
			}
			return list;
		}

		private static MailAttachment.VideoData ParseVideo(JToken attachment)
		{
			MailAttachment.VideoData videoData = new MailAttachment.VideoData();
			JToken jToken = attachment.SelectToken("original_dimensions");
			videoData.Width = jToken.SelectToken("x").ToObject<int>();
			videoData.Height = jToken.SelectToken("y").ToObject<int>();
			videoData.Length = attachment.SelectToken("playable_duration_in_ms").ToObject<int>();
			videoData.Url = attachment.SelectToken("video_url").ToObject<string>();
			videoData.Rotation = attachment.SelectToken("rotation").ToObject<int>();
			JToken jToken2 = attachment.SelectToken("streamingImageThumbnail");
			videoData.PreviewUrl = jToken2.SelectToken("uri").ToObject<string>();
			return videoData;
		}

		private static MailAttachment.ImageData ParseImage(JToken attachment)
		{
			MailAttachment.ImageData imageData = new MailAttachment.ImageData();
			JToken jToken = attachment.SelectToken("original_dimensions");
			imageData.Width = jToken.SelectToken("x").ToObject<int>();
			imageData.Height = jToken.SelectToken("y").ToObject<int>();
			JToken jToken2 = attachment.SelectToken("image_full_screen");
			imageData.Url = jToken2.SelectToken("uri").ToObject<string>();
			JToken jToken3 = attachment.SelectToken("image_medium_preview");
			imageData.PreviewUrl = jToken3.SelectToken("uri").ToObject<string>();
			return imageData;
		}

		private List<DeliveryReceipt> ParseDeliveryReceipts(JToken deliveryReceiptsNodes)
		{
			if (deliveryReceiptsNodes != null)
			{
				List<DeliveryReceipt> list = new List<DeliveryReceipt>();
				IEnumerable<JToken> tokenListFromNodes = GetTokenListFromNodes(deliveryReceiptsNodes);
				{
					foreach (JToken item in tokenListFromNodes)
					{
						DeliveryReceipt deliveryReceipt = new DeliveryReceipt();
						string s = item.SelectToken("timestamp_precise").ToObject<string>();
						deliveryReceipt.Timestamp = long.Parse(s);
						JToken jToken = item.SelectToken("actor");
						deliveryReceipt.Id = jToken.SelectToken("id").ToObject<string>();
						list.Add(deliveryReceipt);
					}
					return list;
				}
			}
			return new List<DeliveryReceipt>();
		}

		private List<ReadReceipt> ParseReadReceipts(JToken readReceiptNodes)
		{
			if (readReceiptNodes != null)
			{
				List<ReadReceipt> list = new List<ReadReceipt>();
				IEnumerable<JToken> tokenListFromNodes = GetTokenListFromNodes(readReceiptNodes);
				{
					foreach (JToken item in tokenListFromNodes)
					{
						ReadReceipt readReceipt = new ReadReceipt();
						string s = item.SelectToken("timestamp_precise").ToObject<string>();
						readReceipt.Timestamp = long.Parse(s);
						JToken jToken = item.SelectToken("actor");
						readReceipt.Id = jToken.SelectToken("id").ToObject<string>();
						list.Add(readReceipt);
					}
					return list;
				}
			}
			return new List<ReadReceipt>();
		}

		private static MailUser ParseSnippetSender(JToken messageSender)
		{
			JToken jToken = messageSender.SelectToken("messaging_actor");
			MailUser mailUser = new MailUser();
			mailUser.Id = jToken.SelectToken("id").ToObject<string>();
			mailUser.Name = jToken.SelectToken("name").ToObject<string>();
			return mailUser;
		}

		private List<MailUser> ParseParticipants(JToken participantsNodes)
		{
			if (participantsNodes != null)
			{
				IEnumerable<JToken> tokenListFromNodes = GetTokenListFromNodes(participantsNodes);
				if (tokenListFromNodes != null)
				{
					List<MailUser> list = new List<MailUser>();
					{
						foreach (JToken item in tokenListFromNodes)
						{
							MailUser mailUser = new MailUser();
							JToken jToken = item.SelectToken("messaging_actor");
							mailUser.Id = jToken.SelectToken("id").ToObject<string>();
							mailUser.IsMessengerUser = jToken.SelectToken("is_messenger_user").ToObject<bool>();
							mailUser.Pushable = jToken.SelectToken("is_mobile_pushable").ToObject<bool>();
							mailUser.IsPartial = jToken.SelectToken("is_partial").ToObject<bool>();
							mailUser.Name = jToken.SelectToken("name").ToObject<string>();
							ParseStructuredName(jToken, mailUser);
							JToken jToken2 = item.SelectToken("last_active_messages_status");
							if (jToken2 != null)
							{
								mailUser.SerializedLastActive = new LastActive
								{
									Active = jToken2.SelectToken("is_currently_active").ToObject<bool>(),
									Timestamp = jToken2.SelectToken("time").ToObject<long>()
								};
							}
							list.Add(mailUser);
						}
						return list;
					}
				}
			}
			return new List<MailUser>();
		}

		private static void ParseStructuredName(JToken messagingActor, MailUser mailUser)
		{
			JToken jToken = messagingActor.SelectToken("structured_name");
			string text = jToken.SelectToken("text").ToObject<string>();
			JArray jArray = jToken.SelectToken("parts").ToObject<JArray>();
			if (jArray == null)
			{
				return;
			}
			foreach (JToken item in jArray)
			{
				string text2 = item.SelectToken("part").ToObject<string>();
				int startIndex = item.SelectToken("offset").ToObject<int>();
				int length = item.SelectToken("length").ToObject<int>();
				string text3 = text.Substring(startIndex, length);
				if (text2.Equals("first"))
				{
					mailUser.FirstName = text3;
				}
				else if (text2.Equals("middle"))
				{
					mailUser.MiddleName = text3;
				}
				else if (text2.Equals("last"))
				{
					mailUser.LastName = text3;
				}
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		private static IEnumerable<JToken> GetTokenListFromNodes(JToken nodes)
		{
			JArray jArray = nodes.SelectToken("nodes").ToObject<JArray>();
			if (jArray != null)
			{
				return jArray.Children().ToList();
			}
			return null;
		}
	}
}
