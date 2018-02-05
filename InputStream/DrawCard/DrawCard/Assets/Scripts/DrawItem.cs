using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawItem : MonoBehaviour
{
    public UILabel m_ID;
    public UILabel m_Name;

    // Use this for initialization
    void Start()
    {
        if (m_ID == null)
        {
            m_ID = this.transform.GetChild(0).GetComponent<UILabel>();
        }
        if (m_Name == null)
        {
            m_Name = this.transform.GetChild(1).GetComponent<UILabel>();
        }
    }


}
