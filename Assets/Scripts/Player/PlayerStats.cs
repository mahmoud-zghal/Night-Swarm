using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;
    public float moveSpeed = 5f;
    public float damage = 10f;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        currentHp -= amount;
        if (currentHp <= 0f)
        {
            currentHp = 0f;
            GameManager.Instance?.GameOver();
        }
    }
}
