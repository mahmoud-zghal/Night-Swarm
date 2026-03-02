using UnityEngine;

public class MobileRuntimeSettings : MonoBehaviour
{
    [Header("Orientation")]
    public bool forceLandscape = true;

    [Header("Performance")]
    public int targetFrameRate = 60;
    public bool runInBackground = false;

    private void Awake()
    {
        if (forceLandscape)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
        }

        Application.targetFrameRate = targetFrameRate;
        Application.runInBackground = runInBackground;
        QualitySettings.vSyncCount = 0;
    }
}
