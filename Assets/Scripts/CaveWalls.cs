using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CaveWalls : MonoBehaviour
{
    public Caveing cave = new Caveing();
    [Range(1f, 10f)] public float wallHeight = 3f;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    Mesh mesh;

    [ContextMenu("Build walls")]
    public void BuildWalls()
    {
        if (cave == null || cave.GetMap() == null)
        {
            return;
        }

        // 2. clear both lists --> optional (use for constant changing walls)

        // 3. loop x from 0 to cave.width - 2, then y from 0 to cave.height - 2
        for (int x = 0; x < cave.GetWidth() - 2; x++)
        {
            for (int y = 0; y < cave.GetHeight() - 2; y++)
            {
                 //the case for this square
                 //the four midpoints ON THE FLOOR, for example:

                int c = cave.GetCaseIndex(x, y);

                Vector3 b = new Vector3(x + 0.5f, 0, y);
                Vector3 r = new Vector3(x + 1f, y + 0.5f);
                Vector3 t = new Vector3(x + 0.5f, y + 1f);
                Vector3 l = new Vector3(x, y + 0.5f);

                AddCaseWalls(c, b, r, t, l);
            }
        }

      ApplyMesh();
    }

    void AddWall(Vector3 from, Vector3 to)
    {
        int s = vertices.Count;              // number of the first new vertex
        Vector3 up = Vector3.up * wallHeight;

        // s      bottom of 'from'
        // s + 1  top of 'from'
        // s + 2  top of 'to'
        // s + 3  bottom of 'to'

        // triangle 1: s, s + 1, s + 2
        // triangle 2: s, s + 2, s + 3
    }

    void AddCaseWalls(int c, Vector3 b, Vector3 r, Vector3 t, Vector3 l)
    {
        // every other case from your arrow table, one per line
        // case 5 and case 10: two AddWall calls each
        // 0 and 15: nothing

        switch (c)
        {
            case 1:case 14:
                Gizmos.DrawLine(b, l); break;
            case 2:case 13:
                Gizmos.DrawLine(b, r); break;
            case 3:case 12:
                Gizmos.DrawLine(r, l); break;
            case 4:case 11:
                Gizmos.DrawLine(t, r); break;
            case 5:
                Gizmos.DrawLine(l, t); Gizmos.DrawLine(b, r); break;
            case 6:case 9:
                Gizmos.DrawLine(t, b); break;
            case 7:case 8:
                Gizmos.DrawLine(t, l); break;
            case 10:
                Gizmos.DrawLine(t, l); Gizmos.DrawLine(b, r); break;

            default:
                break;
        }
    }

    void ApplyMesh()
    {
        // 1. if mesh is null: make a new Mesh and give it a name

        if (mesh == null)
        {
            mesh = new Mesh();
        }

        mesh.Clear();
        // 3. vertices and triangles into the mesh (.ToArray())
        // 4. mesh.RecalculateNormals();
        // 5. GetComponent<MeshFilter>().sharedMesh = mesh;
        // 6. the MeshCollider: set its sharedMesh to null, then to mesh
    }

}
