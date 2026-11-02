"""Ripulisce le classi code-behind decompilate per la ricompilazione delle pagine XAML.

Il compilatore XAML rigenera, per ogni file con x:Class, la classe *partial* con i
campi dei controlli nominati, `_contentLoaded` e `InitializeComponent()`. Nel IL
questi membri sono fusi nella classe, quindi comparendo anche nel codice
decompilato il compilatore li vede duplicati (CS0102/CS0111) e la classe non è
dichiarata `partial` (CS0260).

Questo passo, eseguito dopo l'import:
  * aggiunge `partial` alle classi con un corrispondente x:Class;
  * rimuove i campi dei controlli nominati nel XAML;
  * rimuove `_contentLoaded` e il corpo di `InitializeComponent()`.
"""

import io
import pathlib
import re

APP = pathlib.Path("Messenger")


def read(path):
    with io.open(path, "r", encoding="utf-8-sig", newline="") as fh:
        return fh.read()


def write(path, text):
    with io.open(path, "w", encoding="utf-8-sig", newline="") as fh:
        fh.write(text)


def xaml_classes():
    """Mappa nome semplice -> (nome completo, nomi dichiarati con x:Name)."""
    classes = {}
    for xaml in APP.rglob("*.xaml"):
        text = read(xaml)
        match = re.search(r'x:Class="([^"]+)"', text)
        if not match:
            continue
        # `Name=` senza prefisso crea comunque un campo nel code-behind
        names = set(re.findall(r'(?:x:)?Name="([^"]+)"', text))
        classes[match.group(1).split(".")[-1]] = (match.group(1), names)
    return classes


def strip_initialize_component(text):
    """Rimuove il metodo InitializeComponent() con un matching a graffe."""
    index = text.find("void InitializeComponent()")
    if index < 0:
        return text

    start = text.rfind("\n", 0, index) + 1
    brace = text.find("{", index)
    if brace < 0:
        return text

    # porta con sé gli attributi che decorano il metodo, altrimenti restano
    # appesi davanti alla graffa di chiusura della classe (CS1519).
    while start > 0:
        previous_end = start - 1
        previous_start = text.rfind("\n", 0, previous_end) + 1
        previous = text[previous_start:previous_end].strip()
        if previous.startswith("[") and previous.endswith("]"):
            start = previous_start
        else:
            break

    depth = 0
    position = brace
    while position < len(text):
        char = text[position]
        if char == "{":
            depth += 1
        elif char == "}":
            depth -= 1
            if depth == 0:
                end = position + 1
                newline = text.find("\n", end)
                if newline != -1:
                    end = newline + 1
                return text[:start] + text[end:]
        position += 1
    return text


def main():
    classes = xaml_classes()
    rewritten = 0

    for source in APP.rglob("*.cs"):
        text = read(source)
        original = text

        for simple, (_full, names) in classes.items():
            if not re.search(r"\bclass\s+" + re.escape(simple) + r"\b", text):
                continue

            text = re.sub(
                r"(\b(?:public|internal)\s+(?:sealed\s+)?)class\s+" + re.escape(simple) + r"\b",
                lambda m, s=simple: m.group(1) + "partial class " + s,
                text,
                count=1,
            )

            for name in names:
                text = re.sub(
                    r"(?m)^[ \t]*(?:internal|public|private|protected)[^\r\n;]{0,200}\b"
                    + re.escape(name)
                    + r"\s*;[ \t]*\r?\n",
                    "",
                    text,
                )

            text = re.sub(r"(?m)^[ \t]*private\s+bool\s+_contentLoaded\s*;[ \t]*\r?\n", "", text)
            text = strip_initialize_component(text)

        if text != original:
            write(source, text)
            rewritten += 1

    print("classi code-behind ripulite: " + str(rewritten))


if __name__ == "__main__":
    main()
