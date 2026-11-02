// IL code: Facebook.Messenger.WP8
.class nested family auto ansi beforefieldinit CameraState
	extends [mscorlib]System.Object
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<InitializeAsync>d__1f'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.Controls.CameraView/CameraState '<>4__this'
		.field public bool resetCamera
		.field public class [mscorlib]System.Collections.Generic.IReadOnlyList`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size> '<resolutions>5__20'
		.field public valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size '<captureResolution>5__21'
		.field public uint32 '<sensorRotation>5__22'
		.field public int64 '<rotation>5__23'
		.field public class [System.Windows]System.Windows.Media.VideoBrush '<>g__initLocal1e'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter24'
		.field private object '<>t__stack'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice> '<>u__$awaiter25'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0xf268
			// Header size: 12
			// Code size: 764 (0x2fc)
			.maxstack 5
			.locals init (
				[0] class [mscorlib]System.Exception,
				[1] bool,
				[2] class [mscorlib]System.Exception,
				[3] int32,
				[4] int32,
				[5] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[6] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[7] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice>,
				[8] class Facebook.Messenger.WP8.Controls.CameraView/CameraState,
				[9] class Facebook.Messenger.WP8.Controls.CameraView/CameraState,
				[10] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice>
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.1
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>1__state'
				IL_0008: stloc.3
				IL_0009: ldloc.3
				IL_000a: switch (IL_0017, IL_0017)
				.try
				{
					IL_0017: ldarg.0
					IL_0018: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>1__state'
					IL_001d: stloc.s 4
					IL_001f: ldloc.s 4
					IL_0021: switch (IL_00a1, IL_0156)

					IL_002e: ldarg.0
					IL_002f: ldfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::resetCamera
					IL_0034: brtrue.s IL_0061

					IL_0036: ldarg.0
					IL_0037: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_003c: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
					IL_0041: brfalse.s IL_0061

					IL_0043: ldarg.0
					IL_0044: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0049: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
					IL_004e: brfalse.s IL_0061

					IL_0050: ldarg.0
					IL_0051: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0056: ldfld valuetype Facebook.Messenger.WP8.Controls.CameraView/State Facebook.Messenger.WP8.Controls.CameraView/CameraState::CurrentState
					IL_005b: ldc.i4.4
					IL_005c: bne.un IL_023c

					IL_0061: ldarg.0
					IL_0062: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0067: ldc.i4.1
					IL_0068: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.Controls.CameraView/CameraState::CleanUp(bool)
					IL_006d: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
					IL_0072: stloc.s 5
					IL_0074: ldloca.s 5
					IL_0076: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
					IL_007b: brtrue.s IL_00c0

					IL_007d: ldarg.0
					IL_007e: ldc.i4.0
					IL_007f: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>1__state'
					IL_0084: ldarg.0
					IL_0085: ldloc.s 5
					IL_0087: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>u__$awaiter24'
					IL_008c: ldarg.0
					IL_008d: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__builder'
					IL_0092: ldloca.s 5
					IL_0094: ldarg.0
					IL_0095: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'>(!!0&, !!1&)
					IL_009a: ldc.i4.0
					IL_009b: stloc.1
					IL_009c: leave IL_02fb

					IL_00a1: ldarg.0
					IL_00a2: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>u__$awaiter24'
					IL_00a7: stloc.s 5
					IL_00a9: ldarg.0
					IL_00aa: ldloca.s 6
					IL_00ac: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
					IL_00b2: ldloc.s 6
					IL_00b4: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>u__$awaiter24'
					IL_00b9: ldarg.0
					IL_00ba: ldc.i4.m1
					IL_00bb: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>1__state'

					IL_00c0: ldloca.s 5
					IL_00c2: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
					IL_00c7: ldloca.s 5
					IL_00c9: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
					IL_00cf: ldarg.0
					IL_00d0: ldarg.0
					IL_00d1: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_00d6: ldfld valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation Facebook.Messenger.WP8.Controls.CameraView/CameraState::_sensorLocation
					IL_00db: call class [mscorlib]System.Collections.Generic.IReadOnlyList`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size> [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::GetAvailableCaptureResolutions(valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation)
					IL_00e0: stfld class [mscorlib]System.Collections.Generic.IReadOnlyList`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<resolutions>5__20'
					IL_00e5: ldarg.0
					IL_00e6: ldarg.0
					IL_00e7: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_00ec: ldarg.0
					IL_00ed: ldfld class [mscorlib]System.Collections.Generic.IReadOnlyList`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<resolutions>5__20'
					IL_00f2: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size Facebook.Messenger.WP8.Controls.CameraView/CameraState::GetResolution(class [mscorlib]System.Collections.Generic.IReadOnlyList`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>)
					IL_00f7: stfld valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<captureResolution>5__21'
					IL_00fc: ldarg.0
					IL_00fd: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0102: ldarg.0
					IL_0103: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0108: ldfld valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation Facebook.Messenger.WP8.Controls.CameraView/CameraState::_sensorLocation
					IL_010d: ldarg.0
					IL_010e: ldfld valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<captureResolution>5__21'
					IL_0113: call class [Windows]Windows.Foundation.IAsyncOperation`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice> [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::OpenAsync(valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation, valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size)
					IL_0118: call valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!!0> [System.Runtime.WindowsRuntime]System.WindowsRuntimeSystemExtensions::GetAwaiter<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice>(class [Windows]Windows.Foundation.IAsyncOperation`1<!!0>)
					IL_011d: stloc.s 7
					IL_011f: ldloca.s 7
					IL_0121: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice>::get_IsCompleted()
					IL_0126: brtrue.s IL_018b

					IL_0128: stloc.s 8
					IL_012a: ldarg.0
					IL_012b: ldloc.s 8
					IL_012d: stfld object Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__stack'
					IL_0132: ldarg.0
					IL_0133: ldc.i4.1
					IL_0134: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>1__state'
					IL_0139: ldarg.0
					IL_013a: ldloc.s 7
					IL_013c: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>u__$awaiter25'
					IL_0141: ldarg.0
					IL_0142: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__builder'
					IL_0147: ldloca.s 7
					IL_0149: ldarg.0
					IL_014a: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice>, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'>(!!0&, !!1&)
					IL_014f: ldc.i4.0
					IL_0150: stloc.1
					IL_0151: leave IL_02fb

					IL_0156: ldarg.0
					IL_0157: ldfld object Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__stack'
					IL_015c: unbox.any Facebook.Messenger.WP8.Controls.CameraView/CameraState
					IL_0161: stloc.s 9
					IL_0163: ldloc.s 9
					IL_0165: ldarg.0
					IL_0166: ldnull
					IL_0167: stfld object Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__stack'
					IL_016c: ldarg.0
					IL_016d: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>u__$awaiter25'
					IL_0172: stloc.s 7
					IL_0174: ldarg.0
					IL_0175: ldloca.s 10
					IL_0177: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice>
					IL_017d: ldloc.s 10
					IL_017f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>u__$awaiter25'
					IL_0184: ldarg.0
					IL_0185: ldc.i4.m1
					IL_0186: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>1__state'

					IL_018b: ldloca.s 7
					IL_018d: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice>::GetResult()
					IL_0192: ldloca.s 7
					IL_0194: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice>
					IL_019a: stfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
					IL_019f: ldarg.0
					IL_01a0: ldarg.0
					IL_01a1: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_01a6: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
					IL_01ab: callvirt instance uint32 [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::get_SensorRotationInDegrees()
					IL_01b0: stfld uint32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<sensorRotation>5__22'
					IL_01b5: ldarg.0
					IL_01b6: ldarg.0
					IL_01b7: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_01bc: ldfld valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation Facebook.Messenger.WP8.Controls.CameraView/CameraState::_sensorLocation
					IL_01c1: brfalse.s IL_01cd

					IL_01c3: ldarg.0
					IL_01c4: ldfld uint32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<sensorRotation>5__22'
					IL_01c9: conv.u8
					IL_01ca: neg
					IL_01cb: br.s IL_01d4

					IL_01cd: ldarg.0
					IL_01ce: ldfld uint32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<sensorRotation>5__22'
					IL_01d3: conv.u8

					IL_01d4: stfld int64 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<rotation>5__23'
					IL_01d9: ldarg.0
					IL_01da: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_01df: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
					IL_01e4: call valuetype [mscorlib]System.Guid [Windows]Windows.Phone.Media.Capture.KnownCameraGeneralProperties::get_SpecifiedCaptureOrientation()
					IL_01e9: ldarg.0
					IL_01ea: ldfld int64 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<rotation>5__23'
					IL_01ef: box [mscorlib]System.Int64
					IL_01f4: callvirt instance void [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::SetProperty(valuetype [mscorlib]System.Guid, object)
					IL_01f9: ldarg.0
					IL_01fa: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_01ff: ldarg.0
					IL_0200: newobj instance void [System.Windows]System.Windows.Media.VideoBrush::.ctor()
					IL_0205: stfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>g__initLocal1e'
					IL_020a: ldarg.0
					IL_020b: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>g__initLocal1e'
					IL_0210: ldc.i4.0
					IL_0211: callvirt instance void [System.Windows]System.Windows.Media.TileBrush::set_Stretch(valuetype [System.Windows]System.Windows.Media.Stretch)
					IL_0216: ldarg.0
					IL_0217: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>g__initLocal1e'
					IL_021c: stfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
					IL_0221: ldarg.0
					IL_0222: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0227: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
					IL_022c: ldarg.0
					IL_022d: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0232: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
					IL_0237: callvirt instance void [System.Windows]System.Windows.Controls.Panel::set_Background(class [System.Windows]System.Windows.Media.Brush)

					IL_023c: ldarg.0
					IL_023d: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0242: callvirt instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::SetTransform()
					IL_0247: ldarg.0
					IL_0248: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_024d: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
					IL_0252: ldarg.0
					IL_0253: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0258: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
					IL_025d: call void [Microsoft.Phone]Microsoft.Devices.CameraVideoBrushExtensions::SetSource(class [System.Windows]System.Windows.Media.VideoBrush, object)
					IL_0262: ldarg.0
					IL_0263: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0268: ldc.i4.2
					IL_0269: stfld valuetype Facebook.Messenger.WP8.Controls.CameraView/State Facebook.Messenger.WP8.Controls.CameraView/CameraState::CurrentState
					IL_026e: leave.s IL_02cf
				} // end .try
				catch [mscorlib]System.Exception
				{
					IL_0270: stloc.0
					IL_0271: ldarg.0
					IL_0272: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0277: ldc.i4.0
					IL_0278: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.Controls.CameraView/CameraState::CleanUp(bool)
					IL_027d: call void ['Facebook-Base']Facebook.Base.Extensions.TaskExtensions::DoNotWait(class [mscorlib]System.Threading.Tasks.Task)
					IL_0282: ldarg.0
					IL_0283: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_0288: ldstr "Error initializing camera"
					IL_028d: ldstr "InitializeAsync"
					IL_0292: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
					IL_0297: ldc.i4 145
					IL_029c: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
					IL_02a1: ldarg.0
					IL_02a2: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_02a7: ldloc.0
					IL_02a8: callvirt instance string [mscorlib]System.Object::ToString()
					IL_02ad: ldstr "InitializeAsync"
					IL_02b2: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
					IL_02b7: ldc.i4 146
					IL_02bc: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
					IL_02c1: ldarg.0
					IL_02c2: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
					IL_02c7: ldc.i4.4
					IL_02c8: stfld valuetype Facebook.Messenger.WP8.Controls.CameraView/State Facebook.Messenger.WP8.Controls.CameraView/CameraState::CurrentState
					IL_02cd: leave.s IL_02cf
				} // end handler

				IL_02cf: leave.s IL_02e8
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_02d1: stloc.2
				IL_02d2: ldarg.0
				IL_02d3: ldc.i4.s -2
				IL_02d5: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>1__state'
				IL_02da: ldarg.0
				IL_02db: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__builder'
				IL_02e0: ldloc.2
				IL_02e1: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_02e6: leave.s IL_02fb
			} // end handler

			IL_02e8: ldarg.0
			IL_02e9: ldc.i4.s -2
			IL_02eb: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>1__state'
			IL_02f0: ldarg.0
			IL_02f1: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__builder'
			IL_02f6: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_02fb: ret
		} // end of method '<InitializeAsync>d__1f'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0xf5a4
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<InitializeAsync>d__1f'::SetStateMachine

	} // end of class <InitializeAsync>d__1f

	.class nested private auto ansi sealed beforefieldinit '<CleanUp>d__27'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.Controls.CameraView/CameraState '<>4__this'
		.field public bool resetState
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter28'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0xf604
			// Header size: 12
			// Code size: 410 (0x19a)
			.maxstack 5
			.locals init (
				[0] class [mscorlib]System.Exception,
				[1] bool,
				[2] class [mscorlib]System.Exception,
				[3] int32,
				[4] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[5] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.1
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>1__state'
				IL_0008: stloc.3
				IL_0009: ldloc.3
				IL_000a: ldc.i4.0
				IL_000b: beq.s IL_005c

				IL_000d: ldarg.0
				IL_000e: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
				IL_0013: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
				IL_0018: brfalse IL_0128

				IL_001d: ldarg.0
				IL_001e: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
				IL_0023: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.Controls.CameraView/CameraState::StopRecordingAsync()
				IL_0028: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_002d: stloc.s 4
				IL_002f: ldloca.s 4
				IL_0031: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_0036: brtrue.s IL_007b

				IL_0038: ldarg.0
				IL_0039: ldc.i4.0
				IL_003a: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>1__state'
				IL_003f: ldarg.0
				IL_0040: ldloc.s 4
				IL_0042: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>u__$awaiter28'
				IL_0047: ldarg.0
				IL_0048: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>t__builder'
				IL_004d: ldloca.s 4
				IL_004f: ldarg.0
				IL_0050: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'>(!!0&, !!1&)
				IL_0055: ldc.i4.0
				IL_0056: stloc.1
				IL_0057: leave IL_0199

				IL_005c: ldarg.0
				IL_005d: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>u__$awaiter28'
				IL_0062: stloc.s 4
				IL_0064: ldarg.0
				IL_0065: ldloca.s 5
				IL_0067: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_006d: ldloc.s 5
				IL_006f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>u__$awaiter28'
				IL_0074: ldarg.0
				IL_0075: ldc.i4.m1
				IL_0076: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>1__state'

				IL_007b: ldloca.s 4
				IL_007d: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_0082: ldloca.s 4
				IL_0084: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				.try
				{
					.try
					{
						IL_008a: ldarg.0
						IL_008b: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
						IL_0090: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
						IL_0095: callvirt instance void [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::Dispose()
						IL_009a: leave.s IL_00ea
					} // end .try
					catch [mscorlib]System.Exception
					{
						IL_009c: stloc.0
						IL_009d: ldarg.0
						IL_009e: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
						IL_00a3: ldstr "Error disposing camera"
						IL_00a8: ldstr "CleanUp"
						IL_00ad: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
						IL_00b2: ldc.i4 162
						IL_00b7: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
						IL_00bc: ldarg.0
						IL_00bd: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
						IL_00c2: ldloc.0
						IL_00c3: callvirt instance string [mscorlib]System.Object::ToString()
						IL_00c8: ldstr "CleanUp"
						IL_00cd: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
						IL_00d2: ldc.i4 163
						IL_00d7: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
						IL_00dc: ldarg.0
						IL_00dd: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
						IL_00e2: ldc.i4.4
						IL_00e3: stfld valuetype Facebook.Messenger.WP8.Controls.CameraView/State Facebook.Messenger.WP8.Controls.CameraView/CameraState::CurrentState
						IL_00e8: leave.s IL_00ea
					} // end handler

					IL_00ea: leave.s IL_0128
				} // end .try
				finally
				{
					IL_00ec: ldloc.1
					IL_00ed: brfalse.s IL_0127

					IL_00ef: ldarg.0
					IL_00f0: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
					IL_00f5: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
					IL_00fa: ldftn instance void [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::remove_PreviewFrameAvailable(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
					IL_0100: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
					IL_0105: ldarg.0
					IL_0106: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
					IL_010b: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::OnLastFrameAvailable(class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object)
					IL_0111: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>::.ctor(object, native int)
					IL_0116: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::RemoveEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>>(class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
					IL_011b: ldarg.0
					IL_011c: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
					IL_0121: ldnull
					IL_0122: stfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice

					IL_0127: endfinally
				} // end handler

				IL_0128: ldarg.0
				IL_0129: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
				IL_012e: ldnull
				IL_012f: stfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
				IL_0134: ldarg.0
				IL_0135: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
				IL_013a: ldnull
				IL_013b: stfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPixels
				IL_0140: ldarg.0
				IL_0141: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
				IL_0146: callvirt instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::CleanupLastFrame()
				IL_014b: ldarg.0
				IL_014c: ldfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::resetState
				IL_0151: brfalse.s IL_016d

				IL_0153: ldarg.0
				IL_0154: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
				IL_0159: ldfld valuetype Facebook.Messenger.WP8.Controls.CameraView/State Facebook.Messenger.WP8.Controls.CameraView/CameraState::CurrentState
				IL_015e: ldc.i4.4
				IL_015f: beq.s IL_016d

				IL_0161: ldarg.0
				IL_0162: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
				IL_0167: ldc.i4.0
				IL_0168: stfld valuetype Facebook.Messenger.WP8.Controls.CameraView/State Facebook.Messenger.WP8.Controls.CameraView/CameraState::CurrentState

				IL_016d: leave.s IL_0186
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_016f: stloc.2
				IL_0170: ldarg.0
				IL_0171: ldc.i4.s -2
				IL_0173: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>1__state'
				IL_0178: ldarg.0
				IL_0179: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>t__builder'
				IL_017e: ldloc.2
				IL_017f: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_0184: leave.s IL_0199
			} // end handler

			IL_0186: ldarg.0
			IL_0187: ldc.i4.s -2
			IL_0189: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>1__state'
			IL_018e: ldarg.0
			IL_018f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>t__builder'
			IL_0194: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_0199: ret
		} // end of method '<CleanUp>d__27'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0xf7f8
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<CleanUp>d__27'::SetStateMachine

	} // end of class <CleanUp>d__27

	.class nested private auto ansi sealed beforefieldinit '<StartVideoCaptureAsync>d__30'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.Controls.CameraView/CameraState '<>4__this'
		.field public class ['Facebook-Models']Facebook.Models.FacebookContext '<context>5__31'
		.field public string '<userFolderPath>5__32'
		.field public class [Windows]Windows.Storage.StorageFolder '<folder>5__33'
		.field public class [Windows]Windows.Storage.StorageFile '<file>5__34'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string> '<>u__$awaiter35'
		.field private object '<>t__stack'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder> '<>u__$awaiter36'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile> '<>u__$awaiter37'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream> '<>u__$awaiter38'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter39'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0xfcb4
			// Header size: 12
			// Code size: 980 (0x3d4)
			.maxstack 5
			.locals init (
				[0] class [mscorlib]System.Exception,
				[1] bool,
				[2] class [mscorlib]System.Exception,
				[3] int32,
				[4] int32,
				[5] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>,
				[6] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>,
				[7] string,
				[8] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>,
				[9] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>,
				[10] class [Windows]Windows.Storage.StorageFolder,
				[11] class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice,
				[12] valuetype [mscorlib]System.DateTime,
				[13] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>,
				[14] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>,
				[15] class [Windows]Windows.Storage.StorageFile,
				[16] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream>,
				[17] class Facebook.Messenger.WP8.Controls.CameraView/CameraState,
				[18] class Facebook.Messenger.WP8.Controls.CameraView/CameraState,
				[19] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream>,
				[20] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[21] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.1
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
				IL_0008: stloc.3
				IL_0009: ldloc.3
				IL_000a: switch (IL_0033, IL_0033, IL_0033, IL_0033, IL_0033)

				IL_0023: ldarg.0
				IL_0024: call class [Microsoft.Practices.ServiceLocation]Microsoft.Practices.ServiceLocation.IServiceLocator [Microsoft.Practices.ServiceLocation]Microsoft.Practices.ServiceLocation.ServiceLocator::get_Current()
				IL_0029: callvirt instance !!0 [Microsoft.Practices.ServiceLocation]Microsoft.Practices.ServiceLocation.IServiceLocator::GetInstance<class ['Facebook-Models']Facebook.Models.FacebookContext>()
				IL_002e: stfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<context>5__31'
				.try
				{
					IL_0033: ldarg.0
					IL_0034: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
					IL_0039: stloc.s 4
					IL_003b: ldloc.s 4
					IL_003d: switch (IL_009a, IL_0111, IL_01e3, IL_0281, IL_0319)

					IL_0056: ldarg.0
					IL_0057: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<context>5__31'
					IL_005c: callvirt instance class ['Facebook-Models']Facebook.Base.Settings.IUserSettings ['Facebook-Models']Facebook.Models.FacebookContext::get_Settings()
					IL_0061: callvirt instance class [mscorlib]System.Threading.Tasks.Task`1<string> ['Facebook-Models']Facebook.Base.Settings.IUserSettings::GetUserFolderPath()
					IL_0066: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!0> class [mscorlib]System.Threading.Tasks.Task`1<string>::GetAwaiter()
					IL_006b: stloc.s 5
					IL_006d: ldloca.s 5
					IL_006f: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>::get_IsCompleted()
					IL_0074: brtrue.s IL_00b9

					IL_0076: ldarg.0
					IL_0077: ldc.i4.0
					IL_0078: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
					IL_007d: ldarg.0
					IL_007e: ldloc.s 5
					IL_0080: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter35'
					IL_0085: ldarg.0
					IL_0086: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
					IL_008b: ldloca.s 5
					IL_008d: ldarg.0
					IL_008e: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'>(!!0&, !!1&)
					IL_0093: ldc.i4.0
					IL_0094: stloc.1
					IL_0095: leave IL_03d3

					IL_009a: ldarg.0
					IL_009b: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter35'
					IL_00a0: stloc.s 5
					IL_00a2: ldarg.0
					IL_00a3: ldloca.s 6
					IL_00a5: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>
					IL_00ab: ldloc.s 6
					IL_00ad: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter35'
					IL_00b2: ldarg.0
					IL_00b3: ldc.i4.m1
					IL_00b4: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'

					IL_00b9: ldloca.s 5
					IL_00bb: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>::GetResult()
					IL_00c0: ldloca.s 5
					IL_00c2: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>
					IL_00c8: stloc.s 7
					IL_00ca: ldarg.0
					IL_00cb: ldloc.s 7
					IL_00cd: stfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<userFolderPath>5__32'
					IL_00d2: ldarg.0
					IL_00d3: ldfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<userFolderPath>5__32'
					IL_00d8: call class [Windows]Windows.Foundation.IAsyncOperation`1<class [Windows]Windows.Storage.StorageFolder> [Windows]Windows.Storage.StorageFolder::GetFolderFromPathAsync(string)
					IL_00dd: call valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!!0> [System.Runtime.WindowsRuntime]System.WindowsRuntimeSystemExtensions::GetAwaiter<class [Windows]Windows.Storage.StorageFolder>(class [Windows]Windows.Foundation.IAsyncOperation`1<!!0>)
					IL_00e2: stloc.s 8
					IL_00e4: ldloca.s 8
					IL_00e6: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>::get_IsCompleted()
					IL_00eb: brtrue.s IL_0130

					IL_00ed: ldarg.0
					IL_00ee: ldc.i4.1
					IL_00ef: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
					IL_00f4: ldarg.0
					IL_00f5: ldloc.s 8
					IL_00f7: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter36'
					IL_00fc: ldarg.0
					IL_00fd: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
					IL_0102: ldloca.s 8
					IL_0104: ldarg.0
					IL_0105: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'>(!!0&, !!1&)
					IL_010a: ldc.i4.0
					IL_010b: stloc.1
					IL_010c: leave IL_03d3

					IL_0111: ldarg.0
					IL_0112: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter36'
					IL_0117: stloc.s 8
					IL_0119: ldarg.0
					IL_011a: ldloca.s 9
					IL_011c: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>
					IL_0122: ldloc.s 9
					IL_0124: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter36'
					IL_0129: ldarg.0
					IL_012a: ldc.i4.m1
					IL_012b: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'

					IL_0130: ldloca.s 8
					IL_0132: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>::GetResult()
					IL_0137: ldloca.s 8
					IL_0139: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>
					IL_013f: stloc.s 10
					IL_0141: ldarg.0
					IL_0142: ldloc.s 10
					IL_0144: stfld class [Windows]Windows.Storage.StorageFolder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<folder>5__33'
					IL_0149: ldarg.0
					IL_014a: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_014f: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
					IL_0154: stloc.s 11
					IL_0156: ldloc.s 11
					IL_0158: ldftn instance valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::add_PreviewFrameAvailable(class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>)
					IL_015e: newobj instance void class [mscorlib]System.Func`2<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
					IL_0163: ldloc.s 11
					IL_0165: ldftn instance void [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::remove_PreviewFrameAvailable(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
					IL_016b: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
					IL_0170: ldarg.0
					IL_0171: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_0176: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::OnThumbnailFrameAvailable(class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object)
					IL_017c: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>::.ctor(object, native int)
					IL_0181: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::AddEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>>(class [mscorlib]System.Func`2<!!0, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
					IL_0186: ldarg.0
					IL_0187: ldfld class [Windows]Windows.Storage.StorageFolder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<folder>5__33'
					IL_018c: ldstr "{0}.mp4"
					IL_0191: call valuetype [mscorlib]System.DateTime [mscorlib]System.DateTime::get_Now()
					IL_0196: stloc.s 12
					IL_0198: ldloca.s 12
					IL_019a: ldstr "yyyyMMddHHmmssfff"
					IL_019f: call instance string [mscorlib]System.DateTime::ToString(string)
					IL_01a4: call string [mscorlib]System.String::Format(string, object)
					IL_01a9: ldc.i4.1
					IL_01aa: callvirt instance class [Windows]Windows.Foundation.IAsyncOperation`1<class [Windows]Windows.Storage.StorageFile> [Windows]Windows.Storage.StorageFolder::CreateFileAsync(string, valuetype [Windows]Windows.Storage.CreationCollisionOption)
					IL_01af: call valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!!0> [System.Runtime.WindowsRuntime]System.WindowsRuntimeSystemExtensions::GetAwaiter<class [Windows]Windows.Storage.StorageFile>(class [Windows]Windows.Foundation.IAsyncOperation`1<!!0>)
					IL_01b4: stloc.s 13
					IL_01b6: ldloca.s 13
					IL_01b8: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>::get_IsCompleted()
					IL_01bd: brtrue.s IL_0202

					IL_01bf: ldarg.0
					IL_01c0: ldc.i4.2
					IL_01c1: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
					IL_01c6: ldarg.0
					IL_01c7: ldloc.s 13
					IL_01c9: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter37'
					IL_01ce: ldarg.0
					IL_01cf: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
					IL_01d4: ldloca.s 13
					IL_01d6: ldarg.0
					IL_01d7: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'>(!!0&, !!1&)
					IL_01dc: ldc.i4.0
					IL_01dd: stloc.1
					IL_01de: leave IL_03d3

					IL_01e3: ldarg.0
					IL_01e4: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter37'
					IL_01e9: stloc.s 13
					IL_01eb: ldarg.0
					IL_01ec: ldloca.s 14
					IL_01ee: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>
					IL_01f4: ldloc.s 14
					IL_01f6: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter37'
					IL_01fb: ldarg.0
					IL_01fc: ldc.i4.m1
					IL_01fd: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'

					IL_0202: ldloca.s 13
					IL_0204: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>::GetResult()
					IL_0209: ldloca.s 13
					IL_020b: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>
					IL_0211: stloc.s 15
					IL_0213: ldarg.0
					IL_0214: ldloc.s 15
					IL_0216: stfld class [Windows]Windows.Storage.StorageFile Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<file>5__34'
					IL_021b: ldarg.0
					IL_021c: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_0221: ldarg.0
					IL_0222: ldfld class [Windows]Windows.Storage.StorageFile Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<file>5__34'
					IL_0227: callvirt instance string [Windows]Windows.Storage.StorageFile::get_Path()
					IL_022c: stfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_filePath
					IL_0231: ldarg.0
					IL_0232: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_0237: ldarg.0
					IL_0238: ldfld class [Windows]Windows.Storage.StorageFile Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<file>5__34'
					IL_023d: ldc.i4.1
					IL_023e: callvirt instance class [Windows]Windows.Foundation.IAsyncOperation`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream> [Windows]Windows.Storage.StorageFile::OpenAsync(valuetype [Windows]Windows.Storage.FileAccessMode)
					IL_0243: call valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!!0> [System.Runtime.WindowsRuntime]System.WindowsRuntimeSystemExtensions::GetAwaiter<class [Windows]Windows.Storage.Streams.IRandomAccessStream>(class [Windows]Windows.Foundation.IAsyncOperation`1<!!0>)
					IL_0248: stloc.s 16
					IL_024a: ldloca.s 16
					IL_024c: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream>::get_IsCompleted()
					IL_0251: brtrue.s IL_02b6

					IL_0253: stloc.s 17
					IL_0255: ldarg.0
					IL_0256: ldloc.s 17
					IL_0258: stfld object Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__stack'
					IL_025d: ldarg.0
					IL_025e: ldc.i4.3
					IL_025f: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
					IL_0264: ldarg.0
					IL_0265: ldloc.s 16
					IL_0267: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter38'
					IL_026c: ldarg.0
					IL_026d: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
					IL_0272: ldloca.s 16
					IL_0274: ldarg.0
					IL_0275: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream>, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'>(!!0&, !!1&)
					IL_027a: ldc.i4.0
					IL_027b: stloc.1
					IL_027c: leave IL_03d3

					IL_0281: ldarg.0
					IL_0282: ldfld object Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__stack'
					IL_0287: unbox.any Facebook.Messenger.WP8.Controls.CameraView/CameraState
					IL_028c: stloc.s 18
					IL_028e: ldloc.s 18
					IL_0290: ldarg.0
					IL_0291: ldnull
					IL_0292: stfld object Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__stack'
					IL_0297: ldarg.0
					IL_0298: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter38'
					IL_029d: stloc.s 16
					IL_029f: ldarg.0
					IL_02a0: ldloca.s 19
					IL_02a2: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream>
					IL_02a8: ldloc.s 19
					IL_02aa: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter38'
					IL_02af: ldarg.0
					IL_02b0: ldc.i4.m1
					IL_02b1: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'

					IL_02b6: ldloca.s 16
					IL_02b8: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream>::GetResult()
					IL_02bd: ldloca.s 16
					IL_02bf: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.Streams.IRandomAccessStream>
					IL_02c5: stfld class [Windows]Windows.Storage.Streams.IRandomAccessStream Facebook.Messenger.WP8.Controls.CameraView/CameraState::_stream
					IL_02ca: ldarg.0
					IL_02cb: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_02d0: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
					IL_02d5: ldarg.0
					IL_02d6: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_02db: ldfld class [Windows]Windows.Storage.Streams.IRandomAccessStream Facebook.Messenger.WP8.Controls.CameraView/CameraState::_stream
					IL_02e0: callvirt instance class [Windows]Windows.Foundation.IAsyncAction [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::StartRecordingToStreamAsync(class [Windows]Windows.Storage.Streams.IRandomAccessStream)
					IL_02e5: call valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [System.Runtime.WindowsRuntime]System.WindowsRuntimeSystemExtensions::GetAwaiter(class [Windows]Windows.Foundation.IAsyncAction)
					IL_02ea: stloc.s 20
					IL_02ec: ldloca.s 20
					IL_02ee: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
					IL_02f3: brtrue.s IL_0338

					IL_02f5: ldarg.0
					IL_02f6: ldc.i4.4
					IL_02f7: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
					IL_02fc: ldarg.0
					IL_02fd: ldloc.s 20
					IL_02ff: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter39'
					IL_0304: ldarg.0
					IL_0305: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
					IL_030a: ldloca.s 20
					IL_030c: ldarg.0
					IL_030d: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'>(!!0&, !!1&)
					IL_0312: ldc.i4.0
					IL_0313: stloc.1
					IL_0314: leave IL_03d3

					IL_0319: ldarg.0
					IL_031a: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter39'
					IL_031f: stloc.s 20
					IL_0321: ldarg.0
					IL_0322: ldloca.s 21
					IL_0324: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
					IL_032a: ldloc.s 21
					IL_032c: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>u__$awaiter39'
					IL_0331: ldarg.0
					IL_0332: ldc.i4.m1
					IL_0333: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'

					IL_0338: ldloca.s 20
					IL_033a: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
					IL_033f: ldloca.s 20
					IL_0341: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
					IL_0347: ldarg.0
					IL_0348: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_034d: call valuetype [mscorlib]System.DateTime [mscorlib]System.DateTime::get_Now()
					IL_0352: stfld valuetype [mscorlib]System.DateTime Facebook.Messenger.WP8.Controls.CameraView/CameraState::_recordingStartTime
					IL_0357: ldarg.0
					IL_0358: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_035d: ldc.i4.1
					IL_035e: stfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState::_isRecording
					IL_0363: leave.s IL_03a7
				} // end .try
				catch [mscorlib]System.Exception
				{
					IL_0365: stloc.0
					IL_0366: ldarg.0
					IL_0367: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_036c: ldstr "Unable to create file for video"
					IL_0371: ldstr "StartVideoCaptureAsync"
					IL_0376: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
					IL_037b: ldc.i4 326
					IL_0380: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
					IL_0385: ldarg.0
					IL_0386: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
					IL_038b: ldloc.0
					IL_038c: callvirt instance string [mscorlib]System.Object::ToString()
					IL_0391: ldstr "StartVideoCaptureAsync"
					IL_0396: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
					IL_039b: ldc.i4 327
					IL_03a0: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
					IL_03a5: leave.s IL_03a7
				} // end handler

				IL_03a7: leave.s IL_03c0
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_03a9: stloc.2
				IL_03aa: ldarg.0
				IL_03ab: ldc.i4.s -2
				IL_03ad: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
				IL_03b2: ldarg.0
				IL_03b3: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
				IL_03b8: ldloc.2
				IL_03b9: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_03be: leave.s IL_03d3
			} // end handler

			IL_03c0: ldarg.0
			IL_03c1: ldc.i4.s -2
			IL_03c3: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
			IL_03c8: ldarg.0
			IL_03c9: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
			IL_03ce: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_03d3: ret
		} // end of method '<StartVideoCaptureAsync>d__30'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x100c8
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<StartVideoCaptureAsync>d__30'::SetStateMachine

	} // end of class <StartVideoCaptureAsync>d__30

	.class nested private auto ansi sealed beforefieldinit '<StopVideoCaptureAsync>d__3b'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.Controls.CameraView/CameraState '<>4__this'
		.field public bool delete
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter3c'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x10120
			// Header size: 12
			// Code size: 212 (0xd4)
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
				IL_0003: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>1__state'
				IL_0008: stloc.2
				IL_0009: ldloc.2
				IL_000a: ldc.i4.0
				IL_000b: beq.s IL_0067

				IL_000d: ldarg.0
				IL_000e: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>4__this'
				IL_0013: call valuetype [mscorlib]System.DateTime [mscorlib]System.DateTime::get_Now()
				IL_0018: ldarg.0
				IL_0019: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>4__this'
				IL_001e: ldfld valuetype [mscorlib]System.DateTime Facebook.Messenger.WP8.Controls.CameraView/CameraState::_recordingStartTime
				IL_0023: call valuetype [mscorlib]System.TimeSpan [mscorlib]System.DateTime::op_Subtraction(valuetype [mscorlib]System.DateTime, valuetype [mscorlib]System.DateTime)
				IL_0028: stfld valuetype [mscorlib]System.TimeSpan Facebook.Messenger.WP8.Controls.CameraView/CameraState::_recordDuration
				IL_002d: ldarg.0
				IL_002e: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>4__this'
				IL_0033: callvirt instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.Controls.CameraView/CameraState::StopRecordingAsync()
				IL_0038: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_003d: stloc.3
				IL_003e: ldloca.s 3
				IL_0040: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_0045: brtrue.s IL_0085

				IL_0047: ldarg.0
				IL_0048: ldc.i4.0
				IL_0049: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>1__state'
				IL_004e: ldarg.0
				IL_004f: ldloc.3
				IL_0050: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>u__$awaiter3c'
				IL_0055: ldarg.0
				IL_0056: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>t__builder'
				IL_005b: ldloca.s 3
				IL_005d: ldarg.0
				IL_005e: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'>(!!0&, !!1&)
				IL_0063: ldc.i4.0
				IL_0064: stloc.0
				IL_0065: leave.s IL_00d3

				IL_0067: ldarg.0
				IL_0068: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>u__$awaiter3c'
				IL_006d: stloc.3
				IL_006e: ldarg.0
				IL_006f: ldloca.s 4
				IL_0071: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_0077: ldloc.s 4
				IL_0079: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>u__$awaiter3c'
				IL_007e: ldarg.0
				IL_007f: ldc.i4.m1
				IL_0080: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>1__state'

				IL_0085: ldloca.s 3
				IL_0087: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_008c: ldloca.s 3
				IL_008e: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_0094: ldarg.0
				IL_0095: ldfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::delete
				IL_009a: brfalse.s IL_00a7

				IL_009c: ldarg.0
				IL_009d: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>4__this'
				IL_00a2: callvirt instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::DeleteRecording()

				IL_00a7: leave.s IL_00c0
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_00a9: stloc.1
				IL_00aa: ldarg.0
				IL_00ab: ldc.i4.s -2
				IL_00ad: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>1__state'
				IL_00b2: ldarg.0
				IL_00b3: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>t__builder'
				IL_00b8: ldloc.1
				IL_00b9: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_00be: leave.s IL_00d3
			} // end handler

			IL_00c0: ldarg.0
			IL_00c1: ldc.i4.s -2
			IL_00c3: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>1__state'
			IL_00c8: ldarg.0
			IL_00c9: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>t__builder'
			IL_00ce: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_00d3: ret
		} // end of method '<StopVideoCaptureAsync>d__3b'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x10210
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<StopVideoCaptureAsync>d__3b'::SetStateMachine

	} // end of class <StopVideoCaptureAsync>d__3b

	.class nested private auto ansi sealed beforefieldinit '<CreateVideoResultAsync>d__3f'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> '<>t__builder'
		.field public class Facebook.Messenger.WP8.Controls.CameraView/CameraState '<>4__this'
		.field public int32 '<rotation>5__40'
		.field public int32 '<frameWidth>5__41'
		.field public int32 '<frameHeight>5__42'
		.field public class ['Facebook-Models']Facebook.Models.FacebookContext '<context>5__43'
		.field public string '<userFolderPath>5__44'
		.field public class [Windows]Windows.Storage.StorageFolder '<folder>5__45'
		.field public class [Windows]Windows.Storage.StorageFile '<output>5__46'
		.field public class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap '<rotatedThumbnail>5__47'
		.field public class [mscorlib]System.IO.Stream '<stream>5__48'
		.field public int32 '<absRotation>5__49'
		.field public bool '<isPortrait>5__4a'
		.field public class ['Facebook-Models']Facebook.Models.VideoResult '<result>5__4b'
		.field public class ['Facebook-Models']Facebook.Models.VideoResult '<>g__initLocal3e'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string> '<>u__$awaiter4c'
		.field private object '<>t__stack'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder> '<>u__$awaiter4d'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile> '<>u__$awaiter4e'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream> '<>u__$awaiter4f'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x10270
			// Header size: 12
			// Code size: 1352 (0x548)
			.maxstack 6
			.locals init (
				[0] class [mscorlib]System.Exception,
				[1] bool,
				[2] class ['Facebook-Models']Facebook.Models.VideoResult,
				[3] class [mscorlib]System.Exception,
				[4] int32,
				[5] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size,
				[6] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size,
				[7] int32,
				[8] int32,
				[9] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>,
				[10] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>,
				[11] string,
				[12] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>,
				[13] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>,
				[14] class [Windows]Windows.Storage.StorageFolder,
				[15] valuetype [mscorlib]System.DateTime,
				[16] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>,
				[17] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>,
				[18] class [Windows]Windows.Storage.StorageFile,
				[19] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream>,
				[20] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream>,
				[21] class [mscorlib]System.IO.Stream
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.1
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
				IL_0008: stloc.s 4
				IL_000a: ldloc.s 4
				IL_000c: switch (IL_00de, IL_00de, IL_00de, IL_00de)

				IL_0021: ldarg.0
				IL_0022: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_0027: ldfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_filePath
				IL_002c: brtrue.s IL_0035

				IL_002e: ldnull
				IL_002f: stloc.2
				IL_0030: leave IL_0533

				IL_0035: ldarg.0
				IL_0036: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_003b: ldfld int64 Facebook.Messenger.WP8.Controls.CameraView/CameraState::_streamLength
				IL_0040: conv.r8
				IL_0041: ldc.r8 10240
				IL_004a: bge.un.s IL_005e

				IL_004c: ldarg.0
				IL_004d: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_0052: callvirt instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::DeleteRecording()
				IL_0057: ldnull
				IL_0058: stloc.2
				IL_0059: leave IL_0533

				IL_005e: ldarg.0
				IL_005f: ldarg.0
				IL_0060: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_0065: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
				IL_006a: call valuetype [mscorlib]System.Guid [Windows]Windows.Phone.Media.Capture.KnownCameraGeneralProperties::get_SpecifiedCaptureOrientation()
				IL_006f: callvirt instance object [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::GetProperty(valuetype [mscorlib]System.Guid)
				IL_0074: unbox.any [mscorlib]System.Int32
				IL_0079: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<rotation>5__40'
				IL_007e: ldarg.0
				IL_007f: ldarg.0
				IL_0080: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_0085: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
				IL_008a: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::get_PreviewResolution()
				IL_008f: stloc.s 5
				IL_0091: ldloca.s 5
				IL_0093: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Width()
				IL_0098: conv.i4
				IL_0099: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<frameWidth>5__41'
				IL_009e: ldarg.0
				IL_009f: ldarg.0
				IL_00a0: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_00a5: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
				IL_00aa: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::get_PreviewResolution()
				IL_00af: stloc.s 6
				IL_00b1: ldloca.s 6
				IL_00b3: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Height()
				IL_00b8: conv.i4
				IL_00b9: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<frameHeight>5__42'
				IL_00be: ldarg.0
				IL_00bf: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_00c4: ldfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPixels
				IL_00c9: brfalse IL_0404

				IL_00ce: ldarg.0
				IL_00cf: call class [Microsoft.Practices.ServiceLocation]Microsoft.Practices.ServiceLocation.IServiceLocator [Microsoft.Practices.ServiceLocation]Microsoft.Practices.ServiceLocation.ServiceLocator::get_Current()
				IL_00d4: callvirt instance !!0 [Microsoft.Practices.ServiceLocation]Microsoft.Practices.ServiceLocation.IServiceLocator::GetInstance<class ['Facebook-Models']Facebook.Models.FacebookContext>()
				IL_00d9: stfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<context>5__43'
				.try
				{
					IL_00de: ldarg.0
					IL_00df: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
					IL_00e4: stloc.s 7
					IL_00e6: ldloc.s 7
					IL_00e8: switch (IL_00fd, IL_00fd, IL_00fd, IL_00fd)
					.try
					{
						IL_00fd: ldarg.0
						IL_00fe: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
						IL_0103: stloc.s 8
						IL_0105: ldloc.s 8
						IL_0107: switch (IL_0160, IL_01d7, IL_026c, IL_0311)

						IL_011c: ldarg.0
						IL_011d: ldfld class ['Facebook-Models']Facebook.Models.FacebookContext Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<context>5__43'
						IL_0122: callvirt instance class ['Facebook-Models']Facebook.Base.Settings.IUserSettings ['Facebook-Models']Facebook.Models.FacebookContext::get_Settings()
						IL_0127: callvirt instance class [mscorlib]System.Threading.Tasks.Task`1<string> ['Facebook-Models']Facebook.Base.Settings.IUserSettings::GetUserFolderPath()
						IL_012c: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!0> class [mscorlib]System.Threading.Tasks.Task`1<string>::GetAwaiter()
						IL_0131: stloc.s 9
						IL_0133: ldloca.s 9
						IL_0135: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>::get_IsCompleted()
						IL_013a: brtrue.s IL_017f

						IL_013c: ldarg.0
						IL_013d: ldc.i4.0
						IL_013e: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
						IL_0143: ldarg.0
						IL_0144: ldloc.s 9
						IL_0146: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4c'
						IL_014b: ldarg.0
						IL_014c: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
						IL_0151: ldloca.s 9
						IL_0153: ldarg.0
						IL_0154: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'>(!!0&, !!1&)
						IL_0159: ldc.i4.0
						IL_015a: stloc.1
						IL_015b: leave IL_0547

						IL_0160: ldarg.0
						IL_0161: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4c'
						IL_0166: stloc.s 9
						IL_0168: ldarg.0
						IL_0169: ldloca.s 10
						IL_016b: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>
						IL_0171: ldloc.s 10
						IL_0173: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4c'
						IL_0178: ldarg.0
						IL_0179: ldc.i4.m1
						IL_017a: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'

						IL_017f: ldloca.s 9
						IL_0181: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>::GetResult()
						IL_0186: ldloca.s 9
						IL_0188: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<string>
						IL_018e: stloc.s 11
						IL_0190: ldarg.0
						IL_0191: ldloc.s 11
						IL_0193: stfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<userFolderPath>5__44'
						IL_0198: ldarg.0
						IL_0199: ldfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<userFolderPath>5__44'
						IL_019e: call class [Windows]Windows.Foundation.IAsyncOperation`1<class [Windows]Windows.Storage.StorageFolder> [Windows]Windows.Storage.StorageFolder::GetFolderFromPathAsync(string)
						IL_01a3: call valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!!0> [System.Runtime.WindowsRuntime]System.WindowsRuntimeSystemExtensions::GetAwaiter<class [Windows]Windows.Storage.StorageFolder>(class [Windows]Windows.Foundation.IAsyncOperation`1<!!0>)
						IL_01a8: stloc.s 12
						IL_01aa: ldloca.s 12
						IL_01ac: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>::get_IsCompleted()
						IL_01b1: brtrue.s IL_01f6

						IL_01b3: ldarg.0
						IL_01b4: ldc.i4.1
						IL_01b5: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
						IL_01ba: ldarg.0
						IL_01bb: ldloc.s 12
						IL_01bd: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4d'
						IL_01c2: ldarg.0
						IL_01c3: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
						IL_01c8: ldloca.s 12
						IL_01ca: ldarg.0
						IL_01cb: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'>(!!0&, !!1&)
						IL_01d0: ldc.i4.0
						IL_01d1: stloc.1
						IL_01d2: leave IL_0547

						IL_01d7: ldarg.0
						IL_01d8: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4d'
						IL_01dd: stloc.s 12
						IL_01df: ldarg.0
						IL_01e0: ldloca.s 13
						IL_01e2: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>
						IL_01e8: ldloc.s 13
						IL_01ea: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4d'
						IL_01ef: ldarg.0
						IL_01f0: ldc.i4.m1
						IL_01f1: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'

						IL_01f6: ldloca.s 12
						IL_01f8: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>::GetResult()
						IL_01fd: ldloca.s 12
						IL_01ff: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFolder>
						IL_0205: stloc.s 14
						IL_0207: ldarg.0
						IL_0208: ldloc.s 14
						IL_020a: stfld class [Windows]Windows.Storage.StorageFolder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<folder>5__45'
						IL_020f: ldarg.0
						IL_0210: ldfld class [Windows]Windows.Storage.StorageFolder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<folder>5__45'
						IL_0215: ldstr "thumbnail_{0}.jpg"
						IL_021a: call valuetype [mscorlib]System.DateTime [mscorlib]System.DateTime::get_Now()
						IL_021f: stloc.s 15
						IL_0221: ldloca.s 15
						IL_0223: ldstr "yyyyMMddHHmmssfff"
						IL_0228: call instance string [mscorlib]System.DateTime::ToString(string)
						IL_022d: call string [mscorlib]System.String::Format(string, object)
						IL_0232: ldc.i4.1
						IL_0233: callvirt instance class [Windows]Windows.Foundation.IAsyncOperation`1<class [Windows]Windows.Storage.StorageFile> [Windows]Windows.Storage.StorageFolder::CreateFileAsync(string, valuetype [Windows]Windows.Storage.CreationCollisionOption)
						IL_0238: call valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!!0> [System.Runtime.WindowsRuntime]System.WindowsRuntimeSystemExtensions::GetAwaiter<class [Windows]Windows.Storage.StorageFile>(class [Windows]Windows.Foundation.IAsyncOperation`1<!!0>)
						IL_023d: stloc.s 16
						IL_023f: ldloca.s 16
						IL_0241: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>::get_IsCompleted()
						IL_0246: brtrue.s IL_028b

						IL_0248: ldarg.0
						IL_0249: ldc.i4.2
						IL_024a: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
						IL_024f: ldarg.0
						IL_0250: ldloc.s 16
						IL_0252: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4e'
						IL_0257: ldarg.0
						IL_0258: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
						IL_025d: ldloca.s 16
						IL_025f: ldarg.0
						IL_0260: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'>(!!0&, !!1&)
						IL_0265: ldc.i4.0
						IL_0266: stloc.1
						IL_0267: leave IL_0547

						IL_026c: ldarg.0
						IL_026d: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4e'
						IL_0272: stloc.s 16
						IL_0274: ldarg.0
						IL_0275: ldloca.s 17
						IL_0277: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>
						IL_027d: ldloc.s 17
						IL_027f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4e'
						IL_0284: ldarg.0
						IL_0285: ldc.i4.m1
						IL_0286: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'

						IL_028b: ldloca.s 16
						IL_028d: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>::GetResult()
						IL_0292: ldloca.s 16
						IL_0294: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [Windows]Windows.Storage.StorageFile>
						IL_029a: stloc.s 18
						IL_029c: ldarg.0
						IL_029d: ldloc.s 18
						IL_029f: stfld class [Windows]Windows.Storage.StorageFile Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<output>5__46'
						IL_02a4: ldarg.0
						IL_02a5: ldarg.0
						IL_02a6: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
						IL_02ab: ldarg.0
						IL_02ac: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
						IL_02b1: ldfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPixels
						IL_02b6: ldarg.0
						IL_02b7: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<frameWidth>5__41'
						IL_02bc: ldarg.0
						IL_02bd: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<frameHeight>5__42'
						IL_02c2: ldarg.0
						IL_02c3: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<rotation>5__40'
						IL_02c8: callvirt instance class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap Facebook.Messenger.WP8.Controls.CameraView/CameraState::CreateRotatedBitmapFromPixels(int32[], int32, int32, int32)
						IL_02cd: stfld class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<rotatedThumbnail>5__47'
						IL_02d2: ldarg.0
						IL_02d3: ldfld class [Windows]Windows.Storage.StorageFile Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<output>5__46'
						IL_02d8: call class [mscorlib]System.Threading.Tasks.Task`1<class [mscorlib]System.IO.Stream> [System.Runtime.WindowsRuntime]System.IO.WindowsRuntimeStorageExtensions::OpenStreamForWriteAsync(class [Windows]Windows.Storage.IStorageFile)
						IL_02dd: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<!0> class [mscorlib]System.Threading.Tasks.Task`1<class [mscorlib]System.IO.Stream>::GetAwaiter()
						IL_02e2: stloc.s 19
						IL_02e4: ldloca.s 19
						IL_02e6: call instance bool valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream>::get_IsCompleted()
						IL_02eb: brtrue.s IL_0330

						IL_02ed: ldarg.0
						IL_02ee: ldc.i4.3
						IL_02ef: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
						IL_02f4: ldarg.0
						IL_02f5: ldloc.s 19
						IL_02f7: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4f'
						IL_02fc: ldarg.0
						IL_02fd: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
						IL_0302: ldloca.s 19
						IL_0304: ldarg.0
						IL_0305: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream>, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'>(!!0&, !!1&)
						IL_030a: ldc.i4.0
						IL_030b: stloc.1
						IL_030c: leave IL_0547

						IL_0311: ldarg.0
						IL_0312: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4f'
						IL_0317: stloc.s 19
						IL_0319: ldarg.0
						IL_031a: ldloca.s 20
						IL_031c: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream>
						IL_0322: ldloc.s 20
						IL_0324: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>u__$awaiter4f'
						IL_0329: ldarg.0
						IL_032a: ldc.i4.m1
						IL_032b: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'

						IL_0330: ldloca.s 19
						IL_0332: call instance !0 valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream>::GetResult()
						IL_0337: ldloca.s 19
						IL_0339: initobj valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter`1<class [mscorlib]System.IO.Stream>
						IL_033f: stloc.s 21
						IL_0341: ldarg.0
						IL_0342: ldloc.s 21
						IL_0344: stfld class [mscorlib]System.IO.Stream Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<stream>5__48'
						.try
						{
							IL_0349: ldarg.0
							IL_034a: ldfld class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<rotatedThumbnail>5__47'
							IL_034f: ldarg.0
							IL_0350: ldfld class [mscorlib]System.IO.Stream Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<stream>5__48'
							IL_0355: ldarg.0
							IL_0356: ldfld class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<rotatedThumbnail>5__47'
							IL_035b: callvirt instance int32 [System.Windows]System.Windows.Media.Imaging.BitmapSource::get_PixelWidth()
							IL_0360: ldarg.0
							IL_0361: ldfld class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<rotatedThumbnail>5__47'
							IL_0366: callvirt instance int32 [System.Windows]System.Windows.Media.Imaging.BitmapSource::get_PixelHeight()
							IL_036b: ldc.i4.0
							IL_036c: ldc.i4.s 85
							IL_036e: call void [Microsoft.Phone]System.Windows.Media.Imaging.Extensions::SaveJpeg(class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap, class [mscorlib]System.IO.Stream, int32, int32, int32, int32)
							IL_0373: leave.s IL_038c
						} // end .try
						finally
						{
							IL_0375: ldloc.1
							IL_0376: brfalse.s IL_038b

							IL_0378: ldarg.0
							IL_0379: ldfld class [mscorlib]System.IO.Stream Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<stream>5__48'
							IL_037e: brfalse.s IL_038b

							IL_0380: ldarg.0
							IL_0381: ldfld class [mscorlib]System.IO.Stream Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<stream>5__48'
							IL_0386: callvirt instance void [mscorlib]System.IDisposable::Dispose()

							IL_038b: endfinally
						} // end handler

						IL_038c: ldarg.0
						IL_038d: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
						IL_0392: ldarg.0
						IL_0393: ldfld class [Windows]Windows.Storage.StorageFile Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<output>5__46'
						IL_0398: callvirt instance string [Windows]Windows.Storage.StorageFile::get_Path()
						IL_039d: stfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPath
						IL_03a2: leave.s IL_03f2
					} // end .try
					catch [mscorlib]System.Exception
					{
						IL_03a4: stloc.0
						IL_03a5: ldarg.0
						IL_03a6: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
						IL_03ab: ldstr "Unable to create file for thumbnail"
						IL_03b0: ldstr "CreateVideoResultAsync"
						IL_03b5: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
						IL_03ba: ldc.i4 376
						IL_03bf: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
						IL_03c4: ldarg.0
						IL_03c5: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
						IL_03ca: ldloc.0
						IL_03cb: callvirt instance string [mscorlib]System.Object::ToString()
						IL_03d0: ldstr "CreateVideoResultAsync"
						IL_03d5: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
						IL_03da: ldc.i4 377
						IL_03df: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
						IL_03e4: ldarg.0
						IL_03e5: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
						IL_03ea: ldnull
						IL_03eb: stfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPath
						IL_03f0: leave.s IL_03f2
					} // end handler

					IL_03f2: leave.s IL_0404
				} // end .try
				finally
				{
					IL_03f4: ldloc.1
					IL_03f5: brfalse.s IL_0403

					IL_03f7: ldarg.0
					IL_03f8: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
					IL_03fd: ldnull
					IL_03fe: stfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPixels

					IL_0403: endfinally
				} // end handler

				IL_0404: ldarg.0
				IL_0405: ldarg.0
				IL_0406: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<rotation>5__40'
				IL_040b: call int32 [mscorlib]System.Math::Abs(int32)
				IL_0410: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<absRotation>5__49'
				IL_0415: ldarg.0
				IL_0416: ldarg.0
				IL_0417: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<absRotation>5__49'
				IL_041c: ldc.i4.s 90
				IL_041e: beq.s IL_042f

				IL_0420: ldarg.0
				IL_0421: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<absRotation>5__49'
				IL_0426: ldc.i4 270
				IL_042b: ceq
				IL_042d: br.s IL_0430

				IL_042f: ldc.i4.1

				IL_0430: stfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<isPortrait>5__4a'
				IL_0435: ldarg.0
				IL_0436: ldarg.0
				IL_0437: newobj instance void ['Facebook-Models']Facebook.Models.VideoResult::.ctor()
				IL_043c: stfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_0441: ldarg.0
				IL_0442: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_0447: ldarg.0
				IL_0448: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_044d: ldfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_filePath
				IL_0452: callvirt instance void ['Facebook-Models']Facebook.Models.VideoResult::set_Path(string)
				IL_0457: ldarg.0
				IL_0458: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_045d: ldarg.0
				IL_045e: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_0463: ldfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPath
				IL_0468: callvirt instance void ['Facebook-Models']Facebook.Models.VideoResult::set_ThumbnailPath(string)
				IL_046d: ldarg.0
				IL_046e: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_0473: ldarg.0
				IL_0474: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_0479: ldfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_filePath
				IL_047e: callvirt instance void ['Facebook-Models']Facebook.Models.VideoResult::set_Filename(string)
				IL_0483: ldarg.0
				IL_0484: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_0489: ldarg.0
				IL_048a: ldfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<isPortrait>5__4a'
				IL_048f: brtrue.s IL_0499

				IL_0491: ldarg.0
				IL_0492: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<frameHeight>5__42'
				IL_0497: br.s IL_049f

				IL_0499: ldarg.0
				IL_049a: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<frameWidth>5__41'

				IL_049f: callvirt instance void ['Facebook-Models']Facebook.Models.VideoResult::set_Height(int32)
				IL_04a4: ldarg.0
				IL_04a5: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_04aa: ldarg.0
				IL_04ab: ldfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<isPortrait>5__4a'
				IL_04b0: brtrue.s IL_04ba

				IL_04b2: ldarg.0
				IL_04b3: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<frameWidth>5__41'
				IL_04b8: br.s IL_04c0

				IL_04ba: ldarg.0
				IL_04bb: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<frameHeight>5__42'

				IL_04c0: callvirt instance void ['Facebook-Models']Facebook.Models.VideoResult::set_Width(int32)
				IL_04c5: ldarg.0
				IL_04c6: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_04cb: ldarg.0
				IL_04cc: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_04d1: ldfld int64 Facebook.Messenger.WP8.Controls.CameraView/CameraState::_streamLength
				IL_04d6: callvirt instance void ['Facebook-Models']Facebook.Models.VideoResult::set_FileSize(int64)
				IL_04db: ldarg.0
				IL_04dc: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_04e1: ldarg.0
				IL_04e2: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
				IL_04e7: ldflda valuetype [mscorlib]System.TimeSpan Facebook.Messenger.WP8.Controls.CameraView/CameraState::_recordDuration
				IL_04ec: call instance float64 [mscorlib]System.TimeSpan::get_TotalSeconds()
				IL_04f1: conv.i4
				IL_04f2: callvirt instance void ['Facebook-Models']Facebook.Models.VideoResult::set_Length(int32)
				IL_04f7: ldarg.0
				IL_04f8: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_04fd: ldarg.0
				IL_04fe: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<rotation>5__40'
				IL_0503: callvirt instance void ['Facebook-Models']Facebook.Models.VideoResult::set_Rotation(int32)
				IL_0508: ldarg.0
				IL_0509: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>g__initLocal3e'
				IL_050e: stfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<result>5__4b'
				IL_0513: ldarg.0
				IL_0514: ldfld class ['Facebook-Models']Facebook.Models.VideoResult Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<result>5__4b'
				IL_0519: stloc.2
				IL_051a: leave.s IL_0533
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_051c: stloc.3
				IL_051d: ldarg.0
				IL_051e: ldc.i4.s -2
				IL_0520: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
				IL_0525: ldarg.0
				IL_0526: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
				IL_052b: ldloc.3
				IL_052c: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::SetException(class [mscorlib]System.Exception)
				IL_0531: leave.s IL_0547
			} // end handler

			IL_0533: ldarg.0
			IL_0534: ldc.i4.s -2
			IL_0536: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
			IL_053b: ldarg.0
			IL_053c: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
			IL_0541: ldloc.2
			IL_0542: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::SetResult(!0)

			IL_0547: ret
		} // end of method '<CreateVideoResultAsync>d__3f'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x10828
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<CreateVideoResultAsync>d__3f'::SetStateMachine

	} // end of class <CreateVideoResultAsync>d__3f

	.class nested private auto ansi sealed beforefieldinit '<StopRecordingAsync>d__51'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.Controls.CameraView/CameraState '<>4__this'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter52'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x108b8
			// Header size: 12
			// Code size: 432 (0x1b0)
			.maxstack 5
			.locals init (
				[0] class [mscorlib]System.Exception,
				[1] class [mscorlib]System.IO.Stream,
				[2] bool,
				[3] class [mscorlib]System.Exception,
				[4] int32,
				[5] int32,
				[6] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter,
				[7] valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
			)

			.try
			{
				IL_0000: ldc.i4.1
				IL_0001: stloc.2
				IL_0002: ldarg.0
				IL_0003: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>1__state'
				IL_0008: stloc.s 4
				IL_000a: ldloc.s 4
				IL_000c: ldc.i4.0
				IL_000d: beq.s IL_0067

				IL_000f: ldarg.0
				IL_0010: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_0015: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
				IL_001a: brfalse IL_0136

				IL_001f: ldarg.0
				IL_0020: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_0025: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
				IL_002a: ldftn instance void [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::remove_PreviewFrameAvailable(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
				IL_0030: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
				IL_0035: ldarg.0
				IL_0036: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_003b: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::OnThumbnailFrameAvailable(class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object)
				IL_0041: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>::.ctor(object, native int)
				IL_0046: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::RemoveEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>>(class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
				IL_004b: ldarg.0
				IL_004c: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_0051: ldfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState::_isRecording
				IL_0056: brfalse IL_0136

				IL_005b: ldarg.0
				IL_005c: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_0061: ldc.i4.0
				IL_0062: stfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState::_isRecording
				.try
				{
					IL_0067: ldarg.0
					IL_0068: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>1__state'
					IL_006d: stloc.s 5
					IL_006f: ldloc.s 5
					IL_0071: ldc.i4.0
					IL_0072: beq.s IL_00b8

					IL_0074: ldarg.0
					IL_0075: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
					IL_007a: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
					IL_007f: callvirt instance class [Windows]Windows.Foundation.IAsyncAction [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::StopRecordingAsync()
					IL_0084: call valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [System.Runtime.WindowsRuntime]System.WindowsRuntimeSystemExtensions::GetAwaiter(class [Windows]Windows.Foundation.IAsyncAction)
					IL_0089: stloc.s 6
					IL_008b: ldloca.s 6
					IL_008d: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
					IL_0092: brtrue.s IL_00d7

					IL_0094: ldarg.0
					IL_0095: ldc.i4.0
					IL_0096: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>1__state'
					IL_009b: ldarg.0
					IL_009c: ldloc.s 6
					IL_009e: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>u__$awaiter52'
					IL_00a3: ldarg.0
					IL_00a4: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>t__builder'
					IL_00a9: ldloca.s 6
					IL_00ab: ldarg.0
					IL_00ac: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'>(!!0&, !!1&)
					IL_00b1: ldc.i4.0
					IL_00b2: stloc.2
					IL_00b3: leave IL_01af

					IL_00b8: ldarg.0
					IL_00b9: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>u__$awaiter52'
					IL_00be: stloc.s 6
					IL_00c0: ldarg.0
					IL_00c1: ldloca.s 7
					IL_00c3: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
					IL_00c9: ldloc.s 7
					IL_00cb: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>u__$awaiter52'
					IL_00d0: ldarg.0
					IL_00d1: ldc.i4.m1
					IL_00d2: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>1__state'

					IL_00d7: ldloca.s 6
					IL_00d9: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
					IL_00de: ldloca.s 6
					IL_00e0: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
					IL_00e6: leave.s IL_0136
				} // end .try
				catch [mscorlib]System.Exception
				{
					IL_00e8: stloc.0
					IL_00e9: ldarg.0
					IL_00ea: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
					IL_00ef: ldstr "Error when recording stops"
					IL_00f4: ldstr "StopRecordingAsync"
					IL_00f9: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
					IL_00fe: ldc.i4 429
					IL_0103: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
					IL_0108: ldarg.0
					IL_0109: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
					IL_010e: ldloc.0
					IL_010f: callvirt instance string [mscorlib]System.Object::ToString()
					IL_0114: ldstr "StopRecordingAsync"
					IL_0119: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
					IL_011e: ldc.i4 430
					IL_0123: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
					IL_0128: ldarg.0
					IL_0129: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
					IL_012e: ldc.i4.4
					IL_012f: stfld valuetype Facebook.Messenger.WP8.Controls.CameraView/State Facebook.Messenger.WP8.Controls.CameraView/CameraState::CurrentState
					IL_0134: leave.s IL_0136
				} // end handler

				IL_0136: ldarg.0
				IL_0137: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_013c: ldfld class [Windows]Windows.Storage.Streams.IRandomAccessStream Facebook.Messenger.WP8.Controls.CameraView/CameraState::_stream
				IL_0141: brfalse.s IL_0177

				IL_0143: ldarg.0
				IL_0144: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_0149: ldfld class [Windows]Windows.Storage.Streams.IRandomAccessStream Facebook.Messenger.WP8.Controls.CameraView/CameraState::_stream
				IL_014e: call class [mscorlib]System.IO.Stream [System.Runtime.WindowsRuntime]System.IO.WindowsRuntimeStreamExtensions::AsStream(class [Windows]Windows.Storage.Streams.IRandomAccessStream)
				IL_0153: stloc.1
				IL_0154: ldarg.0
				IL_0155: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_015a: ldloc.1
				IL_015b: callvirt instance int64 [mscorlib]System.IO.Stream::get_Length()
				IL_0160: stfld int64 Facebook.Messenger.WP8.Controls.CameraView/CameraState::_streamLength
				IL_0165: ldloc.1
				IL_0166: callvirt instance void [mscorlib]System.IO.Stream::Dispose()
				IL_016b: ldarg.0
				IL_016c: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_0171: ldnull
				IL_0172: stfld class [Windows]Windows.Storage.Streams.IRandomAccessStream Facebook.Messenger.WP8.Controls.CameraView/CameraState::_stream

				IL_0177: ldarg.0
				IL_0178: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
				IL_017d: ldc.i4.0
				IL_017e: stfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState::_isRecording
				IL_0183: leave.s IL_019c
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_0185: stloc.3
				IL_0186: ldarg.0
				IL_0187: ldc.i4.s -2
				IL_0189: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>1__state'
				IL_018e: ldarg.0
				IL_018f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>t__builder'
				IL_0194: ldloc.3
				IL_0195: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_019a: leave.s IL_01af
			} // end handler

			IL_019c: ldarg.0
			IL_019d: ldc.i4.s -2
			IL_019f: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>1__state'
			IL_01a4: ldarg.0
			IL_01a5: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>t__builder'
			IL_01aa: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_01af: ret
		} // end of method '<StopRecordingAsync>d__51'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x10aa8
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<StopRecordingAsync>d__51'::SetStateMachine

	} // end of class <StopRecordingAsync>d__51

	.class nested private auto ansi sealed beforefieldinit '<<DeleteRecording>b__54>d__55'
		extends [mscorlib]System.ValueType
		implements [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 '<>1__state'
		.field public valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder '<>t__builder'
		.field public class Facebook.Messenger.WP8.Controls.CameraView/CameraState '<>4__this'
		.field public class ['Facebook-Messenger-WP8-Base']Facebook.Messenger.FileAccess '<fileAccess>5__56'
		.field private valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter '<>u__$awaiter57'
		.field private object '<>t__stack'

		// Methods
		.method private final hidebysig newslot virtual 
			instance void MoveNext () cil managed 
		{
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::MoveNext()
			// Method begins at RVA 0x10b00
			// Header size: 12
			// Code size: 201 (0xc9)
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
				IL_0003: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>1__state'
				IL_0008: stloc.2
				IL_0009: ldloc.2
				IL_000a: ldc.i4.0
				IL_000b: beq.s IL_006f

				IL_000d: ldarg.0
				IL_000e: newobj instance void ['Facebook-Messenger-WP8-Base']Facebook.Messenger.FileAccess::.ctor()
				IL_0013: stfld class ['Facebook-Messenger-WP8-Base']Facebook.Messenger.FileAccess Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<fileAccess>5__56'
				IL_0018: ldarg.0
				IL_0019: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>4__this'
				IL_001e: ldfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_filePath
				IL_0023: call bool [mscorlib]System.String::IsNullOrEmpty(string)
				IL_0028: brtrue.s IL_009c

				IL_002a: ldarg.0
				IL_002b: ldfld class ['Facebook-Messenger-WP8-Base']Facebook.Messenger.FileAccess Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<fileAccess>5__56'
				IL_0030: ldarg.0
				IL_0031: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>4__this'
				IL_0036: ldfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_filePath
				IL_003b: callvirt instance class [mscorlib]System.Threading.Tasks.Task ['Facebook-Messenger-WP8-Base']Facebook.Messenger.FileAccess::DeleteFileAtPath(string)
				IL_0040: callvirt instance valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter [mscorlib]System.Threading.Tasks.Task::GetAwaiter()
				IL_0045: stloc.3
				IL_0046: ldloca.s 3
				IL_0048: call instance bool [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::get_IsCompleted()
				IL_004d: brtrue.s IL_008d

				IL_004f: ldarg.0
				IL_0050: ldc.i4.0
				IL_0051: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>1__state'
				IL_0056: ldarg.0
				IL_0057: ldloc.3
				IL_0058: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>u__$awaiter57'
				IL_005d: ldarg.0
				IL_005e: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>t__builder'
				IL_0063: ldloca.s 3
				IL_0065: ldarg.0
				IL_0066: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter, valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'>(!!0&, !!1&)
				IL_006b: ldc.i4.0
				IL_006c: stloc.0
				IL_006d: leave.s IL_00c8

				IL_006f: ldarg.0
				IL_0070: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>u__$awaiter57'
				IL_0075: stloc.3
				IL_0076: ldarg.0
				IL_0077: ldloca.s 4
				IL_0079: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter
				IL_007f: ldloc.s 4
				IL_0081: stfld valuetype [mscorlib]System.Runtime.CompilerServices.TaskAwaiter Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>u__$awaiter57'
				IL_0086: ldarg.0
				IL_0087: ldc.i4.m1
				IL_0088: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>1__state'

				IL_008d: ldloca.s 3
				IL_008f: call instance void [mscorlib]System.Runtime.CompilerServices.TaskAwaiter::GetResult()
				IL_0094: ldloca.s 3
				IL_0096: initobj [mscorlib]System.Runtime.CompilerServices.TaskAwaiter

				IL_009c: leave.s IL_00b5
			} // end .try
			catch [mscorlib]System.Exception
			{
				IL_009e: stloc.1
				IL_009f: ldarg.0
				IL_00a0: ldc.i4.s -2
				IL_00a2: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>1__state'
				IL_00a7: ldarg.0
				IL_00a8: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>t__builder'
				IL_00ad: ldloc.1
				IL_00ae: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetException(class [mscorlib]System.Exception)
				IL_00b3: leave.s IL_00c8
			} // end handler

			IL_00b5: ldarg.0
			IL_00b6: ldc.i4.s -2
			IL_00b8: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>1__state'
			IL_00bd: ldarg.0
			IL_00be: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>t__builder'
			IL_00c3: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetResult()

			IL_00c8: ret
		} // end of method '<<DeleteRecording>b__54>d__55'::MoveNext

		.method private final hidebysig newslot virtual 
			instance void SetStateMachine (
				class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine param0
			) cil managed 
		{
			.custom instance void [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::.ctor() = (
				01 00 00 00
			)
			.override method instance void [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			// Method begins at RVA 0x10be8
			// Header size: 1
			// Code size: 13 (0xd)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>t__builder'
			IL_0006: ldarg.1
			IL_0007: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::SetStateMachine(class [mscorlib]System.Runtime.CompilerServices.IAsyncStateMachine)
			IL_000c: ret
		} // end of method '<<DeleteRecording>b__54>d__55'::SetStateMachine

	} // end of class <<DeleteRecording>b__54>d__55

	.class nested private auto ansi sealed beforefieldinit '<>c__DisplayClass5c'
		extends [mscorlib]System.Object
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Fields
		.field public int32 frameWidth
		.field public int32 frameHeight
		.field public int32 rotation
		.field public class Facebook.Messenger.WP8.Controls.CameraView/CameraState '<>4__this'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x10dc6
			// Header size: 1
			// Code size: 7 (0x7)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: ret
		} // end of method '<>c__DisplayClass5c'::.ctor

		.method public hidebysig 
			instance void '<OnLastFrameAvailable>b__5b' () cil managed 
		{
			// Method begins at RVA 0x10dd0
			// Header size: 12
			// Code size: 212 (0xd4)
			.maxstack 7
			.locals init (
				[0] class [System.Windows]System.Windows.Media.ImageBrush,
				[1] class [System.Windows]System.Windows.Media.CompositeTransform
			)

			IL_0000: ldarg.0
			IL_0001: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_0006: newobj instance void [System.Windows]System.Windows.Media.ImageBrush::.ctor()
			IL_000b: stloc.0
			IL_000c: ldloc.0
			IL_000d: ldarg.0
			IL_000e: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_0013: ldarg.0
			IL_0014: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_0019: ldfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFramePixels
			IL_001e: ldarg.0
			IL_001f: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::frameWidth
			IL_0024: ldarg.0
			IL_0025: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::frameHeight
			IL_002a: ldarg.0
			IL_002b: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::rotation
			IL_0030: call instance class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap Facebook.Messenger.WP8.Controls.CameraView/CameraState::CreateRotatedBitmapFromPixels(int32[], int32, int32, int32)
			IL_0035: callvirt instance void [System.Windows]System.Windows.Media.ImageBrush::set_ImageSource(class [System.Windows]System.Windows.Media.ImageSource)
			IL_003a: ldloc.0
			IL_003b: newobj instance void [System.Windows]System.Windows.Media.CompositeTransform::.ctor()
			IL_0040: stloc.1
			IL_0041: ldloc.1
			IL_0042: ldarg.0
			IL_0043: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_0048: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
			IL_004d: callvirt instance float64 [System.Windows]System.Windows.FrameworkElement::get_ActualWidth()
			IL_0052: ldc.r8 2
			IL_005b: div
			IL_005c: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_CenterX(float64)
			IL_0061: ldloc.1
			IL_0062: ldarg.0
			IL_0063: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_0068: ldfld valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation Facebook.Messenger.WP8.Controls.CameraView/CameraState::_sensorLocation
			IL_006d: ldc.i4.1
			IL_006e: beq.s IL_0073

			IL_0070: ldc.i4.1
			IL_0071: br.s IL_0074

			IL_0073: ldc.i4.m1

			IL_0074: conv.r8
			IL_0075: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_ScaleX(float64)
			IL_007a: ldloc.1
			IL_007b: callvirt instance void [System.Windows]System.Windows.Media.Brush::set_Transform(class [System.Windows]System.Windows.Media.Transform)
			IL_0080: ldloc.0
			IL_0081: stfld class [System.Windows]System.Windows.Media.ImageBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameImageBrush
			IL_0086: ldarg.0
			IL_0087: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_008c: ldfld class [System.Windows]System.Windows.Media.ImageBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameImageBrush
			IL_0091: ldarg.0
			IL_0092: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_0097: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::LastFrameOnImageOpened(object, class [System.Windows]System.Windows.RoutedEventArgs)
			IL_009d: newobj instance void class [mscorlib]System.EventHandler`1<class [System.Windows]System.Windows.RoutedEventArgs>::.ctor(object, native int)
			IL_00a2: callvirt instance void [System.Windows]System.Windows.Media.ImageBrush::add_ImageOpened(class [mscorlib]System.EventHandler`1<class [System.Windows]System.Windows.RoutedEventArgs>)
			IL_00a7: ldarg.0
			IL_00a8: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_00ad: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameCanvas
			IL_00b2: ldc.i4.0
			IL_00b3: callvirt instance void [System.Windows]System.Windows.UIElement::set_Visibility(valuetype [System.Windows]System.Windows.Visibility)
			IL_00b8: ldarg.0
			IL_00b9: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_00be: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameCanvas
			IL_00c3: ldarg.0
			IL_00c4: ldfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
			IL_00c9: ldfld class [System.Windows]System.Windows.Media.ImageBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameImageBrush
			IL_00ce: callvirt instance void [System.Windows]System.Windows.Controls.Panel::set_Background(class [System.Windows]System.Windows.Media.Brush)
			IL_00d3: ret
		} // end of method '<>c__DisplayClass5c'::'<OnLastFrameAvailable>b__5b'

	} // end of class <>c__DisplayClass5c


	// Fields
	.field private static literal float64 MinimumVideoStreamLength = float64(10240)
	.field private initonly class [System.Windows]System.Windows.Controls.Canvas _lastFrameCanvas
	.field private initonly class [System.Windows]System.Windows.Controls.Canvas _canvas
	.field private initonly class [System.Windows]System.Windows.Threading.Dispatcher _dispatcher
	.field public valuetype Facebook.Messenger.WP8.Controls.CameraView/State CurrentState
	.field private class [System.Windows]System.Windows.Media.VideoBrush _videoBrush
	.field private class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice _avDevice
	.field private valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation _sensorLocation
	.field private class [Windows]Windows.Storage.Streams.IRandomAccessStream _stream
	.field private int64 _streamLength
	.field private string _filePath
	.field private string _thumbnailPath
	.field private int32[] _thumbnailPixels
	.field private int32[] _lastFramePixels
	.field private class [System.Windows]System.Windows.Media.ImageBrush _lastFrameImageBrush
	.field private bool _isRecording
	.field private valuetype [mscorlib]System.DateTime _recordingStartTime
	.field private valuetype [mscorlib]System.TimeSpan _recordDuration
	.field private static class [mscorlib]System.Func`2<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size, float64> 'CS$<>9__CachedAnonymousMethodDelegate1d'
	.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
		01 00 00 00
	)

	// Methods
	.method public hidebysig specialname rtspecialname 
		instance void .ctor (
			valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation sensorLocation,
			class [System.Windows]System.Windows.Controls.Canvas canvas,
			class [System.Windows]System.Windows.Controls.Canvas lastFrameCanvas,
			class [System.Windows]System.Windows.Threading.Dispatcher dispatcher
		) cil managed 
	{
		// Method begins at RVA 0xf0b8
		// Header size: 1
		// Code size: 59 (0x3b)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance void [mscorlib]System.Object::.ctor()
		IL_0006: ldarg.0
		IL_0007: ldarg.1
		IL_0008: stfld valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation Facebook.Messenger.WP8.Controls.CameraView/CameraState::_sensorLocation
		IL_000d: ldarg.0
		IL_000e: ldarg.2
		IL_000f: stfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0014: ldarg.0
		IL_0015: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_001a: ldarg.0
		IL_001b: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::CanvasOnSizeChanged(object, class [System.Windows]System.Windows.SizeChangedEventArgs)
		IL_0021: newobj instance void [System.Windows]System.Windows.SizeChangedEventHandler::.ctor(object, native int)
		IL_0026: callvirt instance void [System.Windows]System.Windows.FrameworkElement::add_SizeChanged(class [System.Windows]System.Windows.SizeChangedEventHandler)
		IL_002b: ldarg.0
		IL_002c: ldarg.3
		IL_002d: stfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameCanvas
		IL_0032: ldarg.0
		IL_0033: ldarg.s dispatcher
		IL_0035: stfld class [System.Windows]System.Windows.Threading.Dispatcher Facebook.Messenger.WP8.Controls.CameraView/CameraState::_dispatcher
		IL_003a: ret
	} // end of method CameraState::.ctor

	.method private hidebysig 
		instance void CanvasOnSizeChanged (
			object sender,
			class [System.Windows]System.Windows.SizeChangedEventArgs sizeChangedEventArgs
		) cil managed 
	{
		// Method begins at RVA 0xf0f4
		// Header size: 1
		// Code size: 7 (0x7)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::SetTransform()
		IL_0006: ret
	} // end of method CameraState::CanvasOnSizeChanged

	.method public hidebysig specialname 
		instance float64 get_CaptureHeight () cil managed 
	{
		// Method begins at RVA 0xf0fc
		// Header size: 12
		// Code size: 100 (0x64)
		.maxstack 2
		.locals init (
			[0] int32,
			[1] int32,
			[2] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size,
			[3] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size
		)

		IL_0000: ldarg.0
		IL_0001: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0006: brtrue.s IL_0012

		IL_0008: ldc.r8 0.0
		IL_0011: ret

		IL_0012: ldarg.0
		IL_0013: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0018: call valuetype [mscorlib]System.Guid [Windows]Windows.Phone.Media.Capture.KnownCameraGeneralProperties::get_SpecifiedCaptureOrientation()
		IL_001d: callvirt instance object [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::GetProperty(valuetype [mscorlib]System.Guid)
		IL_0022: unbox.any [mscorlib]System.Int32
		IL_0027: stloc.0
		IL_0028: ldloc.0
		IL_0029: call int32 [mscorlib]System.Math::Abs(int32)
		IL_002e: stloc.1
		IL_002f: ldloc.1
		IL_0030: ldc.i4.s 90
		IL_0032: beq.s IL_0050

		IL_0034: ldloc.1
		IL_0035: ldc.i4 270
		IL_003a: beq.s IL_0050

		IL_003c: ldarg.0
		IL_003d: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0042: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::get_CaptureResolution()
		IL_0047: stloc.2
		IL_0048: ldloca.s 2
		IL_004a: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Height()
		IL_004f: ret

		IL_0050: ldarg.0
		IL_0051: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0056: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::get_CaptureResolution()
		IL_005b: stloc.3
		IL_005c: ldloca.s 3
		IL_005e: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Width()
		IL_0063: ret
	} // end of method CameraState::get_CaptureHeight

	.method private hidebysig 
		instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size GetResolution (
			class [mscorlib]System.Collections.Generic.IReadOnlyList`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size> resolutions
		) cil managed 
	{
		// Method begins at RVA 0xf180
		// Header size: 12
		// Code size: 201 (0xc9)
		.maxstack 3
		.locals init (
			[0] class [mscorlib]System.Collections.Generic.List`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>,
			[1] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size,
			[2] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size,
			[3] float64,
			[4] float64,
			[5] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>
		)

		IL_0000: ldarg.1
		IL_0001: brfalse.s IL_000b

		IL_0003: ldarg.1
		IL_0004: call bool [System.Core]System.Linq.Enumerable::Any<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>)
		IL_0009: brtrue.s IL_0023

		IL_000b: ldc.r8 0.0
		IL_0014: ldc.r8 0.0
		IL_001d: newobj instance void [System.Runtime.WindowsRuntime]Windows.Foundation.Size::.ctor(float64, float64)
		IL_0022: ret

		IL_0023: ldarg.1
		IL_0024: ldsfld class [mscorlib]System.Func`2<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size, float64> Facebook.Messenger.WP8.Controls.CameraView/CameraState::'CS$<>9__CachedAnonymousMethodDelegate1d'
		IL_0029: brtrue.s IL_003c

		IL_002b: ldnull
		IL_002c: ldftn float64 Facebook.Messenger.WP8.Controls.CameraView/CameraState::'<GetResolution>b__1c'(valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size)
		IL_0032: newobj instance void class [mscorlib]System.Func`2<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size, float64>::.ctor(object, native int)
		IL_0037: stsfld class [mscorlib]System.Func`2<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size, float64> Facebook.Messenger.WP8.Controls.CameraView/CameraState::'CS$<>9__CachedAnonymousMethodDelegate1d'

		IL_003c: ldsfld class [mscorlib]System.Func`2<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size, float64> Facebook.Messenger.WP8.Controls.CameraView/CameraState::'CS$<>9__CachedAnonymousMethodDelegate1d'
		IL_0041: call class [System.Core]System.Linq.IOrderedEnumerable`1<!!0> [System.Core]System.Linq.Enumerable::OrderByDescending<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size, float64>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, !!1>)
		IL_0046: call class [mscorlib]System.Collections.Generic.List`1<!!0> [System.Core]System.Linq.Enumerable::ToList<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>)
		IL_004b: stloc.0
		IL_004c: ldloc.0
		IL_004d: ldc.i4.0
		IL_004e: callvirt instance !0 class [mscorlib]System.Collections.Generic.List`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>::get_Item(int32)
		IL_0053: stloc.1
		IL_0054: ldloc.0
		IL_0055: callvirt instance valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<!0> class [mscorlib]System.Collections.Generic.List`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>::GetEnumerator()
		IL_005a: stloc.s 5
		.try
		{
			IL_005c: br.s IL_00ae
			// loop start (head: IL_00ae)
				IL_005e: ldloca.s 5
				IL_0060: call instance !0 valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>::get_Current()
				IL_0065: stloc.2
				IL_0066: ldloca.s 2
				IL_0068: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Width()
				IL_006d: ldloca.s 2
				IL_006f: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Height()
				IL_0074: mul
				IL_0075: stloc.3
				IL_0076: ldloca.s 1
				IL_0078: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Width()
				IL_007d: ldloca.s 1
				IL_007f: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Height()
				IL_0084: mul
				IL_0085: stloc.s 4
				IL_0087: ldloc.3
				IL_0088: ldc.r8 307200
				IL_0091: blt.un.s IL_00ae

				IL_0093: ldloc.3
				IL_0094: ldc.r8 307200
				IL_009d: sub
				IL_009e: ldloc.s 4
				IL_00a0: ldc.r8 307200
				IL_00a9: sub
				IL_00aa: bge.un.s IL_00ae

				IL_00ac: ldloc.2
				IL_00ad: stloc.1

				IL_00ae: ldloca.s 5
				IL_00b0: call instance bool valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>::MoveNext()
				IL_00b5: brtrue.s IL_005e
			// end loop

			IL_00b7: leave.s IL_00c7
		} // end .try
		finally
		{
			IL_00b9: ldloca.s 5
			IL_00bb: constrained. valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>
			IL_00c1: callvirt instance void [mscorlib]System.IDisposable::Dispose()
			IL_00c6: endfinally
		} // end handler

		IL_00c7: ldloc.1
		IL_00c8: ret
	} // end of method CameraState::GetResolution

	.method public hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task InitializeAsync (
			bool resetCamera
		) cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 4d 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 43 6f 6e 74 72 6f
			6c 73 2e 43 61 6d 65 72 61 56 69 65 77 2b 43 61
			6d 65 72 61 53 74 61 74 65 2b 3c 49 6e 69 74 69
			61 6c 69 7a 65 41 73 79 6e 63 3e 64 5f 5f 31 66
			00 00
		)
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0xf5b4
		// Header size: 12
		// Code size: 66 (0x42)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: ldarg.1
		IL_000b: stfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::resetCamera
		IL_0010: ldloca.s 0
		IL_0012: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_0017: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__builder'
		IL_001c: ldloca.s 0
		IL_001e: ldc.i4.m1
		IL_001f: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>1__state'
		IL_0024: ldloca.s 0
		IL_0026: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__builder'
		IL_002b: stloc.1
		IL_002c: ldloca.s 1
		IL_002e: ldloca.s 0
		IL_0030: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'>(!!0&)
		IL_0035: ldloca.s 0
		IL_0037: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<InitializeAsync>d__1f'::'<>t__builder'
		IL_003c: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0041: ret
	} // end of method CameraState::InitializeAsync

	.method public hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task CleanUp (
			[opt] bool resetState
		) cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 45 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 43 6f 6e 74 72 6f
			6c 73 2e 43 61 6d 65 72 61 56 69 65 77 2b 43 61
			6d 65 72 61 53 74 61 74 65 2b 3c 43 6c 65 61 6e
			55 70 3e 64 5f 5f 32 37 00 00
		)
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.param [1] = bool(true)
		// Method begins at RVA 0xf808
		// Header size: 12
		// Code size: 66 (0x42)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: ldarg.1
		IL_000b: stfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::resetState
		IL_0010: ldloca.s 0
		IL_0012: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_0017: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>t__builder'
		IL_001c: ldloca.s 0
		IL_001e: ldc.i4.m1
		IL_001f: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>1__state'
		IL_0024: ldloca.s 0
		IL_0026: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>t__builder'
		IL_002b: stloc.1
		IL_002c: ldloca.s 1
		IL_002e: ldloca.s 0
		IL_0030: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'>(!!0&)
		IL_0035: ldloca.s 0
		IL_0037: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CleanUp>d__27'::'<>t__builder'
		IL_003c: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0041: ret
	} // end of method CameraState::CleanUp

	.method public hidebysig 
		instance void SetTransform () cil managed 
	{
		// Method begins at RVA 0xf856
		// Header size: 1
		// Code size: 48 (0x30)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
		IL_0006: brfalse.s IL_002f

		IL_0008: ldarg.0
		IL_0009: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
		IL_000e: ldarg.0
		IL_000f: ldarg.0
		IL_0010: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0015: callvirt instance float64 [System.Windows]System.Windows.FrameworkElement::get_ActualWidth()
		IL_001a: ldarg.0
		IL_001b: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0020: callvirt instance float64 [System.Windows]System.Windows.FrameworkElement::get_ActualHeight()
		IL_0025: call instance class [System.Windows]System.Windows.Media.CompositeTransform Facebook.Messenger.WP8.Controls.CameraView/CameraState::GetTransform(float64, float64)
		IL_002a: callvirt instance void [System.Windows]System.Windows.Media.Brush::set_Transform(class [System.Windows]System.Windows.Media.Transform)

		IL_002f: ret
	} // end of method CameraState::SetTransform

	.method private hidebysig 
		instance class [System.Windows]System.Windows.Media.CompositeTransform GetTransform (
			float64 width,
			float64 height
		) cil managed 
	{
		// Method begins at RVA 0xf888
		// Header size: 12
		// Code size: 283 (0x11b)
		.maxstack 5
		.locals init (
			[0] valuetype [mscorlib]System.Nullable`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>,
			[1] class [mscorlib]System.Exception,
			[2] float64,
			[3] float64,
			[4] bool,
			[5] uint32,
			[6] int64,
			[7] class [System.Windows]System.Windows.Media.CompositeTransform,
			[8] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size
		)

		IL_0000: ldloca.s 0
		IL_0002: initobj valuetype [mscorlib]System.Nullable`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>
		.try
		{
			IL_0008: ldarg.0
			IL_0009: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
			IL_000e: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::get_PreviewResolution()
			IL_0013: newobj instance void valuetype [mscorlib]System.Nullable`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>::.ctor(!0)
			IL_0018: stloc.0
			IL_0019: leave.s IL_005a
		} // end .try
		catch [mscorlib]System.Exception
		{
			IL_001b: stloc.1
			IL_001c: ldarg.0
			IL_001d: ldstr "Error getting PreviewResolution"
			IL_0022: ldstr "GetTransform"
			IL_0027: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
			IL_002c: ldc.i4 199
			IL_0031: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
			IL_0036: ldarg.0
			IL_0037: ldloc.1
			IL_0038: callvirt instance string [mscorlib]System.Object::ToString()
			IL_003d: ldstr "GetTransform"
			IL_0042: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs"
			IL_0047: ldc.i4 200
			IL_004c: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Error(object, string, string, string, int32)
			IL_0051: ldarg.0
			IL_0052: ldc.i4.4
			IL_0053: stfld valuetype Facebook.Messenger.WP8.Controls.CameraView/State Facebook.Messenger.WP8.Controls.CameraView/CameraState::CurrentState
			IL_0058: leave.s IL_005a
		} // end handler

		IL_005a: ldloca.s 0
		IL_005c: call instance bool valuetype [mscorlib]System.Nullable`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>::get_HasValue()
		IL_0061: brtrue.s IL_0069

		IL_0063: newobj instance void [System.Windows]System.Windows.Media.CompositeTransform::.ctor()
		IL_0068: ret

		IL_0069: ldarg.1
		IL_006a: ldloca.s 0
		IL_006c: call instance !0 valuetype [mscorlib]System.Nullable`1<valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size>::get_Value()
		IL_0071: stloc.s 8
		IL_0073: ldloca.s 8
		IL_0075: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Height()
		IL_007a: div
		IL_007b: stloc.2
		IL_007c: ldc.r8 1
		IL_0085: stloc.3
		IL_0086: ldloc.2
		IL_0087: ldc.r8 1
		IL_0090: bgt.s IL_009e

		IL_0092: ldloc.2
		IL_0093: ldc.r8 1
		IL_009c: bge.un.s IL_00a0

		IL_009e: ldloc.2
		IL_009f: stloc.3

		IL_00a0: ldarg.0
		IL_00a1: ldfld valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation Facebook.Messenger.WP8.Controls.CameraView/CameraState::_sensorLocation
		IL_00a6: ldc.i4.1
		IL_00a7: ceq
		IL_00a9: stloc.s 4
		IL_00ab: ldarg.0
		IL_00ac: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_00b1: callvirt instance uint32 [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::get_SensorRotationInDegrees()
		IL_00b6: stloc.s 5
		IL_00b8: ldarg.0
		IL_00b9: ldfld valuetype [Windows]Windows.Phone.Media.Capture.CameraSensorLocation Facebook.Messenger.WP8.Controls.CameraView/CameraState::_sensorLocation
		IL_00be: brfalse.s IL_00c6

		IL_00c0: ldloc.s 5
		IL_00c2: conv.u8
		IL_00c3: neg
		IL_00c4: br.s IL_00c9

		IL_00c6: ldloc.s 5
		IL_00c8: conv.u8

		IL_00c9: stloc.s 6
		IL_00cb: newobj instance void [System.Windows]System.Windows.Media.CompositeTransform::.ctor()
		IL_00d0: stloc.s 7
		IL_00d2: ldloc.s 7
		IL_00d4: ldloc.s 6
		IL_00d6: conv.r8
		IL_00d7: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_Rotation(float64)
		IL_00dc: ldloc.s 7
		IL_00de: ldloc.3
		IL_00df: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_ScaleX(float64)
		IL_00e4: ldloc.s 7
		IL_00e6: ldloc.s 4
		IL_00e8: brtrue.s IL_00ed

		IL_00ea: ldloc.3
		IL_00eb: br.s IL_00ef

		IL_00ed: ldloc.3
		IL_00ee: neg

		IL_00ef: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_ScaleY(float64)
		IL_00f4: ldloc.s 7
		IL_00f6: ldarg.1
		IL_00f7: ldc.r8 2
		IL_0100: div
		IL_0101: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_CenterX(float64)
		IL_0106: ldloc.s 7
		IL_0108: ldarg.2
		IL_0109: ldc.r8 2
		IL_0112: div
		IL_0113: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_CenterY(float64)
		IL_0118: ldloc.s 7
		IL_011a: ret
	} // end of method CameraState::GetTransform

	.method public hidebysig 
		instance void PopulateExpandStoryboard (
			class [System.Windows]System.Windows.Media.Animation.Storyboard sb
		) cil managed 
	{
		// Method begins at RVA 0xf9c0
		// Header size: 12
		// Code size: 139 (0x8b)
		.maxstack 3
		.locals init (
			[0] class [System.Windows]System.Windows.Media.Animation.DoubleAnimation,
			[1] class [System.Windows]System.Windows.Media.Animation.DoubleAnimation,
			[2] class [System.Windows]System.Windows.Media.Animation.CubicEase
		)

		IL_0000: newobj instance void [System.Windows]System.Windows.Media.Animation.DoubleAnimation::.ctor()
		IL_0005: stloc.1
		IL_0006: ldloc.1
		IL_0007: ldc.r8 0.3
		IL_0010: call valuetype [mscorlib]System.TimeSpan [mscorlib]System.TimeSpan::FromSeconds(float64)
		IL_0015: call valuetype [System.Windows]System.Windows.Duration [System.Windows]System.Windows.Duration::op_Implicit(valuetype [mscorlib]System.TimeSpan)
		IL_001a: callvirt instance void [System.Windows]System.Windows.Media.Animation.Timeline::set_Duration(valuetype [System.Windows]System.Windows.Duration)
		IL_001f: ldloc.1
		IL_0020: ldarg.0
		IL_0021: call instance float64 Facebook.Messenger.WP8.Controls.CameraView/CameraState::get_CaptureHeight()
		IL_0026: newobj instance void valuetype [mscorlib]System.Nullable`1<float64>::.ctor(!0)
		IL_002b: callvirt instance void [System.Windows]System.Windows.Media.Animation.DoubleAnimation::set_To(valuetype [mscorlib]System.Nullable`1<float64>)
		IL_0030: ldloc.1
		IL_0031: ldarg.0
		IL_0032: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0037: callvirt instance float64 [System.Windows]System.Windows.FrameworkElement::get_ActualHeight()
		IL_003c: newobj instance void valuetype [mscorlib]System.Nullable`1<float64>::.ctor(!0)
		IL_0041: callvirt instance void [System.Windows]System.Windows.Media.Animation.DoubleAnimation::set_From(valuetype [mscorlib]System.Nullable`1<float64>)
		IL_0046: ldloc.1
		IL_0047: newobj instance void [System.Windows]System.Windows.Media.Animation.CubicEase::.ctor()
		IL_004c: stloc.2
		IL_004d: ldloc.2
		IL_004e: ldc.i4.0
		IL_004f: callvirt instance void [System.Windows]System.Windows.Media.Animation.EasingFunctionBase::set_EasingMode(valuetype [System.Windows]System.Windows.Media.Animation.EasingMode)
		IL_0054: ldloc.2
		IL_0055: callvirt instance void [System.Windows]System.Windows.Media.Animation.DoubleAnimation::set_EasingFunction(class [System.Windows]System.Windows.Media.Animation.IEasingFunction)
		IL_005a: ldloc.1
		IL_005b: stloc.0
		IL_005c: ldloc.0
		IL_005d: ldarg.0
		IL_005e: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0063: call void [System.Windows]System.Windows.Media.Animation.Storyboard::SetTarget(class [System.Windows]System.Windows.Media.Animation.Timeline, class [System.Windows]System.Windows.DependencyObject)
		IL_0068: ldloc.0
		IL_0069: ldstr "Height"
		IL_006e: ldc.i4.0
		IL_006f: newarr [mscorlib]System.Object
		IL_0074: newobj instance void [System.Windows]System.Windows.PropertyPath::.ctor(string, object[])
		IL_0079: call void [System.Windows]System.Windows.Media.Animation.Storyboard::SetTargetProperty(class [System.Windows]System.Windows.Media.Animation.Timeline, class [System.Windows]System.Windows.PropertyPath)
		IL_007e: ldarg.1
		IL_007f: callvirt instance class [System.Windows]System.Windows.Media.Animation.TimelineCollection [System.Windows]System.Windows.Media.Animation.Storyboard::get_Children()
		IL_0084: ldloc.0
		IL_0085: callvirt instance void class [System.Windows]System.Windows.PresentationFrameworkCollection`1<class [System.Windows]System.Windows.Media.Animation.Timeline>::Add(!0)
		IL_008a: ret
	} // end of method CameraState::PopulateExpandStoryboard

	.method public hidebysig 
		instance void PopulateCollapseStoryboard (
			class [System.Windows]System.Windows.Media.Animation.Storyboard sb
		) cil managed 
	{
		// Method begins at RVA 0xfa58
		// Header size: 12
		// Code size: 139 (0x8b)
		.maxstack 3
		.locals init (
			[0] class [System.Windows]System.Windows.Media.Animation.DoubleAnimation,
			[1] class [System.Windows]System.Windows.Media.Animation.DoubleAnimation,
			[2] class [System.Windows]System.Windows.Media.Animation.CubicEase
		)

		IL_0000: newobj instance void [System.Windows]System.Windows.Media.Animation.DoubleAnimation::.ctor()
		IL_0005: stloc.1
		IL_0006: ldloc.1
		IL_0007: ldc.r8 0.3
		IL_0010: call valuetype [mscorlib]System.TimeSpan [mscorlib]System.TimeSpan::FromSeconds(float64)
		IL_0015: call valuetype [System.Windows]System.Windows.Duration [System.Windows]System.Windows.Duration::op_Implicit(valuetype [mscorlib]System.TimeSpan)
		IL_001a: callvirt instance void [System.Windows]System.Windows.Media.Animation.Timeline::set_Duration(valuetype [System.Windows]System.Windows.Duration)
		IL_001f: ldloc.1
		IL_0020: ldarg.0
		IL_0021: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0026: callvirt instance float64 [System.Windows]System.Windows.FrameworkElement::get_ActualHeight()
		IL_002b: newobj instance void valuetype [mscorlib]System.Nullable`1<float64>::.ctor(!0)
		IL_0030: callvirt instance void [System.Windows]System.Windows.Media.Animation.DoubleAnimation::set_To(valuetype [mscorlib]System.Nullable`1<float64>)
		IL_0035: ldloc.1
		IL_0036: ldarg.0
		IL_0037: call instance float64 Facebook.Messenger.WP8.Controls.CameraView/CameraState::get_CaptureHeight()
		IL_003c: newobj instance void valuetype [mscorlib]System.Nullable`1<float64>::.ctor(!0)
		IL_0041: callvirt instance void [System.Windows]System.Windows.Media.Animation.DoubleAnimation::set_From(valuetype [mscorlib]System.Nullable`1<float64>)
		IL_0046: ldloc.1
		IL_0047: newobj instance void [System.Windows]System.Windows.Media.Animation.CubicEase::.ctor()
		IL_004c: stloc.2
		IL_004d: ldloc.2
		IL_004e: ldc.i4.0
		IL_004f: callvirt instance void [System.Windows]System.Windows.Media.Animation.EasingFunctionBase::set_EasingMode(valuetype [System.Windows]System.Windows.Media.Animation.EasingMode)
		IL_0054: ldloc.2
		IL_0055: callvirt instance void [System.Windows]System.Windows.Media.Animation.DoubleAnimation::set_EasingFunction(class [System.Windows]System.Windows.Media.Animation.IEasingFunction)
		IL_005a: ldloc.1
		IL_005b: stloc.0
		IL_005c: ldloc.0
		IL_005d: ldarg.0
		IL_005e: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0063: call void [System.Windows]System.Windows.Media.Animation.Storyboard::SetTarget(class [System.Windows]System.Windows.Media.Animation.Timeline, class [System.Windows]System.Windows.DependencyObject)
		IL_0068: ldloc.0
		IL_0069: ldstr "Height"
		IL_006e: ldc.i4.0
		IL_006f: newarr [mscorlib]System.Object
		IL_0074: newobj instance void [System.Windows]System.Windows.PropertyPath::.ctor(string, object[])
		IL_0079: call void [System.Windows]System.Windows.Media.Animation.Storyboard::SetTargetProperty(class [System.Windows]System.Windows.Media.Animation.Timeline, class [System.Windows]System.Windows.PropertyPath)
		IL_007e: ldarg.1
		IL_007f: callvirt instance class [System.Windows]System.Windows.Media.Animation.TimelineCollection [System.Windows]System.Windows.Media.Animation.Storyboard::get_Children()
		IL_0084: ldloc.0
		IL_0085: callvirt instance void class [System.Windows]System.Windows.PresentationFrameworkCollection`1<class [System.Windows]System.Windows.Media.Animation.Timeline>::Add(!0)
		IL_008a: ret
	} // end of method CameraState::PopulateCollapseStoryboard

	.method public hidebysig 
		instance class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap Capture () cil managed 
	{
		// Method begins at RVA 0xfaf0
		// Header size: 12
		// Code size: 380 (0x17c)
		.maxstack 5
		.locals init (
			[0] int32,
			[1] int32,
			[2] class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap,
			[3] class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap,
			[4] valuetype [System.Windows]System.Windows.Rect,
			[5] class [System.Windows]System.Windows.Media.CompositeTransform,
			[6] valuetype [System.Windows]System.Windows.Rect,
			[7] float64,
			[8] float64,
			[9] float64,
			[10] valuetype [System.Windows]System.Windows.Rect,
			[11] class [System.Windows]System.Windows.Controls.Image,
			[12] class [System.Windows]System.Windows.Controls.Image,
			[13] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size,
			[14] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size
		)

		IL_0000: ldarg.0
		IL_0001: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0006: callvirt instance float64 [System.Windows]System.Windows.FrameworkElement::get_ActualWidth()
		IL_000b: conv.i4
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0013: callvirt instance float64 [System.Windows]System.Windows.FrameworkElement::get_ActualHeight()
		IL_0018: conv.i4
		IL_0019: stloc.1
		IL_001a: ldarg.0
		IL_001b: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0020: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::get_PreviewResolution()
		IL_0025: stloc.s 13
		IL_0027: ldloca.s 13
		IL_0029: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Width()
		IL_002e: conv.i4
		IL_002f: ldarg.0
		IL_0030: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0035: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::get_PreviewResolution()
		IL_003a: stloc.s 14
		IL_003c: ldloca.s 14
		IL_003e: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Height()
		IL_0043: conv.i4
		IL_0044: newobj instance void [System.Windows]System.Windows.Media.Imaging.WriteableBitmap::.ctor(int32, int32)
		IL_0049: stloc.2
		IL_004a: ldarg.0
		IL_004b: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0050: ldloc.2
		IL_0051: callvirt instance int32[] [System.Windows]System.Windows.Media.Imaging.WriteableBitmap::get_Pixels()
		IL_0056: callvirt instance void [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::GetPreviewBufferArgb(int32[])
		IL_005b: ldloc.2
		IL_005c: callvirt instance void [System.Windows]System.Windows.Media.Imaging.WriteableBitmap::Invalidate()
		IL_0061: ldloc.0
		IL_0062: ldloc.1
		IL_0063: newobj instance void [System.Windows]System.Windows.Media.Imaging.WriteableBitmap::.ctor(int32, int32)
		IL_0068: stloc.3
		IL_0069: ldloca.s 4
		IL_006b: ldc.r8 0.0
		IL_0074: ldc.r8 0.0
		IL_007d: ldloc.2
		IL_007e: callvirt instance int32 [System.Windows]System.Windows.Media.Imaging.BitmapSource::get_PixelWidth()
		IL_0083: conv.r8
		IL_0084: ldloc.2
		IL_0085: callvirt instance int32 [System.Windows]System.Windows.Media.Imaging.BitmapSource::get_PixelHeight()
		IL_008a: conv.r8
		IL_008b: call instance void [System.Windows]System.Windows.Rect::.ctor(float64, float64, float64, float64)
		IL_0090: ldarg.0
		IL_0091: ldloc.2
		IL_0092: callvirt instance int32 [System.Windows]System.Windows.Media.Imaging.BitmapSource::get_PixelWidth()
		IL_0097: conv.r8
		IL_0098: ldloc.2
		IL_0099: callvirt instance int32 [System.Windows]System.Windows.Media.Imaging.BitmapSource::get_PixelHeight()
		IL_009e: conv.r8
		IL_009f: call instance class [System.Windows]System.Windows.Media.CompositeTransform Facebook.Messenger.WP8.Controls.CameraView/CameraState::GetTransform(float64, float64)
		IL_00a4: stloc.s 5
		IL_00a6: ldloc.s 5
		IL_00a8: ldloc.s 4
		IL_00aa: callvirt instance valuetype [System.Windows]System.Windows.Rect [System.Windows]System.Windows.Media.GeneralTransform::TransformBounds(valuetype [System.Windows]System.Windows.Rect)
		IL_00af: stloc.s 6
		IL_00b1: ldloc.3
		IL_00b2: callvirt instance int32 [System.Windows]System.Windows.Media.Imaging.BitmapSource::get_PixelWidth()
		IL_00b7: conv.r8
		IL_00b8: ldloca.s 6
		IL_00ba: call instance float64 [System.Windows]System.Windows.Rect::get_Width()
		IL_00bf: div
		IL_00c0: stloc.s 7
		IL_00c2: ldloc.3
		IL_00c3: callvirt instance int32 [System.Windows]System.Windows.Media.Imaging.BitmapSource::get_PixelHeight()
		IL_00c8: conv.r8
		IL_00c9: ldloca.s 6
		IL_00cb: call instance float64 [System.Windows]System.Windows.Rect::get_Width()
		IL_00d0: div
		IL_00d1: stloc.s 8
		IL_00d3: ldloc.s 7
		IL_00d5: ldloc.s 8
		IL_00d7: call float64 [mscorlib]System.Math::Max(float64, float64)
		IL_00dc: stloc.s 9
		IL_00de: ldloc.s 5
		IL_00e0: dup
		IL_00e1: callvirt instance float64 [System.Windows]System.Windows.Media.CompositeTransform::get_ScaleX()
		IL_00e6: ldloc.s 9
		IL_00e8: mul
		IL_00e9: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_ScaleX(float64)
		IL_00ee: ldloc.s 5
		IL_00f0: dup
		IL_00f1: callvirt instance float64 [System.Windows]System.Windows.Media.CompositeTransform::get_ScaleY()
		IL_00f6: ldloc.s 9
		IL_00f8: mul
		IL_00f9: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_ScaleY(float64)
		IL_00fe: ldloc.s 5
		IL_0100: ldloc.s 4
		IL_0102: callvirt instance valuetype [System.Windows]System.Windows.Rect [System.Windows]System.Windows.Media.GeneralTransform::TransformBounds(valuetype [System.Windows]System.Windows.Rect)
		IL_0107: stloc.s 10
		IL_0109: ldloc.s 5
		IL_010b: ldloc.0
		IL_010c: conv.r8
		IL_010d: ldloca.s 10
		IL_010f: call instance float64 [System.Windows]System.Windows.Rect::get_Width()
		IL_0114: sub
		IL_0115: ldc.r8 2
		IL_011e: div
		IL_011f: ldloca.s 10
		IL_0121: call instance float64 [System.Windows]System.Windows.Rect::get_X()
		IL_0126: sub
		IL_0127: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_TranslateX(float64)
		IL_012c: ldloc.s 5
		IL_012e: ldloc.1
		IL_012f: conv.r8
		IL_0130: ldloca.s 10
		IL_0132: call instance float64 [System.Windows]System.Windows.Rect::get_Height()
		IL_0137: sub
		IL_0138: ldc.r8 2
		IL_0141: div
		IL_0142: ldloca.s 10
		IL_0144: call instance float64 [System.Windows]System.Windows.Rect::get_Y()
		IL_0149: sub
		IL_014a: callvirt instance void [System.Windows]System.Windows.Media.CompositeTransform::set_TranslateY(float64)
		IL_014f: newobj instance void [System.Windows]System.Windows.Controls.Image::.ctor()
		IL_0154: stloc.s 12
		IL_0156: ldloc.s 12
		IL_0158: ldc.i4.0
		IL_0159: callvirt instance void [System.Windows]System.Windows.Controls.Image::set_Stretch(valuetype [System.Windows]System.Windows.Media.Stretch)
		IL_015e: ldloc.s 12
		IL_0160: ldloc.2
		IL_0161: callvirt instance void [System.Windows]System.Windows.Controls.Image::set_Source(class [System.Windows]System.Windows.Media.ImageSource)
		IL_0166: ldloc.s 12
		IL_0168: stloc.s 11
		IL_016a: ldloc.3
		IL_016b: ldloc.s 11
		IL_016d: ldloc.s 5
		IL_016f: callvirt instance void [System.Windows]System.Windows.Media.Imaging.WriteableBitmap::Render(class [System.Windows]System.Windows.UIElement, class [System.Windows]System.Windows.Media.Transform)
		IL_0174: ldloc.3
		IL_0175: callvirt instance void [System.Windows]System.Windows.Media.Imaging.WriteableBitmap::Invalidate()
		IL_017a: ldloc.3
		IL_017b: ret
	} // end of method CameraState::Capture

	.method public hidebysig 
		instance void Focus () cil managed 
	{
		// Method begins at RVA 0xfc78
		// Header size: 12
		// Code size: 30 (0x1e)
		.maxstack 1

		.try
		{
			IL_0000: ldarg.0
			IL_0001: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
			IL_0006: brfalse.s IL_0018

			IL_0008: ldarg.0
			IL_0009: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
			IL_000e: callvirt instance class [Windows]Windows.Foundation.IAsyncOperation`1<valuetype [Windows]Windows.Phone.Media.Capture.CameraFocusStatus> [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::FocusAsync()
			IL_0013: call void ['Facebook-Messenger-WP8-Base']Facebook.Messenger.Extensions.AsyncOperationExtensions::DoNotWait<valuetype [Windows]Windows.Phone.Media.Capture.CameraFocusStatus>(class [Windows]Windows.Foundation.IAsyncOperation`1<!!0>)

			IL_0018: leave.s IL_001d
		} // end .try
		catch [mscorlib]System.Object
		{
			IL_001a: pop
			IL_001b: leave.s IL_001d
		} // end handler

		IL_001d: ret
	} // end of method CameraState::Focus

	.method public hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task StartVideoCaptureAsync () cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 54 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 43 6f 6e 74 72 6f
			6c 73 2e 43 61 6d 65 72 61 56 69 65 77 2b 43 61
			6d 65 72 61 53 74 61 74 65 2b 3c 53 74 61 72 74
			56 69 64 65 6f 43 61 70 74 75 72 65 41 73 79 6e
			63 3e 64 5f 5f 33 30 00 00
		)
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0x100d8
		// Header size: 12
		// Code size: 58 (0x3a)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_000f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
		IL_0014: ldloca.s 0
		IL_0016: ldc.i4.m1
		IL_0017: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>1__state'
		IL_001c: ldloca.s 0
		IL_001e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
		IL_0023: stloc.1
		IL_0024: ldloca.s 1
		IL_0026: ldloca.s 0
		IL_0028: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'>(!!0&)
		IL_002d: ldloca.s 0
		IL_002f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StartVideoCaptureAsync>d__30'::'<>t__builder'
		IL_0034: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0039: ret
	} // end of method CameraState::StartVideoCaptureAsync

	.method public hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task StopVideoCaptureAsync (
			[opt] bool delete
		) cil managed 
	{
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 53 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 43 6f 6e 74 72 6f
			6c 73 2e 43 61 6d 65 72 61 56 69 65 77 2b 43 61
			6d 65 72 61 53 74 61 74 65 2b 3c 53 74 6f 70 56
			69 64 65 6f 43 61 70 74 75 72 65 41 73 79 6e 63
			3e 64 5f 5f 33 62 00 00
		)
		.param [1] = bool(false)
		// Method begins at RVA 0x10220
		// Header size: 12
		// Code size: 66 (0x42)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: ldarg.1
		IL_000b: stfld bool Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::delete
		IL_0010: ldloca.s 0
		IL_0012: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_0017: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>t__builder'
		IL_001c: ldloca.s 0
		IL_001e: ldc.i4.m1
		IL_001f: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>1__state'
		IL_0024: ldloca.s 0
		IL_0026: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>t__builder'
		IL_002b: stloc.1
		IL_002c: ldloca.s 1
		IL_002e: ldloca.s 0
		IL_0030: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'>(!!0&)
		IL_0035: ldloca.s 0
		IL_0037: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopVideoCaptureAsync>d__3b'::'<>t__builder'
		IL_003c: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0041: ret
	} // end of method CameraState::StopVideoCaptureAsync

	.method public hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task`1<class ['Facebook-Models']Facebook.Models.VideoResult> CreateVideoResultAsync () cil managed 
	{
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 54 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 43 6f 6e 74 72 6f
			6c 73 2e 43 61 6d 65 72 61 56 69 65 77 2b 43 61
			6d 65 72 61 53 74 61 74 65 2b 3c 43 72 65 61 74
			65 56 69 64 65 6f 52 65 73 75 6c 74 41 73 79 6e
			63 3e 64 5f 5f 33 66 00 00
		)
		// Method begins at RVA 0x10838
		// Header size: 12
		// Code size: 58 (0x3a)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<!0> valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::Create()
		IL_000f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
		IL_0014: ldloca.s 0
		IL_0016: ldc.i4.m1
		IL_0017: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>1__state'
		IL_001c: ldloca.s 0
		IL_001e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
		IL_0023: stloc.1
		IL_0024: ldloca.s 1
		IL_0026: ldloca.s 0
		IL_0028: call instance void valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::Start<valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'>(!!0&)
		IL_002d: ldloca.s 0
		IL_002f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult> Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<CreateVideoResultAsync>d__3f'::'<>t__builder'
		IL_0034: call instance class [mscorlib]System.Threading.Tasks.Task`1<!0> valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<class ['Facebook-Models']Facebook.Models.VideoResult>::get_Task()
		IL_0039: ret
	} // end of method CameraState::CreateVideoResultAsync

	.method private hidebysig 
		instance class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap CreateRotatedBitmapFromPixels (
			int32[] pixels,
			int32 frameWidth,
			int32 frameHeight,
			int32 rotation
		) cil managed 
	{
		// Method begins at RVA 0x10880
		// Header size: 12
		// Code size: 44 (0x2c)
		.maxstack 4
		.locals init (
			[0] class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap,
			[1] int32,
			[2] class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap
		)

		IL_0000: ldarg.2
		IL_0001: ldarg.3
		IL_0002: newobj instance void [System.Windows]System.Windows.Media.Imaging.WriteableBitmap::.ctor(int32, int32)
		IL_0007: stloc.0
		IL_0008: ldc.i4.0
		IL_0009: stloc.1
		IL_000a: br.s IL_001b
		// loop start (head: IL_001b)
			IL_000c: ldloc.0
			IL_000d: callvirt instance int32[] [System.Windows]System.Windows.Media.Imaging.WriteableBitmap::get_Pixels()
			IL_0012: ldloc.1
			IL_0013: ldarg.1
			IL_0014: ldloc.1
			IL_0015: ldelem.i4
			IL_0016: stelem.i4
			IL_0017: ldloc.1
			IL_0018: ldc.i4.1
			IL_0019: add
			IL_001a: stloc.1

			IL_001b: ldloc.1
			IL_001c: ldarg.1
			IL_001d: ldlen
			IL_001e: conv.i4
			IL_001f: blt.s IL_000c
		// end loop

		IL_0021: ldloc.0
		IL_0022: ldarg.s rotation
		IL_0024: call class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap [WriteableBitmapExWinPhone]System.Windows.Media.Imaging.WriteableBitmapExtensions::Rotate(class [System.Windows]System.Windows.Media.Imaging.WriteableBitmap, int32)
		IL_0029: stloc.2
		IL_002a: ldloc.2
		IL_002b: ret
	} // end of method CameraState::CreateRotatedBitmapFromPixels

	.method private hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task StopRecordingAsync () cil managed 
	{
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 50 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 43 6f 6e 74 72 6f
			6c 73 2e 43 61 6d 65 72 61 56 69 65 77 2b 43 61
			6d 65 72 61 53 74 61 74 65 2b 3c 53 74 6f 70 52
			65 63 6f 72 64 69 6e 67 41 73 79 6e 63 3e 64 5f
			5f 35 31 00 00
		)
		// Method begins at RVA 0x10ab8
		// Header size: 12
		// Code size: 58 (0x3a)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_000f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>t__builder'
		IL_0014: ldloca.s 0
		IL_0016: ldc.i4.m1
		IL_0017: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>1__state'
		IL_001c: ldloca.s 0
		IL_001e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>t__builder'
		IL_0023: stloc.1
		IL_0024: ldloca.s 1
		IL_0026: ldloca.s 0
		IL_0028: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'>(!!0&)
		IL_002d: ldloca.s 0
		IL_002f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<StopRecordingAsync>d__51'::'<>t__builder'
		IL_0034: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0039: ret
	} // end of method CameraState::StopRecordingAsync

	.method private hidebysig 
		instance void DeleteRecording () cil managed 
	{
		// Method begins at RVA 0x10c3e
		// Header size: 1
		// Code size: 39 (0x27)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldftn instance class [mscorlib]System.Threading.Tasks.Task Facebook.Messenger.WP8.Controls.CameraView/CameraState::'<DeleteRecording>b__54'()
		IL_0007: newobj instance void class [mscorlib]System.Func`1<class [mscorlib]System.Threading.Tasks.Task>::.ctor(object, native int)
		IL_000c: call class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Threading.Tasks.Task::Run(class [mscorlib]System.Func`1<class [mscorlib]System.Threading.Tasks.Task>)
		IL_0011: pop
		IL_0012: ldarg.0
		IL_0013: ldnull
		IL_0014: stfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPixels
		IL_0019: ldarg.0
		IL_001a: ldnull
		IL_001b: stfld string Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPath
		IL_0020: ldarg.0
		IL_0021: call instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::CleanupLastFrame()
		IL_0026: ret
	} // end of method CameraState::DeleteRecording

	.method public hidebysig 
		instance void RestoreDefaults () cil managed 
	{
		// Method begins at RVA 0x10c68
		// Header size: 12
		// Code size: 68 (0x44)
		.maxstack 2

		IL_0000: ldarg.0
		IL_0001: call instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::CleanupLastFrame()
		IL_0006: ldarg.0
		IL_0007: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
		IL_000c: brfalse.s IL_0043

		IL_000e: ldarg.0
		IL_000f: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0014: callvirt instance class [System.Windows]System.Windows.Media.Brush [System.Windows]System.Windows.Controls.Panel::get_Background()
		IL_0019: ldarg.0
		IL_001a: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
		IL_001f: beq.s IL_0043

		IL_0021: ldarg.0
		IL_0022: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
		IL_0027: ldarg.0
		IL_0028: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_002d: call void [Microsoft.Phone]Microsoft.Devices.CameraVideoBrushExtensions::SetSource(class [System.Windows]System.Windows.Media.VideoBrush, object)
		IL_0032: ldarg.0
		IL_0033: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0038: ldarg.0
		IL_0039: ldfld class [System.Windows]System.Windows.Media.VideoBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_videoBrush
		IL_003e: callvirt instance void [System.Windows]System.Windows.Controls.Panel::set_Background(class [System.Windows]System.Windows.Media.Brush)

		IL_0043: ret
	} // end of method CameraState::RestoreDefaults

	.method private hidebysig 
		instance void CleanupLastFrame () cil managed 
	{
		// Method begins at RVA 0x10cb8
		// Header size: 12
		// Code size: 70 (0x46)
		.maxstack 3

		IL_0000: ldarg.0
		IL_0001: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameCanvas
		IL_0006: ldc.i4.1
		IL_0007: callvirt instance void [System.Windows]System.Windows.UIElement::set_Visibility(valuetype [System.Windows]System.Windows.Visibility)
		IL_000c: ldarg.0
		IL_000d: ldnull
		IL_000e: stfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFramePixels
		IL_0013: ldarg.0
		IL_0014: ldfld class [System.Windows]System.Windows.Media.ImageBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameImageBrush
		IL_0019: brfalse.s IL_0039

		IL_001b: ldarg.0
		IL_001c: ldfld class [System.Windows]System.Windows.Media.ImageBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameImageBrush
		IL_0021: ldarg.0
		IL_0022: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::LastFrameOnImageOpened(object, class [System.Windows]System.Windows.RoutedEventArgs)
		IL_0028: newobj instance void class [mscorlib]System.EventHandler`1<class [System.Windows]System.Windows.RoutedEventArgs>::.ctor(object, native int)
		IL_002d: callvirt instance void [System.Windows]System.Windows.Media.ImageBrush::remove_ImageOpened(class [mscorlib]System.EventHandler`1<class [System.Windows]System.Windows.RoutedEventArgs>)
		IL_0032: ldarg.0
		IL_0033: ldnull
		IL_0034: stfld class [System.Windows]System.Windows.Media.ImageBrush Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameImageBrush

		IL_0039: ldarg.0
		IL_003a: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFrameCanvas
		IL_003f: ldnull
		IL_0040: callvirt instance void [System.Windows]System.Windows.Controls.Panel::set_Background(class [System.Windows]System.Windows.Media.Brush)
		IL_0045: ret
	} // end of method CameraState::CleanupLastFrame

	.method public hidebysig 
		instance void PausePreview () cil managed 
	{
		// Method begins at RVA 0x10d0c
		// Header size: 12
		// Code size: 49 (0x31)
		.maxstack 4
		.locals init (
			[0] class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice
		)

		IL_0000: ldarg.0
		IL_0001: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0006: stloc.0
		IL_0007: ldloc.0
		IL_0008: ldftn instance valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::add_PreviewFrameAvailable(class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>)
		IL_000e: newobj instance void class [mscorlib]System.Func`2<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_0013: ldloc.0
		IL_0014: ldftn instance void [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::remove_PreviewFrameAvailable(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
		IL_001a: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_001f: ldarg.0
		IL_0020: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::OnLastFrameAvailable(class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object)
		IL_0026: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>::.ctor(object, native int)
		IL_002b: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::AddEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>>(class [mscorlib]System.Func`2<!!0, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
		IL_0030: ret
	} // end of method CameraState::PausePreview

	.method private hidebysig 
		instance void OnThumbnailFrameAvailable (
			class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice sender,
			object args
		) cil managed 
	{
		// Method begins at RVA 0x10d4c
		// Header size: 12
		// Code size: 110 (0x6e)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] int32,
			[2] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size,
			[3] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size
		)

		IL_0000: ldarg.0
		IL_0001: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0006: brtrue.s IL_0009

		IL_0008: ret

		IL_0009: ldarg.0
		IL_000a: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_000f: ldftn instance void [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::remove_PreviewFrameAvailable(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
		IL_0015: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_001a: ldarg.0
		IL_001b: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::OnThumbnailFrameAvailable(class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object)
		IL_0021: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>::.ctor(object, native int)
		IL_0026: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::RemoveEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>>(class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
		IL_002b: ldarg.0
		IL_002c: ldfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPixels
		IL_0031: brtrue.s IL_006d

		IL_0033: ldarg.1
		IL_0034: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice::get_PreviewResolution()
		IL_0039: stloc.2
		IL_003a: ldloca.s 2
		IL_003c: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Width()
		IL_0041: conv.i4
		IL_0042: stloc.0
		IL_0043: ldarg.1
		IL_0044: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice::get_PreviewResolution()
		IL_0049: stloc.3
		IL_004a: ldloca.s 3
		IL_004c: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Height()
		IL_0051: conv.i4
		IL_0052: stloc.1
		IL_0053: ldarg.0
		IL_0054: ldloc.0
		IL_0055: ldloc.1
		IL_0056: mul
		IL_0057: newarr [mscorlib]System.Int32
		IL_005c: stfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPixels
		IL_0061: ldarg.1
		IL_0062: ldarg.0
		IL_0063: ldfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_thumbnailPixels
		IL_0068: callvirt instance void [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice::GetPreviewBufferArgb(int32[])

		IL_006d: ret
	} // end of method CameraState::OnThumbnailFrameAvailable

	.method private hidebysig 
		instance void OnLastFrameAvailable (
			class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice sender,
			object args
		) cil managed 
	{
		// Method begins at RVA 0x10eb0
		// Header size: 12
		// Code size: 197 (0xc5)
		.maxstack 3
		.locals init (
			[0] class Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c',
			[1] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size,
			[2] valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size
		)

		IL_0000: ldarg.0
		IL_0001: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0006: brtrue.s IL_0009

		IL_0008: ret

		IL_0009: ldarg.0
		IL_000a: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_000f: ldftn instance void [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::remove_PreviewFrameAvailable(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
		IL_0015: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_001a: ldarg.0
		IL_001b: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState::OnLastFrameAvailable(class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object)
		IL_0021: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>::.ctor(object, native int)
		IL_0026: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::RemoveEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice, object>>(class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
		IL_002b: ldarg.0
		IL_002c: ldfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFramePixels
		IL_0031: brtrue IL_00c4

		IL_0036: newobj instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::.ctor()
		IL_003b: stloc.0
		IL_003c: ldloc.0
		IL_003d: ldarg.0
		IL_003e: stfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<>4__this'
		IL_0043: ldloc.0
		IL_0044: ldarg.1
		IL_0045: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice::get_PreviewResolution()
		IL_004a: stloc.1
		IL_004b: ldloca.s 1
		IL_004d: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Width()
		IL_0052: conv.i4
		IL_0053: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::frameWidth
		IL_0058: ldloc.0
		IL_0059: ldarg.1
		IL_005a: callvirt instance valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice::get_PreviewResolution()
		IL_005f: stloc.2
		IL_0060: ldloca.s 2
		IL_0062: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Height()
		IL_0067: conv.i4
		IL_0068: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::frameHeight
		IL_006d: ldloc.0
		IL_006e: ldarg.0
		IL_006f: ldfld class [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice Facebook.Messenger.WP8.Controls.CameraView/CameraState::_avDevice
		IL_0074: call valuetype [mscorlib]System.Guid [Windows]Windows.Phone.Media.Capture.KnownCameraGeneralProperties::get_SpecifiedCaptureOrientation()
		IL_0079: callvirt instance object [Windows]Windows.Phone.Media.Capture.AudioVideoCaptureDevice::GetProperty(valuetype [mscorlib]System.Guid)
		IL_007e: unbox.any [mscorlib]System.Int32
		IL_0083: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::rotation
		IL_0088: ldarg.0
		IL_0089: ldloc.0
		IL_008a: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::frameWidth
		IL_008f: ldloc.0
		IL_0090: ldfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::frameHeight
		IL_0095: mul
		IL_0096: newarr [mscorlib]System.Int32
		IL_009b: stfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFramePixels
		IL_00a0: ldarg.1
		IL_00a1: ldarg.0
		IL_00a2: ldfld int32[] Facebook.Messenger.WP8.Controls.CameraView/CameraState::_lastFramePixels
		IL_00a7: callvirt instance void [Windows]Windows.Phone.Media.Capture.ICameraCaptureDevice::GetPreviewBufferArgb(int32[])
		IL_00ac: ldarg.0
		IL_00ad: ldfld class [System.Windows]System.Windows.Threading.Dispatcher Facebook.Messenger.WP8.Controls.CameraView/CameraState::_dispatcher
		IL_00b2: ldloc.0
		IL_00b3: ldftn instance void Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<>c__DisplayClass5c'::'<OnLastFrameAvailable>b__5b'()
		IL_00b9: newobj instance void [mscorlib]System.Action::.ctor(object, native int)
		IL_00be: callvirt instance class [System.Windows]System.Windows.Threading.DispatcherOperation [System.Windows]System.Windows.Threading.Dispatcher::BeginInvoke(class [mscorlib]System.Action)
		IL_00c3: pop

		IL_00c4: ret
	} // end of method CameraState::OnLastFrameAvailable

	.method private hidebysig 
		instance void LastFrameOnImageOpened (
			object sender,
			class [System.Windows]System.Windows.RoutedEventArgs routedEventArgs
		) cil managed 
	{
		// Method begins at RVA 0x10f81
		// Header size: 1
		// Code size: 13 (0xd)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class [System.Windows]System.Windows.Controls.Canvas Facebook.Messenger.WP8.Controls.CameraView/CameraState::_canvas
		IL_0006: ldnull
		IL_0007: callvirt instance void [System.Windows]System.Windows.Controls.Panel::set_Background(class [System.Windows]System.Windows.Media.Brush)
		IL_000c: ret
	} // end of method CameraState::LastFrameOnImageOpened

	.method private hidebysig static 
		float64 '<GetResolution>b__1c' (
			valuetype [System.Runtime.WindowsRuntime]Windows.Foundation.Size s
		) cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0xf16c
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: ldarga.s s
		IL_0002: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Width()
		IL_0007: ldarga.s s
		IL_0009: call instance float64 [System.Runtime.WindowsRuntime]Windows.Foundation.Size::get_Height()
		IL_000e: mul
		IL_000f: ret
	} // end of method CameraState::'<GetResolution>b__1c'

	.method private hidebysig 
		instance class [mscorlib]System.Threading.Tasks.Task '<DeleteRecording>b__54' () cil managed 
	{
		.custom instance void [mscorlib]System.Runtime.CompilerServices.AsyncStateMachineAttribute::.ctor(class [mscorlib]System.Type) = (
			01 00 54 46 61 63 65 62 6f 6f 6b 2e 4d 65 73 73
			65 6e 67 65 72 2e 57 50 38 2e 43 6f 6e 74 72 6f
			6c 73 2e 43 61 6d 65 72 61 56 69 65 77 2b 43 61
			6d 65 72 61 53 74 61 74 65 2b 3c 3c 44 65 6c 65
			74 65 52 65 63 6f 72 64 69 6e 67 3e 62 5f 5f 35
			34 3e 64 5f 5f 35 35 00 00
		)
		.custom instance void [mscorlib]System.Diagnostics.DebuggerStepThroughAttribute::.ctor() = (
			01 00 00 00
		)
		.custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = (
			01 00 00 00
		)
		// Method begins at RVA 0x10bf8
		// Header size: 12
		// Code size: 58 (0x3a)
		.maxstack 2
		.locals init (
			[0] valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55',
			[1] valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder
		)

		IL_0000: ldloca.s 0
		IL_0002: ldarg.0
		IL_0003: stfld class Facebook.Messenger.WP8.Controls.CameraView/CameraState Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>4__this'
		IL_0008: ldloca.s 0
		IL_000a: call valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Create()
		IL_000f: stfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>t__builder'
		IL_0014: ldloca.s 0
		IL_0016: ldc.i4.m1
		IL_0017: stfld int32 Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>1__state'
		IL_001c: ldloca.s 0
		IL_001e: ldfld valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>t__builder'
		IL_0023: stloc.1
		IL_0024: ldloca.s 1
		IL_0026: ldloca.s 0
		IL_0028: call instance void [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<valuetype Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'>(!!0&)
		IL_002d: ldloca.s 0
		IL_002f: ldflda valuetype [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder Facebook.Messenger.WP8.Controls.CameraView/CameraState/'<<DeleteRecording>b__54>d__55'::'<>t__builder'
		IL_0034: call instance class [mscorlib]System.Threading.Tasks.Task [mscorlib]System.Runtime.CompilerServices.AsyncTaskMethodBuilder::get_Task()
		IL_0039: ret
	} // end of method CameraState::'<DeleteRecording>b__54'

	// Properties
	.property instance float64 CaptureHeight()
	{
		.get instance float64 Facebook.Messenger.WP8.Controls.CameraView/CameraState::get_CaptureHeight()
	}

} // end of class CameraState
