# Unity 2D Platformer — Game Design and Development Lab

2D platformer built in Unity for the **Game Design and Development** course lab
(BUE, Year 2 — Semester 1).

## Project info

| | |
|---|---|
| Unity version | 2021.3.10f1 |
| Render pipeline | Built-in (2D) |
| Main scene | `Assets/Scenes/SampleScene.unity` |
| Target platform | Windows / Standalone |

## Controls

| Action | Input |
|---|---|
| Move left | `L` |
| Move right | `R` |
| Jump | `Space` |
| Camera zoom | Mouse scroll wheel |

`L`, `R` and `Space` are exposed as serialized `KeyCode` fields on the
`PlayerController` component, so they can be reassigned in the Inspector.

## Scripts

| Script | Responsibility |
|---|---|
| `Assets/Scripts/PlayerController.cs` | Horizontal movement via `Rigidbody2D.velocity`, jump impulse, ground check (`Physics2D.OverlapCircle`), sprite flip, animation parameters (`Speed`, `Height`, `Grounded`) |
| `Assets/Scripts/CameraFollow.cs` | Lerped camera follow toward a target transform, clamped to `minX/maxX/minY/maxY` bounds |
| `Assets/Scripts/CameraZoom.cs` | Orthographic zoom on mouse scroll, clamped between 3 and 6, lerped toward the target zoom |

## Assets

- `Assets/Animation/` — player animator controller with Idle, Run, Jump and Fall clips.
- `Assets/Particles/` — particle sprite sheets and materials.
- `Assets/Simple 2D Platformer BE2/` — third-party Unity Asset Store package (sprites, demo scene, readme).
- Loose sprites at the `Assets/` root: `clouds.png`, `sky.png`, `Platform.png`, `s_water_img.png`.

## Getting started

```bash
git clone <this-repo-url>
```

Then open the folder as a project in Unity Hub with **Unity 2021.3.10f1**
(Unity regenerates `Library/` and the `.sln`/`.csproj` files on first open),
and open `Assets/Scenes/SampleScene.unity`.

## Not tracked

`Library/`, `Logs/`, `obj/`, `UserSettings/` and generated solution/project
files are excluded via `.gitignore` — they are machine-specific build caches.
