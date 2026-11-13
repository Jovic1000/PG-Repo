using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier)
    {
        int m_width = heightMap.GetLength(0);
        int m_height = heightMap.GetLength(1);

        // 2. CENTRE the mesh on the origin:
        float offsetX = (m_width - 1) / -2f;
        float offsetZ = (m_height - 1) / -2f;

        MeshData data = new MeshData(m_width, m_height);
        int vertexIndex = 0;

        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++) 
            {
                data.vertices[vertexIndex] = new Vector3(offsetX + x, heightMap[x, y] * heightMultiplier, offsetZ + y);
                data.uvs[vertexIndex] = new Vector2(x / (float)(m_width - 1), y / (float)(m_height - 1));

                if (x < m_width - 1 && y < m_height - 1)
                {
                    int i = vertexIndex;
                    data.AddTriangle(i, i + m_width, i + m_width + 1);
                    data.AddTriangle(i, i + m_width + 1, i + 1);

                }
                vertexIndex++;
            }
        }

        return data;
    }
}
