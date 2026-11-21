<div align="center">

<img src="Assets/Art/GameIcon.png" alt="White-hooded character jumping toward a gold coin" width="104">

# 2D Unity Lab

**Jump gaps, collect coins, and find your way past dinosaurs and moving saws in a two-level pixel-art platformer.**

[Download for Windows](https://github.com/m4hmvd/Unity-2D-Platformer/releases/latest) · [Screenshots](#screenshots) · [Controls](#controls) · [Open in Unity](#open-in-unity)

<img src="docs/screenshots/game-cover.png" alt="Pixel-art illustration of the white-hooded player jumping toward three coins, a teal dinosaur, and the red-hooded NPC" width="900">

<sub>Cover illustration inspired by the game.</sub>

</div>

## Play

Download `Unity-2D-Platformer-Windows-x64.zip` from the [latest release](https://github.com/m4hmvd/Unity-2D-Platformer/releases/latest), extract the whole ZIP, and run `Unity-2D-Platformer.exe` from the extracted folder.

## What you’ll find

- **Level 1.** Pass the moving saw, use checkpoints and a rising platform, and collect the heart to open the way to Level 2. Look for the hooded NPC to start a conversation about the Eternal Dahlia.
- **Level 2.** Cross the platforms, dodge or shoot the teal dinosaurs, and find the final door to reach the victory screen.
- **Pickups and checkpoints.** Coins add to your score. Hearts heal when you’ve taken damage. Checkpoints let you recover after losing health, and your remaining health and lives carry into Level 2.

## Screenshots

| Level 1 · Moving saw | Level 2 · Dinosaur enemy |
| :---: | :---: |
| ![The player beside a moving saw in Level 1](docs/screenshots/moving-saw.png) | ![The player facing a teal dinosaur with coins overhead in Level 2](docs/screenshots/dinosaur-enemy.png) |

### Hooded NPC dialogue

![The player meets the red-hooded NPC in Level 1, with the dialogue box open](docs/screenshots/hooded-npc.png)

## Controls

| Action | Input |
| --- | --- |
| Move | **A** / **D** |
| Jump | **Space** |
| Shoot | **Right mouse button** in Level 1, **left mouse button** in Level 2 |
| Zoom | **Mouse wheel** |
| Advance dialogue | Click **Next** |

## Open in Unity

1. Add the project folder in Unity Hub and open it with **Unity 2021.3.10f1**.
2. Open [`Assets/Scenes/Intro.unity`](Assets/Scenes/Intro.unity) and press **Play** to start from the title screen.
3. To build a Windows player, open **File → Build Settings**, select **PC, Mac & Linux Standalone**, choose **Windows** and **x86-64**, and select **Build**.

The build starts at `Intro` and includes `Level1`, `Level2`, `GameOver`, and `Finish`.

Made for the **Game Design and Development** lab at **The British University in Egypt**.

| Folder | Contents |
| --- | --- |
| [`Assets/Scenes`](Assets/Scenes) | Title screen, gameplay levels, and end screens |
| [`Assets/Scripts`](Assets/Scripts) | Movement, combat, dialogue, audio, and level flow |
| [`Assets/Prefabs`](Assets/Prefabs) | Reusable enemies, hazards, and pickups |
