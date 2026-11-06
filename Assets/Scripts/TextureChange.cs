using UnityEngine;

public class TextureChange : MonoBehaviour
{
    private void Start()
    {
        System.Random r = new System.Random(23);
        Texture2D texture = new(64, 64);
        texture.filterMode = FilterMode.Point;

        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 64; j++)
            {
                double v = Mathf.PerlinNoise(i / 64 + 23, j / 64 + 23);
                texture.SetPixel(i, j, new Color((float)v, (float)v, (float)v));
            }
        }
        texture.Apply();
        GetComponent<Renderer>().material.mainTexture = texture;

        // shading
        
        
    }

}
