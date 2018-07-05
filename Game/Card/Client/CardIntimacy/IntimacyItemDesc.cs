using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntimacyItemDesc : MonoBehaviour {

    public UILabel m_ItemName;//道具名称
    public UILabel m_ItemDesc;//道具描述
    public UILabel m_IntimacyAdd;//增加多少亲密度Label
    public void InitDesc(string ItemName,string ItemDesc,int intimacyAdd)
    {
        if(m_ItemName!=null)
        {
            m_ItemName.text = ItemName;
        }
        if(m_ItemDesc!=null)
        {
            m_ItemDesc.text = ItemDesc;
        }
        if(m_IntimacyAdd!=null)
        {
            m_IntimacyAdd.text =StrDictionary.GetClientDictionaryString("#{6775}",intimacyAdd);
        }
    }
}
