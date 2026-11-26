using System.Collections.Generic;
using System.Text;
public static class LSystem
{
    public static string Generate(string axiom, List<LSystemRule> rules, int iterations, int seed)
    {
        string current = axiom;

        for(int i = 0; i < iterations; i++)
        {
            StringBuilder next = new();
            foreach (char c in current)
            {
                // add what c becomes to the end of next
                next.Append(Replace(c, rules));
            }
            current = next.ToString();
        }

        return current;
    }

    static string Replace(char c, List<LSystemRule> rules)
    {
        if(rules.Count == 0) { return ""; }
        // if the symbol is c, return replacement
        foreach (LSystemRule rule in rules) 
        {
            if(rule.symbol == c)
            {
                return rule.replacement;
            }
        }
        // no rule matched: c is a constant
        return c.ToString();
    }
}

[System.Serializable]
public class LSystemRule
{
    public char symbol; // what will be replaced : F
    public string replacement; // what the symbol will become
}