using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Editor-only helpers:
//   Tools > Level 1 - Add Door to Level 2
//   Tools > Level 2 - Build Continue Level
// Both can be undone with Ctrl+Z. Save the scene (Ctrl+S) afterwards.
public static class LevelTools
{
    const string DoorPath = "Assets/Art/Door.png";
    const string PlatformGuid = "e183720b6d63fe44e8d260d9abbee7fc"; // Platform.png, the ground sprite used in Level 1
    const string BulletGuid = "a3ab9c0beb09b484f8e98e9a22bc6e1b";   // Prefabs/Bullet.prefab
    const string ShootSoundGuid = "10888be4f19f2e046bd97466683365e9"; // Audio/bulletLaser.mp3

    // same ground sizes as Level 1
    const float GroundY = -1.77f;
    const float GroundScaleY = 1.9716f;
    const float GroundTop = -0.51f;
    static readonly Vector2 SmallPlatformScale = new Vector2(1.8158356f, 0.6028667f);

    const float DoorScale = 1.4f;
    const float DoorSink = 0.1f;
    const string Undo_Name = "Level Tools";

    // ------------------------------------------------------------------ Level 1

    [MenuItem("Tools/Level 1 - Add Door to Level 2")]
    static void AddDoorToLevel1()
    {
        if (!RequireScene("Level1")) return;

        GameObject entry = GameObject.Find("EntryToLevel2");
        if (entry == null)
        {
            Debug.LogError("LevelTools: couldn't find the EntryToLevel2 object in Level1.");
            return;
        }

        PlaceDoor(entry, entry.transform.position.x);
        EditorSceneManager.MarkSceneDirty(entry.scene);
        Selection.activeGameObject = entry;
        Debug.Log("LevelTools: door added to Level 1. Save the scene with Ctrl+S.");
    }

    // ------------------------------------------------------------------ Level 2

    [MenuItem("Tools/Level 2 - Build Continue Level")]
    static void BuildLevel2()
    {
        if (!RequireScene("Level2")) return;
        if (!EditorUtility.DisplayDialog("Build Level 2",
            "This replaces the old Level 2 layout (a copy of Level 1) with a new stage that continues from Level 1.\n\n" +
            "Camera, Player, Canvas, EventSystem, LevelManager, AudioManager and Background are kept.\n\nYou can undo with Ctrl+Z.",
            "Build", "Cancel")) return;

        Sprite platform = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(PlatformGuid));
        if (platform == null)
        {
            Debug.LogError("LevelTools: couldn't load the Platform sprite.");
            return;
        }

        // 1. remove the old copied layout
        string[] keep = { "Main Camera", "Player", "Canvas", "EventSystem", "LevelManager", "AudioManager", "Background", "--- Background" };
        foreach (GameObject root in EditorSceneManager.GetActiveScene().GetRootGameObjects())
            if (!keep.Contains(root.name))
                Undo.DestroyObjectImmediate(root);

        GameObject layout = new GameObject("Level 2 Layout");
        Undo.RegisterCreatedObjectUndo(layout, Undo_Name);
        Transform t = layout.transform;

        // 2. ground: big sections with gaps between them
        Ground(t, platform, "Ground Start", -14f, 22f);
        Ground(t, platform, "Ground 2", 25f, 46f);
        Ground(t, platform, "Ground Arena", 66f, 100f);
        Ground(t, platform, "Ground End", 103.5f, 152f);

        // floating platforms over the lake
        SmallPlatform(t, platform, "Platform 1", 49f, -0.6f);
        SmallPlatform(t, platform, "Platform 2", 52.5f, 0.4f);
        SmallPlatform(t, platform, "Platform 3", 56f, 1.2f);
        SmallPlatform(t, platform, "Platform 4", 59.5f, 0.4f);
        SmallPlatform(t, platform, "Platform 5", 63f, -0.6f);
        SmallPlatform(t, platform, "Platform High", 112f, 1.6f);

        // anything that falls off the level respawns at the last checkpoint
        GameObject fall = new GameObject("Fall Zone");
        Undo.RegisterCreatedObjectUndo(fall, Undo_Name);
        fall.transform.SetParent(t, false);
        fall.transform.position = new Vector3(69f, -7f, 0f);
        BoxCollider2D fallCol = fall.AddComponent<BoxCollider2D>();
        fallCol.isTrigger = true;
        fallCol.size = new Vector2(200f, 2f);
        fall.AddComponent<DeadlyDrop>();

        // 3. hazards, checkpoints, pickups, enemies
        Prefab(t, "Assets/Prefabs/Lake with damage.prefab", 56f, -3.1f); // the water itself sits ~0.8 above the prefab's root

        Prefab(t, "Assets/Prefabs/Spawn Point.prefab", 27f, 0.13f);
        Prefab(t, "Assets/Prefabs/Spawn Point.prefab", 68f, 0.13f);
        Prefab(t, "Assets/Prefabs/Spawn Point.prefab", 105f, 0.13f);

        float coinY = GroundTop + 0.9f;
        foreach (float x in new[] { 5f, 6.5f, 8f, 30f, 32f, 34f, 74f, 76f, 92f, 94f, 120f, 122f, 136f, 138f, 140f })
            Prefab(t, "Assets/Prefabs/Coin.prefab", x, coinY);
        Prefab(t, "Assets/Prefabs/Coin.prefab", 52.5f, 1.3f);
        Prefab(t, "Assets/Prefabs/Coin.prefab", 56f, 2.1f);
        Prefab(t, "Assets/Prefabs/Coin.prefab", 59.5f, 1.3f);

        Prefab(t, "Assets/Prefabs/Heart.prefab", 112f, 2.5f);   // on the high platform
        Prefab(t, "Assets/Prefabs/Heart.prefab", 84f, coinY);

        // enemies walk between walls, like the arena in Level 1
        Wall(t, "Wall Arena Left", 67.5f);
        Wall(t, "Wall Arena Right", 98.5f);
        Prefab(t, "Assets/Prefabs/Enemy.prefab", 78f, 0.1f);
        Prefab(t, "Assets/Prefabs/Enemy.prefab", 90f, 0.1f);

        Wall(t, "Wall End Left", 108f);
        Wall(t, "Wall End Right", 130f);
        Prefab(t, "Assets/Prefabs/Enemy.prefab", 118f, 0.1f);

        // 4. exit door at the end (opens the Finish screen)
        GameObject exit = new GameObject("Exit Door");
        Undo.RegisterCreatedObjectUndo(exit, Undo_Name);
        exit.transform.SetParent(t, false);
        exit.AddComponent<BoxCollider2D>().isTrigger = true;
        exit.AddComponent<LevelExit>().sceneToLoad = "Finish";
        PlaceDoor(exit, 146f);

        // 5. player, camera and decorations
        SetUpPlayer();
        SetUpCamera();

        Physics2D.SyncTransforms();
        GameObject deco = AddDecorations.Decorate(
            new[] { 10f, 36f, 72f, 93f, 115f, 135f },
            new[] { 3f, 15f, 28f, 41f, 70f, 83f, 95f, 106f, 122f, 133f, 141f, 150f });
        if (deco != null) Undo.SetTransformParent(deco.transform, t, Undo_Name);

        EditorSceneManager.MarkSceneDirty(layout.scene);
        Selection.activeGameObject = layout;
        Debug.Log("LevelTools: Level 2 built. Save the scene with Ctrl+S.");
    }

    // ------------------------------------------------------------------ helpers

    static bool RequireScene(string sceneName)
    {
        if (EditorSceneManager.GetActiveScene().name == sceneName) return true;
        EditorUtility.DisplayDialog("Wrong scene", "Open the " + sceneName + " scene first.", "OK");
        return false;
    }

    static void Ground(Transform parent, Sprite sprite, string name, float x0, float x1)
    {
        GameObject g = Block(parent, sprite, name);
        g.transform.position = new Vector3((x0 + x1) / 2f, GroundY, 0f);
        g.transform.localScale = new Vector3((x1 - x0) / sprite.bounds.size.x, GroundScaleY, 1f);
    }

    static void SmallPlatform(Transform parent, Sprite sprite, string name, float x, float y)
    {
        GameObject g = Block(parent, sprite, name);
        g.transform.position = new Vector3(x, y, 0f);
        g.transform.localScale = new Vector3(SmallPlatformScale.x, SmallPlatformScale.y, 1f);
    }

    static GameObject Block(Transform parent, Sprite sprite, string name)
    {
        GameObject g = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(g, Undo_Name);
        g.transform.SetParent(parent, false);
        g.layer = LayerMask.NameToLayer("Ground");
        SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Ground";
        g.AddComponent<BoxCollider2D>(); // sizes itself to the sprite
        return g;
    }

    static void Wall(Transform parent, string name, float x)
    {
        GameObject w = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(w, Undo_Name);
        w.transform.SetParent(parent, false);
        w.transform.position = new Vector3(x, 0.99f, 0f);
        w.tag = "Wall";
        w.layer = LayerMask.NameToLayer("Wall");
        w.AddComponent<BoxCollider2D>().size = new Vector2(0.5f, 3f);
    }

    static GameObject Prefab(Transform parent, string path, float x, float y)
    {
        GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (asset == null)
        {
            Debug.LogWarning("LevelTools: missing prefab " + path);
            return null;
        }
        GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(asset);
        Undo.RegisterCreatedObjectUndo(g, Undo_Name);
        g.transform.SetParent(parent, false);
        g.transform.position = new Vector3(x, y, 0f);
        return g;
    }

    // puts a door sprite on the ground at x and sizes the trigger to match
    static void PlaceDoor(GameObject owner, float x)
    {
        Sprite door = LoadDoorSprite();
        if (door == null) return;

        Transform oldDoor = owner.transform.Find("Door");
        if (oldDoor != null) Undo.DestroyObjectImmediate(oldDoor.gameObject);

        Vector2 size = door.bounds.size * DoorScale;

        Undo.RecordObject(owner.transform, Undo_Name);
        owner.transform.position = new Vector3(x, GroundTop + size.y / 2f - DoorSink, 0f);
        owner.transform.localScale = Vector3.one;

        BoxCollider2D col = owner.GetComponent<BoxCollider2D>();
        if (col == null) col = Undo.AddComponent<BoxCollider2D>(owner);
        Undo.RecordObject(col, Undo_Name);
        col.isTrigger = true;
        col.offset = Vector2.zero;
        col.size = new Vector2(size.x * 0.6f, size.y);

        GameObject visual = new GameObject("Door");
        Undo.RegisterCreatedObjectUndo(visual, Undo_Name);
        visual.transform.SetParent(owner.transform, false);
        visual.transform.localScale = Vector3.one * DoorScale;
        SpriteRenderer sr = visual.AddComponent<SpriteRenderer>();
        sr.sprite = door;
        sr.sortingLayerName = "Default"; // behind the player
        sr.sortingOrder = 6;
    }

    static Sprite LoadDoorSprite()
    {
        TextureImporter ti = AssetImporter.GetAtPath(DoorPath) as TextureImporter;
        if (ti == null)
        {
            Debug.LogError("LevelTools: missing " + DoorPath);
            return null;
        }
        if (ti.textureType != TextureImporterType.Sprite || ti.filterMode != FilterMode.Point || ti.spritePixelsPerUnit != 16)
        {
            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Single;
            ti.spritePixelsPerUnit = 16;
            ti.filterMode = FilterMode.Point;
            ti.textureCompression = TextureImporterCompression.Uncompressed;
            ti.SaveAndReimport();
        }
        return AssetDatabase.LoadAssetAtPath<Sprite>(DoorPath);
    }

    static void SetUpPlayer()
    {
        GameObject player = GameObject.Find("Player");
        if (player == null) return;

        Undo.RecordObject(player.transform, Undo_Name);
        player.transform.position = new Vector3(0f, 0f, 0f);

        // same controls as Level 1
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            Undo.RecordObject(pc, Undo_Name);
            pc.L = KeyCode.A;
            pc.R = KeyCode.D;
            pc.Spacebar = KeyCode.Space;
        }

        // shooting, like Level 1
        ABShooting shooting = player.GetComponent<ABShooting>();
        if (shooting == null) shooting = Undo.AddComponent<ABShooting>(player);

        Transform point = player.transform.Find("ShootingPoint");
        if (point == null)
        {
            GameObject p = new GameObject("ShootingPoint");
            Undo.RegisterCreatedObjectUndo(p, Undo_Name);
            p.transform.SetParent(player.transform, false);
            p.transform.localPosition = new Vector3(0.512f, -0.141f, 0f);
            point = p.transform;
        }

        Undo.RecordObject(shooting, Undo_Name);
        shooting.Return = KeyCode.Mouse0;
        shooting.shootingPoint = point;
        shooting.sr = player.GetComponent<SpriteRenderer>();
        if (shooting.bullet == null)
            shooting.bullet = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(BulletGuid));
        if (shooting.shootSound == null)
            shooting.shootSound = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(ShootSoundGuid));
    }

    static void SetUpCamera()
    {
        GameObject cam = GameObject.Find("Main Camera");
        if (cam == null) return;
        CameraFollow follow = cam.GetComponent<CameraFollow>();
        if (follow == null) return;

        Undo.RecordObject(follow, Undo_Name);
        follow.minX = 0f;
        follow.maxX = 141f; // keeps the exit door on screen
        GameObject player = GameObject.Find("Player");
        if (player != null) follow.target = player.transform;
    }
}
