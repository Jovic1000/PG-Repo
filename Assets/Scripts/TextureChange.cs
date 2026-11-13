using UnityEngine;

public class TextureChange : MonoBehaviour
{
    // heightMap = Noise.GenerateNoiseMap(mapSize, mapSize, seed, scale, octaves, persistence, lacunarity, offset); 
    public enum DrawMode { NoiseMap, ColourMap, FalloffMap }
    public DrawMode drawMode = DrawMode.NoiseMap;


    [Range(1, 8)] public int octaves;
    [Range(0f, 1f)] public float persistence;
    [Range(1f, 4f)] public float lacunarity;

    [SerializeField] private int m_width;
    [SerializeField] private int m_height;
    [SerializeField] private int m_seed;
    [SerializeField] private float m_scale;

    public void OnValidate()
    {
        if (octaves < 1) { octaves = 1; }
        if (persistence < 0.5f) { persistence = 0.5f; }
        if (lacunarity < 2f) { lacunarity = 2f; }
        if (m_scale <= 0f) { m_scale = 0.0001f; }
    }

    public Texture2D CreateNoiseTexture(float[,] noiseMap)
    {
        Texture2D texture = new(noiseMap.GetLength(0), noiseMap.GetLength(1));
        texture.filterMode = FilterMode.Point;

        // sets idavidual pixel colour when iterating

        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                //creates a noise map \/ \/ \/
                //float v = Mathf.PerlinNoise(((float)i / m_width + m_seed) / m_scale, ((float)j / m_height + m_seed) / m_scale);

                // create white noise \/ \/ \/
                //float v = (float)r.NextDouble();

                float v = noiseMap[i, j];

                texture.SetPixel(i, j, new Color(v, v, v));
            }
        }

        texture.Apply();
        return texture;
    }
}
