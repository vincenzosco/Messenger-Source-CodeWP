<p align="center">
  <img src="docs/logo.png" width="128" alt="Icon of Facebook Messenger for Windows Phone 8.1">
</p>

<h1 align="center">Messenger 2015 — recovered source</h1>

<p align="center">
  <strong>The C# and XAML behind Messenger for Windows Phone 8.1, rebuilt into projects that compile.</strong><br>
  Decompiled from the 11.0.1.0 package and brought back to C# 5, the language it shipped with in 2015.
</p>

<p align="center">
  <a href="#build-it">Build</a> ·
  <a href="#layout">Layout</a> ·
  <a href="#known-limits">Limits</a> ·
  <a href="RE_ANALYSIS.md">Package analysis</a>
</p>

<p align="center">
  Read in: <strong>English</strong> · <a href="README.it.md">Italiano</a>
</p>

This repository holds the sources recovered from the Messenger 11.0.1.0 `.xap`
for Windows Phone 8.1: 1,346 C# files, 119 XAML files and the eight first-party
assemblies, each one as its own project. The solution compiles with MSBuild 18
against the WP8.1 reference sets.

It is not a fork and it is not the official source. It is the output of a
decompiler, with the gaps that implies.

## What is inside

| Assembly | Job | Project |
| --- | --- | --- |
| `Facebook.Messenger.WP8` | app: pages, controls, view models | `Messenger/` |
| `Facebook-Services` | GraphQL, caches, message sending, analytics | `src/Facebook-Services/` |
| `Facebook-Models` | data model and signals | `src/Facebook-Models/` |
| `Facebook-Base` | networking, abstract MQTT, logging, extensions | `src/Facebook-Base/` |
| `Facebook-Messenger-WP8-Base` | MQTT over a WinRT socket, platform helpers | `src/Facebook-Messenger-WP8-Base/` |
| `Facebook-Messenger-WP8-Agents` | background sending agent | `src/Facebook-Messenger-WP8-Agents/` |
| `Facebook-Messenger-Localization` | string library | `src/Facebook-Messenger-Localization/` |
| `ThriftPortable` | Thrift transport | `src/ThriftPortable/` |

The reconstructed architecture is written up in `RE_ANALYSIS.md`: MVVM with
MvvmLight, MQTT over `StreamSocket`, GraphQL queries in
`Facebook/Services/Api/GraphQL/Constants.cs` (282 KB of them), caches on
`Newtonsoft.Json` and `Zlib.Portable`.

## Status

- **0 compile errors** in `Debug|Any CPU` and `Release|Any CPU`.
- All eight assemblies produce their DLLs, and the app emits 115 `.g.cs` files
  from the markup compiler, so the XAML pages really do compile.
- **No `.xap` comes out.** The packaging targets run to the end
  (`FilesToXap` and `XapPackager` both exit 0) but write nothing: the task that
  collects the payload ships with the standalone Windows Phone 8.1 SDK, which is
  not installed here. Everything before that step is verified.

## Requirements

- Windows with the **Windows Phone 8.1** targets installed
  (`%ProgramFiles(x86)%\MSBuild\Microsoft\WindowsPhone\v8.1`).
- MSBuild 18 (Visual Studio 2026 or Build Tools). The .NET SDK is not needed.
- Third-party DLLs are already in `lib/`.

## Build it

The sources under `Messenger/` and `src/` are **generated**. Do not edit them by
hand; regenerate them from `decompiled/`:

```bash
bash tools/import-sources.sh
```

That script copies `decompiled/` into the projects, rewrites the `.csproj`
files, the manifest and the resources, then applies the fixups. Then compile:

```bash
"C:/Program Files (x86)/Microsoft Visual Studio/18/BuildTools/MSBuild/Current/Bin/MSBuild.exe" \
  Messenger.sln -p:Configuration=Debug -p:Platform="Any CPU"
```

After an import, the XAML `.g.cs` files have to be regenerated once before the
sources agree with them:

```bash
MSBuild.exe Messenger.sln -p:Configuration=Debug -p:Platform="Any CPU"
python tools/fixups-gfields.py
MSBuild.exe Messenger.sln -p:Configuration=Debug -p:Platform="Any CPU"
```

The decompiler also emits, inside each page class, the fields the XAML compiler
regenerates. Reconciling those is what the middle command does; without it the
project reports duplicate members.

Building a single project instead of the solution needs `-p:Platform=AnyCPU`,
without the space: the generated projects key their conditions on `Debug|AnyCPU`.

## Layout

```
Messenger/              the app: pages, controls, view models, App.xaml
src/<assembly>/         the seven first-party libraries
lib/                    third-party DLLs (toolkit, Json.NET, WebP, Thrift...)
decompiled/             decompiler output, the source of truth for the import
recovered/              the 15 async methods retyped by hand from IL
localization/           .resx per language, pulled from the satellite assemblies
extracted/              raw .xap payload (not committed)
build/                  packaging fallback for the missing WP8.1 SDK
tools/                  import and fixup scripts
```

`decompiled/`, `recovered/` and `localization/` are analysis material and stay
out of the build.

<details>
<summary><strong>How the import works</strong> what each script in <code>tools/</code> does</summary>
<br>

`import-sources.sh` regenerates the eight projects and calls, in order:

- `fixups-xaml.py` rebuilds the page code-behinds and strips the members the XAML
  compiler regenerates.
- `fixups-ambiguous.py` resolves names that exist in two namespaces
  (`GestureEventArgs`, `Rect`, `Size`) with explicit aliases.
- `fixups-residual.py` handles one-off shapes: `Nullable` without an argument,
  casts to `this`, page property overrides, `out` parameters, rebuilt events.
- `fixups-il.py` handles IL artifacts: qualified protected access, enum members
  without a name, accessors called like methods, `Duration.op_Implicit`.
- `fixups-links.py` turns `HyperlinkButton.Command`, which Silverlight does not
  have, into `Click`, and drops the redundant `Windows` reference.
- `fixups-resources.py` puts the dictionary `x:Name` back to `x:Key`.
- `fixups-gfields.py` reconciles the duplicate fields in `.g.cs` (run after a
  build).

`build/WpPackagingFallback.targets` covers two pieces of the packaging chain that
ship with the standalone WP8.1 SDK: it defines the `_GetRecursiveResolvedSDKReferences`
target the WP targets reference but never declare, and it seeds
`$(WMAppManifestOutput)` from `Properties/WMAppManifest.xml`.

</details>

## Known limits

- **15 async methods** were not decompiled automatically (state machines in
  `CameraView`, `FBGeoLocator`, `SingleInputPaneHelper`). They stay as stubs that
  throw, with a pointer to `recovered/il/`.
- **Local variable names are synthetic.** The package has no PDB, so fields kept
  their real names but locals are `num`, `text`, `flag`.
- **Localization is partial**: the 60 satellite assemblies sit in `extracted/`;
  only three string libraries were turned into `.resx`.
- **No installable `.xap` yet**, so the code compiles and reads, but cannot be
  deployed to a device from here.
- The XAML designer needs these projects loaded and built once; designer errors
  about `LocalizedStrings` or `StaticResource` mean the app assembly was not
  resolved in the design-time build, not that the type is missing.

## Legal

The code belongs to Facebook/Meta. It is kept here for study and interoperability
only. The repository is private: do not publish it, do not redistribute it in
another form. `decompiled/`, `recovered/` and `localization/` are analysis
material, not code this repository holds rights to.

No license is granted. The terms of the original software apply.
