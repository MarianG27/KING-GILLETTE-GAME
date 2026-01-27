using UnityEngine;

public class generator : MonoBehaviour
{
    public GameObject[] gm;
    public float spawnY = 6f;
    public float spawnDelay = 0.7f;

    float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            float posX = Random.Range(-4f, 4f);
            int chance = Random.Range(1, 101);

            // 💣 20% șansă bombă
            if (chance <= 40)
            {
                Instantiate(gm[0], new Vector3(posX, spawnY, 0), Quaternion.identity);
            }
            else
            {
                // 🎁 obiect bun random (1 → ultimul)
                int goodIndex = Random.Range(1, gm.Length);
                Instantiate(gm[goodIndex], new Vector3(posX, spawnY, 0), Quaternion.identity);
            }

            timer = spawnDelay;
        }
    }
}
