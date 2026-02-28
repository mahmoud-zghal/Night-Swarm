using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1.5f;
    public float range = 8f;

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

        Vector2 dir = (target.position - transform.position).normalized;
        var go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Init(dir, stats.damage);
        }

        timer = 1f / Mathf.Max(0.01f, fireRate);
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
