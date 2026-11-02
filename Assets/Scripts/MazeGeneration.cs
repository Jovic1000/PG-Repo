using System.Collections.Generic;
using UnityEngine;

public class MazeGeneration : MonoBehaviour
{
    [SerializeField] private int m_width;
    [SerializeField] private int m_height;
    [SerializeField] private int m_steps;
    [SerializeField] private int m_speed;
    [SerializeField] private int m_seed;

    private Stack<Vector2> m_ancors;

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

    private void DrawSteps()
    {
        /*m_mike.transform.position = new Vector3(m_width / 2, m_height / 2);
        m_mike.GetComponent<Renderer>().material.color = Color.gold;*/

        Vector2 m_matt = new Vector2(m_width / 2, m_height / 2);

        while (m_steps > 0)
        {
            // which random direction could we go
            // put this new pos into m_matt
            Movements m_move = (Movements)m_rand.Next(0, 4);

            switch (m_move)
            {
                case Movements.UP:
                    m_matt = new Vector2(m_matt.x, m_matt.y + 1);
                    break;
                case Movements.DOWN:
                    m_matt = new Vector2(m_matt.x, m_matt.y - 1);
                    break;
                case Movements.LEFT:
                    m_matt = new Vector2(m_matt.x - 1, m_matt.y);
                    break;
                case Movements.RIGHT:
                    m_matt = new Vector2(m_matt.x + 1, m_matt.y);
                    break;
                default:
                    break;
            }

            m_maze[(int)m_matt.x, (int)m_matt.y] = true;

            m_steps--;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rand = new System.Random(m_seed);
        m_mike = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        m_maze = new bool[m_width, m_height];

        /*for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
            {
                m_maze[x, y]
            }
        }*/

        DrawSteps();
        DisplayMaze();
    }

    private void DisplayMaze()
    {
        for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++) 
            {
                if (m_maze[x, y])
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    cube.transform.position = new Vector3(x,y);
                }
                else
                {
                    // makes ety space red orbs
                    /*GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.GetComponent<Renderer>().material.color = Color.red;
                    sphere.transform.position = new Vector3(x, y);*/
                }
            }
        }
    }

    private void MikeMove()
    {
        switch (m_movement) 
        {
            case Movements.UP:
                m_mike.transform.position = new Vector3(m_mike.transform.position.x, m_mike.transform.position.y + 1);
                break;
            case Movements.DOWN:
                m_mike.transform.position = new Vector3(m_mike.transform.position.x, m_mike.transform.position.y - 1);
                break;
            case Movements.LEFT:
                m_mike.transform.position = new Vector3(m_mike.transform.position.x - 1, m_mike.transform.position.y);
                break;
            case Movements.RIGHT:
                m_mike.transform.position = new Vector3(m_mike.transform.position.x + 1, m_mike.transform.position.y);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
     /*   switch (m_rand.Next(0, 3))
        {
            case 0:
                m_movement = Movements.UP;
                 break;
            case 1:
                m_movement = Movements.DOWN;
                break;
            case 2:
                m_movement = Movements.LEFT;
                break;
            case 3:
                m_movement = Movements.RIGHT;
                break;
            default:
                break;
        }

        MikeMove();*/
    }
}
