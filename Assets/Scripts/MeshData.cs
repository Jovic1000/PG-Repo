using UnityEngine;

public class MeshData : MonoBehaviour
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;
    public int m_hight;
    public int m_width;
    int triangleIndex;

    public MeshData(int width, int height)
    {
        vertices = new Vector3[(width * height)];
        uvs = new Vector2[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }
    public Mesh CreateMesh()
    {
        Mesh m_newMesh = new Mesh();

        m_newMesh.vertices = vertices;
        m_newMesh.triangles = triangles;
        m_newMesh.uv = uvs;

        m_newMesh.RecalculateNormals();

        return m_newMesh;
    }

}
