# Metodi recuperati a mano dal IL

ILSpy 11.2.0.10050 non è riuscito a tradurre 15 metodi della DLL principale
(`Facebook.Messenger.WP8.dll`). L'errore non dipende dal codice dell'app ma dal
disambiguatore di argomenti del decompilatore:

```
System.ArgumentNullException: Value cannot be null. (Parameter 'source')
   at ICSharpCode.Decompiler.CSharp.ArgumentList.GetArgumentResolveResultsDirect(Int32)
   at ICSharpCode.Decompiler.CSharp.Disambiguator.Probe()
```

Colpisce i metodi che passano un gruppo di metodi come argomento di
`WindowsRuntimeMarshal.AddEventHandler` / `RemoveEventHandler`. Il IL è integro:
i metodi sono stati tradotti a mano in C#, con i riferimenti IL accanto a ogni
blocco.

## Elenco

| # | Metodo | Ricostruzione | IL |
| --- | --- | --- | --- |
| 1 | `CameraView.CameraState.CleanUp(bool)` | `Facebook.Messenger.WP8/Controls/CameraView.CameraState.Recovered.cs` | `il/...CameraState.il:2466` (stub), `:361` (MoveNext) |
| 2 | `CameraView.CameraState.StartVideoCaptureAsync()` | idem | `:2994` (stub), `:591` (MoveNext) |
| 3 | `CameraView.CameraState.StopRecordingAsync()` | idem | `:3185` (stub), `:1722` (MoveNext) |
| 4 | `CameraView.CameraState.PausePreview()` | idem | `:3324` |
| 5 | `CameraView.CameraState.OnThumbnailFrameAvailable(...)` | idem | `:3351` |
| 6 | `CameraView.CameraState.OnLastFrameAvailable(...)` | idem | `:3414` |
| 7 | `<CleanUp>d__27.MoveNext()` | idem | `:361` |
| 8 | `<StartVideoCaptureAsync>d__30.MoveNext()` | idem | `:591` |
| 9 | `<StopRecordingAsync>d__51.MoveNext()` | idem | `:1722` |
| 10 | `FBGeoLocator.Service.StartGeolocatorAsync()` | `Facebook.Messenger.WP8/FBGeoLocator.Service.Recovered.cs` | `il/...Service.il:2853` (stub), `:489` (MoveNext) |
| 11 | `FBGeoLocator.Service.StopGeolocatorAsync()` | idem | `:2921` (stub), `:903` (MoveNext) |
| 12 | `<StartGeolocatorAsync>d__d.MoveNext()` | idem | `:489` |
| 13 | `<StopGeolocatorAsync>d__14.MoveNext()` | idem | `:903` |
| 14 | `SingleInputPaneHelper.SubscribeToKeyboardEvents(...)` | `Facebook.Messenger.WP8/Utilities/SingleInputPaneHelper.Recovered.cs` | `il/...SingleInputPaneHelper.il:10` |
| 15 | `SingleInputPaneHelper.UnsubscribeFromKeyboardEvents()` | idem | `:52` |

I dump IL completi dei tre tipi sono in `il/`; sono la fonte delle ricostruzioni e
si rigenerano con:

```bash
./.tools/bin/ilspycmd.cmd -il -t "Facebook.Messenger.WP8.Controls.CameraView+CameraState" \
  extracted/Facebook.Messenger.WP8.dll > recovered/il/Facebook_Messenger_WP8_Controls_CameraView_CameraState.il
```

## Avvertenze

- Questi file sono **materiale di analisi**, non compilano da soli: i tipi sono
  estratti (campi e metodi di supporto vivono nel decompilato completo) e le
  direttive `using` coprono solo ciò che serve a leggere il codice.
- I nomi dei parametri locali sono ricostruiti; le stringhe di log, i numeri di
  riga e i valori HResult sono copiati letteralmente dal IL.
- In `CameraView.CameraState.CleanUp` l'`await` di `StopRecordingAsync` sta
  **fuori** dal `try/finally` che azzera `_avDevice`: se l'await lancia, la
  pulizia non avviene. È il comportamento reale del binario, non una svista
  della ricostruzione.
- `PausePreview` non fa null check su `_avDevice` (il IL carica il campo in una
  locale e sottoscrive subito): con dispositivo null lancia.
