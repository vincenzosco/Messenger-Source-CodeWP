#!/usr/bin/env bash
# Importa il codice decompilato dentro il progetto, ricostruendo le 8 assembly
# originali come progetti WP8.1 Silverlight. Idempotente: rilanciarlo riallinea
# le cartelle sorgente ai file in decompiled/.
set -euo pipefail
cd "$(dirname "$0")/.."
ROOT=$(pwd)
DEC="$ROOT/decompiled"
LIB="$ROOT/lib"

APP="Facebook.Messenger.WP8"
APP_DIR="Messenger"
LIBS=(Facebook-Base Facebook-Models Facebook-Services Facebook-Messenger-WP8-Base
      Facebook-Messenger-WP8-Agents Facebook-Messenger-Localization ThriftPortable)

guid_for() {
  echo -n "$1" | md5sum | cut -c1-32 |
    sed -E 's/(.{8})(.{4})(.{4})(.{4})(.{12})/{\U\1-\2-\3-\4-\5}/'
}

first_party() {
  local n="$1"
  [ "$n" = "$APP" ] && return 0
  for l in "${LIBS[@]}"; do [ "$l" = "$n" ] && return 0; done
  return 1
}

copy_assembly() {
  local asm="$1" dest="$2"
  mkdir -p "$dest"
  cp -r "$DEC/$asm/." "$dest/"
  rm -f "$dest"/*.csproj
}

# --- librerie di terze parti -------------------------------------------------
mkdir -p "$LIB"
for dll in "$ROOT"/extracted/*.dll; do
  name=$(basename "$dll")
  case "$name" in
    "$APP".dll|Facebook-Base.dll|Facebook-Models.dll|Facebook-Services.dll|\
    Facebook-Messenger-WP8-Base.dll|Facebook-Messenger-WP8-Agents.dll|\
    Facebook-Messenger-Localization.dll|ThriftPortable.dll|Facebook-Base-RT.dll)
      continue ;;
  esac
  case "$name" in Facebook*|ThriftPortable*) continue;; esac  # le assembly di prima parte sono progetti
  cp -n "$dll" "$LIB/$name" 2>/dev/null || true
done
for component in "$ROOT"/extracted/*.winmd; do
  [ -f "$component" ] && cp -f "$component" "$LIB/"
done

# --- applicazione ------------------------------------------------------------
rm -rf "$APP_DIR"/App.xaml "$APP_DIR"/App.xaml.cs "$APP_DIR"/MainPage.xaml "$APP_DIR"/MainPage.xaml.cs \
       "$APP_DIR"/Assets "$APP_DIR"/Properties/AssemblyInfo.cs "$APP_DIR"/Package.appxmanifest
copy_assembly "$APP" "$APP_DIR"
[ -f "$APP_DIR/app.xaml" ] && mv "$APP_DIR/app.xaml" "$APP_DIR/App.xaml"
mkdir -p "$APP_DIR/Assets"
cp -f "$ROOT"/extracted/Assets/*.png "$APP_DIR/Assets/" 2>/dev/null || true
cp -f "$ROOT"/extracted/SplashScreenImage.*.jpg "$APP_DIR/Assets/" 2>/dev/null || true
  cp -n "$ROOT"/extracted/AppxManifest.xml "$APP_DIR/Package.appxmanifest" 2>/dev/null || true
  cp -n "$ROOT"/extracted/AppManifest.xaml "$APP_DIR/AppManifest.xaml" 2>/dev/null || true
# --- sorgenti delle altre assembly ------------------------------------------
for asm in "${LIBS[@]}"; do
  copy_assembly "$asm" "src/$asm"
done

# --- manifest applicativo ricostruito ---------------------------------------
cat > "$APP_DIR/Properties/WMAppManifest.xml" <<'XML'
<?xml version="1.0" encoding="utf-8"?>
<!-- Ricostruito da AppManifest.xaml + AppxManifest.xml del pacchetto: il
     WMAppManifest originale non è distribuito nel XAP, viene generato in build. -->
<Deployment xmlns="http://schemas.microsoft.com/windowsphone/2014/deployment" AppPlatformVersion="8.1">
  <DefaultLanguage xmlns="" code="en-US" />
  <App xmlns="" ProductID="{317180b0-bb48-6110-0000-000000000000}"
       Title="Messenger" RuntimeType="Silverlight" Version="11.0.1.0"
       Genre="apps.normal" Author="Facebook" Description="Messenger"
       Publisher="Facebook" PublisherID="{6e08453f-9ba7-4311-999c-d22fba2fb1b8}">
    <IconPath IsRelative="true" IsResource="false">Assets\ApplicationIcon.png</IconPath>
    <Capabilities>
      <Capability Name="ID_CAP_NETWORKING" />
      <Capability Name="ID_CAP_IDENTITY_DEVICE" />
      <Capability Name="ID_CAP_MEDIALIB_PHOTO" />
      <Capability Name="ID_CAP_MICROPHONE" />
      <Capability Name="ID_CAP_ISV_CAMERA" />
      <Capability Name="ID_CAP_LOCATION" />
      <Capability Name="ID_CAP_CONTACTS" />
      <Capability Name="ID_CAP_PHONEDIALER" />
      <Capability Name="ID_CAP_MEDIALIB_AUDIO" />
      <Capability Name="ID_CAP_MEDIALIB_PLAYBACK" />
      <Capability Name="ID_CAP_SPEECH_RECOGNITION" />
      <Capability Name="ID_CAP_SENSORS" />
      <Capability Name="ID_CAP_WEBBROWSERCOMPONENT" />
    </Capabilities>
    <Tasks>
      <DefaultTask Name="_default" NavigationPage="pages/loginpage.xaml" />
    </Tasks>
    <Tokens>
      <PrimaryToken TokenID="FacebookMessengerToken" TaskName="_default">
        <TemplateFlip>
          <SmallImageURI IsRelative="true" IsResource="false">Assets\Logo.png</SmallImageURI>
          <Count>0</Count>
          <BackgroundImageURI IsRelative="true" IsResource="false">Assets\Logo.png</BackgroundImageURI>
          <Title>Messenger</Title>
        </TemplateFlip>
      </PrimaryToken>
    </Tokens>
  </App>
</Deployment>
XML

# --- generazione csproj ------------------------------------------------------
emit_references() {
  local asm="$1" prefix="$2"
  local src="$DEC/$asm/$asm.csproj"
  local refs=""
  while IFS= read -r name; do
    [ -z "$name" ] && continue
    if first_party "$name"; then
      local dir="src/$name" proj="src/$name/$name.csproj"
      [ "$name" = "$APP" ] && dir="$APP_DIR" && proj="$APP_DIR/$APP_DIR.csproj"
      refs+="    <ProjectReference Include=\"$prefix$proj\">
      <Project>$(guid_for "$name")</Project>
      <Name>$name</Name>
    </ProjectReference>
"
    else
      if [ -f "$LIB/$name.dll" ]; then
        refs+="    <Reference Include=\"$name\">
      <HintPath>${prefix}lib\\$name.dll</HintPath>
    </Reference>
"
      elif [ -f "$LIB/$name.winmd" ]; then
        refs+="    <Reference Include=\"$name\">
      <HintPath>${prefix}lib\\$name.winmd</HintPath>
    </Reference>
"
      else
        refs+="    <Reference Include=\"$name\" />
"
      fi
    fi
  done < <(grep -oE '<Reference Include="[^"]+"' "$src" 2>/dev/null | sed 's/.*Include="//; s/"$//' | sort -u)
  printf '%s' "$refs"
}

emit_resources() {
  local asm="$1" dir="$2"
  find "$dir" -maxdepth 2 -name "*.resx" -print0 2>/dev/null | while IFS= read -r -d '' resx; do
    local rel logical base
    rel="${resx#"$dir"/}"
    base=$(basename "$resx" .resx)
    logical="${base}.resources"
    printf '    <EmbeddedResource Include="%s">\n      <LogicalName>%s</LogicalName>\n    </EmbeddedResource>\n' \
      "$(echo "$rel" | tr '/' '\\')" "$logical"
  done
  find "$dir" -maxdepth 2 -name "*.resx" -print0 2>/dev/null | while IFS= read -r -d '' resx; do
    local designer="${resx%.resx}.Designer.cs"
    if [ -f "$designer" ]; then
      printf '    <Compile Include="%s">\n      <DependentUpon>%s</DependentUpon>\n    </Compile>\n' \
        "$(echo "${designer#"$dir"/}" | tr '/' '\\')" "$(basename "$resx")"
    fi
  done || true
  return 0
}

emit_csproj() {
  local asm="$1" ns="$2" dir="$3" csproj="$4" is_app="$5"
  local guid refs resx xaml prefix
  guid=$(guid_for "$asm")
  if [ "$is_app" = "yes" ]; then prefix='..\\'; else prefix='..\\..\\'; fi
  refs=$(emit_references "$asm" "$prefix")
  resx=$(emit_resources "$asm" "$dir")

  cat > "$csproj" <<XML
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <ProjectGuid>$guid</ProjectGuid>
    <ProjectTypeGuids>{C089C8C0-30E0-4E22-80C0-CE093F111A43};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>$ns</RootNamespace>
    <AssemblyName>$asm</AssemblyName>
    <TargetFrameworkIdentifier>WindowsPhone</TargetFrameworkIdentifier>
    <TargetFrameworkVersion>v8.1</TargetFrameworkVersion>
    <SilverlightVersion>v8.1</SilverlightVersion>
    <LangVersion>5</LangVersion>
    <FileAlignment>512</FileAlignment>
    <MinimumVisualStudioVersion>12</MinimumVisualStudioVersion>
    <NoWarn>1591</NoWarn>
  </PropertyGroup>
  <PropertyGroup Condition=" '\$(Configuration)|\$(Platform)' == 'Debug|AnyCPU' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\\Debug\\</OutputPath>
    <DefineConstants>DEBUG;TRACE;SILVERLIGHT;WINDOWS_PHONE;WINDOWS_PHONE8_1</DefineConstants>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '\$(Configuration)|\$(Platform)' == 'Release|AnyCPU' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\\Release\\</OutputPath>
    <DefineConstants>TRACE;SILVERLIGHT;WINDOWS_PHONE;WINDOWS_PHONE8_1</DefineConstants>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
$refs  </ItemGroup>
  <ItemGroup>
    <Compile Include="**\\*.cs" Exclude="bin\\**;obj\\**" />
$resx  </ItemGroup>
XML

  if [ "$is_app" = "yes" ]; then
    cat >> "$csproj" <<'XML'
  <ItemGroup>
    <ApplicationDefinition Include="App.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </ApplicationDefinition>
    <Page Include="**\*.xaml" Exclude="App.xaml;bin\**;obj\**">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
  </ItemGroup>
  <ItemGroup>
    <Content Include="Assets\**\*" />
    <Content Include="AppManifest.xaml" />
  </ItemGroup>
XML
  fi

  cat >> "$csproj" <<'XML'
  <Import Project="$(MSBuildExtensionsPath)\Microsoft\WindowsPhone\v8.1\Microsoft.WindowsPhone.CSharp.targets" />
</Project>
XML
}

emit_csproj "$APP" "Facebook.Messenger.WP8" "$APP_DIR" "$APP_DIR/Messenger.csproj" "yes"
for asm in "${LIBS[@]}"; do
  emit_csproj "$asm" "$(echo "$asm" | tr '-' '_')" "src/$asm" "src/$asm/$asm.csproj" "no"
done

# --- soluzione ---------------------------------------------------------------
{
  echo ""
  echo "Microsoft Visual Studio Solution File, Format Version 12.00"
  echo "# Visual Studio 14"
  echo "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"$APP\", \"$APP_DIR\\\\Messenger.csproj\", \"$(guid_for "$APP")\""
  echo "EndProject"
  for asm in "${LIBS[@]}"; do
    echo "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"$asm\", \"src\\\\$asm\\\\$asm.csproj\", \"$(guid_for "$asm")\""
    echo "EndProject"
  done
  echo "Global"
  echo "	GlobalSection(SolutionConfigurationPlatforms) = preSolution"
  echo "		Debug|Any CPU = Debug|Any CPU"
  echo "		Release|Any CPU = Release|Any CPU"
  echo "	EndGlobalSection"
  echo "	GlobalSection(ProjectConfigurationPlatforms) = postSolution"
  for asm in "$APP" "${LIBS[@]}"; do
    g=$(guid_for "$asm")
    echo "		$g.Debug|Any CPU.ActiveCfg = Debug|Any CPU"
    echo "		$g.Debug|Any CPU.Build.0 = Debug|Any CPU"
    echo "		$g.Release|Any CPU.ActiveCfg = Release|Any CPU"
    echo "		$g.Release|Any CPU.Build.0 = Release|Any CPU"
  done
  echo "	EndGlobalSection"
  echo "	GlobalSection(SolutionProperties) = preSolution"
  echo "		HideSolutionNode = FALSE"
  echo "	EndGlobalSection"
  echo "EndGlobal"
} > Messenger.sln

# --- correzioni degli artefatti del decompilatore ---------------------------
# Pochi costrutti che ILSpy emette in forma non compilabile (setter WinRT
# espliciti, chiamata al costruttore base, parametro out passato per ref).
# Restano qui, non in decompiled/, così la rigenerazione li riapplica sempre.
apply_fixups() {
  python - <<'PY'
import io
import pathlib

fixes = {
    "src/Facebook-Messenger-WP8-Base/Facebook/Messenger/Mqtt/MqttClient.cs": [
        ("socket.Control.put_KeepAlive(false);", "socket.Control.KeepAlive = false;"),
        ("socket.Control.put_NoDelay(true);", "socket.Control.NoDelay = true;"),
        ("SocketProtectionLevel socketProtection = (SocketProtectionLevel)(!Parameters.UseUnsecureConnection);",
         "SocketProtectionLevel socketProtection = Parameters.UseUnsecureConnection\n\t\t\t\t? SocketProtectionLevel.PlainSocket\n\t\t\t\t: SocketProtectionLevel.Ssl;"),
    ],
    "src/Facebook-Messenger-WP8-Base/Facebook/Messenger/VoiceRecordingHelper.cs": [
        ("\t\t\tbase._002Ector();\r\n", ""),
        ("\t\t\tbase._002Ector();\n", ""),
    ],
    "src/Facebook-Messenger-WP8-Base/Facebook/Messenger/Settings/AppSettingsManager.cs": [
        ("TryGetValue<object>(key, ref value)", "TryGetValue<object>(key, out value)"),
    ],
}

for path, pairs in fixes.items():
    p = pathlib.Path(path)
    if not p.exists():
        continue
    with io.open(p, "r", encoding="utf-8-sig", newline="") as fh:
        text = fh.read()
    original = text
    for old, new in pairs:
        text = text.replace(old, new)
    if text != original:
        with io.open(p, "w", encoding="utf-8-sig", newline="") as fh:
            fh.write(text)
        print("fixup applicato: " + path)
PY
}

apply_fixups
python tools/fixups-xaml.py
python tools/fixups-ambiguous.py
python tools/fixups-residual.py
python tools/fixups-links.py
python tools/fixups-il.py
python tools/fixups-resources.py

echo "import completato: app in $APP_DIR/, librerie in src/, dll in lib/"
