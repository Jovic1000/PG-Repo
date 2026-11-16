using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;    // at the top: List and Queue

public class Caveing : MonoBehaviour
{
 
    public bool keepLargestCavern = true;

    private bool[,] m_map;

    [SerializeField] private int m_width;
    [SerializeField] private int m_height;
    [SerializeField] private float m_fill;
    [SerializeField] private int seed;
    [SerializeField] private int m_smoothing;

    private System.Random m_random;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_random = new System.Random(seed);
        m_map = new bool[m_width, m_height];

        GenerateMap();

        for (int i = 0; i < m_smoothing; i++)
        {
            SmoothMap();
        }
        if (keepLargestCavern)
        {
            KeepLargestCavern();
        }

        GetComponent<MeshRenderer>().material.mainTexture = IMGtool.BoolToTextureMap(m_map);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        for (int y = 1; y < m_height - 1; y++)
        {
            for(int x = 1; x < m_width - 1; x++)
            {
                if (m_fill > m_random.Next(100))
                {
                    m_map[x, y] = true;
                }
                else
                {
                    m_map[x, y] = false;
                }

            }
        }
    }

    public void SmoothMap()
    {
        int loop = 0;
        bool[,] m_newMap = new bool[m_width, m_height];

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
                        else if (m_map[j, i] == true)
                        {
                            count++;
                        }
                    }
                }

                if (count > 4)
                {
                    m_newMap[x, y] = true; // create a wall
                }
                else if (count < 4) // flip values for maze /\ \/ (no longer working)
                {
                    m_newMap[x, y] = false; // create a floor
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
        if (m_map[x, y] == true && !viseted[x, y])
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
                if (m_map[x,y] == true && !visited[x,y])
                {
                    caverns.Add(FloodFill(x, y, visited));
                }
            }
        }

        // 2. find the biggest (compare .Count)

        List<Vector2Int> LargestCavern = new List<Vector2Int>();

        //for (int i = 1; i < caverns.Count; i++)
        //{
        //    if (caverns[i - 1].Count > caverns[i].Count)
        //    {
        //        LargestCavern = caverns[i - 1];
        //    }
        //    else
        //    {
        //        LargestCavern = caverns[i];
        //    }
        //}

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
                    m_map[cell.x,cell.y] = false;
                }
            }
        }

        Debug.Log("Number Of Caverns: " + caverns.Count + ", Number of cells kept: " + LargestCavern.Count);
    }
}

public static class IMGtool
{
    public static Texture2D BoolToTextureMap(bool[,] noiseMap)
    {
        Texture2D texture = new(noiseMap.GetLength(0), noiseMap.GetLength(1));
        texture.filterMode = FilterMode.Point;

        // sets idavidual pixel colour when iterating

        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            { 

                if (!noiseMap[i, j])
                {
                    texture.SetPixel(i, j, Color.black);
                }
                else if (noiseMap[i, j])
                {
                    texture.SetPixel(i, j, Color.white);
                }
            }
        }

        texture.Apply();
        return texture;
    }
}