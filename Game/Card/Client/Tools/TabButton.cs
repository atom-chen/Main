using UnityEngine;
using System.Collections;
using Games;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TabButton : MonoBehaviour
{
    public GameObject objHighLight;
    public GameObject objNormal;
    public GameObject objDisabled;

    [Space]
    public UISprite colorChange;
    public Color colorHightLight = Color.white;
    public Color colorNormal = Color.gray;
    public float tweenTime = 0.3f;
    [Space]

    public GameObject targetObject;
    public bool m_LoadTarget;

    public GameObject m_TargetParent;
    public string m_TargetName;
    public string m_TargetGroup;

    public bool selfClick = false;

    [Space]
    public UILabel tabNameLb;

    public int Index { get; set; }

    private TabController m_curController;
    private bool m_bWaitHide = false;
    private bool m_bLoading = false;
    private bool m_disabled = false;

    private string m_DisabledNotice = "";
    public string DisabledNotice
    {
        get { return m_DisabledNotice; }
        set { m_DisabledNotice = value; }
    }

    private GameObject loadedGo;

    public void HighLightTab(bool bHighLight)
    {
        if (m_disabled && bHighLight)
        {
            return;
        }

        if (objHighLight != null)
        {
            objHighLight.SetActive(bHighLight);
        }

        if (objNormal != null)
        {
            objNormal.SetActive(!bHighLight && !m_disabled);
        }

        if (colorChange != null)
        {
            Color target = bHighLight ? colorHightLight : colorNormal;
            if (tweenTime > 0f)
            {
                TweenColor.Begin(colorChange.gameObject, tweenTime, target);
            }
            else
            {
                colorChange.color = target;
            }
        }

        if (null != targetObject)
        {
            targetObject.SetActive(bHighLight);
        }
        else if (m_LoadTarget)
        {
            if (m_curController == null)
            {
                FindController();
            }

            if (m_curController != null)
            {
                if (loadedGo)
                {
                    loadedGo.SetActive(bHighLight);

                    if (bHighLight)
                    {
                        m_curController.TabButtonLoadBundleOver();
                    }
                }
                else if (bHighLight)
                {
                    LoadPage();
                }
                else
                {
                    if (m_bLoading)
                    {
                        m_bWaitHide = true;
                    }   
                }
            }
        }
    }

    public void SetDisabled(bool val)
    {
        if (objDisabled != null)
        {
            objDisabled.SetActive(val);
        }
        if (val)
        {
            if (null != objHighLight) objHighLight.SetActive(false);
            if (null != objNormal) objNormal.SetActive(false);
        }
        else
        {
            if (null != objNormal) objNormal.SetActive(true);
        }
        m_disabled = val;
    }

    void OnLoadTargetObject(BundleTask task)
    {
        if (null == task)
        {
            return;
        }

        UnityEngine.Object TargetObjcetRes = task.GetFinishObj();
        if (TargetObjcetRes == null)
        {
            return;
        }

        if (null == m_TargetParent || null == m_TargetParent.transform)
        {
            return;
        }

        GameObject go = AssetManager.InstantiateObjToParent(TargetObjcetRes, m_TargetParent.transform,TargetObjcetRes.name);
        if (go == null)
        {
            return;
        }
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            go = PrefabUtility.ConnectGameObjectToPrefab(go, TargetObjcetRes as GameObject);
        }
#endif
        go.SetActive(true);

        if (m_curController == null)
        {
            FindController();
        }

        if (m_curController != null)
        {
            if (loadedGo)
            {
                GameObject.DestroyImmediate(loadedGo);
            }
            loadedGo = go;

            m_curController.TabButtonLoadBundleOver();

            if (m_bWaitHide)
            {
                if (loadedGo != null)
                {
                    loadedGo.SetActive(false);
                }
                m_bWaitHide = false;
            }
        }
        m_bLoading = false;
    }

    void OnClick()
    {
        if (selfClick)
        {
            OnTabClick();
        }
    }

    public void OnTabClick()
    {
        if (m_disabled)
        {
            if (false == string.IsNullOrEmpty(m_DisabledNotice))
            {
                Utils.CenterNotice(m_DisabledNotice);
            }
            return;
        }

        if (m_curController == null)
        {
            FindController();
        }
        if (m_curController != null)
        {
            m_curController.OnTabClicked(this);
        }
    }

    void FindController()
    {
        Transform parent = transform.parent;
        while (parent != null)
        {
            m_curController = parent.gameObject.GetComponent<TabController>();
            if (null != m_curController)
            {
                break;
            }

            parent = parent.parent;
        }
    }

    public void LoadPage()
    {
        BundleTask task = new BundleTask(OnLoadTargetObject);
        if (false == string.IsNullOrEmpty(m_TargetGroup))
        {
            task.Add(BundleTask.BundleType.UI, m_TargetGroup, "", true, m_TargetName);
        }
        else
        {
            task.Add(BundleTask.BundleType.UI, m_TargetName);
        }
        //AssetLoader.Instance.AddUITask(task);
        AssetManager.LoadBundle(task, AssetLoader.LoadQueueType.SYNC);
    }

#if UNITY_EDITOR
    [ContextMenu("加载子节点")]
    public void LoadPageForEditor()
    {
        if (string.IsNullOrEmpty(m_TargetName))
        {
            return;
        }
        LoadPage();
        if (!Application.isPlaying && AssetLoader.Instance != null)
        {
            DestroyImmediate(AssetLoader.Instance.gameObject);
        }
    }

    [ContextMenu("销毁子节点")]
    public void DestoryChild()
    {
        if (loadedGo != null)
        {
            DestroyImmediate(loadedGo);
        }
    }
#endif
}
