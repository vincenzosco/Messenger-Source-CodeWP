"""Riporta a C# idiomatico gli artefatti che ILSpy emette quando l'IL usa
membri protetti, operatori di conversione o enum senza nome.

Ogni regola e' meccanica e non cambia il comportamento: sono le stesse
costruzioni che il compilatore C# aveva generato nel 2015.
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


def sources():
    for base in (ROOT / "Messenger", ROOT / "src"):
        for path in base.rglob("*.cs"):
            if "obj" in path.parts or "bin" in path.parts:
                continue
            yield path


# ---------------------------------------------------------------- regole base

# CS1540: ILSpy qualifica con un cast alla classe base l'accesso a un membro
# protetto (`((Control)this).GetTemplateChild(...)`), che C# vieta. Il sorgente
# originale usava semplicemente il membro ereditato.
BASE_CAST = re.compile(r"\(\((?!object\b|string\b)[A-Za-z_][\w.]*\)this\)\.")

# `Duration.op_Implicit(ts)` / `KeyTime.op_Implicit(ts)`: l'operatore implicito
# non si puo' chiamare, ma l'assegnazione lo applica da sola.

# CS0571: chiamata esplicita della funzione di accesso (`val.put_Subject(s)`).
PUT_ACCESSOR = re.compile(r"([A-Za-z_][\w.\)\]]*)\.put_([A-Za-z_]\w*)\(([^;()]*)\);")
GET_ACCESSOR = re.compile(r"([A-Za-z_][\w.\)\]]*)\.get_([A-Za-z_]\w*)\(\)")

# CS0571: costruttore base chiamato come metodo (`base._002Ector(...)`).
CTOR_STATEMENT = re.compile(r"^\s*(?:base|\(\([A-Za-z_][\w.]*\)this\))\._002Ector\([^)]*\);\s*\r?\n", re.M)

# CS0161: il decompilatore non ha prodotto corpo, il metodo resta vuoto.
STUB_BODY = re.compile(
    r"(\)\s*\r?\n\s*\{\s*\r?\n)((?:\s*//[^\n]*\n)+)\s*\r?\n",
)

ENUM_LITERALS = {
    "(Visibility)0": "Visibility.Visible",
    "(Visibility)1": "Visibility.Collapsed",
}


def fix_base_cast(text):
    return BASE_CAST.sub("base.", text)


def fix_implicit_operator(text):
    for type_name in ("Duration", "KeyTime"):
        needle = type_name + ".op_Implicit("
        index = 0
        while True:
            found = text.find(needle, index)
            if found < 0:
                break
            start = found + len(needle)
            depth = 1
            cursor = start
            while cursor < len(text) and depth:
                if text[cursor] == "(":
                    depth += 1
                elif text[cursor] == ")":
                    depth -= 1
                cursor += 1
            inner = text[start:cursor - 1]
            text = text[:found] + inner + text[cursor:]
            index = found + len(inner)
    return text


def fix_accessors(text):
    text = PUT_ACCESSOR.sub(lambda m: m.group(1) + "." + m.group(2) + " = " + m.group(3) + ";", text)
    text = GET_ACCESSOR.sub(lambda m: m.group(1) + "." + m.group(2), text)
    return text


def fix_ctor_statements(text):
    return CTOR_STATEMENT.sub("", text)


def fix_visibility(text):
    for literal, replacement in ENUM_LITERALS.items():
        text = text.replace(literal, replacement)

    needle = "(Visibility)("
    start = 0
    while True:
        index = text.find(needle, start)
        if index < 0:
            return text
        open_paren = index + len("(Visibility)")
        depth = 0
        cursor = open_paren
        while cursor < len(text):
            if text[cursor] == "(":
                depth += 1
            elif text[cursor] == ")":
                depth -= 1
                if depth == 0:
                    break
            cursor += 1
        if cursor >= len(text):
            return text
        inner = text[open_paren + 1:cursor]
        if re.search(r"\?\s*1\s*:\s*0", inner):
            body = re.sub(r"\?\s*1\s*:\s*0", "? Visibility.Collapsed : Visibility.Visible", inner)
        else:
            body = inner + " ? Visibility.Collapsed : Visibility.Visible"
        replacement = "(" + body + ")"
        text = text[:index] + replacement + text[cursor + 1:]
        start = index + len(replacement)


def fix_stub_bodies(text):
    def replace(match):
        head, comments = match.group(1), match.group(2)
        last = comments.strip().splitlines()[-1].strip()
        return (
            head
            + "\t\t\tthrow new NotImplementedException(\"IL original body not decompiled: "
            + last.replace('"', "'")
            + "\");\r\n"
        )

    return STUB_BODY.sub(replace, text)


def fix_enum_switch(text):
    # `switch (enumValue - 3)` confronta un enum con costanti int: il IL
    # sottraeva su int, quindi il cast esplicito ripristina il comportamento.
    return re.sub(r"switch \((\w+) - (\d+)\)", r"switch ((int)\1 - \2)", text)


def fix_tilt(text):
    return re.sub(r"\((\w+(?:\.\w+)*)\.Orientation & (\d+)\) != (\d+)", r"(((int)\1.Orientation & \2) != \3)", text)


def main():
    touched = 0
    for path in sources():
        original = read(path)
        text = original
        text = fix_base_cast(text)
        text = fix_implicit_operator(text)
        text = fix_accessors(text)
        text = fix_ctor_statements(text)
        text = fix_visibility(text)
        text = fix_enum_switch(text)
        text = fix_tilt(text)
        if text != original:
            write(path, text)
            touched += 1
    print("artefatti IL corretti in " + str(touched) + " file")


if __name__ == "__main__":
    main()
