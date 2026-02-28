using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public static LevelUpSystem Instance;

    [Header("Simple stat increments")]
    public float damagePerLevel = 1.5f;
    public float hpPerLevel = 5f;
    public float moveSpeedPerLevel = 0.1f;

    private PlayerStats player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null) player = playerGO.GetComponent<PlayerStats>();
    }

    public void OfferUpgrades()
    {
        // Placeholder: random auto-pick for MVP.
        // Replace with UI panel offering 3 choices.
        if (player == null) return;

        int choice = Random.Range(0, 3);
        switch (choice)
        {
            case 0:
                player.damage += damagePerLevel;
                Debug.Log("Upgrade: Damage+");
                break;
            case 1:
                player.maxHp += hpPerLevel;
                player.currentHp += hpPerLevel;
                Debug.Log("Upgrade: MaxHP+");
                break;
            default:
                player.moveSpeed += moveSpeedPerLevel;
                Debug.Log("Upgrade: MoveSpeed+");
                break;
        }
    }
}
