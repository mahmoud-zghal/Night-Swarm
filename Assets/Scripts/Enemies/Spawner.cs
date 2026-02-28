using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float baseSpawnInterval = 1.2f;
    public float minSpawnInterval = 0.25f;
    public float difficultyRampPerMinute = 0.15f;
    public float spawnRadius = 12f;

    private float timer;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        if (enemyPrefab == null || player == null) return;

        float minutes = Time.timeSinceLevelLoad / 60f;
        float interval = Mathf.Max(minSpawnInterval, baseSpawnInterval - minutes * difficultyRampPerMinute);

        timer -= Time.deltaTime;
        if (timer > 0f) return;

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
