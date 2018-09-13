/********************************************************************
	created:	2015/11/04
	created:	4:11:2015   18:52
	author:		WD
	
	purpose:	Bundle任务类 AssetLoader根据BundleTask加载对应资源并回调
*********************************************************************/

//wtm，修改
//GC优化，默认一个加载unit，list初始不创建，doinglist、finishlist、params同理
//接口封装，数据结构不暴露出去

using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class BundleTaskUnit
{
    public BundleTask.BundleType bundleType;
    public string bundeName;
    public string tag;
    public Object assetLoaded;
    public bool isGroup;
    public string groupSubName;
    public bool isLoaded;

    public string GetABAssetName()
    {
        string loadName = bundeName;
        if (isGroup)
        {
            loadName = groupSubName;
        }
        else if (bundleType == BundleTask.BundleType.SOUND)
        {
            loadName = loadName.Substring(loadName.LastIndexOf('/') + 1);
        }
        return loadName;
    }
}

public class BundleTask
{
    public enum BundleType
    {
        MODEL,
        EFFECT,
        SOUND,
        UI,
        UIRES,
        SHADER,
        COMMON,
        
    }

    public BundleTask(DelBundleLoadFinish delFinish)
    {
        m_delFinish = delFinish;
    }

    public delegate void DelBundleLoadFinish(BundleTask task);

    //public DelBundleLoadFinish FinishDel { get { return m_delFinish; } }

    //public List<BundleTaskUnit> DoingList { get { return m_dongList; } }
    //public List<BundleTaskUnit> FinishList { get { return m_finishList; } }
    //public List<object> ParamList { get { return m_paramList; } }

    private List<BundleTaskUnit> m_dongList;
    private List<BundleTaskUnit> m_finishList = new List<BundleTaskUnit>();
    private List<object> m_paramList;
    private DelBundleLoadFinish m_delFinish;

    private BundleTaskUnit m_unit;
    private BundleTaskUnit m_finUnit;

    private int m_PopDoingIndex = -1;

    public void Add(BundleType bundleType, string name, string tag = "", bool isGroup = false, string groupSubName = "")
    {
        BundleTaskUnit task = new BundleTaskUnit();
        task.bundleType = bundleType;
        task.bundeName = name;
        task.tag = tag;
        task.isGroup = isGroup;
        task.groupSubName = groupSubName;
        task.assetLoaded = null;
        task.isLoaded = false;

        if (m_unit == null)
        {
            m_unit = task;
        }
        else
        {
            if (m_dongList == null)
            {
                m_dongList = new List<BundleTaskUnit>();
            }
            m_dongList.Add(task);
        }
    }

    public BundleTaskUnit PeekDoing()
    {
        if (m_PopDoingIndex == -1)
        {
            return m_unit;
        }
        else
        {
            if (m_dongList == null)
            {
                return null;
            }
            //int index = m_dongList.Count - m_PopDoingIndex - 1;
            int index = m_PopDoingIndex;
            if (index < 0 || index >= m_dongList.Count)
            {
                return null;
            }
            return m_dongList[index];
        }
    }

    public BundleTaskUnit PopDoing()
    {
        BundleTaskUnit top = PeekDoing();
        if (top == null)
        {
            return null;
        }
        m_PopDoingIndex++;
        return top;
    }

    //重置Doing队列，移除已经加载的资源
    public void RewindDoing()
    {
        if (m_unit == null)
        {
            m_PopDoingIndex = -1;
            return;
        }
        //删除所有的已经加载的
        if (m_dongList != null)
        {
            m_dongList.RemoveAll(LoadedDoing);
        }
        if (m_unit.isLoaded && m_dongList != null && m_dongList.Count > 0)
        {
            int index = m_dongList.Count - 1;
            m_unit = m_dongList[index];
            m_dongList.RemoveAt(index);
        }
        //第一个资源没有加载
        if (!m_unit.isLoaded)
        {
            m_PopDoingIndex = -1;
        }
        else
        {
            m_PopDoingIndex = 0;
        }
    }

    private bool LoadedDoing(BundleTaskUnit bundleTaskUnit)
    {
        return bundleTaskUnit.isLoaded;
    }

    public int DoingCount()
    {
        int count = 0;
        if (m_unit != null)
        {
            count++;
        }
        if (m_dongList != null)
        {
            count += m_dongList.Count;
        }
        return count;
    }

    public int FinishedCount()
    {
        int count = 0;
        if (m_finUnit != null)
        {
            count++;
        }
        if (m_finishList != null)
        {
            count += m_finishList.Count;
        }

        return count;
    }

    public void AddParam(object param)
    {
        if (m_paramList == null)
        {
            m_paramList = new List<object>();
        }
        m_paramList.Add(param);
    }

    public int ParamCount()
    {
        if (m_paramList == null)
        {
            return 0;
        }
        return m_paramList.Count;
    }

    public Object GetFinishObj()
    {
        if (m_finUnit != null)
        {
            return m_finUnit.assetLoaded;
        }
        return null;
    }

    public Object GetFinishObjByTag(string tag)
    {
        if (m_finUnit != null && m_finUnit.tag.Equals(tag))
        {
            return m_finUnit.assetLoaded;
        }
        if (m_finishList != null)
        {
            for (int i = 0; i < m_finishList.Count; i++)
            {
                if (m_finishList[i].tag == tag)
                {
                    return m_finishList[i].assetLoaded;
                }
            }
        }
        return null;
    }

    public Object GetFinishObjByName(string name)
    {
        if (m_finUnit != null && m_finUnit.bundeName.Equals(name))
        {
            return m_finUnit.assetLoaded;
        }

        if (m_finishList != null)
        {
            for (int i = 0; i < m_finishList.Count; i++)
            {
                if (m_finishList[i].bundeName == name)
                {
                    return m_finishList[i].assetLoaded;
                }
            }
        }
        return null;
    }

    public object GetParamByIndex(int index)
    {
        if (index >= m_paramList.Count || index < 0)
        {
            return null;
        }

        return m_paramList[index];
    }

    public void AssetLoaded(BundleTaskUnit unit)
    {
        unit.isLoaded = true;
        if (m_finUnit == null)
        {
            m_finUnit = unit;
        }
        else
        {
            if (m_finishList == null)
            {
                m_finishList = new List<BundleTaskUnit>();
            }
            m_finishList.Add(unit);
        }
    }

    public void Finish()
    {
        try
        {
            if (null != m_delFinish) m_delFinish(this);
        }
        catch (System.Exception e)
        {
            LogModule.ExceptionLog(e);
        }
    }
}