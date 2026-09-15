using System.Collections.Generic;
using UnityEngine;

public class CatChaser : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3f;
    public float recomputeInterval = 0.3f;

    private Vector2 moveTarget;
    private float timer = 0f;

    private static readonly Vector2Int[] Directions =
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
    };

    void Start()
    {
        moveTarget = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= recomputeInterval)
        {
            timer = 0f;
            RecomputePath();
        }

        transform.position = Vector2.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
    }

    void RecomputePath()
    {
        if (target == null) return;

        MazeData maze = MazeData.Instance;
        Vector2Int start = maze.WorldToGrid(transform.position);
        Vector2Int goal = maze.WorldToGrid(target.position);

        Vector2Int? nextStep = FindNextStep(maze, start, goal);
        if (nextStep.HasValue)
        {
            moveTarget = maze.GridToWorld(nextStep.Value.x, nextStep.Value.y);
        }
    }

    public static Vector2Int? FindNextStep(MazeData maze, Vector2Int start, Vector2Int goal)
    {
        if (start == goal) return start;

        var visited = new HashSet<Vector2Int> { start };
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        bool found = false;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == goal)
            {
                found = true;
                break;
            }

            foreach (Vector2Int dir in Directions)
            {
                Vector2Int next = current + dir;
                if (visited.Contains(next)) continue;
                if (!maze.IsOpen(next.x, next.y)) continue;

                visited.Add(next);
                cameFrom[next] = current;
                queue.Enqueue(next);
            }
        }

        if (!found) return null;

        Vector2Int step = goal;
        while (cameFrom.ContainsKey(step) && cameFrom[step] != start)
        {
            step = cameFrom[step];
        }

        return step;
    }
}
