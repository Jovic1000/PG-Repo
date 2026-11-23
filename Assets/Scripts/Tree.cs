
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class Tree : MonoBehaviour
{
    public List<LSyestemRule> rules = new List<LSyestemRule>();

    [SerializeField] private string axiom;
    [SerializeField] private int iterations;
    [SerializeField] private int seed;
    [SerializeField] private bool drawGizmo;

    Vector3 m_basePosition = new Vector3(0, 0, 0);
    Vector3 m_rotation = new Vector3(0, 0, 0);
    float angle = 45;
    string current;

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            current = LSystem.Generate(axiom, rules, iterations, seed);

            foreach (char c in current)
            {
                Vector3 priorPosition = m_basePosition;
                switch (c)
                {
                    case 'F':
                    case 'X':
                        m_basePosition += new Vector3(0, 1, 0);
                        break;

                    case '-':
                        m_rotation += new Vector3(0, 0, -angle);
                        break;

                    case '+':
                        m_rotation += new Vector3(0, 0, angle);
                        break;

                    default:
                        break;
                }

                Gizmos.DrawLine(priorPosition, m_basePosition + m_rotation);
            }
        }
    }
}

[System.Serializable]
public class LSyestemRule
{
    public char symbol; // the symble to replace: F
    public string replacement; // what it becomes: FF


}

public static class LSystem
{
    public static string Generate (string axiom, List<LSyestemRule> rules, int iterations, int seed)
    {
        string current = axiom;
        

        for (int i = 0; i < iterations; i++)
        {
            // a NEW string: never change the one Your Reading
            string next = "";

            foreach (char c in current)
            {

                // add what c becomes to the end of next:
                next += Replace(c, rules);

            }

            // Swap: current becomes next
            current = next;
        }

      


        Debug.Log("Current: " + current);
        return current;
    }

    static string Replace(char c, List<LSyestemRule> rules)
    {
        // look at every rule:
        //    if its symbol is c, return its replacement
        for (int i = 0; i < rules.Count; i++)
        {
            if (c == rules[i].symbol)
            { 
                return rules[i].replacement;
            }
        }

        // no rule matched: c is a constant

        return "Missing string";
    }

}
