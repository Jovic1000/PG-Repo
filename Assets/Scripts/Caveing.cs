using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;    // at the top: List and Queue

public class Caveing : MonoBehaviour
{
 
    public bool showOutline;
    public bool keepLargestCavern;

    private int[,] m_map;

    [SerializeField] private int m_width;
    [SerializeField] private int m_height;
    [SerializeField] private float m_fill;
    [SerializeField] private int seed;
    [SerializeField] private int m_smoothing;

    private System.Random m_random;

    private void OnValidate()
    {
        transform.localScale = new Vector3(m_width, m_height, 0.1f);
        transform.position = new Vector3 ((m_width  - 1f) / 2f, (m_height - 1f) / 2);

        Generate();
        Debug.Log("Validated"); // outputs to console that the assets have successfully valadated
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnDrawGizmos()
    { 
        
        if (m_map == null || !showOutline)
        {
            Debug.Log("oopsy nopsy mapy");
            return;
        }

        Gizmos.color = Color.cyan;

        // 3. loop x from 0 to width - 2, then y from 0 to height - 2
        //      the case for this square
        //      the four midpoints (table above), as Vector3s

        int c;
        Vector3 b;
        Vector3 r;
        Vector3 t;
        Vector3 l;

        for (int x = 0; x < m_width - 1; x++)
        {
            for (int y = 0; y < m_height - 1; y++)
            {
                c = GetCaseIndex(x, y);

                b = new Vector3(x + 0.5f, y);
                r = new Vector3(x + 1f, y + 0.5f);
                t = new Vector3(x + 0.5f, y + 1f);
                l = new Vector3(x, y + 0.5f);

                DrawCaseLines(c, b, r, t, l);
            }
        }
    }
    private void Generate()
    {
        m_random = new System.Random(seed);
        m_map = new int[m_width, m_height];

        GenerateMap();

        for (int i = 0; i < m_smoothing; i++)
        {
            SmoothMap();
        }
        if (keepLargestCavern)
        {
            KeepLargestCavern();
        }

        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = IMGtool.IntToTextureMap(m_map);
    }

    public void GenerateMap()
    {
        for (int y = 1; y < m_height - 1; y++)
        {
            for(int x = 1; x < m_width - 1; x++)
            {
                if (m_fill > m_random.Next(100))
                {
                    m_map[x, y] = 1;
                }
                else
                {
                    m_map[x, y] = 0;
                }

            }
        }
    }

    public void SmoothMap()
    {
        int loop = 0;
        int[,] m_newMap = new int[m_width, m_height];

        for (int y = 1; y < m_height - 1; y++)
        {
            for (int x = 1; x < m_width - 1; x++)
            {
                int count = 0;

                for(int i = y - 1; i < y + 2; i++)
                {
                    for (int j = x - 1; j < x + 2; j++)
                    {
                        loop++;

                        if ((j, i) == (x, y))
                        { 
                            // do nothing
                        }
                        else if (m_map[j, i] == 1)
                        {
                            count++;
                        }
                    }
                }

                if (count > 4)
                {
                    m_newMap[x, y] = 1; // create a wall
                }
                else if (count < 4) // flip values for maze /\ \/ (no longer working)
                {
                    m_newMap[x, y] = 0; // create a floor
                }
                else
                {
                    m_newMap[x, y] = m_map[x, y];
                }
            }
        }

        m_map = m_newMap;
    }

    private List<Vector2Int> FloodFill(int startX, int startY, bool[,] visited)
    {
        List<Vector2Int> cells = new List<Vector2Int>();
        Queue<Vector2Int> caverns = new Queue<Vector2Int>();

        // 1. Enqueue the start, mark it visited

        caverns.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        // 2. while the queue isn't empty:
        while (caverns.Count != 0)
        {
            //      Dequeue a cell, add it to cells
            Vector2Int cell = caverns.Dequeue();
            cells.Add(cell);

            //      for each of its 4 neighbours:
            //          inside the map?  floor?  not visited?
            //              mark visited, Enqueue it

            TryAdd(cell.x - 1, cell.y, visited, caverns);
            TryAdd(cell.x + 1, cell.y, visited, caverns);
            TryAdd(cell.x, cell.y + 1, visited, caverns);
            TryAdd(cell.x, cell.y - 1, visited, caverns);
        }

        return cells;
    }

    private void TryAdd(int x, int y, bool[,] viseted, Queue<Vector2Int> queue)
    {
        if (m_map[x, y] == 1 && !viseted[x, y])
        {
            viseted[x, y] = true;
            queue.Enqueue(new Vector2Int(x, y));
        }
    }

    public void KeepLargestCavern()
    {
        bool[,] visited = new bool[m_width, m_height];
        List<List<Vector2Int>> caverns = new List<List<Vector2Int>>();

        // 1. loop x, then y:  a floor cell not yet visited  →  add FloodFill(...) to caverns
        for (int y = 1; y < m_height - 1; y++) 
        {
            for (int x = 1; x < m_width - 1; x++)
            {
                if (m_map[x,y] == 1 && !visited[x,y])
                {
                    caverns.Add(FloodFill(x, y, visited));
                }
            }
        }

        // 2. find the biggest (compare .Count)

        List<Vector2Int> LargestCavern = new List<Vector2Int>();

        foreach (List<Vector2Int> cavern in caverns)
            if (cavern.Count > LargestCavern.Count)
                LargestCavern = cavern;


        // 3. every OTHER cavern: set each of its cells to wall

        foreach (List<Vector2Int> cavern in caverns)
        {
            if (cavern != LargestCavern)
            {
                foreach (Vector2Int cell in cavern)
                {
                    m_map[cell.x,cell.y] = 0;
                }
            }
        }

        Debug.Log("Number Of Caverns: " + caverns.Count + ", Number of cells kept: " + LargestCavern.Count);
    }

    public int GetCaseIndex(int x, int y)
    {
        // top-left     map[x,     y + 1]  is worth 8
        // top-right    map[x + 1, y + 1]  is worth 4
        // bottom-right ...                is worth 2
        // bottom-left  ...                is worth 1
        // wall = 1, floor = 0: multiply each by its value and add

        return (m_map[x, y + 1] * 8) + (m_map[x + 1, y + 1] * 4) + (m_map[x + 1, y] * 2) + (m_map[x, y] * 1);
    }



    void DrawCaseLines(int c, Vector3 b, Vector3 r, Vector3 t, Vector3 l)
    {
        switch (c)
        {
            case 1: case 14: 
                Gizmos.DrawLine(b, l); break;
            case 2: case 13: 
                Gizmos.DrawLine(b, r); break;
            case 3: case 12: 
                Gizmos.DrawLine(r, l); break;
            case 4: case 11: 
                Gizmos.DrawLine(t, r); break;
            case 5:
                Gizmos.DrawLine(l, t); Gizmos.DrawLine(b, r); break;
            case 6: case 9: 
                Gizmos.DrawLine(t, b); break;
            case 7: case 8: 
                Gizmos.DrawLine(t, l); break;
            case 10: 
                Gizmos.DrawLine(t, l); Gizmos.DrawLine(b, r); break;

            default:
                break;
        }
    }

    public int[,] GetMap()
    {
        return m_map;
    }

    public int GetWidth()
    {
        return m_width;
    }

    public int GetHeight()
    {
        return m_height;
    }

}

public static class IMGtool
{
    public static Texture2D IntToTextureMap(int[,] noiseMap)
    {
        Texture2D texture = new(noiseMap.GetLength(0), noiseMap.GetLength(1));
        texture.filterMode = FilterMode.Point;

        // sets idavidual pixel colour when iterating

        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            { 

                if (noiseMap[i, j] == 0)
                {
                    texture.SetPixel(i, j, Color.black);
                }
                else if (noiseMap[i, j] == 1)
                {
                    texture.SetPixel(i, j, Color.white);
                }
            }
        }

        texture.Apply();
        return texture;
    }
}