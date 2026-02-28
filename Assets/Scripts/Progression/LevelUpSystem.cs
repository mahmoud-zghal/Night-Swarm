using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public static LevelUpSystem Instance;

    [Header("Simple stat increments")]
    public float damagePerLevel = 1.5f;
    public float hpPerLevel = 5f;
    public float moveSpeedPerLevel = 0.1f;

    private PlayerStats player;
    private AutoAttack autoAttack;
    private RuneRing runeRing;

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
        if (playerGO != null)
        {
            player = playerGO.GetComponent<PlayerStats>();
            autoAttack = playerGO.GetComponent<AutoAttack>();
            runeRing = playerGO.GetComponent<RuneRing>();
        }
    }

    public void OfferUpgrades()
    {
        // Placeholder random picks for MVP.
        // Later replace with 3-choice UI panel.
        if (player == null) return;

        int choice = Random.Range(0, 5);
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
            case 2:
                player.moveSpeed += moveSpeedPerLevel;
                Debug.Log("Upgrade: MoveSpeed+");
                break;
            case 3:
                if (autoAttack != null) autoAttack.AddMultiShot(1);
                break;
            default:
                if (runeRing != null) runeRing.UnlockOrUpgrade();
                break;
        }
    }
}
