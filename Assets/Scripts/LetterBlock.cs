using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    public float moveSpeed = 6f;
    public char letter;

    public Vector2Int Cell { get; private set; }

    private Vector2 targetPos;
    private bool isMoving = false;

    public void SetStartCell(int col, int row)
    {
        Cell = new Vector2Int(col, row);
        targetPos = transform.position;
        MazeData.Instance.SetBlocked(col, row, true);
    }

    void Update()
    {
        if (!isMoving) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == targetPos)
        {
            isMoving = false;
            GameManager.Instance.CheckWord();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isMoving) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        Vector2 diff = (Vector2)transform.position - (Vector2)collision.transform.position;
        Vector2Int dir;
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            dir = diff.x > 0 ? new Vector2Int(1, 0) : new Vector2Int(-1, 0);
        }
        else
        {
            dir = diff.y > 0 ? new Vector2Int(0, -1) : new Vector2Int(0, 1);
        }

        Vector2Int nextCell = Cell + dir;
        if (!MazeData.Instance.IsOpen(nextCell.x, nextCell.y)) return;
        if (nextCell == MazeData.Instance.ExitCell) return;

        MazeData.Instance.SetBlocked(Cell.x, Cell.y, false);
        MazeData.Instance.SetBlocked(nextCell.x, nextCell.y, true);
        Cell = nextCell;
        targetPos = MazeData.Instance.GridToWorld(Cell.x, Cell.y);
        isMoving = true;
    }
}
