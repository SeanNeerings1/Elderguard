using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class waveHelper : MonoBehaviour
{
    void Awake() { GetComponent<Button>().onClick.AddListener(OnClick); }

    void OnClick()
    {
        Debug.Log("Button clicked");
        var spawner = FindFirstObjectByType<WaveManager>();
        if (spawner != null) spawner.StartNextWave();
        else Debug.LogWarning("No WaveManager found in the loaded scenes.");
    }
}