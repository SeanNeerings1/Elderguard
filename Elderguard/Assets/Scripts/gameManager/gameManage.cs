using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManage : MonoBehaviour
{
    public static GameManage Instance { get; private set; }

    [SerializeField] int startingBaseHealth = 20;
    [SerializeField] TMP_Text healthText;     
    [SerializeField] TMP_Text waveText;
    [SerializeField] string[] levelOrder;   

    public int BaseHealth { get; private set; }
    public bool IsOver { get; private set; }

    public event Action<int> OnBaseHealthChanged;

    void Awake()
    {
        Instance = this;
        BaseHealth = startingBaseHealth;
        Time.timeScale = 1f;
        UpdateText();
    }

    void Start() { OnBaseHealthChanged?.Invoke(BaseHealth); }

    void OnDestroy() { if (Instance == this) Instance = null; }

    public void DamageBase(int amount)
    {
        Debug.Log("DamageBase called: " + amount + ", IsOver = " + IsOver);   // <-- add this line
        if (IsOver) return;
        BaseHealth = Mathf.Max(0, BaseHealth - amount);
        UpdateText();
        OnBaseHealthChanged?.Invoke(BaseHealth);
        if (BaseHealth == 0) End(false);
    }

    public void Win() { if (!IsOver) End(true); }

    void End(bool won)
    {
        IsOver = true;

        LevelMemory.Current = SceneManager.GetActiveScene().name;
        int i = Array.IndexOf(levelOrder, LevelMemory.Current);
        LevelMemory.Next = (i >= 0 && i + 1 < levelOrder.Length) ? levelOrder[i + 1] : "";

        Time.timeScale = 1f; 
        SceneManager.LoadScene(won ? "Victory" : "GameOver");
    }

    public void SetWave(int wave, int total) { if (waveText) waveText.text = "Wave " + wave + "/" + total; }

    public void ToggleSpeed() { if (!IsOver) Time.timeScale = Time.timeScale > 1f ? 1f : 2f; }
    public void Restart() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }

    public void NextArea()
    {
        int n = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(n < SceneManager.sceneCountInBuildSettings ? n : 0);
    }

    void UpdateText() { if (healthText) healthText.text = "Village " + BaseHealth; }
}