using System.Linq;
using UnityEditor;

// Makes sure the Finish scene is in File > Build Settings so LevelExit can load it.
// Runs automatically whenever Unity reloads scripts; does nothing if it's already there.
[InitializeOnLoad]
public static class AddFinishSceneToBuild
{
    const string FinishScene = "Assets/Scenes/Finish.unity";

    static AddFinishSceneToBuild()
    {
        EditorApplication.delayCall += Ensure;
    }

    static void Ensure()
    {
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(FinishScene) == null) return;

        var scenes = EditorBuildSettings.scenes.ToList();
        var existing = scenes.FirstOrDefault(s => s.path == FinishScene);
        if (existing != null && existing.enabled) return;

        scenes.RemoveAll(s => s.path == FinishScene);
        scenes.Add(new EditorBuildSettingsScene(FinishScene, true)); // added last, so Intro/Level1/Level2/GameOver keep their numbers
        EditorBuildSettings.scenes = scenes.ToArray();
        UnityEngine.Debug.Log("Added Finish scene to Build Settings.");
    }
}
