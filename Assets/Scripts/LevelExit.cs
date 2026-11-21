using UnityEngine;
using UnityEngine.SceneManagement;

// Door at the end of a level: touching it loads another scene by name.
public class LevelExit : MonoBehaviour
{
    public string sceneToLoad = "Finish"; // the "You Win!" screen

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Level complete!");
            PlayerStats stats = other.GetComponentInParent<PlayerStats>();
            if (stats != null) stats.SaveForNextLevel();
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
