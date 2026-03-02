using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemy prefabs")]
    public GameObject basicPrefab;
    public GameObject shardPrefab;
    public GameObject wardenPrefab;
    public GameObject echoPrefab;

    public Transform player;
    public float spawnRadius = 12f;

    [Header("Corruption surge")]
    public float surgeEverySeconds = 90f;
    public float surgeDurationSeconds = 12f;

    [Header("Optional")]
    public WaveDirector waveDirector;

    private float timer;

    private void Start()
    {
        if (waveDirector == null)
            waveDirector = FindObjectOfType<WaveDirector>();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        if (player == null) return;

        float runTime = Time.timeSinceLevelLoad;
        float interval;
        int burst;

        if (waveDirector != null)
        {
            interval = waveDirector.GetCurrentInterval(runTime);
            burst = waveDirector.GetCurrentBurstCount(runTime);
        }
        else
        {
            float minutes = runTime / 60f;
            interval = Mathf.Max(0.25f, 1.2f - minutes * 0.15f);
            burst = 1;
        }

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        for (int i = 0; i < burst; i++)
            SpawnOne(runTime);

        timer = interval;
    }

    private void SpawnOne(float runTime)
    {
        GameObject prefab = PickEnemy(runTime);
        if (prefab == null) return;

        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 pos = player.position + new Vector3(dir.x, dir.y, 0f) * spawnRadius;
        Instantiate(prefab, pos, Quaternion.identity);
    }

    private GameObject PickEnemy(float runTime)
    {
        bool surge = IsSurge(runTime);

        // Fallback for old setups.
        if (basicPrefab == null && shardPrefab == null && wardenPrefab == null && echoPrefab == null)
            return null;

        if (surge)
            return PickSurgeEnemy(runTime);

        // Phase weights.
        if (runTime < 120f)
            return WeightedPick(70, basicPrefab, 25, shardPrefab, 5, wardenPrefab, 0, echoPrefab);

        if (runTime < 300f)
            return WeightedPick(40, basicPrefab, 30, shardPrefab, 20, wardenPrefab, 10, echoPrefab);

        return WeightedPick(20, basicPrefab, 25, shardPrefab, 25, wardenPrefab, 30, echoPrefab);
    }

    private GameObject PickSurgeEnemy(float runTime)
    {
        int cycle = Mathf.FloorToInt(runTime / surgeEverySeconds) % 3;

        // 0 = shard surge, 1 = warden surge, 2 = echo surge
        if (cycle == 0)
            return WeightedPick(10, basicPrefab, 70, shardPrefab, 15, wardenPrefab, 5, echoPrefab);
        if (cycle == 1)
            return WeightedPick(10, basicPrefab, 15, shardPrefab, 70, wardenPrefab, 5, echoPrefab);
        return WeightedPick(10, basicPrefab, 15, shardPrefab, 10, wardenPrefab, 65, echoPrefab);
    }

    private bool IsSurge(float runTime)
    {
        if (surgeEverySeconds <= 0f || surgeDurationSeconds <= 0f) return false;
        float t = runTime % surgeEverySeconds;
        return t <= surgeDurationSeconds;
    }

    private GameObject WeightedPick(
        int wBasic, GameObject pBasic,
        int wShard, GameObject pShard,
        int wWarden, GameObject pWarden,
        int wEcho, GameObject pEcho)
    {
        int total = 0;
        if (pBasic != null) total += Mathf.Max(0, wBasic);
        if (pShard != null) total += Mathf.Max(0, wShard);
        if (pWarden != null) total += Mathf.Max(0, wWarden);
        if (pEcho != null) total += Mathf.Max(0, wEcho);

        if (total <= 0) return pBasic ?? pShard ?? pWarden ?? pEcho;

        int roll = Random.Range(0, total);
        int acc = 0;

        if (pBasic != null)
        {
            acc += Mathf.Max(0, wBasic);
            if (roll < acc) return pBasic;
        }
        if (pShard != null)
        {
            acc += Mathf.Max(0, wShard);
            if (roll < acc) return pShard;
        }
        if (pWarden != null)
        {
            acc += Mathf.Max(0, wWarden);
            if (roll < acc) return pWarden;
        }

        return pEcho ?? pBasic ?? pShard ?? pWarden;
    }
}
