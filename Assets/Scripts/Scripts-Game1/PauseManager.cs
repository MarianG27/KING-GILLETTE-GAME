using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [SerializeField] private GameObject optionsPanel;


    public bool IsPaused { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        optionsPanel.SetActive(false);
        IsPaused = false;
    }

    // === OPEN PAUSE ===
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    // === RESUME ===
    public void Resume()
    {
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    // === RESTART ===
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // === MAIN MENU ===
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // === LevelsMenu ===
    public void LevelsMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelsMenu");
    }

}
