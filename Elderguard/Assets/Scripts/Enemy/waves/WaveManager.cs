using System.Collections;
using UnityEngine;

[System.Serializable]
public class SpawnGroup
{
    public Enemy prefab;          
    public int count = 5;
    public float interval = 0.9f;
}

[System.Serializable]
public class Wave { public SpawnGroup[] groups; }

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;         
    public Enemy bossPrefab;      

    int nextWave;
    bool spawning;

    public void StartNextWave()
    {
        //Debug.Log("StartNextWave called");
        //if (spawning) { Debug.Log("Blocked: already spawning"); return; }
        //if (Enemy.All.Count > 0) { Debug.Log("Blocked: enemies alive: " + Enemy.All.Count); return; }
        //if (GameManage.Instance.IsOver) { Debug.Log("Blocked: game over"); return; }
        //if (nextWave >= waves.Length) { Debug.Log("Blocked: no waves left"); return; }
        //StartCoroutine(Run(waves[nextWave++]));
        if (spawning || Enemy.All.Count > 0 || GameManage.Instance.IsOver || nextWave >= waves.Length) return;
        StartCoroutine(Run(waves[nextWave++]));
    }

    IEnumerator Run(Wave wave)
    {
        spawning = true;
        GameManage.Instance.SetWave(nextWave, waves.Length);

        foreach (SpawnGroup g in wave.groups)
            for (int i = 0; i < g.count; i++)
            {
                Spawn(g.prefab);
                yield return new WaitForSeconds(g.interval);
            }

        if (nextWave >= waves.Length && bossPrefab != null) Spawn(bossPrefab);
        spawning = false;
    }

    void Spawn(Enemy prefab)
    {
        Enemy e = Instantiate(prefab, WaypointPath.Instance.Get(0), Quaternion.identity);
        e.Init(nextWave);
    }

    void Update()
    {
        if (!spawning && nextWave >= waves.Length && nextWave > 0 && Enemy.All.Count == 0)
            GameManage.Instance.Win();
    }
}