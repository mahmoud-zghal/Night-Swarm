using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 12f;

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
        if (enemyPrefab == null || player == null) return;

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
            // Fallback old behavior when no wave director is attached.
            float minutes = runTime / 60f;
            interval = Mathf.Max(0.25f, 1.2f - minutes * 0.15f);
            burst = 1;
        }

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        for (int i = 0; i < burst; i++)
            SpawnOne();

        timer = interval;
    }

    private void SpawnOne()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 pos = player.position + new Vector3(dir.x, dir.y, 0f) * spawnRadius;
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
}
