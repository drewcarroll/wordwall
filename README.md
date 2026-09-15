# WordWall

## First-time setup

1. Install **Unity Hub**, then install Unity version **6000.3.22f1** through it.
2. Make sure you have an SSH key added to your GitHub account on this machine (needed to clone/push) — see [GitHub's SSH setup guide](https://docs.github.com/en/authentication/connecting-to-github-with-ssh) if you haven't done this before.
3. Install [Git LFS](https://git-lfs.com/), then run once:
   ```
   git lfs install
   ```
4. Clone the repo:
   ```
   git clone git@github.com:drewcarroll/wordwall.git
   ```
5. In Unity Hub, click **Add** and select the cloned project folder. Open it.
6. Run this once (fixes scene/prefab merge conflicts). Run it in **Git Bash** (installed alongside Git for Windows, and the default terminal on Mac/Linux) — the quoting only works correctly there, not in Command Prompt or PowerShell. Swap in your own Unity install path if it's different.

   **macOS:**
   ```
   git config merge.unityyamlmerge.name "Unity smart merge"
   git config merge.unityyamlmerge.driver '"/Applications/Unity/Hub/Editor/6000.3.22f1/Unity.app/Contents/Helpers/UnityYAMLMerge" merge -p "%O" "%A" "%B" "%A"'
   ```

   **Windows (in Git Bash):**
   ```
   git config merge.unityyamlmerge.name "Unity smart merge"
   git config merge.unityyamlmerge.driver '"/c/Program Files/Unity/Hub/Editor/6000.3.22f1/Editor/Data/Tools/UnityYAMLMerge.exe" merge -p "%O" "%A" "%B" "%A"'
   ```
   (Git Bash maps `C:\Program Files\...` to `/c/Program Files/...` — adjust the drive letter/path if Unity installed somewhere else.)

## Every time you work

```
git pull          # get the latest before you start
# ...do your work in Unity...
git add .
git commit -m "what you did"
git push
```

## Rules to avoid pain

- **Don't both edit the same scene at once.** Split work by scene, or build as prefabs.
- **Commit small and often.** Don't sit on a huge pile of changes.
- **Move/rename files inside Unity**, never in Finder/Explorer.
- **Never commit `Library/`, `Temp/`, `Logs/`, `UserSettings/`** — already ignored, leave them alone.
- Images, audio, models, and fonts are handled automatically by Git LFS — just add/commit like normal.
