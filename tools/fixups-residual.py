"""Correzioni puntuali dei residui del decompilatore.

* `Nullable` senza argomento generico: ILSpy perde il tipo quando il valore e'
  un `Nullable<T>` di un tipo WinRT; il IL originale dice
  `System.Nullable`1<Windows.Phone.Media.Capture.CameraSensorLocation>` e
  `System.Nullable`1<System.Windows.Size>`.
* cast espliciti a `this` su membri protetti della classe base
  (`((BackgroundAgent)this).NotifyComplete()`), che C# rifiuta con CS1540.
"""

import io
import pathlib
import re

ROOT = pathlib.Path(".")


def read(path):
    with io.open(path, "r", encoding="utf-8-sig", newline="") as fh:
        return fh.read()


def write(path, text):
    with io.open(path, "w", encoding="utf-8-sig", newline="") as fh:
        fh.write(text)


NULLABLE_PATCHES = [
    (
        "ViewfinderDrawer.cs",
        [
            ("private Nullable _rotationPreference;", "private CameraSensorLocation? _rotationPreference;"),
            ("public Nullable GetRotationPreference()", "public CameraSensorLocation? GetRotationPreference()"),
            ("using System.Windows;", "using System.Windows;\nusing Windows.Phone.Media.Capture;"),
        ],
    ),
    (
        "CameraView.cs",
        [
            (
                "public async void Setup(Nullable type = null)",
                "public async void Setup(CameraSensorLocation? type = null)",
            ),
        ],
    ),
    (
        "MediaViewer.cs",
        [
            ("private Nullable _size = null;", "private Size? _size = null;"),
        ],
    ),
]

TARGETED = [
    # Conversione di tipo fra le due `Size` in gioco: le risoluzioni della
    # fotocamera sono Windows.Foundation.Size, mentre l'alias del file punta a
    # System.Windows.Size.
    (
        "CameraView.cs",
        [
            (
                "private Size GetResolution(IReadOnlyList<Size> resolutions)",
                "private Windows.Foundation.Size GetResolution(IReadOnlyList<Windows.Foundation.Size> resolutions)",
            ),
            ("return new Size(0.0, 0.0);", "return new Windows.Foundation.Size(0.0, 0.0);"),
            (
                "List<Size> list = resolutions.OrderByDescending((Size s)",
                "List<Windows.Foundation.Size> list = resolutions.OrderByDescending((Windows.Foundation.Size s)",
            ),
            ("Size result = list[0];", "Windows.Foundation.Size result = list[0];"),
            ("foreach (Size item in list)", "foreach (Windows.Foundation.Size item in list)"),
            (
                "Size captureResolution = GetResolution(resolutions);",
                "Windows.Foundation.Size captureResolution = GetResolution(resolutions);",
            ),
            ("Size? size = null;", "Windows.Foundation.Size? size = null;"),
        ],
    ),
    # L'estensione vive nell'SDK: il IL la chiama in forma statica, il sorgente
    # 2015 la usava come metodo di estensione su WriteableBitmap.
    (
        "CameraView.cs",
        [("Extensions.SaveJpeg(rotatedThumbnail, ", "rotatedThumbnail.SaveJpeg(")],
    ),
    (
        "PhotoAttachmentHelper.cs",
        [("Extensions.SaveJpeg(source, ", "source.SaveJpeg(")],
    ),
    (
        "PhotoAttachmentsViewModel.cs",
        [("Extensions.SaveJpeg(writeableBitmap, ", "writeableBitmap.SaveJpeg(")],
    ),
    # Nel drawer di ricerca `Image` e' il modello di ricerca, non il controllo.
    (
        "ImageSearchDrawer.cs",
        [("using Image = System.Windows.Controls.Image;\r\n", "")],
    ),
    # Tap/gesture degli elementi: il delegato e' quello del framework.
    (
        "ImageAttachmentsControl.cs",
        [
            (
                "private void ImageOnTap(object sender, GestureEventArgs gestureEventArgs)",
                "private void ImageOnTap(object sender, System.Windows.Input.GestureEventArgs gestureEventArgs)",
            )
        ],
    ),
    (
        "PinnedThreadGridView.cs",
        [
            (
                "private void RearrangeCanvas_OnTap(object sender, GestureEventArgs e)",
                "private void RearrangeCanvas_OnTap(object sender, System.Windows.Input.GestureEventArgs e)",
            )
        ],
    ),
    (
        "ComposerBarPage.cs",
        [
            (
                "private void OnTokenTap(object sender, GestureEventArgs e)",
                "private void OnTokenTap(object sender, System.Windows.Input.GestureEventArgs e)",
            )
        ],
    ),
    # In GalleryDrawer `MediaSource` e' il tipo XNA (librerie del telefono),
    # mentre il risultato da inviare usa l'enum dell'app.
    (
        "GalleryDrawer.cs",
        [
            (
                "new ImageResult(MediaSource.Gallery",
                "new ImageResult(Facebook.Models.MediaSource.Gallery",
            )
        ],
    ),
    (
        "CameraView.cs",
        [
            (
                "IReadOnlyList<Size> resolutions = AudioVideoCaptureDevice",
                "IReadOnlyList<Windows.Foundation.Size> resolutions = AudioVideoCaptureDevice",
            )
        ],
    ),
    (
        "InputKeyboardOffsetHelper.cs",
        [("Rect occludedRect = e.OccludedRect;", "Windows.Foundation.Rect occludedRect = e.OccludedRect;")],
    ),
    # Il delegato di Tap e' quello del framework in tutti i gestori.
    (
        "PinnedThreadGridView.cs",
        [
            (
                "private void RearrangeCanvas_OnHold(object sender, GestureEventArgs e)",
                "private void RearrangeCanvas_OnHold(object sender, System.Windows.Input.GestureEventArgs e)",
            )
        ],
    ),
    (
        "MessageSendingPage.cs",
        [
            (
                "private void DetectDrawerDeselect(object sender, GestureEventArgs e)",
                "private void DetectDrawerDeselect(object sender, System.Windows.Input.GestureEventArgs e)",
            )
        ],
    ),
    # Il risultato della ricerca immagini e' il modello, non il controllo.
    (
        "ImageSearchDrawer.cs",
        [
            (
                "private void OnSearchImagesCompleted(object sender, List<Image> searchResults)",
                "private void OnSearchImagesCompleted(object sender, List<Facebook.Services.Api.Methods.SearchImages.Image> searchResults)",
            ),
            (
                "(Image result) => new Uri(result.Thumbnail.Url)",
                "(Facebook.Services.Api.Methods.SearchImages.Image result) => new Uri(result.Thumbnail.Url)",
            ),
        ],
    ),
    # L'alias serve agli override di pagina; i comandi di navigazione usano il
    # tipo dell'app.
    (
        "MessengerMainPage.cs",
        [
            (
                "new NavigationEventArgs(uri)",
                "new Facebook.Messenger.WP8.Events.NavigationEventArgs(uri)",
            ),
            (
                "new NavigationEventArgs(threadUri)",
                "new Facebook.Messenger.WP8.Events.NavigationEventArgs(threadUri)",
            ),
        ],
    ),
    (
        "MessageSendingPage.cs",
        [
            (
                "using System.Windows.Controls;",
                "using System.Windows.Controls;\r\nusing Point = System.Windows.Point;",
            )
        ],
    ),
    (
        "BoolToVisibilityConverter.cs",
        [("Nullable val = value as Nullable;", "Visibility? val = value as Visibility?;")],
    ),
    (
        "MediaPlaybackViewModel.cs",
        [("switch (currentState - 1)", "switch ((int)currentState - 1)")],
    ),
    (
        "VoiceClipDrawer.cs",
        [
            (
                "public VoiceClipDrawer()\r\n\t\t{",
                "public VoiceClipDrawer()\r\n\t\t\t: base(DrawerType.Voice)\r\n\t\t{",
            )
        ],
    ),
]

MISSING_EVENTS = {
    "ContactListUserControl.cs": [
        "public event EventHandler<NavigationEventArgs> NavigateEvent;\r\n\t\t",
        "public event EventHandler<UserSelectedEventArgs> UserSelectedEvent;\r\n\t\t",
    ],
    "RecentThreadsUserControl.cs": [
        "public event EventHandler<NavigationEventArgs> NavigateEvent;\r\n\t\t",
    ],
}

CLASS_HEADER = re.compile(r"^[ \t]*public (?:partial )?class \w+[^\n]*\r?\n[ \t]*\{\r?\n", re.M)
GESTURE_ALIAS_LINE = re.compile(r"^using GestureEventArgs = [^\n]*\r?\n", re.M)

MEDIA_SOURCE_ALIAS = "using MediaSource = Microsoft.Xna.Framework.Media.MediaSource;"
MEDIA_SOURCE_APP = "using MediaSource = Facebook.Models.MediaSource;"
APP_MEDIA_FILES = ("MessageBaseViewModel.cs", "MessageComposerViewModel.cs")

OUT_PARAM = re.compile(r'TryGetValue\(("[^"]+"), ref ')
OUT_PARAM_GENERIC = re.compile(r'(TryGetValue(?:<[^>]+>)?\()(\w+), ref ')

STUB_MARKER = "// ILSpy could not decompile this member: "

BASE_CAST = re.compile(r"\(\((BackgroundAgent|MediaStreamSource|BackgroundAudioPlayer|AudioPlayerAgent)\)this\)\.")


def apply_nullable():
    for filename, pairs in NULLABLE_PATCHES:
        for path in ROOT.joinpath("Messenger").rglob(filename):
            if "obj" in path.parts:
                continue
            text = read(path)
            original = text
            for old, new in pairs:
                if new.startswith("using ") and new in text:
                    continue
                text = text.replace(old, new)
            if text != original:
                write(path, text)
                print("nullable: " + str(path))


def apply_on_double_tap():
    # MediaViewer vive in Microsoft.Phone.Controls, dove esiste anche l'omonimo
    # GestureEventArgs del toolkit: l'override di Control.OnDoubleTap vuole
    # invece System.Windows.Input.GestureEventArgs.
    for path in ROOT.joinpath("Messenger").rglob("*.cs"):
        if "obj" in path.parts:
            continue
        text = read(path)
        if "OnDoubleTap(GestureEventArgs" in text:
            write(path, text.replace("OnDoubleTap(GestureEventArgs", "OnDoubleTap(System.Windows.Input.GestureEventArgs"))
            print("OnDoubleTap qualificato: " + path.name)


def apply_page_overrides():
    # Nel IL le pagine espongono get_PhotoPopupParent -> LayoutRoot: ILSpy ha
    # perso la proprieta' (il campo LayoutRoot e' generato dal XAML), quindi
    # MessageSendingPage resta astratta e le due pagine non compilano (CS0534).
    marker = "protected override ImageUploadAttachmentControl ImageUploadControl => MessageComposerBubble.UploadBar;"
    addition = "\r\n\r\n\t\tprotected override Grid PhotoPopupParent => LayoutRoot;"
    for path in ROOT.joinpath("Messenger/Facebook/Messenger/WP8/Pages").glob("*.cs"):
        text = read(path)
        if marker in text and "PhotoPopupParent" not in text:
            write(path, text.replace(marker, marker + addition, 1))
            print("override PhotoPopupParent: " + path.name)


def apply_targeted():
    for filename, pairs in TARGETED:
        for path in ROOT.joinpath("Messenger").rglob(filename):
            if "obj" in path.parts:
                continue
            text = read(path)
            original = text
            for old, new in pairs:
                text = text.replace(old, new)
            if text != original:
                write(path, text)


def apply_missing_events():
    for filename, declarations in MISSING_EVENTS.items():
        for path in ROOT.joinpath("Messenger").rglob(filename):
            if "obj" in path.parts:
                continue
            text = read(path)
            anchor = re.search(r"^\s*public event ", text, re.M)
            position = anchor.start() if anchor is not None else None
            if position is None:
                header = CLASS_HEADER.search(text)
                if header is None:
                    print("eventi: nessun ancoraggio in " + path.name)
                    continue
                position = header.end()
            missing = [d for d in declarations if d.strip() not in text]
            if not missing:
                continue
            block = "".join(d.replace("\r\n", "\n") + "\n" for d in missing)
            text = text[:position] + block + text[position:]
            write(path, text)
            print("eventi aggiunti: " + path.name)


def apply_gesture_alias():
    # In MessageSendingPage il delegato di `UIElement.TapEvent` e' quello del
    # framework: l'alias verso il toolkit rendeva il gestore incompatibile con
    # l'evento e con DrawerContainer.GetRelativePoint.
    for path in ROOT.joinpath("Messenger").rglob("MessageSendingPage.cs"):
        if "obj" in path.parts:
            continue
        text = read(path)
        if "using GestureEventArgs = " not in text:
            continue
        text = GESTURE_ALIAS_LINE.sub("using GestureEventArgs = System.Windows.Input.GestureEventArgs;\n", text, count=1)
        write(path, text)
        print("alias GestureEventArgs rimosso: " + path.name)


def apply_media_source_alias():
    for filename in APP_MEDIA_FILES:
        for path in ROOT.joinpath("Messenger").rglob(filename):
            if "obj" in path.parts:
                continue
            text = read(path)
            if MEDIA_SOURCE_ALIAS in text:
                write(path, text.replace(MEDIA_SOURCE_ALIAS, MEDIA_SOURCE_APP))
                print("alias MediaSource -> app: " + path.name)


def apply_socket_protection():
    # `(SocketProtectionLevel)(!UseUnsecureConnection)` nel IL e' un cast da
    # bool a enum: 0 = PlainSocket, 1 = Ssl.
    old = "SocketProtectionLevel socketProtection = (SocketProtectionLevel)(!Parameters.UseUnsecureConnection);"
    new = ("SocketProtectionLevel socketProtection = Parameters.UseUnsecureConnection"
           " ? SocketProtectionLevel.PlainSocket : SocketProtectionLevel.Ssl;")
    for base in (ROOT / "Messenger", ROOT / "src"):
        for path in base.rglob("MqttClient.cs"):
            if "obj" in path.parts:
                continue
            text = read(path)
            if old in text:
                write(path, text.replace(old, new))
                print("SocketProtectionLevel: " + path.name)


def apply_out_parameters():
    for base in (ROOT / "Messenger", ROOT / "src"):
        for path in base.rglob("*.cs"):
            if "obj" in path.parts:
                continue
            text = read(path)
            updated = OUT_PARAM.sub(lambda m: "TryGetValue(" + m.group(1) + ", out ", text)
            updated = OUT_PARAM_GENERIC.sub(lambda m: m.group(1) + m.group(2) + ", out ", updated)
            if updated != text:
                write(path, updated)

def apply_stub_bodies():
    # I metodi che ILSpy non ha decompilato restano senza corpo: un throw
    # esplicito li rende compilabili senza inventare comportamento.
    touched = 0
    for path in ROOT.joinpath("Messenger").rglob("*.cs"):
        if "obj" in path.parts:
            continue
        text = read(path)
        index = 0
        while True:
            marker = text.find(STUB_MARKER, index)
            if marker < 0:
                break
            brace = text.rfind("{", 0, marker)
            if brace < 0 or text[brace + 1:marker].strip():
                index = marker + 1
                continue
            insertion = (
                "\r\n\t\t\tthrow new System.NotImplementedException(\"ILSpy could not decompile this "
                "member; the original IL is under recovered/il.\");\r\n"
            )
            text = text[: brace + 1] + insertion + text[brace + 1:]
            index = brace + len(insertion)
            touched += 1
        if touched:
            write(path, text)
            touched = 0


def apply_base_casts():
    touched = 0
    for path in ROOT.joinpath("src").rglob("*.cs"):
        text = read(path)
        updated = BASE_CAST.sub("", text)
        if updated != text:
            write(path, updated)
            touched += 1
    print("cast a this rimossi in " + str(touched) + " file")


if __name__ == "__main__":
    apply_nullable()
    apply_on_double_tap()
    apply_page_overrides()
    apply_targeted()
    apply_missing_events()
    apply_media_source_alias()
    apply_gesture_alias()
    apply_socket_protection()
    apply_out_parameters()
    apply_stub_bodies()
    apply_base_casts()
