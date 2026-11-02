# Regole del progetto

## Commit

- Messaggi in inglese, all'imperativo (`Add ...`, `Restore ...`): soggetto
  breve, corpo solo se serve a spiegare il perché.
- **Nessun trailer di attribuzione AI.** Vietati `Co-Authored-By:` con un agente,
  `Generated with ...`, `Made with ...`, `Created by`, la variante con emoji e
  qualunque nome di agente nei campi `author`/`committer`. Il messaggio deve
  sembrare scritto dall'autore del repository.
- Controllo prima di pushare:

  ```bash
  git log -1 --format='%B' | grep -inE 'co-authored-by|generated with|claude|codebuff|copilot'
  ```

  Nessun output = regola rispettata.

## Codice decompilato (`decompiled/`)

- È output generato da `ilspycmd`: non va modificato a mano, si rigenera
  (comando in `RE_ANALYSIS.md`).
- Non fa parte di `Messenger.sln` e non deve essere aggiunto al build: è
  materiale di analisi, non codice compilabile.

## Materiale ricostruito e localizzazioni

- `recovered/` contiene i metodi ritradotti a mano dal IL: qui si può
  intervenire, ma resta materiale di analisi e non entra nel build.
- `localization/` è generata da `tools/resx-dump`: le `.resx` non si correggono a
  mano, si rilancia l'estrattore sul payload in `extracted/`.

## File non versionati

- `*.xap` (pacchetto originale), `extracted/` (payload grezzo) e `.tools/`
  (SDK .NET + decompilatore locali) restano fuori da Git: vedi `.gitignore`.
