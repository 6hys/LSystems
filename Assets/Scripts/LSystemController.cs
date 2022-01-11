using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSystemController : MonoBehaviour
{
    public LSystem currentLSystem = new LSystem();

    private List<LSystem> m_Presets = new List<LSystem>();
    public float m_Length;
    private string m_CurrentString;

    public GameObject m_LinePrefab;
    private List<LineRenderer> m_Lines = new List<LineRenderer>();

    public Vector3 startPosition;
    public Quaternion startRotation;

    private int m_Randomness;
    private int m_Generations;
    public Slider m_GenSlider;
    public Slider m_RandSlider;
    public Slider m_LenSlider;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        LoadPresets();
        currentLSystem = m_Presets[0];

        m_CurrentString = currentLSystem.axiom;
        Generate();
        DrawLine();

        m_Generations = (int)m_GenSlider.value;
        m_Randomness = (int)m_RandSlider.value;
        m_Length = m_LenSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        m_Generations = (int)m_GenSlider.value;
        m_Randomness = (int)m_RandSlider.value;
    }

    public void ShowPreset(int i)
    {
        if(m_Presets.Count >= i)
        {
            currentLSystem = m_Presets[i - 1];

            ResetLines();

            m_GenSlider.maxValue = currentLSystem.maxGenerations;
            if(m_Generations > currentLSystem.maxGenerations)
            {
                Debug.Log("gens too high");
                m_Generations = currentLSystem.maxGenerations;
            }

            m_CurrentString = currentLSystem.axiom;
            Generate();
            DrawLine();
        }
    }

    void LoadPresets()
    {
        // 1.
        LSystem preset = new LSystem();
        preset.axiom = "F";
        preset.rules = new Dictionary<char, List<Rule>>();
        preset.rules.Add('F', new List<Rule>());
        preset.rules['F'].Add(new Rule(1f, "F[+F]F[-F]F"));
        preset.transformStack = new Stack<TransformInfo>();
        preset.angle = 25.7f;
        preset.contextFree = true;
        preset.maxGenerations = 5;

        m_Presets.Add(preset);

        // 2.
        preset = new LSystem();
        preset.axiom = "F";
        preset.rules = new Dictionary<char, List<Rule>>();
        preset.rules.Add('F', new List<Rule>());
        preset.rules['F'].Add(new Rule(1f, "F[+F]F[-F][F]"));
        preset.transformStack = new Stack<TransformInfo>();
        preset.angle = 20f;
        preset.contextFree = true;
        preset.maxGenerations = 5;

        m_Presets.Add(preset);

        // 3.
        preset = new LSystem();
        preset.axiom = "F";
        preset.rules = new Dictionary<char, List<Rule>>();
        preset.rules.Add('F', new List<Rule>());
        preset.rules['F'].Add(new Rule(1f, "FF-[-F+F+F]+[+F-F-F]"));
        preset.transformStack = new Stack<TransformInfo>();
        preset.angle = 22.5f;
        preset.contextFree = true;
        preset.maxGenerations = 4;

        m_Presets.Add(preset);

        // 4.
        preset = new LSystem();
        preset.axiom = "X";
        preset.rules = new Dictionary<char, List<Rule>>();
        preset.rules.Add('F', new List<Rule>());
        preset.rules['F'].Add(new Rule(1f, "FF"));
        preset.rules.Add('X', new List<Rule>());
        preset.rules['X'].Add(new Rule(1f, "F[+X]F[-X]+X"));
        preset.transformStack = new Stack<TransformInfo>();
        preset.angle = 20f;
        preset.contextFree = true;
        preset.maxGenerations = 7;

        m_Presets.Add(preset);

        // 5.
        preset = new LSystem();
        preset.axiom = "X";
        preset.rules = new Dictionary<char, List<Rule>>();
        preset.rules.Add('F', new List<Rule>());
        preset.rules['F'].Add(new Rule(1f, "FF"));
        preset.rules.Add('X', new List<Rule>());
        preset.rules['X'].Add(new Rule(1f, "F[+X][-X]FX"));
        preset.transformStack = new Stack<TransformInfo>();
        preset.angle = 25.7f;
        preset.contextFree = true;
        preset.maxGenerations = 7;

        m_Presets.Add(preset);

        // 6.
        preset = new LSystem();
        preset.axiom = "X";
        preset.rules = new Dictionary<char, List<Rule>>();
        preset.rules.Add('F', new List<Rule>());
        preset.rules['F'].Add(new Rule(1f, "FF"));
        preset.rules.Add('X', new List<Rule>());
        preset.rules['X'].Add(new Rule(1f, "F-[[X]+X]+F[+FX]-X"));
        preset.transformStack = new Stack<TransformInfo>();
        preset.angle = 22.5f;
        preset.contextFree = true;
        preset.maxGenerations = 5;

        m_Presets.Add(preset);

        // 7. Stochastic
        preset = new LSystem();
        preset.axiom = "YYY";
        preset.rules = new Dictionary<char, List<Rule>>();
        preset.rules.Add('X', new List<Rule>());
        preset.rules['X'].Add(new Rule(1f, "X[-FFF][+FFF]FX"));
        preset.rules.Add('Y', new List<Rule>());
        preset.rules['Y'].Add(new Rule(1f / 3f, "YXF[-Y]"));
        preset.rules['Y'].Add(new Rule(1f / 3f, "YXF[+Y][-Y]"));
        preset.rules['Y'].Add(new Rule(1f / 3f, "YXF[+Y]"));
        preset.transformStack = new Stack<TransformInfo>();
        preset.angle = 25f;
        preset.contextFree = true;
        preset.maxGenerations = 5;

        m_Presets.Add(preset);

        // 8. Context sensitive attempt.
        preset = new LSystem();
        preset.axiom = "FAFAFA";
        preset.contextRules = new Dictionary<char, List<ContextRule>>();
        preset.contextRules.Add('A', new List<ContextRule>());
        preset.contextRules['A'].Add(new ContextRule("A", "A", "B"));
        preset.contextRules['A'].Add(new ContextRule("A", "B", "A"));
        preset.contextRules['A'].Add(new ContextRule("B", "A", "A"));
        preset.contextRules['A'].Add(new ContextRule("B", "B", "A"));
        preset.contextRules.Add('B', new List<ContextRule>());
        preset.contextRules['B'].Add(new ContextRule("A", "A", "AFA"));
        preset.contextRules['B'].Add(new ContextRule("A", "B", "B"));
        preset.contextRules['B'].Add(new ContextRule("B", "A", "A[-FAFA]"));
        preset.contextRules['B'].Add(new ContextRule("B", "B", "B"));
        preset.contextRules.Add('-', new List<ContextRule>());
        preset.contextRules['-'].Add(new ContextRule("*", "*", "+"));
        preset.contextRules.Add('+', new List<ContextRule>());
        preset.contextRules['+'].Add(new ContextRule("*", "*", "-"));
        preset.angle = 22.5f;
        preset.contextFree = false;
        preset.ignoreList.Add('+');
        preset.ignoreList.Add('F');
        preset.ignoreList.Add('-');
        preset.ignoreList.Add(']');
        preset.ignoreList.Add('[');
        preset.maxGenerations = 26;
        preset.lenDiv = 1.1f;

        m_Presets.Add(preset);

        //LSystem preset = new LSystem();
        //preset.axiom = "X";
        //preset.rules = new Dictionary<char, List<Rule>>();
        //preset.rules.Add('X', new List<Rule>());
        //preset.rules['X'].Add(new Rule(1f, "F+[[X]-X]-F[-FX]+X"));
        //preset.rules.Add('F', new List<Rule>());
        //preset.rules['F'].Add(new Rule(1f, "FF"));
        //preset.transformStack = new Stack<TransformInfo>();
        //preset.angle = 25f;
        //preset.contextFree = true;

        //preset = new LSystem();
        //preset.axiom = "F";
        //preset.rules = new Dictionary<char, List<Rule>>();
        //preset.rules.Add('F', new List<Rule>());
        //preset.rules['F'].Add(new Rule(1f, "FG[-FG][+FG]"));
        //preset.transformStack = new Stack<TransformInfo>();
        //preset.angle = 25f;
        //preset.contextFree = true;
        //preset.maxGenerations = 5;

        //preset = new LSystem();
        //preset.axiom = "X";
        //preset.rules = new Dictionary<char, List<Rule>>();
        //preset.rules.Add('F', new List<Rule>());
        //preset.rules['F'].Add(new Rule(1f, "FF"));
        //preset.rules.Add('X', new List<Rule>());
        //preset.rules['X'].Add(new Rule(1f, "-F[+F][---X]+F-F[++++X]-X"));
        //preset.transformStack = new Stack<TransformInfo>();
        //preset.angle = 5f;
        //preset.contextFree = true;
        //preset.maxGenerations = 5;

        //preset = new LSystem();
        //preset.axiom = "X";
        //preset.rules = new Dictionary<char, List<Rule>>();
        //preset.rules.Add('F', new List<Rule>());
        //preset.rules['F'].Add(new Rule(1f, "FF"));
        //preset.rules.Add('X', new List<Rule>());
        //preset.rules['X'].Add(new Rule(1f, "F+[-F-XF-X][+XF][--XF[+X]][++F-X]"));
        //preset.transformStack = new Stack<TransformInfo>();
        //preset.angle = 25f;
        //preset.contextFree = true;
        //preset.maxGenerations = 4;

        //preset = new LSystem();
        //preset.axiom = "X";
        //preset.rules = new Dictionary<char, List<Rule>>();
        //preset.rules.Add('F', new List<Rule>());
        //preset.rules['F'].Add(new Rule(1f, "FX[FX[+XF]]"));
        //preset.rules.Add('X', new List<Rule>());
        //preset.rules['X'].Add(new Rule(1f, "FF[+XZ++X-F[+ZX]][-X++F-X]"));
        //preset.rules.Add('Z', new List<Rule>());
        //preset.rules['Z'].Add(new Rule(1f, "[+F-X-F][++ZX]"));
        //preset.transformStack = new Stack<TransformInfo>();
        //preset.angle = 25f;
        //preset.contextFree = true;
        //preset.maxGenerations = 4;
    }

    void ResetLines()
    {
        m_Length = m_LenSlider.value;

        while(m_Lines.Count > 0)
        {
            GameObject line = m_Lines[0].gameObject;
            m_Lines.RemoveAt(0);
            Destroy(line);
        }

        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    void Generate()
    {
        Debug.Log("=========== GENERATING ===========");
        for (int j = 0; j < m_Generations; j++)
        {
            m_Length = m_Length / currentLSystem.lenDiv;

            string newString = "";
            char[] stringChars = m_CurrentString.ToCharArray();
            for (int i = 0; i < stringChars.Length; i++)
            {
                char currentChar = stringChars[i];

                if (currentLSystem.contextFree == false)
                {
                    string rule = currentChar.ToString();

                    if (currentLSystem.contextRules.ContainsKey(currentChar))
                    {
                        foreach (ContextRule cr in currentLSystem.contextRules[currentChar])
                        {
                            bool ruleValid = false;

                            char[] before = cr.before.ToCharArray();
                            char[] after = cr.after.ToCharArray();

                            if (before.Length != 0)
                            {
                                if (cr.before == "*")
                                {
                                    ruleValid = true;
                                }
                                else
                                {
                                    int skippedChars = 0;

                                    // Check before.
                                    for (int h = 0; h < before.Length; h++)
                                    {
                                        // Find next valid character.
                                        while (i - before.Length + h - skippedChars >= 0 && currentLSystem.ignoreList.Contains(stringChars[i - before.Length + h - skippedChars]))
                                        {
                                            skippedChars++;
                                        }

                                        // Check validity.
                                        if (i - before.Length + h - skippedChars < 0 ||
                                            before[h] != stringChars[i - before.Length + h - skippedChars])
                                        {
                                            break;
                                        }

                                        ruleValid = true;
                                    }
                                }
                            }

                            if (after.Length != 0 && ruleValid)
                            {
                                ruleValid = false;
                                int skippedChars = 0;

                                if (cr.after == "*")
                                {
                                    ruleValid = true;
                                }
                                else
                                {
                                    // Check after.
                                    for (int h = 0; h < after.Length; h++)
                                    {
                                        // Find next valid character.
                                        while (i + 1 + h + skippedChars < stringChars.Length && currentLSystem.ignoreList.Contains(stringChars[i + 1 + h + skippedChars]))
                                        {
                                            skippedChars++;
                                        }

                                        // Check validity.
                                        if (i + 1 + h + skippedChars >= stringChars.Length ||
                                            after[h] != stringChars[i + 1 + h + skippedChars])
                                        {
                                            break;
                                        }

                                        ruleValid = true;
                                    }
                                }
                            }

                            if (ruleValid)
                            {
                                rule = cr.rule;
                                break;
                            }
                        }

                        newString += rule;
                    }
                    else
                    {
                        newString += rule;
                    }
                }
                else if (currentLSystem.rules.ContainsKey(currentChar))
                {
                    // Store character as rule incase a rule can't be found.
                    string rule = currentChar.ToString();

                    // Get random number.
                    float rand = Random.value;
                    float sum = 0;

                    // Find rule
                    foreach (Rule r in currentLSystem.rules[currentChar])
                    {
                        sum += r.probability;
                        if (rand < sum)
                        {
                            rule = r.rule;
                            break;
                        }
                    }

                    newString += rule;
                }
                else
                {
                    newString += currentChar.ToString();
                }
            }

            m_CurrentString = newString;
            Debug.Log(m_CurrentString);
        }
    }

    void DrawLine()
    {
        //Debug.Log("===========================");
        //Debug.Log(transform.position);
        char[] stringChars = m_CurrentString.ToCharArray();

        for (int i = 0; i < stringChars.Length; i++)
        {
            char currentChar = stringChars[i];
            if (currentChar == 'F' || currentChar == 'G')
            {
                Vector3 initialPos = transform.position;
                transform.Translate(Vector3.forward * m_Length);
                //Debug.DrawLine(initialPos, transform.position, Color.white, 10000f, false);

                // Draw Forward
                LineRenderer line = Instantiate(m_LinePrefab, transform).GetComponent<LineRenderer>();
                line.SetPosition(0, initialPos);
                line.SetPosition(1, transform.position);
                m_Lines.Add(line);
            }
            else if (currentChar == '+')
            {
                transform.Rotate(Vector3.up * (Random.Range(currentLSystem.angle - m_Randomness,currentLSystem.angle + m_Randomness)));
            }
            else if (currentChar == '-')
            {
                transform.Rotate(Vector3.up * -(Random.Range(currentLSystem.angle - m_Randomness, currentLSystem.angle + m_Randomness)));
            }
            else if (currentChar == '[')
            {
                TransformInfo ti = new TransformInfo();
                ti.position = transform.position;
                ti.rotation = transform.rotation;
                currentLSystem.transformStack.Push(ti);
            }
            else if (currentChar == ']')
            {
                TransformInfo ti = currentLSystem.transformStack.Pop();
                transform.position = ti.position;
                transform.rotation = ti.rotation;
            }

        }
    }

    public void GenerateButton()
    {
        ResetLines();

        m_CurrentString = currentLSystem.axiom;
        Generate();
        DrawLine();
    }

    public void CustomGenerate(LSystem ls, int gens, int rand, float len)
    {
        currentLSystem = ls;
        m_Generations = gens;
        m_Randomness = rand;
        m_Length = len;

        ResetLines();
        m_CurrentString = ls.axiom;
        Generate();
        DrawLine();
    }
}
