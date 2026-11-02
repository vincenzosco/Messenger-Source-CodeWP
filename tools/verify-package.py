"""Controlla il XAP prima del deploy: manifest coerenti e parti presenti.

WP8.1 non firma nulla in build, quindi questi controlli sono l'unica prova che il
pacchetto regga: se un AssemblyPart punta a un file assente l'app non parte e
l'errore sul telefono non dice perche'.
"""

import sys
import xml.etree.ElementTree as ET
import zipfile

XAP = sys.argv[1] if len(sys.argv) > 1 else "Messenger/bin/Debug/Messenger.xap"
DEPLOY_NS = "http://schemas.microsoft.com/windowsphone/2014/deployment"
SILVERLIGHT_NS = "http://schemas.microsoft.com/client/2007/deployment"

problems = []


def check(condition, message):
    print(("ok   " if condition else "FAIL ") + message)
    if not condition:
        problems.append(message)


archive = zipfile.ZipFile(XAP)
names = set(archive.namelist())
check(archive.testzip() is None, "archivio integro")
check("AppManifest.xaml" in names, "AppManifest.xaml alla radice")
check("WMAppManifest.xml" in names, "WMAppManifest.xml alla radice")

app_manifest = ET.fromstring(archive.read("AppManifest.xaml"))
entry_assembly = app_manifest.get("EntryPointAssembly")
entry_type = app_manifest.get("EntryPointType")
runtime = app_manifest.get("RuntimeVersion", "")
check(entry_assembly + ".dll" in names, "entry point presente: " + str(entry_assembly))
check(bool(entry_type), "EntryPointType dichiarato: " + str(entry_type))
check(len(runtime.split(".")) == 4, "RuntimeVersion a 4 parti: " + runtime)

parts = [node for node in app_manifest.iter() if node.tag.endswith("AssemblyPart")]
missing = [p.get("Source") for p in parts if p.get("Source") not in names]
check(not missing, "tutte le %d parti esistono" % len(parts))
if missing:
    print("     mancanti:", ", ".join(missing[:6]))

appx = ET.fromstring(archive.read("WMAppManifest.xml"))
app = appx.find("{%s}App" % DEPLOY_NS)
if app is None:
    app = [c for c in appx.iter() if c.tag.endswith("}App") or c.tag == "App"][0]
version = app.get("Version", "")
check(len(version.split(".")) == 4, "versione applicazione a 4 parti: " + version)
product = app.get("ProductID", "")
check(product.startswith("{") and product.endswith("}"), "ProductID con graffe: " + product)
def child(parent, local_name):
    """Trova un figlio per nome locale: gli elementi di App usano xmlns=""."""
    for node in parent:
        if node.tag == local_name or node.tag.endswith("}" + local_name):
            return node
    return None


icon = child(app, "IconPath")
icon_path = icon.text if icon is not None else None
normalized = icon_path.replace(chr(92), "/") if icon_path else icon_path
check(normalized in names, "icona dichiarata presente: " + str(normalized))

satellites = [n for n in names if n.count("/") == 1 and n.endswith(".resources.dll")]
declared = [p.get("Source") for p in parts if p.get("Source") and p.get("Source").endswith(".resources.dll")]
check(sorted(satellites) == sorted(declared), "satellite dichiarate = presenti (%d)" % len(satellites))

print("")
print("esito:", "PRONTO PER IL DEPLOY" if not problems else "%d problemi" % len(problems))
sys.exit(1 if problems else 0)
