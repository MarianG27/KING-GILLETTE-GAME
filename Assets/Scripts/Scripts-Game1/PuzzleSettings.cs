using UnityEngine;

public class PanelController : MonoBehaviour
{
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
    }

    public void ClosePanel()
    {
        shadowPanel.SetActive(false);
        mainPanel.SetActive(false);
    }
}
