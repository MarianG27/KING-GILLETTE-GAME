using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Puzzle")]
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;

    [Header("UI")]
    public TMP_Text timerText;

    [Header("Size Input")]
    public TMP_InputField sizeInputTMP;
    public InputField sizeInputLegacy;


    private List<Transform> pieces;
    private int emptyLocation;
    private int size = 3;
    private bool shuffling = false;

    // ===== TIMER =====
    private float timer = 0f;
    private bool timerRunning = false;
    private bool puzzleFinished = false;

    // ===== BLOCK INPUT =====
    public bool puzzleBlocked = false;

    void Start()
    {
        CreateNewPuzzle(size);
        UpdateTimerText();
    }

    void Update()
    {
        // ⛔ dacă puzzle-ul e blocat (panel deschis)
        if (puzzleBlocked)
            return;

        // ⏱️ update timer
        if (timerRunning && !puzzleFinished)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }

        // ✅ verificare final
        if (!shuffling && !puzzleFinished && CheckCompletion())
        {
            puzzleFinished = true;
            timerRunning = false; // ⛔ STOP TIMER
            Debug.Log("Puzzle terminat în " + timer + " secunde");
        }

        // 🖱️ click pe piese
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero);

            if (hit)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        bool moved =
                            SwapIfValid(i, -size, size) ||
                            SwapIfValid(i, +size, size) ||
                            SwapIfValid(i, -1, 0) ||
                            SwapIfValid(i, +1, size - 1);

                        // ▶️ PORNEȘTE TIMER LA PRIMA MUTARE
                        if (moved && !timerRunning && !puzzleFinished)
                        {
                            timerRunning = true;
                        }

                        break;
                    }
                }
            }
        }
    }

    // ================= PUZZLE =================

    private void CreateNewPuzzle(int newSize)
    {
        StopAllCoroutines();
        ClearBoard();

        size = newSize;
        pieces = new List<Transform>();

        timer = 0f;
        timerRunning = false;
        puzzleFinished = false;
        UpdateTimerText();

        CreateGamePieces(0.01f);
        Shuffle();
    }

    private void ClearBoard()
    {
        foreach (Transform child in gameTransform)
            Destroy(child.gameObject);
    }

    private void CreateGamePieces(float gapThickness)
    {
        float width = 1f / size;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);

                piece.localPosition = new Vector3(
                    -1 + (2 * width * col) + width,
                    +1 - (2 * width * row) - width,
                    0);

                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";

                if (row == size - 1 && col == size - 1)
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];

                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));

                    mesh.uv = uv;
                }
            }
        }
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            (pieces[i].localPosition, pieces[i + offset].localPosition) =
                (pieces[i + offset].localPosition, pieces[i].localPosition);

            emptyLocation = i;
            return true;
        }
        return false;
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
                return false;
        }
        return true;
    }

    private void Shuffle()
    {
        int count = 0;
        int last = -1;

        while (count < size * size * size)
        {
            int rnd = Random.Range(0, size * size);
            if (rnd == last) continue;

            last = emptyLocation;

            if (SwapIfValid(rnd, -size, size) ||
                SwapIfValid(rnd, +size, size) ||
                SwapIfValid(rnd, -1, 0) ||
                SwapIfValid(rnd, +1, size - 1))
            {
                count++;
            }
        }
    }

    // ================= TIMER UI =================

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // ================= PAUSE FROM PANEL =================

    public void PausePuzzle()
    {
        puzzleBlocked = true;
        timerRunning = false;
    }

    public void ResumePuzzle()
    {
        puzzleBlocked = false;
        if (!puzzleFinished)
            timerRunning = true;
    }
    public void SetSizeFromInput()
    {
        string input = "";

        if (sizeInputTMP != null && !string.IsNullOrEmpty(sizeInputTMP.text))
            input = sizeInputTMP.text;
        else if (sizeInputLegacy != null && !string.IsNullOrEmpty(sizeInputLegacy.text))
            input = sizeInputLegacy.text;

        if (int.TryParse(input, out int newSize))
        {
            newSize = Mathf.Clamp(newSize, 2, 5);

            // RESET TOTAL
            CreateNewPuzzle(newSize);
        }
    }

}
