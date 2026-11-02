"""Ripristina le chiavi dei dizionari di risorse.

L'estrazione dal BAML ha talvolta reso `x:Key` come `x:Name`: un `x:Name` in un
ResourceDictionary non registra una chiave, quindi ogni `{StaticResource ...}`
che punta a quella voce fallisce (ed e' la raffica di errori che il designer
XAML mostra). Qui, per ogni nome usato come StaticResource ma non definito come
chiave, si converte la voce `x:Name` corrispondente in `x:Key`.
"""

import io
import pathlib
import re

ROOT = pathlib.Path("Messenger")

# Risorse che arrivano dai temi di sistema (ThemeResources), non dagli XAML
# dell'app: non devono comparire nei dizionari.
SYSTEM_PREFIXES = ("Phone", "Zoom", "TransparentBrush", "Default", "System")

KEY_ATTR = re.compile(r"""x:Key\s*=\s*["']([^"']+)["']""")
NAME_ATTR = re.compile(r"""x:Name\s*=\s*["']([^"']+)["']""")
STATIC_RESOURCE = re.compile(r"StaticResource\s+([A-Za-z0-9_.]+)")

# Converter presenti come classe ma senza voce nel dizionario.
CONVERTERS = {
    "BoolToVisibilityConverter": "BoolToVisibilityConverter",
    "VisibilityToBooleanConverter": "VisibilityToBooleanConverter",
    "AlternateColorConverter": "AlternateColorConverter",
}


def read(path):
    with io.open(path, "r", encoding="utf-8-sig", newline="") as fh:
        return fh.read()


def write(path, text):
    with io.open(path, "w", encoding="utf-8-sig", newline="") as fh:
        fh.write(text)


def main():
    files = [p for p in ROOT.rglob("*.xaml") if "obj" not in p.parts]
    texts = {p: read(p) for p in files}

    defined = set()
    for text in texts.values():
        defined |= set(KEY_ATTR.findall(text))

    used = set()
    for text in texts.values():
        used |= set(STATIC_RESOURCE.findall(text))

    missing = {
        name
        for name in used - defined
        if not name.startswith(SYSTEM_PREFIXES)
    }

    converted = 0
    for path, text in texts.items():
        updated = text
        for name in missing:
            for quote in ('"', "'"):
                attribute = "x:Name=" + quote + name + quote
                if attribute in updated:
                    updated = updated.replace(attribute, "x:Key=" + quote + name + quote, 1)
                    converted += 1
        if updated != text:
            write(path, updated)
            texts[path] = updated

    defined = set()
    for text in texts.values():
        defined |= set(KEY_ATTR.findall(text))
    still_missing = {
        name for name in used - defined if not name.startswith(SYSTEM_PREFIXES)
    }

    # Converter usati dal XAML ma assenti dal dizionario.
    dictionary = ROOT / "converters" / "converters.xaml"
    if dictionary.exists() and still_missing:
        text = read(dictionary)
        additions = []
        for name in sorted(still_missing & set(CONVERTERS)):
            if 'x:Key="' + name + '"' not in text and "x:Key =\"" + name + '"' not in text:
                additions.append(
                    '\t<converters:' + CONVERTERS[name] + ' x:Key="' + name + '" />\n'
                )
        if additions:
            anchor = text.rfind("</ResourceDictionary>")
            if anchor > 0:
                text = text[:anchor] + "".join(additions) + text[anchor:]
                write(dictionary, text)
                for name in sorted(still_missing & set(CONVERTERS)):
                    still_missing.discard(name)
                print("converter aggiunti: " + ", ".join(sorted(set(CONVERTERS) & (used - defined)))[:120])

    print("x:Name -> x:Key: " + str(converted) + "; chiavi ancora assenti: " + str(len(still_missing)))
    for name in sorted(still_missing):
        print("   " + name)


if __name__ == "__main__":
    main()
