using UnityEngine;

public class RuntimeHUD : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;

    private PlayerStats player;
    private XPSystem xp;
    private bool paused;

    private void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.GetComponent<PlayerStats>();
        xp = XPSystem.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
            TogglePause();
    }

    private void TogglePause()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    private void OnGUI()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        float w = Screen.width;
        float h = Screen.height;

        GUIStyle label = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.RoundToInt(h * 0.024f),
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        // HP
        float barW = w * 0.32f;
        float barH = h * 0.03f;
        Rect hpBg = new Rect(w * 0.03f, h * 0.03f, barW, barH);
        GUI.Box(hpBg, "");
        float hp01 = (player != null && player.maxHp > 0f) ? Mathf.Clamp01(player.currentHp / player.maxHp) : 0f;
        Color prev = GUI.color;
        GUI.color = new Color(0.95f, 0.25f, 0.35f, 1f);
        GUI.DrawTexture(new Rect(hpBg.x + 2, hpBg.y + 2, (hpBg.width - 4) * hp01, hpBg.height - 4), Texture2D.whiteTexture);
        GUI.color = prev;
        GUI.Label(new Rect(hpBg.x + 8, hpBg.y - 2, hpBg.width, hpBg.height), $"HP {(player != null ? player.currentHp : 0):0}/{(player != null ? player.maxHp : 0):0}", label);

        // XP
        float xpY = hpBg.y + barH + 8;
        Rect xpBg = new Rect(w * 0.03f, xpY, barW, barH);
        GUI.Box(xpBg, "");
        float xp01 = (xp != null && xp.xpToNext > 0f) ? Mathf.Clamp01(xp.currentXP / xp.xpToNext) : 0f;
        GUI.color = new Color(0.25f, 0.75f, 1f, 1f);
        GUI.DrawTexture(new Rect(xpBg.x + 2, xpBg.y + 2, (xpBg.width - 4) * xp01, xpBg.height - 4), Texture2D.whiteTexture);
        GUI.color = prev;
        GUI.Label(new Rect(xpBg.x + 8, xpBg.y - 2, xpBg.width, xpBg.height), $"LVL {(xp != null ? xp.level : 1)}", label);

        // Timer
        float t = GameManager.Instance != null ? GameManager.Instance.RunTime : 0f;
        GUI.Label(new Rect(w * 0.42f, h * 0.03f, w * 0.2f, 32f), $"Time {t:0.0}s", label);

        // Pause button (mobile-friendly)
        float btnW = w * 0.14f;
        float btnH = h * 0.06f;
        Rect pauseRect = new Rect(w - btnW - w * 0.03f, h * 0.03f, btnW, btnH);
        if (GUI.Button(pauseRect, paused ? "Resume" : "Pause"))
            TogglePause();

        if (paused)
        {
            GUIStyle pauseStyle = new GUIStyle(label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.RoundToInt(h * 0.04f)
            };
            GUI.Label(new Rect(0, h * 0.44f, w, 50), "Paused", pauseStyle);
        }
    }
}
