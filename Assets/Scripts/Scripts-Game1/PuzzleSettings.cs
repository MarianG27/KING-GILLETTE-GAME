using UnityEngine;

public class PanelController : MonoBehaviour
{
<<<<<<< HEAD
    public GameObject shadowPanel;
    public GameObject mainPanel;

    void Start()
    {
        ClosePanel();
    }

    public void OpenPanel()
    {
        shadowPanel.SetActive(true);
        mainPanel.SetActive(true);
=======
    [SerializeField] private GameObject panel;
    [SerializeField] private GameManager gameManager;

    public void OpenPanel()
    {
        panel.SetActive(true);
        gameManager.PausePuzzle();
>>>>>>> 1-1-2026
    }

    public void ClosePanel()
    {
<<<<<<< HEAD
        shadowPanel.SetActive(false);
        mainPanel.SetActive(false);
=======
        panel.SetActive(false);
        gameManager.ResumePuzzle();
>>>>>>> 1-1-2026
    }
}
