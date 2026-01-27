using UnityEngine;
using TMPro;

public class game : MonoBehaviour
{
    public static game instance;

    public int items = 0;
    public float timeLeft = 60f;

    public TMP_Text itemsText;
    public TMP_Text timeText;

    public GameObject[] lives;

    int currentLives;

    // 🔊 AUDIO FX
    public AudioSource sfxSource;
    public AudioClip loseLifeSound;
    public AudioClip goodItemSound;
    public AudioClip bombSound;

    // 🎵 BG MUSIC
    public AudioSource bgMusic;

    void Awake()
    {
        instance = this;
        currentLives = lives.Length;
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = Mathf.FloorToInt(timeLeft % 60f);

            timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    public void AddItem()
    {
        items++;
        itemsText.text = "pts: " + items;
        sfxSource.PlayOneShot(goodItemSound);
    }

    public void LoseLife()
    {
        if (currentLives <= 0) return;

        currentLives--;
        lives[currentLives].SetActive(false);

        sfxSource.PlayOneShot(loseLifeSound);

        if (currentLives == 0)
        {
            GameOver();
        }
    }

    public void HitBomb()
    {
        sfxSource.PlayOneShot(bombSound);
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        bgMusic.Stop();      // ⛔ oprește muzica
        Time.timeScale = 0f;
    }
}
