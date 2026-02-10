using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class CardsController : MonoBehaviour
{
    [Header("Cards")]
    [SerializeField] Sprite[] sprites;
    [SerializeField] Card cardPrefab;
    [SerializeField] Transform gridTransform;

    [Header("Win UI")]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject[] stars; // 3 stele
    [SerializeField] TMP_Text movesText;
    [SerializeField] TMP_Text winMovesText;

    [Header("Stars based on moves")]
    [SerializeField] int threeStarLimit = 8;
    [SerializeField] int twoStarLimit = 12;

    [Header("Audio")]
    [SerializeField] AudioMngGame2 audioMng;

    List<Sprite> spritePairs;

    Card firstSelected;
    Card secondSelected;

    bool isChecking;
    int moves;
    int matchedPairs;

    void Start()
    {
        winPanel.SetActive(false);
        PrepareSprites();
        CreateCards();
        UpdateMovesUI();
    }

    void PrepareSprites()
    {
        spritePairs = new List<Sprite>();

        foreach (var s in sprites)
        {
            spritePairs.Add(s);
            spritePairs.Add(s);
        }

        Shuffle(spritePairs);
    }

    void CreateCards()
    {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.HideInstant();
            card.controller = this;
        }
    }

    public void SetSelected(Card card)
    {
        if (isChecking) return;
        if (card.isSelected) return;

        card.Show();
        audioMng.PlayFlip(); // 🔊 flip sound

        if (firstSelected == null)
        {
            firstSelected = card;
        }
        else
        {
            secondSelected = card;
            moves++;
            UpdateMovesUI();
            isChecking = true;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.6f);

        if (firstSelected.iconSprite != secondSelected.iconSprite)
        {
            firstSelected.Hide();
            secondSelected.Hide();
        }
        else
        {
            matchedPairs++;
            audioMng.PlayMatch(); // 🔊 match sound

            if (matchedPairs == sprites.Length)
                WinGame();
        }

        firstSelected = null;
        secondSelected = null;
        isChecking = false;
    }

    void WinGame()
    {
        winPanel.SetActive(true);
        winMovesText.text = "" + moves;
        SetStars();
        audioMng.PlayWin(); // 🔊 win sound
    }

    void SetStars()
    {
        foreach (var s in stars)
            s.SetActive(true);

        if (moves > threeStarLimit)
            stars[2].SetActive(false);

        if (moves > twoStarLimit)
            stars[1].SetActive(false);

        if (moves > twoStarLimit + 4)
            stars[0].SetActive(false);
    }

    void UpdateMovesUI()
    {
        movesText.text = "mv:" + moves;
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            Sprite t = list[i];
            list[i] = list[r];
            list[r] = t;
        }
    }
    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }   
}
