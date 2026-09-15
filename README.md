# WordWall

## First-time setup

1. Install **Unity Hub**, then install Unity version **6000.3.22f1** through it.
2. Install [Git LFS](https://git-lfs.com/), then run once:
   ```
   git lfs install
   ```
3. Clone the repo:
   ```
   git clone git@github.com:drewcarroll/wordwall.git
   ```
4. In Unity Hub, click **Add** and select the cloned project folder. Open it.
5. Run this once (fixes scene/prefab merge conflicts). Swap in your own Unity install path if it's different:
   ```
   git config merge.unityyamlmerge.name "Unity smart merge"
   git config merge.unityyamlmerge.driver '"/Applications/Unity/Hub/Editor/6000.3.22f1/Unity.app/Contents/Helpers/UnityYAMLMerge" merge -p "%O" "%A" "%B" "%A"'
   ```
   Windows path: `C:\Program Files\Unity\Hub\Editor\6000.3.22f1\Editor\Data\Tools\UnityYAMLMerge.exe`

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
