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

    [Header("Stele WIN")]
    public int score3Stars = 30; // puncte pentru 3 stele
    public int score2Stars = 20; // puncte pentru 2 stele

    bool gameEnded = false;

    void Awake()
    {
        instance = this;
        currentLives = lives.Length;
    }

    void Update()
    {
        if (gameEnded) return;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = Mathf.FloorToInt(timeLeft % 60f);

            timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            // WIN instant dacă atingi scor maxim
            if (items >= score3Stars)
                WinGame();
        }
        else
        {
            // timp expirat
            WinGame();
        }
    }

    public void AddItem()
    {
        if (gameEnded) return;

        items++;
        itemsText.text = "pts: " + items;

        // WIN instant dacă atingi scor maxim
        if (items >= score3Stars)
            WinGame();
    }

    public void LoseLife()
    {
        if (gameEnded) return;
        if (currentLives <= 0) return;

        currentLives--;
        if (currentLives >= 0 && currentLives < lives.Length)
            lives[currentLives].SetActive(false);

        if (currentLives == 0)
        {
            // apel WinPanel chiar dacă toate inimile s-au pierdut
            WinGame();
        }
    }


    void GameOver()
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0f;
    }

    void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("WIN! Scor: " + items);

        // Redă sunetul de win
        AudioMngGame2.instance.PlayWin();

        // Afișează Win Panel și stelele
        WinGame2.Instance.ShowWin();

        Time.timeScale = 0f;
    }


}
