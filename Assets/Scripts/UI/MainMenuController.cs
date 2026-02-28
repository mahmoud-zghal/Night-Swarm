using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene names")]
    public string gameSceneName = "Game";
    public string upgradeSceneName = "UpgradeHub";

    private void OnGUI()
    {
        float w = Screen.width;
        float h = Screen.height;

        float panelW = Mathf.Min(560f, w * 0.9f);
        float panelH = Mathf.Min(440f, h * 0.8f);
        Rect panel = new Rect((w - panelW) * 0.5f, (h - panelH) * 0.5f, panelW, panelH);

        GUI.Box(panel, "");

        GUIStyle title = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = Mathf.RoundToInt(h * 0.05f)
        };

        GUIStyle line = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.RoundToInt(h * 0.024f)
        };

        GUI.Label(new Rect(panel.x, panel.y + 18, panel.width, 60), "Night Swarm", title);
        GUI.Label(new Rect(panel.x, panel.y + 72, panel.width, 30), $"Coins: {MetaProgression.Coins}", line);

        float btnW = panel.width - 90;
        float x = panel.x + 45;
        float y = panel.y + 130;

        if (GUI.Button(new Rect(x, y, btnW, 46), "Play"))
            SceneManager.LoadScene(gameSceneName);

        if (GUI.Button(new Rect(x, y + 58, btnW, 46), "Upgrades"))
            SceneManager.LoadScene(upgradeSceneName);

        if (GUI.Button(new Rect(x, y + 116, btnW, 46), "Quit"))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
