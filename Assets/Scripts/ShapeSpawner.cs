using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent (typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class ShapeSpawner : MonoBehaviour
{
    [System.Serializable]

    public struct TerrainType
    {
        public string name;
        public float height;    // this band goes UP TO this height 
        public Color color;
    }

    public AnimationCurve m_fallOfCurve;

    public TerrainType[] regions;    // fill this in the Inspector, LOWEST first 

    [SerializeField] float m_size;

    [Range(1, 8)] public int octaves;

    [Range(2f, 256f)] public int m_height;

    [Range(2f, 256f)] public int m_width;

    [Range(0f, 1f)] public float persistence;

    [Range(1f, 4f)] public float lacunarity;

    TextureChange changer;

    float[,] m_noiseMap;
    public float scale;

    public void OnValidate()
    {
        if (octaves < 1) { octaves = 1; }
        if (scale < 1) { scale = 1; }
        if (persistence < 0.5f) { persistence = 0.5f; }
        if (lacunarity < 2f) { lacunarity = 2f; }
        if (m_size < 0f) { m_size = 0f; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        changer = new TextureChange();

        m_noiseMap = Noise.GenerateNoiseMap(m_width, m_height, 23, scale, octaves, persistence, lacunarity, new Vector2(1, 1));

        MeshData data = MeshGenerator.GenerateTerrainMesh(m_noiseMap, m_size);

        Texture2D texture = changer.CreateNoiseTexture(m_noiseMap);

        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;

        GetComponent<MeshFilter>().mesh = data.CreateMesh();

        GetComponent<MeshCollider>().sharedMesh = data.CreateMesh();

    }

    private void GenerateColour(float[,] noiseMap, Texture2D texture)
    {
        for (int i = 0; i < noiseMap.GetLength(0); i++)
        {
            for(int j = 0; j < noiseMap.GetLength(1); j++)
            {
                Color colorMap = CheckColorHeight(m_fallOfCurve.Evaluate(m_noiseMap[i, j]));
                texture.SetPixel(i, j, colorMap);
            }
        }
    }

    private Color CheckColorHeight(float height)
    {
        for (int i = 0; i < regions.Length; i++)
        {
            if (height <= regions[i].height) return regions[i].color;
        }
        return regions[regions.Length - 1].color;
    }

}