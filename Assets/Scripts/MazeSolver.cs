using System.Collections.Generic;
using UnityEngine;

public class MazeSolver : MonoBehaviour
{
    public MazeGeneration generator;

    [Header("Markers")]
    public GameObject startMarkerPrefab;
    public GameObject goalMarkerPrefab;
    public GameObject pathMarkerPrefab;

    public Vector2Int start = new(1,1);
    public Vector2Int goal;

    public int[,] distance; // -1 = NOT reached yet
    private Vector2Int[,] cameFrom;
    public List<Vector2Int> path = new();


    // The same four directions as the generator - but we step ONE tile this time.
    static readonly Vector2Int[] Directions =
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
    };

    //Call this AFTER the maze has been Generated
    public void Solve()
    {
        ComputateDistances();
        PlaceMarkers();
        BuildPath();
        DrawPath();
    }
    private void ComputateDistances()
    {
        bool[,] maze = generator.maze;
        int w = generator.width;
        int h = generator.height;

        // 1. CREATE the distance grid and fill every entry with -1.
        distance = new int[w, h];
        // (nested loops to set -1)
        for(int i = 0; i < w; i++)
        {
            for(int j = 0; j < h; j++)
            {
                // -1 means "BFS has not reached this tile yet".
                distance[i, j] = -1;
            }
        }
        //    CREATE cameFrom the same size:
        cameFrom = new Vector2Int[w, h];
        // 2. CREATE the queue:   Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Queue<Vector2Int> queue = new();

        // 3. SEED it:
        distance[start.x, start.y] = 0; 
        queue.Enqueue(start);

        // 4. Track the best tile as we go:
        goal = start;
        int bestDistance = 0;

        // 5. WHILE the queue is not empty:
        while(queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();     // front of the queue
            if(distance[current.x, current.y] > bestDistance)
            {
                bestDistance = distance[current.x, current.y];
                goal = current;
            }
            foreach(Vector2Int dir in Directions)
            {
                Vector2Int next = current + dir;
                //Check if it is within maze boundaries
                if(0 <= next.x && next.x < w && 0 <= next.y && next.y < h)
                {
                    //Check for floor tile
                    if (maze[next.x, next.y] == true)
                    {
                        //check is already visited
                        if (distance[next.x, next.y] == -1)
                        {
                            distance[next.x, next.y] = distance[current.x, current.y] + 1;
                            cameFrom[next.x, next.y] = current;
                            queue.Enqueue(next);

                        }
                    }
                }
            }
        }
        Debug.Log(distance[goal.x, goal.y]);
        // WHY a queue and not a stack?  The queue gives out the OLDEST (closest)
        // tiles first, so distances grow 0, 1, 2, 3... in order. A stack would
        // dive down one corridor and give the wrong numbers.
    }
    private void DrawPath()
    {
        if (path.Count == distance[goal.x, goal.y] +1)
        {
            foreach(Vector2Int tile in path)
            {
                if(tile != start && tile != goal)
                {
                    Instantiate(pathMarkerPrefab, new Vector3(tile.x, 0.1f, tile.y), Quaternion.identity, transform);
                }
            }
        }
    }

    private void BuildPath()
    {
        path.Clear();
        Vector2Int current = goal;
        while(current != start)
        {
            path.Add(current);
            current = cameFrom[current.x, current.y];
        }
        path.Add(start);
        path.Reverse();
        // Why does this work? Every tile remembers the tile it was reached FROM.
        // Following those links from the goal leads back to the start - like
        // following breadcrumbs home.
    }

    private void PlaceMarkers()
    {
        Instantiate(startMarkerPrefab, new Vector3(start.x, 0.1f, start.y), Quaternion.identity, transform);
        Instantiate(goalMarkerPrefab, new Vector3(goal.x, 0.1f, goal.y), Quaternion.identity, transform);
    }
}
