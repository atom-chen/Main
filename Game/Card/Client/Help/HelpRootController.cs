using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games.GlobeDefine;

public class HelpRootController : MonoBehaviour
{
    private static HelpRootController _Ins;
    public static HelpRootController Instance
    {
        get { return _Ins; }
    }

    public GameObject m_ClassItemRes;
    public GameObject m_SubItemRes;

    public UITable m_ClassTable;
    public HelpGroupType_Content m_ContentDetail;
    public HelpGroupType_Texture m_TextureDetail;
    public GameObject m_BottomObj;
    public UILabel m_PageLabel;


    private List<HelpGroupItem> m_HelpItemList = new List<HelpGroupItem>();
    private HelpGroupItem m_CurClass = null;
    private Tab_HelpSubClass m_CurSubClass = null;
    private int m_NowPage = 0;                                 //当前页数
    private int m_CurMaxPage = GlobeVar.INVALID_ID;            //最大页数
    public static void Show()
    {
        UIManager.ShowUI(UIInfo.HelpGroupRoot);
    }
    public void Close()
    {
        UIManager.CloseUI(UIInfo.HelpGroupRoot);
    }
    void Awake()
    {
        _Ins = this;
    }
    void OnDestroy()
    {
        _Ins = null;
    }
    void Start()
    {
        m_HelpItemList.Clear();
        Dictionary<int, List<Tab_HelpSubClass>> mSubClassDic = new Dictionary<int, List<Tab_HelpSubClass>>();  //key：所属classId
        //遍历表格 制作2级分类字典
        foreach (var subClass in TableManager.GetHelpSubClass().Values)
        {
            Tab_HelpSubClass tab = subClass[0];
            List<Tab_HelpSubClass> dicData = null;
            if (mSubClassDic.TryGetValue(tab.ClassId, out dicData))
            {
                dicData.Add(tab);
            }
            else
            {
                dicData = new List<Tab_HelpSubClass>();
                dicData.Add(tab);
                mSubClassDic.Add(tab.ClassId, dicData);
            }
        }
        //遍历表格 拿到所有一级分类
        foreach (var helpClass in TableManager.GetHelpClass().Values)
        {
            Tab_HelpClass tabClass = helpClass[0];
            GameObject obj = NGUITools.AddChild(m_ClassTable.gameObject, m_ClassItemRes);
            //找到属于该class的二级分类 加载
            List<Tab_HelpSubClass> subClassList = null;
            if (mSubClassDic.TryGetValue(tabClass.Id, out subClassList))
            {
                HelpGroupItem item = obj.GetComponent<HelpGroupItem>();
                if (item != null)
                {
                    item.Init(tabClass, subClassList, m_SubItemRes);
                }
                m_HelpItemList.Add(item);
            }
        }
        m_ClassTable.Reposition();
        HideDetail();
    }

    //点击一个1级标题
    public void HandleOnClickItem(HelpGroupItem item)
    {
        //通知其他的展开或关闭
        for (int i = 0; i < m_HelpItemList.Count; i++)
        {
            HelpGroupItem temp = m_HelpItemList[i];
            if (temp == item)
            {
                temp.ShowSubClassList(true);
                m_CurClass = item;
            }
            else
            {
                temp.ShowSubClassList(false);
            }
        }
        m_ClassTable.Reposition();
        //HideDetail();
    }

    //点击一个1级标题下的2级标题
    public void HandleOnClickSubItem(HelpGroupSubItem item)
    {
        if (item.TabData == null || m_CurClass == null)
        {
            return;
        }
        //通知当前标题
        m_CurClass.OnChooseSubClassItem(item);
        m_CurSubClass = item.TabData;
        //更新右边界面表现，默认选中第一个可用页
        m_NowPage = 0;
        m_CurMaxPage = 0;
        for (int i = 0; i < m_CurSubClass.getTypeCount(); i++)
        {
            if (m_CurSubClass.GetTypebyIndex(i) == GlobeVar.INVALID_ID)
            {
                break;
            }
            m_CurMaxPage++;
        }
        m_BottomObj.SetActive(true);
        ShowDetailByIdx();
    }
    public void OnCliclLeft()
    {
        if (m_NowPage <= 0)
        {
            return;
        }
        m_NowPage--;
        ShowDetailByIdx();
    }

    public void OnClickRight()
    {
        if (m_NowPage >= m_CurMaxPage - 1)
        {
            return;
        }
        m_NowPage++;
        ShowDetailByIdx();
    }
    public void ShowDetailByIdx()
    {
        if (m_CurSubClass == null || m_NowPage < 0 || m_NowPage >= m_CurMaxPage)
        {
            return;
        }
        if (m_CurSubClass.GetTypebyIndex(m_NowPage) != GlobeVar.INVALID_ID)
        {
            switch (m_CurSubClass.GetTypebyIndex(m_NowPage))
            {
                case (int)HELP_MODEL_TYPE.CONTENT:
                    string strpara = m_CurSubClass.GetSTRParabyIndex(m_NowPage);
                    //优先读特有的content
                    if (string.IsNullOrEmpty(strpara))
                    {
                        m_ContentDetail.Show(m_CurSubClass.GetINTParabyIndex(m_NowPage));
                    }
                    else
                    {
                        m_ContentDetail.Show(strpara);
                    }
                    m_TextureDetail.gameObject.SetActive(false);
                    break;
                case (int)HELP_MODEL_TYPE.TEXTURE:
                    m_TextureDetail.Show(m_CurSubClass.GetSTRParabyIndex(m_NowPage));
                    m_ContentDetail.gameObject.SetActive(false);
                    break;
            }
            m_PageLabel.text = string.Format("({0}/{1})", m_NowPage + 1, m_CurMaxPage);
        }
    }

    public void HideDetail()
    {
        m_BottomObj.SetActive(false);
        m_ContentDetail.gameObject.SetActive(false);
        m_TextureDetail.gameObject.SetActive(false);
    }
}
