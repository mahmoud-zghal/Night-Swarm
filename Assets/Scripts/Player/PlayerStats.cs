using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Core Stats")]
    public float maxHp = 100f;
    public float currentHp;
    public float moveSpeed = 5f;
    public float damage = 10f;

    [Header("Hit Feedback")]
    public float hitInvulnSeconds = 0.2f;
    public float flashSeconds = 0.08f;
    public Color flashColor = new Color(1f, 0.35f, 0.35f, 1f);

    private float invulnTimer;
    private SpriteRenderer sr;
    private Color baseColor = Color.white;
    private float flashTimer;

    private void Awake()
    {
        currentHp = maxHp;
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) baseColor = sr.color;
    }

    private void Update()
    {
        if (invulnTimer > 0f) invulnTimer -= Time.deltaTime;

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f && sr != null)
                sr.color = baseColor;
        }
    }

    public void TakeDamage(float amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        if (invulnTimer > 0f) return;

        currentHp -= amount;
        invulnTimer = hitInvulnSeconds;

        if (sr != null)
        {
            sr.color = flashColor;
            flashTimer = flashSeconds;
        }

        if (currentHp <= 0f)
        {
            currentHp = 0f;
            GameManager.Instance?.GameOver();
        }
    }
}
