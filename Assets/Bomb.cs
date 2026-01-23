using UnityEngine;

public class Bomb : MonoBehaviour
{
    Transform tr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tr.position -= new Vector3(0f, 0.12f, 0f);

        if (tr.position.y < -7f) Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "basket")
        {
            game.instance.LoseLife();
            Destroy(gameObject);
        }
    }


}
