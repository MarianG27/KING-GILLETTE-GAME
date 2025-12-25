using UnityEngine;

public class PuzzleSettings : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameManager1 gameManager;

    public void OpenPanel()
    {
        panel.SetActive(true);
        gameManager.PausePuzzle();
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        gameManager.ResumePuzzle();
    }
}
