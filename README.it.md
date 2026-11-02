<p align="center">
  <img src="docs/logo.png" width="128" alt="Icona di Facebook Messenger per Windows Phone 8.1">
</p>

<h1 align="center">Messenger 2015 — codice recuperato</h1>

<p align="center">
  <strong>Il codice C# e XAML di Messenger per Windows Phone 8.1, ricostruito in progetti compilabili.</strong><br>
  Decompilato dal pacchetto 11.0.1.0 e riportato a C# 5, la lingua con cui usciva nel 2015.
</p>

<p align="center">
  <a href="#come-si-compila">Compilare</a> ·
  <a href="#struttura">Struttura</a> ·
  <a href="#limiti-noti">Limiti</a> ·
  <a href="RE_ANALYSIS.md">Analisi del pacchetto</a>
</p>

<p align="center">
  Leggi in: <a href="README.md">English</a> · <strong>Italiano</strong>
</p>

Questo repository contiene i sorgenti recuperati dal `.xap` di Messenger 11.0.1.0
per Windows Phone 8.1: 1.346 file C#, 119 file XAML e le otto assembly di prima
parte, ognuna come progetto separato. La soluzione compila con MSBuild 18 contro
i reference set WP8.1.

Non è un fork e non è il sorgente ufficiale. È l'output di un decompilatore, con
i buchi che questo comporta.

## Cosa c'è dentro

| Assembly | Ruolo | Progetto |
| --- | --- | --- |
| `Facebook.Messenger.WP8` | app: pagine, controlli, view model | `Messenger/` |
| `Facebook-Services` | GraphQL, cache, invio messaggi, analytics | `src/Facebook-Services/` |
| `Facebook-Models` | modello dati e signal | `src/Facebook-Models/` |
| `Facebook-Base` | rete, MQTT astratto, logging, estensioni | `src/Facebook-Base/` |
| `Facebook-Messenger-WP8-Base` | MQTT su socket WinRT, helper di piattaforma | `src/Facebook-Messenger-WP8-Base/` |
| `Facebook-Messenger-WP8-Agents` | background agent di invio | `src/Facebook-Messenger-WP8-Agents/` |
| `Facebook-Messenger-Localization` | string library | `src/Facebook-Messenger-Localization/` |
| `ThriftPortable` | trasporto Thrift | `src/ThriftPortable/` |

L'architettura ricostruita è descritta in `RE_ANALYSIS.md`: MVVM con MvvmLight,
MQTT sopra `StreamSocket`, le query GraphQL in
`Facebook/Services/Api/GraphQL/Constants.cs` (282 KB di query), cache su
`Newtonsoft.Json` e `Zlib.Portable`.

## Stato

- **0 errori di compilazione** in `Debug|Any CPU` e `Release|Any CPU`.
- Tutte e otto le assembly producono la propria DLL e l'app emette 115 file
  `.g.cs` dal markup compiler, quindi le pagine XAML compilano davvero.
- **Il `.xap` non viene prodotto.** I target di packaging arrivano in fondo
  (`FilesToXap` e `XapPackager` escono con 0) ma non scrivono nulla: il task che
  raccoglie il payload arriva con il Windows Phone 8.1 SDK standalone, che qui
  non è installato. Tutto quello che precede quel passo è verificato.

## Requisiti

- Windows con i target **Windows Phone 8.1** installati
  (`%ProgramFiles(x86)%\MSBuild\Microsoft\WindowsPhone\v8.1`).
- MSBuild 18 (Visual Studio 2026 o Build Tools). Non serve .NET SDK.
- Le DLL di terze parti sono già in `lib/`.

## Come si compila

I sorgenti in `Messenger/` e `src/` sono **generati**. Non si modificano a mano,
si rigenerano da `decompiled/`:

```bash
bash tools/import-sources.sh
```

Lo script ricopia `decompiled/` nei progetti, riscrive i `.csproj`, il manifest e
le risorse, e applica i fixup. Poi si compila:

```bash
"C:/Program Files (x86)/Microsoft Visual Studio/18/BuildTools/MSBuild/Current/Bin/MSBuild.exe" \
  Messenger.sln -p:Configuration=Debug -p:Platform="Any CPU"
```

Dopo un import i `.g.cs` del XAML vanno rigenerati una volta prima che i sorgenti
tornino d'accordo con loro:

```bash
MSBuild.exe Messenger.sln -p:Configuration=Debug -p:Platform="Any CPU"
python tools/fixups-gfields.py
MSBuild.exe Messenger.sln -p:Configuration=Debug -p:Platform="Any CPU"
```

Il decompilatore emette dentro ogni classe di pagina anche i campi che il
compilatore XAML rigenera; il comando centrale riconcilia i due elenchi. Senza
quel passaggio il progetto segnala membri duplicati.

Compilare un singolo progetto invece della soluzione richiede
`-p:Platform=AnyCPU`, senza spazio: i progetti generati hanno condizioni su
`Debug|AnyCPU`.

## Struttura

```
Messenger/              app: pagine, controlli, view model, App.xaml
src/<assembly>/         le sette librerie di prima parte
lib/                    DLL di terze parti (toolkit, Json.NET, WebP, Thrift...)
decompiled/             output del decompilatore, sorgente di verità dell'import
recovered/              i 15 metodi async ritradotti a mano dal IL
localization/           .resx per lingua, estratti dalle assembly satellite
extracted/              payload grezzo del .xap (non versionato)
build/                  fallback di packaging per l'SDK WP8.1 mancante
tools/                  import e fixup
```

`decompiled/`, `recovered/` e `localization/` sono materiale di analisi: non
entrano nel build.

<details>
<summary><strong>Come funziona l'import</strong> cosa fa ogni script in <code>tools/</code></summary>
<br>

`import-sources.sh` rigenera gli otto progetti e chiama in ordine:

- `fixups-xaml.py` ricostruisce i code-behind delle pagine e rimuove i membri che
  il compilatore XAML rigenera.
- `fixups-ambiguous.py` risolve i nomi presenti in due namespace
  (`GestureEventArgs`, `Rect`, `Size`) con alias espliciti.
- `fixups-residual.py` corregge le forme puntuali: `Nullable` senza argomento,
  cast a `this`, override delle proprietà di pagina, parametri `out`, eventi
  ricostruiti.
- `fixups-il.py` sistema gli artefatti del IL: accessi protetti qualificati,
  membri enum senza nome, accessor chiamati come metodi, `Duration.op_Implicit`.
- `fixups-links.py` traduce `HyperlinkButton.Command`, che Silverlight non ha, in
  `Click`, e toglie il riferimento `Windows` ridondante.
- `fixups-resources.py` riporta a `x:Key` gli `x:Name` dei dizionari.
- `fixups-gfields.py` riconcilia i campi duplicati dei `.g.cs` (dopo una build).

`build/WpPackagingFallback.targets` copre due pezzi della catena di packaging che
arrivano con l'SDK WP8.1 standalone: definisce il target
`_GetRecursiveResolvedSDKReferences`, citato dai target WP ma mai dichiarato, e
inizializza `$(WMAppManifestOutput)` da `Properties/WMAppManifest.xml`.

</details>

## Limiti noti

- **15 metodi async** non sono stati decompilati automaticamente (state machine
  di `CameraView`, `FBGeoLocator`, `SingleInputPaneHelper`). Restano come stub che
  lanciano, con il rimando a `recovered/il/`.
- **Nomi delle variabili locali sintetici**: il pacchetto non contiene PDB, quindi
  i campi hanno i nomi veri ma le locali si chiamano `num`, `text`, `flag`.
- **Localizzazioni parziali**: le 60 assembly satellite sono in `extracted/`; solo
  tre string library sono state convertite in `.resx`.
- **Nessun `.xap` installabile**, quindi il codice si compila e si legge ma da qui
  non si distribuisce su un dispositivo.
- Il designer XAML va aperto con questi progetti caricati e compilati una volta;
  gli errori su `LocalizedStrings` o `StaticResource` dicono che l'assembly
  dell'app non era risolta nella build di design, non che il tipo manca.

## Note legali

Il codice è proprietà di Facebook/Meta ed è qui conservato a scopo di studio e
interoperabilità. Il repository è privato: non va reso pubblico né ridistribuito
in altra forma. `decompiled/`, `recovered/` e `localization/` sono materiale di
analisi, non codice di cui questo repository detenga i diritti.

Nessuna licenza è concessa: i termini d'uso sono quelli del software originale.
