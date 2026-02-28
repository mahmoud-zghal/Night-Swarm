using UnityEngine;

public class WaveDirector : MonoBehaviour
{
    [Header("Base pacing")]
    public float baseSpawnInterval = 1.2f;
    public float minSpawnInterval = 0.25f;
    public float difficultyRampPerMinute = 0.15f;

    [Header("Intensity spikes")]
    public float spikeEverySeconds = 45f;
    public float spikeDurationSeconds = 10f;
    [Range(1f, 4f)] public float spikeSpawnMultiplier = 1.8f;

    [Header("Burst control")]
    [Range(1, 5)] public int baseBurstCount = 1;
    [Range(1, 8)] public int maxBurstCount = 4;

    public bool IsSpikeActive { get; private set; }

    public float GetCurrentInterval(float runTimeSeconds)
    {
        float minutes = runTimeSeconds / 60f;
        float interval = Mathf.Max(minSpawnInterval, baseSpawnInterval - minutes * difficultyRampPerMinute);

        if (IsSpike(runTimeSeconds))
        {
            IsSpikeActive = true;
            interval /= Mathf.Max(1f, spikeSpawnMultiplier);
        }
        else
        {
            IsSpikeActive = false;
        }

        return interval;
    }

    public int GetCurrentBurstCount(float runTimeSeconds)
    {
        int progressive = baseBurstCount + Mathf.FloorToInt(runTimeSeconds / 75f);
        int burst = Mathf.Clamp(progressive, 1, maxBurstCount);

        if (IsSpike(runTimeSeconds))
            burst = Mathf.Min(maxBurstCount, burst + 1);

        return burst;
    }

    private bool IsSpike(float runTimeSeconds)
    {
        if (spikeEverySeconds <= 0f || spikeDurationSeconds <= 0f) return false;

        float t = runTimeSeconds % spikeEverySeconds;
        return t <= spikeDurationSeconds;
    }
}
