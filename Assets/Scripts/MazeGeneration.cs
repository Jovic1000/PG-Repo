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

    private GameObject m_mike;

    private Movements m_movement;

    private System.Random m_rand;

    private enum Movements
    { 
        UP,
        DOWN,    
        LEFT,
        RIGHT
    }

   /* private void CreateAncorPoints()
    { 
        // loops through the stack and adds a anchor for every second square within the boundries
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                if (x != m_width && x != 0)
                {
                    if (y != m_height && y != 0)
                    {
                        // creates a temp object for testing
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.position = new Vector3(x, y);
                        sphere.GetComponent<Renderer>().material.color = Color.purple;
                        //-------------------------------------------------------------


                        m_ancors.Push(new Vector2(x, y));
                        x++;
                    }
                }
            }
            // adds one to hight so it skips a line keeping the anchors 1 space apart
            if (y != m_height && y != 0)
            {
                y++;
            }
        }
    }*/

    private void DrawSteps()
    {
        Stack<Vector2> m_ancors = new();

        // the vector carver that will carve out the maze
        Vector2 m_carver = new Vector2(1, 1);
        Vector2 m_priorPosition = new();

        int runs = 0;

        m_ancors.Push(new Vector2(m_carver.x, m_carver.y));
        Movements m_movement = new();

        while (m_steps > 0)
        {
            bool m_passMoveCheck = false;

            while (!m_passMoveCheck)
            {
                int random = m_rand.Next(0, 4);
                bool cantMove1 = false;
                bool cantMove2 = false;
                bool cantMove3 = false;
                bool cantMove4 = false;

                //loops through all possible paths and chooses the only available one
                if (random == 2 && !(((int)m_carver.x - 2, (int)m_carver.y) == ((int)m_priorPosition.x, (int)m_priorPosition.y)))
                {
                    if (!(m_carver.x - 2 <= 1))
                    {
                        m_passMoveCheck = true;
                        m_movement = Movements.LEFT;
                        m_priorPosition = m_ancors.Peek();
                        m_ancors.Push(new Vector2(m_carver.x, m_carver.y));
                    }
                    else
                    {
                        cantMove1 = true;
                    }
                }
                else if (random == 3 && !(((int)m_carver.x + 2, (int)m_carver.y) == ((int)m_priorPosition.x, (int)m_priorPosition.y)))
                {
                    if (!(m_carver.x + 2 >= m_width))
                    {
                        m_passMoveCheck = true;
                        m_movement = Movements.RIGHT;
                        m_priorPosition = m_ancors.Peek();
                        m_ancors.Push(new Vector2(m_carver.x, m_carver.y));
                    }
                    else 
                    { 
                        cantMove2 = true; 
                    }
                }
                else if (random == 0 && !(((int)m_carver.x, (int)m_carver.y + 2) == ((int)m_priorPosition.x, (int)m_priorPosition.y)))
                {
                    if (!(m_carver.y + 2 >= m_height))
                    {
                        m_passMoveCheck = true;
                        m_movement = Movements.UP;
                        m_priorPosition = m_ancors.Peek();
                        m_ancors.Push(new Vector2(m_carver.x, m_carver.y));
                    }
                    else
                    {
                        cantMove3 = true;
                    }
                }
                else if (random == 1 && !(((int)m_carver.x, (int)m_carver.y - 2) == ((int)m_priorPosition.x, (int)m_priorPosition.y)))
                {
                    if (!(m_carver.y - 2 <= 1))
                    {
                        m_passMoveCheck = true;
                        m_movement = Movements.DOWN;
                        m_priorPosition = m_ancors.Peek();
                        m_ancors.Push(new Vector2(m_carver.x, m_carver.y));
                    }
                    else
                    {
                        cantMove4 = true;
                    }
                }
                
                if (cantMove1 && cantMove2 && cantMove3 && cantMove4)
                {
                    m_priorPosition = m_ancors.Peek();
                    m_ancors.Pop();
                    m_carver = m_ancors.Peek();
                }

            }

            // decidedes which direction the maze will be carved out by the vector
            switch (m_movement)
            {
                case Movements.UP:
                    m_carver = new Vector2(m_carver.x, m_carver.y + 2);
                    m_maze[(int)m_carver.x, (int)m_carver.y - 1] = true;
                    break;
                case Movements.DOWN:
                    m_carver = new Vector2(m_carver.x, m_carver.y - 2);
                    m_maze[(int)m_carver.x, (int)m_carver.y + 1] = true;
                    break;
                case Movements.LEFT:
                    m_carver = new Vector2(m_carver.x - 2, m_carver.y);
                    m_maze[(int)m_carver.x + 1, (int)m_carver.y] = true;
                    break;
                case Movements.RIGHT:
                    m_carver = new Vector2(m_carver.x + 2, m_carver.y);
                    m_maze[(int)m_carver.x - 1, (int)m_carver.y] = true;
                    break;
                default:
                    break;
            }

            Debug.Log(++runs);
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
                    //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //sphere.GetComponent<Renderer>().material.color = Color.red;
                    //sphere.transform.position = new Vector3(x, y);
                }
            }
        }
    }
}
