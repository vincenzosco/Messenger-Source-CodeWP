// IL code: Facebook.Messenger.WP8
.class public auto ansi beforefieldinit Facebook.Messenger.WP8.FBGeoLocator.Service
	extends ['Facebook-Services']Facebook.Services.BackgroundService
{
	// Nested Types
	.class nested public auto ansi sealed GeolocatorStatus
		extends [mscorlib]System.Enum
	{
		// Fields
		.field public specialname rtspecialname int32 value__
		.field public static literal valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus NotAvailable = int32(0)
		.field public static literal valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus Invalid = int32(1)
		.field public static literal valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus Ready = int32(2)

	} // end of class GeolocatorStatus

	.class nested private auto ansi sealed beforefieldinit '<SetLocationEnabledAsync>d__0'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool> '<>t__builder'
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field public bool enabled
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter1'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x1bb44
			// Header size: 12
			// Code size: 296 (0x128)
			.maxstack 3
			.locals init (
				[0] bool,
				[1] bool,
				[2] class [mscorlib]System.Exception,
				[3] int32,
				[4] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[5] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[6] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[7] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.0
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>1__state'
				IL_0008: stloc.3
				IL_0009: ldloc.3
				IL_000a: switch (IL_005e, IL_00ca)

				IL_0017: ldarg.0
				IL_0018: ldfld bool Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::enabled
				IL_001d: brfalse.s IL_008e

				IL_001f: ldarg.0
				IL_0020: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>4__this'
				IL_0025: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::EnableLocationAsync()
				IL_002a: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_002f: stloc.s 4
				IL_0031: ldloca.s 4
				IL_0033: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_0038: brtrue.s IL_007d

				IL_003a: ldarg.0
				IL_003b: ldc.i4.0
				IL_003c: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>1__state'
				IL_0041: ldarg.0
				IL_0042: ldloc.s 4
				IL_0044: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>u__$awaiter1'
				IL_0049: ldarg.0
				IL_004a: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool> Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>t__builder'
				IL_004f: ldloca.s 4
				IL_0051: ldarg.0
				IL_0052: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool>::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'>(!!0&, !!1&)
				IL_0057: ldc.i4.0
				IL_0058: stloc.0
				IL_0059: leave IL_0127

				IL_005e: ldarg.0
				IL_005f: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>u__$awaiter1'
				IL_0064: stloc.s 4
				IL_0066: ldarg.0
				IL_0067: ldloca.s 5
				IL_0069: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_006f: ldloc.s 5
				IL_0071: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>u__$awaiter1'
				IL_0076: ldarg.0
				IL_0077: ldc.i4.m1
				IL_0078: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>1__state'

				IL_007d: ldloca.s 4
				IL_007f: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_0084: ldloca.s 4
				IL_0086: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_008c: br.s IL_00f8

				IL_008e: ldarg.0
				IL_008f: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>4__this'
				IL_0094: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::DisableLocationAsync()
				IL_0099: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_009e: stloc.s 6
				IL_00a0: ldloca.s 6
				IL_00a2: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_00a7: brtrue.s IL_00e9

				IL_00a9: ldarg.0
				IL_00aa: ldc.i4.1
				IL_00ab: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>1__state'
				IL_00b0: ldarg.0
				IL_00b1: ldloc.s 6
				IL_00b3: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>u__$awaiter1'
				IL_00b8: ldarg.0
				IL_00b9: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool> Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>t__builder'
				IL_00be: ldloca.s 6
				IL_00c0: ldarg.0
				IL_00c1: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool>::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'>(!!0&, !!1&)
				IL_00c6: ldc.i4.0
				IL_00c7: stloc.0
				IL_00c8: leave.s IL_0127

				IL_00ca: ldarg.0
				IL_00cb: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>u__$awaiter1'
				IL_00d0: stloc.s 6
				IL_00d2: ldarg.0
				IL_00d3: ldloca.s 7
				IL_00d5: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_00db: ldloc.s 7
				IL_00dd: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>u__$awaiter1'
				IL_00e2: ldarg.0
				IL_00e3: ldc.i4.m1
				IL_00e4: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>1__state'

				IL_00e9: ldloca.s 6
				IL_00eb: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_00f0: ldloca.s 6
				IL_00f2: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter

				IL_00f8: ldc.i4.1
				IL_00f9: stloc.1
				IL_00fa: leave.s IL_0113
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_00fc: stloc.2
				IL_00fd: ldarg.0
				IL_00fe: ldc.i4.s -2
				IL_0100: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>1__state'
				IL_0105: ldarg.0
				IL_0106: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool> Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>t__builder'
				IL_010b: ldloc.2
				IL_010c: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool>::SetException(class [mscorlib]System.Exception)
				IL_0111: leave.s IL_0127
			} // end handler

			IL_0113: ldarg.0
			IL_0114: ldc.i4.s -2
			IL_0116: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>1__state'
			IL_011b: ldarg.0
			IL_011c: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool> Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>t__builder'
			IL_0121: ldloc.1
			IL_0122: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool>::SetResult(!0)

			IL_0127: ret
		} // end of method '<SetLocationEnabledAsync>d__0'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x1bc88
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool> Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool>::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<SetLocationEnabledAsync>d__0'::SetStateMachine

	} // end of class <SetLocationEnabledAsync>d__0

	.class nested private auto ansi sealed beforefieldinit '<EnableLocationAsync>d__3'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter4'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x1bd78
			// Header size: 12
			// Code size: 178 (0xb2)
			.maxstack 3
			.locals init (
				[0] bool,
				[1] class [mscorlib]System.Exception,
				[2] int32,
				[3] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[4] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.0
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>1__state'
				IL_0008: stloc.2
				IL_0009: ldloc.2
				IL_000a: ldc.i4.0
				IL_000b: beq.s IL_0058

				IL_000d: ldarg.0
				IL_000e: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>4__this'
				IL_0013: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
				IL_0018: ldc.i4.1
				IL_0019: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookContext::SetLocationEnabled(bool)
				IL_001e: ldarg.0
				IL_001f: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>4__this'
				IL_0024: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::StartGeolocatorAsync()
				IL_0029: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_002e: stloc.3
				IL_002f: ldloca.s 3
				IL_0031: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_0036: brtrue.s IL_0076

				IL_0038: ldarg.0
				IL_0039: ldc.i4.0
				IL_003a: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>1__state'
				IL_003f: ldarg.0
				IL_0040: ldloc.3
				IL_0041: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>u__$awaiter4'
				IL_0046: ldarg.0
				IL_0047: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>t__builder'
				IL_004c: ldloca.s 3
				IL_004e: ldarg.0
				IL_004f: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'>(!!0&, !!1&)
				IL_0054: ldc.i4.0
				IL_0055: stloc.0
				IL_0056: leave.s IL_00b1

				IL_0058: ldarg.0
				IL_0059: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>u__$awaiter4'
				IL_005e: stloc.3
				IL_005f: ldarg.0
				IL_0060: ldloca.s 4
				IL_0062: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_0068: ldloc.s 4
				IL_006a: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>u__$awaiter4'
				IL_006f: ldarg.0
				IL_0070: ldc.i4.m1
				IL_0071: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>1__state'

				IL_0076: ldloca.s 3
				IL_0078: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_007d: ldloca.s 3
				IL_007f: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_0085: leave.s IL_009e
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_0087: stloc.1
				IL_0088: ldarg.0
				IL_0089: ldc.i4.s -2
				IL_008b: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>1__state'
				IL_0090: ldarg.0
				IL_0091: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>t__builder'
				IL_0096: ldloc.1
				IL_0097: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_009c: leave.s IL_00b1
			} // end handler

			IL_009e: ldarg.0
			IL_009f: ldc.i4.s -2
			IL_00a1: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>1__state'
			IL_00a6: ldarg.0
			IL_00a7: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>t__builder'
			IL_00ac: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_00b1: ret
		} // end of method '<EnableLocationAsync>d__3'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x1be48
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<EnableLocationAsync>d__3'::SetStateMachine

	} // end of class <EnableLocationAsync>d__3

	.class nested private auto ansi sealed beforefieldinit '<DisableLocationAsync>d__6'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter7'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x1bea0
			// Header size: 12
			// Code size: 178 (0xb2)
			.maxstack 3
			.locals init (
				[0] bool,
				[1] class [mscorlib]System.Exception,
				[2] int32,
				[3] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[4] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.0
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>1__state'
				IL_0008: stloc.2
				IL_0009: ldloc.2
				IL_000a: ldc.i4.0
				IL_000b: beq.s IL_0058

				IL_000d: ldarg.0
				IL_000e: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>4__this'
				IL_0013: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
				IL_0018: ldc.i4.0
				IL_0019: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookContext::SetLocationEnabled(bool)
				IL_001e: ldarg.0
				IL_001f: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>4__this'
				IL_0024: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::StopGeolocatorAsync()
				IL_0029: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_002e: stloc.3
				IL_002f: ldloca.s 3
				IL_0031: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_0036: brtrue.s IL_0076

				IL_0038: ldarg.0
				IL_0039: ldc.i4.0
				IL_003a: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>1__state'
				IL_003f: ldarg.0
				IL_0040: ldloc.3
				IL_0041: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>u__$awaiter7'
				IL_0046: ldarg.0
				IL_0047: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>t__builder'
				IL_004c: ldloca.s 3
				IL_004e: ldarg.0
				IL_004f: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'>(!!0&, !!1&)
				IL_0054: ldc.i4.0
				IL_0055: stloc.0
				IL_0056: leave.s IL_00b1

				IL_0058: ldarg.0
				IL_0059: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>u__$awaiter7'
				IL_005e: stloc.3
				IL_005f: ldarg.0
				IL_0060: ldloca.s 4
				IL_0062: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_0068: ldloc.s 4
				IL_006a: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>u__$awaiter7'
				IL_006f: ldarg.0
				IL_0070: ldc.i4.m1
				IL_0071: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>1__state'

				IL_0076: ldloca.s 3
				IL_0078: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_007d: ldloca.s 3
				IL_007f: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_0085: leave.s IL_009e
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_0087: stloc.1
				IL_0088: ldarg.0
				IL_0089: ldc.i4.s -2
				IL_008b: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>1__state'
				IL_0090: ldarg.0
				IL_0091: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>t__builder'
				IL_0096: ldloc.1
				IL_0097: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_009c: leave.s IL_00b1
			} // end handler

			IL_009e: ldarg.0
			IL_009f: ldc.i4.s -2
			IL_00a1: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>1__state'
			IL_00a6: ldarg.0
			IL_00a7: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>t__builder'
			IL_00ac: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_00b1: ret
		} // end of method '<DisableLocationAsync>d__6'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x1bf70
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<DisableLocationAsync>d__6'::SetStateMachine

	} // end of class <DisableLocationAsync>d__6

	.class nested private auto ansi sealed beforefieldinit '<StartGeolocatorAsync>d__d'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field public class [Windows]Windows.Devices.Geolocation.Geoposition '<position>5__e'
		.field public class [mscorlib]System.IDisposable '<>7__wrapf'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable> '<>u__$awaiter10'
		.field private object '<>t__stack'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition> '<>u__$awaiter11'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x1c1f4
			// Header size: 12
			// Code size: 894 (0x37e)
			.maxstack 4
			.locals init (
				[0] class [Windows]Windows.Devices.Geolocation.Geolocator,
				[1] class [mscorlib]System.UnauthorizedAccessException,
				[2] class [mscorlib]System.Threading.Tasks.TaskCanceledException,
				[3] class [mscorlib]System.Exception,
				[4] bool,
				[5] class [mscorlib]System.Exception,
				[6] int32,
				[7] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>,
				[8] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>,
				[9] class [mscorlib]System.IDisposable,
				[10] int32,
				[11] class [Windows]Windows.Devices.Geolocation.Geolocator,
				[12] class [Windows]Windows.Devices.Geolocation.Geolocator,
				[13] int32,
				[14] int32,
				[15] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition>,
				[16] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition>,
				[17] class [Windows]Windows.Devices.Geolocation.Geoposition,
				[18] uint32
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.s 4
				IL_0003: ldarg.0
				IL_0004: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'
				IL_0009: stloc.s 6
				IL_000b: ldloc.s 6
				IL_000d: switch (IL_007c, IL_00b4)

				IL_001a: ldarg.0
				IL_001b: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
				IL_0020: callvirt instance bool Facebook.Messenger.WP8.FBGeoLocator.Service::CanStartGeolocator()
				IL_0025: brtrue.s IL_0037

				IL_0027: ldarg.0
				IL_0028: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
				IL_002d: callvirt instance void Facebook.Messenger.WP8.FBGeoLocator.Service::OnGeolocatorStatusChanged()
				IL_0032: leave IL_036a

				IL_0037: ldarg.0
				IL_0038: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
				IL_003d: ldfld class ['Facebook-Base']Facebook.Base.DisposableMutex Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorLock
				IL_0042: callvirt instance class [mscorlib]System.Threading.Tasks.Task`1<class [mscorlib]System.IDisposable> ['Facebook-Base']Facebook.Base.DisposableMutex::WaitAsync()
				IL_0047: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!0> class [mscorlib]System.Threading.Tasks.Task`1<class [mscorlib]System.IDisposable>::GetAwaiter()
				IL_004c: stloc.s 7
				IL_004e: ldloca.s 7
				IL_0050: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>::get_IsCompleted()
				IL_0055: brtrue.s IL_009b

				IL_0057: ldarg.0
				IL_0058: ldc.i4.0
				IL_0059: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'
				IL_005e: ldarg.0
				IL_005f: ldloc.s 7
				IL_0061: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable> Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>u__$awaiter10'
				IL_0066: ldarg.0
				IL_0067: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>t__builder'
				IL_006c: ldloca.s 7
				IL_006e: ldarg.0
				IL_006f: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'>(!!0&, !!1&)
				IL_0074: ldc.i4.0
				IL_0075: stloc.s 4
				IL_0077: leave IL_037d

				IL_007c: ldarg.0
				IL_007d: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable> Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>u__$awaiter10'
				IL_0082: stloc.s 7
				IL_0084: ldarg.0
				IL_0085: ldloca.s 8
				IL_0087: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>
				IL_008d: ldloc.s 8
				IL_008f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable> Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>u__$awaiter10'
				IL_0094: ldarg.0
				IL_0095: ldc.i4.m1
				IL_0096: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'

				IL_009b: ldloca.s 7
				IL_009d: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>::GetResult()
				IL_00a2: ldloca.s 7
				IL_00a4: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>
				IL_00aa: stloc.s 9
				IL_00ac: ldarg.0
				IL_00ad: ldloc.s 9
				IL_00af: stfld class [mscorlib]System.IDisposable Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>7__wrapf'
				.try
				{
					IL_00b4: ldarg.0
					IL_00b5: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'
					IL_00ba: stloc.s 10
					IL_00bc: ldloc.s 10
					IL_00be: ldc.i4.1
					IL_00bf: beq IL_01a9

					IL_00c4: ldarg.0
					IL_00c5: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_00ca: ldfld valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorStatus
					IL_00cf: ldc.i4.2
					IL_00d0: bne.un.s IL_00d7

					IL_00d2: leave IL_036a

					IL_00d7: ldarg.0
					IL_00d8: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_00dd: callvirt instance void Facebook.Messenger.WP8.FBGeoLocator.Service::StopRefetchLocationTimer()
					IL_00e2: ldarg.0
					IL_00e3: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_00e8: ldfld class [Windows]Windows.Devices.Geolocation.Geolocator Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocator
					IL_00ed: brtrue IL_018d

					IL_00f2: ldarg.0
					IL_00f3: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_00f8: newobj instance void [Windows]Windows.Devices.Geolocation.Geolocator::.ctor()
					IL_00fd: stloc.0
					IL_00fe: ldloc.0
					IL_00ff: ldc.r8 1
					IL_0108: callvirt instance void [Windows]Windows.Devices.Geolocation.Geolocator::put_MovementThreshold(float64)
					IL_010d: ldloc.0
					IL_010e: stfld class [Windows]Windows.Devices.Geolocation.Geolocator Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocator
					IL_0113: ldarg.0
					IL_0114: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_0119: ldfld class [Windows]Windows.Devices.Geolocation.Geolocator Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocator
					IL_011e: stloc.s 11
					IL_0120: ldloc.s 11
					IL_0122: ldftn instance valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken [Windows]Windows.Devices.Geolocation.Geolocator::add_PositionChanged(class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs>)
					IL_0128: newobj instance void class [mscorlib]System.Func`2<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs>, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
					IL_012d: ldloc.s 11
					IL_012f: ldftn instance void [Windows]Windows.Devices.Geolocation.Geolocator::remove_PositionChanged(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
					IL_0135: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
					IL_013a: ldarg.0
					IL_013b: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_0140: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorOnPositionChanged(class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs)
					IL_0146: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs>::.ctor(object, native int)
					IL_014b: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::AddEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs>>(class [mscorlib]System.Func`2<!!0, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
					IL_0150: ldarg.0
					IL_0151: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_0156: ldfld class [Windows]Windows.Devices.Geolocation.Geolocator Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocator
					IL_015b: stloc.s 12
					IL_015d: ldloc.s 12
					IL_015f: ldftn instance valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken [Windows]Windows.Devices.Geolocation.Geolocator::add_StatusChanged(class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs>)
					IL_0165: newobj instance void class [mscorlib]System.Func`2<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs>, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
					IL_016a: ldloc.s 12
					IL_016c: ldftn instance void [Windows]Windows.Devices.Geolocation.Geolocator::remove_StatusChanged(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
					IL_0172: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
					IL_0177: ldarg.0
					IL_0178: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_017d: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorOnStatusChanged(class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs)
					IL_0183: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs>::.ctor(object, native int)
					IL_0188: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::AddEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs>>(class [mscorlib]System.Func`2<!!0, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)

					IL_018d: ldarg.0
					IL_018e: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_0193: ldc.i4.2
					IL_0194: stfld valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorStatus
					IL_0199: ldarg.0
					IL_019a: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
					IL_019f: newobj instance void [mscorlib]System.Threading.CancellationTokenSource::.ctor()
					IL_01a4: stfld class [mscorlib]System.Threading.CancellationTokenSource Facebook.Messenger.WP8.FBGeoLocator.Service::_cancellationTokenSource
					.try
					{
						IL_01a9: ldarg.0
						IL_01aa: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'
						IL_01af: stloc.s 13
						IL_01b1: ldloc.s 13
						IL_01b3: ldc.i4.1
						IL_01b4: pop
						IL_01b5: pop
						.try
						{
							IL_01b6: ldarg.0
							IL_01b7: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'
							IL_01bc: stloc.s 14
							IL_01be: ldloc.s 14
							IL_01c0: ldc.i4.1
							IL_01c1: beq.s IL_021d

							IL_01c3: ldarg.0
							IL_01c4: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
							IL_01c9: ldfld class [Windows]Windows.Devices.Geolocation.Geolocator Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocator
							IL_01ce: callvirt instance class [Windows]Windows.Foundation.IAsyncOperation`1<class [Windows]Windows.Devices.Geolocation.Geoposition> [Windows]Windows.Devices.Geolocation.Geolocator::GetGeopositionAsync()
							IL_01d3: ldarg.0
							IL_01d4: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
							IL_01d9: ldfld class [mscorlib]System.Threading.CancellationTokenSource Facebook.Messenger.WP8.FBGeoLocator.Service::_cancellationTokenSource
							IL_01de: callvirt instance valuetype [mscorlib]System.Threading.CancellationToken [mscorlib]System.Threading.CancellationTokenSource::get_Token()
							IL_01e3: call class [mscorlib]System.Threading.Tasks.Task`1<!!0> [System.Runtime.WindowsRuntime]System.WindowsRuntimeSystemExtensions::AsTask<class [Windows]Windows.Devices.Geolocation.Geoposition>(class [Windows]Windows.Foundation.IAsyncOperation`1<!!0>, valuetype [mscorlib]System.Threading.CancellationToken)
							IL_01e8: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!0> class [mscorlib]System.Threading.Tasks.Task`1<class [Windows]Windows.Devices.Geolocation.Geoposition>::GetAwaiter()
							IL_01ed: stloc.s 15
							IL_01ef: ldloca.s 15
							IL_01f1: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition>::get_IsCompleted()
							IL_01f6: brtrue.s IL_023c

							IL_01f8: ldarg.0
							IL_01f9: ldc.i4.1
							IL_01fa: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'
							IL_01ff: ldarg.0
							IL_0200: ldloc.s 15
							IL_0202: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition> Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>u__$awaiter11'
							IL_0207: ldarg.0
							IL_0208: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>t__builder'
							IL_020d: ldloca.s 15
							IL_020f: ldarg.0
							IL_0210: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition>, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'>(!!0&, !!1&)
							IL_0215: ldc.i4.0
							IL_0216: stloc.s 4
							IL_0218: leave IL_037d

							IL_021d: ldarg.0
							IL_021e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition> Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>u__$awaiter11'
							IL_0223: stloc.s 15
							IL_0225: ldarg.0
							IL_0226: ldloca.s 16
							IL_0228: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition>
							IL_022e: ldloc.s 16
							IL_0230: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition> Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>u__$awaiter11'
							IL_0235: ldarg.0
							IL_0236: ldc.i4.m1
							IL_0237: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'

							IL_023c: ldloca.s 15
							IL_023e: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition>::GetResult()
							IL_0243: ldloca.s 15
							IL_0245: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Devices.Geolocation.Geoposition>
							IL_024b: stloc.s 17
							IL_024d: ldarg.0
							IL_024e: ldloc.s 17
							IL_0250: stfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<position>5__e'
							IL_0255: ldarg.0
							IL_0256: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
							IL_025b: ldarg.0
							IL_025c: ldfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<position>5__e'
							IL_0261: callvirt instance void Facebook.Messenger.WP8.FBGeoLocator.Service::UpdateCurrentLocationWithGeoposition(class [Windows]Windows.Devices.Geolocation.Geoposition)
							IL_0266: ldarg.0
							IL_0267: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
							IL_026c: ldarg.0
							IL_026d: ldfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<position>5__e'
							IL_0272: callvirt instance void Facebook.Messenger.WP8.FBGeoLocator.Service::ReverseGeocodeWithGeoposition(class [Windows]Windows.Devices.Geolocation.Geoposition)
							IL_0277: leave IL_0323
						} // end .try
						catch [mscorlib]System.UnauthorizedAccessException
						{
							IL_027c: stloc.1
							IL_027d: ldloc.1
							IL_027e: callvirt instance string [mscorlib]System.Object::ToString()
							IL_0283: ldstr "StartGeolocatorAsync"
							IL_0288: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\FBGeoLocator\\Service.cs"
							IL_028d: ldc.i4 354
							IL_0292: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(string, string, string, int32)
							IL_0297: leave IL_0323
						} // end handler
						catch [mscorlib]System.Threading.Tasks.TaskCanceledException
						{
							IL_029c: stloc.2
							IL_029d: ldloc.2
							IL_029e: callvirt instance string [mscorlib]System.Object::ToString()
							IL_02a3: ldstr "StartGeolocatorAsync"
							IL_02a8: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\FBGeoLocator\\Service.cs"
							IL_02ad: ldc.i4 358
							IL_02b2: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(string, string, string, int32)
							IL_02b7: leave.s IL_0323
						} // end handler
						catch [mscorlib]System.Exception
						{
							IL_02b9: stloc.3
							IL_02ba: ldloc.3
							IL_02bb: callvirt instance int32 [mscorlib]System.Exception::get_HResult()
							IL_02c0: stloc.s 18
							IL_02c2: ldloc.s 18
							IL_02c4: ldc.i4 -2147024664 // 0x800700e8
							IL_02c9: bgt.un.s IL_02e8

							IL_02cb: ldloc.s 18
							IL_02cd: ldc.i4 -2147467260 // 0x80004004
							IL_02d2: beq.s IL_0303

							IL_02d4: ldloc.s 18
							IL_02d6: ldc.i4 -2147024883 // 0x8007000d
							IL_02db: beq.s IL_0303

							IL_02dd: ldloc.s 18
							IL_02df: ldc.i4 -2147024664 // 0x800700e8
							IL_02e4: beq.s IL_0303

							IL_02e6: br.s IL_031f

							IL_02e8: ldloc.s 18
							IL_02ea: ldc.i4 -2147023436 // 0x800705b4
							IL_02ef: beq.s IL_0303

							IL_02f1: ldloc.s 18
							IL_02f3: ldc.i4 -2147019873 // 0x8007139f
							IL_02f8: beq.s IL_0303

							IL_02fa: ldloc.s 18
							IL_02fc: ldc.i4 -2147012894 // 0x80072ee2
							IL_0301: bne.un.s IL_031f

							IL_0303: ldloc.3
							IL_0304: callvirt instance string [mscorlib]System.Object::ToString()
							IL_0309: ldstr "StartGeolocatorAsync"
							IL_030e: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\FBGeoLocator\\Service.cs"
							IL_0313: ldc.i4 377
							IL_0318: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(string, string, string, int32)
							IL_031d: br.s IL_0321

							IL_031f: rethrow

							IL_0321: leave.s IL_0323
						} // end handler

						IL_0323: leave.s IL_0335
					} // end .try
					finally
					{
						IL_0325: ldloc.s 4
						IL_0327: brfalse.s IL_0334

						IL_0329: ldarg.0
						IL_032a: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
						IL_032f: callvirt instance void Facebook.Messenger.WP8.FBGeoLocator.Service::StartRefetchLocationTimer()

						IL_0334: endfinally
					} // end handler

					IL_0335: leave.s IL_034f
				} // end .try
				finally
				{
					IL_0337: ldloc.s 4
					IL_0339: brfalse.s IL_034e

					IL_033b: ldarg.0
					IL_033c: ldfld class [mscorlib]System.IDisposable Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>7__wrapf'
					IL_0341: brfalse.s IL_034e

					IL_0343: ldarg.0
					IL_0344: ldfld class [mscorlib]System.IDisposable Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>7__wrapf'
					IL_0349: callvirt instance void [mscorlib]System.IDisposable::Dispose()

					IL_034e: endfinally
				} // end handler

				IL_034f: leave.s IL_036a
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_0351: stloc.s 5
				IL_0353: ldarg.0
				IL_0354: ldc.i4.s -2
				IL_0356: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'
				IL_035b: ldarg.0
				IL_035c: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>t__builder'
				IL_0361: ldloc.s 5
				IL_0363: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_0368: leave.s IL_037d
			} // end handler

			IL_036a: ldarg.0
			IL_036b: ldc.i4.s -2
			IL_036d: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'
			IL_0372: ldarg.0
			IL_0373: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>t__builder'
			IL_0378: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_037d: ret
		} // end of method '<StartGeolocatorAsync>d__d'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x1c614
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<StartGeolocatorAsync>d__d'::SetStateMachine

	} // end of class <StartGeolocatorAsync>d__d

	.class nested private auto ansi sealed beforefieldinit '<StopGeolocatorAsync>d__14'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter15'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x1c728
			// Header size: 12
			// Code size: 178 (0xb2)
			.maxstack 3
			.locals init (
				[0] bool,
				[1] class [mscorlib]System.Exception,
				[2] int32,
				[3] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[4] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.0
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>1__state'
				IL_0008: stloc.2
				IL_0009: ldloc.2
				IL_000a: ldc.i4.0
				IL_000b: beq.s IL_0058

				IL_000d: ldarg.0
				IL_000e: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>4__this'
				IL_0013: ldarg.0
				IL_0014: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>4__this'
				IL_0019: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::'<StopGeolocatorAsync>b__13'()
				IL_001f: newobj instance void [mscorlib]System.Action::.ctor(object, native int)
				IL_0024: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::PerformWithGeolocatorLockAsync(class [mscorlib]System.Action)
				IL_0029: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_002e: stloc.3
				IL_002f: ldloca.s 3
				IL_0031: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_0036: brtrue.s IL_0076

				IL_0038: ldarg.0
				IL_0039: ldc.i4.0
				IL_003a: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>1__state'
				IL_003f: ldarg.0
				IL_0040: ldloc.3
				IL_0041: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>u__$awaiter15'
				IL_0046: ldarg.0
				IL_0047: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>t__builder'
				IL_004c: ldloca.s 3
				IL_004e: ldarg.0
				IL_004f: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'>(!!0&, !!1&)
				IL_0054: ldc.i4.0
				IL_0055: stloc.0
				IL_0056: leave.s IL_00b1

				IL_0058: ldarg.0
				IL_0059: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>u__$awaiter15'
				IL_005e: stloc.3
				IL_005f: ldarg.0
				IL_0060: ldloca.s 4
				IL_0062: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_0068: ldloc.s 4
				IL_006a: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>u__$awaiter15'
				IL_006f: ldarg.0
				IL_0070: ldc.i4.m1
				IL_0071: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>1__state'

				IL_0076: ldloca.s 3
				IL_0078: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_007d: ldloca.s 3
				IL_007f: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_0085: leave.s IL_009e
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_0087: stloc.1
				IL_0088: ldarg.0
				IL_0089: ldc.i4.s -2
				IL_008b: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>1__state'
				IL_0090: ldarg.0
				IL_0091: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>t__builder'
				IL_0096: ldloc.1
				IL_0097: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_009c: leave.s IL_00b1
			} // end handler

			IL_009e: ldarg.0
			IL_009f: ldc.i4.s -2
			IL_00a1: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>1__state'
			IL_00a6: ldarg.0
			IL_00a7: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>t__builder'
			IL_00ac: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_00b1: ret
		} // end of method '<StopGeolocatorAsync>d__14'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x1c7f8
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<StopGeolocatorAsync>d__14'::SetStateMachine

	} // end of class <StopGeolocatorAsync>d__14

	.class nested private auto ansi sealed beforefieldinit '<PerformWithGeolocatorLockAsync>d__17'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field public class [mscorlib]System.Action action
		.field public class [mscorlib]System.IDisposable '<>7__wrap18'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable> '<>u__$awaiter19'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x1c850
			// Header size: 12
			// Code size: 215 (0xd7)
			.maxstack 3
			.locals init (
				[0] bool,
				[1] class [mscorlib]System.Exception,
				[2] int32,
				[3] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>,
				[4] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>,
				[5] class [mscorlib]System.IDisposable
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.0
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>1__state'
				IL_0008: stloc.2
				IL_0009: ldloc.2
				IL_000a: ldc.i4.0
				IL_000b: beq.s IL_004f

				IL_000d: ldarg.0
				IL_000e: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>4__this'
				IL_0013: ldfld class ['Facebook-Base']Facebook.Base.DisposableMutex Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorLock
				IL_0018: callvirt instance class [mscorlib]System.Threading.Tasks.Task`1<class [mscorlib]System.IDisposable> ['Facebook-Base']Facebook.Base.DisposableMutex::WaitAsync()
				IL_001d: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!0> class [mscorlib]System.Threading.Tasks.Task`1<class [mscorlib]System.IDisposable>::GetAwaiter()
				IL_0022: stloc.3
				IL_0023: ldloca.s 3
				IL_0025: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>::get_IsCompleted()
				IL_002a: brtrue.s IL_006d

				IL_002c: ldarg.0
				IL_002d: ldc.i4.0
				IL_002e: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>1__state'
				IL_0033: ldarg.0
				IL_0034: ldloc.3
				IL_0035: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable> Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>u__$awaiter19'
				IL_003a: ldarg.0
				IL_003b: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>t__builder'
				IL_0040: ldloca.s 3
				IL_0042: ldarg.0
				IL_0043: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'>(!!0&, !!1&)
				IL_0048: ldc.i4.0
				IL_0049: stloc.0
				IL_004a: leave IL_00d6

				IL_004f: ldarg.0
				IL_0050: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable> Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>u__$awaiter19'
				IL_0055: stloc.3
				IL_0056: ldarg.0
				IL_0057: ldloca.s 4
				IL_0059: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>
				IL_005f: ldloc.s 4
				IL_0061: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable> Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>u__$awaiter19'
				IL_0066: ldarg.0
				IL_0067: ldc.i4.m1
				IL_0068: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>1__state'

				IL_006d: ldloca.s 3
				IL_006f: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>::GetResult()
				IL_0074: ldloca.s 3
				IL_0076: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IDisposable>
				IL_007c: stloc.s 5
				IL_007e: ldarg.0
				IL_007f: ldloc.s 5
				IL_0081: stfld class [mscorlib]System.IDisposable Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>7__wrap18'
				.try
				{
					IL_0086: ldarg.0
					IL_0087: ldfld class [mscorlib]System.Action Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::action
					IL_008c: callvirt instance void [mscorlib]System.Action::Invoke()
					IL_0091: leave.s IL_00aa
				} // end .try
				finally
				{
					IL_0093: ldloc.0
					IL_0094: brfalse.s IL_00a9

					IL_0096: ldarg.0
					IL_0097: ldfld class [mscorlib]System.IDisposable Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>7__wrap18'
					IL_009c: brfalse.s IL_00a9

					IL_009e: ldarg.0
					IL_009f: ldfld class [mscorlib]System.IDisposable Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>7__wrap18'
					IL_00a4: callvirt instance void [mscorlib]System.IDisposable::Dispose()

					IL_00a9: endfinally
				} // end handler

				IL_00aa: leave.s IL_00c3
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_00ac: stloc.1
				IL_00ad: ldarg.0
				IL_00ae: ldc.i4.s -2
				IL_00b0: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>1__state'
				IL_00b5: ldarg.0
				IL_00b6: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>t__builder'
				IL_00bb: ldloc.1
				IL_00bc: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_00c1: leave.s IL_00d6
			} // end handler

			IL_00c3: ldarg.0
			IL_00c4: ldc.i4.s -2
			IL_00c6: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>1__state'
			IL_00cb: ldarg.0
			IL_00cc: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>t__builder'
			IL_00d1: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_00d6: ret
		} // end of method '<PerformWithGeolocatorLockAsync>d__17'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x1c950
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<PerformWithGeolocatorLockAsync>d__17'::SetStateMachine

	} // end of class <PerformWithGeolocatorLockAsync>d__17

	.class nested private auto ansi sealed beforefieldinit '<>c__DisplayClass1c'
		extends [mscorlib]System.Object
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field public class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs args

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x1ca0d
			// Header size: 1
			// Code size: 7 (0x7)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass1c'::.ctor

		.method public hidebysig 
			instance void '<GeolocatorOnPositionChanged>b__1b' () cil managed 
		{
			// Method begins at RVA 0x1ca15
			// Header size: 1
			// Code size: 60 (0x3c)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::'<>4__this'
			IL_0006: ldfld valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorStatus
			IL_000b: ldc.i4.2
			IL_000c: beq.s IL_000f

			IL_000e: ret

			IL_000f: ldarg.0
			IL_0010: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::'<>4__this'
			IL_0015: ldarg.0
			IL_0016: ldfld class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::args
			IL_001b: callvirt instance class [Windows]Windows.Devices.Geolocation.Geoposition [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs::get_Position()
			IL_0020: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::UpdateCurrentLocationWithGeoposition(class [Windows]Windows.Devices.Geolocation.Geoposition)
			IL_0025: ldarg.0
			IL_0026: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::'<>4__this'
			IL_002b: ldarg.0
			IL_002c: ldfld class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::args
			IL_0031: callvirt instance class [Windows]Windows.Devices.Geolocation.Geoposition [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs::get_Position()
			IL_0036: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::ReverseGeocodeWithGeoposition(class [Windows]Windows.Devices.Geolocation.Geoposition)
			IL_003b: ret
		} // end of method '<>c__DisplayClass1c'::'<GeolocatorOnPositionChanged>b__1b'

	} // end of class <>c__DisplayClass1c

	.class nested private auto ansi sealed beforefieldinit '<GeolocatorOnPositionChanged>d__1e'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field public class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs args
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c' 'CS$<>8__locals1d'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter1f'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x1ca54
			// Header size: 12
			// Code size: 249 (0xf9)
			.maxstack 3
			.locals init (
				[0] bool,
				[1] class [mscorlib]System.Exception,
				[2] int32,
				[3] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[4] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.0
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>1__state'
				IL_0008: stloc.2
				IL_0009: ldloc.2
				IL_000a: ldc.i4.0
				IL_000b: beq IL_009f

				IL_0010: ldarg.0
				IL_0011: newobj instance void Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::.ctor()
				IL_0016: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c' Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'CS$<>8__locals1d'
				IL_001b: ldarg.0
				IL_001c: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c' Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'CS$<>8__locals1d'
				IL_0021: ldarg.0
				IL_0022: ldfld class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::args
				IL_0027: stfld class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::args
				IL_002c: ldarg.0
				IL_002d: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c' Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'CS$<>8__locals1d'
				IL_0032: ldarg.0
				IL_0033: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>4__this'
				IL_0038: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::'<>4__this'
				IL_003d: ldarg.0
				IL_003e: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c' Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'CS$<>8__locals1d'
				IL_0043: ldfld class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::args
				IL_0048: callvirt instance class [Windows]Windows.Devices.Geolocation.Geoposition [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs::get_Position()
				IL_004d: brtrue.s IL_0054

				IL_004f: leave IL_00e5

				IL_0054: ldarg.0
				IL_0055: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>4__this'
				IL_005a: ldarg.0
				IL_005b: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c' Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'CS$<>8__locals1d'
				IL_0060: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass1c'::'<GeolocatorOnPositionChanged>b__1b'()
				IL_0066: newobj instance void [mscorlib]System.Action::.ctor(object, native int)
				IL_006b: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::PerformWithGeolocatorLockAsync(class [mscorlib]System.Action)
				IL_0070: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_0075: stloc.3
				IL_0076: ldloca.s 3
				IL_0078: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_007d: brtrue.s IL_00bd

				IL_007f: ldarg.0
				IL_0080: ldc.i4.0
				IL_0081: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>1__state'
				IL_0086: ldarg.0
				IL_0087: ldloc.3
				IL_0088: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>u__$awaiter1f'
				IL_008d: ldarg.0
				IL_008e: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>t__builder'
				IL_0093: ldloca.s 3
				IL_0095: ldarg.0
				IL_0096: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'>(!!0&, !!1&)
				IL_009b: ldc.i4.0
				IL_009c: stloc.0
				IL_009d: leave.s IL_00f8

				IL_009f: ldarg.0
				IL_00a0: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>u__$awaiter1f'
				IL_00a5: stloc.3
				IL_00a6: ldarg.0
				IL_00a7: ldloca.s 4
				IL_00a9: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_00af: ldloc.s 4
				IL_00b1: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>u__$awaiter1f'
				IL_00b6: ldarg.0
				IL_00b7: ldc.i4.m1
				IL_00b8: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>1__state'

				IL_00bd: ldloca.s 3
				IL_00bf: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_00c4: ldloca.s 3
				IL_00c6: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_00cc: leave.s IL_00e5
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_00ce: stloc.1
				IL_00cf: ldarg.0
				IL_00d0: ldc.i4.s -2
				IL_00d2: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>1__state'
				IL_00d7: ldarg.0
				IL_00d8: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>t__builder'
				IL_00dd: ldloc.1
				IL_00de: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_00e3: leave.s IL_00f8
			} // end handler

			IL_00e5: ldarg.0
			IL_00e6: ldc.i4.s -2
			IL_00e8: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>1__state'
			IL_00ed: ldarg.0
			IL_00ee: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>t__builder'
			IL_00f3: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::SetResult()

			IL_00f8: ret
		} // end of method '<GeolocatorOnPositionChanged>d__1e'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x1cb6c
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<GeolocatorOnPositionChanged>d__1e'::SetStateMachine

	} // end of class <GeolocatorOnPositionChanged>d__1e

	.class nested private auto ansi sealed beforefieldinit '<>c__DisplayClass22'
		extends [mscorlib]System.Object
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress address
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x1cbbe
			// Header size: 1
			// Code size: 7 (0x7)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass22'::.ctor

		.method public hidebysig 
			instance void '<GeocodeQueryOnQueryCompleted>b__21' () cil managed 
		{
			// Method begins at RVA 0x1cbc6
			// Header size: 1
			// Code size: 33 (0x21)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22'::'<>4__this'
			IL_0006: ldfld valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorStatus
			IL_000b: ldc.i4.2
			IL_000c: beq.s IL_000f

			IL_000e: ret

			IL_000f: ldarg.0
			IL_0010: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22'::'<>4__this'
			IL_0015: ldarg.0
			IL_0016: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22'::address
			IL_001b: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::UpdateCurrentLocationWithMapAddress(class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress)
			IL_0020: ret
		} // end of method '<>c__DisplayClass22'::'<GeocodeQueryOnQueryCompleted>b__21'

	} // end of class <>c__DisplayClass22

	.class nested private auto ansi sealed beforefieldinit '<GeocodeQueryOnQueryCompleted>d__24'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field public class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>> args
		.field public class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation '<firstResult>5__25'
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22' 'CS$<>8__locals23'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter26'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x1cbe8
			// Header size: 12
			// Code size: 362 (0x16a)
			.maxstack 3
			.locals init (
				[0] bool,
				[1] class [mscorlib]System.Exception,
				[2] int32,
				[3] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[4] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.0
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>1__state'
				IL_0008: stloc.2
				IL_0009: ldloc.2
				IL_000a: ldc.i4.0
				IL_000b: beq IL_0110

				IL_0010: ldarg.0
				IL_0011: newobj instance void Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22'::.ctor()
				IL_0016: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22' Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'CS$<>8__locals23'
				IL_001b: ldarg.0
				IL_001c: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22' Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'CS$<>8__locals23'
				IL_0021: ldarg.0
				IL_0022: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>4__this'
				IL_0027: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22'::'<>4__this'
				IL_002c: ldarg.0
				IL_002d: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>4__this'
				IL_0032: callvirt instance void Facebook.Messenger.WP8.FBGeoLocator.Service::ReleaseGeocodeQueryLock()
				IL_0037: ldarg.0
				IL_0038: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>> Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::args
				IL_003d: callvirt instance class [mscorlib]System.Exception [System]System.ComponentModel.AsyncCompletedEventArgs::get_Error()
				IL_0042: brtrue.s IL_0063

				IL_0044: ldarg.0
				IL_0045: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>> Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::args
				IL_004a: callvirt instance !0 class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>::get_Result()
				IL_004f: brfalse.s IL_0063

				IL_0051: ldarg.0
				IL_0052: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>> Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::args
				IL_0057: callvirt instance !0 class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>::get_Result()
				IL_005c: call bool [System.Core]System.Linq.Enumerable::Any<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>)
				IL_0061: brtrue.s IL_0068

				IL_0063: leave IL_0156

				IL_0068: ldarg.0
				IL_0069: ldarg.0
				IL_006a: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>> Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::args
				IL_006f: callvirt instance !0 class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>::get_Result()
				IL_0074: call !!0 [System.Core]System.Linq.Enumerable::FirstOrDefault<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>)
				IL_0079: stfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<firstResult>5__25'
				IL_007e: ldarg.0
				IL_007f: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<firstResult>5__25'
				IL_0084: brfalse.s IL_00a5

				IL_0086: ldarg.0
				IL_0087: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<firstResult>5__25'
				IL_008c: callvirt instance class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.LocationInformation [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation::get_Information()
				IL_0091: brfalse.s IL_00a5

				IL_0093: ldarg.0
				IL_0094: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<firstResult>5__25'
				IL_0099: callvirt instance class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.LocationInformation [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation::get_Information()
				IL_009e: callvirt instance class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.LocationInformation::get_Address()
				IL_00a3: brtrue.s IL_00aa

				IL_00a5: leave IL_0156

				IL_00aa: ldarg.0
				IL_00ab: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22' Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'CS$<>8__locals23'
				IL_00b0: ldarg.0
				IL_00b1: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<firstResult>5__25'
				IL_00b6: callvirt instance class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.LocationInformation [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation::get_Information()
				IL_00bb: callvirt instance class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.LocationInformation::get_Address()
				IL_00c0: stfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22'::address
				IL_00c5: ldarg.0
				IL_00c6: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>4__this'
				IL_00cb: ldarg.0
				IL_00cc: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22' Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'CS$<>8__locals23'
				IL_00d1: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass22'::'<GeocodeQueryOnQueryCompleted>b__21'()
				IL_00d7: newobj instance void [mscorlib]System.Action::.ctor(object, native int)
				IL_00dc: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::PerformWithGeolocatorLockAsync(class [mscorlib]System.Action)
				IL_00e1: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_00e6: stloc.3
				IL_00e7: ldloca.s 3
				IL_00e9: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_00ee: brtrue.s IL_012e

				IL_00f0: ldarg.0
				IL_00f1: ldc.i4.0
				IL_00f2: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>1__state'
				IL_00f7: ldarg.0
				IL_00f8: ldloc.3
				IL_00f9: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>u__$awaiter26'
				IL_00fe: ldarg.0
				IL_00ff: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>t__builder'
				IL_0104: ldloca.s 3
				IL_0106: ldarg.0
				IL_0107: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'>(!!0&, !!1&)
				IL_010c: ldc.i4.0
				IL_010d: stloc.0
				IL_010e: leave.s IL_0169

				IL_0110: ldarg.0
				IL_0111: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>u__$awaiter26'
				IL_0116: stloc.3
				IL_0117: ldarg.0
				IL_0118: ldloca.s 4
				IL_011a: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_0120: ldloc.s 4
				IL_0122: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>u__$awaiter26'
				IL_0127: ldarg.0
				IL_0128: ldc.i4.m1
				IL_0129: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>1__state'

				IL_012e: ldloca.s 3
				IL_0130: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_0135: ldloca.s 3
				IL_0137: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_013d: leave.s IL_0156
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_013f: stloc.1
				IL_0140: ldarg.0
				IL_0141: ldc.i4.s -2
				IL_0143: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>1__state'
				IL_0148: ldarg.0
				IL_0149: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>t__builder'
				IL_014e: ldloc.1
				IL_014f: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_0154: leave.s IL_0169
			} // end handler

			IL_0156: ldarg.0
			IL_0157: ldc.i4.s -2
			IL_0159: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>1__state'
			IL_015e: ldarg.0
			IL_015f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>t__builder'
			IL_0164: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::SetResult()

			IL_0169: ret
		} // end of method '<GeocodeQueryOnQueryCompleted>d__24'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x1cd7c
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<GeocodeQueryOnQueryCompleted>d__24'::SetStateMachine

	} // end of class <GeocodeQueryOnQueryCompleted>d__24

	.class nested private auto ansi sealed beforefieldinit '<>c__DisplayClass29'
		extends [mscorlib]System.Object
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Nested Types
		.class nested private auto ansi sealed beforefieldinit '<<ReverseGeocodeWithGeoposition>b__28>d__2b'
			extends [mscorlib]System.ValueType
			implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
		{
			// Fields
			.field public int32 '<>1__state'
			.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder '<>t__builder'
			.field public class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' '<>4__this'
			.field public class [System.Device]System.Device.Location.GeoCoordinate '<coordinate>5__2c'
			.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter2d'
			.field private object '<>t__stack'

			// Methods
			.method private final hidebysig newslot virtual 
				instance void MoveNext () cil managed 
			{
				.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
				// Method begins at RVA 0x1cec8
				// Header size: 12
				// Code size: 706 (0x2c2)
				.maxstack 8
				.locals init (
					[0] class [mscorlib]System.Runtime.InteropServices.COMException,
					[1] bool,
					[2] class [mscorlib]System.Exception,
					[3] int32,
					[4] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
					[5] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
					[6] valuetype [mscorlib]System.Nullable`1<float64>,
					[7] valuetype [mscorlib]System.Nullable`1<float64>,
					[8] valuetype [mscorlib]System.Nullable`1<float64>,
					[9] valuetype [mscorlib]System.Nullable`1<float64>,
					[10] uint32
				)

				.try
				{
					IL_0000: ldc.i4.1
					IL_0001: stloc.1
					IL_0002: ldarg.0
					IL_0003: ldfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>1__state'
					IL_0008: stloc.3
					IL_0009: ldloc.3
					IL_000a: ldc.i4.0
					IL_000b: beq.s IL_0056

					IL_000d: ldarg.0
					IL_000e: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_0013: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
					IL_0018: ldfld class [mscorlib]System.Threading.SemaphoreSlim Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQueryLock
					IL_001d: callvirt instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Threading.SemaphoreSlim::WaitAsync()
					IL_0022: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
					IL_0027: stloc.s 4
					IL_0029: ldloca.s 4
					IL_002b: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
					IL_0030: brtrue.s IL_0075

					IL_0032: ldarg.0
					IL_0033: ldc.i4.0
					IL_0034: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>1__state'
					IL_0039: ldarg.0
					IL_003a: ldloc.s 4
					IL_003c: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>u__$awaiter2d'
					IL_0041: ldarg.0
					IL_0042: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>t__builder'
					IL_0047: ldloca.s 4
					IL_0049: ldarg.0
					IL_004a: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'>(!!0&, !!1&)
					IL_004f: ldc.i4.0
					IL_0050: stloc.1
					IL_0051: leave IL_02c1

					IL_0056: ldarg.0
					IL_0057: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>u__$awaiter2d'
					IL_005c: stloc.s 4
					IL_005e: ldarg.0
					IL_005f: ldloca.s 5
					IL_0061: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
					IL_0067: ldloc.s 5
					IL_0069: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>u__$awaiter2d'
					IL_006e: ldarg.0
					IL_006f: ldc.i4.m1
					IL_0070: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>1__state'

					IL_0075: ldloca.s 4
					IL_0077: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
					IL_007c: ldloca.s 4
					IL_007e: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
					IL_0084: ldarg.0
					IL_0085: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_008a: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
					IL_008f: ldfld valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorStatus
					IL_0094: ldc.i4.2
					IL_0095: beq.s IL_009c

					IL_0097: leave IL_02ae

					IL_009c: ldarg.0
					IL_009d: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_00a2: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
					IL_00a7: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQuery
					IL_00ac: brtrue.s IL_00f0

					IL_00ae: ldarg.0
					IL_00af: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_00b4: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
					IL_00b9: newobj instance void [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery::.ctor()
					IL_00be: stfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQuery
					IL_00c3: ldarg.0
					IL_00c4: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_00c9: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
					IL_00ce: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQuery
					IL_00d3: ldarg.0
					IL_00d4: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_00d9: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
					IL_00de: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::GeocodeQueryOnQueryCompleted(object, class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>)
					IL_00e4: newobj instance void class [mscorlib]System.EventHandler`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>>::.ctor(object, native int)
					IL_00e9: callvirt instance void class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.Query`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>::add_QueryCompleted(class [mscorlib]System.EventHandler`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<!0>>)
					IL_00ee: br.s IL_010c

					IL_00f0: ldarg.0
					IL_00f1: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_00f6: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
					IL_00fb: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQuery
					IL_0100: callvirt instance bool class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.Query`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>::get_IsBusy()
					IL_0105: brfalse.s IL_010c

					IL_0107: leave IL_02ae

					IL_010c: ldarg.0
					IL_010d: ldarg.0
					IL_010e: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_0113: ldfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::position
					IL_0118: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
					IL_011d: callvirt instance float64 [Windows]Windows.Devices.Geolocation.Geocoordinate::get_Latitude()
					IL_0122: ldarg.0
					IL_0123: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_0128: ldfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::position
					IL_012d: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
					IL_0132: callvirt instance float64 [Windows]Windows.Devices.Geolocation.Geocoordinate::get_Longitude()
					IL_0137: ldarg.0
					IL_0138: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_013d: ldfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::position
					IL_0142: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
					IL_0147: callvirt instance valuetype [mscorlib]System.Nullable`1<float64> [Windows]Windows.Devices.Geolocation.Geocoordinate::get_Altitude()
					IL_014c: stloc.s 6
					IL_014e: ldloca.s 6
					IL_0150: call instance bool valuetype [mscorlib]System.Nullable`1<float64>::get_HasValue()
					IL_0155: brtrue.s IL_0162

					IL_0157: ldc.r8 (00 00 00 00 00 00 F8 FF)
					IL_0160: br.s IL_016b

					IL_0162: ldloca.s 6
					IL_0164: call instance !0 valuetype [mscorlib]System.Nullable`1<float64>::GetValueOrDefault()
					IL_0169: conv.r8
					IL_016a: conv.r8

					IL_016b: ldarg.0
					IL_016c: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_0171: ldfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::position
					IL_0176: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
					IL_017b: callvirt instance float64 [Windows]Windows.Devices.Geolocation.Geocoordinate::get_Accuracy()
					IL_0180: ldarg.0
					IL_0181: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_0186: ldfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::position
					IL_018b: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
					IL_0190: callvirt instance valuetype [mscorlib]System.Nullable`1<float64> [Windows]Windows.Devices.Geolocation.Geocoordinate::get_AltitudeAccuracy()
					IL_0195: stloc.s 7
					IL_0197: ldloca.s 7
					IL_0199: call instance bool valuetype [mscorlib]System.Nullable`1<float64>::get_HasValue()
					IL_019e: brtrue.s IL_01ab

					IL_01a0: ldc.r8 (00 00 00 00 00 00 F8 FF)
					IL_01a9: br.s IL_01b4

					IL_01ab: ldloca.s 7
					IL_01ad: call instance !0 valuetype [mscorlib]System.Nullable`1<float64>::GetValueOrDefault()
					IL_01b2: conv.r8
					IL_01b3: conv.r8

					IL_01b4: ldarg.0
					IL_01b5: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_01ba: ldfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::position
					IL_01bf: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
					IL_01c4: callvirt instance valuetype [mscorlib]System.Nullable`1<float64> [Windows]Windows.Devices.Geolocation.Geocoordinate::get_Speed()
					IL_01c9: stloc.s 8
					IL_01cb: ldloca.s 8
					IL_01cd: call instance bool valuetype [mscorlib]System.Nullable`1<float64>::get_HasValue()
					IL_01d2: brtrue.s IL_01df

					IL_01d4: ldc.r8 (00 00 00 00 00 00 F8 FF)
					IL_01dd: br.s IL_01e8

					IL_01df: ldloca.s 8
					IL_01e1: call instance !0 valuetype [mscorlib]System.Nullable`1<float64>::GetValueOrDefault()
					IL_01e6: conv.r8
					IL_01e7: conv.r8

					IL_01e8: ldarg.0
					IL_01e9: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_01ee: ldfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::position
					IL_01f3: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
					IL_01f8: callvirt instance valuetype [mscorlib]System.Nullable`1<float64> [Windows]Windows.Devices.Geolocation.Geocoordinate::get_Heading()
					IL_01fd: stloc.s 9
					IL_01ff: ldloca.s 9
					IL_0201: call instance bool valuetype [mscorlib]System.Nullable`1<float64>::get_HasValue()
					IL_0206: brtrue.s IL_0213

					IL_0208: ldc.r8 (00 00 00 00 00 00 F8 FF)
					IL_0211: br.s IL_021c

					IL_0213: ldloca.s 9
					IL_0215: call instance !0 valuetype [mscorlib]System.Nullable`1<float64>::GetValueOrDefault()
					IL_021a: conv.r8
					IL_021b: conv.r8

					IL_021c: newobj instance void [System.Device]System.Device.Location.GeoCoordinate::.ctor(float64, float64, float64, float64, float64, float64, float64)
					IL_0221: stfld class [System.Device]System.Device.Location.GeoCoordinate Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<coordinate>5__2c'
					IL_0226: ldarg.0
					IL_0227: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
					IL_022c: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
					IL_0231: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQuery
					IL_0236: ldarg.0
					IL_0237: ldfld class [System.Device]System.Device.Location.GeoCoordinate Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<coordinate>5__2c'
					IL_023c: callvirt instance void [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery::set_GeoCoordinate(class [System.Device]System.Device.Location.GeoCoordinate)
					.try
					{
						IL_0241: ldarg.0
						IL_0242: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
						IL_0247: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
						IL_024c: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQuery
						IL_0251: callvirt instance void class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.Query`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>::QueryAsync()
						IL_0256: leave.s IL_0295
					} // end .try
					catch [mscorlib]System.Runtime.InteropServices.COMException
					{
						IL_0258: stloc.0
						IL_0259: ldloc.0
						IL_025a: callvirt instance int32 [mscorlib]System.Runtime.InteropServices.ExternalException::get_ErrorCode()
						IL_025f: stloc.s 10
						IL_0261: ldloc.s 10
						IL_0263: ldc.i4 -2147188735 // 0x80048001
						IL_0268: bne.un.s IL_0291

						IL_026a: ldarg.0
						IL_026b: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
						IL_0270: ldfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
						IL_0275: ldloc.0
						IL_0276: callvirt instance string [mscorlib]System.Object::ToString()
						IL_027b: ldstr "ReverseGeocodeWithGeoposition"
						IL_0280: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\FBGeoLocator\\Service.cs"
						IL_0285: ldc.i4 568
						IL_028a: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
						IL_028f: br.s IL_0293

						IL_0291: rethrow

						IL_0293: leave.s IL_0295
					} // end handler

					IL_0295: leave.s IL_02ae
				} // end .try
				catch [mscorlib]System.Exception
				{
					IL_0297: stloc.2
					IL_0298: ldarg.0
					IL_0299: ldc.i4.s -2
					IL_029b: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>1__state'
					IL_02a0: ldarg.0
					IL_02a1: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>t__builder'
					IL_02a6: ldloc.2
					IL_02a7: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::SetException(class [mscorlib]System.Exception)
					IL_02ac: leave.s IL_02c1
				} // end handler

				IL_02ae: ldarg.0
				IL_02af: ldc.i4.s -2
				IL_02b1: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>1__state'
				IL_02b6: ldarg.0
				IL_02b7: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>t__builder'
				IL_02bc: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::SetResult()

				IL_02c1: ret
			} // end of method '<<ReverseGeocodeWithGeoposition>b__28>d__2b'::MoveNext

			.method private final hidebysig newslot virtual 
				instance void SetStateMachine (
					class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
				) cil managed 
			{
				.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
					01 00 00 00
				)
				.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
				// Method begins at RVA 0x1d1cc
				// Header size: 1
				// Code size: 13 (0xd)
				.maxstack 8

				IL_0000: ldarg.0
				IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>t__builder'
				IL_0006: ldarg.1
				IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
				IL_000c: ret
			} // end of method '<<ReverseGeocodeWithGeoposition>b__28>d__2b'::SetStateMachine

		} // end of class <<ReverseGeocodeWithGeoposition>b__28>d__2b


		// Fields
		.field public class Facebook.Messenger.WP8.FBGeoLocator.Service '<>4__this'
		.field public class [Windows]Windows.Devices.Geolocation.Geoposition position

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x1cec0
			// Header size: 1
			// Code size: 7 (0x7)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass29'::.ctor

		.method public hidebysig 
			instance void '<ReverseGeocodeWithGeoposition>b__28' () cil managed 
		{
			.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
				01 00 6b 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
				65 6e 67 65 72 2e 57 50 38 2e 46 42 47 65 6f 4c
				6f 63 61 74 6f 72 2e 53 65 72 76 69 63 65 2b 3c
				3e 63 5f 5f 44 69 73 70 6c 61 79 43 6c 61 73 73
				32 39 2b 3c 3c 52 65 76 65 72 73 65 47 65 6f 63
				6f 64 65 57 69 74 68 47 65 6f 70 6f 73 69 74 69
				6f 6e 3e 62 5f 5f 32 38 3e 64 5f 5f 32 62 00 00
			)
			.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
				01 00 00 00
			)
			// Method begins at RVA 0x1d1dc
			// Header size: 12
			// Code size: 46 (0x2e)
			.maxstack 2
			.locals init (
				[0] valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b',
				[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder
			)

			IL_0000: ldloca.s 0
			IL_0002: ldarg.0
			IL_0003: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29' Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>4__this'
			IL_0008: ldloca.s 0
			IL_000a: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Create()
			IL_000f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>t__builder'
			IL_0014: ldloca.s 0
			IL_0016: ldc.i4.m1
			IL_0017: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>1__state'
			IL_001c: ldloca.s 0
			IL_001e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'::'<>t__builder'
			IL_0023: stloc.1
			IL_0024: ldloca.s 1
			IL_0026: ldloca.s 0
			IL_0028: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Start<valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'/'<<ReverseGeocodeWithGeoposition>b__28>d__2b'>(!!0&)
			IL_002d: ret
		} // end of method '<>c__DisplayClass29'::'<ReverseGeocodeWithGeoposition>b__28'

	} // end of class <>c__DisplayClass29


	// Fields
	.field private static literal string ThreadsLocationSettingsKey = "ThreadsLocationSettings"
	.field private static literal string ThreadsLocationVersionKey = "ThreadsLocationVersion"
	.field private static literal string ThreadsLocationVersion = "1.0"
	.field private static literal int32 StaleLocationThreshold = int32(10)
	.field private initonly class ['Facebook-Models']Facebook.Models.FacebookContext _context
	.field private class [Windows]Windows.Devices.Geolocation.Geolocator _geolocator
	.field private class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery _geocodeQuery
	.field private class [mscorlib]System.Threading.CancellationTokenSource _cancellationTokenSource
	.field private valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus _geolocatorStatus
	.field private class ['Facebook-Models']Facebook.Models.FacebookPlace _currentLocation
	.field private bool _deviceLocationEnabled
	.field private valuetype [mscorlib]System.DateTime _lastKnownGoodLocationTime
	.field private initonly valuetype [mscorlib]System.TimeSpan _staleLocationTime
	.field private class [mscorlib]System.Threading.Timer _refetchLocationTimer
	.field private class [mscorlib]System.Collections.Generic.List`1<string> _threadsLocationSettings
	.field private initonly object _threadsLocationSettingsLock
	.field private initonly class [mscorlib]System.Threading.SemaphoreSlim _geocodeQueryLock
	.field private initonly class ['Facebook-Base']Facebook.Base.DisposableMutex _geolocatorLock
	.field private class [mscorlib]System.EventHandler GeolocatorStatusChanged

	// Methods
	.method public hidebysig specialname 
		instance void add_GeolocatorStatusChanged (
			class [mscorlib]System.EventHandler 'value'
		) cil managed 
	{
		// Method begins at RVA 0x1b97c
		// Header size: 12
		// Code size: 41 (0x29)
		.maxstack 3
		.locals init (
			[0] class [mscorlib]System.EventHandler,
			[1] class [mscorlib]System.EventHandler,
			[2] class [mscorlib]System.EventHandler
		)

		IL_0000: ldarg.0
		IL_0001: ldfld class [mscorlib]System.EventHandler Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorStatusChanged
		IL_0006: stloc.0
		// loop start (head: IL_0007)
			IL_0007: ldloc.0
			IL_0008: stloc.1
			IL_0009: ldloc.1
			IL_000a: ldarg.1
			IL_000b: call class [mscorlib]System.Delegate [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate, class [mscorlib]System.Delegate)
			IL_0010: castclass [mscorlib]System.EventHandler
			IL_0015: stloc.2
			IL_0016: ldarg.0
			IL_0017: ldflda class [mscorlib]System.EventHandler Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorStatusChanged
			IL_001c: ldloc.2
			IL_001d: ldloc.1
			IL_001e: call !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class [mscorlib]System.EventHandler>(!!0&, !!0, !!0)
			IL_0023: stloc.0
			IL_0024: ldloc.0
			IL_0025: ldloc.1
			IL_0026: bne.un.s IL_0007
		// end loop
		IL_0028: ret
	} // end of method Service::add_GeolocatorStatusChanged

	.method public hidebysig specialname 
		instance void remove_GeolocatorStatusChanged (
			class [mscorlib]System.EventHandler 'value'
		) cil managed 
	{
		// Method begins at RVA 0x1b9b4
		// Header size: 12
		// Code size: 41 (0x29)
		.maxstack 3
		.locals init (
			[0] class [mscorlib]System.EventHandler,
			[1] class [mscorlib]System.EventHandler,
			[2] class [mscorlib]System.EventHandler
		)

		IL_0000: ldarg.0
		IL_0001: ldfld class [mscorlib]System.EventHandler Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorStatusChanged
		IL_0006: stloc.0
		// loop start (head: IL_0007)
			IL_0007: ldloc.0
			IL_0008: stloc.1
			IL_0009: ldloc.1
			IL_000a: ldarg.1
			IL_000b: call class [mscorlib]System.Delegate [mscorlib]System.Delegate::Remove(class [mscorlib]System.Delegate, class [mscorlib]System.Delegate)
			IL_0010: castclass [mscorlib]System.EventHandler
			IL_0015: stloc.2
			IL_0016: ldarg.0
			IL_0017: ldflda class [mscorlib]System.EventHandler Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorStatusChanged
			IL_001c: ldloc.2
			IL_001d: ldloc.1
			IL_001e: call !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class [mscorlib]System.EventHandler>(!!0&, !!0, !!0)
			IL_0023: stloc.0
			IL_0024: ldloc.0
			IL_0025: ldloc.1
			IL_0026: bne.un.s IL_0007
		// end loop
		IL_0028: ret
	} // end of method Service::remove_GeolocatorStatusChanged

	.method public hidebysig specialname rtspecialname 
		instance void .ctor (
			class ['Facebook-Base']Facebook.Base.IDispatcher ui,
			class ['Facebook-Models']Facebook.Models.FacebookContext context
		) cil managed 
	{
		// Method begins at RVA 0x1b9ec
		// Header size: 12
		// Code size: 65 (0x41)
		.maxstack 4

		IL_0000: ldarg.0
		IL_0001: ldc.i4.0
		IL_0002: ldc.i4.s 10
		IL_0004: ldc.i4.0
		IL_0005: newobj instance void [mscorlib]System.TimeSpan::.ctor(int32, int32, int32)
		IL_000a: stfld valuetype [mscorlib]System.TimeSpan Facebook.Messenger.WP8.FBGeoLocator.Service::_staleLocationTime
		IL_000f: ldarg.0
		IL_0010: newobj instance void [mscorlib]System.Object::.ctor()
		IL_0015: stfld object Facebook.Messenger.WP8.FBGeoLocator.Service::_threadsLocationSettingsLock
		IL_001a: ldarg.0
		IL_001b: ldc.i4.1
		IL_001c: ldc.i4.1
		IL_001d: newobj instance void [mscorlib]System.Threading.SemaphoreSlim::.ctor(int32, int32)
		IL_0022: stfld class [mscorlib]System.Threading.SemaphoreSlim Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQueryLock
		IL_0027: ldarg.0
		IL_0028: newobj instance void ['Facebook-Base']Facebook.Base.DisposableMutex::.ctor()
		IL_002d: stfld class ['Facebook-Base']Facebook.Base.DisposableMutex Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorLock
		IL_0032: ldarg.0
		IL_0033: ldarg.1
		IL_0034: call instance void ['Facebook-Services']Facebook.Services.BackgroundService::.ctor(class ['Facebook-Base']Facebook.Base.IDispatcher)
		IL_0039: ldarg.0
		IL_003a: ldarg.2
		IL_003b: stfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_0040: ret
	} // end of method Service::.ctor

	.method public hidebysig virtual 
		instance void StartService (
			class ['Facebook-Models']Facebook.Models.FacebookContext context
		) cil managed 
	{
		// Method begins at RVA 0x1ba39
		// Header size: 1
		// Code size: 58 (0x3a)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::StartGeolocatorAsync()
		IL_0006: call void ['Facebook-Base']Facebook.Base.Extensions.TaskExtensions::DoNotWait(class [mscorlib]System.Threading.Tasks.Task)
		IL_000b: ldarg.0
		IL_000c: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_0011: ldarg.0
		IL_0012: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::ContextOnLoggedIn(object, class ['Facebook-Models']Facebook.Models.FacebookSession)
		IL_0018: newobj instance void class [mscorlib]System.EventHandler`1<class ['Facebook-Models']Facebook.Models.FacebookSession>::.ctor(object, native int)
		IL_001d: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookContext::add_LoggedIn(class [mscorlib]System.EventHandler`1<class ['Facebook-Models']Facebook.Models.FacebookSession>)
		IL_0022: ldarg.0
		IL_0023: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_0028: ldarg.0
		IL_0029: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::ContextOnLoggedOut(object, class ['Facebook-Models']Facebook.Models.LogoutEventArgs)
		IL_002f: newobj instance void class [mscorlib]System.EventHandler`1<class ['Facebook-Models']Facebook.Models.LogoutEventArgs>::.ctor(object, native int)
		IL_0034: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookContext::add_LoggedOut(class [mscorlib]System.EventHandler`1<class ['Facebook-Models']Facebook.Models.LogoutEventArgs>)
		IL_0039: ret
	} // end of method Service::StartService

	.method public hidebysig virtual 
		instance void StopService () cil managed 
	{
		// Method begins at RVA 0x1ba74
		// Header size: 12
		// Code size: 64 (0x40)
		.maxstack 3

		IL_0000: ldarg.0
		IL_0001: call instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::StopGeolocatorAsync()
		IL_0006: call void ['Facebook-Base']Facebook.Base.Extensions.TaskExtensions::DoNotWait(class [mscorlib]System.Threading.Tasks.Task)
		IL_000b: ldarg.0
		IL_000c: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::SaveThreadsLocationSettings()
		IL_0011: ldarg.0
		IL_0012: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_0017: ldarg.0
		IL_0018: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::ContextOnLoggedIn(object, class ['Facebook-Models']Facebook.Models.FacebookSession)
		IL_001e: newobj instance void class [mscorlib]System.EventHandler`1<class ['Facebook-Models']Facebook.Models.FacebookSession>::.ctor(object, native int)
		IL_0023: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookContext::remove_LoggedIn(class [mscorlib]System.EventHandler`1<class ['Facebook-Models']Facebook.Models.FacebookSession>)
		IL_0028: ldarg.0
		IL_0029: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_002e: ldarg.0
		IL_002f: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::ContextOnLoggedOut(object, class ['Facebook-Models']Facebook.Models.LogoutEventArgs)
		IL_0035: newobj instance void class [mscorlib]System.EventHandler`1<class ['Facebook-Models']Facebook.Models.LogoutEventArgs>::.ctor(object, native int)
		IL_003a: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookContext::remove_LoggedOut(class [mscorlib]System.EventHandler`1<class ['Facebook-Models']Facebook.Models.LogoutEventArgs>)
		IL_003f: ret
	} // end of method Service::StopService

	.method private hidebysig 
		instance void ContextOnLoggedIn (
			object sender,
			class ['Facebook-Models']Facebook.Models.FacebookSession facebookSession
		) cil managed 
	{
		// Method begins at RVA 0x1bac0
		// Header size: 1
		// Code size: 12 (0xc)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::StartGeolocatorAsync()
		IL_0006: call void ['Facebook-Base']Facebook.Base.Extensions.TaskExtensions::DoNotWait(class [mscorlib]System.Threading.Tasks.Task)
		IL_000b: ret
	} // end of method Service::ContextOnLoggedIn

	.method private hidebysig 
		instance void ContextOnLoggedOut (
			object sender,
			class ['Facebook-Models']Facebook.Models.LogoutEventArgs logoutEventArgs
		) cil managed 
	{
		// Method begins at RVA 0x1bacd
		// Header size: 1
		// Code size: 12 (0xc)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::StopGeolocatorAsync()
		IL_0006: call void ['Facebook-Base']Facebook.Base.Extensions.TaskExtensions::DoNotWait(class [mscorlib]System.Threading.Tasks.Task)
		IL_000b: ret
	} // end of method Service::ContextOnLoggedOut

	.method public hidebysig 
		instance bool IsDeviceLocationEnabled () cil managed 
	{
		// Method begins at RVA 0x1bada
		// Header size: 1
		// Code size: 7 (0x7)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld bool Facebook.Messenger.WP8.FBGeoLocator.Service::_deviceLocationEnabled
		IL_0006: ret
	} // end of method Service::IsDeviceLocationEnabled

	.method public hidebysig 
		instance bool IsAppLocationEnabled () cil managed 
	{
		// Method begins at RVA 0x1bae2
		// Header size: 1
		// Code size: 27 (0x1b)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_0006: callvirt instance bool ['Facebook-Models']Facebook.Models.FacebookContext::IsAuthenticated()
		IL_000b: brfalse.s IL_0019

		IL_000d: ldarg.0
		IL_000e: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_0013: callvirt instance bool ['Facebook-Models']Facebook.Models.FacebookContext::IsLocationEnabled()
		IL_0018: ret

		IL_0019: ldc.i4.0
		IL_001a: ret
	} // end of method Service::IsAppLocationEnabled

	.method public hidebysig 
		instance bool IsLocationEnabled () cil managed 
	{
		// Method begins at RVA 0x1bafe
		// Header size: 1
		// Code size: 17 (0x11)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld bool Facebook.Messenger.WP8.FBGeoLocator.Service::_deviceLocationEnabled
		IL_0006: brfalse.s IL_000f

		IL_0008: ldarg.0
		IL_0009: call instance bool Facebook.Messenger.WP8.FBGeoLocator.Service::IsAppLocationEnabled()
		IL_000e: ret

		IL_000f: ldc.i4.0
		IL_0010: ret
	} // end of method Service::IsLocationEnabled

	.method public hidebysig 
		instance bool IsLocationEnabledForThread (
			string settingsId
		) cil managed 
	{
		// Method begins at RVA 0x1bb10
		// Header size: 12
		// Code size: 38 (0x26)
		.maxstack 2
		.locals init (
			[0] class [mscorlib]System.Collections.Generic.List`1<string>
		)

		IL_0000: ldarg.0
		IL_0001: call instance bool Facebook.Messenger.WP8.FBGeoLocator.Service::IsLocationEnabled()
		IL_0006: brtrue.s IL_000a

		IL_0008: ldc.i4.0
		IL_0009: ret

		IL_000a: ldarg.1
		IL_000b: call bool Facebook.Messenger.WP8.Extensions.EnumerableExtensions::IsNullOrEmpty<char>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>)
		IL_0010: brfalse.s IL_0014

		IL_0012: ldc.i4.1
		IL_0013: ret

		IL_0014: ldarg.0
		IL_0015: call instance class [mscorlib]System.Collections.Generic.List`1<string> Facebook.Messenger.WP8.FBGeoLocator.Service::GetThreadsLocationSettings()
		IL_001a: stloc.0
		IL_001b: ldloc.0
		IL_001c: ldarg.1
		IL_001d: callvirt instance bool class [mscorlib]System.Collections.Generic.List`1<string>::Contains(!0)
		IL_0022: ldc.i4.0
		IL_0023: ceq
		IL_0025: ret
	} // end of method Service::IsLocationEnabledForThread

	.method public hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task`1<bool> SetLocationEnabledAsync (
			bool enabled
		) cil managed 
	{
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 49 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 46 42 47 65 6f 4c
			6f 63 61 74 6f 72 2e 53 65 72 76 69 63 65 2b 3c
			53 65 74 4c 6f 63 61 74 69 6f 6e 45 6e 61 62 6c
			65 64 41 73 79 6e 63 3e 64 5f 5f 30 00 00
		)
		// Method begins at RVA 0x1bc98
		// Header size: 12
		// Code size: 66 (0x42)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool>
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: ldarg.1
		IL_000b: stfld bool Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::enabled
		IL_0010: ldloca.s 0
		IL_0012: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<!0> valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool>::Create()
		IL_0017: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool> Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>t__builder'
		IL_001c: ldloca.s 0
		IL_001e: ldc.i4.m1
		IL_001f: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>1__state'
		IL_0024: ldloca.s 0
		IL_0026: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool> Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>t__builder'
		IL_002b: stloc.1
		IL_002c: ldloca.s 1
		IL_002e: ldloca.s 0
		IL_0030: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool>::Start<valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'>(!!0&)
		IL_0035: ldloca.s 0
		IL_0037: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool> Facebook.Messenger.WP8.FBGeoLocator.Service/'<SetLocationEnabledAsync>d__0'::'<>t__builder'
		IL_003c: call instance class [mscorlib]System.Threading.Tasks.Task`1<!0> valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<bool>::get_Task()
		IL_0041: ret
	} // end of method Service::SetLocationEnabledAsync

	.method public hidebysig 
		instance bool SetLocationForThread (
			bool enable,
			string settingsId
		) cil managed 
	{
		// Method begins at RVA 0x1bce6
		// Header size: 1
		// Code size: 31 (0x1f)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance bool Facebook.Messenger.WP8.FBGeoLocator.Service::IsLocationEnabled()
		IL_0006: brtrue.s IL_000a

		IL_0008: ldc.i4.0
		IL_0009: ret

		IL_000a: ldarg.1
		IL_000b: brfalse.s IL_0016

		IL_000d: ldarg.0
		IL_000e: ldarg.2
		IL_000f: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::EnableLocationForThread(string)
		IL_0014: br.s IL_001d

		IL_0016: ldarg.0
		IL_0017: ldarg.2
		IL_0018: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::DisableLocationForThread(string)

		IL_001d: ldc.i4.1
		IL_001e: ret
	} // end of method Service::SetLocationForThread

	.method public hidebysig 
		instance class ['Facebook-Models']Facebook.Models.FacebookPlace GetCurrentLocation () cil managed 
	{
		// Method begins at RVA 0x1bd08
		// Header size: 12
		// Code size: 65 (0x41)
		.maxstack 2
		.locals init (
			[0] valuetype [mscorlib]System.TimeSpan
		)

		IL_0000: call valuetype [mscorlib]System.DateTime [mscorlib]System.DateTime::get_UtcNow()
		IL_0005: ldarg.0
		IL_0006: ldfld valuetype [mscorlib]System.DateTime Facebook.Messenger.WP8.FBGeoLocator.Service::_lastKnownGoodLocationTime
		IL_000b: call valuetype [mscorlib]System.TimeSpan [mscorlib]System.DateTime::op_Subtraction(valuetype [mscorlib]System.DateTime, valuetype [mscorlib]System.DateTime)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldarg.0
		IL_0013: ldfld valuetype [mscorlib]System.TimeSpan Facebook.Messenger.WP8.FBGeoLocator.Service::_staleLocationTime
		IL_0018: call bool [mscorlib]System.TimeSpan::op_GreaterThan(valuetype [mscorlib]System.TimeSpan, valuetype [mscorlib]System.TimeSpan)
		IL_001d: brtrue.s IL_0034

		IL_001f: ldarg.0
		IL_0020: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_0025: brfalse.s IL_0034

		IL_0027: ldarg.0
		IL_0028: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_002d: callvirt instance bool ['Facebook-Models']Facebook.Models.FacebookPlace::IsValidLocation()
		IL_0032: brtrue.s IL_003a

		IL_0034: ldarg.0
		IL_0035: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::InvalidateAndRefetchLocation()

		IL_003a: ldarg.0
		IL_003b: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_0040: ret
	} // end of method Service::GetCurrentLocation

	.method private hidebysig 
		instance bool GetDeviceLocationStatusEnabled () cil managed 
	{
		// Method begins at RVA 0x1bd58
		// Header size: 12
		// Code size: 19 (0x13)
		.maxstack 2
		.locals init (
			[0] class [Windows]Windows.Devices.Geolocation.Geolocator
		)

		IL_0000: newobj instance void [Windows]Windows.Devices.Geolocation.Geolocator::.ctor()
		IL_0005: stloc.0
		IL_0006: ldloc.0
		IL_0007: callvirt instance valuetype [Windows]Windows.Devices.Geolocation.PositionStatus [Windows]Windows.Devices.Geolocation.Geolocator::get_LocationStatus()
		IL_000c: ldc.i4.3
		IL_000d: ceq
		IL_000f: ldc.i4.0
		IL_0010: ceq
		IL_0012: ret
	} // end of method Service::GetDeviceLocationStatusEnabled

	.method private hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task EnableLocationAsync () cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 45 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 46 42 47 65 6f 4c
			6f 63 61 74 6f 72 2e 53 65 72 76 69 63 65 2b 3c
			45 6e 61 62 6c 65 4c 6f 63 61 74 69 6f 6e 41 73
			79 6e 63 3e 64 5f 5f 33 00 00
		)
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0x1be58
		// Header size: 12
		// Code size: 58 (0x3a)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_000f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>t__builder'
		IL_0014: ldloca.s 0
		IL_0016: ldc.i4.m1
		IL_0017: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>1__state'
		IL_001c: ldloca.s 0
		IL_001e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>t__builder'
		IL_0023: stloc.1
		IL_0024: ldloca.s 1
		IL_0026: ldloca.s 0
		IL_0028: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'>(!!0&)
		IL_002d: ldloca.s 0
		IL_002f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<EnableLocationAsync>d__3'::'<>t__builder'
		IL_0034: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0039: ret
	} // end of method Service::EnableLocationAsync

	.method private hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task DisableLocationAsync () cil managed 
	{
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 46 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 46 42 47 65 6f 4c
			6f 63 61 74 6f 72 2e 53 65 72 76 69 63 65 2b 3c
			44 69 73 61 62 6c 65 4c 6f 63 61 74 69 6f 6e 41
			73 79 6e 63 3e 64 5f 5f 36 00 00
		)
		// Method begins at RVA 0x1bf80
		// Header size: 12
		// Code size: 58 (0x3a)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_000f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>t__builder'
		IL_0014: ldloca.s 0
		IL_0016: ldc.i4.m1
		IL_0017: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>1__state'
		IL_001c: ldloca.s 0
		IL_001e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>t__builder'
		IL_0023: stloc.1
		IL_0024: ldloca.s 1
		IL_0026: ldloca.s 0
		IL_0028: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'>(!!0&)
		IL_002d: ldloca.s 0
		IL_002f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<DisableLocationAsync>d__6'::'<>t__builder'
		IL_0034: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0039: ret
	} // end of method Service::DisableLocationAsync

	.method private hidebysig 
		instance void EnableLocationForThread (
			string threadId
		) cil managed 
	{
		// Method begins at RVA 0x1bfc8
		// Header size: 12
		// Code size: 56 (0x38)
		.maxstack 2
		.locals init (
			[0] class [mscorlib]System.Collections.Generic.List`1<string>,
			[1] bool,
			[2] object
		)

		IL_0000: ldc.i4.0
		IL_0001: stloc.1
		.try
		{
			IL_0002: ldarg.0
			IL_0003: ldfld object Facebook.Messenger.WP8.FBGeoLocator.Service::_threadsLocationSettingsLock
			IL_0008: dup
			IL_0009: stloc.2
			IL_000a: ldloca.s 1
			IL_000c: call void [mscorlib]System.Threading.Monitor::Enter(object, bool&)
			IL_0011: ldarg.0
			IL_0012: call instance class [mscorlib]System.Collections.Generic.List`1<string> Facebook.Messenger.WP8.FBGeoLocator.Service::GetThreadsLocationSettings()
			IL_0017: stloc.0
			IL_0018: ldloc.0
			IL_0019: ldarg.1
			IL_001a: callvirt instance bool class [mscorlib]System.Collections.Generic.List`1<string>::Contains(!0)
			IL_001f: brtrue.s IL_0023

			IL_0021: leave.s IL_0037

			IL_0023: ldloc.0
			IL_0024: ldarg.1
			IL_0025: callvirt instance bool class [mscorlib]System.Collections.Generic.List`1<string>::Remove(!0)
			IL_002a: pop
			IL_002b: leave.s IL_0037
		} // end .try
		finally
		{
			IL_002d: ldloc.1
			IL_002e: brfalse.s IL_0036

			IL_0030: ldloc.2
			IL_0031: call void [mscorlib]System.Threading.Monitor::Exit(object)

			IL_0036: endfinally
		} // end handler

		IL_0037: ret
	} // end of method Service::EnableLocationForThread

	.method private hidebysig 
		instance void DisableLocationForThread (
			string threadId
		) cil managed 
	{
		// Method begins at RVA 0x1c01c
		// Header size: 12
		// Code size: 55 (0x37)
		.maxstack 2
		.locals init (
			[0] class [mscorlib]System.Collections.Generic.List`1<string>,
			[1] bool,
			[2] object
		)

		IL_0000: ldc.i4.0
		IL_0001: stloc.1
		.try
		{
			IL_0002: ldarg.0
			IL_0003: ldfld object Facebook.Messenger.WP8.FBGeoLocator.Service::_threadsLocationSettingsLock
			IL_0008: dup
			IL_0009: stloc.2
			IL_000a: ldloca.s 1
			IL_000c: call void [mscorlib]System.Threading.Monitor::Enter(object, bool&)
			IL_0011: ldarg.0
			IL_0012: call instance class [mscorlib]System.Collections.Generic.List`1<string> Facebook.Messenger.WP8.FBGeoLocator.Service::GetThreadsLocationSettings()
			IL_0017: stloc.0
			IL_0018: ldloc.0
			IL_0019: ldarg.1
			IL_001a: callvirt instance bool class [mscorlib]System.Collections.Generic.List`1<string>::Contains(!0)
			IL_001f: brfalse.s IL_0023

			IL_0021: leave.s IL_0036

			IL_0023: ldloc.0
			IL_0024: ldarg.1
			IL_0025: callvirt instance void class [mscorlib]System.Collections.Generic.List`1<string>::Add(!0)
			IL_002a: leave.s IL_0036
		} // end .try
		finally
		{
			IL_002c: ldloc.1
			IL_002d: brfalse.s IL_0035

			IL_002f: ldloc.2
			IL_0030: call void [mscorlib]System.Threading.Monitor::Exit(object)

			IL_0035: endfinally
		} // end handler

		IL_0036: ret
	} // end of method Service::DisableLocationForThread

	.method private hidebysig 
		instance class [mscorlib]System.Collections.Generic.List`1<string> GetThreadsLocationSettings () cil managed 
	{
		// Method begins at RVA 0x1c070
		// Header size: 12
		// Code size: 64 (0x40)
		.maxstack 2
		.locals init (
			[0] string
		)

		IL_0000: ldarg.0
		IL_0001: ldfld class [mscorlib]System.Collections.Generic.List`1<string> Facebook.Messenger.WP8.FBGeoLocator.Service::_threadsLocationSettings
		IL_0006: brtrue.s IL_0039

		IL_0008: ldarg.0
		IL_0009: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_000e: callvirt instance class ['Facebook-Models']Facebook.Base.Settings.IUserSettings ['Facebook-Models']Facebook.Models.FacebookContext::get_Settings()
		IL_0013: ldstr "ThreadsLocationSettings"
		IL_0018: callvirt instance !!0 ['Facebook-Models']Facebook.Base.Settings.IUserSettings::GetSetting<string>(string)
		IL_001d: stloc.0
		IL_001e: ldarg.0
		IL_001f: ldloc.0
		IL_0020: call bool [mscorlib]System.String::IsNullOrWhiteSpace(string)
		IL_0025: brfalse.s IL_002e

		IL_0027: newobj instance void class [mscorlib]System.Collections.Generic.List`1<string>::.ctor()
		IL_002c: br.s IL_0034

		IL_002e: ldloc.0
		IL_002f: call !!0 [Newtonsoft.Json]Newtonsoft.Json.JsonConvert::DeserializeObject<class [mscorlib]System.Collections.Generic.List`1<string>>(string)

		IL_0034: stfld class [mscorlib]System.Collections.Generic.List`1<string> Facebook.Messenger.WP8.FBGeoLocator.Service::_threadsLocationSettings

		IL_0039: ldarg.0
		IL_003a: ldfld class [mscorlib]System.Collections.Generic.List`1<string> Facebook.Messenger.WP8.FBGeoLocator.Service::_threadsLocationSettings
		IL_003f: ret
	} // end of method Service::GetThreadsLocationSettings

	.method private hidebysig 
		instance void SaveThreadsLocationSettings () cil managed 
	{
		// Method begins at RVA 0x1c0bc
		// Header size: 12
		// Code size: 211 (0xd3)
		.maxstack 3
		.locals init (
			[0] class [mscorlib]System.IO.StringWriter,
			[1] class [Newtonsoft.Json]Newtonsoft.Json.JsonTextWriter,
			[2] string,
			[3] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<string>
		)

		IL_0000: ldarg.0
		IL_0001: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_0006: callvirt instance bool ['Facebook-Models']Facebook.Models.FacebookContext::IsAuthenticated()
		IL_000b: brtrue.s IL_000e

		IL_000d: ret

		IL_000e: ldarg.0
		IL_000f: ldfld class [mscorlib]System.Collections.Generic.List`1<string> Facebook.Messenger.WP8.FBGeoLocator.Service::_threadsLocationSettings
		IL_0014: brfalse.s IL_0023

		IL_0016: ldarg.0
		IL_0017: ldfld class [mscorlib]System.Collections.Generic.List`1<string> Facebook.Messenger.WP8.FBGeoLocator.Service::_threadsLocationSettings
		IL_001c: call bool [System.Core]System.Linq.Enumerable::Any<string>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>)
		IL_0021: brtrue.s IL_004e

		IL_0023: ldarg.0
		IL_0024: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_0029: callvirt instance class ['Facebook-Models']Facebook.Base.Settings.IUserSettings ['Facebook-Models']Facebook.Models.FacebookContext::get_Settings()
		IL_002e: ldstr "ThreadsLocationSettings"
		IL_0033: callvirt instance void ['Facebook-Models']Facebook.Base.Settings.IUserSettings::RemoveSetting(string)
		IL_0038: ldarg.0
		IL_0039: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_003e: callvirt instance class ['Facebook-Models']Facebook.Base.Settings.IUserSettings ['Facebook-Models']Facebook.Models.FacebookContext::get_Settings()
		IL_0043: ldstr "ThreadsLocationVersion"
		IL_0048: callvirt instance void ['Facebook-Models']Facebook.Base.Settings.IUserSettings::RemoveSetting(string)
		IL_004d: ret

		IL_004e: newobj instance void [mscorlib]System.IO.StringWriter::.ctor()
		IL_0053: stloc.0
		IL_0054: ldloc.0
		IL_0055: newobj instance void [Newtonsoft.Json]Newtonsoft.Json.JsonTextWriter::.ctor(class [mscorlib]System.IO.TextWriter)
		IL_005a: stloc.1
		IL_005b: ldloc.1
		IL_005c: callvirt instance void [Newtonsoft.Json]Newtonsoft.Json.JsonWriter::WriteStartArray()
		IL_0061: ldarg.0
		IL_0062: ldfld class [mscorlib]System.Collections.Generic.List`1<string> Facebook.Messenger.WP8.FBGeoLocator.Service::_threadsLocationSettings
		IL_0067: callvirt instance valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<!0> class [mscorlib]System.Collections.Generic.List`1<string>::GetEnumerator()
		IL_006c: stloc.3
		.try
		{
			IL_006d: br.s IL_007e
			// loop start (head: IL_007e)
				IL_006f: ldloca.s 3
				IL_0071: call instance !0 valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<string>::get_Current()
				IL_0076: stloc.2
				IL_0077: ldloc.1
				IL_0078: ldloc.2
				IL_0079: callvirt instance void [Newtonsoft.Json]Newtonsoft.Json.JsonWriter::WriteValue(string)

				IL_007e: ldloca.s 3
				IL_0080: call instance bool valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<string>::MoveNext()
				IL_0085: brtrue.s IL_006f
			// end loop

			IL_0087: leave.s IL_0097
		} // end .try
		finally
		{
			IL_0089: ldloca.s 3
			IL_008b: constrained. valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<string>
			IL_0091: callvirt instance void [mscorlib]System.IDisposable::Dispose()
			IL_0096: endfinally
		} // end handler

		IL_0097: ldloc.1
		IL_0098: callvirt instance void [Newtonsoft.Json]Newtonsoft.Json.JsonWriter::WriteEndArray()
		IL_009d: ldarg.0
		IL_009e: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_00a3: callvirt instance class ['Facebook-Models']Facebook.Base.Settings.IUserSettings ['Facebook-Models']Facebook.Models.FacebookContext::get_Settings()
		IL_00a8: ldstr "ThreadsLocationSettings"
		IL_00ad: ldloc.0
		IL_00ae: callvirt instance string [mscorlib]System.Object::ToString()
		IL_00b3: callvirt instance void ['Facebook-Models']Facebook.Base.Settings.IUserSettings::SaveSetting(string, object)
		IL_00b8: ldarg.0
		IL_00b9: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.FBGeoLocator.Service::_context
		IL_00be: callvirt instance class ['Facebook-Models']Facebook.Base.Settings.IUserSettings ['Facebook-Models']Facebook.Models.FacebookContext::get_Settings()
		IL_00c3: ldstr "ThreadsLocationVersion"
		IL_00c8: ldstr "1.0"
		IL_00cd: callvirt instance void ['Facebook-Models']Facebook.Base.Settings.IUserSettings::SaveSetting(string, object)
		IL_00d2: ret
	} // end of method Service::SaveThreadsLocationSettings

	.method private hidebysig 
		instance void OnGeolocatorStatusChanged () cil managed 
	{
		// Method begins at RVA 0x1c1c3
		// Header size: 1
		// Code size: 26 (0x1a)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class ['Facebook-Base']Facebook.Base.IDispatcher ['Facebook-Services']Facebook.Services.BackgroundService::UiDispatcher
		IL_0006: ldarg.0
		IL_0007: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::'<OnGeolocatorStatusChanged>b__b'()
		IL_000d: newobj instance void [mscorlib]System.Action::.ctor(object, native int)
		IL_0012: ldc.i4.0
		IL_0013: callvirt instance class [mscorlib]System.Threading.Tasks.Task ['Facebook-Base']Facebook.Base.IDispatcher::RunAsync(class [mscorlib]System.Action, valuetype ['Facebook-Base']Facebook.Base.DispatcherPriority)
		IL_0018: pop
		IL_0019: ret
	} // end of method Service::OnGeolocatorStatusChanged

	.method private hidebysig 
		instance bool CanStartGeolocator () cil managed 
	{
		// Method begins at RVA 0x1c1de
		// Header size: 1
		// Code size: 19 (0x13)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldarg.0
		IL_0002: call instance bool Facebook.Messenger.WP8.FBGeoLocator.Service::GetDeviceLocationStatusEnabled()
		IL_0007: stfld bool Facebook.Messenger.WP8.FBGeoLocator.Service::_deviceLocationEnabled
		IL_000c: ldarg.0
		IL_000d: call instance bool Facebook.Messenger.WP8.FBGeoLocator.Service::IsLocationEnabled()
		IL_0012: ret
	} // end of method Service::CanStartGeolocator

	.method private hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task StartGeolocatorAsync () cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 46 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 46 42 47 65 6f 4c
			6f 63 61 74 6f 72 2e 53 65 72 76 69 63 65 2b 3c
			53 74 61 72 74 47 65 6f 6c 6f 63 61 74 6f 72 41
			73 79 6e 63 3e 64 5f 5f 64 00 00
		)
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0x1c624
		// Header size: 12
		// Code size: 58 (0x3a)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_000f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>t__builder'
		IL_0014: ldloca.s 0
		IL_0016: ldc.i4.m1
		IL_0017: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>1__state'
		IL_001c: ldloca.s 0
		IL_001e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>t__builder'
		IL_0023: stloc.1
		IL_0024: ldloca.s 1
		IL_0026: ldloca.s 0
		IL_0028: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'>(!!0&)
		IL_002d: ldloca.s 0
		IL_002f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StartGeolocatorAsync>d__d'::'<>t__builder'
		IL_0034: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0039: ret
	} // end of method Service::StartGeolocatorAsync

	.method private hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task StopGeolocatorAsync () cil managed 
	{
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 46 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 46 42 47 65 6f 4c
			6f 63 61 74 6f 72 2e 53 65 72 76 69 63 65 2b 3c
			53 74 6f 70 47 65 6f 6c 6f 63 61 74 6f 72 41 73
			79 6e 63 3e 64 5f 5f 31 34 00 00
		)
		// Method begins at RVA 0x1c808
		// Header size: 12
		// Code size: 58 (0x3a)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_000f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>t__builder'
		IL_0014: ldloca.s 0
		IL_0016: ldc.i4.m1
		IL_0017: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>1__state'
		IL_001c: ldloca.s 0
		IL_001e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>t__builder'
		IL_0023: stloc.1
		IL_0024: ldloca.s 1
		IL_0026: ldloca.s 0
		IL_0028: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'>(!!0&)
		IL_002d: ldloca.s 0
		IL_002f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<StopGeolocatorAsync>d__14'::'<>t__builder'
		IL_0034: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0039: ret
	} // end of method Service::StopGeolocatorAsync

	.method private hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task PerformWithGeolocatorLockAsync (
			class [mscorlib]System.Action action
		) cil managed 
	{
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 51 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 46 42 47 65 6f 4c
			6f 63 61 74 6f 72 2e 53 65 72 76 69 63 65 2b 3c
			50 65 72 66 6f 72 6d 57 69 74 68 47 65 6f 6c 6f
			63 61 74 6f 72 4c 6f 63 6b 41 73 79 6e 63 3e 64
			5f 5f 31 37 00 00
		)
		// Method begins at RVA 0x1c960
		// Header size: 12
		// Code size: 66 (0x42)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: ldarg.1
		IL_000b: stfld class [mscorlib]System.Action Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::action
		IL_0010: ldloca.s 0
		IL_0012: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_0017: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>t__builder'
		IL_001c: ldloca.s 0
		IL_001e: ldc.i4.m1
		IL_001f: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>1__state'
		IL_0024: ldloca.s 0
		IL_0026: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>t__builder'
		IL_002b: stloc.1
		IL_002c: ldloca.s 1
		IL_002e: ldloca.s 0
		IL_0030: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'>(!!0&)
		IL_0035: ldloca.s 0
		IL_0037: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<PerformWithGeolocatorLockAsync>d__17'::'<>t__builder'
		IL_003c: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0041: ret
	} // end of method Service::PerformWithGeolocatorLockAsync

	.method private hidebysig 
		instance void ReleaseGeocodeQueryLock () cil managed 
	{
		// Method begins at RVA 0x1c9ae
		// Header size: 1
		// Code size: 26 (0x1a)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class [mscorlib]System.Threading.SemaphoreSlim Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQueryLock
		IL_0006: callvirt instance int32 [mscorlib]System.Threading.SemaphoreSlim::get_CurrentCount()
		IL_000b: brtrue.s IL_0019

		IL_000d: ldarg.0
		IL_000e: ldfld class [mscorlib]System.Threading.SemaphoreSlim Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQueryLock
		IL_0013: callvirt instance int32 [mscorlib]System.Threading.SemaphoreSlim::Release()
		IL_0018: pop

		IL_0019: ret
	} // end of method Service::ReleaseGeocodeQueryLock

	.method private hidebysig 
		instance void InvalidateAndRefetchLocation () cil managed 
	{
		// Method begins at RVA 0x1c9c9
		// Header size: 1
		// Code size: 26 (0x1a)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldnull
		IL_0002: stfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_0007: ldarg.0
		IL_0008: ldc.i4.1
		IL_0009: stfld valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorStatus
		IL_000e: ldarg.0
		IL_000f: call instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.FBGeoLocator.Service::StartGeolocatorAsync()
		IL_0014: call void ['Facebook-Base']Facebook.Base.Extensions.TaskExtensions::DoNotWait(class [mscorlib]System.Threading.Tasks.Task)
		IL_0019: ret
	} // end of method Service::InvalidateAndRefetchLocation

	.method private hidebysig 
		instance void GeolocatorOnStatusChanged (
			class [Windows]Windows.Devices.Geolocation.Geolocator sender,
			class [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs args
		) cil managed 
	{
		// Method begins at RVA 0x1c9e4
		// Header size: 1
		// Code size: 40 (0x28)
		.maxstack 8

		IL_0000: ldarg.2
		IL_0001: callvirt instance valuetype [Windows]Windows.Devices.Geolocation.PositionStatus [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs::get_Status()
		IL_0006: ldc.i4.3
		IL_0007: bne.un.s IL_0012

		IL_0009: ldarg.0
		IL_000a: ldc.i4.0
		IL_000b: stfld bool Facebook.Messenger.WP8.FBGeoLocator.Service::_deviceLocationEnabled
		IL_0010: br.s IL_0021

		IL_0012: ldarg.2
		IL_0013: callvirt instance valuetype [Windows]Windows.Devices.Geolocation.PositionStatus [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs::get_Status()
		IL_0018: brtrue.s IL_0021

		IL_001a: ldarg.0
		IL_001b: ldc.i4.1
		IL_001c: stfld bool Facebook.Messenger.WP8.FBGeoLocator.Service::_deviceLocationEnabled

		IL_0021: ldarg.0
		IL_0022: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::OnGeolocatorStatusChanged()
		IL_0027: ret
	} // end of method Service::GeolocatorOnStatusChanged

	.method private hidebysig 
		instance void GeolocatorOnPositionChanged (
			class [Windows]Windows.Devices.Geolocation.Geolocator sender,
			class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs args
		) cil managed 
	{
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 4e 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 46 42 47 65 6f 4c
			6f 63 61 74 6f 72 2e 53 65 72 76 69 63 65 2b 3c
			47 65 6f 6c 6f 63 61 74 6f 72 4f 6e 50 6f 73 69
			74 69 6f 6e 43 68 61 6e 67 65 64 3e 64 5f 5f 31
			65 00 00
		)
		// Method begins at RVA 0x1cb7c
		// Header size: 12
		// Code size: 54 (0x36)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: ldarg.2
		IL_000b: stfld class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::args
		IL_0010: ldloca.s 0
		IL_0012: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Create()
		IL_0017: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>t__builder'
		IL_001c: ldloca.s 0
		IL_001e: ldc.i4.m1
		IL_001f: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>1__state'
		IL_0024: ldloca.s 0
		IL_0026: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'::'<>t__builder'
		IL_002b: stloc.1
		IL_002c: ldloca.s 1
		IL_002e: ldloca.s 0
		IL_0030: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Start<valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeolocatorOnPositionChanged>d__1e'>(!!0&)
		IL_0035: ret
	} // end of method Service::GeolocatorOnPositionChanged

	.method private hidebysig 
		instance void GeocodeQueryOnQueryCompleted (
			object sender,
			class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>> args
		) cil managed 
	{
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 4f 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 46 42 47 65 6f 4c
			6f 63 61 74 6f 72 2e 53 65 72 76 69 63 65 2b 3c
			47 65 6f 63 6f 64 65 51 75 65 72 79 4f 6e 51 75
			65 72 79 43 6f 6d 70 6c 65 74 65 64 3e 64 5f 5f
			32 34 00 00
		)
		// Method begins at RVA 0x1cd8c
		// Header size: 12
		// Code size: 54 (0x36)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: ldarg.2
		IL_000b: stfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>> Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::args
		IL_0010: ldloca.s 0
		IL_0012: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Create()
		IL_0017: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>t__builder'
		IL_001c: ldloca.s 0
		IL_001e: ldc.i4.m1
		IL_001f: stfld int32 Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>1__state'
		IL_0024: ldloca.s 0
		IL_0026: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'::'<>t__builder'
		IL_002b: stloc.1
		IL_002c: ldloca.s 1
		IL_002e: ldloca.s 0
		IL_0030: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Start<valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/'<GeocodeQueryOnQueryCompleted>d__24'>(!!0&)
		IL_0035: ret
	} // end of method Service::GeocodeQueryOnQueryCompleted

	.method private hidebysig 
		instance void UpdateCurrentLocationWithGeoposition (
			class [Windows]Windows.Devices.Geolocation.Geoposition position
		) cil managed 
	{
		// Method begins at RVA 0x1cdd0
		// Header size: 12
		// Code size: 150 (0x96)
		.maxstack 3
		.locals init (
			[0] float64,
			[1] float64
		)

		IL_0000: ldarg.1
		IL_0001: brfalse.s IL_000b

		IL_0003: ldarg.1
		IL_0004: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
		IL_0009: brtrue.s IL_000c

		IL_000b: ret

		IL_000c: ldarg.0
		IL_000d: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_0012: brtrue.s IL_001f

		IL_0014: ldarg.0
		IL_0015: newobj instance void ['Facebook-Models']Facebook.Models.FacebookPlace::.ctor()
		IL_001a: stfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation

		IL_001f: ldarg.0
		IL_0020: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_0025: ldarg.1
		IL_0026: callvirt instance class [Windows]Windows.Devices.Geolocation.CivicAddress [Windows]Windows.Devices.Geolocation.Geoposition::get_CivicAddress()
		IL_002b: brtrue.s IL_0034

		IL_002d: ldsfld string [mscorlib]System.String::Empty
		IL_0032: br.s IL_003f

		IL_0034: ldarg.1
		IL_0035: callvirt instance class [Windows]Windows.Devices.Geolocation.CivicAddress [Windows]Windows.Devices.Geolocation.Geoposition::get_CivicAddress()
		IL_003a: callvirt instance string [Windows]Windows.Devices.Geolocation.CivicAddress::get_City()

		IL_003f: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookPlace::set_Name(string)
		IL_0044: ldarg.0
		IL_0045: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_004a: ldarg.1
		IL_004b: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
		IL_0050: callvirt instance float64 [Windows]Windows.Devices.Geolocation.Geocoordinate::get_Latitude()
		IL_0055: stloc.0
		IL_0056: ldloca.s 0
		IL_0058: call class [mscorlib]System.Globalization.CultureInfo [mscorlib]System.Globalization.CultureInfo::get_InvariantCulture()
		IL_005d: call instance string [mscorlib]System.Double::ToString(class [mscorlib]System.IFormatProvider)
		IL_0062: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookPlace::set_Latitude(string)
		IL_0067: ldarg.0
		IL_0068: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_006d: ldarg.1
		IL_006e: callvirt instance class [Windows]Windows.Devices.Geolocation.Geocoordinate [Windows]Windows.Devices.Geolocation.Geoposition::get_Coordinate()
		IL_0073: callvirt instance float64 [Windows]Windows.Devices.Geolocation.Geocoordinate::get_Longitude()
		IL_0078: stloc.1
		IL_0079: ldloca.s 1
		IL_007b: call class [mscorlib]System.Globalization.CultureInfo [mscorlib]System.Globalization.CultureInfo::get_InvariantCulture()
		IL_0080: call instance string [mscorlib]System.Double::ToString(class [mscorlib]System.IFormatProvider)
		IL_0085: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookPlace::set_Longitude(string)
		IL_008a: ldarg.0
		IL_008b: call valuetype [mscorlib]System.DateTime [mscorlib]System.DateTime::get_UtcNow()
		IL_0090: stfld valuetype [mscorlib]System.DateTime Facebook.Messenger.WP8.FBGeoLocator.Service::_lastKnownGoodLocationTime
		IL_0095: ret
	} // end of method Service::UpdateCurrentLocationWithGeoposition

	.method private hidebysig 
		instance void UpdateCurrentLocationWithMapAddress (
			class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress address
		) cil managed 
	{
		// Method begins at RVA 0x1ce74
		// Header size: 12
		// Code size: 64 (0x40)
		.maxstack 2

		IL_0000: ldarg.0
		IL_0001: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_0006: brfalse.s IL_000b

		IL_0008: ldarg.1
		IL_0009: brtrue.s IL_000c

		IL_000b: ret

		IL_000c: ldarg.0
		IL_000d: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_0012: ldarg.1
		IL_0013: callvirt instance string [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress::get_CountryCode()
		IL_0018: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookPlace::set_CountryCode(string)
		IL_001d: ldarg.0
		IL_001e: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_0023: ldarg.1
		IL_0024: callvirt instance string [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress::get_State()
		IL_0029: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookPlace::set_State(string)
		IL_002e: ldarg.0
		IL_002f: ldfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_0034: ldarg.1
		IL_0035: callvirt instance string [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapAddress::get_City()
		IL_003a: callvirt instance void ['Facebook-Models']Facebook.Models.FacebookPlace::set_Name(string)
		IL_003f: ret
	} // end of method Service::UpdateCurrentLocationWithMapAddress

	.method private hidebysig 
		instance void ReverseGeocodeWithGeoposition (
			class [Windows]Windows.Devices.Geolocation.Geoposition position
		) cil managed 
	{
		// Method begins at RVA 0x1d218
		// Header size: 12
		// Code size: 46 (0x2e)
		.maxstack 3
		.locals init (
			[0] class Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'
		)

		IL_0000: newobj instance void Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::.ctor()
		IL_0005: stloc.0
		IL_0006: ldloc.0
		IL_0007: ldarg.1
		IL_0008: stfld class [Windows]Windows.Devices.Geolocation.Geoposition Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::position
		IL_000d: ldloc.0
		IL_000e: ldarg.0
		IL_000f: stfld class Facebook.Messenger.WP8.FBGeoLocator.Service Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<>4__this'
		IL_0014: ldarg.0
		IL_0015: ldfld class ['Facebook-Base']Facebook.Base.IDispatcher ['Facebook-Services']Facebook.Services.BackgroundService::UiDispatcher
		IL_001a: ldloc.0
		IL_001b: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service/'<>c__DisplayClass29'::'<ReverseGeocodeWithGeoposition>b__28'()
		IL_0021: newobj instance void [mscorlib]System.Action::.ctor(object, native int)
		IL_0026: ldc.i4.m1
		IL_0027: callvirt instance class [mscorlib]System.Threading.Tasks.Task ['Facebook-Base']Facebook.Base.IDispatcher::RunAsync(class [mscorlib]System.Action, valuetype ['Facebook-Base']Facebook.Base.DispatcherPriority)
		IL_002c: pop
		IL_002d: ret
	} // end of method Service::ReverseGeocodeWithGeoposition

	.method private hidebysig 
		instance void StartRefetchLocationTimer () cil managed 
	{
		// Method begins at RVA 0x1d254
		// Header size: 12
		// Code size: 61 (0x3d)
		.maxstack 5
		.locals init (
			[0] valuetype [mscorlib]System.TimeSpan,
			[1] int32
		)

		IL_0000: ldarg.0
		IL_0001: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::StopRefetchLocationTimer()
		IL_0006: ldc.r8 9
		IL_000f: call valuetype [mscorlib]System.TimeSpan [mscorlib]System.TimeSpan::FromMinutes(float64)
		IL_0014: stloc.0
		IL_0015: ldloca.s 0
		IL_0017: call instance float64 [mscorlib]System.TimeSpan::get_TotalMilliseconds()
		IL_001c: call int32 [mscorlib]System.Convert::ToInt32(float64)
		IL_0021: stloc.1
		IL_0022: ldarg.0
		IL_0023: ldarg.0
		IL_0024: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::RefetchLocationTimerCallback(object)
		IL_002a: newobj instance void [mscorlib]System.Threading.TimerCallback::.ctor(object, native int)
		IL_002f: ldnull
		IL_0030: ldloc.1
		IL_0031: ldloc.1
		IL_0032: newobj instance void [mscorlib]System.Threading.Timer::.ctor(class [mscorlib]System.Threading.TimerCallback, object, int32, int32)
		IL_0037: stfld class [mscorlib]System.Threading.Timer Facebook.Messenger.WP8.FBGeoLocator.Service::_refetchLocationTimer
		IL_003c: ret
	} // end of method Service::StartRefetchLocationTimer

	.method private hidebysig 
		instance void StopRefetchLocationTimer () cil managed 
	{
		// Method begins at RVA 0x1d29d
		// Header size: 1
		// Code size: 28 (0x1c)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class [mscorlib]System.Threading.Timer Facebook.Messenger.WP8.FBGeoLocator.Service::_refetchLocationTimer
		IL_0006: brtrue.s IL_0009

		IL_0008: ret

		IL_0009: ldarg.0
		IL_000a: ldfld class [mscorlib]System.Threading.Timer Facebook.Messenger.WP8.FBGeoLocator.Service::_refetchLocationTimer
		IL_000f: callvirt instance void [mscorlib]System.Threading.Timer::Dispose()
		IL_0014: ldarg.0
		IL_0015: ldnull
		IL_0016: stfld class [mscorlib]System.Threading.Timer Facebook.Messenger.WP8.FBGeoLocator.Service::_refetchLocationTimer
		IL_001b: ret
	} // end of method Service::StopRefetchLocationTimer

	.method private hidebysig 
		instance void RefetchLocationTimerCallback (
			object state
		) cil managed 
	{
		// Method begins at RVA 0x1d2ba
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class [mscorlib]System.Threading.Timer Facebook.Messenger.WP8.FBGeoLocator.Service::_refetchLocationTimer
		IL_0006: brtrue.s IL_0009

		IL_0008: ret

		IL_0009: ldarg.0
		IL_000a: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::InvalidateAndRefetchLocation()
		IL_000f: ret
	} // end of method Service::RefetchLocationTimerCallback

	.method private hidebysig 
		instance void '<OnGeolocatorStatusChanged>b__b' () cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0x1c1ac
		// Header size: 1
		// Code size: 22 (0x16)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class [mscorlib]System.EventHandler Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorStatusChanged
		IL_0006: brfalse.s IL_0015

		IL_0008: ldarg.0
		IL_0009: ldfld class [mscorlib]System.EventHandler Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorStatusChanged
		IL_000e: ldarg.0
		IL_000f: ldnull
		IL_0010: callvirt instance void [mscorlib]System.EventHandler::Invoke(object, class [mscorlib]System.EventArgs)

		IL_0015: ret
	} // end of method Service::'<OnGeolocatorStatusChanged>b__b'

	.method private hidebysig 
		instance void '<StopGeolocatorAsync>b__13' () cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0x1c66c
		// Header size: 12
		// Code size: 174 (0xae)
		.maxstack 3

		IL_0000: ldarg.0
		IL_0001: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::StopRefetchLocationTimer()
		IL_0006: ldarg.0
		IL_0007: ldc.i4.0
		IL_0008: stfld valuetype Facebook.Messenger.WP8.FBGeoLocator.Service/GeolocatorStatus Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocatorStatus
		IL_000d: ldarg.0
		IL_000e: ldfld class [Windows]Windows.Devices.Geolocation.Geolocator Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocator
		IL_0013: brfalse.s IL_0060

		IL_0015: ldarg.0
		IL_0016: ldfld class [Windows]Windows.Devices.Geolocation.Geolocator Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocator
		IL_001b: ldftn instance void [Windows]Windows.Devices.Geolocation.Geolocator::remove_PositionChanged(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
		IL_0021: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_0026: ldarg.0
		IL_0027: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorOnPositionChanged(class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs)
		IL_002d: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs>::.ctor(object, native int)
		IL_0032: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::RemoveEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.PositionChangedEventArgs>>(class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
		IL_0037: ldarg.0
		IL_0038: ldfld class [Windows]Windows.Devices.Geolocation.Geolocator Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocator
		IL_003d: ldftn instance void [Windows]Windows.Devices.Geolocation.Geolocator::remove_StatusChanged(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
		IL_0043: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_0048: ldarg.0
		IL_0049: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::GeolocatorOnStatusChanged(class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs)
		IL_004f: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs>::.ctor(object, native int)
		IL_0054: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::RemoveEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Devices.Geolocation.Geolocator, class [Windows]Windows.Devices.Geolocation.StatusChangedEventArgs>>(class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
		IL_0059: ldarg.0
		IL_005a: ldnull
		IL_005b: stfld class [Windows]Windows.Devices.Geolocation.Geolocator Facebook.Messenger.WP8.FBGeoLocator.Service::_geolocator

		IL_0060: ldarg.0
		IL_0061: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQuery
		IL_0066: brfalse.s IL_008c

		IL_0068: ldarg.0
		IL_0069: ldfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQuery
		IL_006e: ldarg.0
		IL_006f: ldftn instance void Facebook.Messenger.WP8.FBGeoLocator.Service::GeocodeQueryOnQueryCompleted(object, class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>)
		IL_0075: newobj instance void class [mscorlib]System.EventHandler`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>>::.ctor(object, native int)
		IL_007a: callvirt instance void class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.Query`1<class [mscorlib]System.Collections.Generic.IList`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.MapLocation>>::remove_QueryCompleted(class [mscorlib]System.EventHandler`1<class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.QueryCompletedEventArgs`1<!0>>)
		IL_007f: ldarg.0
		IL_0080: ldnull
		IL_0081: stfld class [Microsoft.Phone.Maps]Microsoft.Phone.Maps.Services.ReverseGeocodeQuery Facebook.Messenger.WP8.FBGeoLocator.Service::_geocodeQuery
		IL_0086: ldarg.0
		IL_0087: call instance void Facebook.Messenger.WP8.FBGeoLocator.Service::ReleaseGeocodeQueryLock()

		IL_008c: ldarg.0
		IL_008d: ldfld class [mscorlib]System.Threading.CancellationTokenSource Facebook.Messenger.WP8.FBGeoLocator.Service::_cancellationTokenSource
		IL_0092: brfalse.s IL_00a6

		IL_0094: ldarg.0
		IL_0095: ldfld class [mscorlib]System.Threading.CancellationTokenSource Facebook.Messenger.WP8.FBGeoLocator.Service::_cancellationTokenSource
		IL_009a: callvirt instance void [mscorlib]System.Threading.CancellationTokenSource::Cancel()
		IL_009f: ldarg.0
		IL_00a0: ldnull
		IL_00a1: stfld class [mscorlib]System.Threading.CancellationTokenSource Facebook.Messenger.WP8.FBGeoLocator.Service::_cancellationTokenSource

		IL_00a6: ldarg.0
		IL_00a7: ldnull
		IL_00a8: stfld class ['Facebook-Models']Facebook.Models.FacebookPlace Facebook.Messenger.WP8.FBGeoLocator.Service::_currentLocation
		IL_00ad: ret
	} // end of method Service::'<StopGeolocatorAsync>b__13'

	// Events
	.event [mscorlib]System.EventHandler GeolocatorStatusChanged
	{
		.addon instance void Facebook.Messenger.WP8.FBGeoLocator.Service::add_GeolocatorStatusChanged(class [mscorlib]System.EventHandler)
		.removeon instance void Facebook.Messenger.WP8.FBGeoLocator.Service::remove_GeolocatorStatusChanged(class [mscorlib]System.EventHandler)
	}


} // end of class Facebook.Messenger.WP8.FBGeoLocator.Service
