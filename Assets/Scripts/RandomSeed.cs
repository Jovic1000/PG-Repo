using UnityEngine;

public class RandomSeed : MonoBehaviour
{
    System.Random rand = new System.Random(23);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 5; i++)
        { 
            Debug.Log(rand.Next(0, 10));
        }
    }
}
