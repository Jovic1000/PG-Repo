using Unity.Mathematics;
using UnityEngine;
[RequireComponent (typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class ShapeSpawner : MonoBehaviour
{

    [Range(1, 8)] public int octaves;

    [Range(0f, 1f)] public float persistence;

    [Range(1f, 4f)] public float lacunarity;

    public float scale;

    public void OnValidate()
    {
        if (octaves < 1) { octaves = 1; }
        if (scale < 1) { scale = 1; }
        if (persistence < 0.5f) { persistence = 0.5f; }
        if (lacunarity < 2f) { lacunarity = 2f; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshData data = MeshGenerator.GenerateTerrainMesh(Noise.GenerateNoiseMap(20, 20, 23, scale, octaves, persistence, lacunarity, new Vector2(1, 1)), 7f);

        GetComponent<MeshFilter>().mesh = data.CreateMesh();

    }

}