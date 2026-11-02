// IL code: Facebook.Messenger.WP8
.class public auto ansi beforefieldinit Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper
	extends [mscorlib]System.Object
{
	// Fields
	.field private class Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler _inputPaneHidingHandler
	.field private class Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler _inputPaneShowingHandler

	// Methods
	.method public hidebysig 
		instance void SubscribeToKeyboardEvents (
			class Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler showingHandler,
			class Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler hidingHandler
		) cil managed 
	{
		// Method begins at RVA 0x487d4
		// Header size: 12
		// Code size: 103 (0x67)
		.maxstack 4
		.locals init (
			[0] class [Windows]Windows.UI.ViewManagement.InputPane
		)

		IL_0000: call class [Windows]Windows.UI.ViewManagement.InputPane [Windows]Windows.UI.ViewManagement.InputPane::GetForCurrentView()
		IL_0005: stloc.0
		IL_0006: ldloc.0
		IL_0007: ldftn instance valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken [Windows]Windows.UI.ViewManagement.InputPane::add_Showing(class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>)
		IL_000d: newobj instance void class [mscorlib]System.Func`2<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_0012: ldloc.0
		IL_0013: ldftn instance void [Windows]Windows.UI.ViewManagement.InputPane::remove_Showing(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
		IL_0019: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_001e: ldarg.0
		IL_001f: ldftn instance void Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::ShowingHandler(class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs)
		IL_0025: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>::.ctor(object, native int)
		IL_002a: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::AddEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>>(class [mscorlib]System.Func`2<!!0, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
		IL_002f: ldloc.0
		IL_0030: ldftn instance valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken [Windows]Windows.UI.ViewManagement.InputPane::add_Hiding(class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>)
		IL_0036: newobj instance void class [mscorlib]System.Func`2<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_003b: ldloc.0
		IL_003c: ldftn instance void [Windows]Windows.UI.ViewManagement.InputPane::remove_Hiding(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
		IL_0042: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_0047: ldarg.0
		IL_0048: ldftn instance void Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::HidingHandler(class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs)
		IL_004e: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>::.ctor(object, native int)
		IL_0053: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::AddEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>>(class [mscorlib]System.Func`2<!!0, valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
		IL_0058: ldarg.0
		IL_0059: ldarg.1
		IL_005a: call instance void Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::AddShowingHandler(class Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler)
		IL_005f: ldarg.0
		IL_0060: ldarg.2
		IL_0061: call instance void Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::AddHidingHandler(class Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler)
		IL_0066: ret
	} // end of method SingleInputPaneHelper::SubscribeToKeyboardEvents

	.method public hidebysig 
		instance void UnsubscribeFromKeyboardEvents () cil managed 
	{
		// Method begins at RVA 0x48848
		// Header size: 12
		// Code size: 79 (0x4f)
		.maxstack 3
		.locals init (
			[0] class [Windows]Windows.UI.ViewManagement.InputPane
		)

		IL_0000: call class [Windows]Windows.UI.ViewManagement.InputPane [Windows]Windows.UI.ViewManagement.InputPane::GetForCurrentView()
		IL_0005: stloc.0
		IL_0006: ldloc.0
		IL_0007: ldftn instance void [Windows]Windows.UI.ViewManagement.InputPane::remove_Showing(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
		IL_000d: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_0012: ldarg.0
		IL_0013: ldftn instance void Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::ShowingHandler(class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs)
		IL_0019: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>::.ctor(object, native int)
		IL_001e: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::RemoveEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>>(class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
		IL_0023: ldloc.0
		IL_0024: ldftn instance void [Windows]Windows.UI.ViewManagement.InputPane::remove_Hiding(valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)
		IL_002a: newobj instance void class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>::.ctor(object, native int)
		IL_002f: ldarg.0
		IL_0030: ldftn instance void Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::HidingHandler(class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs)
		IL_0036: newobj instance void class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>::.ctor(object, native int)
		IL_003b: call void [mscorlib]System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal::RemoveEventHandler<class [Windows]Windows.Foundation.TypedEventHandler`2<class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs>>(class [mscorlib]System.Action`1<valuetype [mscorlib]System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken>, !!0)
		IL_0040: ldarg.0
		IL_0041: ldnull
		IL_0042: stfld class Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneShowingHandler
		IL_0047: ldarg.0
		IL_0048: ldnull
		IL_0049: stfld class Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneHidingHandler
		IL_004e: ret
	} // end of method SingleInputPaneHelper::UnsubscribeFromKeyboardEvents

	.method private hidebysig 
		instance void AddShowingHandler (
			class Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler 'handler'
		) cil managed 
	{
		// Method begins at RVA 0x488a3
		// Header size: 1
		// Code size: 40 (0x28)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneShowingHandler
		IL_0006: brfalse.s IL_0020

		IL_0008: ldarg.0
		IL_0009: ldstr "A handler is already registered!"
		IL_000e: ldstr "AddShowingHandler"
		IL_0013: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\SingleInputPaneHelper.cs"
		IL_0018: ldc.i4.s 38
		IL_001a: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Warning(object, string, string, string, int32)
		IL_001f: ret

		IL_0020: ldarg.0
		IL_0021: ldarg.1
		IL_0022: stfld class Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneShowingHandler
		IL_0027: ret
	} // end of method SingleInputPaneHelper::AddShowingHandler

	.method private hidebysig 
		instance void AddHidingHandler (
			class Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler 'handler'
		) cil managed 
	{
		// Method begins at RVA 0x488cc
		// Header size: 1
		// Code size: 40 (0x28)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneHidingHandler
		IL_0006: brfalse.s IL_0020

		IL_0008: ldarg.0
		IL_0009: ldstr "A handler is already registered!"
		IL_000e: ldstr "AddHidingHandler"
		IL_0013: ldstr "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\SingleInputPaneHelper.cs"
		IL_0018: ldc.i4.s 50
		IL_001a: call void ['Facebook-Base']Facebook.Base.Logging.Logger::Warning(object, string, string, string, int32)
		IL_001f: ret

		IL_0020: ldarg.0
		IL_0021: ldarg.1
		IL_0022: stfld class Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneHidingHandler
		IL_0027: ret
	} // end of method SingleInputPaneHelper::AddHidingHandler

	.method private hidebysig 
		instance void ShowingHandler (
			class [Windows]Windows.UI.ViewManagement.InputPane sender,
			class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs e
		) cil managed 
	{
		// Method begins at RVA 0x488f5
		// Header size: 1
		// Code size: 30 (0x1e)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneShowingHandler
		IL_0006: brtrue.s IL_0009

		IL_0008: ret

		IL_0009: ldarg.2
		IL_000a: ldc.i4.1
		IL_000b: callvirt instance void [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs::put_EnsuredFocusedElementInView(bool)
		IL_0010: ldarg.0
		IL_0011: ldfld class Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneShowingHandler
		IL_0016: ldarg.1
		IL_0017: ldarg.2
		IL_0018: callvirt instance void Facebook.Messenger.WP8.Utilities.InputPaneShowingHandler::Invoke(object, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs)
		IL_001d: ret
	} // end of method SingleInputPaneHelper::ShowingHandler

	.method private hidebysig 
		instance void HidingHandler (
			class [Windows]Windows.UI.ViewManagement.InputPane sender,
			class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs e
		) cil managed 
	{
		// Method begins at RVA 0x48914
		// Header size: 1
		// Code size: 30 (0x1e)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: ldfld class Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneHidingHandler
		IL_0006: brtrue.s IL_0009

		IL_0008: ret

		IL_0009: ldarg.2
		IL_000a: ldc.i4.1
		IL_000b: callvirt instance void [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs::put_EnsuredFocusedElementInView(bool)
		IL_0010: ldarg.0
		IL_0011: ldfld class Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper::_inputPaneHidingHandler
		IL_0016: ldarg.1
		IL_0017: ldarg.2
		IL_0018: callvirt instance void Facebook.Messenger.WP8.Utilities.InputPaneHidingHandler::Invoke(class [Windows]Windows.UI.ViewManagement.InputPane, class [Windows]Windows.UI.ViewManagement.InputPaneVisibilityEventArgs)
		IL_001d: ret
	} // end of method SingleInputPaneHelper::HidingHandler

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x48933
		// Header size: 1
		// Code size: 7 (0x7)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance void [mscorlib]System.Object::.ctor()
		IL_0006: ret
	} // end of method SingleInputPaneHelper::.ctor

} // end of class Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper
