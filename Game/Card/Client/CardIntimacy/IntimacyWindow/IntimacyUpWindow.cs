using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntimacyUpWindow : MonoBehaviour {
    public UILabel m_LvLabel;

    public void InitUpdateWindow(int nextLv)
    {
        if(m_LvLabel!=null)
        {
            m_LvLabel.text = StrDictionary.GetClientDictionaryString("#{6798}", nextLv);
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
