using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    [Header("Base attack")]
    public GameObject projectilePrefab;
    public float fireRate = 1.5f;
    public float range = 8f;

    [Header("Multi-shot")]
    [Range(1, 7)] public int projectileCount = 1;
    [Range(0f, 45f)] public float spreadAngle = 14f;
    [Range(1, 7)] public int maxProjectileCount = 5;

    private float timer;
    private PlayerStats stats;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        if (projectilePrefab == null) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        Transform target = FindNearestEnemy();
        if (target == null) return;

        Vector2 baseDir = (target.position - transform.position).normalized;
        FireBurst(baseDir);

        timer = 1f / Mathf.Max(0.01f, fireRate);
    }

    private void FireBurst(Vector2 baseDir)
    {
        if (projectileCount <= 1)
        {
            SpawnProjectile(baseDir);
            return;
        }

        float step = spreadAngle;
        float start = -step * (projectileCount - 1) * 0.5f;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = start + step * i;
            Vector2 dir = Rotate(baseDir, angle).normalized;
            SpawnProjectile(dir);
        }
    }

    private void SpawnProjectile(Vector2 dir)
    {
        var go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Init(dir, stats.damage);
        }
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float r = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(r);
        float sin = Mathf.Sin(r);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    public void AddMultiShot(int amount = 1)
    {
        projectileCount = Mathf.Clamp(projectileCount + amount, 1, maxProjectileCount);
        Debug.Log($"Upgrade: Multi-shot x{projectileCount}");
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < bestDist && d <= range)
            {
                bestDist = d;
                best = e.transform;
            }
        }
        return best;
    }
}
