using UnityEngine;

public class XPSystem : MonoBehaviour
{
    public static XPSystem Instance;

    public int level = 1;
    public float currentXP;
    public float xpToNext = 30f;
    public float growthFactor = 1.25f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddXP(float amount)
    {
        currentXP += amount;
        while (currentXP >= xpToNext)
        {
            currentXP -= xpToNext;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        xpToNext *= growthFactor;
        LevelUpSystem.Instance?.OfferUpgrades();
        Debug.Log($"Level Up! -> {level}");
    }
}
