using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeGeneration : MonoBehaviour
{
    [SerializeField] private int m_width;
    [SerializeField] private int m_height;
    [SerializeField] private int m_steps;
    [SerializeField] private int m_speed;
    [SerializeField] private int m_seed;

    private bool[,] m_maze;

    private Movements m_movement;
    private System.Random m_rand;

    private enum Movements
    { 
        UP,
        DOWN,    
        LEFT,
        RIGHT
    }

    private void DrawSteps()
    {
        List<Vector2> m_ancors = new();

        // the vector carver that will carve out the maze
        Vector2 m_carver = new Vector2(1, 1);
        Vector2 m_priorPosition = new();

        m_ancors.Add(new Vector2(m_carver.x, m_carver.y));
        Movements m_movement = new();

        while (m_steps > 0)
        {
            Vector2 newPosition = new Vector2(0, 0);
            bool move = false;

            while (!move)
            {

                int tempNum = m_rand.Next(0, 4);
                m_movement = (Movements)tempNum;

                // decidedes which direction the maze will be carved out by the vector
                switch (m_movement)
                {
                    case Movements.UP:
                        newPosition = new Vector2(m_carver.x, m_carver.y + 2);
                        break;
                    case Movements.DOWN:
                        newPosition = new Vector2(m_carver.x, m_carver.y - 2);
                        break;
                    case Movements.LEFT:
                        newPosition = new Vector2(m_carver.x - 2, m_carver.y);
                        break;
                    case Movements.RIGHT:
                        newPosition = new Vector2(m_carver.x + 2, m_carver.y);
                        break;
                    default:
                        Debug.Log("Cannot Find Movement Enum");
                        break;
                }


                // checks if the player has been to an old position and stops them from going back
                
                for(int i = 0; i < m_ancors.Count; i++)
                {
                    if (!m_ancors.Contains(newPosition) && newPosition.x > 0 && newPosition.y > 0 &&
                        newPosition.x < m_width - 1 && newPosition.y < m_height - 1)
                    {
                        m_priorPosition = m_carver;
                        move = true;
                    }
                    else if (i == 20)
                    {
                        newPosition = m_priorPosition;
                        m_priorPosition = m_carver;
                        move = true;
                    }
                }
            }

            m_ancors.Add(newPosition);


                float movedX = 0;
                float movedY = 0;

                //gets the space between new position and the carver
                if (m_carver.x < newPosition.x)
                {
                    movedX = 1;
                }
                else if (!(m_carver.x == newPosition.x))
                {
                    movedX = -1;
                }

                if (m_carver.y < newPosition.y)
                {
                    movedY = 1;
                }
                else if (!(m_carver.y == newPosition.y))
                {
                    movedY = -1;
                }

                Vector2 newPath = new Vector2(m_carver.x + movedX, m_carver.y + movedY);

                m_maze[(int)newPath.x, (int)newPath.y] = true;
                m_carver = newPosition;

                m_maze[(int)m_carver.x, (int)m_carver.y] = true;

                m_steps--;

        }
    }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            m_rand = new System.Random(m_seed);

            m_maze = new bool[m_width, m_height];
            m_maze[1, 1] = true;

            DrawSteps();
            DisplayMaze();
    }
    

    private void DisplayMaze()
    {
        for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
            {
                if (m_maze[x, y] == false)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    cube.transform.position = new Vector3(x, y);
                }
                else
                {
                    //// makes empty space red spheres
                    //    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //    sphere.GetComponent<Renderer>().material.color = Color.red;
                    //    sphere.transform.position = new Vector3(x, y);
                }
            }
        }
    }
}
