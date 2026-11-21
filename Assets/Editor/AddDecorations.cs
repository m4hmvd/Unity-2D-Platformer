using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Editor-only helper: Tools > Add Bushes and Trees
// Places bushes and trees along the main ground of the open scene.
// Running it again replaces the old "Decorations" group, and Ctrl+Z undoes it.
public static class AddDecorations
{
    const string BushFolder = "Assets/Lab 5 Art Assets/Bushes & Flowers/Bushes & Flowers/";
    const string TreeSheet = "Assets/Lab 5 Art Assets/trees and green objects.png";
    const string TreeSpriteName = "trees and green objects_4";

    static readonly string[] BushPaths =
    {
        BushFolder + "Bush 1/Bush 1 - GREEN.png",
        BushFolder + "Bush 1/Bush 1 - WARM GREEN.png",
        BushFolder + "Bush 1 (No Flowers)/Bush 1 (no flowers) - GREEN.png",
        BushFolder + "Bush 2/Bush 2 - PINK.png",
        BushFolder + "Bush 1/Bush 1 - TEAL.png",
    };

    // x positions along the level (kept away from the NPC, rose, trap and level exit)
    static readonly float[] TreeX = { 12f, 36f, 77f, 93f, 118f, 137f };
    static readonly float[] BushX = { 4f, 8.5f, 21f, 27f, 41f, 74f, 81f, 89f, 108f, 115f, 131f, 140f };

    const float TreeScale = 3.35f;  // same as the tree already in Level1
    const float BushScale = 1.5f;
    const float SinkIntoGround = 0.15f; // hides the flat bottom edge behind the ground

    [MenuItem("Tools/Add Bushes and Trees")]
    static void Add()
    {
        Decorate(TreeX, BushX);
    }

    // also used by LevelTools to decorate Level 2
    public static GameObject Decorate(float[] treeX, float[] bushX)
    {
        Sprite tree = AssetDatabase.LoadAllAssetsAtPath(TreeSheet)
            .OfType<Sprite>().FirstOrDefault(s => s.name == TreeSpriteName);
        Sprite[] bushes = BushPaths.Select(AssetDatabase.LoadAssetAtPath<Sprite>)
            .Where(s => s != null).ToArray();
        if (tree == null || bushes.Length == 0)
        {
            Debug.LogError("AddDecorations: couldn't load the tree or bush sprites.");
            return null;
        }

        GameObject old = GameObject.Find("Decorations");
        if (old != null) Undo.DestroyObjectImmediate(old);

        GameObject parent = new GameObject("Decorations");
        Undo.RegisterCreatedObjectUndo(parent, "Add Bushes and Trees");

        Physics2D.SyncTransforms();
        int groundMask = LayerMask.GetMask("Ground");
        int placed = 0;

        for (int i = 0; i < treeX.Length; i++)
            if (Place(parent.transform, "Tree " + (i + 1), tree, treeX[i], TreeScale, i % 2 == 1, groundMask)) placed++;

        for (int i = 0; i < bushX.Length; i++)
            if (Place(parent.transform, "Bush " + (i + 1), bushes[i % bushes.Length], bushX[i], BushScale, i % 3 == 2, groundMask)) placed++;

        EditorSceneManager.MarkSceneDirty(parent.scene);
        Selection.activeGameObject = parent;
        Debug.Log("AddDecorations: placed " + placed + " bushes and trees. Save the scene with Ctrl+S.");
        return parent;
    }

    static bool Place(Transform parent, string name, Sprite sprite, float x, float scale, bool flip, int groundMask)
    {
        // look straight down for the main ground (wide colliders only, so small floating platforms are skipped)
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(x, 40f), Vector2.down, 80f, groundMask);
        RaycastHit2D hit = hits.FirstOrDefault(h => !h.collider.isTrigger && h.collider.bounds.size.x > 8f);
        if (hit.collider == null)
        {
            Debug.LogWarning("AddDecorations: no ground under x = " + x + ", skipped " + name);
            return false;
        }

        GameObject go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, "Add Bushes and Trees");
        go.transform.SetParent(parent, false);
        go.transform.localScale = Vector3.one * scale;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.flipX = flip;
        sr.sortingLayerName = "Default"; // behind the Player, Enemy and Ground layers
        sr.sortingOrder = 5;             // in front of the sky and clouds

        // put the bottom of the sprite on the ground surface
        float halfHeight = sprite.bounds.extents.y * scale;
        float pivotOffset = sprite.bounds.center.y * scale;
        go.transform.position = new Vector3(x, hit.point.y + halfHeight - pivotOffset - SinkIntoGround, 0f);
        return true;
    }
}
