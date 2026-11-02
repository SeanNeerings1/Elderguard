using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void Load(string sceneName) => SceneManager.LoadScene(sceneName);

    public void RestartLevel() => SceneManager.LoadScene(LevelMemory.Current);

    public void NextLevel()
    {
        string next = string.IsNullOrEmpty(LevelMemory.Next) ? "Start" : LevelMemory.Next;
        SceneManager.LoadScene(next);
    }
}