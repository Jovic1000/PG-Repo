using Unity.Mathematics;
using UnityEngine;
[RequireComponent (typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class ShapeSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshData data = MeshGenerator.GenerateTerrainMesh(Noise.GetNoise(), 1f);
        
        GetComponent<MeshFilter>().mesh = data.CreateMesh();

    }

}