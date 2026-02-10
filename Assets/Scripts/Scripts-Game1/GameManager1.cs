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
    [SerializeField] private TMP_Text timerText;

    [Header("Size Input")]
    [SerializeField] private TMP_InputField sizeInputTMP;
    [SerializeField] private InputField sizeInputLegacy;

    private List<Transform> pieces = new List<Transform>();
    private int emptyLocation;
    private int size = 3;
    private bool shuffling;

    // ===== TIMER =====
    private float timer;
    private bool timerRunning;
    private bool puzzleFinished;

    // ===== BLOCK INPUT (panels) =====
    public bool puzzleBlocked;

    void Start()
    {
        CreateNewPuzzle(size);
        UpdateTimerText();
    }

    void Update()
    {
        // ⛔ PAUSE GLOBAL
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            return;

        // ⛔ PANELS (options / settings)
        if (puzzleBlocked)
            return;

        // ⏱️ TIMER
        if (timerRunning && !puzzleFinished)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }

        // ✅ WIN CHECK
        if (!shuffling && !puzzleFinished && CheckCompletion())
        {
            puzzleFinished = true;
            timerRunning = false;
            puzzleBlocked = true;

            if (WinPanelManager.Instance != null)
                WinPanelManager.Instance.ShowWin(timer);
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayWin();

        }



        // 🖱️ INPUT
        if (Input.GetMouseButtonDown(0))
            HandleClick();
    }

    // ================= INPUT =================
    private void HandleClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero
        );

        if (!hit) return;

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] != hit.transform) continue;

            bool moved =
                SwapIfValid(i, -size, size) ||
                SwapIfValid(i, +size, size) ||
                SwapIfValid(i, -1, 0) ||
                SwapIfValid(i, +1, size - 1);

            if (moved && AudioManager.Instance != null)
                AudioManager.Instance.PlayMove();


            // ▶️ PORNEȘTE TIMER LA PRIMA MUTARE
            if (moved && !timerRunning && !puzzleFinished)
                timerRunning = true;

            break;
        }
    }

    // ================= PUZZLE =================
    private void CreateNewPuzzle(int newSize)
    {
        ClearBoard();

        size = newSize;
        pieces.Clear();

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

    private void CreateGamePieces(float gap)
    {
        float width = 1f / size;

        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);

                piece.localPosition = new Vector3(
                    -1 + (2 * width * c) + width,
                    +1 - (2 * width * r) - width,
                    0f
                );

                piece.localScale = ((2 * width) - gap) * Vector3.one;
                piece.name = $"{(r * size) + c}";

                if (r == size - 1 && c == size - 1)
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];

                    uv[0] = new Vector2(width * c, 1 - width * (r + 1));
                    uv[1] = new Vector2(width * (c + 1), 1 - width * (r + 1));
                    uv[2] = new Vector2(width * c, 1 - width * r);
                    uv[3] = new Vector2(width * (c + 1), 1 - width * r);

                    mesh.uv = uv;
                }
            }
        }
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if ((i % size != colCheck) && (i + offset == emptyLocation))
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
            if (pieces[i].name != i.ToString())
                return false;

        return true;
    }

    private void Shuffle()
    {
        int count = 0;

        while (count < size * size * size)
        {
            int rnd = Random.Range(0, size * size);

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
        int m = Mathf.FloorToInt(timer / 60);
        int s = Mathf.FloorToInt(timer % 60);
        timerText.text = $"{m:00}:{s:00}";
    }

    // ================= PANELS =================
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

    // ================= SIZE INPUT =================
    public void SetSizeFromInput()
    {
        string input = "";

        if (sizeInputTMP && !string.IsNullOrEmpty(sizeInputTMP.text))
            input = sizeInputTMP.text;
        else if (sizeInputLegacy && !string.IsNullOrEmpty(sizeInputLegacy.text))
            input = sizeInputLegacy.text;

        if (int.TryParse(input, out int newSize))
        {
            newSize = Mathf.Clamp(newSize, 2, 6);
            CreateNewPuzzle(newSize);
        }
    }
}
