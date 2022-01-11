using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Text m_ValueText;
    public Slider m_Slider;
    public bool m_Decimal;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        m_ValueText.text = (Mathf.Round(m_Slider.value * 100f) / 100f).ToString();
    }
}
