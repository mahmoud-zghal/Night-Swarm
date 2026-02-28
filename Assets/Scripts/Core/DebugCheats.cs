using UnityEngine;

public class DebugCheats : MonoBehaviour
{
    [Header("Hotkeys")]
    public KeyCode killPlayerKey = KeyCode.K;
    public KeyCode addCoinsKey = KeyCode.C;
    public KeyCode levelUpKey = KeyCode.L;

    [Header("Values")]
    public int coinsPerPress = 100;

    private PlayerStats player;

    private void Start()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            player = playerGO.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(killPlayerKey))
        {
            if (player != null)
            {
                player.TakeDamage(99999f);
                Debug.Log("[Cheat] Kill player");
            }
        }

        if (Input.GetKeyDown(addCoinsKey))
        {
            MetaProgression.AddCoins(coinsPerPress);
            Debug.Log($"[Cheat] +{coinsPerPress} coins | total: {MetaProgression.Coins}");
        }

        if (Input.GetKeyDown(levelUpKey))
        {
            XPSystem.Instance?.AddXP(99999f);
            Debug.Log("[Cheat] Force level up");
        }
    }
}
