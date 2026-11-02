using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Messenger.Extensions;
using Facebook.Models;
using Microsoft.Devices;
using Microsoft.Practices.ServiceLocation;
using Windows.Foundation;
using Windows.Phone.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Facebook.Messenger.WP8.Controls
{
	public class CameraView : UserControl
	{
		public enum State
		{
			NotStarted,
			Initializing,
			Ready,
			Recording,
			Error
		}

		protected class CameraState
		{
			[StructLayout(LayoutKind.Auto)]
			[CompilerGenerated]
			private struct _003CCleanUp_003Ed__27 : IAsyncStateMachine
			{
				public int _003C_003E1__state;

				public AsyncTaskMethodBuilder _003C_003Et__builder;

				public CameraState _003C_003E4__this;

				public bool resetState;

				private TaskAwaiter _003C_003Eu___0024awaiter28;

				private object _003C_003Et__stack;

				private void MoveNext()
				{
					// ILSpy could not decompile this member: Error decompiling @060014BD Facebook.Messenger.WP8.Controls.CameraView.CameraState.<CleanUp>d__27.MoveNext: Value cannot be null. (Parameter 'source')
					// Please report the exception below,
					// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
					// Error decompiling @060014BD Facebook.Messenger.WP8.Controls.CameraView.CameraState.<CleanUp>d__27.MoveNext
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
					//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitTryFinally(TryFinally) + 0x28
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
			private struct _003CStartVideoCaptureAsync_003Ed__30 : IAsyncStateMachine
			{
				public int _003C_003E1__state;

				public AsyncTaskMethodBuilder _003C_003Et__builder;

				public CameraState _003C_003E4__this;

				public FacebookContext _003Ccontext_003E5__31;

				public string _003CuserFolderPath_003E5__32;

				public StorageFolder _003Cfolder_003E5__33;

				public StorageFile _003Cfile_003E5__34;

				private TaskAwaiter<string> _003C_003Eu___0024awaiter35;

				private object _003C_003Et__stack;

				private TaskAwaiter<StorageFolder> _003C_003Eu___0024awaiter36;

				private TaskAwaiter<StorageFile> _003C_003Eu___0024awaiter37;

				private TaskAwaiter<IRandomAccessStream> _003C_003Eu___0024awaiter38;

				private TaskAwaiter _003C_003Eu___0024awaiter39;

				private void MoveNext()
				{
					// ILSpy could not decompile this member: Error decompiling @060014BF Facebook.Messenger.WP8.Controls.CameraView.CameraState.<StartVideoCaptureAsync>d__30.MoveNext: Value cannot be null. (Parameter 'source')
					// Please report the exception below,
					// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
					// Error decompiling @060014BF Facebook.Messenger.WP8.Controls.CameraView.CameraState.<StartVideoCaptureAsync>d__30.MoveNext
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
					//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.TranslateSwitch(BlockContainer, SwitchInstruction) + 0xa76
					//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x66
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
			private struct _003CStopRecordingAsync_003Ed__51 : IAsyncStateMachine
			{
				public int _003C_003E1__state;

				public AsyncTaskMethodBuilder _003C_003Et__builder;

				public CameraState _003C_003E4__this;

				private TaskAwaiter _003C_003Eu___0024awaiter52;

				private object _003C_003Et__stack;

				private void MoveNext()
				{
					// ILSpy could not decompile this member: Error decompiling @060014C5 Facebook.Messenger.WP8.Controls.CameraView.CameraState.<StopRecordingAsync>d__51.MoveNext: Value cannot be null. (Parameter 'source')
					// Please report the exception below,
					// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
					// Error decompiling @060014C5 Facebook.Messenger.WP8.Controls.CameraView.CameraState.<StopRecordingAsync>d__51.MoveNext
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

			private const double MinimumVideoStreamLength = 10240.0;

			private readonly Canvas _lastFrameCanvas;

			private readonly Canvas _canvas;

			private readonly Dispatcher _dispatcher;

			public State CurrentState;

			private VideoBrush _videoBrush;

			private AudioVideoCaptureDevice _avDevice;

			private CameraSensorLocation _sensorLocation;

			private IRandomAccessStream _stream;

			private long _streamLength;

			private string _filePath;

			private string _thumbnailPath;

			private int[] _thumbnailPixels;

			private int[] _lastFramePixels;

			private ImageBrush _lastFrameImageBrush;

			private bool _isRecording;

			private DateTime _recordingStartTime;

			private TimeSpan _recordDuration;

			public double CaptureHeight
			{
				get
				{
					if (_avDevice == null)
					{
						return 0.0;
					}
					int value = (int)_avDevice.GetProperty(KnownCameraGeneralProperties.SpecifiedCaptureOrientation);
					int num = Math.Abs(value);
					if (num != 90 && num != 270)
					{
						return _avDevice.CaptureResolution.Height;
					}
					return _avDevice.CaptureResolution.Width;
				}
			}

			public CameraState(CameraSensorLocation sensorLocation, Canvas canvas, Canvas lastFrameCanvas, Dispatcher dispatcher)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected Obj, but got Unknown
				_sensorLocation = sensorLocation;
				_canvas = canvas;
				((FrameworkElement)_canvas).SizeChanged += CanvasOnSizeChanged;
				_lastFrameCanvas = lastFrameCanvas;
				_dispatcher = dispatcher;
			}

			private void CanvasOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
			{
				SetTransform();
			}

			private Size GetResolution(IReadOnlyList<Size> resolutions)
			{
				if (resolutions == null || !resolutions.Any())
				{
					return new Size(0.0, 0.0);
				}
				List<Size> list = resolutions.OrderByDescending((Size s) => s.Width * s.Height).ToList();
				Size result = list[0];
				foreach (Size item in list)
				{
					double num = item.Width * item.Height;
					double num2 = result.Width * result.Height;
					if (num >= 307200.0 && num - 307200.0 < num2 - 307200.0)
					{
						result = item;
					}
				}
				return result;
			}

			public async Task InitializeAsync(bool resetCamera)
			{
				try
				{
					if (resetCamera || _videoBrush == null || _avDevice == null || CurrentState == State.Error)
					{
						await CleanUp();
						IReadOnlyList<Size> resolutions = AudioVideoCaptureDevice.GetAvailableCaptureResolutions(_sensorLocation);
						Size captureResolution = GetResolution(resolutions);
						_avDevice = await AudioVideoCaptureDevice.OpenAsync(_sensorLocation, captureResolution);
						uint sensorRotation = _avDevice.SensorRotationInDegrees;
						long rotation = (((int)_sensorLocation == 0) ? sensorRotation : (0L - (long)sensorRotation));
						_avDevice.SetProperty(KnownCameraGeneralProperties.SpecifiedCaptureOrientation, (object)rotation);
						CameraState cameraState = this;
						VideoBrush val = new VideoBrush();
						((TileBrush)val).Stretch = (Stretch)0;
						cameraState._videoBrush = val;
						((Panel)_canvas).Background = (Brush)(object)_videoBrush;
					}
					SetTransform();
					CameraVideoBrushExtensions.SetSource(_videoBrush, (object)_avDevice);
					CurrentState = State.Ready;
				}
				catch (Exception ex)
				{
					CleanUp(false).DoNotWait();
					Logger.Error(this, "Error initializing camera", "InitializeAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs", 145);
					Logger.Error(this, ex.ToString(), "InitializeAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs", 146);
					CurrentState = State.Error;
				}
			}

			[AsyncStateMachine(typeof(_003CCleanUp_003Ed__27))]
			[DebuggerStepThrough]
			public Task CleanUp(bool resetState = true)
			{
				// ILSpy could not decompile this member: Error decompiling @0600028A Facebook.Messenger.WP8.Controls.CameraView.CameraState.CleanUp: Value cannot be null. (Parameter 'source')
				// Please report the exception below,
				// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
				// Error decompiling @0600028A Facebook.Messenger.WP8.Controls.CameraView.CameraState.CleanUp
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
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitTryFinally(TryFinally) + 0x28
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlock(Block) + 0x51
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitIfInstruction(IfInstruction) + 0x3d
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
				//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod, EntityDeclaration, DecompileRun, ITypeResolveContext, ExtensionInfo) + 0x426
				// -- continuing with outer exception (ICSharpCode.Decompiler.DecompilerException) --
				// 
			}

			public void SetTransform()
			{
				if (_videoBrush != null)
				{
					((Brush)_videoBrush).Transform = (Transform)(object)GetTransform(((FrameworkElement)_canvas).ActualWidth, ((FrameworkElement)_canvas).ActualHeight);
				}
			}

			private CompositeTransform GetTransform(double width, double height)
			{
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Expected Obj, but got Unknown
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a7: Invalid comparison between Unknown and I4
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d2: Expected Obj, but got Unknown
				Size? size = null;
				try
				{
					size = _avDevice.PreviewResolution;
				}
				catch (Exception ex)
				{
					Logger.Error(this, "Error getting PreviewResolution", "GetTransform", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs", 199);
					Logger.Error(this, ex.ToString(), "GetTransform", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs", 200);
					CurrentState = State.Error;
				}
				if (!size.HasValue)
				{
					return new CompositeTransform();
				}
				double num = width / size.Value.Height;
				double num2 = 1.0;
				if (num > 1.0 || num < 1.0)
				{
					num2 = num;
				}
				bool flag = (int)_sensorLocation == 1;
				uint sensorRotationInDegrees = _avDevice.SensorRotationInDegrees;
				long num3 = (((int)_sensorLocation == 0) ? sensorRotationInDegrees : (0L - (long)sensorRotationInDegrees));
				CompositeTransform val = new CompositeTransform();
				val.Rotation = num3;
				val.ScaleX = num2;
				val.ScaleY = (flag ? (0.0 - num2) : num2);
				val.CenterX = width / 2.0;
				val.CenterY = height / 2.0;
				return val;
			}

			public void PopulateExpandStoryboard(Storyboard sb)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Expected Obj, but got Unknown
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Expected Obj, but got Unknown
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Expected Obj, but got Unknown
				DoubleAnimation val = new DoubleAnimation();
				((Timeline)val).Duration = Duration.op_Implicit(TimeSpan.FromSeconds(0.3));
				val.To = CaptureHeight;
				val.From = ((FrameworkElement)_canvas).ActualHeight;
				CubicEase val2 = new CubicEase();
				((EasingFunctionBase)val2).EasingMode = (EasingMode)0;
				val.EasingFunction = (IEasingFunction)(object)val2;
				DoubleAnimation val3 = val;
				Storyboard.SetTarget((Timeline)(object)val3, (DependencyObject)(object)_canvas);
				Storyboard.SetTargetProperty((Timeline)(object)val3, new PropertyPath("Height", new object[0]));
				((PresentationFrameworkCollection<Timeline>)(object)sb.Children).Add((Timeline)(object)val3);
			}

			public void PopulateCollapseStoryboard(Storyboard sb)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Expected Obj, but got Unknown
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Expected Obj, but got Unknown
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Expected Obj, but got Unknown
				DoubleAnimation val = new DoubleAnimation();
				((Timeline)val).Duration = Duration.op_Implicit(TimeSpan.FromSeconds(0.3));
				val.To = ((FrameworkElement)_canvas).ActualHeight;
				val.From = CaptureHeight;
				CubicEase val2 = new CubicEase();
				((EasingFunctionBase)val2).EasingMode = (EasingMode)0;
				val.EasingFunction = (IEasingFunction)(object)val2;
				DoubleAnimation val3 = val;
				Storyboard.SetTarget((Timeline)(object)val3, (DependencyObject)(object)_canvas);
				Storyboard.SetTargetProperty((Timeline)(object)val3, new PropertyPath("Height", new object[0]));
				((PresentationFrameworkCollection<Timeline>)(object)sb.Children).Add((Timeline)(object)val3);
			}

			public WriteableBitmap Capture()
			{
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Expected Obj, but got Unknown
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Expected Obj, but got Unknown
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00af: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				//IL_0107: Unknown result type (might be due to invalid IL or missing references)
				//IL_014f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0156: Expected Obj, but got Unknown
				int num = (int)((FrameworkElement)_canvas).ActualWidth;
				int num2 = (int)((FrameworkElement)_canvas).ActualHeight;
				WriteableBitmap val = new WriteableBitmap((int)_avDevice.PreviewResolution.Width, (int)_avDevice.PreviewResolution.Height);
				_avDevice.GetPreviewBufferArgb(val.Pixels);
				val.Invalidate();
				WriteableBitmap val2 = new WriteableBitmap(num, num2);
				Rect val3 = new Rect(0.0, 0.0, (double)((BitmapSource)val).PixelWidth, (double)((BitmapSource)val).PixelHeight);
				CompositeTransform transform = GetTransform(((BitmapSource)val).PixelWidth, ((BitmapSource)val).PixelHeight);
				Rect val4 = ((GeneralTransform)transform).TransformBounds(val3);
				double val5 = (double)((BitmapSource)val2).PixelWidth / val4.Width;
				double val6 = (double)((BitmapSource)val2).PixelHeight / val4.Width;
				double num3 = Math.Max(val5, val6);
				transform.ScaleX *= num3;
				transform.ScaleY *= num3;
				Rect val7 = ((GeneralTransform)transform).TransformBounds(val3);
				transform.TranslateX = ((double)num - val7.Width) / 2.0 - val7.X;
				transform.TranslateY = ((double)num2 - val7.Height) / 2.0 - val7.Y;
				Image val8 = new Image();
				val8.Stretch = (Stretch)0;
				val8.Source = (ImageSource)(object)val;
				Image val9 = val8;
				val2.Render((UIElement)(object)val9, (Transform)(object)transform);
				val2.Invalidate();
				return val2;
			}

			public void Focus()
			{
				try
				{
					if (_avDevice != null)
					{
						_avDevice.FocusAsync().DoNotWait<CameraFocusStatus>();
					}
				}
				catch
				{
				}
			}

			[AsyncStateMachine(typeof(_003CStartVideoCaptureAsync_003Ed__30))]
			[DebuggerStepThrough]
			public Task StartVideoCaptureAsync()
			{
				// ILSpy could not decompile this member: Error decompiling @06000291 Facebook.Messenger.WP8.Controls.CameraView.CameraState.StartVideoCaptureAsync: Value cannot be null. (Parameter 'source')
				// Please report the exception below,
				// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
				// Error decompiling @06000291 Facebook.Messenger.WP8.Controls.CameraView.CameraState.StartVideoCaptureAsync
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

			public async Task StopVideoCaptureAsync(bool delete = false)
			{
				_recordDuration = DateTime.Now - _recordingStartTime;
				await StopRecordingAsync();
				if (delete)
				{
					DeleteRecording();
				}
			}

			public async Task<VideoResult> CreateVideoResultAsync()
			{
				if (_filePath == null)
				{
					return null;
				}
				if ((double)_streamLength < 10240.0)
				{
					DeleteRecording();
					return null;
				}
				int rotation = (int)_avDevice.GetProperty(KnownCameraGeneralProperties.SpecifiedCaptureOrientation);
				int frameWidth = (int)_avDevice.PreviewResolution.Width;
				int frameHeight = (int)_avDevice.PreviewResolution.Height;
				if (_thumbnailPixels != null)
				{
					FacebookContext context = ServiceLocator.Current.GetInstance<FacebookContext>();
					try
					{
						StorageFile output = await (await StorageFolder.GetFolderFromPathAsync(await context.Settings.GetUserFolderPath())).CreateFileAsync(string.Format("thumbnail_{0}.jpg", DateTime.Now.ToString("yyyyMMddHHmmssfff")), (CreationCollisionOption)1);
						WriteableBitmap rotatedThumbnail = CreateRotatedBitmapFromPixels(_thumbnailPixels, frameWidth, frameHeight, rotation);
						using (Stream stream = await ((IStorageFile)(object)output).OpenStreamForWriteAsync())
						{
							Extensions.SaveJpeg(rotatedThumbnail, stream, ((BitmapSource)rotatedThumbnail).PixelWidth, ((BitmapSource)rotatedThumbnail).PixelHeight, 0, 85);
						}
						_thumbnailPath = output.Path;
					}
					catch (Exception ex)
					{
						Logger.Error(this, "Unable to create file for thumbnail", "CreateVideoResultAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs", 376);
						Logger.Error(this, ex.ToString(), "CreateVideoResultAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs", 377);
						_thumbnailPath = null;
					}
					finally
					{
						_thumbnailPixels = null;
					}
				}
				int absRotation = Math.Abs(rotation);
				bool isPortrait = absRotation == 90 || absRotation == 270;
				return new VideoResult
				{
					Path = _filePath,
					ThumbnailPath = _thumbnailPath,
					Filename = _filePath,
					Height = (isPortrait ? frameWidth : frameHeight),
					Width = (isPortrait ? frameHeight : frameWidth),
					FileSize = _streamLength,
					Length = (int)_recordDuration.TotalSeconds,
					Rotation = rotation
				};
			}

			private WriteableBitmap CreateRotatedBitmapFromPixels(int[] pixels, int frameWidth, int frameHeight, int rotation)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Expected Obj, but got Unknown
				WriteableBitmap val = new WriteableBitmap(frameWidth, frameHeight);
				for (int i = 0; i < pixels.Length; i++)
				{
					val.Pixels[i] = pixels[i];
				}
				return val.Rotate(rotation);
			}

			[DebuggerStepThrough]
			[AsyncStateMachine(typeof(_003CStopRecordingAsync_003Ed__51))]
			private Task StopRecordingAsync()
			{
				// ILSpy could not decompile this member: Error decompiling @06000295 Facebook.Messenger.WP8.Controls.CameraView.CameraState.StopRecordingAsync: Value cannot be null. (Parameter 'source')
				// Please report the exception below,
				// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
				// Error decompiling @06000295 Facebook.Messenger.WP8.Controls.CameraView.CameraState.StopRecordingAsync
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
				//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod, EntityDeclaration, DecompileRun, ITypeResolveContext, ExtensionInfo) + 0x426
				// -- continuing with outer exception (ICSharpCode.Decompiler.DecompilerException) --
				// 
			}

			private void DeleteRecording()
			{
				Task.Run(async () =>
				{
					FileAccess fileAccess = new FileAccess();
					if (!string.IsNullOrEmpty(_filePath))
					{
						await fileAccess.DeleteFileAtPath(_filePath);
					}
				});
				_thumbnailPixels = null;
				_thumbnailPath = null;
				CleanupLastFrame();
			}

			public void RestoreDefaults()
			{
				CleanupLastFrame();
				if (_videoBrush != null && (object)((Panel)_canvas).Background != _videoBrush)
				{
					CameraVideoBrushExtensions.SetSource(_videoBrush, (object)_avDevice);
					((Panel)_canvas).Background = (Brush)(object)_videoBrush;
				}
			}

			private void CleanupLastFrame()
			{
				((UIElement)_lastFrameCanvas).Visibility = (Visibility)1;
				_lastFramePixels = null;
				if (_lastFrameImageBrush != null)
				{
					_lastFrameImageBrush.ImageOpened -= LastFrameOnImageOpened;
					_lastFrameImageBrush = null;
				}
				((Panel)_lastFrameCanvas).Background = null;
			}

			public void PausePreview()
			{
				// ILSpy could not decompile this member: Error decompiling @06000299 Facebook.Messenger.WP8.Controls.CameraView.CameraState.PausePreview: Value cannot be null. (Parameter 'source')
				// Please report the exception below,
				// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
				// Error decompiling @06000299 Facebook.Messenger.WP8.Controls.CameraView.CameraState.PausePreview
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
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
				//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod, EntityDeclaration, DecompileRun, ITypeResolveContext, ExtensionInfo) + 0x426
				// -- continuing with outer exception (ICSharpCode.Decompiler.DecompilerException) --
				// 
			}

			private void OnThumbnailFrameAvailable(ICameraCaptureDevice sender, object args)
			{
				// ILSpy could not decompile this member: Error decompiling @0600029A Facebook.Messenger.WP8.Controls.CameraView.CameraState.OnThumbnailFrameAvailable: Value cannot be null. (Parameter 'source')
				// Please report the exception below,
				// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
				// Error decompiling @0600029A Facebook.Messenger.WP8.Controls.CameraView.CameraState.OnThumbnailFrameAvailable
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
				//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod, EntityDeclaration, DecompileRun, ITypeResolveContext, ExtensionInfo) + 0x426
				// -- continuing with outer exception (ICSharpCode.Decompiler.DecompilerException) --
				// 
			}

			private void OnLastFrameAvailable(ICameraCaptureDevice sender, object args)
			{
				// ILSpy could not decompile this member: Error decompiling @0600029B Facebook.Messenger.WP8.Controls.CameraView.CameraState.OnLastFrameAvailable: Value cannot be null. (Parameter 'source')
				// Please report the exception below,
				// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
				// Error decompiling @0600029B Facebook.Messenger.WP8.Controls.CameraView.CameraState.OnLastFrameAvailable
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
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement, BlockContainer, IEnumerable`1, Boolean) + 0x26b
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer, Boolean) + 0x47
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer) + 0x94
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction) + 0x5d
				//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction) + 0x11
				//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod, EntityDeclaration, DecompileRun, ITypeResolveContext, ExtensionInfo) + 0x426
				// -- continuing with outer exception (ICSharpCode.Decompiler.DecompilerException) --
				// 
			}

			private void LastFrameOnImageOpened(object sender, RoutedEventArgs routedEventArgs)
			{
				((Panel)_canvas).Background = null;
			}
		}

		private const int MinimumVideoQualityInPixels = 307200;

		public const int MaximumVideoLengthInMilliseconds = 15000;

		public static readonly DependencyProperty FlashOnCaptureProperty = DependencyProperty.Register("FlashOnCapture", typeof(bool), typeof(CameraView), new PropertyMetadata((object)false));

		private readonly CameraState _frontCamera;

		private readonly CameraState _primaryCamera;

		private CameraState _activeCamera;

		private readonly SemaphoreSlim _cameraLock = new SemaphoreSlim(1, 1);

		private State _state;

		private CameraSensorLocation _sensor;

		private DateTime _startTime;

		internal Storyboard FlashAnimation;

		internal Storyboard FadeCameraIn;

		internal Storyboard FadeCameraOut;

		internal ProgressBar InitializingProgressBar;

		internal Canvas CameraCanvas;

		internal Canvas LastFrameCanvas;

		internal Rectangle FlashMask;

		private bool _contentLoaded;

		public bool FlashOnCapture
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(FlashOnCaptureProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(FlashOnCaptureProperty, (object)value);
			}
		}

		public bool CanRotate { get; private set; }

		public bool IsInitializing
		{
			get
			{
				return _state == State.Initializing;
			}
		}

		public double CaptureHeight
		{
			get
			{
				if (_activeCamera == null)
				{
					return 0.0;
				}
				return _activeCamera.CaptureHeight;
			}
		}

		public DateTime StartTime
		{
			get
			{
				return _startTime;
			}
		}

		public event EventHandler<State> StateChanged;

		public CameraView()
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			InitializeComponent();
			_frontCamera = new CameraState((CameraSensorLocation)1, CameraCanvas, LastFrameCanvas, ((DependencyObject)this).Dispatcher);
			_primaryCamera = new CameraState((CameraSensorLocation)0, CameraCanvas, LastFrameCanvas, ((DependencyObject)this).Dispatcher);
			bool flag = Camera.IsCameraTypeSupported((CameraType)0);
			bool flag2 = Camera.IsCameraTypeSupported((CameraType)1);
			CanRotate = flag2 && flag;
			_sensor = (CameraSensorLocation)(flag2 ? 1 : 0);
		}

		public async void CleanUp()
		{
			await _cameraLock.WaitAsync();
			await Reset();
			await _frontCamera.CleanUp();
			await _primaryCamera.CleanUp();
			((Panel)CameraCanvas).Background = null;
			SetState(State.NotStarted);
			_cameraLock.Release();
		}

		public void PopulateExpandStoryboard(Storyboard sb)
		{
			if (_activeCamera != null)
			{
				_activeCamera.PopulateExpandStoryboard(sb);
			}
		}

		public void PopulateCollapseStoryboard(Storyboard sb)
		{
			if (_activeCamera != null)
			{
				_activeCamera.PopulateCollapseStoryboard(sb);
			}
		}

		private void AnimateCameraInFocus(CameraState camera)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected Obj, but got Unknown
			if (_activeCamera == null)
			{
				SetState(State.NotStarted);
			}
			else if (_activeCamera == camera)
			{
				InitializingProgressBar.IsIndeterminate = false;
				FadeCameraIn.Begin();
				SetState(camera.CurrentState);
				if (camera.CurrentState == State.Error)
				{
					((Panel)CameraCanvas).Background = (Brush)new SolidColorBrush(Colors.Black);
				}
			}
		}

		public async Task Reset()
		{
			FadeCameraOut.Begin();
			if (_activeCamera != null)
			{
				await _activeCamera.CleanUp();
				_activeCamera = null;
			}
		}

		public async void Setup(Nullable type = null)
		{
			await _cameraLock.WaitAsync();
			if (_state == State.NotStarted || _state == State.Error)
			{
				SetState(State.Initializing);
				await Reset();
				CameraSensorLocation nextType = type.GetValueOrDefault(_sensor);
				bool cameraNeedsToBeReinitialized = nextType != _sensor;
				_sensor = nextType;
				CameraSensorLocation sensor = _sensor;
				switch ((int)sensor)
				{
				case 1:
					_activeCamera = _frontCamera;
					break;
				case 0:
					_activeCamera = _primaryCamera;
					break;
				}
				InitializingProgressBar.IsIndeterminate = true;
				await _activeCamera.InitializeAsync(cameraNeedsToBeReinitialized);
				if (_activeCamera.CurrentState != State.Initializing)
				{
					AnimateCameraInFocus(_activeCamera);
				}
			}
			_cameraLock.Release();
		}

		public CameraSensorLocation Rotate()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Invalid comparison between Unknown and I4
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (!CanRotate)
			{
				return _sensor;
			}
			CameraSensorLocation value = (CameraSensorLocation)(((int)_sensor != 1) ? 1 : 0);
			SetState(State.NotStarted);
			Setup(value);
			return _sensor;
		}

		public WriteableBitmap Capture()
		{
			if (_activeCamera == null || _activeCamera.CurrentState != State.Ready)
			{
				return null;
			}
			if (FlashOnCapture)
			{
				FlashAnimation.Begin();
			}
			return _activeCamera.Capture();
		}

		public async Task StartVideoCaptureAsync()
		{
			await _cameraLock.WaitAsync();
			if (_activeCamera != null && _state == State.Ready)
			{
				await _activeCamera.StartVideoCaptureAsync();
				SetState(State.Recording);
				_startTime = DateTime.Now;
			}
			_cameraLock.Release();
		}

		public async Task StopVideoCaptureAsync(bool delete = false)
		{
			await _cameraLock.WaitAsync();
			if (_activeCamera != null && _state == State.Recording)
			{
				await _activeCamera.StopVideoCaptureAsync(delete);
				SetState(State.Ready);
			}
			_cameraLock.Release();
		}

		public async Task<VideoResult> GetVideoResultAsync()
		{
			await _cameraLock.WaitAsync();
			VideoResult result = null;
			if (_activeCamera != null && _state == State.Ready)
			{
				result = await _activeCamera.CreateVideoResultAsync();
			}
			_cameraLock.Release();
			return result;
		}

		public async void RestoreDefaults()
		{
			await _cameraLock.WaitAsync();
			if (_activeCamera != null)
			{
				_activeCamera.RestoreDefaults();
			}
			_cameraLock.Release();
		}

		public async void PausePreview()
		{
			await _cameraLock.WaitAsync();
			if (_activeCamera != null)
			{
				_activeCamera.PausePreview();
			}
			_cameraLock.Release();
		}

		private void SetState(State state)
		{
			if (_state != state)
			{
				_state = state;
				if (StateChanged != null)
				{
					StateChanged(this, state);
				}
			}
		}

		public State GetState()
		{
			return _state;
		}

		private void OnCameraGridTapped(object sender, GestureEventArgs e)
		{
			if (_activeCamera != null && _activeCamera.CurrentState == State.Ready)
			{
				_activeCamera.Focus();
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/CameraView.xaml", UriKind.Relative));
				FlashAnimation = (Storyboard)((FrameworkElement)this).FindName("FlashAnimation");
				FadeCameraIn = (Storyboard)((FrameworkElement)this).FindName("FadeCameraIn");
				FadeCameraOut = (Storyboard)((FrameworkElement)this).FindName("FadeCameraOut");
				InitializingProgressBar = (ProgressBar)((FrameworkElement)this).FindName("InitializingProgressBar");
				CameraCanvas = (Canvas)((FrameworkElement)this).FindName("CameraCanvas");
				LastFrameCanvas = (Canvas)((FrameworkElement)this).FindName("LastFrameCanvas");
				FlashMask = (Rectangle)((FrameworkElement)this).FindName("FlashMask");
			}
		}

		static CameraView()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected Obj, but got Unknown
		}
	}
}
