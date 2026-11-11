using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        float[,] map = new float[width, height];
        System.Random rand = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        // Make an offset for EACH octave: 
        for (int i = 0; i < octaves; i++)
        {
            octaveOffsets[i] = new Vector2(rand.Next(1000, 10000) + offset.x, rand.Next(1000, 10000) + offset.y);
        }

        //LOOPS y, then x. For each pixel
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //LOOP o from 0 to octaves - 1: 
                for (int o = 0; o < octaves; o++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    float sampleX = (x - width / 2f) / scale * frequency + octaveOffsets[o].x;

                    float sampleY = (y - height / 2f) / scale * frequency + octaveOffsets[o].y;

                    float p = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;   // now -1 .. 1 

                    noiseHeight += p * amplitude;

                    amplitude *= persistence;     // quieter 

                    frequency *= lacunarity;      // busier 

                    if (minHeight > noiseHeight) minHeight = noiseHeight;
                    if (maxHeight < noiseHeight) maxHeight = noiseHeight;

                    map[x, y] = noiseHeight;
                }
            }
        }



        // 4. NORMALISE: loop every value again: 

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = Mathf.InverseLerp(minHeight, maxHeight, map[x, y]);
            }
        }

        return map;

    }
}
