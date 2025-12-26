using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject[] stars; // 3 stele

    public static WinPanelManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        winPanel.SetActive(false);
    }

    // === APELAT CÂND PUZZLE-UL E TERMINAT ===
    public void ShowWin(float time)
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;

        // afișare timp
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timeText.text = $"{minutes:00}:{seconds:00}";

        // dezactivăm toate stelele
        foreach (var star in stars)
            star.SetActive(false);

        int starCount = 0;

        if (time < 30f) starCount = 3;
        else if (time < 60f) starCount = 2;
        else if (time < 400f) starCount = 1;
        else starCount = 0;

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
