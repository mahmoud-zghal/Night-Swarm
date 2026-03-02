using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    [Range(0f, 1f)] public float sfxVolume = 0.85f;

    [Header("Clips")]
    public AudioClip hitClip;
    public AudioClip enemyDeathClip;
    public AudioClip levelUpClip;
    public AudioClip uiClickClip;
    public AudioClip surgeClip;

    private AudioSource source;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0f;
    }

    public void PlayHit() => Play(hitClip, 0.9f);
    public void PlayEnemyDeath() => Play(enemyDeathClip, 0.85f);
    public void PlayLevelUp() => Play(levelUpClip, 1f);
    public void PlayUIClick() => Play(uiClickClip, 0.8f);
    public void PlaySurge() => Play(surgeClip, 1f);

    private void Play(AudioClip clip, float scale)
    {
        if (clip == null || source == null) return;
        source.PlayOneShot(clip, sfxVolume * scale);
    }
}
