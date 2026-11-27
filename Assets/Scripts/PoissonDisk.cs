using System.Collections.Generic;
using UnityEngine;

public static class PoissonDisk
{
    public static List<Vector2> Generate(float width, float height, float radius, int tries, int seed)
    {
        System.Random prng = new System.Random(seed);
        List<Vector2> points = new List<Vector2>();
        List<Vector2> active = new List<Vector2>();

        // 1. the first point: x = (float)prng.NextDouble() * width, y the same with height
        // 2. add it to points AND to active
        // 3. the main loop (below)

        float cellSize = radius / Mathf.Sqrt(2);
        int cols = Mathf.CeilToInt(width / cellSize);     // CeilToInt rounds UP
        int rows = Mathf.CeilToInt(height / cellSize);
        int[,] grid = new int[cols, rows];

        // fill every cell with -1

        // whenever you keep a point p:
        //   points.Add(p);  active.Add(p);
        //   grid[(int)(p.x / cellSize), (int)(p.y / cellSize)] = points.Count - 1;


        while (active.Count > 0)
        {
            int i = prng.Next(active.Count);           // a random active point
            Vector2 centre = active[i];
            bool found = false;

            for (int t = 0; t < tries; t++)
            {
                // angle: (float)prng.NextDouble() * Mathf.PI * 2
                // distance: radius + (float)prng.NextDouble() * radius      (radius to 2 x radius)
                // Vector2 candidate = centre + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
                // outside the area?  continue
                // far enough?  keep it (points AND active), found = true, break
            }

            if (!found) active.RemoveAt(i);            // no room left round it

        }
            return points;
    }

    static bool IsFarEnoughSlow(Vector2 candidate, List<Vector2> points, float radius)
    {
        // for every point p in points:
        //     if Vector2.Distance(candidate, p) < radius: return false
        return true;
    }

    static private void CreateGrid(float radius, int width, int height)
    {

    }

    // the candidate's cell
    int cx = (int)(candidate.x / cellSize);
    int cy = (int)(candidate.y / cellSize);

    // loop x from cx - 2 to cx + 2, y from cy - 2 to cy + 2   (the 5 x 5 block)
    //   skip cells outside the grid  (Mathf.Max(0, ...) and Mathf.Min(cols - 1, ...))
    //   int index = grid[x, y];
    //   if index is not -1 and points[index] is closer than radius: return false
    // return true


}
