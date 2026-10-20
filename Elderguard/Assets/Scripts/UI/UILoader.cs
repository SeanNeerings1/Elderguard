using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }
}