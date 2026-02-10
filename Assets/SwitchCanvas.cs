using UnityEngine;

public class SwitchCanvas : MonoBehaviour
{
    public GameObject canvasToClose;
    public GameObject canvasToOpen;

    public void Switch()
    {
        if (canvasToClose != null)
            canvasToClose.SetActive(false);

        if (canvasToOpen != null)
            canvasToOpen.SetActive(true);
    }
}
