using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public string axiom = "A";
    public List<LSystemRule> rules = new List<LSystemRule>();
    [Range(0, 7)]public int iterations = 3;
    public int seed = 23;

    readonly List<Vector3> starts = new();
    readonly List<Vector3> ends = new();
    readonly List<Vector3> leaves = new();

    string sentence = string.Empty;

    public float length = 3f;
    public int angle = 90;
    [Range(0f, 180f)] public float rollAngle = 120f;

    [Header("Prefabs")]
    public GameObject branchPrefab;
    public GameObject leafPrefab;

    public GameObject parts;
    public GameObject tree;

    public float branchWidth = 0.5f;
    public float leafSize = 1f;

    [ContextMenu("Generate")]
    public void Generate()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        sentence = LSystem.Generate(axiom, rules, iterations, seed);
        CreateTreeLines();
        BuildParts();
        Debug.Log(sentence.Length + " symbols, " + watch.ElapsedMilliseconds + " ms");
        Debug.Log(sentence);
    }

    void CreateTreeLines()
    {
        starts.Clear();
        ends.Clear();
        leaves.Clear();
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        Stack<Vector3> storedPos = new();
        Stack<Quaternion> storedRot = new();

        // GOAL - remove end points that match up with start positions
        // need to check if the end points are valid spawn points for leaves
        // if end point is in the same place as start point - remove end point

        foreach(char c in sentence)
        {
            switch (c) 
            {
                case 'F': // move forward
                    Vector3 end = pos + (rot * Vector3.up) * length;
                    starts.Add(pos);
                    ends.Add(end);
                    pos = end;
                    break;
                case '+': // turn left                   
                    rot *= Quaternion.Euler(0, 0, angle);
                    break;
                case '-': // turn right
                    rot *= Quaternion.Euler(0, 0, -angle);
                    break;
                case '[': // save pos and rot
                    storedPos.Push(pos);
                    storedRot.Push(rot);
                    break;
                case ']': // revert to last pos and rot
                    if (storedPos.Count <= 0 && storedRot.Count <= 0) { return; }
                    pos = storedPos.Pop();
                    rot = storedRot.Pop();
                    break;
                case '/':
                    rot *= Quaternion.Euler(0, rollAngle, 0);
                    break;
                case 'X':
                    leaves.Add(pos);
                    break;
            }
        }
        Debug.Log(starts.Count + " Lines");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        // for every line
        for (int i = 0; i < starts.Count; i++)
        {
            Gizmos.DrawLine(transform.position + starts[i], transform.position + ends[i]);
        }
    }

    void PlaceBranch(int i)
    {
        Vector3 direction = ends[i] - starts[i];
        Transform t = Instantiate(branchPrefab, tree.transform).transform;

        t.localPosition = (starts[i] + ends[i]) / 2f;                           // the middle
        t.localRotation = Quaternion.FromToRotation(Vector3.up, direction);     // point along the line
        t.localScale = new(branchWidth, direction.magnitude / 2f, branchWidth); // thin and long

    }

    void PlaceLeaves(Vector3 leafPos)
    {
        Transform l = Instantiate(leafPrefab, parts.transform).transform;
        l.localPosition = leafPos;
        l.localScale = Vector3.one * leafSize;
    }

    public void BuildParts()
    {
        ClearChildren();
        // for every leaf position - Instantiate leafPrefab in parts object
        // for every end point - make leaf
        for (int i = 0; i < starts.Count; i++)
        {
            PlaceBranch(i);
        }
        foreach (var leaf in leaves)
        {
            PlaceLeaves(leaf);
        }

        Debug.Log("Created Branches and Leaves");
    }

    [ContextMenu("Clear Children")]
    public void ClearChildren()
    {
        // leaf children
        while (parts.transform.childCount > 0)
        {
            if (Application.isPlaying)
            {
                Destroy(parts.transform.GetChild(0).gameObject);
            }
            else
            {
                DestroyImmediate(parts.transform.GetChild(0).gameObject);
            }
        }

        // tree children
        while(tree.transform.childCount > 0)
        {
            if (Application.isPlaying)
            {
                Destroy(tree.transform.GetChild(0).gameObject);
            }
            else
            {
                DestroyImmediate(tree.transform.GetChild(0).gameObject);
            }
        }
    }
}
