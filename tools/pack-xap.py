"""Impagina un XAP dalla cartella di build, senza la toolchain Windows Phone.

Il pacchetto e' un archivio ZIP: AppManifest.xaml e WMAppManifest.xml alla radice,
le assembly accanto, le risorse nelle loro cartelle. L'AppManifest viene generato
dalle DLL realmente incluse, non da un elenco fisso.

Tre cose le fa il packaging WP8.1 e qui vanno fatte a mano: copiare Assets/,
portare dentro le DLL dei progetti referenziati (l'agent finisce nel suo bin, non
in quello dell'app) e scrivere il manifest.
"""

import glob
import os
import zipfile

BIN = os.path.join("Messenger", "bin", "Debug")
OUT = os.path.join(BIN, "Messenger.xap")
MANIFEST = os.path.join("Messenger", "Properties", "WMAppManifest.xml")
ASSETS = os.path.join("Messenger", "Assets")

APP_MANIFEST = """<Deployment xmlns="http://schemas.microsoft.com/client/2007/deployment" \
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" EntryPointAssembly="Facebook.Messenger.WP8" \
EntryPointType="Facebook.Messenger.WP8.App" RuntimeVersion="6.7.50308.0">
  <Deployment.Parts>
{parts}  </Deployment.Parts>
</Deployment>
"""


def read(path):
    with open(path, "rb") as fh:
        return fh.read()


def build_manifest(assemblies):
    rows = "".join(
        '    <AssemblyPart x:Name="%s" Source="%s" />\n' % (os.path.splitext(a)[0], a)
        for a in assemblies
    )
    return APP_MANIFEST.format(parts=rows).encode("utf-8")


def application_dlls():
    """DLL dell'app piu' quelle dei progetti referenziati."""
    found = {}
    for path in glob.glob(os.path.join(BIN, "*.dll")):
        found[os.path.basename(path)] = read(path)
    for path in glob.glob(os.path.join("src", "*", "bin", "Debug", "*.dll")):
        name = os.path.basename(path)
        found.setdefault(name, read(path))
    return found


def main():
    assemblies = application_dlls()
    if not assemblies:
        raise SystemExit("nessuna DLL in " + BIN)

    entries = {
        "AppManifest.xaml": build_manifest(sorted(assemblies)),
        "WMAppManifest.xml": read(MANIFEST),
    }
    entries.update(assemblies)

    for root, _dirs, files in os.walk(BIN):
        for name in files:
            if os.path.splitext(name)[1].lower() in (".pdb", ".xap"):
                continue
            path = os.path.join(root, name)
            rel = os.path.relpath(path, BIN).replace("\\", "/")
            if rel not in entries:
                entries[rel] = read(path)

    for root, _dirs, files in os.walk(ASSETS):
        for name in files:
            path = os.path.join(root, name)
            rel = os.path.relpath(path, "Messenger").replace("\\", "/")
            entries[rel] = read(path)

    with zipfile.ZipFile(OUT, "w", zipfile.ZIP_DEFLATED) as archive:
        for name in sorted(entries):
            archive.writestr(name, entries[name])
    print("scritto %s con %d voci (%d assembly)" % (OUT, len(entries), len(assemblies)))


if __name__ == "__main__":
    main()
