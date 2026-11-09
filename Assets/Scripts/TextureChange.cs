using TreeEditor;
using UnityEngine;

public class TextureChange : MonoBehaviour
{
    [Range(1, 8)] public int octaves = 1;

    [Range(0f, 1f)] public float persistence = 0.5f;

    [Range(1f, 4f)] public float lacunarity = 2f;

    public float scale;

    public void OnValidate()
    {
        if (octaves < 1)
        {
            octaves = 1;
        }
        if (scale < 1)
        {
            scale = 1;
        }
    }
    // heightMap = Noise.GenerateNoiseMap(mapSize, mapSize, seed, scale, octaves, persistence, lacunarity, offset); 

    public static float[,] GenerateNoiseMap(

    int width, int height, int seed,

    float scale, int octaves, float persistence, float lacunarity, Vector2 offset)

    {

        float[,] map = new float[width, height];



        // GUARD scale > 0, octaves >= 1. 



        // 1. SEED: one System.Random(seed). Make an offset for EACH octave: 

        //        Vector2[] octaveOffsets = new Vector2[octaves]; 

        //        octaveOffsets[o] = new Vector2(rng.Next(1000, 10000) + offset.x, 

        //                                       rng.Next(1000, 10000) + offset.y); 

        //    Why? Otherwise every layer samples the same place and just stacks up. 



        // 2. TRACK the lowest and highest value: 

        //        float minHeight = float.MaxValue;  float maxHeight = float.MinValue; 



        // 3. LOOP y, then x. For each pixel: 

        //        float amplitude = 1;  float frequency = 1;  float noiseHeight = 0; 

        // 

        //        LOOP o from 0 to octaves - 1: 

        //          a. float sampleX = (x - width / 2f) / scale * frequency + octaveOffsets[o].x; 

        //             float sampleY = (y - height / 2f) / scale * frequency + octaveOffsets[o].y; 

        //          b. float p = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;   // now -1 .. 1 

        //          c. noiseHeight += p * amplitude; 

        //          d. amplitude *= persistence;     // quieter 

        //             frequency *= lacunarity;      // busier 

        // 

        //        Update minHeight / maxHeight, then:   map[x, y] = noiseHeight; 



        // 4. NORMALISE: loop every value again: 

        //        map[x, y] = Mathf.InverseLerp(minHeight, maxHeight, map[x, y]); 



        return map;

    }
}

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