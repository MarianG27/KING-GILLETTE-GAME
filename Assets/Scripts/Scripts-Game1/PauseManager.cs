using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject soundSettingsPanel;

    public bool IsPaused { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        optionsPanel.SetActive(false);
        soundSettingsPanel.SetActive(false);
        IsPaused = false;
    }

    // ================= PAUSE =================
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        soundSettingsPanel.SetActive(false);

        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void Resume()
    {
        optionsPanel.SetActive(false);
        soundSettingsPanel.SetActive(false);

        Time.timeScale = 1f;
        IsPaused = false;
    }

    // ================= SOUND SETTINGS =================
    public void OpenSoundSettings()
    {
        optionsPanel.SetActive(false);
        soundSettingsPanel.SetActive(true);
    }

    public void BackToOptions()
    {
        soundSettingsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // ================= OTHER BUTTONS =================
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
