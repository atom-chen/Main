using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.Table;
using Games.GlobeDefine;
using UnityEngine;

public class HouseProdPreview : UIControllerBase<HouseProdPreview>
{
    public static void Open(int intimacyLevel)
    {
        UIManager.ShowUI(UIInfo.HouseProdPreviewRoot, (success, param) =>
        {
            if (!success || HouseProdPreview.Instance() == null)
                return;
            HouseProdPreview.Instance().Setup(intimacyLevel);
        },null,UIStack.StackType.PushAndPop);
    }

    [SerializeField] private HouseProdPreviewItem[] mItems;

    void Awake()
    {
        SetInstance(this);
    }

    public void Setup(int intLevel)
    {
        var table = TableManager.GetHouseProd();
        if (table == null)
            return;

        int[] flags = new int[(int)GlobeVar.HouseProdType.Count];
        for (int i = 0; i < flags.Length; ++i)
            flags[i] = 0;
        
        int itemIndex = 0;

        for (int i = 0; i < mItems.Length; ++i)
        {
            mItems[i].gameObject.SetActive(false);
        }

        List<Tab_HouseProd> list = (from vlist in table.Values where vlist.Count > 0 where vlist[0].IntimacyLevel == intLevel select vlist[0]).ToList();
        foreach (var tab in list)
        {
            if (tab.Type < 0 || tab.Type >= flags.Length)
                continue;

            if (itemIndex < mItems.Length && flags[(int)tab.Type] == 0)
            {
                mItems[itemIndex].gameObject.SetActive(true);
                mItems[itemIndex].Setup(tab);

                ++flags[(int)tab.Type];
                ++itemIndex;
            }
        }
    }

    public void OnClickClose()
    {
        UIManager.CloseUI(UIInfo.HouseProdPreviewRoot);
    }
}
