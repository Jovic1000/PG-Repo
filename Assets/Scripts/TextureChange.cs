using UnityEngine;

public class TextureChange : MonoBehaviour
{
    // heightMap = Noise.GenerateNoiseMap(mapSize, mapSize, seed, scale, octaves, persistence, lacunarity, offset); 

    [Range(1, 8)] public int octaves;
    [Range(0f, 1f)] public float persistence;
    [Range(1f, 4f)] public float lacunarity;

    [SerializeField] private int m_width;
    [SerializeField] private int m_height;
    [SerializeField] private int m_seed;
    [SerializeField] private float m_scale;

    public void OnValidate()
    {
        if (octaves < 1) {octaves = 1;}
        if (persistence < 0.5f) {persistence = 0.5f;}
        if (lacunarity < 2f) {lacunarity = 2f;}
    }

    private void Start()
    {
        m_scale = m_scale / 100;
        System.Random r = new System.Random(m_seed);
        Texture2D texture = new(m_width, m_height);
        texture.filterMode = FilterMode.Point;

        // sets idavidual pixel colour when iterating

        float[,] z = Noise.GenerateNoiseMap(m_width, m_height, m_seed, m_scale, octaves, persistence, lacunarity, new Vector2(2, 2));

        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                //creates a noise map \/ \/ \/
                //float v = Mathf.PerlinNoise(((float)i / m_width + m_seed) / m_scale, ((float)j / m_height + m_seed) / m_scale);

                // create white noise \/ \/ \/
                //float v = (float)r.NextDouble();

                float v = z[i, j];


                texture.SetPixel(i, j, new Color(v, v, v));
            }
        }

        texture.Apply();
        GetComponent<Renderer>().material.mainTexture = texture;
    }
}
