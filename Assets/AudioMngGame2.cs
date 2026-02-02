using UnityEngine;

public class AudioMngGame2 : MonoBehaviour
{
    public static AudioMngGame2 instance;

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip GoodItem;
    public AudioClip BadItem;
    public AudioClip Background;
    public AudioClip Click;
    public AudioClip Win;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayMusic();
    }

    // 🎵 MUSIC
    public void PlayMusic()
    {
        MusicSource.clip = Background;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    // 🔊 SFX
    public void PlayGoodItem()
    {
        SFXSource.PlayOneShot(GoodItem);
    }

    public void PlayBadItem()
    {
        SFXSource.PlayOneShot(BadItem);
    }

    public void PlayClick()
    {
        SFXSource.PlayOneShot(Click);
    }

    public void PlayWin()
    {
        SFXSource.PlayOneShot(Win);
    }
}
