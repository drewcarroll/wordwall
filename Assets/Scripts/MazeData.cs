using UnityEngine;

public class MazeData : MonoBehaviour
{
    public static MazeData Instance;

    public const float CellSize = 1f;

    public static readonly string Word = "CAT";

    public static readonly string[] Layout =
    {
        "#######################",
        "#....................C#",
        "#.c...................#",
        "#..##..##..##......##.#",
        "#..##..##..##......##.#",
        "#.....................E",
        "#............a........#",
        "#..##..##......##..##.#",
        "#..##..##......##..##.#",
        "#................t....#",
        "#M....................#",
        "#######################",
    };

    private bool[,] blocked;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public Vector2Int ExitCell { get; private set; }

    void Awake()
    {
        Instance = this;
        Height = Layout.Length;
        Width = Layout[0].Length;
        blocked = new bool[Width, Height];

        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                char c = Layout[row][col];
                blocked[col, row] = c == '#';

                if (c == 'E')
                {
                    ExitCell = new Vector2Int(col, row);
                }
            }
        }
    }

    public bool IsOpen(int col, int row)
    {
        if (col < 0 || col >= Width || row < 0 || row >= Height) return false;
        return !blocked[col, row];
    }

    public void SetBlocked(int col, int row, bool value)
    {
        blocked[col, row] = value;
    }

    public Vector2 GridToWorld(int col, int row)
    {
        return new Vector2(col * CellSize, -row * CellSize);
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        int col = Mathf.RoundToInt(worldPos.x / CellSize);
        int row = Mathf.RoundToInt(-worldPos.y / CellSize);
        return new Vector2Int(col, row);
    }
}
