using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyStats))]
public class EnemyAI : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private EnemyStats stats;

    [Header("Echo behavior")]
    public float echoBlinkCooldown = 3.5f;
    public float echoBlinkDistance = 2.2f;
    public float echoBlinkTriggerRange = 5.5f;
    private float blinkTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<EnemyStats>();
        blinkTimer = echoBlinkCooldown;
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
            rb.linearVelocity = Vector2.zero;
            return;
        }

        TryEchoBlink();

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.linearVelocity = dir * stats.moveSpeed;
    }

    private void TryEchoBlink()
    {
        if (stats.archetype != EnemyStats.Archetype.Echo) return;

        blinkTimer -= Time.fixedDeltaTime;
        if (blinkTimer > 0f) return;

        Vector2 toPlayer = (Vector2)target.position - rb.position;
        float dist = toPlayer.magnitude;
        if (dist < echoBlinkTriggerRange) return;

        Vector2 dir = toPlayer.normalized;
        rb.position += dir * Mathf.Min(echoBlinkDistance, dist * 0.5f);
        blinkTimer = echoBlinkCooldown;
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
