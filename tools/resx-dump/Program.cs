using System.Collections;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

// Estrae le risorse embedded dalle assembly satellite (.resources.dll) e le
// riscrive come file .resx, una cartella per cultura. Le risorse non stringa
// (per esempio i BAML) non sono convertibili in .resx: vengono elencate come
// commento nel file e contate nel riepilogo.
// Uso: dotnet run --project tools/resx-dump -- <cartella-estratta> <cartella-output>

var root = args.Length > 0 ? args[0] : "extracted";
var outRoot = args.Length > 1 ? args[1] : "localization";
Directory.CreateDirectory(outRoot);

var rows = new List<(string Culture, string Assembly, int Keys, int Skipped)>();
var emptySatellites = 0;

foreach (var dir in Directory.GetDirectories(root).OrderBy(d => d, StringComparer.Ordinal))
{
    var culture = Path.GetFileName(dir);
    if (!Regex.IsMatch(culture, "^[a-z]{2}(-[A-Z]{2})?$"))
    {
        continue;
    }

    foreach (var dll in Directory.GetFiles(dir, "*.resources.dll").OrderBy(f => f, StringComparer.Ordinal))
    {
        var assemblyBase = Path.GetFileName(dll)[..^".resources.dll".Length];
        var outDir = Path.Combine(outRoot, culture);
        Directory.CreateDirectory(outDir);

        foreach (var (entries, skipped) in ReadResources(dll))
        {
            if (entries.Count == 0 && skipped.Count == 0)
            {
                emptySatellites++;
                Console.WriteLine($"{assemblyBase}.{culture}: nessuna risorsa stringa, file non creato");
                continue;
            }

            var outFile = Path.Combine(outDir, $"{assemblyBase}.{culture}.resx");
            WriteResx(outFile, entries, skipped);
            rows.Add((culture, assemblyBase, entries.Count, skipped.Count));
            Console.WriteLine($"{outFile}: {entries.Count} stringhe, {skipped.Count} risorse non stringa");
        }
    }
}

var summary = new StringBuilder();
summary.AppendLine("# Risorse di localizzazione");
summary.AppendLine();
summary.AppendLine($"Generate da `tools/resx-dump` leggendo le assembly satellite in `{root}/`.");
summary.AppendLine($"Culture: {rows.Select(r => r.Culture).Distinct().Count()}, file: {rows.Count}, stringhe: {rows.Sum(r => r.Keys)}, risorse non stringa: {rows.Sum(r => r.Skipped)}.");
summary.AppendLine($"Assembly satellite senza risorse stringa (file non creato): {emptySatellites}.");
summary.AppendLine();
summary.AppendLine("| Cultura | Assembly | Stringhe | Non stringa | File |");
summary.AppendLine("| --- | --- | --- | --- | --- |");
foreach (var row in rows.OrderBy(r => r.Culture, StringComparer.Ordinal).ThenBy(r => r.Assembly, StringComparer.Ordinal))
{
    summary.AppendLine($"| `{row.Culture}` | `{row.Assembly}` | {row.Keys} | {row.Skipped} | `{row.Culture}/{row.Assembly}.{row.Culture}.resx` |");
}
File.WriteAllText(Path.Combine(outRoot, "README.md"), summary.ToString(), new UTF8Encoding(true));

// ---------------------------------------------------------------------------

static IEnumerable<(SortedDictionary<string, string> Entries, List<string> Skipped)> ReadResources(string dllPath)
{
    using var stream = File.OpenRead(dllPath);
    using var pe = new PEReader(stream);
    var md = pe.GetMetadataReader();
    var resourcesDirectory = pe.PEHeaders.CorHeader!.ResourcesDirectory;
    var section = pe.GetSectionData(resourcesDirectory.RelativeVirtualAddress);

    foreach (var handle in md.ManifestResources)
    {
        var resource = md.GetManifestResource(handle);
        if (!resource.Implementation.IsNil)
        {
            continue; // risorsa esterna, non embedded
        }

        // voce del manifest: prefisso a 4 byte con la lunghezza, poi il blob .resources
        var reader = section.GetReader();
        reader.Offset = (int)resource.Offset;
        var length = reader.ReadInt32();
        var blob = reader.ReadBytes(length);

        var entries = new SortedDictionary<string, string>(StringComparer.Ordinal);
        var skipped = new List<string>();
        using var ms = new MemoryStream(blob);
        using var resourceReader = new ResourceReader(ms);
        foreach (DictionaryEntry entry in resourceReader)
        {
            if (entry.Key is not string key)
            {
                skipped.Add($"chiave non stringa: {entry.Key?.GetType().FullName ?? "null"}");
                continue;
            }

            if (entry.Value is string text)
            {
                entries[key] = text;
            }
            else
            {
                var type = entry.Value?.GetType().FullName ?? "null";
                if (entry.Value is Stream valueStream)
                {
                    type += $" ({valueStream.Length} byte)";
                }
                skipped.Add($"{key}: {type}");
            }
        }

        yield return (entries, skipped);
    }
}

static void WriteResx(string path, SortedDictionary<string, string> entries, List<string> skipped)
{
    var sb = new StringBuilder();
    sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
    sb.AppendLine("<root>");
    sb.AppendLine("  <resheader name=\"resmimetype\">");
    sb.AppendLine("    <value>text/microsoft-resx</value>");
    sb.AppendLine("  </resheader>");
    sb.AppendLine("  <resheader name=\"version\">");
    sb.AppendLine("    <value>2.0</value>");
    sb.AppendLine("  </resheader>");
    sb.AppendLine("  <resheader name=\"reader\">");
    sb.AppendLine("    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>");
    sb.AppendLine("  </resheader>");
    sb.AppendLine("  <resheader name=\"writer\">");
    sb.AppendLine("    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>");
    sb.AppendLine("  </resheader>");
    foreach (var note in skipped)
    {
        sb.AppendLine($"  <!-- risorsa non stringa, non rappresentabile in resx: {Escape(note)} -->");
    }
    foreach (var (key, value) in entries)
    {
        sb.Append("  <data name=\"").Append(Escape(key)).Append("\" xml:space=\"preserve\"><value>")
          .Append(Escape(value)).AppendLine("</value></data>");
    }
    sb.AppendLine("</root>");
    File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
}

static string Escape(string value) => value
    .Replace("&", "&amp;")
    .Replace("<", "&lt;")
    .Replace(">", "&gt;");
