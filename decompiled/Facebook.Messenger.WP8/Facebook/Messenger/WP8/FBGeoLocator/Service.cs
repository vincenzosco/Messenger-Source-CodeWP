using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Models;
using Facebook.Services;
using Microsoft.Phone.Maps.Services;
using Newtonsoft.Json;
using Windows.Devices.Geolocation;

namespace Facebook.Messenger.WP8.FBGeoLocator;

public class Service : BackgroundService
{
	public enum GeolocatorStatus
	{
		NotAvailable,
		Invalid,
		Ready
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CStartGeolocatorAsync_003Ed__d : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public Service _003C_003E4__this;

		public Geoposition _003Cposition_003E5__e;

		public IDisposable _003C_003E7__wrapf;

		private TaskAwaiter<IDisposable> _003C_003Eu___0024awaiter10;

		private object _003C_003Et__stack;

		private TaskAwaiter<Geoposition> _003C_003Eu___0024awaiter11;

		private void MoveNext()
		{
			// ILSpy could not decompile this member: Error decompiling @0600150E Facebook.Messenger.WP8.FBGeoLocator.Service.<StartGeolocatorAsync>d__d.MoveNext: Value cannot be null. (Parameter 'source')
			// Please report the exception below,
			// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
			// Error decompiling @0600150E Facebook.Messenger.WP8.FBGeoLocator.Service.<StartGeolocatorAsync>d__d.MoveNext
			// in assembly "extracted/Facebook.Messenger.WP8.dll"
			//  ---> System.ArgumentNullException: Value cannot be null. (Parameter 'source')
			//    at System.Linq.ThrowHelper.ThrowArgumentNullException(ExceptionArgument) + 0x31
			//    at System.Linq.Enumerable.Take[TSource](IEnumerable`1, Int32) + 0x48
			//    at ICSharpCode.Decompiler.CSharp.ArgumentList.GetArgumentResolveResultsDirect(Int32) + 0x24
			//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Probe() + 0xc8
			//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Run() + 0x10
			//    at ICSharpCode.Decompiler.CSharp.Disambiguator.ForMethodReference(ExpressionBuilder, IMethod, IType, TranslatedExpression, Boolean, ExpectedTargetDetails, Boolean, Boolean) + 0x103
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.DisambiguateDelegateReference(IMethod, IMethod, ExpectedTargetDetails, ILInstruction) + 0x25f
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildDelegateReference(IMethod, IMethod, ExpectedTargetDetails, ILInstruction) + 0x53
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitLdFtn(LdFtn, TranslationContext) + 0x98
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.HandleDelegateConstruction(CallInstruction) + 0x139
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x9f
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitNewObj(NewObj, TranslationContext) + 0x150
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(OpCode, IMethod, IReadOnlyList`1, IReadOnlyList`1, IType) + 0x4e7
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x12f
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitCall(Call, TranslationContext) + 0x57
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Default(ILInstruction) + 0xa7
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlock(Block) + 0x51
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitIfInstruction(IfInstruction) + 0x3d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlock(Block) + 0x51
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitIfInstruction(IfInstruction) + 0x3d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.MakeTryCatch(ILInstruction) + 0xc
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitTryFinally(TryFinally) + 0x19
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitTryCatch(TryCatch) + 0x45
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
			//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod, EntityDeclaration, DecompileRun, ITypeResolveContext, ExtensionInfo) + 0x426
			// -- continuing with outer exception (ICSharpCode.Decompiler.DecompilerException) --
			// 
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine param0)
		{
			_003C_003Et__builder.SetStateMachine(param0);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine param0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(param0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CStopGeolocatorAsync_003Ed__14 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public Service _003C_003E4__this;

		private TaskAwaiter _003C_003Eu___0024awaiter15;

		private object _003C_003Et__stack;

		private void MoveNext()
		{
			// ILSpy could not decompile this member: Error decompiling @06001510 Facebook.Messenger.WP8.FBGeoLocator.Service.<StopGeolocatorAsync>d__14.MoveNext: Value cannot be null. (Parameter 'source')
			// Please report the exception below,
			// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
			// Error decompiling @06001510 Facebook.Messenger.WP8.FBGeoLocator.Service.<StopGeolocatorAsync>d__14.MoveNext
			// in assembly "extracted/Facebook.Messenger.WP8.dll"
			//  ---> System.ArgumentNullException: Value cannot be null. (Parameter 'source')
			//    at System.Linq.ThrowHelper.ThrowArgumentNullException(ExceptionArgument) + 0x31
			//    at System.Linq.Enumerable.Take[TSource](IEnumerable`1, Int32) + 0x48
			//    at ICSharpCode.Decompiler.CSharp.ArgumentList.GetArgumentResolveResultsDirect(Int32) + 0x24
			//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Probe() + 0xc8
			//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Run() + 0x10
			//    at ICSharpCode.Decompiler.CSharp.Disambiguator.ForMethodReference(ExpressionBuilder, IMethod, IType, TranslatedExpression, Boolean, ExpectedTargetDetails, Boolean, Boolean) + 0x103
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.DisambiguateDelegateReference(IMethod, IMethod, ExpectedTargetDetails, ILInstruction) + 0x25f
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildDelegateReference(IMethod, IMethod, ExpectedTargetDetails, ILInstruction) + 0x53
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitLdFtn(LdFtn, TranslationContext) + 0x98
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.HandleDelegateConstruction(CallInstruction) + 0x139
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x9f
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitNewObj(NewObj, TranslationContext) + 0x150
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(OpCode, IMethod, IReadOnlyList`1, IReadOnlyList`1, IType) + 0x4e7
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x12f
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitCall(Call, TranslationContext) + 0x57
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Default(ILInstruction) + 0xa7
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlock(Block) + 0x51
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitIfInstruction(IfInstruction) + 0x3d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.TranslateFunction(IType, ILFunction) + 0x135
			//    at ICSharpCode.Decompiler.IL.ILFunction.AcceptVisitor[C,T](ILVisitor`2, C) + 0x32
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(OpCode, IMethod, IReadOnlyList`1, IReadOnlyList`1, IType) + 0x4e7
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x12f
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitCallVirt(CallVirt, TranslationContext) + 0x57
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.TranslateTarget(ILInstruction, Boolean, Boolean, IType, IType) + 0x21f
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(OpCode, IMethod, IReadOnlyList`1, IReadOnlyList`1, IType) + 0x259
			//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x12f
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitCallVirt(CallVirt, TranslationContext) + 0x57
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
			//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitStLoc(StLoc, TranslationContext) + 0x67
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitStLoc(StLoc) + 0xa9
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlock(Block) + 0x51
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitIfInstruction(IfInstruction) + 0x3d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitTryCatch(TryCatch) + 0x45
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
			//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
			//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod, EntityDeclaration, DecompileRun, ITypeResolveContext, ExtensionInfo) + 0x426
			// -- continuing with outer exception (ICSharpCode.Decompiler.DecompilerException) --
			// 
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine param0)
		{
			_003C_003Et__builder.SetStateMachine(param0);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine param0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(param0);
		}
	}

	private const string ThreadsLocationSettingsKey = "ThreadsLocationSettings";

	private const string ThreadsLocationVersionKey = "ThreadsLocationVersion";

	private const string ThreadsLocationVersion = "1.0";

	private const int StaleLocationThreshold = 10;

	private readonly FacebookContext _context;

	private Geolocator _geolocator;

	private ReverseGeocodeQuery _geocodeQuery;

	private CancellationTokenSource _cancellationTokenSource;

	private GeolocatorStatus _geolocatorStatus;

	private FacebookPlace _currentLocation;

	private bool _deviceLocationEnabled;

	private DateTime _lastKnownGoodLocationTime;

	private readonly TimeSpan _staleLocationTime = new TimeSpan(0, 10, 0);

	private Timer _refetchLocationTimer;

	private List<string> _threadsLocationSettings;

	private readonly object _threadsLocationSettingsLock = new object();

	private readonly SemaphoreSlim _geocodeQueryLock = new SemaphoreSlim(1, 1);

	private readonly DisposableMutex _geolocatorLock = new DisposableMutex();

	public event EventHandler GeolocatorStatusChanged;

	public Service(IDispatcher ui, FacebookContext context)
		: base(ui)
	{
		_context = context;
	}

	public override void StartService(FacebookContext context)
	{
		StartGeolocatorAsync().DoNotWait();
		_context.LoggedIn += ContextOnLoggedIn;
		_context.LoggedOut += ContextOnLoggedOut;
	}

	public override void StopService()
	{
		StopGeolocatorAsync().DoNotWait();
		SaveThreadsLocationSettings();
		_context.LoggedIn -= ContextOnLoggedIn;
		_context.LoggedOut -= ContextOnLoggedOut;
	}

	private void ContextOnLoggedIn(object sender, FacebookSession facebookSession)
	{
		StartGeolocatorAsync().DoNotWait();
	}

	private void ContextOnLoggedOut(object sender, LogoutEventArgs logoutEventArgs)
	{
		StopGeolocatorAsync().DoNotWait();
	}

	public bool IsDeviceLocationEnabled()
	{
		return _deviceLocationEnabled;
	}

	public bool IsAppLocationEnabled()
	{
		if (_context.IsAuthenticated())
		{
			return _context.IsLocationEnabled();
		}
		return false;
	}

	public bool IsLocationEnabled()
	{
		if (_deviceLocationEnabled)
		{
			return IsAppLocationEnabled();
		}
		return false;
	}

	public bool IsLocationEnabledForThread(string settingsId)
	{
		if (!IsLocationEnabled())
		{
			return false;
		}
		if (settingsId.IsNullOrEmpty())
		{
			return true;
		}
		List<string> threadsLocationSettings = GetThreadsLocationSettings();
		return !threadsLocationSettings.Contains(settingsId);
	}

	public async Task<bool> SetLocationEnabledAsync(bool enabled)
	{
		if (!enabled)
		{
			await DisableLocationAsync();
		}
		else
		{
			await EnableLocationAsync();
		}
		return true;
	}

	public bool SetLocationForThread(bool enable, string settingsId)
	{
		if (!IsLocationEnabled())
		{
			return false;
		}
		if (enable)
		{
			EnableLocationForThread(settingsId);
		}
		else
		{
			DisableLocationForThread(settingsId);
		}
		return true;
	}

	public FacebookPlace GetCurrentLocation()
	{
		TimeSpan timeSpan = DateTime.UtcNow - _lastKnownGoodLocationTime;
		if (timeSpan > _staleLocationTime || _currentLocation == null || !_currentLocation.IsValidLocation())
		{
			InvalidateAndRefetchLocation();
		}
		return _currentLocation;
	}

	private bool GetDeviceLocationStatusEnabled()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		Geolocator val = new Geolocator();
		return (int)val.LocationStatus != 3;
	}

	private async Task EnableLocationAsync()
	{
		_context.SetLocationEnabled(value: true);
		await StartGeolocatorAsync();
	}

	private async Task DisableLocationAsync()
	{
		_context.SetLocationEnabled(value: false);
		await StopGeolocatorAsync();
	}

	private void EnableLocationForThread(string threadId)
	{
		lock (_threadsLocationSettingsLock)
		{
			List<string> threadsLocationSettings = GetThreadsLocationSettings();
			if (threadsLocationSettings.Contains(threadId))
			{
				threadsLocationSettings.Remove(threadId);
			}
		}
	}

	private void DisableLocationForThread(string threadId)
	{
		lock (_threadsLocationSettingsLock)
		{
			List<string> threadsLocationSettings = GetThreadsLocationSettings();
			if (!threadsLocationSettings.Contains(threadId))
			{
				threadsLocationSettings.Add(threadId);
			}
		}
	}

	private List<string> GetThreadsLocationSettings()
	{
		if (_threadsLocationSettings == null)
		{
			string setting = _context.Settings.GetSetting<string>("ThreadsLocationSettings");
			_threadsLocationSettings = ((!string.IsNullOrWhiteSpace(setting)) ? JsonConvert.DeserializeObject<List<string>>(setting) : new List<string>());
		}
		return _threadsLocationSettings;
	}

	private void SaveThreadsLocationSettings()
	{
		if (!_context.IsAuthenticated())
		{
			return;
		}
		if (_threadsLocationSettings == null || !_threadsLocationSettings.Any())
		{
			_context.Settings.RemoveSetting("ThreadsLocationSettings");
			_context.Settings.RemoveSetting("ThreadsLocationVersion");
			return;
		}
		StringWriter stringWriter = new StringWriter();
		JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
		jsonTextWriter.WriteStartArray();
		foreach (string threadsLocationSetting in _threadsLocationSettings)
		{
			jsonTextWriter.WriteValue(threadsLocationSetting);
		}
		jsonTextWriter.WriteEndArray();
		_context.Settings.SaveSetting("ThreadsLocationSettings", stringWriter.ToString());
		_context.Settings.SaveSetting("ThreadsLocationVersion", "1.0");
	}

	private void OnGeolocatorStatusChanged()
	{
		UiDispatcher.RunAsync(() =>
		{
			if (GeolocatorStatusChanged != null)
			{
				GeolocatorStatusChanged(this, null);
			}
		}, DispatcherPriority.Normal);
	}

	private bool CanStartGeolocator()
	{
		_deviceLocationEnabled = GetDeviceLocationStatusEnabled();
		return IsLocationEnabled();
	}

	[AsyncStateMachine(typeof(_003CStartGeolocatorAsync_003Ed__d))]
	[DebuggerStepThrough]
	private Task StartGeolocatorAsync()
	{
		// ILSpy could not decompile this member: Error decompiling @060004B8 Facebook.Messenger.WP8.FBGeoLocator.Service.StartGeolocatorAsync: Value cannot be null. (Parameter 'source')
		// Please report the exception below,
		// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
		// Error decompiling @060004B8 Facebook.Messenger.WP8.FBGeoLocator.Service.StartGeolocatorAsync
		// in assembly "extracted/Facebook.Messenger.WP8.dll"
		//  ---> System.ArgumentNullException: Value cannot be null. (Parameter 'source')
		//    at System.Linq.ThrowHelper.ThrowArgumentNullException(ExceptionArgument) + 0x31
		//    at System.Linq.Enumerable.Take[TSource](IEnumerable`1, Int32) + 0x48
		//    at ICSharpCode.Decompiler.CSharp.ArgumentList.GetArgumentResolveResultsDirect(Int32) + 0x24
		//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Probe() + 0xc8
		//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Run() + 0x10
		//    at ICSharpCode.Decompiler.CSharp.Disambiguator.ForMethodReference(ExpressionBuilder, IMethod, IType, TranslatedExpression, Boolean, ExpectedTargetDetails, Boolean, Boolean) + 0x103
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.DisambiguateDelegateReference(IMethod, IMethod, ExpectedTargetDetails, ILInstruction) + 0x25f
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildDelegateReference(IMethod, IMethod, ExpectedTargetDetails, ILInstruction) + 0x53
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitLdFtn(LdFtn, TranslationContext) + 0x98
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.HandleDelegateConstruction(CallInstruction) + 0x139
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x9f
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitNewObj(NewObj, TranslationContext) + 0x150
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(OpCode, IMethod, IReadOnlyList`1, IReadOnlyList`1, IType) + 0x4e7
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x12f
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitCall(Call, TranslationContext) + 0x57
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Default(ILInstruction) + 0xa7
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlock(Block) + 0x51
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitIfInstruction(IfInstruction) + 0x3d
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitUsingInstruction(UsingInstruction) + 0x39e
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
		//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod, EntityDeclaration, DecompileRun, ITypeResolveContext, ExtensionInfo) + 0x426
		// -- continuing with outer exception (ICSharpCode.Decompiler.DecompilerException) --
		// 
	}

	[DebuggerStepThrough]
	[AsyncStateMachine(typeof(_003CStopGeolocatorAsync_003Ed__14))]
	private Task StopGeolocatorAsync()
	{
		// ILSpy could not decompile this member: Error decompiling @060004B9 Facebook.Messenger.WP8.FBGeoLocator.Service.StopGeolocatorAsync: Value cannot be null. (Parameter 'source')
		// Please report the exception below,
		// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
		// Error decompiling @060004B9 Facebook.Messenger.WP8.FBGeoLocator.Service.StopGeolocatorAsync
		// in assembly "extracted/Facebook.Messenger.WP8.dll"
		//  ---> System.ArgumentNullException: Value cannot be null. (Parameter 'source')
		//    at System.Linq.ThrowHelper.ThrowArgumentNullException(ExceptionArgument) + 0x31
		//    at System.Linq.Enumerable.Take[TSource](IEnumerable`1, Int32) + 0x48
		//    at ICSharpCode.Decompiler.CSharp.ArgumentList.GetArgumentResolveResultsDirect(Int32) + 0x24
		//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Probe() + 0xc8
		//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Run() + 0x10
		//    at ICSharpCode.Decompiler.CSharp.Disambiguator.ForMethodReference(ExpressionBuilder, IMethod, IType, TranslatedExpression, Boolean, ExpectedTargetDetails, Boolean, Boolean) + 0x103
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.DisambiguateDelegateReference(IMethod, IMethod, ExpectedTargetDetails, ILInstruction) + 0x25f
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildDelegateReference(IMethod, IMethod, ExpectedTargetDetails, ILInstruction) + 0x53
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitLdFtn(LdFtn, TranslationContext) + 0x98
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.HandleDelegateConstruction(CallInstruction) + 0x139
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x9f
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitNewObj(NewObj, TranslationContext) + 0x150
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(OpCode, IMethod, IReadOnlyList`1, IReadOnlyList`1, IType) + 0x4e7
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x12f
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitCall(Call, TranslationContext) + 0x57
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Default(ILInstruction) + 0xa7
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlock(Block) + 0x51
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitIfInstruction(IfInstruction) + 0x3d
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.TranslateFunction(IType, ILFunction) + 0x135
		//    at ICSharpCode.Decompiler.IL.ILFunction.AcceptVisitor[C,T](ILVisitor`2, C) + 0x32
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails, ResolveResult, IMethod, Int32, IReadOnlyList`1, IReadOnlyList`1, Boolean) + 0x38f
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(OpCode, IMethod, IReadOnlyList`1, IReadOnlyList`1, IType) + 0x4e7
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction, IType) + 0x12f
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitCallVirt(CallVirt, TranslationContext) + 0x57
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction, IType) + 0xa7
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitAwait(Await, TranslationContext) + 0xb8
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Default(ILInstruction) + 0xa7
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
		//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod, EntityDeclaration, DecompileRun, ITypeResolveContext, ExtensionInfo) + 0x426
		// -- continuing with outer exception (ICSharpCode.Decompiler.DecompilerException) --
		// 
	}

	private async Task PerformWithGeolocatorLockAsync(Action action)
	{
		using (await _geolocatorLock.WaitAsync())
		{
			action();
		}
	}

	private void ReleaseGeocodeQueryLock()
	{
		if (_geocodeQueryLock.CurrentCount == 0)
		{
			_geocodeQueryLock.Release();
		}
	}

	private void InvalidateAndRefetchLocation()
	{
		_currentLocation = null;
		_geolocatorStatus = GeolocatorStatus.Invalid;
		StartGeolocatorAsync().DoNotWait();
	}

	private void GeolocatorOnStatusChanged(Geolocator sender, StatusChangedEventArgs args)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if ((int)args.Status == 3)
		{
			_deviceLocationEnabled = false;
		}
		else if ((int)args.Status == 0)
		{
			_deviceLocationEnabled = true;
		}
		OnGeolocatorStatusChanged();
	}

	private async void GeolocatorOnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
	{
		if (args.Position == null)
		{
			return;
		}
		await PerformWithGeolocatorLockAsync(() =>
		{
			if (_geolocatorStatus == GeolocatorStatus.Ready)
			{
				UpdateCurrentLocationWithGeoposition(args.Position);
				ReverseGeocodeWithGeoposition(args.Position);
			}
		});
	}

	private async void GeocodeQueryOnQueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> args)
	{
		ReleaseGeocodeQueryLock();
		if (((AsyncCompletedEventArgs)(object)args).Error != null || args.Result == null || !args.Result.Any())
		{
			return;
		}
		MapLocation firstResult = args.Result.FirstOrDefault();
		if (firstResult == null || firstResult.Information == null || firstResult.Information.Address == null)
		{
			return;
		}
		MapAddress address = firstResult.Information.Address;
		await PerformWithGeolocatorLockAsync(() =>
		{
			if (_geolocatorStatus == GeolocatorStatus.Ready)
			{
				UpdateCurrentLocationWithMapAddress(address);
			}
		});
	}

	private void UpdateCurrentLocationWithGeoposition(Geoposition position)
	{
		if (position != null && position.Coordinate != null)
		{
			if (_currentLocation == null)
			{
				_currentLocation = new FacebookPlace();
			}
			_currentLocation.Name = ((position.CivicAddress != null) ? position.CivicAddress.City : string.Empty);
			_currentLocation.Latitude = position.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture);
			_currentLocation.Longitude = position.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture);
			_lastKnownGoodLocationTime = DateTime.UtcNow;
		}
	}

	private void UpdateCurrentLocationWithMapAddress(MapAddress address)
	{
		if (_currentLocation != null && address != null)
		{
			_currentLocation.CountryCode = address.CountryCode;
			_currentLocation.State = address.State;
			_currentLocation.Name = address.City;
		}
	}

	private void ReverseGeocodeWithGeoposition(Geoposition position)
	{
		UiDispatcher.RunAsync(async () =>
		{
			await _geocodeQueryLock.WaitAsync();
			if (_geolocatorStatus != GeolocatorStatus.Ready)
			{
				return;
			}
			if (_geocodeQuery == null)
			{
				_geocodeQuery = new ReverseGeocodeQuery();
				((Query<IList<MapLocation>>)(object)_geocodeQuery).QueryCompleted += GeocodeQueryOnQueryCompleted;
			}
			else if (((Query<IList<MapLocation>>)(object)_geocodeQuery).IsBusy)
			{
				return;
			}
			GeoCoordinate coordinate = new GeoCoordinate(position.Coordinate.Latitude, position.Coordinate.Longitude, position.Coordinate.Altitude ?? double.NaN, position.Coordinate.Accuracy, position.Coordinate.AltitudeAccuracy ?? double.NaN, position.Coordinate.Speed ?? double.NaN, position.Coordinate.Heading ?? double.NaN);
			_geocodeQuery.GeoCoordinate = coordinate;
			try
			{
				((Query<IList<MapLocation>>)(object)_geocodeQuery).QueryAsync();
			}
			catch (COMException ex)
			{
				uint errorCode = (uint)ex.ErrorCode;
				if (errorCode == 2147778561u)
				{
					Logger.Error(this, ex.ToString(), "ReverseGeocodeWithGeoposition", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\FBGeoLocator\\Service.cs", 568);
					return;
				}
				throw;
			}
		});
	}

	private void StartRefetchLocationTimer()
	{
		StopRefetchLocationTimer();
		int num = Convert.ToInt32(TimeSpan.FromMinutes(9.0).TotalMilliseconds);
		_refetchLocationTimer = new Timer(RefetchLocationTimerCallback, null, num, num);
	}

	private void StopRefetchLocationTimer()
	{
		if (_refetchLocationTimer != null)
		{
			_refetchLocationTimer.Dispose();
			_refetchLocationTimer = null;
		}
	}

	private void RefetchLocationTimerCallback(object state)
	{
		if (_refetchLocationTimer != null)
		{
			InvalidateAndRefetchLocation();
		}
	}
}
