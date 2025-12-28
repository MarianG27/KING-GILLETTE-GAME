using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] playlist;

    [Header("Scene Control")]
    [SerializeField] private int scenesAllowed = 5; // scena curentă + 4

    private int startSceneIndex;
    private int currentTrackIndex = 0;

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

        startSceneIndex = SceneManager.GetActiveScene().buildIndex;

        musicSource.loop = false;
        musicSource.playOnAwake = false;
        musicSource.volume = lastVolume;

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (playlist.Length > 0)
            PlayCurrentTrack();
    }

    void Update()
    {
        if (!musicSource.isPlaying && !muted && playlist.Length > 0)
            NextTrack();
    }

    // 🎬 SCENE CHECK
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int maxAllowedIndex = startSceneIndex + scenesAllowed - 1;

        if (scene.buildIndex > maxAllowedIndex)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }

    // ▶️ PLAY
    private void PlayCurrentTrack()
    {
        musicSource.clip = playlist[currentTrackIndex];
        musicSource.Play();
    }

    // ⏭️ NEXT
    private void NextTrack()
    {
        currentTrackIndex++;
        if (currentTrackIndex >= playlist.Length)
            currentTrackIndex = 0;

        PlayCurrentTrack();
    }

    // 🎚️ SLIDER
    public void SetVolume(float value)
    {
        lastVolume = value;
        if (!muted)
            musicSource.volume = value;
    }

    // 🔇 MUTE
    public void ToggleMute()
    {
        muted = !muted;
        musicSource.volume = muted ? 0f : lastVolume;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
