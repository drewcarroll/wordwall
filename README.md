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

---

# WordWall（中文说明 - Windows 版，新手向）

这一节是给第一次用 GitHub 和 Unity 的人看的。我们推荐用 **GitHub Desktop**（一个带界面的图形化工具），这样就不用记命令行、也不用折腾 SSH 密钥。

一些基本概念：
- **仓库 (repository)**：这个项目的所有文件，存在 GitHub 网站上的一份，加上你电脑上的一份。
- **克隆 (clone)**：把 GitHub 上的项目第一次下载到你电脑上。
- **提交 (commit)**：给你做的一批修改起个名字，记录下来（还没有上传）。
- **推送 (push)**：把你的提交上传到 GitHub，让其他人也能看到。
- **拉取 (pull)**：把别人上传的最新内容下载到你电脑上。

## 首次设置

1. **安装 Unity Hub 和 Unity 编辑器**：去 [unity.com/download](https://unity.com/download) 下载安装 **Unity Hub**。打开 Hub 后，去安装 Unity 版本 **6000.3.22f1**（版本号必须完全一致，不能装成别的版本）。
2. **安装 Git 和 Git LFS**（这一步是给电脑装上处理这个项目所需的底层工具，之后基本不用再管它）：
   - 下载安装 [Git for Windows](https://git-scm.com/download/win)（一路下一步默认选项就行）。
   - 下载安装 [Git LFS](https://git-lfs.com/)。
   - 装完之后，随便打开一个叫 **Git Bash** 的黑色命令行窗口（装完 Git for Windows 后桌面/开始菜单会有这个程序），输入下面这行，敲回车，只需要做一次：
     ```
     git lfs install
     ```
3. **安装 GitHub Desktop**：去 [desktop.github.com](https://desktop.github.com/) 下载安装，打开后用你的 GitHub 账号登录（网页里点一下登录就行，不需要 SSH 密钥）。
4. **克隆项目**：在 GitHub Desktop 里，点左上角 **File → Clone Repository**，切换到 **URL** 这个标签页，粘贴：
   ```
   https://github.com/drewcarroll/wordwall.git
   ```
   选好保存的文件夹位置，点 **Clone**（克隆）。下载可能需要一点时间。
5. **在 Unity Hub 里打开项目**：打开 Unity Hub，点 **Add**（添加），选择刚才克隆下来的项目文件夹，然后点击打开。

   ⚠️ **第一次打开会很慢，可能要等 10-20 分钟**，Unity 会在后台导入和编译很多东西，看起来像卡住了、转圈圈不动，**这是正常的，不是电脑坏了**，请耐心等待，不要强制关闭。如果实在担心卡死了，可以在群里发截图问 Drew。

6. **设置合并工具**（这一步稍微有点技术性，如果卡住了直接找 Drew 帮忙，不丢人）。这一步能让 Unity 自动处理场景文件的冲突，减少以后合作时的麻烦。打开 **Git Bash**，粘贴下面两行，回车执行（如果你的 Unity 装在别的路径，需要相应修改）：
   ```
   git config merge.unityyamlmerge.name "Unity smart merge"
   git config merge.unityyamlmerge.driver '"/c/Program Files/Unity/Hub/Editor/6000.3.22f1/Editor/Data/Tools/UnityYAMLMerge.exe" merge -p "%O" "%A" "%B" "%A"'
   ```
   注意：这行命令要在克隆下来的项目文件夹里运行（在 Git Bash 里先用 `cd` 进入那个文件夹，或者右键项目文件夹选"Git Bash Here"）。

## 每次开始工作时

1. 打开 GitHub Desktop，点 **Fetch origin** / **Pull origin**（拉取最新代码），确保你是最新版本再开始改东西。
2. 在 Unity 里做你的工作（改场景、写代码、加美术资源等）。
3. 回到 GitHub Desktop，左边会列出你改动过的文件。在左下角输入这次改动的简单说明（比如"加了主菜单按钮"），点 **Commit to main**。
4. 点右上角 **Push origin**，把你的提交上传上去。

## 避免踩坑的规则

- **不要和别人同时编辑同一个场景（scene）文件。** 容易冲突。尽量各自负责不同场景，或者把功能做成预制体（prefab）再拖进场景里。
- **小步提交，频繁提交。** 不要攒一大堆改动才提交一次。
- **移动或重命名文件要在 Unity 里操作**，不要直接在 Windows 文件资源管理器里拖动/改名，否则会破坏引用。
- **`Library`、`Temp`、`Logs`、`UserSettings` 这几个文件夹永远不用管** — 它们不会被提交，也不需要理会。
- 图片、音频、模型、字体这些大文件会自动通过 Git LFS 处理，正常提交推送就行，不用额外操作。
- **卡住了随时截图问 Drew**，Git/Unity 踩坑很正常，第一次配环境大家都会遇到问题。
