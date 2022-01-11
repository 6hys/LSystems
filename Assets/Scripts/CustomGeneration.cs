using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomGeneration : MonoBehaviour
{
    public InputField m_Axiom;
    public Toggle m_ContextToggle;
    public InputField m_IgnoredSymbols;
    public GameObject m_CFList;
    public GameObject m_CSList;
    public InputField m_Angle;
    public Slider m_Generations;
    public Slider m_Randomness;
    public Slider m_Length;

    public LSystemController m_LSC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateLSystem()
    {
        LSystem lSystem = new LSystem();

        lSystem.axiom = m_Axiom.text;
        lSystem.contextFree = m_ContextToggle.isOn ? false : true;

        if(lSystem.contextFree)
        {
            // Get m_CFList.
            foreach(Transform child in m_CFList.transform)
            {
                Transform key = child.GetChild(0);
                InputField keyField = key.GetComponentInChildren<InputField>();
                if(keyField.text != "")
                {
                    char c = keyField.text.ToCharArray()[0];
                    lSystem.rules.Add(c, new List<Rule>());

                    Transform rules = child.GetChild(1);
                    foreach(Transform r in rules)
                    {
                        bool isValid = true;

                        float probability = 0f;
                        string rule = "";

                        InputField prob = r.GetChild(0).GetComponentInChildren<InputField>();
                        if(prob.text != "")
                        {
                            bool parsed = float.TryParse(prob.text, out probability);
                            if(probability <= 0f || parsed == false)
                                isValid = false;
                        }
                        else
                            isValid = false;

                        InputField ruleField = r.GetChild(1).GetComponentInChildren<InputField>();
                        if(ruleField.text != "")
                            rule = ruleField.text;
                        else
                            isValid = false;

                        if (isValid)
                        {
                            lSystem.rules[c].Add(new Rule(probability, rule));
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("CS List");
            // Get m_CSList and ignore list
            foreach(Transform child in m_CSList.transform)
            {
                Debug.Log("Rule: " + child.name);
                Transform key = child.GetChild(0);
                InputField keyField = key.GetComponentInChildren<InputField>();
                if (keyField.text != "")
                {
                    char c = keyField.text.ToCharArray()[0];
                    lSystem.contextRules.Add(c, new List<ContextRule>());
                    Debug.Log("Key: " + c);
                    Transform rules = child.GetChild(1);
                    foreach(Transform r in rules)
                    {
                        bool isValid = true;

                        string before = "";
                        string after = "";
                        string rule = "";

                        InputField b = r.GetChild(0).GetComponentInChildren<InputField>();
                        if (b.text != "")
                            before = b.text;
                        else
                            isValid = false;

                        Debug.Log("Before: " + before);
                        InputField a = r.GetChild(1).GetComponentInChildren<InputField>();
                        if (a.text != "")
                            after = a.text;
                        else
                            isValid = false;

                        Debug.Log("After: " + after);
                        InputField ruleField = r.GetChild(2).GetComponentInChildren<InputField>();
                        if (ruleField.text != "")
                        {
                            rule = ruleField.text;
                            Debug.Log("Rule: " + rule);
                        }
                        else
                            isValid = false;

                        if(isValid)
                        {
                            Debug.Log("Is valid, adding context rule");
                            lSystem.contextRules[c].Add(new ContextRule(before, after, rule));
                        }
                    }
                }
            }

            if (m_IgnoredSymbols.text != "")
            {
                string[] ignoreList = m_IgnoredSymbols.text.Split(","[0]);
                foreach (string s in ignoreList)
                {
                    if (s != "")
                    {
                        Debug.Log("Adding " + s + " to ignore list");
                        lSystem.ignoreList.Add(s.ToCharArray()[0]);
                    }
                }
            }

            lSystem.lenDiv = 1.1f;
        }

        if (float.TryParse(m_Angle.text, out lSystem.angle) == false)
        {
            lSystem.angle = 0f;
        }

        lSystem.maxGenerations = (int)m_Generations.maxValue;

        m_LSC.CustomGenerate(lSystem, (int)m_Generations.value, (int)m_Randomness.value, m_Length.value);
    }
}
