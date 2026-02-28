using System.Collections.Generic;
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

    private bool choosing;
    private readonly List<UpgradeType> currentChoices = new();

    private enum UpgradeType
    {
        Damage,
        MaxHP,
        MoveSpeed,
        MultiShot,
        RuneRing
    }

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
        if (player == null || choosing) return;

        BuildChoices();
        if (currentChoices.Count == 0) return;

        choosing = true;
        Time.timeScale = 0f;
    }

    private void BuildChoices()
    {
        currentChoices.Clear();

        List<UpgradeType> pool = new()
        {
            UpgradeType.Damage,
            UpgradeType.MaxHP,
            UpgradeType.MoveSpeed,
            UpgradeType.MultiShot,
            UpgradeType.RuneRing
        };

        // Remove unavailable upgrades.
        if (autoAttack == null) pool.Remove(UpgradeType.MultiShot);
        if (runeRing == null) pool.Remove(UpgradeType.RuneRing);

        int picks = Mathf.Min(3, pool.Count);
        for (int i = 0; i < picks; i++)
        {
            int index = Random.Range(0, pool.Count);
            currentChoices.Add(pool[index]);
            pool.RemoveAt(index);
        }
    }

    private void ApplyUpgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                player.damage += damagePerLevel;
                Debug.Log("Upgrade: Damage+");
                break;
            case UpgradeType.MaxHP:
                player.maxHp += hpPerLevel;
                player.currentHp += hpPerLevel;
                Debug.Log("Upgrade: MaxHP+");
                break;
            case UpgradeType.MoveSpeed:
                player.moveSpeed += moveSpeedPerLevel;
                Debug.Log("Upgrade: MoveSpeed+");
                break;
            case UpgradeType.MultiShot:
                autoAttack?.AddMultiShot(1);
                break;
            case UpgradeType.RuneRing:
                runeRing?.UnlockOrUpgrade();
                break;
        }
    }

    private string GetTitle(UpgradeType type)
    {
        return type switch
        {
            UpgradeType.Damage => "Damage+",
            UpgradeType.MaxHP => "Max HP+",
            UpgradeType.MoveSpeed => "Move Speed+",
            UpgradeType.MultiShot => "Multi-Shot+",
            UpgradeType.RuneRing => "Rune Ring+",
            _ => "Upgrade"
        };
    }

    private string GetDescription(UpgradeType type)
    {
        return type switch
        {
            UpgradeType.Damage => $"+{damagePerLevel:0.0} attack damage",
            UpgradeType.MaxHP => $"+{hpPerLevel:0} max health (and heal)",
            UpgradeType.MoveSpeed => $"+{moveSpeedPerLevel:0.00} move speed",
            UpgradeType.MultiShot => "Fire one extra projectile per attack",
            UpgradeType.RuneRing => "Unlock/upgrade orbiting rune blades",
            _ => ""
        };
    }

    private void Choose(UpgradeType type)
    {
        ApplyUpgrade(type);
        choosing = false;
        currentChoices.Clear();
        Time.timeScale = 1f;
    }

    private void OnGUI()
    {
        if (!choosing || currentChoices.Count == 0) return;

        float w = Screen.width;
        float h = Screen.height;

        // Dim background
        Color prev = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, 0.75f);
        GUI.DrawTexture(new Rect(0, 0, w, h), Texture2D.whiteTexture);
        GUI.color = prev;

        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.RoundToInt(h * 0.038f),
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        GUI.Label(new Rect(0, h * 0.12f, w, 50), "Choose an Upgrade", titleStyle);

        float cardW = w * 0.26f;
        float cardH = h * 0.32f;
        float gap = w * 0.03f;
        float total = cardW * 3f + gap * 2f;
        float startX = (w - total) * 0.5f;
        float y = h * 0.32f;

        for (int i = 0; i < currentChoices.Count; i++)
        {
            UpgradeType choice = currentChoices[i];
            Rect r = new Rect(startX + i * (cardW + gap), y, cardW, cardH);

            GUI.Box(r, "");

            GUIStyle cardTitle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperCenter,
                fontSize = Mathf.RoundToInt(h * 0.028f),
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };

            GUIStyle cardDesc = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperCenter,
                fontSize = Mathf.RoundToInt(h * 0.021f),
                wordWrap = true
            };

            GUI.Label(new Rect(r.x + 10, r.y + 14, r.width - 20, 48), GetTitle(choice), cardTitle);
            GUI.Label(new Rect(r.x + 14, r.y + 70, r.width - 28, 90), GetDescription(choice), cardDesc);

            if (GUI.Button(new Rect(r.x + 18, r.y + r.height - 52, r.width - 36, 34), "Pick"))
            {
                Choose(choice);
            }
        }
    }
}
