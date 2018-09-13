/********************************************************************
	created:	2015/11/03
	created:	3:11:2015   16:52
	author:		WD
	
	purpose:	一些通用的资源池，避免频繁加载卸载资源。
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;

//频率池，针对不重复obj使用，每次调用GetObjFromFreqPool都会更新当前路径物体使用频率
//Remove时如果频率大于池子中物体使用频率，或者有空间，则加入池子
//加载时，如果池子中有Obj则直接返回
public class FreqAssetPool
{
    public FreqAssetPool(int maxSize, float autoIncPercent = 0)
    {
        m_maxSize = maxSize;
        m_autoIncPercent = Mathf.Clamp01(autoIncPercent);
    }

    private float m_autoIncPercent = 0;        //自动增长比例，如果大于0，则开启自动增长模式
    private int m_maxSize = 5;
    private Dictionary<string, int> m_dicObjFreq = new Dictionary<string, int>();
    private Dictionary<string, Object> m_dicFreqObjsCache = new Dictionary<string, Object>();
    
    public Dictionary<string, Object> FreqObjDic { get { return m_dicFreqObjsCache; } }
    // 清空字典，注意此防范不负责卸载数据
    public void Clean()
    {
        m_dicObjFreq.Clear();
        m_dicFreqObjsCache.Clear();
    }

    public Object GetObjFromFreqPool(string path)
    {
        if (m_dicObjFreq.ContainsKey(path))
        {
            m_dicObjFreq[path]++;
        }
        else
        {
            m_dicObjFreq.Add(path, 1);
        }

        if (m_dicFreqObjsCache.ContainsKey(path))
        {
            return m_dicFreqObjsCache[path];
        }
        return null;
    }

    public Object RemoveFreqPoolObj(Object obj, string path)
    {
        if (m_dicFreqObjsCache.ContainsKey(path))
        {
            return null;
        }

        if (!m_dicObjFreq.ContainsKey(path))
        {
            // 不应该出现这种情况，没有配合成对使用
            return obj;
        }

        // 如果缓存数量小于总量的某个比例，可以继续加
        if (m_dicFreqObjsCache.Count >= m_maxSize)
        {
            if(m_dicFreqObjsCache.Count < m_dicObjFreq.Count * m_autoIncPercent)
            {
                m_maxSize++;
            }
        }

        if (m_dicFreqObjsCache.Count < m_maxSize)
        {
            m_dicFreqObjsCache.Add(path, obj);
            return null;
        }

        int curObjFreq = m_dicObjFreq[path];
        string replaceObjPath = "";
        int minObjFreq = int.MaxValue;
        foreach (string cachingObjPath in m_dicFreqObjsCache.Keys)
        {
            if (m_dicObjFreq.ContainsKey(cachingObjPath))
            {
                if (m_dicObjFreq[cachingObjPath] < minObjFreq)
                {
                    minObjFreq = m_dicObjFreq[cachingObjPath];
                    replaceObjPath = cachingObjPath;
                }
            }
        }

        if(curObjFreq <= minObjFreq)
        {
            return obj;
        }

        if (!m_dicFreqObjsCache.ContainsKey(replaceObjPath))
        {
            return obj;
        }

        Object cachingObj = m_dicFreqObjsCache[replaceObjPath];
        m_dicFreqObjsCache.Remove(replaceObjPath);
        m_dicFreqObjsCache.Add(path, obj);

        return cachingObj;
    }

    public string StatusLog()
    {
        string strLog = "pool max size:" + m_maxSize.ToString() + "\n";
        strLog += "object freq:\n";
        foreach(var pair in m_dicObjFreq)
        {
            strLog += "  name:" + pair.Key + " value:" + pair.Value + "\n";
        }

        strLog += "object caching:\n";
        foreach (var pair in m_dicFreqObjsCache)
        {
            strLog += "  name:" + pair.Key;
        }

        strLog += "\n";
        return strLog;
    }

}

