using UnityEngine;

public class PuzzleSettings : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    // APELAT de butonul OPEN
    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    // APELAT de butonul CLOSE (X)
    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
