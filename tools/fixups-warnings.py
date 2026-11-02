"""Toglie i warning che sono difetti reali, senza sopprimerli.

* CS0105: `using` ripetuto dallo stesso namespace.
* CS0108: un evento che nasconde un membro della classe base senza `new`.

I warning che restano sono soppressi nel csproj, non qui: vedi il commento
accanto a `NoWarn` in `emit_csproj`.
"""

import io
import pathlib
import re

ROOT = pathlib.Path(".")
TARGETS = [ROOT / "Messenger", ROOT / "src"]


def read(path):
    with io.open(path, "r", encoding="utf-8-sig", newline="") as fh:
        return fh.read()


def write(path, text):
    with io.open(path, "w", encoding="utf-8-sig", newline="") as fh:
        fh.write(text)


def dedupe_usings(text):
    """Tiene la prima occorrenza di ogni `using`, scarta le ripetizioni."""
    seen = set()
    out = []
    for line in text.splitlines():
        candidate = " ".join(line.split())
        if candidate.startswith("using ") and candidate.endswith(";"):
            name = candidate[len("using "):-1].strip()
            if "=" not in name:
                if name in seen:
                    continue
                seen.add(name)
        out.append(line)
    return chr(10).join(out) + chr(10)


def add_new_keyword(text):
    changed = 0
    for event in ("GotFocus", "LostFocus"):
        pattern = re.compile(r"(?m)^([ \t]*)public event (EventHandler|RoutedEventHandler|EventHandler<[\w\.]+>) " + event + r"\b")
        text, n = pattern.subn(lambda m: "%spublic new event %s %s" % (m.group(1), m.group(2), event), text)
        changed += n
    return text, changed


def main():
    usings = events = 0
    for base in TARGETS:
        for path in base.rglob("*.cs"):
            if "obj" in path.parts or "bin" in path.parts:
                continue
            original = read(path)
            text = dedupe_usings(original)
            if text != original:
                usings += 1
            text, n = add_new_keyword(text)
            events += n
            if text != original:
                write(path, text)
    print("using ripetuti rimossi: %d file, 'new' aggiunti: %d" % (usings, events))


if __name__ == "__main__":
    main()
