using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeHubController : MonoBehaviour
{
    public string menuSceneName = "MainMenu";
    public string gameSceneName = "Game";

    private string status = "";

    private void OnGUI()
    {
        float w = Screen.width;
        float h = Screen.height;

        float panelW = Mathf.Min(640f, w * 0.92f);
        float panelH = Mathf.Min(560f, h * 0.9f);
        Rect panel = new Rect((w - panelW) * 0.5f, (h - panelH) * 0.5f, panelW, panelH);

        GUI.Box(panel, "");

        GUIStyle title = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = Mathf.RoundToInt(h * 0.042f)
        };

        GUIStyle line = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.RoundToInt(h * 0.023f)
        };

        GUI.Label(new Rect(panel.x, panel.y + 18, panel.width, 50), "Upgrade Hub", title);
        GUI.Label(new Rect(panel.x, panel.y + 64, panel.width, 30), $"Coins: {MetaProgression.Coins}", line);

        float btnW = panel.width - 100;
        float x = panel.x + 50;
        float y = panel.y + 120;

        DrawUpgradeButton(
            new Rect(x, y, btnW, 46),
            $"Max HP Lv {MetaProgression.HpLevel}  (Cost: {MetaProgression.CostForLevel(MetaProgression.HpLevel)})",
            MetaProgression.TryUpgradeHP
        );

        DrawUpgradeButton(
            new Rect(x, y + 58, btnW, 46),
            $"Damage Lv {MetaProgression.DamageLevel}  (Cost: {MetaProgression.CostForLevel(MetaProgression.DamageLevel)})",
            MetaProgression.TryUpgradeDamage
        );

        DrawUpgradeButton(
            new Rect(x, y + 116, btnW, 46),
            $"Move Speed Lv {MetaProgression.SpeedLevel}  (Cost: {MetaProgression.CostForLevel(MetaProgression.SpeedLevel)})",
            MetaProgression.TryUpgradeSpeed
        );

        GUI.Label(new Rect(panel.x + 20, y + 180, panel.width - 40, 30), status, line);

        if (GUI.Button(new Rect(x, y + 230, btnW, 44), "Back to Menu"))
            SceneManager.LoadScene(menuSceneName);

        if (GUI.Button(new Rect(x, y + 282, btnW, 44), "Play"))
            SceneManager.LoadScene(gameSceneName);
    }

    private void DrawUpgradeButton(Rect rect, string label, System.Func<bool> buyFn)
    {
        if (GUI.Button(rect, label))
        {
            bool ok = buyFn();
            status = ok ? "Upgrade purchased" : "Not enough coins";
        }
    }
}
