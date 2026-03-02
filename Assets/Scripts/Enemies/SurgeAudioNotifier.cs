using UnityEngine;

public class SurgeAudioNotifier : MonoBehaviour
{
    public WaveDirector waveDirector;
    private bool wasSurge;

    private void Start()
    {
        if (waveDirector == null)
            waveDirector = GetComponent<WaveDirector>();

        if (waveDirector == null)
            waveDirector = FindObjectOfType<WaveDirector>();
    }

    private void Update()
    {
        if (waveDirector == null) return;

        bool isSurge = waveDirector.IsSpikeActive;
        if (isSurge && !wasSurge)
            AudioManager.Instance?.PlaySurge();

        wasSurge = isSurge;
    }
}
