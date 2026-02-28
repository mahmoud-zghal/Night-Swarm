using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyStats))]
public class EnemyAI : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private EnemyStats stats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) target = player.transform;
    }

    private void FixedUpdate()
    {
        if (target == null || (GameManager.Instance != null && GameManager.Instance.IsGameOver))
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.velocity = dir * stats.moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var playerStats = other.gameObject.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(stats.touchDamage * Time.fixedDeltaTime);
        }
    }
}
