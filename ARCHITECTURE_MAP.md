# Mappa dei punti di ingresso e dei flussi

Riferimenti al codice decompilato in `decompiled/`. Dove un passaggio è stato
letto nel codice è marcato **(letto)**; dove è desunto dal nome dei tipi o dalla
presenza dei file è marcato **(desunto)**.

## 1. Punti di ingresso

| Ingresso | Dove | Note |
| --- | --- | --- |
| Avvio app | `extracted/AppManifest.xaml` | `EntryPointAssembly="Facebook.Messenger.WP8"`, `EntryPointType="Facebook.Messenger.WP8.App"`, runtime `6.7.50308.0` |
| Costruttore `App` | `decompiled/Facebook.Messenger.WP8/Facebook/Messenger/WP8/App.cs:75` | **(letto)** cabla tutto il grafo di servizi, vedi §2 |
| `Application_Launching` | `App.cs:200` | **(letto)** `StartApplication(...)`, `_navigator.ApplicationStarted()`, salva `PendingShare` se l'avvio è uno share target |
| `Application_Activated` | `App.cs:211` | **(letto)** `StartApplication` + `_navigator.ApplicationResumed(e.IsApplicationInstancePreserved)` |
| `Application_Deactivated` / `_Closing` | `App.cs:255`, `:260` | **(letto)** `Teardown()`: log di spegnimento, `SavePhoneLanguage`, avvio dell'agent se ci sono invii in coda |
| Share target | `App.cs:73` (`PendingShare`) + `pages/externallaunch.xaml` | **(letto)** `ShareOperation` conservata e consumata dalla pagina di lancio |
| Background agent | `decompiled/Facebook-Messenger-WP8-Agents/.../MessageSendingBackgroundInstance.cs:65` | **(letto)** chiama `_serviceEngine.StartServices(_context)` come il processo principale |
| Host nativo del background | `extracted/ServerHost.exe`, `ServerHost.exe` + `BackgroundServerHost` in `AppxManifest.xml` | **(letto)** processo separato che ospita l'agent |

`StartApplication` (`App.cs:217`) è il punto in cui si entra davvero nel runtime:
`ShutDownBackgroundAgent` → `_applicationState.AppIsStartingUp()` →
`ResumeApplicationFrame()` → `_serviceEngine.StartServices(_context)`, con i log di
avvio/spegnimento raccolti da `StartupLogger`/`ShutdownLogger` e spediti con
`CrashSender`.

## 2. Bootstrap dei servizi

**(letto)** `App` ctor:

1. `ServiceLocator.SetLocatorProvider(() => FacebookMessengerUiLocator.Instance)`.
2. `ServiceEngine`, `MessageSender.Service`, `Analytics.Service`, `INetworkStatus`,
   `ApplicationState`, `BackgroundSendingAgentEnabledProvider`, `FacebookContext`.
3. Sottoscrizioni `FacebookContext.LoggedIn/LoggedOut/GlobalMuteEvent`.
4. `InitializePhoneApplication()`, `InitializeDispatcher()` (`UiDispatcher` sopra
   il dispatcher del `RootFrame`), `PhoneContactsManager.Register()`,
   `InitializeLanguage()`, `ThemeManager.ToLightTheme()`.

**(letto)** `Facebook-Messenger-WP8-Base/Facebook/Messenger/ServiceEngine.cs:28`
espone `StartServices(FacebookContext)` / `StopServices()`: è l'unico punto in cui
i servizi applicativi vengono avviati, condiviso da app e background agent.

**(letto)** `Facebook-Messenger-WP8-Base/Facebook/Messenger/FacebookMessengerLocator.cs:73`
registra il trasporto: `RegisterSingle((Func<AbstractMqttClient>)(() => new MqttClient()))`.
Quindi tutto il resto del codice parla con l'astrazione `AbstractMqttClient`, e
solo la locator conosce l'implementazione WinRT.

## 3. Flusso MQTT

```
ServiceEngine.StartServices
        │
        ├── AbstractMqttClient.Connect(MqttParameters)        Facebook-Base/Mqtt/AbstractMqttClient.cs:175
        │        └── MqttClient.OpenSocket()                  Messenger-WP8-Base/Mqtt/MqttClient.cs:105
        │                 StreamSocket + DataReader/DataWriter (WinRT)
        │
        ├── AbstractMqttClient.LoopMessages()                 Facebook-Base/Mqtt/AbstractMqttClient.cs (log "LoopMessages", ~riga 145)
        │        └── MessageDecoder → MqttMessage → segnali MVVM
        │
        └── MqttConnectUser (handshake di login)              Facebook-Services/Mqtt/MqttConnectUser.cs
```

Dettagli verificati:

- **(letto)** `AbstractMqttClient` registra con `Logger.Info` host e porta al
  `Connect` ("Connect called for host - {0}:{1}") e ignora le chiamate ripetute
  ("Already connected or connecting").
- **(letto)** `MqttClient` implementa `IMqttStreamReader`/`IMqttStreamWriter` sopra
  `IDataReader`/`IDataWriter`; `WriteMessageAndStoreAsync` codifica il messaggio con
  `MessageEncoder.Write` e poi `StoreAsync` + `FlushAsync`.
- **(letto)** l'invio applicativo passa da
  `Facebook-Services/Api/Methods/SendMessage/MqttFilter.cs`, un `Filter<Request,Response>`
  sulla pipeline delle API: la stessa chiamata può cioè usare MQTT o HTTP.
- **(letto)** lo stato della connessione viaggia come segnale MVVM
  (`Facebook-Models/Signals/MqttConnectionStateChangedSignal.cs`), e l'agent aspetta
  la connessione con `Agents/Helpers/MqttConnectionWaiter.cs`.
- **(letto)** i messaggi hanno header Thrift:
  `Facebook-Models/SyncProtocol/MqttThriftHeader.cs` + `ThriftPortable`.

## 4. Flusso GraphQL

```
UI / ViewModel
    │  (ServiceLocator)
    ▼
Facebook.Services.Api.Service                         Facebook-Services/Api/Service.cs
    │
    ├── GraphQL query: hash + nome in Constants.cs    Api/GraphQL/Constants.cs (282 KB)
    ├── tipi comuni / fragment                        Api/GraphQL/Common.cs (123 KB)
    ├── cache per entità                              Api/Cache/ThreadCache.cs
    └── trasporto HTTP                                System.Net.Http + Zlib.Portable
```

- **(letto)** `Constants.cs` è il catalogo delle query GraphQL usate dal client: una
  costante per query, con il nome dell'operazione e l'hash/identificatore.
- **(letto)** `Service.cs` (38 KB) è il chiamante unico: compone la richiesta,
  applica i filtri (`MqttFilter` compreso) e restituisce i modelli.
- **(desunto)** la cache `ThreadCache` serve le conversazioni già viste prima della
  risposta di rete: i `Signal` in `Facebook-Models` notificano i view model.

## 5. Dove guardare

```bash
# ingressi
grep -n "Application_Launching\|StartApplication\|PendingShare" \
  decompiled/Facebook.Messenger.WP8/Facebook/Messenger/WP8/App.cs

# bootstrap
grep -rn "StartServices\|RegisterSingle" decompiled/Facebook-Messenger-WP8-Base decompiled/Facebook.Messenger.WP8

# MQTT
grep -rn "AbstractMqttClient\|MqttFilter\|MqttConnectUser" decompiled/

# GraphQL
grep -n "GraphQL" decompiled/Facebook-Services/Facebook/Services/Api/Service.cs | head
ls -l decompiled/Facebook-Services/Facebook/Services/Api/GraphQL/
```

## 6. Cosa manca

- Non è stato tracciato un singolo scambio reale end-to-end (nessun traffico): la
  mappa unisce lettura del codice e struttura dei tipi.
- Le pagine XAML sono recuperate, ma i binding non sono stati incrociati con i view
  model per ogni schermata.
- Lato nativo (`ServerHost.exe`, immagini MDIL) non è stato analizzato: qui conta
  solo il IL gestito.
