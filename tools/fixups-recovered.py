"""Sostituisce gli stub del decompilatore con le ricostruzioni dal IL.

ILSpy 11.2.0.10050 non traduce i metodi che passano un gruppo di metodi a
`WindowsRuntimeMarshal.AddEventHandler` (difetto del Disambiguator, non del
codice). `recovered/` contiene la traduzione a mano; questo passo la reinserisce
nei file generati, quindi si riapplica a ogni import e `decompiled/` resta
intatto.
"""

import io
import pathlib
import re

APP = pathlib.Path("Messenger")

# (dichiarazione esatta nel file generato, corpo ricostruito)
PATCHES = {
    "Facebook.Messenger.WP8.Utilities/SingleInputPaneHelper.cs": [
        (
            "public void SubscribeToKeyboardEvents(InputPaneShowingHandler showingHandler, InputPaneHidingHandler hidingHandler)",
            "\t\t\tInputPane inputPane = InputPane.GetForCurrentView();\n"
            "\t\t\tinputPane.Showing += ShowingHandler;\n"
            "\t\t\tinputPane.Hiding += HidingHandler;\n"
            "\t\t\tAddShowingHandler(showingHandler);\n"
            "\t\t\tAddHidingHandler(hidingHandler);",
        ),
        (
            "public void UnsubscribeFromKeyboardEvents()",
            "\t\t\tInputPane inputPane = InputPane.GetForCurrentView();\n"
            "\t\t\tinputPane.Showing -= ShowingHandler;\n"
            "\t\t\tinputPane.Hiding -= HidingHandler;\n"
            "\t\t\t_inputPaneShowingHandler = null;\n"
            "\t\t\t_inputPaneHidingHandler = null;",
        ),
    ],
}


def read(path):
    with io.open(path, "r", encoding="utf-8-sig", newline="") as fh:
        return fh.read()


def write(path, text):
    with io.open(path, "w", encoding="utf-8-sig", newline="") as fh:
        fh.write(text)


def replace_stub(text, declaration, body):
    """Sostituisce il corpo dello stub che segue `declaration`."""
    pattern = re.compile(
        re.escape(declaration) + r"\s*\{(?P<stub>.*?)\n(?P<indent>[ \t]*)\}",
        re.S,
    )
    match = pattern.search(text)
    if not match or "NotImplementedException" not in match.group("stub"):
        return text, False
    replacement = declaration + "\n\t\t{\n" + body + "\n\t\t}"
    return text[: match.start()] + replacement + text[match.end():], True


def main():
    rewired = 0
    for relative, patches in PATCHES.items():
        path = APP / relative
        if not path.exists():
            continue
        text = read(path)
        for declaration, body in patches:
            text, changed = replace_stub(text, declaration, body)
            if changed:
                rewired += 1
                print("stub sostituito: " + declaration.split("(")[0])
        write(path, text)

    remaining = 0
    for source in APP.rglob("*.cs"):
        if "obj" in source.parts:
            continue
        remaining += read(source).count("NotImplementedException(\"ILSpy could not decompile")
    print("metodi riscritti: %d, stub rimasti: %d" % (rewired, remaining))


if __name__ == "__main__":
    main()
