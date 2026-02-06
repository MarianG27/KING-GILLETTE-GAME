using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinGame2 : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text timeText;   // folosim pentru scor
    [SerializeField] private GameObject[] stars;  // 3 stele ON/OFF

    public static WinGame2 Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        winPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // APELAT când jocul e câștigat
    public void ShowWin(float unusedTimeParameter = 0f)
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;

        // afișăm scorul
        timeText.text = "" + game.instance.items;

        // dezactivăm toate stelele
        foreach (var star in stars)
            star.SetActive(false);

        int starCount = 0; // 0 stele dacă nu ai puncte

        if (game.instance.items > 0)
        {
            if (game.instance.items >= game.instance.score3Stars)
                starCount = 3;
            else if (game.instance.items >= game.instance.score2Stars)
                starCount = 2;
            else
                starCount = 1;
        }

        // activăm stelele corespunzătoare
        for (int i = 0; i < starCount; i++)
            stars[i].SetActive(true);
    }


    // === BUTTONS ===
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
