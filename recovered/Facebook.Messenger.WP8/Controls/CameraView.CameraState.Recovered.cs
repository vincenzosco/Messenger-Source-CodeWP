// Ricostruzione manuale dal IL dei sei metodi di Facebook.Messenger.WP8.Controls.CameraView.CameraState
// che ILSpy 11.2.0.10050 non riesce a tradurre (ArgumentException/ArgumentNullException nel
// Disambiguator, dentro la risoluzione di WindowsRuntimeMarshal.AddEventHandler).
//
// Enumerazione di riferimento (decompilata senza errori, CameraView.cs:33):
//   NotStarted = 0, Initializing = 1, Ready = 2, Recording = 3, Error = 4
//
// File originale (dai ldstr di log): c:\Open\messenger\Facebook\Facebook-Messenger-WP8\Controls\CameraView.xaml.cs

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Practices.ServiceLocation;
using Windows.Foundation;
using Windows.Phone.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Facebook.Messenger.WP8.Controls
{
    protected partial class CameraView
    {
        private const string SourceFile = "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\CameraView.xaml.cs";

        protected class CameraState
        {
            private AudioVideoCaptureDevice _avDevice;
            private VideoBrush _videoBrush;
            private int[] _thumbnailPixels;
            private int[] _lastFramePixels;
            private IRandomAccessStream _stream;
            private long _streamLength;
            private string _filePath;
            private DateTime _recordingStartTime;
            private bool _isRecording;
            private Dispatcher _dispatcher;

            public State CurrentState { get; set; }

            // ---------------------------------------------------------------
            // CleanUp(bool resetState = true)
            // Stub: IL_0000..IL_0041 (AsyncTaskMethodBuilder.Start + get_Task)
            // MoveNext: IL_0000..IL_0199
            // ---------------------------------------------------------------
            public async Task CleanUp(bool resetState = true)
            {
                // IL_000d..IL_0018: brfalse su _avDevice -> salta tutto il blocco
                if (_avDevice != null)
                {
                    // IL_001d..IL_007d: await StopRecordingAsync(), sta FUORI dal try:
                    // se lancia, il finally di sotto non esegue (comportamento reale).
                    await StopRecordingAsync();

                    // IL_0084..IL_0128: try/catch/finally annidati
                    try
                    {
                        // IL_008a..IL_009a
                        _avDevice.Dispose();
                    }
                    catch (Exception ex)
                    {
                        // IL_009c..IL_00d7: due Logger.Error, righe 162 e 163
                        Facebook.Base.Logging.Logger.Error(this, "Error disposing camera", "CleanUp", SourceFile, 162);
                        Facebook.Base.Logging.Logger.Error(this, ex.ToString(), "CleanUp", SourceFile, 163);
                        // IL_00e2: ldc.i4.4 -> Error
                        CurrentState = State.Error;
                    }
                    finally
                    {
                        // IL_00ef..IL_0116: RemoveEventHandler(remove_PreviewFrameAvailable, OnLastFrameAvailable)
                        _avDevice.PreviewFrameAvailable -= OnLastFrameAvailable;
                        // IL_011b..IL_0122
                        _avDevice = null;
                    }
                }

                // IL_0128..IL_013b: i due riferimenti vengono azzerati
                _videoBrush = null;
                _thumbnailPixels = null;
                // IL_0140..IL_0146
                CleanupLastFrame();

                // IL_014b..IL_0168: resetState && CurrentState != Error -> NotStarted
                if (resetState && CurrentState != State.Error)
                {
                    CurrentState = State.NotStarted;
                }
            }

            // ---------------------------------------------------------------
            // StartVideoCaptureAsync()
            // Stub: IL_0000..IL_0039; MoveNext: IL_0000..IL_03d3
            // ---------------------------------------------------------------
            public async Task StartVideoCaptureAsync()
            {
                // IL_0023..IL_002e: ServiceLocator.Current.GetInstance<FacebookContext>()
                Facebook.Models.FacebookContext context = ServiceLocator.Current.GetInstance<Facebook.Models.FacebookContext>();

                try
                {
                    // IL_0056..IL_00c8
                    string userFolderPath = await context.Settings.GetUserFolderPath();
                    // IL_00d2..IL_0144
                    StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(userFolderPath);

                    // IL_0149..IL_0181: AddEventHandler su PreviewFrameAvailable
                    _avDevice.PreviewFrameAvailable += OnThumbnailFrameAvailable;

                    // IL_0186..IL_0216: "{0}.mp4" con DateTime.Now "yyyyMMddHHmmssfff",
                    // CreationCollisionOption.ReplaceExisting (ldc.i4.1)
                    StorageFile file = await folder.CreateFileAsync(
                        string.Format("{0}.mp4", DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                        CreationCollisionOption.ReplaceExisting);

                    // IL_021b..IL_022c
                    _filePath = file.Path;
                    // IL_0231..IL_02c5: FileAccessMode.ReadWrite (ldc.i4.1)
                    _stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                    // IL_02ca..IL_033a
                    await _avDevice.StartRecordingToStreamAsync(_stream);

                    // IL_0347..IL_035e
                    _recordingStartTime = DateTime.Now;
                    _isRecording = true;
                }
                catch (Exception ex)
                {
                    // IL_0366..IL_03a0, righe 326 e 327
                    Facebook.Base.Logging.Logger.Error(this, "Unable to create file for video", "StartVideoCaptureAsync", SourceFile, 326);
                    Facebook.Base.Logging.Logger.Error(this, ex.ToString(), "StartVideoCaptureAsync", SourceFile, 327);
                }
            }

            // ---------------------------------------------------------------
            // StopRecordingAsync()
            // Stub: IL_0000..IL_0039; MoveNext: IL_0000..IL_01af
            // ---------------------------------------------------------------
            private async Task StopRecordingAsync()
            {
                if (_avDevice != null)
                {
                    // IL_001f..IL_0046: rimozione dell'handler dei thumbnail
                    _avDevice.PreviewFrameAvailable -= OnThumbnailFrameAvailable;

                    // IL_004b..IL_0056
                    if (_isRecording)
                    {
                        // IL_005b..IL_0062: azzerato PRIMA dell'await
                        _isRecording = false;
                        try
                        {
                            // IL_0074..IL_00e0
                            await _avDevice.StopRecordingAsync();
                        }
                        catch (Exception ex)
                        {
                            // IL_00e9..IL_0123, righe 429 e 430
                            Facebook.Base.Logging.Logger.Error(this, "Error when recording stops", "StopRecordingAsync", SourceFile, 429);
                            Facebook.Base.Logging.Logger.Error(this, ex.ToString(), "StopRecordingAsync", SourceFile, 430);
                            // IL_012e: Error
                            CurrentState = State.Error;
                        }
                    }
                }

                // IL_0136..IL_0172: misura e chiude lo stream, solo se esiste
                if (_stream != null)
                {
                    Stream stream = _stream.AsStream();
                    _streamLength = stream.Length;
                    stream.Dispose();
                    _stream = null;
                }

                // IL_0177..IL_017e: azzerato di nuovo, incondizionatamente
                _isRecording = false;
            }

            // ---------------------------------------------------------------
            // PausePreview()
            // IL_0000..IL_0030
            // ---------------------------------------------------------------
            public void PausePreview()
            {
                // Il IL copia _avDevice in una locale e non fa null check:
                // con _avDevice null questo metodo lancia NullReferenceException.
                AudioVideoCaptureDevice avDevice = _avDevice;
                avDevice.PreviewFrameAvailable += OnLastFrameAvailable;
            }

            // ---------------------------------------------------------------
            // OnThumbnailFrameAvailable(ICameraCaptureDevice, object)
            // IL_0000..IL_006d
            // ---------------------------------------------------------------
            private void OnThumbnailFrameAvailable(ICameraCaptureDevice sender, object args)
            {
                if (_avDevice == null)
                {
                    return;
                }

                // IL_0009..IL_0026
                _avDevice.PreviewFrameAvailable -= OnThumbnailFrameAvailable;

                // IL_002b..IL_0068
                if (_thumbnailPixels == null)
                {
                    Size previewResolution = sender.PreviewResolution;
                    int width = (int)previewResolution.Width;
                    Size previewResolution2 = sender.PreviewResolution;
                    int height = (int)previewResolution2.Height;
                    _thumbnailPixels = new int[width * height];
                    sender.GetPreviewBufferArgb(_thumbnailPixels);
                }
            }

            // ---------------------------------------------------------------
            // OnLastFrameAvailable(ICameraCaptureDevice, object)
            // IL_0000..IL_00c4  (+ lambda <OnLastFrameAvailable>b__5b sul dispatcher)
            // ---------------------------------------------------------------
            private void OnLastFrameAvailable(ICameraCaptureDevice sender, object args)
            {
                if (_avDevice == null)
                {
                    return;
                }

                // IL_0009..IL_0026
                _avDevice.PreviewFrameAvailable -= OnLastFrameAvailable;

                if (_lastFramePixels != null)
                {
                    return;
                }

                // IL_0036..IL_0068: display class con width/height/rotation della preview
                _LastFrameCapture capture = new _LastFrameCapture();
                capture.CameraState = this;

                Size previewResolution = sender.PreviewResolution;
                capture.FrameWidth = (int)previewResolution.Width;
                Size previewResolution2 = sender.PreviewResolution;
                capture.FrameHeight = (int)previewResolution2.Height;
                // IL_006d..IL_0083: KnownCameraGeneralProperties.SpecifiedCaptureOrientation
                capture.Rotation = (int)_avDevice.GetProperty(KnownCameraGeneralProperties.SpecifiedCaptureOrientation);

                // IL_0088..IL_00a7
                _lastFramePixels = new int[capture.FrameWidth * capture.FrameHeight];
                sender.GetPreviewBufferArgb(_lastFramePixels);

                // IL_00ac..IL_00be: il lavoro grafico (CreateRotatedBitmapFromPixels)
                // viene rimandato al dispatcher UI.
                _dispatcher.BeginInvoke(capture.CreateBitmap);
            }

            // Il compilatore generava una classe di supporto <>c__DisplayClass5c:
            // qui è resa esplicita per leggibilità, con gli stessi campi.
            private sealed class _LastFrameCapture
            {
                public CameraState CameraState;
                public int FrameWidth;
                public int FrameHeight;
                public int Rotation;

                public void CreateBitmap()
                {
                    // corpo originale: <>c__DisplayClass5c.<OnLastFrameAvailable>b__5b
                    // (presente senza errori in CameraView.cs, non riprodotto qui)
                }
            }
        }
    }
}
