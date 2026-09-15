using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    void Awake()
    {
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
                    SpawnWall(pos);
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

    void SpawnWall(Vector2 pos)
    {
        GameObject go = new GameObject("Wall");
        go.transform.position = pos;
        go.AddComponent<SpriteRenderer>().sprite = MakeSquareSprite(new Color(0.3f, 0.3f, 0.3f));
        go.AddComponent<BoxCollider2D>();
    }

    Transform SpawnPlayer(Vector2 pos)
    {
        GameObject go = new GameObject("Mouse");
        go.tag = "Player";
        go.transform.position = pos;

        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = MakeSquareSprite(Color.gray);
        renderer.sortingOrder = 1;

        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        go.AddComponent<CircleCollider2D>().radius = 0.4f;
        go.AddComponent<PlayerController>();

        return go.transform;
    }

    GameObject SpawnCat(Vector2 pos)
    {
        GameObject go = new GameObject("Cat");
        go.tag = "Cat";
        go.transform.position = pos;

        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = MakeSquareSprite(new Color(0.8f, 0.3f, 0.1f));
        renderer.sortingOrder = 1;

        CircleCollider2D collider = go.AddComponent<CircleCollider2D>();
        collider.radius = 0.4f;
        collider.isTrigger = true;

        go.AddComponent<CatChaser>();
        return go;
    }

    SpriteRenderer SpawnExit(Vector2 pos)
    {
        GameObject go = new GameObject("Exit");
        go.tag = "Exit";
        go.transform.position = pos;

        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = MakeSquareSprite(new Color(0.7f, 0.1f, 0.1f));

        go.AddComponent<BoxCollider2D>();
        return renderer;
    }

    LetterBlock SpawnLetter(Vector2 pos, char letter, int col, int row)
    {
        GameObject go = new GameObject("Letter_" + letter);
        go.transform.position = pos;
        go.AddComponent<SpriteRenderer>().sprite = MakeSquareSprite(new Color(0.9f, 0.8f, 0.2f));

        go.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        go.AddComponent<BoxCollider2D>();

        LetterBlock block = go.AddComponent<LetterBlock>();
        block.letter = letter;
        block.SetStartCell(col, row);

        GameObject label = new GameObject("Label");
        label.transform.SetParent(go.transform);
        label.transform.localPosition = new Vector3(0, 0, -1);

        TextMesh text = label.AddComponent<TextMesh>();
        text.text = letter.ToString();
        text.color = Color.black;
        text.characterSize = 0.15f;
        text.fontSize = 40;
        text.anchor = TextAnchor.MiddleCenter;

        return block;
    }

    Sprite MakeSquareSprite(Color color)
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
