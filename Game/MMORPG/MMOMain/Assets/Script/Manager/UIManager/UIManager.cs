using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public delegate void OnLoadUIFinish(bool success,object para);
public class UIManager :MonoBehaviour
{
    private static Transform m_BaseUI;
    public static Transform BaseUI
    {
        get
        {
            if (m_BaseUI == null)
            {
                m_BaseUI = GameObject.Find("BaseUIRoot").transform;
            }
            return m_BaseUI;
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

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        BaseUI.gameObject.SetActive(false);
        PopUI.gameObject.SetActive(false);
        MessageUI.gameObject.SetActive(false);  
    }
    /// <summary>
    /// 在某个父物体下创建一个UI
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    private static GameObject CreateUI(string path, Transform parent)
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
    public static GameObject ShowUI(UIInfoData info, OnLoadUIFinish loadUIEvent = null, object para = null)
    {
        if (info == null)
        {
            return null;
        }
        GameObject obj = GetActiveUI(info);
        if(obj !=null)
        {
            obj.SetActive(true);
            return obj;
        }
        switch (info._UIType)
        {
            case UIType.BASE:
                obj = CreateUI(info.UIPath, BaseUI);
                if (obj != null)
                {
                    obj.layer = BaseUI.gameObject.layer;
                    obj.SetActive(true);
                }
                m_BaseUIDic.Add(info.UIPath, obj);
                BaseUI.gameObject.SetActive(true);
                break;
            case UIType.POP:
                obj = CreateUI(info.UIPath, PopUI);
                if (obj != null)
                {
                    obj.layer = PopUI.gameObject.layer;
                    obj.SetActive(true);
                }
                m_PopUIDic.Add(info.UIPath, obj);
                PopUI.gameObject.SetActive(true);
                break;
            case UIType.MESSAGE:
                obj = CreateUI(info.UIPath, MessageUI);
                if (obj != null)
                {
                    obj.layer = MessageUI.gameObject.layer;
                    obj.SetActive(true);
                }
                m_MessageUIDic.Add(info.UIPath, obj);
                MessageUI.gameObject.SetActive(true);
                break;
            case UIType.TIPS:                    
                obj = CreateUI(info.UIPath, TipsUI);
                if (obj != null)
                {
                    obj.layer = TipsUI.gameObject.layer;
                    obj.SetActive(true);
                    m_TipsUIDic.Add(info.UIPath, obj);
                }
                return obj;
        }
        return obj;
    }

    public static void CloseUI(UIInfoData info)
    {
        GameObject obj = GetActiveUI(info);
        if(obj!=null)
        {
            switch(info._UIType)
            {
                case UIType.BASE:
                    m_BaseUIDic.Remove(info.UIPath);
                    break;
                case UIType.POP:
                    m_PopUIDic.Remove(info.UIPath);
                    break;
                case UIType.MESSAGE:
                    m_MessageUIDic.Remove(info.UIPath);
                    break;
            }
            Destroy(obj);
        }
        if(m_BaseUIDic.Count <= 0)
        {
            BaseUI.gameObject.SetActive(false);
        }
        if (m_PopUIDic.Count <= 0)
        {
            PopUI.gameObject.SetActive(false);
        }
        if (m_MessageUIDic.Count <= 0)
        {
            MessageUI.gameObject.SetActive(false);
        }
    }

    public static GameObject GetActiveUI(UIInfoData info)
    {
        if (info == null)
        {
            return null;
        }
        GameObject obj = null;
        switch (info._UIType)
        {
            case UIType.BASE:
                m_BaseUIDic.TryGetValue(info.UIPath, out obj);
                break;
            case UIType.POP:
                m_PopUIDic.TryGetValue(info.UIPath, out obj);
                break;
            case UIType.MESSAGE:
                m_MessageUIDic.TryGetValue(info.UIPath, out obj);
                break;
            case UIType.TIPS:
                m_TipsUIDic.TryGetValue(info.UIPath, out obj);
                break;
        }
        return obj;
    }

    private static Dictionary<string, GameObject> m_BaseUIDic = new Dictionary<string, GameObject>();
    private static Dictionary<string, GameObject> m_PopUIDic = new Dictionary<string, GameObject>();
    private static Dictionary<string, GameObject> m_MessageUIDic = new Dictionary<string, GameObject>();
    private static Dictionary<string, GameObject> m_TipsUIDic = new Dictionary<string, GameObject>();
}
