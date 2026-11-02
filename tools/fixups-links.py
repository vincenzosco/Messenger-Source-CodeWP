"""Riporta a Silverlight due costrutti che il designer segnala.

* `HyperlinkButton.Command` non esiste in Silverlight: il comando del view model
  va eseguito sul Click, che e' quello che l'app faceva.
* il riferimento esplicito `Windows` resta irrisolto (MSB3245) perche' i tipi
  WinRT arrivano gia' dalle reference assembly del target framework.
"""

import io
import pathlib
import re

ROOT = pathlib.Path(".")
APP = ROOT / "Messenger"

HYPERLINKS = [
    (
        "loginapprovalpage.xaml",
        'Command="{Binding ResendApprovalCodeCommand}"',
        'Click="ResendApprovalCode_OnClick"',
    ),
    (
        "verifyphonenumberpage.xaml",
        'Command="{Binding SendCodeAgainCommand}"',
        'Click="SendCodeAgain_OnClick"',
    ),
]

HANDLERS = [
    ("LoginApprovalPage.cs", "LoginViewModel", "ResendApprovalCode_OnClick", "ResendApprovalCodeCommand"),
    ("VerifyPhoneNumberPage.cs", "VerifyPhoneNumberViewModel", "SendCodeAgain_OnClick", "SendCodeAgainCommand"),
]


def read(path):
    with io.open(path, "r", encoding="utf-8-sig", newline="") as fh:
        return fh.read()


def write(path, text):
    with io.open(path, "w", encoding="utf-8-sig", newline="") as fh:
        fh.write(text)


def handler_block(view_model, handler, command):
    return (
        "\n\t\tprivate void " + handler + "(object sender, RoutedEventArgs e)\n"
        "\t\t{\n"
        "\t\t\tWithViewModel((" + view_model + " vm) =>\n"
        "\t\t\t{\n"
        "\t\t\t\tif (vm." + command + ".CanExecute(null))\n"
        "\t\t\t\t{\n"
        "\t\t\t\t\tvm." + command + ".Execute(null);\n"
        "\t\t\t\t}\n"
        "\t\t\t});\n"
        "\t\t}\n"
    )


def insert_member(text, block):
    """Inserisce il membro prima della graffa che chiude la classe.

    Il decompilatore chiude la classe con `\t}` e il namespace con `}` alla
    colonna zero: la graffa da cercare e' la penultima, non l'ultima.
    """
    trimmed = text.rstrip()
    match = re.search(r"\n(\t*)\}\s*\n\}\s*$", trimmed)
    pos = match.start() + 1 if match else trimmed.rfind("\n}") + 1
    return trimmed[:pos] + block + trimmed[pos:] + "\r\n"


def main():
    for filename, old, new in HYPERLINKS:
        for path in APP.rglob(filename):
            if "obj" in path.parts:
                continue
            text = read(path)
            if old in text:
                write(path, text.replace(old, new))
                print("hyperlink click: " + path.name)

    for filename, view_model, handler, command in HANDLERS:
        for path in APP.rglob(filename):
            if "obj" in path.parts:
                continue
            text = read(path)
            if handler in text:
                continue
            write(path, insert_member(text, handler_block(view_model, handler, command)))
            print("handler aggiunto: " + path.name)

    for base in (APP, ROOT / "src"):
        for path in base.rglob("*.csproj"):
            text = read(path)
            updated = re.sub(r'[ \t]*<Reference Include="Windows" />\r?\n', "", text)
            if updated != text:
                write(path, updated)
                print("riferimento Windows rimosso: " + path.name)


if __name__ == "__main__":
    main()
