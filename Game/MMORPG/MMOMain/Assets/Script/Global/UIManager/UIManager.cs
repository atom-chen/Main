using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager :MonoBehaviour
{
    private static Transform m_BaseUI;
    public static Transform BaseUI
    {
        get
        {
            if (m_PopUI == null)
            {
                m_PopUI = GameObject.Find("BaseUIRoot").transform;
            }
            return m_PopUI;
        }
    }
    private static Transform m_PopUI;
    public static Transform PopUI
    {
        get
        {
            if (m_PopUI == null)
            {
                m_PopUI = GameObject.Find("PopUIRoot").transform;
            }
            return m_PopUI;
        }
    }

    private static Transform m_MessageUI;
    public static Transform MessageUI
    {
        get
        {
            if (m_MessageUI == null)
            {
                m_MessageUI = GameObject.Find("MessageUIRoot").transform;
            }
            return m_MessageUI;
        }
    }

    private static Transform m_TipsUI;
    public static Transform TipsUI
    {
        get
        {
            if (m_TipsUI == null)
            {
                m_TipsUI = GameObject.Find("TipUIRoot").transform;
            }
            return m_TipsUI;
        }
    }

    
    
    /// <summary>
    /// 在某个父物体下创建一个UI
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    private static GameObject CreateUI(string path, Transform parent = null)
    {
        GameObject asset = ResourceManager.Load("Prefabs/UI/" + path);
        if(asset==null)
        {
            return null;
        }
        Transform obj = GameObject.Instantiate(asset).transform;
        if (parent != null)
        {
            obj.parent = parent;
            obj.localScale = new Vector3(1, 1, 1);
            obj.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            List<UIRoot> uiRoot = UIRoot.list;
            if (uiRoot.Count > 0)
            {
                NGUITools.AddChild(uiRoot[0].gameObject, obj);
                obj.localScale = new Vector3(1, 1, 1);
                obj.localPosition = new Vector3(0, 0, 0);
            }
        }

        return obj.gameObject;
    }

    public delegate void OpenUIEvent(object para);
    public static GameObject ShowUI(UIInfoData info)
    {
        if(info == null)
        {
            return null;
        }
        GameObject obj=null;
        //如果已经打开，则关闭再打开
        if(m_ActiveUIDic.TryGetValue(info.UIPath,out obj))
        {
            obj.SetActive(false);
            obj.SetActive(true);
        }
        else
        {
            switch (info._UIType)
            {
                case UIType.POP:
                    obj = CreateUI(info.UIPath, PopUI);
                    obj.layer = PopUI.gameObject.layer;
                    break;
                case UIType.MESSAGE:
                    obj = CreateUI(info.UIPath, MessageUI);
                    obj.layer = MessageUI.gameObject.layer;
                    break;
                case UIType.TIPS:                    //TIP不做管理
                    obj = CreateUI(info.UIPath, TipsUI);
                    obj.layer = TipsUI.gameObject.layer;
                    return obj;
            }
            m_ActiveUIDic.Add(info.UIPath, obj);
        }
        return obj;
    }
    
    public static void CloseUI(UIInfoData info)
    {
        GameObject obj = null;
        if(m_ActiveUIDic.TryGetValue(info.UIPath,out obj))
        {
            GameObject.Destroy(obj);
            m_ActiveUIDic.Remove(info.UIPath);
        }
    }

    private static Dictionary<string, GameObject> m_ActiveUIDic = new Dictionary<string, GameObject>();

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
