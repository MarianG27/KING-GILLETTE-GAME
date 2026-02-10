using UnityEngine;

public class Basket : MonoBehaviour
{
    public float speed = 6f;

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * move * speed * Time.deltaTime;
    }
}
