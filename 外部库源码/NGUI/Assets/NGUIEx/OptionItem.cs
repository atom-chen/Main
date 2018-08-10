using UnityEngine;
using System.Collections;

public class OptionItem : MonoBehaviour {

    private OptionList m_OptionList;

    public string text
    {
        get
        {
            if (GetComponentInChildren<UILabel>() != null)
            {
                return GetComponentInChildren<UILabel>().text;
            }
            else
            {
                return name;
            }
        }
    }

    void OnEnable()
    {
        if (m_OptionList == null)
        {
            m_OptionList = GetComponentInParent<OptionList>();
        }
    }

	void OnClick()
    {
        if (m_OptionList == null)
        {
            m_OptionList = GetComponentInParent<OptionList>();
        }

        if (m_OptionList != null)
        {
            m_OptionList.OnOptionItemClick(this);
        }
    }

    public void Choose()
    {
        OnClick();
    }
}
