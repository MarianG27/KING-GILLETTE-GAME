using UnityEngine;
using TMPro;

public class game : MonoBehaviour
{
    public static game instance;

    public int items = 0;
    public float timeLeft = 60f;

    public TMP_Text itemsText;
    public TMP_Text timeText;

    public GameObject[] lives; // ❤️ asseturile din scenă (puse de TINE)

    int currentLives;

    void Awake()
    {
        instance = this;
        currentLives = lives.Length;
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.Ceil(timeLeft);
        }
    }

    public void AddItem()
    {
        items++;
        itemsText.text = "Items: " + items;
    }

    public void LoseLife()
    {
        if (currentLives <= 0) return;

        currentLives--;
        lives[currentLives].SetActive(false); // ❌ dispare o inimă

        if (currentLives == 0)
        {
            Debug.Log("GAME OVER");
            Time.timeScale = 0f;
        }
    }
}
