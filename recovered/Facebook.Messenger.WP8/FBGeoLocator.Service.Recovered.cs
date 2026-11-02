// Ricostruzione manuale dal IL di Facebook.Messenger.WP8.FBGeoLocator.Service
// (stub + state machine StartGeolocatorAsync/StopGeolocatorAsync), non tradotti da
// ILSpy 11.2.0.10050 per lo stesso difetto del Disambiguator visto su CameraState.
//
// Enumerazione (decompilata correttamente, Service.cs:27):
//   NotAvailable = 0, Invalid = 1, Ready = 2
//
// File originale (dai ldstr di log): c:\Open\messenger\Facebook\Facebook-Messenger-WP8\FBGeoLocator\Service.cs

using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Facebook.Messenger.WP8.FBGeoLocator
{
    public partial class Service
    {
        private const string SourceFile = "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\FBGeoLocator\\Service.cs";

        private Facebook.Base.DisposableMutex _geolocatorLock;
        private Geolocator _geolocator;
        private GeolocatorStatus _geolocatorStatus;
        private CancellationTokenSource _cancellationTokenSource;

        public enum GeolocatorStatus
        {
            NotAvailable,
            Invalid,
            Ready
        }

        // ---------------------------------------------------------------
        // StartGeolocatorAsync()
        // Stub: IL_0000..IL_0039  (AsyncTaskMethodBuilder.Start + get_Task)
        // MoveNext: IL_0000..IL_037d
        // ---------------------------------------------------------------
        public async Task StartGeolocatorAsync()
        {
            // IL_001a..IL_0032: nessun permesso/stato valido -> notifica e esce
            if (!CanStartGeolocator())
            {
                OnGeolocatorStatusChanged();
                return;
            }

            // IL_0037..IL_00af: il mutex è preso con await, quindi "using (await ...)"
            using (await _geolocatorLock.WaitAsync())
            {
                // IL_00c4..IL_00d2: già pronto -> esce (ldc.i4.2 == Ready)
                if (_geolocatorStatus == GeolocatorStatus.Ready)
                {
                    return;
                }

                // IL_00d7..IL_00dd
                StopRefetchLocationTimer();

                // IL_00e2..IL_0188: creazione lazy del Geolocator e handler
                if (_geolocator == null)
                {
                    Geolocator geolocator = new Geolocator();
                    geolocator.MovementThreshold = 1.0;
                    _geolocator = geolocator;
                    _geolocator.PositionChanged += GeolocatorOnPositionChanged;
                    _geolocator.StatusChanged += GeolocatorOnStatusChanged;
                }

                // IL_018d..IL_01a4
                _geolocatorStatus = GeolocatorStatus.Ready;
                _cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    // IL_01c3..IL_0245: lettura posizione annullabile
                    Geoposition position = await _geolocator
                        .GetGeopositionAsync()
                        .AsTask(_cancellationTokenSource.Token);

                    // IL_0255..IL_0272
                    UpdateCurrentLocationWithGeoposition(position);
                    ReverseGeocodeWithGeoposition(position);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // IL_027c..IL_0292, riga 354
                    Facebook.Base.Logging.Logger.Error(ex.ToString(), "StartGeolocatorAsync", SourceFile, 354);
                }
                catch (TaskCanceledException ex2)
                {
                    // IL_029c..IL_02b2, riga 358
                    Facebook.Base.Logging.Logger.Error(ex2.ToString(), "StartGeolocatorAsync", SourceFile, 358);
                }
                catch (Exception ex3)
                {
                    // IL_02b9..IL_0321: alcuni HResult sono "attesi" (permesso negato,
                    // servizio spento, timeout) e vengono solo loggati; gli altri
                    // vengono rilanciati con rethrow.
                    switch (ex3.HResult)
                    {
                        case -2147467260: // E_ABORT
                        case -2147024883: // 0x8007000D
                        case -2147024664: // 0x800700E8
                        case -2147023436: // 0x800705B4 (timeout)
                        case -2147019873: // 0x8007139F
                        case -2147012894: // 0x80072EE2 (timeout rete)
                            // IL_0303..IL_0318, riga 377
                            Facebook.Base.Logging.Logger.Error(ex3.ToString(), "StartGeolocatorAsync", SourceFile, 377);
                            break;
                        default:
                            throw;
                    }
                }
                finally
                {
                    // IL_0325..IL_032f
                    StartRefetchLocationTimer();
                }
            }
        }

        // ---------------------------------------------------------------
        // StopGeolocatorAsync()
        // Stub: IL_0000..IL_0039; MoveNext: IL_0000..IL_00b1
        // ---------------------------------------------------------------
        private async Task StopGeolocatorAsync()
        {
            // IL_000d..IL_0024: PerformWithGeolocatorLockAsync(<StopGeolocatorAsync>b__13)
            // Il corpo del lambda (rimozione handler, Dispose del Geolocator, stato)
            // è decompilato senza errori in Service.cs: è l'unica parte già leggibile.
            await PerformWithGeolocatorLockAsync(delegate
            {
                // <StopGeolocatorAsync>b__13: vedi Service.cs decompilato
            });
        }
    }
}
