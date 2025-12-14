using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;
    [SerializeField] private float gapThickness = 0.01f;

    [Header("UI")]
    public TMP_InputField sizeInput;

    private List<Transform> pieces = new List<Transform>();
    private int emptyLocation;
    private int size = 3;
    private bool shuffling = false;

    void Start()
    {
        CreateNewPuzzle(size);
    }

    // ================== UI BUTTON ==================
    public void ApplyNewSize()
    {
        if (int.TryParse(sizeInput.text, out int newSize))
        {
            newSize = Mathf.Clamp(newSize, 2, 8); // limite sigure
            CreateNewPuzzle(newSize);
        }
    }

    // ================== PUZZLE RESET ==================
    private void CreateNewPuzzle(int newSize)
    {
        StopAllCoroutines();
        ClearBoard();

        size = newSize;
        pieces = new List<Transform>();

        CreateGamePieces(gapThickness);
        Shuffle();
    }

    private void ClearBoard()
    {
        foreach (Transform child in gameTransform)
        {
            Destroy(child.gameObject);
        }
    }

    // ================== CREATE PIECES ==================
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
                    float gap = gapThickness / 2f;
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

    // ================== UPDATE ==================
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

    // ================== LOGIC ==================
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
}
