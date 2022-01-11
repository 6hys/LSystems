using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleController : MonoBehaviour
{
    public GameObject m_Prefab;

    private List<GameObject> m_Rules;

    // Start is called before the first frame update
    void Start()
    {
        m_Rules = new List<GameObject>();
        AddRule();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRule()
    {
        GameObject rule = Instantiate(m_Prefab, this.transform);
        m_Rules.Add(rule);
    }

    public void RemoveRule()
    {
        if (m_Rules.Count > 0)
        {
            Destroy(m_Rules[m_Rules.Count - 1]);
            m_Rules.RemoveAt(m_Rules.Count - 1);
        }
    }
}
