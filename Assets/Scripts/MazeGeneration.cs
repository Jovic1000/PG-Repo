using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGeneration : MonoBehaviour
{

    [Header("Maze settings")]
    public int width = 21;    // MUST be odd
    public int height = 21;    // MUST be odd
    public int seed = 23;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;

    // true = floor / path, false = wall.   Index it as maze[x, y].
    public bool[,] maze;
    public MazeSolver solver;

    // CLASS CONVENTION: always check directions in THIS order.
    // Then seed 23 gives everybody the same maze.
    static readonly Vector2Int[] Directions =
    {
        Vector2Int.up,      // (0,  1)
        Vector2Int.right,   // (1,  0)
        Vector2Int.down,    // (0, -1)
        Vector2Int.left     // (-1, 0)
    };

    private void OnValidate()
    {
        if (width % 2 == 0) { width += 1; } // will divide by 2 until it can't
        if (height % 2 == 0) { height += 1; } // if even, make odd
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Generate();
        solver.Solve();
    }

    public void Generate()
    {
        ClearTiles();
        maze = new bool[width, height];
        CarveMaze();
        Draw();
    }

    void Draw()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject prefab = maze[x, y] ? floorPrefab : wallPrefab;
                Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, transform);
            }
        }
    }

    void CarveMaze()
    {
        // B1. CREATE the random generator ONCE:
        System.Random rng = new(seed);

        // B2. CREATE the stack (Mike's breadcrumb trail):
        Stack<Vector2Int> stack = new();

        // B3. PICK the start cell (1,1). It's a corner CELL (odd coordinates).
        Vector2Int start = new(1, 1);
        maze[start.x, start.y] = true;
        stack.Push(start);

        // B4. WHILE the stack is not empty  (stack.Count is greater than nothing):
        while (stack.Count > 0)
        {
            var current = stack.Peek();
            List<Vector2Int> options = GetUnvisitedNeighbours(current);

            if (options.Count() > 0)
            {
                var next = options[rng.Next(options.Count)];
                Vector2Int between = (current + next) / 2;

                maze[between.x, between.y] = true;
                maze[next.x, next.y] = true;
                stack.Push(next);
            }
            else
            {
                stack.Pop();
            }
        }
        int floors = 0;
        foreach (bool tile in maze) if (tile) floors++;
        Debug.Log("Floor tiles: " + floors);

    }
    List<Vector2Int> GetUnvisitedNeighbours(Vector2Int cell)
    {
        List<Vector2Int> result = new();

        foreach (Vector2Int dir in Directions)   // up, right, down, left
        {
            // The candidate cell is TWO tiles away:
            Vector2Int next = cell + dir * 2;

            // 1. Is it inside the playable area?
            //    The border (x = 0, x = width-1, y = 0, y = height-1) must stay wall,
            //    so valid values are 1 .. width-2  and  1 .. height-2.
            if (1 <= next.x && next.x < width)
            {
                if (1 <= next.y && next.y < height)
                {
                    //only checking for wall - do not want to walk backwards
                    if (maze[next.x, next.y] == false) // if still wall
                    {
                        result.Add(next);
                    }
                }
            }
            // 2. Is it UNVISITED?   Unvisited = still a wall:   maze[next.x, next.y] == false
            //
            // If BOTH are true:  result.Add(next);
        }

        return result;
    }

    public void ClearTiles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

}

