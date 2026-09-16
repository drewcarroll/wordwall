using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    public static readonly Color LavaColor = new Color(0.38f, 0.07f, 0.06f);

    [Header("Gameplay Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject catPrefab;
    [SerializeField] private GameObject letterBlockPrefab;
    [SerializeField] private GameObject exitPrefab;

    void Awake()
    {
        if (!PrefabsAreAssigned())
        {
            Debug.LogError(
                "MazeBuilder is missing prefab references. Run Tools > WordWall > Create Placeholder Prefabs, then save the scene.",
                this);
            return;
        }

        MazeData maze = gameObject.AddComponent<MazeData>();
        GameManager manager = gameObject.AddComponent<GameManager>();

        Transform playerTransform = null;
        GameObject catObject = null;

        for (int row = 0; row < maze.Height; row++)
        {
            for (int col = 0; col < maze.Width; col++)
            {
                char c = MazeData.Layout[row][col];
                Vector2 pos = maze.GridToWorld(col, row);

                if (c == '#')
                {
                    SpawnWall(pos, maze.IsBorderWall(col, row));
                }
                else if (c == 'M')
                {
                    playerTransform = SpawnPlayer(pos);
                }
                else if (c == 'C')
                {
                    catObject = SpawnCat(pos);
                }
                else if (c == 'E')
                {
                    manager.exitRenderer = SpawnExit(pos);
                }
                else if (char.IsLower(c))
                {
                    manager.blocks.Add(SpawnLetter(pos, char.ToUpper(c), col, row));
                }
            }
        }

        catObject.GetComponent<CatChaser>().target = playerTransform;

        FitCamera(maze);
    }

    void SpawnWall(Vector2 pos, bool isLava)
    {
        GameObject go = new GameObject(isLava ? "Lava" : "Wall");
        go.transform.position = pos;
        go.AddComponent<SpriteRenderer>().sprite =
            MakeSquareSprite(isLava ? LavaColor : new Color(0.3f, 0.3f, 0.3f));
        go.AddComponent<BoxCollider2D>();

        if (isLava) go.tag = "Lava";
    }

    Transform SpawnPlayer(Vector2 pos)
    {
        GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity, transform);
        player.name = "Mouse";
        return player.transform;
    }

    GameObject SpawnCat(Vector2 pos)
    {
        GameObject cat = Instantiate(catPrefab, pos, Quaternion.identity, transform);
        cat.name = "Cat";
        return cat;
    }

    SpriteRenderer SpawnExit(Vector2 pos)
    {
        GameObject exit = Instantiate(exitPrefab, pos, Quaternion.identity, transform);
        exit.name = "Exit";
        return exit.GetComponent<SpriteRenderer>();
    }

    LetterBlock SpawnLetter(Vector2 pos, char letter, int col, int row)
    {
        GameObject letterObject = Instantiate(letterBlockPrefab, pos, Quaternion.identity, transform);
        letterObject.name = "Letter_" + letter;

        LetterBlock block = letterObject.GetComponent<LetterBlock>();
        block.letter = letter;
        block.SetStartCell(col, row);

        TextMesh label = letterObject.GetComponentInChildren<TextMesh>();
        if (label != null)
        {
            label.text = letter.ToString();
        }

        return block;
    }

    bool PrefabsAreAssigned()
    {
        return playerPrefab != null
            && catPrefab != null
            && letterBlockPrefab != null
            && exitPrefab != null;
    }

    public static Sprite MakeSquareSprite(Color color)
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
    }

    void FitCamera(MazeData maze)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float sizeForHeight = maze.Height / 2f + 1f;
        float sizeForWidth = (maze.Width / 2f + 1f) / cam.aspect;
        cam.orthographicSize = Mathf.Max(sizeForHeight, sizeForWidth);
        cam.transform.position = new Vector3((maze.Width - 1) / 2f, -(maze.Height - 1) / 2f, -10f);
    }
}
