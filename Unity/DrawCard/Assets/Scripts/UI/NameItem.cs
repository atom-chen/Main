using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameItem : MonoBehaviour
{
    public UILabel m_IDLabel;
    public UILabel m_NameLabel;

    void Start()
    {
        if (m_IDLabel == null)
        {
            m_IDLabel = this.transform.GetChild(0).GetComponent<UILabel>();
        }
        if (m_NameLabel == null)
        {
            m_NameLabel = this.transform.GetChild(1).GetComponent<UILabel>();
        }
    }


}
