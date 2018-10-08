using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games.GlobeDefine;
using Games;

public class HouseProdPreviewItem : MonoBehaviour
{
    [SerializeField]
    UILabel mDesc;
    [SerializeField]
    DropPreviewItem[] mItems;

    public void Setup(Tab_HouseProd tab)
    {
        if (tab == null)
            return;

        string type = Utils.GetHouseProdTypeStr((GlobeVar.HouseProdType)tab.Type);
        mDesc.text = StrDictionary.GetDicByID(7945, type);

        var tabDrop = TableManager.GetDropPreviewByID(tab.DropPreview, 0);
        if (tabDrop == null)
            return;
        
        for (int i = 0; i < mItems.Length; ++i)
        {
            if (i < tabDrop.getDropPreviewCount())
            {
                mItems[i].gameObject.SetActive(false);
                mItems[i].Refresh(tabDrop.GetDropPreviewbyIndex(i), tabDrop.GetDropPreviewNumbyIndex(i));
            }
            else
            {
                mItems[i].gameObject.SetActive(false);
            }
        }
    }

}
