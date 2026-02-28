using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGameOver { get; private set; }
    public float RunTime => Time.timeSinceLevelLoad;
    public int Kills { get; private set; }
    public int CoinsEarnedThisRun { get; private set; }

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
        if (playerGO != null)
        {
            player = playerGO.GetComponent<PlayerStats>();
            MetaProgression.ApplyTo(player);
        }
    }

    public void RegisterKill()
    {
        if (IsGameOver) return;
        Kills++;
    }

    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;

        int survivalCoins = Mathf.FloorToInt(RunTime * 0.9f);
        int killCoins = Kills;
        CoinsEarnedThisRun = survivalCoins + killCoins;
        MetaProgression.AddCoins(CoinsEarnedThisRun);

        Time.timeScale = 0f;
        Debug.Log($"Game Over | Kills: {Kills} | Coins +{CoinsEarnedThisRun} | Total: {MetaProgression.Coins}");
    }

    private void RestartRun()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGUI()
    {
        if (!IsGameOver) return;

        float w = Screen.width;
        float h = Screen.height;
        float panelW = Mathf.Min(520f, w * 0.9f);
        float panelH = Mathf.Min(500f, h * 0.85f);
        Rect r = new Rect((w - panelW) * 0.5f, (h - panelH) * 0.5f, panelW, panelH);

        GUI.Box(r, "");

        GUIStyle title = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.RoundToInt(h * 0.035f),
            fontStyle = FontStyle.Bold
        };

        GUIStyle line = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.RoundToInt(h * 0.024f)
        };

        GUI.Label(new Rect(r.x, r.y + 18, r.width, 44), "Run Ended", title);
        GUI.Label(new Rect(r.x, r.y + 72, r.width, 30), $"Time: {RunTime:0.0}s", line);
        GUI.Label(new Rect(r.x, r.y + 102, r.width, 30), $"Kills: {Kills}", line);
        GUI.Label(new Rect(r.x, r.y + 132, r.width, 30), $"Coins earned: +{CoinsEarnedThisRun}", line);
        GUI.Label(new Rect(r.x, r.y + 162, r.width, 30), $"Total coins: {MetaProgression.Coins}", line);

        float btnW = r.width - 80;
        float x = r.x + 40;
        float y = r.y + 210;
        int hpCost = MetaProgression.CostForLevel(MetaProgression.HpLevel);
        int dmgCost = MetaProgression.CostForLevel(MetaProgression.DamageLevel);
        int spdCost = MetaProgression.CostForLevel(MetaProgression.SpeedLevel);

        if (GUI.Button(new Rect(x, y, btnW, 38), $"Upgrade Max HP (Lv {MetaProgression.HpLevel}) - {hpCost}c"))
            MetaProgression.TryUpgradeHP();
        if (GUI.Button(new Rect(x, y + 46, btnW, 38), $"Upgrade Damage (Lv {MetaProgression.DamageLevel}) - {dmgCost}c"))
            MetaProgression.TryUpgradeDamage();
        if (GUI.Button(new Rect(x, y + 92, btnW, 38), $"Upgrade Move Speed (Lv {MetaProgression.SpeedLevel}) - {spdCost}c"))
            MetaProgression.TryUpgradeSpeed();

        if (GUI.Button(new Rect(x, y + 154, btnW, 42), "Play Again"))
            RestartRun();
    }
}
