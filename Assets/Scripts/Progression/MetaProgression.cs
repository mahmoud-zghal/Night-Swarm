using UnityEngine;

public static class MetaProgression
{
    private const string CoinsKey = "meta.coins";
    private const string HpLvlKey = "meta.hp_lvl";
    private const string DmgLvlKey = "meta.dmg_lvl";
    private const string SpeedLvlKey = "meta.speed_lvl";

    public static int Coins
    {
        get => PlayerPrefs.GetInt(CoinsKey, 0);
        private set => PlayerPrefs.SetInt(CoinsKey, Mathf.Max(0, value));
    }

    public static int HpLevel
    {
        get => PlayerPrefs.GetInt(HpLvlKey, 0);
        private set => PlayerPrefs.SetInt(HpLvlKey, Mathf.Max(0, value));
    }

    public static int DamageLevel
    {
        get => PlayerPrefs.GetInt(DmgLvlKey, 0);
        private set => PlayerPrefs.SetInt(DmgLvlKey, Mathf.Max(0, value));
    }

    public static int SpeedLevel
    {
        get => PlayerPrefs.GetInt(SpeedLvlKey, 0);
        private set => PlayerPrefs.SetInt(SpeedLvlKey, Mathf.Max(0, value));
    }

    public static void AddCoins(int amount)
    {
        Coins += Mathf.Max(0, amount);
        PlayerPrefs.Save();
    }

    public static int CostForLevel(int currentLevel)
    {
        return 20 + currentLevel * 15;
    }

    public static bool TryUpgradeHP()
    {
        int cost = CostForLevel(HpLevel);
        if (Coins < cost) return false;
        Coins -= cost;
        HpLevel += 1;
        PlayerPrefs.Save();
        return true;
    }

    public static bool TryUpgradeDamage()
    {
        int cost = CostForLevel(DamageLevel);
        if (Coins < cost) return false;
        Coins -= cost;
        DamageLevel += 1;
        PlayerPrefs.Save();
        return true;
    }

    public static bool TryUpgradeSpeed()
    {
        int cost = CostForLevel(SpeedLevel);
        if (Coins < cost) return false;
        Coins -= cost;
        SpeedLevel += 1;
        PlayerPrefs.Save();
        return true;
    }

    public static void ApplyTo(PlayerStats player)
    {
        if (player == null) return;

        player.maxHp += HpLevel * 8f;
        player.currentHp = player.maxHp;
        player.damage += DamageLevel * 1.5f;
        player.moveSpeed += SpeedLevel * 0.12f;
    }
}
