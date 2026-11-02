using System.Text;

namespace Facebook.Services.Api.GraphQL;

public class ThreadQuery
{
	private const string ThreadQueryString = "Query ThreadQuery {message_threads(<thread_ids>) {@ThreadInfo}}";

	private const string MessageImageAttachmentString = "QueryFragment MessageImageAttachment : MessageBlobAttachment {__type__ {name},MessageImage? image.size(<full_screen_height>, <full_screen_width>) as image_full_screen {uri},MessageImage? image.size(<medium_preview_size>, <medium_preview_size>) as image_medium_preview {uri},MessageImage? original_dimensions {x,y}}";

	private const string MessageInfoString = "QueryFragment MessageInfo : Message {__type__ {name},answered,blob_attachments{__type__{name},@MessageImageAttachment,@MessageVideoAttachment,legacy_attachment_id as attachment_fbid,filename,filesize,mimetype},location {latitude,longitude},message {text},message_id,message_sender {@ParticipantInfo},offline_threading_id,P2PPaymentMessage? transfer_id as p2p_transfer_id,ParticipantsAddedMessage? participants_added {@ParticipantInfo},ParticipantLeftMessage? participants_removed {@ParticipantInfo},sticker {id},tags_list,GenericAdminTextMessage? theme_color,GenericAdminTextMessage? thread_icon as thread_icon_emoji,ThreadNameMessage? thread_name,ThreadImageMessage? image as thread_image{uri},timestamp_precise,unread}";

	private const string MessageVideoAttachmentString = "QueryFragment MessageVideoAttachment : MessageBlobAttachment {__type__ {name},MessageVideo? original_dimensions {x,y},MessageVideo? playable_duration_in_ms,MessageVideo? rotation,MessageVideo? streaming_image.resolution(720) as streamingImageThumbnail {uri},MessageVideo? url as video_url}";

	private const string MessagesString = "QueryFragment Messages : MessagesOfThreadConnection {nodes {__type__ {name},@MessageInfo},page_info {has_previous_page}}";

	private const string MessagingActorInfoString = "QueryFragment MessagingActorInfo : MessagingParticipant {messaging_actor {__type__ {name},@UserInfo}}";

	private const string NameFieldsString = "QueryFragment NameFields : Name {parts {length,offset,part},text}";

	private const string ParticipantInfoString = "QueryFragment ParticipantInfo : MessagingParticipant {email,messaging_actor {__type__ {name},id,name}}";

	private const string ReceiptInfoString = "QueryFragment ReceiptInfo : Node {__type__ {name},actor {__type__ {name},id},timestamp_precise}";

	private const string ThreadInfoString = "QueryFragment ThreadInfo : MessageThread{all_participants{nodes{@MessagingActorInfo}},can_viewer_reply,delivery_receipts{nodes{@ReceiptInfo}},folder,former_participants{nodes{@MessagingActorInfo}},has_viewer_archived,image{uri},is_viewer_subscribed,messages.last(1) as last_message {nodes{__type__{name},message_id,message_sender{@ParticipantInfo},snippet}},messages.last(<msg_count>){@Messages},mute_until,name,read_receipts{nodes {@ReceiptInfo}},thread_key{other_user_id,thread_fbid},unread_count,updated_time_precise}";

	private const string UserInfoString = "QueryFragment UserInfo : MessagingActor {__type__ {name},id,is_messenger_user,is_mobile_pushable,is_partial,name,last_active_messages_status{time,is_currently_active},structured_name{@NameFields}}";

	public static string CreateThreadQuery()
	{
		StringBuilder stringBuilder = new StringBuilder("Query ThreadQuery {message_threads(<thread_ids>) {@ThreadInfo}}");
		stringBuilder.Append("QueryFragment MessageImageAttachment : MessageBlobAttachment {__type__ {name},MessageImage? image.size(<full_screen_height>, <full_screen_width>) as image_full_screen {uri},MessageImage? image.size(<medium_preview_size>, <medium_preview_size>) as image_medium_preview {uri},MessageImage? original_dimensions {x,y}}");
		stringBuilder.Append("QueryFragment MessageInfo : Message {__type__ {name},answered,blob_attachments{__type__{name},@MessageImageAttachment,@MessageVideoAttachment,legacy_attachment_id as attachment_fbid,filename,filesize,mimetype},location {latitude,longitude},message {text},message_id,message_sender {@ParticipantInfo},offline_threading_id,P2PPaymentMessage? transfer_id as p2p_transfer_id,ParticipantsAddedMessage? participants_added {@ParticipantInfo},ParticipantLeftMessage? participants_removed {@ParticipantInfo},sticker {id},tags_list,GenericAdminTextMessage? theme_color,GenericAdminTextMessage? thread_icon as thread_icon_emoji,ThreadNameMessage? thread_name,ThreadImageMessage? image as thread_image{uri},timestamp_precise,unread}");
		stringBuilder.Append("QueryFragment MessageVideoAttachment : MessageBlobAttachment {__type__ {name},MessageVideo? original_dimensions {x,y},MessageVideo? playable_duration_in_ms,MessageVideo? rotation,MessageVideo? streaming_image.resolution(720) as streamingImageThumbnail {uri},MessageVideo? url as video_url}");
		stringBuilder.Append("QueryFragment Messages : MessagesOfThreadConnection {nodes {__type__ {name},@MessageInfo},page_info {has_previous_page}}");
		stringBuilder.Append("QueryFragment MessagingActorInfo : MessagingParticipant {messaging_actor {__type__ {name},@UserInfo}}");
		stringBuilder.Append("QueryFragment NameFields : Name {parts {length,offset,part},text}");
		stringBuilder.Append("QueryFragment ParticipantInfo : MessagingParticipant {email,messaging_actor {__type__ {name},id,name}}");
		stringBuilder.Append("QueryFragment ReceiptInfo : Node {__type__ {name},actor {__type__ {name},id},timestamp_precise}");
		stringBuilder.Append("QueryFragment ThreadInfo : MessageThread{all_participants{nodes{@MessagingActorInfo}},can_viewer_reply,delivery_receipts{nodes{@ReceiptInfo}},folder,former_participants{nodes{@MessagingActorInfo}},has_viewer_archived,image{uri},is_viewer_subscribed,messages.last(1) as last_message {nodes{__type__{name},message_id,message_sender{@ParticipantInfo},snippet}},messages.last(<msg_count>){@Messages},mute_until,name,read_receipts{nodes {@ReceiptInfo}},thread_key{other_user_id,thread_fbid},unread_count,updated_time_precise}");
		stringBuilder.Append("QueryFragment UserInfo : MessagingActor {__type__ {name},id,is_messenger_user,is_mobile_pushable,is_partial,name,last_active_messages_status{time,is_currently_active},structured_name{@NameFields}}");
		return stringBuilder.ToString();
	}
}
