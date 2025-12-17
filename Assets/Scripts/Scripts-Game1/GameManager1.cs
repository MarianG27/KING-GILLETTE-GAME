using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Pentru TMP

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;

    [Header("UI")]
    public InputField legacyInput; // InputField legacy
    public TMP_InputField tmpInput; // TMP InputField

    private List<Transform> pieces;
    private int emptyLocation;
    private int size = 3;
    private bool shuffling = false;
    public bool puzzleActive = true;


    void Start()
    {
        CreateNewPuzzle(size);
    }

    // ================= RESET PUZZLE =================
    private void CreateNewPuzzle(int newSize)
    {
        StopAllCoroutines();
        ClearBoard();

        size = newSize;
        pieces = new List<Transform>();

        CreateGamePieces(0.01f);
        Shuffle();
    }

    private void ClearBoard()
    {
        foreach (Transform child in gameTransform)
        {
            Destroy(child.gameObject);
        }
    }

    // ================= CREATE PIECES =================
    private void CreateGamePieces(float gapThickness)
    {
        float width = 1f / size;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);

                piece.gameObject.layer = LayerMask.NameToLayer("Puzzle");

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

    // ================= UPDATE =================
    void Update()
    {
        if (!shuffling && CheckCompletion())
        {
            shuffling = true;
            StartCoroutine(WaitShuffle(0.5f));
        }

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
                        if (SwapIfValid(i, -size, size)) break;
                        if (SwapIfValid(i, +size, size)) break;
                        if (SwapIfValid(i, -1, 0)) break;
                        if (SwapIfValid(i, +1, size - 1)) break;
                    }
                }
            }
        }
    }

    // ================= LOGIC =================
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

    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
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

    // ================= INPUT =================
    public void SetSizeFromInput()
    {
        string inputText = "";

        if (tmpInput != null && !string.IsNullOrEmpty(tmpInput.text))
        {
            inputText = tmpInput.text;
        }
        else if (legacyInput != null && !string.IsNullOrEmpty(legacyInput.text))
        {
            inputText = legacyInput.text;
        }

        if (int.TryParse(inputText, out int newSize))
        {
            newSize = Mathf.Clamp(newSize, 2, 5); // limite sigure
            CreateNewPuzzle(newSize);
        }
    }
}
