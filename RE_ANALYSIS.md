# Analisi del pacchetto `Messenger v11.0.1.0`

Reverse engineering statico del pacchetto Windows Phone fornito dall'utente
(`Messenger v11.0.1.0 [file.windowsviet.com].xap`, 6.670.984 byte) e recupero
del codice sorgente C#/XAML in `decompiled/`.

## Oggetto

| Campo | Valore |
| --- | --- |
| Formato | XAP (container OPC/ZIP, Windows Phone 8.1 Silverlight) |
| Identity | `Facebook.317180B0BB486` |
| Publisher | `CN=6E08453F-9BA7-4311-999C-D22FBA2FB1B8` |
| Versione | `11.0.1.0`, architettura `arm` |
| Entry point | assembly `Facebook.Messenger.WP8`, tipo `Facebook.Messenger.WP8.App`, runtime `6.7.50308.0` |
| Capability | `internetClientServer` |
| Contenuto | 343 file, 18.147.121 byte decompressi |

Il pacchetto contiene immagini **MDIL** precompilate (`MDILFileListXap.xml`,
`MDILProjectFiles.xml`, `ServerHost.exe`), ma il IL gestito è rimasto intatto:
tutte le assembly first-party sono decompilabili.

## Metodo

1. Estrazione del container ZIP in `extracted/` (`unzip`), lettura di
   `AppManifest.xaml` e `AppxManifest.xml` per entry point e parti caricate.
2. Toolchain locale, senza installazione di sistema: .NET SDK 10.0.100 portabile
   in `.tools/dotnet`, decompilatore `ilspycmd` 11.2.0.10050 in `.tools/bin`.
3. Decompilazione per progetto (`-p --nested-directories`) con reference path
   `extracted/`, che ha ricostruito anche le risorse XAML sciolte incluse
   nell'assembly.

## Inventario assembly decompilate

| Assembly | Ruolo | File `.cs` |
| --- | --- | --- |
| `Facebook.Messenger.WP8` | applicazione, pagine, controlli, view model | 422 |
| `Facebook-Services` | servizi: GraphQL, cache, invio messaggi, analytics | 429 |
| `Facebook-Models` | modello dati e signal | 291 |
| `Facebook-Base` | rete, MQTT astratto, logging, estensioni | 92 |
| `Facebook-Messenger-WP8-Base` | MQTT su socket WinRT, helper di piattaforma | 59 |
| `Facebook-Messenger-WP8-Agents` | background agent (invio in background) | 10 |
| `Facebook-Messenger-Localization` | string library | 5 |
| `ThriftPortable` | trasporto Thrift | 38 |

Totale recuperato: **1.346 file C#** (112.254 righe) e **119 file XAML**
(pagine, controlli, stili, temi), più i manifest e gli asset di `extracted/`.

## Architettura ricostruita

- MVVM: `GalaSoft.MvvmLight` + `Microsoft.Practices.ServiceLocation`, view model
  risolti da `FacebookMessengerUiLocator.Instance` in `App`.
- Bootstrap: `App` costruisce `FacebookContext`, `ServiceEngine` e i servizi
  `MessageSender`/`Analytics`; `SingleExecutionRuntimeLock` evita doppie istanze.
- Trasporto messaggi: MQTT proprio sopra `StreamSocket` WinRT
  (`MqttClient`, `MqttStreamReader/Writer`) con codec in `Facebook.Base.Mqtt`.
- API: livello GraphQL con le query in `Facebook/Services/Api/GraphQL/Constants.cs`
  (282 KB) e i tipi comuni in `Common.cs`.
- Persistenza/cache: `Facebook/Services/Api/Cache/ThreadCache.cs` e cache per
  entità; serializzazione con `Newtonsoft.Json`, compressione `Zlib.Portable`.
- Media: `CameraView`, `VoiceClipDrawer`, `ImageTools`, `PixelLab`, `WebP-WP8`,
  `WriteableBitmapExWinPhone`, `ShakeGestures`, `ExifLib`.

## Limiti dell'analisi

- **15 metodi non decompilati automaticamente** nella DLL principale: state
  machine `async` di `CameraView.CameraState`, `FBGeoLocator.Service` e
  `SingleInputPaneHelper` (`Value cannot be null (Parameter 'source')`). Sono
  stati ritradotti a mano dal IL in `recovered/` (indice in
  `recovered/README.md`, dump IL in `recovered/il/`).
- Nessun PDB nel pacchetto: i nomi dei campi sono corretti, quelli delle
  variabili locali sono sintetici (`num`, `text`).
- Assembly di terze parti non decompilate perché ridistribuite invariate e con
  sorgente pubblico: `Newtonsoft.Json`, `GalaSoft.MvvmLight*`,
  `Microsoft.Phone.Controls*`, `WriteableBitmapExWinPhone`, `Google.PhoneNumbers`,
  `Zlib.Portable`, `ExifLib`, `WebP-WP8`, `Microsoft.Practices.ServiceLocation`,
  `System.Net.Http*`, `System.Windows.Interactivity`.
- Le `.resources` delle 60 assembly satellite di localizzazione non sono state
  ricostruite in `.resx`: restano in `extracted/<lingua>/`.
- Asset grafici e audio non sono copiati in `decompiled/`: sono in `extracted/`.

## Riproduzione

```bash
# toolchain locale
curl -fsSL -o .tools/dotnet-sdk.zip \
  https://builds.dotnet.microsoft.com/dotnet/Sdk/10.0.100/dotnet-sdk-10.0.100-win-x64.zip
unzip -q .tools/dotnet-sdk.zip -d .tools/dotnet
./.tools/dotnet/dotnet.exe tool install ilspycmd --tool-path ./.tools/bin

# payload e decompilazione
unzip -o -q "Messenger v11.0.1.0 [file.windowsviet.com].xap" -d extracted
for a in Facebook.Messenger.WP8 Facebook-Messenger-WP8-Base \
         Facebook-Messenger-WP8-Agents Facebook-Base Facebook-Services \
         Facebook-Models Facebook-Messenger-Localization ThriftPortable; do
  ./.tools/bin/ilspycmd.cmd -p -o "decompiled/$a" --nested-directories \
    -r extracted "extracted/$a.dll"
done
```

## Nota d'uso

Il codice decompilato è proprietà di Facebook/Meta ed è qui conservato a solo
scopo di studio e interoperabilità. Non ridistribuirlo e non pubblicarlo: il
repository è di lavoro personale, `decompiled/` non entra in alcun build.
