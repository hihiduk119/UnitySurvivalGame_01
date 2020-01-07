using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TestCode2 : MonoBehaviour
{
    public GameObject obj;
    Toggle m_Toggle;

    private void Awake()
    {
        Toggle m_Toggle = this.GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
    }

    void ToggleValueChanged(Toggle change)
    {
        this.obj.SetActive(change.isOn);
    }
}
