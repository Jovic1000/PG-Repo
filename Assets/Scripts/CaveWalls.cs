using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CaveWalls : MonoBehaviour
{
    public Caving cave;
    [Range(1f, 10f)] public float wallHeight = 3f;
    List<Vector3> vertices = new();
    List<int> triangles = new();

    private MeshCollider collider;
    private Mesh mesh;

    [ContextMenu("Build Walls")]
    public void BuildWalls()
    {
        if (cave == null || cave.GetMap() == null) return;
        vertices.Clear();
        triangles.Clear();

        for (int x = 0; x < cave.GetWidth() - 2; x++)
        {
            for (int y = 0; y < cave.GetHeight() - 2; y++)
            {
                int c = cave.GetCaseIndex(x, y);

                Vector3 bottom = new(x + 0.5f, wallHeight, y);
                Vector3 right = new(x + 1, wallHeight, y + 0.5f);
                Vector3 top = new(x + 0.5f, wallHeight, y + 1);
                Vector3 left = new(x, wallHeight, y + 0.5f);
                AddCaseWalls(c, bottom, right, top, left);
            }
        }
        ApplyMesh();
    }

    void AddWall(Vector3 from, Vector3 to)
    {
        int s = vertices.Count; // number of the first new vertex
        Vector3 up = Vector3.up * wallHeight;

        vertices.Add(from); // s - bottom of 'from'
        vertices.Add(from + up); // s + 1 - top of 'from'

        vertices.Add(to); // s - bottom of 'from'
        vertices.Add(to + up); // s + 1 - top of 'from'

        triangles.Add(s); triangles.Add(s + 1); triangles.Add(s + 2);
        triangles.Add(s + 1); triangles.Add(s + 3); triangles.Add(s + 2);
    }

    void AddCaseWalls(int c, Vector3 bottom, Vector3 right, Vector3 top, Vector3 left)
    {
        switch (c)
        {
            case 1: AddWall(bottom, left); break;
            case 2: AddWall(right, bottom); break;
            case 3: AddWall(right, left); break;
            case 4: AddWall(top, right); break;
            case 5: AddWall(left, top); AddWall(right, bottom); break;
            case 6: AddWall(top, bottom); break;
            case 7: AddWall(top, left); break;
            case 8: AddWall(left, top); break;
            case 9: AddWall(bottom, top); break;
            case 10: AddWall(right, top); AddWall(left, bottom); break;
            case 11: AddWall(right, top); break;
            case 12: AddWall(left, right); break;
            case 13: AddWall(bottom, right); break;
            case 14: AddWall(left, bottom); break;
        }
    }

    void ApplyMesh()
    {
        if (mesh == null) { mesh = new() { name = "Cave Walls" }; } // make new mesh if none available
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        collider = GetComponent<MeshCollider>();
        collider.sharedMesh = null; // let go of the old shape
        collider.sharedMesh = mesh; // rebuild the new mesh
    }
}
