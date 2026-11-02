"""Riconcilia i campi rigenerati dal compilatore XAML.

Dopo la prima build, `obj/Debug/**/*.g.cs` contiene i campi dei controlli
nominati nel XAML. Il codice decompilato porta gli stessi campi (nel IL erano
fusi nella classe), quindi il compilatore li vede duplicati (CS0102).

Il passo legge i nomi campo dai .g.cs e rimuove le dichiarazioni corrispondenti
dal code-behind: il campo torna a essere generato dal XAML, come nell'originale.
"""

import io
import pathlib
import re

APP = pathlib.Path("Messenger")
FIELD = re.compile(r"(?m)^[ \t]*internal\s+[\w\.<>,\[\]\s]+\s+(\w+)\s*;")


def read(path):
    with io.open(path, "r", encoding="utf-8-sig", newline="") as fh:
        return fh.read()


def write(path, text):
    with io.open(path, "w", encoding="utf-8-sig", newline="") as fh:
        fh.write(text)


def main():
    generated = {}
    for g in APP.rglob("*.g.cs"):
        text = read(g)
        match = re.search(r"partial class (\w+)", text)
        if not match:
            continue
        generated.setdefault(match.group(1), set()).update(FIELD.findall(text))

    if not generated:
        print("nessun .g.cs: eseguire una build prima della riconciliazione")
        return

    touched = 0
    for source in APP.rglob("*.cs"):
        if "obj" in source.parts:
            continue
        text = read(source)
        original = text
        for name, fields in generated.items():
            if not re.search(r"\bclass\s+" + re.escape(name) + r"\b", text):
                continue
            for field in fields:
                text = re.sub(
                    r"(?m)^[ \t]*internal\s+[\w\.<>,\[\]\s]+\s+" + re.escape(field) + r"\s*;[ \t]*\r?\n",
                    "",
                    text,
                )
        if text != original:
            write(source, text)
            touched += 1
    print("code-behind riconciliati: " + str(touched))


if __name__ == "__main__":
    main()
