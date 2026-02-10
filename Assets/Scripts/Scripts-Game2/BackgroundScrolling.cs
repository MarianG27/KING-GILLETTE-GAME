using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    private float startPos;
    private Transform cam;
    public float parallaxEffect;

    void Start()
    {
        cam = Camera.main.transform;
        startPos = transform.position.x;
    }

    void FixedUpdate()
    {
        float distance = cam.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
