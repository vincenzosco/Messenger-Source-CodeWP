using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchFriendLastActive;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.Presence;

public class Service : BackgroundService
{
	private readonly TypingHelper _typingHelper;

	private readonly UserPresenceHelper _userPresenceHelper;

	private FacebookContext _context;

	public Service(IDispatcher ui, Facebook.Services.Api.Service service)
		: base(ui)
	{
		_typingHelper = new TypingHelper(ui);
		_userPresenceHelper = new UserPresenceHelper(ui, service);
	}

	public override void StartService(FacebookContext context)
	{
		_context = context;
		Messenger.Default.Register<LoginSignal>(this, OnLoginSignal);
		Messenger.Default.Register<LogoutSignal>(this, OnLogoutSignal);
		Messenger.Default.Register<FriendsUpdatedSignal>(this, OnFriendsUpdatedSignal);
		Messenger.Default.Register<PresenceUpdateSignal>(this, OnPresenceUpdatedSignal);
		Messenger.Default.Register<MqttTypingReceivedSignal>(this, OnTypingReceivedSignal);
		if (_context.IsAuthenticated())
		{
			_userPresenceHelper.Start(_context);
		}
	}

	public override void StopService()
	{
		Messenger.Default.Unregister(this);
		_typingHelper.Stop();
		_userPresenceHelper.Stop();
	}

	public bool IsUserTyping(string id)
	{
		return _typingHelper.IsUserTyping(id);
	}

	public async Task<List<FacebookUser>> GetOnlineUsers()
	{
		return await _userPresenceHelper.GetOnlineUsers();
	}

	public async Task<LastActiveParams> GetLastActiveForUser(string id)
	{
		return await _userPresenceHelper.GetLastActiveForUser(id);
	}

	private void OnTypingReceivedSignal(MqttTypingReceivedSignal signal)
	{
		_typingHelper.UpdateTyping(signal.SenderId, signal.State);
	}

	public async void OnFriendsUpdatedSignal(FriendsUpdatedSignal signal)
	{
		await _userPresenceHelper.FetchAndUpdatePresence();
	}

	private async void OnPresenceUpdatedSignal(PresenceUpdateSignal signal)
	{
		await _userPresenceHelper.UpdatePresence(signal.OnlineIds, signal.OfflineIds, signal.UidToClientType, signal.UidToLastActiveTime, signal.Type);
	}

	private void OnLogoutSignal(LogoutSignal signal)
	{
		_typingHelper.Cleanup();
		_userPresenceHelper.Cleanup();
	}

	private void OnLoginSignal(LoginSignal signal)
	{
		_userPresenceHelper.Start(_context);
	}
}
