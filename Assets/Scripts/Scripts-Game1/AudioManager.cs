using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Puzzle Sounds")]
    [SerializeField] private AudioClip movePiece;
    [SerializeField] private AudioClip win;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip buttonClick;

    private AudioSource source;

    // 🔊 VOLUME INDIVIDUAL
    [Header("---------------")]
    [SerializeField] private float moveVolume = 1f;
    [SerializeField] private float winVolume = 1f;
    [SerializeField] private float uiVolume = 1f;

    private bool muted = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
    }

    // ================= SLIDERS =================

    public void SetMoveVolume(float value)
    {
        moveVolume = Mathf.Log10(value) * 20;
    }

    public void SetWinVolume(float value)
    {
        winVolume = Mathf.Log10(value) * 20;
    }

    public void SetUIVolume(float value)
    {
        uiVolume = Mathf.Log10(value) * 20;
    }

    // ================= MUTE =================

    public void ToggleMute()
    {
        muted = !muted;
    }

    // ================= PLAY =================

    public void PlayMove()
    {
        if (!muted && movePiece != null)
            source.PlayOneShot(movePiece, moveVolume);
    }

    public void PlayWin()
    {
        if (!muted && win != null)
            source.PlayOneShot(win, winVolume);
    }

    public void PlayButton()
    {
        if (!muted && buttonClick != null)
            source.PlayOneShot(buttonClick, uiVolume);
    }
}
