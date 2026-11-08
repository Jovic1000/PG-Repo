using TreeEditor;
using UnityEngine;

public static class TextureChange
{
    public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        float[,] map = new float[width, height];
        // GUARD scale > 0, octave >= 1
        if (scale <= 0) scale = 1;


        System.Random rng = new System.Random(seed);

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++) // offset for each octave
        {
            octaveOffsets[i] = new Vector2(rng.Next(1000, 10000) + offset.x,
                                           rng.Next(1000, 10000) + offset.y);
        }
        for (int y = 0; y < height; y++) // loop through pixels
        {
            for (int x = 0; x < width; x++)
            {
                for (int o = 0; o < octaves - 1; o++) // loop through octaves
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    float sampleX = (x - width / 2f) / scale * frequency + octaveOffsets[o].x;
                    float sampleY = (y - height / 2f) / scale * frequency + octaveOffsets[o].y;

                    float p = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += p * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;

                    if (minHeight > noiseHeight) minHeight = noiseHeight;
                    if (maxHeight < noiseHeight) maxHeight = noiseHeight;

                    map[x, y] = noiseHeight;
                }
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = Mathf.InverseLerp(minHeight, maxHeight, map[x, y]);
            }
        }

        return map;
    }
// old code for basic noise map
//[SerializeField] private int m_width;
//[SerializeField] private int m_height;
//[SerializeField] private int m_seed;
//[SerializeField] private float m_scale;

//private void Start()
//{
//    m_scale = m_scale / 100;
//    System.Random r = new System.Random(m_seed);
//    Texture2D texture = new(m_width, m_height);
//    texture.filterMode = FilterMode.Point;

//    // sets idavidual pixel colour when iterating
//    for (int i = 0; i < m_width; i++)
//    {
//        for (int j = 0; j < m_height; j++)
//        {
//            //creates a noise map \/ \/ \/
//            float v = Mathf.PerlinNoise(((float)i / m_width + m_seed) / m_scale, ((float)j / m_height + m_seed) / m_scale);

//            // create white noise \/ \/ \/
//            //float v = (float)r.NextDouble();
//            texture.SetPixel(i, j, new Color(v, v, v));
//        }
//    }

//    texture.Apply();
//    GetComponent<Renderer>().material.mainTexture = texture;
//}
}
