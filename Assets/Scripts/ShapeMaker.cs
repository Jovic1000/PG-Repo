using UnityEngine;
using UnityEngine.UIElements;

public static class ShapeMaker
{
    public static Mesh CreateTriangle(float scale = 1)
    {
        Mesh m_newTriangle = new Mesh();
        Vector3[] verts = new Vector3[3];
        int[] order = {0, 2, 1};

        verts[0] = new(0, 0, 0);
        verts[1] = new(scale, 0, 0);
        verts[2] = new(0, 0, scale);

        // assigns the variables to the mesh
        m_newTriangle.vertices = verts;
        m_newTriangle.triangles = order;
        m_newTriangle.RecalculateNormals();

        return m_newTriangle;
    }

    public static Mesh CreateSquare(float scale = 1)
    {
        Mesh m_newSquare = new Mesh();

        Vector3[] verts = new Vector3[4];
        int[] order = {0, 2, 3, 0, 3, 1};

        verts[0] = new(0, 0, 0);
        verts[1] = new(scale, 0, 0);
        verts[2] = new(0, 0, scale);
        verts[3] = new(scale, 0, scale);


        m_newSquare.vertices = verts;
        m_newSquare.triangles = order;
        m_newSquare.RecalculateNormals();

        return m_newSquare;
    }

    public static Mesh CreateSquareMap(float scale = 1)
    {
        Mesh m_newMesh = new Mesh();

        Vector3[] verts = new Vector3[4];
        int[] order = { 0, 2, 3, 0, 3, 1 };

        verts[0] = new(0, 0, 0);
        verts[1] = new(scale, 0, 0);
        verts[2] = new(0, 0, scale);
        verts[3] = new(scale, 0, scale);

        m_newMesh.vertices = verts;
        m_newMesh.triangles = order;
        m_newMesh.RecalculateNormals();

        return m_newMesh;
    }
}
