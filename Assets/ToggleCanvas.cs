using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    public GameObject canvasUI;

    public void Toggle()
    {
        if (canvasUI == null) return;

        canvasUI.SetActive(!canvasUI.activeSelf);
    }
}
