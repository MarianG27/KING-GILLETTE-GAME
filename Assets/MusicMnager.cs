using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    private bool muted = false;
    private float lastVolume = 0.4f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = lastVolume;

        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // 🎚️ SLIDER
    public void SetVolume(float value)
    {
        lastVolume = value;
        if (!muted)
            musicSource.volume = value;
    }

    // 🔇 MUTE BUTTON
    public void ToggleMute()
    {
        muted = !muted;

        if (muted)
            musicSource.volume = 0f;
        else
            musicSource.volume = lastVolume;
    }

    public bool IsPlaying()
    {
        return musicSource.isPlaying;
    }
}
