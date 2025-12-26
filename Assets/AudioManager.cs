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
    }

    public void PlayMove()
    {
        source.PlayOneShot(movePiece);
    }

    public void PlayWin()
    {
        source.PlayOneShot(win);
    }

    public void PlayButton()
    {
        source.PlayOneShot(buttonClick);
    }
}
