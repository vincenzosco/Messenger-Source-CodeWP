"""Risolve le ambiguita' introdotte dal decompilatore.

Quando una classe compilata riferisce due tipi omonimi da namespace diversi, ILSpy
emette entrambi i `using`, e il codice che prima compilava diventa ambiguo
(CS0104). Qui si aggiunge un alias esplicito verso il tipo che il codice WP8 usava
realmente, solo nei file che importano entrambi i namespace e usano il nome.
"""

import io
import pathlib
import re

APP = pathlib.Path("Messenger")

# nome semplice -> (namespace concorrente A, namespace concorrente B, tipo scelto)
ALIASES = {
    "GestureEventArgs": (
        "Microsoft.Phone.Controls",
        "System.Windows.Input",
        "Microsoft.Phone.Controls.GestureEventArgs",
    ),
    "Rect": ("Windows.Foundation", "System.Windows", "System.Windows.Rect"),
    "Size": ("Windows.Foundation", "System.Windows", "System.Windows.Size"),
    "NavigationEventArgs": (
        "Facebook.Messenger.WP8.Events",
        "System.Windows.Navigation",
        "Facebook.Messenger.WP8.Events.NavigationEventArgs",
    ),
    "Image": (
        "System.Windows.Controls",
        "Facebook.Services.Api.Methods.SearchImages",
        "System.Windows.Controls.Image",
    ),
    "MediaSource": (
        "Microsoft.Xna.Framework.Media",
        "Facebook.Models",
        "Microsoft.Xna.Framework.Media.MediaSource",
    ),
}


def read(path):
    with io.open(path, "r", encoding="utf-8-sig", newline="") as fh:
        return fh.read()


def write(path, text):
    with io.open(path, "w", encoding="utf-8-sig", newline="") as fh:
        fh.write(text)


def add_alias(text, symbol, target):
    if re.search(r"^using\s+" + re.escape(symbol) + r"\s*=", text, re.M):
        return text

    lines = text.split("\n")
    last_using = -1
    for index, line in enumerate(lines):
        stripped = line.strip()
        if stripped.startswith("using ") and stripped.endswith(";") and "=" not in stripped:
            last_using = index
        elif stripped.startswith("namespace ") and last_using >= 0:
            break
    if last_using < 0:
        return text

    lines.insert(last_using + 1, "using " + symbol + " = " + target + ";")
    return "\n".join(lines)


def main():
    touched = 0
    for source in APP.rglob("*.cs"):
        text = read(source)
        original = text
        for symbol, (first, second, target) in ALIASES.items():
            if "using " + first + ";" not in text or "using " + second + ";" not in text:
                continue
            if not re.search(r"\b" + re.escape(symbol) + r"\b", text):
                continue

            # Le pagine fanno override dei membri di PhoneApplicationPage e i
            # controlli di Control.OnDoubleTap: li' il tipo giusto e' quello del
            # framework, non quello dell'app.
            if symbol == "NavigationEventArgs" and re.search(r"OnNavigated(To|From)\s*\(", text):
                text = add_alias(text, symbol, "System.Windows.Navigation.NavigationEventArgs")
                continue
            if symbol == "GestureEventArgs" and re.search(r"OnDoubleTap\s*\(", text):
                text = add_alias(text, symbol, "System.Windows.Input.GestureEventArgs")
                continue

            text = add_alias(text, symbol, target)
        if text != original:
            write(source, text)
            touched += 1
    print("alias aggiunti in " + str(touched) + " file")


if __name__ == "__main__":
    main()
