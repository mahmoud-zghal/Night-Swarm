using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float maxHp = 20f;
    public float currentHp;
    public float moveSpeed = 2.5f;
    public float touchDamage = 8f;
    public float xpReward = 5f;

    private void Awake()
    {
        currentHp = maxHp;
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
}
