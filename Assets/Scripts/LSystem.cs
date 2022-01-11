using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem
{
    public string axiom;

    public Dictionary<char, List<Rule>> rules = new Dictionary<char, List<Rule>>();

    public Stack<TransformInfo> transformStack = new Stack<TransformInfo>();
    public float angle;
    public int maxGenerations;
    public float lenDiv = 2f;

    public bool contextFree;
    public Dictionary<char, List<ContextRule>> contextRules = new Dictionary<char, List<ContextRule>>();
    public List<char> ignoreList = new List<char>();
}

public class Rule
{
    public float probability;
    public string rule;

    public Rule(float prob, string r)
    {
        probability = prob;
        rule = r;
    }
}

public class ContextRule
{
    public string before;
    public string after;
    public string rule;

    public ContextRule(string b, string a, string r)
    {
        before = b;
        after = a;
        rule = r;
    }
}
