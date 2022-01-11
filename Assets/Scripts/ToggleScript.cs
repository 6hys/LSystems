using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    Toggle m_Toggle;

    public GameObject m_ContextScreen;
    public GameObject m_FreeScreen;

    // Start is called before the first frame update
    void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleValueChanged(Toggle change)
    {
        m_ContextScreen.SetActive(change.isOn);
        m_FreeScreen.SetActive(!change.isOn);
    }
}
