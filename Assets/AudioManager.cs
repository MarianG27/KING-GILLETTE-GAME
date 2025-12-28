using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Puzzle Sounds")]
    public AudioClip movePiece;
    public AudioClip win;

    [Header("UI Sounds")]
    public AudioClip buttonClick;

    private AudioSource source;

    private bool muted = false;
    private float volume = 1f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
        source.volume = volume;
    }

    // 🎚️ SLIDER
    public void SetVolume(float value)
    {
        volume = value;
        if (!muted)
            source.volume = value;
    }

    // 🔇 MUTE BUTTON
    public void ToggleMute()
    {
        muted = !muted;

        if (muted)
            source.volume = 0f;
        else
            source.volume = volume;
    }

    public void PlayMove()
    {
        if (!muted)
            source.PlayOneShot(movePiece);
    }

    public void PlayWin()
    {
        if (!muted)
            source.PlayOneShot(win);
    }

    public void PlayButton()
    {
        if (!muted)
            source.PlayOneShot(buttonClick);
    }
}
