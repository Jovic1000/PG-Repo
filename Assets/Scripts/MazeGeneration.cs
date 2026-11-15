using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGeneration : MonoBehaviour
{
    // old code that uses a list
    //[SerializeField] private int m_width;
    //[SerializeField] private int m_height;
    //[SerializeField] private int m_steps;
    //[SerializeField] private int m_speed;
    //[SerializeField] private int m_seed;

    //private bool[,] m_maze;

    //private Movements m_movement;
    //private System.Random m_rand;

    //private enum Movements
    //{
    //    UP,
    //    DOWN,
    //    LEFT,
    //    RIGHT
    //}

    //private void DrawSteps()
    //{
    //    List<Vector2> m_ancors = new();

    //    // the vector carver that will carve out the maze
    //    Vector2 m_carver = new Vector2(1, 1);
    //    Vector2 m_priorPosition = new();

    //    m_ancors.Add(new Vector2(m_carver.x, m_carver.y));
    //    Movements m_movement = new();

    //    while (m_steps > 0)
    //    {
    //        Vector2 newPosition = new Vector2(0, 0);
    //        bool move = false;

    //        while (!move)
    //        {

    //            int tempNum = m_rand.Next(0, 4);
    //            m_movement = (Movements)tempNum;

    //            // decidedes which direction the maze will be carved out by the vector
    //            switch (m_movement)
    //            {
    //                case Movements.UP:
    //                    newPosition = new Vector2(m_carver.x, m_carver.y + 2);
    //                    break;
    //                case Movements.DOWN:
    //                    newPosition = new Vector2(m_carver.x, m_carver.y - 2);
    //                    break;
    //                case Movements.LEFT:
    //                    newPosition = new Vector2(m_carver.x - 2, m_carver.y);
    //                    break;
    //                case Movements.RIGHT:
    //                    newPosition = new Vector2(m_carver.x + 2, m_carver.y);
    //                    break;
    //                default:
    //                    Debug.Log("Cannot Find Movement Enum");
    //                    break;
    //            }


    //            // checks if the player has been to an old position and stops them from going back

    //            for (int i = 0; i < m_ancors.Count; i++)
    //            {
    //                if (!m_ancors.Contains(newPosition) && newPosition.x > 0 && newPosition.y > 0 &&
    //                    newPosition.x < m_width - 1 && newPosition.y < m_height - 1)
    //                {
    //                    m_priorPosition = m_carver;
    //                    move = true;
    //                }
    //                else if (i == 20)
    //                {
    //                    newPosition = m_priorPosition;
    //                    m_priorPosition = m_carver;
    //                    move = true;
    //                }
    //            }
    //        }

    //        m_ancors.Add(newPosition);


    //        float movedX = 0;
    //        float movedY = 0;

    //        //gets the space between new position and the carver
    //        if (m_carver.x < newPosition.x)
    //        {
    //            movedX = 1;
    //        }
    //        else if (!(m_carver.x == newPosition.x))
    //        {
    //            movedX = -1;
    //        }

    //        if (m_carver.y < newPosition.y)
    //        {
    //            movedY = 1;
    //        }
    //        else if (!(m_carver.y == newPosition.y))
    //        {
    //            movedY = -1;
    //        }

    //        Vector2 newPath = new Vector2(m_carver.x + movedX, m_carver.y + movedY);

    //        m_maze[(int)newPath.x, (int)newPath.y] = true;
    //        m_carver = newPosition;

    //        m_maze[(int)m_carver.x, (int)m_carver.y] = true;

    //        m_steps--;

    //    }
    //}

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    m_rand = new System.Random(m_seed);

    //    m_maze = new bool[m_width, m_height];
    //    m_maze[1, 1] = true;

    //    DrawSteps();
    //    DisplayMaze();
    //}


    //private void DisplayMaze()
    //{
    //    for (int x = 0; x < m_width; x++)
    //    {
    //        for (int y = 0; y < m_height; y++)
    //        {
    //            if (m_maze[x, y] == false)
    //            {
    //                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //                cube.GetComponent<Renderer>().material.color = Color.green;
    //                cube.transform.position = new Vector3(x, y);
    //            }
    //            else
    //            {
    //                //// makes empty space red spheres
    //                //    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //                //    sphere.GetComponent<Renderer>().material.color = Color.red;
    //                //    sphere.transform.position = new Vector3(x, y);
    //            }
    //        }
    //    }
    //}

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

