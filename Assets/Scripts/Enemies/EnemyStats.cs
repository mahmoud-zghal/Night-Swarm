using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public enum Archetype
    {
        Basic,
        Shard,
        Warden,
        Echo
    }

    [Header("Identity")]
    public Archetype archetype = Archetype.Basic;

    [Header("Stats")]
    public float maxHp = 20f;
    public float currentHp;
    public float moveSpeed = 2.5f;
    public float touchDamage = 8f;
    public float xpReward = 5f;

    private void Awake()
    {
        currentHp = maxHp;
        ApplyArchetypeTint();
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        if (currentHp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        XPSystem.Instance?.AddXP(xpReward);
        GameManager.Instance?.RegisterKill();
        Destroy(gameObject);
    }

    private void ApplyArchetypeTint()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        switch (archetype)
        {
            case Archetype.Shard:
                sr.color = new Color(0.35f, 0.9f, 1f, 1f);
                break;
            case Archetype.Warden:
                sr.color = new Color(0.62f, 0.48f, 1f, 1f);
                break;
            case Archetype.Echo:
                sr.color = new Color(1f, 0.40f, 0.75f, 0.95f);
                break;
            default:
                sr.color = Color.white;
                break;
        }
    }
}
