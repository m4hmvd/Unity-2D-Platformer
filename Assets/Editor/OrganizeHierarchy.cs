using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Editor-only helper: Tools > Organize Hierarchy
// Sorts the loose root objects of the open scene into tidy group folders.
// Objects keep their world position. Ctrl+Z undoes it. Works on any level.
public static class OrganizeHierarchy
{
    // these stay at the top level: AudioManager must be a root object for DontDestroyOnLoad,
    // and the others are easier to find there
    static readonly string[] StayAtRoot =
        { "Main Camera", "Player", "LevelManager", "AudioManager", "DialogueManager", "Canvas", "EventSystem" };

    // group folders, in the order they appear in the Hierarchy
    static readonly string[] Groups =
        { "--- Ground & Platforms", "--- Collectables", "--- Checkpoints", "--- Enemies & Hazards", "--- NPCs", "--- Level Exit", "--- Decorations", "--- Background", "--- Other" };

    [MenuItem("Tools/Organize Hierarchy")]
    static void Organize()
    {
        var scene = EditorSceneManager.GetActiveScene();
        var roots = scene.GetRootGameObjects();

        // reuse folders from an earlier run
        var folders = new Dictionary<string, Transform>();
        foreach (var r in roots)
            if (Groups.Contains(r.name)) folders[r.name] = r.transform;

        int moved = 0;
        foreach (GameObject go in roots)
        {
            if (Groups.Contains(go.name) || StayAtRoot.Contains(go.name)) continue;

            string group = GroupFor(go);
            if (!folders.TryGetValue(group, out Transform folder))
            {
                GameObject f = new GameObject(group);
                Undo.RegisterCreatedObjectUndo(f, "Organize Hierarchy");
                folders[group] = folder = f.transform;
            }
            Undo.SetTransformParent(go.transform, folder, "Organize Hierarchy");
            moved++;
        }

        // root order: camera/player/managers/UI first, then the folders
        int index = 0;
        foreach (string name in StayAtRoot)
        {
            GameObject go = roots.FirstOrDefault(r => r.name == name);
            if (go != null) { go.transform.SetSiblingIndex(index++); }
        }
        foreach (string g in Groups)
            if (folders.TryGetValue(g, out Transform f)) { f.SetSiblingIndex(index++); }

        // sort each folder's children by name so e.g. Coin (1), Coin (2)... sit together
        foreach (Transform f in folders.Values)
        {
            var kids = f.Cast<Transform>().OrderBy(k => k.name).ToList();
            for (int i = 0; i < kids.Count; i++) kids[i].SetSiblingIndex(i);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("Organize Hierarchy: moved " + moved + " objects into groups. Save the scene with Ctrl+S.");
    }

    static string GroupFor(GameObject go)
    {
        string n = go.name.ToLower();

        if (Has<Collectable>(go) || Has<CollectHeart>(go)) return "--- Collectables";
        if (Has<Checkpoint>(go)) return "--- Checkpoints";
        if (Has<EntryToLevel2>(go) || Has<LevelExit>(go)) return "--- Level Exit";
        if (Has<NPCDialogue>(go)) return "--- NPCs";

        if (Has<EnemyController>(go) || Has<DeadlyDrop>(go) || Has<SpawnDinasour>(go)
            || n.Contains("saw") || n.Contains("trap") || n.Contains("lake") || n.Contains("particle")
            || n.StartsWith("wall") || n == "spawn point")
            return "--- Enemies & Hazards";

        if (n.StartsWith("ground") || n.Contains("platform") || n.Contains("brick")
            || n == "point a" || n == "point b" || go.layer == LayerMask.NameToLayer("Ground"))
            return "--- Ground & Platforms";

        if (n.Contains("tree") || n.Contains("bush") || n.Contains("flower") || n.Contains("rose") || n == "decorations")
            return "--- Decorations";

        if (n.Contains("sky") || n.Contains("cloud") || n.Contains("background"))
            return "--- Background";

        return "--- Other";
    }

    static bool Has<T>(GameObject go) where T : Component
    {
        return go.GetComponentInChildren<T>(true) != null;
    }
}
