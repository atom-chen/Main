using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABLoader
{
    private FreqAssetPool m_modelFreqPool = new FreqAssetPool(20, 0.5f);
    private FreqAssetPool m_effectFreqPool = new FreqAssetPool(20, 0.5f);
    private FreqAssetPool m_uiFreqPool = new FreqAssetPool(10);
    private FreqAssetPool m_soundFreqPool = new FreqAssetPool(10);
    private Dictionary<string, Object> m_uiGroupPool = new Dictionary<string, Object>();

    public void ReleaseBundlePool()
    {
        ReleaseBundleDic(m_modelFreqPool.FreqObjDic);
        ReleaseBundleDic(m_effectFreqPool.FreqObjDic);
        ReleaseBundleDic(m_uiFreqPool.FreqObjDic);
        ReleaseBundleDic(m_soundFreqPool.FreqObjDic);
        ReleaseBundleDic(m_uiGroupPool);

        m_modelFreqPool.Clean();
        m_effectFreqPool.Clean();
        m_uiFreqPool.Clean();
        m_soundFreqPool.Clean();
        m_uiGroupPool.Clear();
    }

    public string PoolStatusLog()
    {
        string strLog = "modelFreqPoolStatus:\n";
        strLog += m_modelFreqPool.StatusLog();
        strLog += "\neffectFreqPoolStatus:\n";
        strLog += m_effectFreqPool.StatusLog();
        strLog += "\nsoundFreqPoolStatus:\n";
        strLog += m_soundFreqPool.StatusLog();
        strLog += "\nuiFreqPoolStatus:\n";
        strLog += m_uiFreqPool.StatusLog();
        strLog += "\nGroupPoolStatus:\n";
        foreach (var pair in m_uiGroupPool)
        {
            strLog += "  " + pair.Key + " ";
        }
        return strLog;
    }

    void ReleaseBundleDic(Dictionary<string, Object> bundleDic)
    {
        foreach (Object curObj in bundleDic.Values)
        {
            AssetBundle curBundle = curObj as AssetBundle;
            if (curBundle != null)
            {
                curBundle.Unload(false);
            }
        }
    }

    public IEnumerator LoadAsync(string assetPath,BundleTaskUnit unit)
    {
        if (!TryLoadBundleFromPool(unit))
        {
            var abReq = AssetBundle.LoadFromFileAsync(assetPath);
            if (abReq.assetBundle == null)
            {
                yield return null;
            }
            AssetBundle ab = abReq.assetBundle;
            if (null == ab)
            {
                LogModule.ErrorLog("load bundle[" + unit.bundeName + "] failed.");
                yield break;
            }
            var req = ab.LoadAssetAsync(unit.GetABAssetName());
            if (req.asset == null)
            {
                yield return req;
            }
            PoolAB(ab, unit);
        }
    }

    public void Load(string assetPath, BundleTaskUnit unit)
    {
        if (!TryLoadBundleFromPool(unit))
        {
            AssetBundle ab = AssetBundle.LoadFromFile(assetPath);
            if (null == ab)
            {
                LogModule.ErrorLog("load bundle[" + unit.bundeName + "] failed.");
                return;
            }
            unit.assetLoaded = ab.LoadAsset(unit.GetABAssetName());
            PoolAB(ab, unit);
        }
    }

    private void PoolAB(AssetBundle ab, BundleTaskUnit curUnit)
    {
        Dictionary<string, Object> groupPool = null;
        FreqAssetPool freqPool = null;
        GetPoolByType(curUnit.bundleType, out groupPool, out freqPool);

        if (!curUnit.isGroup)
        {
            if (freqPool != null)
            {
                AssetBundle removeAB = freqPool.RemoveFreqPoolObj(ab, curUnit.bundeName) as AssetBundle;
                if (null != removeAB)
                {
                    removeAB.Unload(false);
                }
            }
        }
        else
        {
            if (groupPool != null)
            {
                if (!m_uiGroupPool.ContainsKey(curUnit.bundeName))
                {
                    m_uiGroupPool.Add(curUnit.bundeName, ab);
                }
            }
        }
    }

    void GetPoolByType(BundleTask.BundleType bundleType, out Dictionary<string, Object> groupPool, out FreqAssetPool freqPool)
    {
        groupPool = null;
        freqPool = null;
        switch (bundleType)
        {
            case BundleTask.BundleType.UI:
                groupPool = m_uiGroupPool;
                freqPool = m_uiFreqPool;
                break;
            case BundleTask.BundleType.MODEL:
                freqPool = m_modelFreqPool;
                break;
            case BundleTask.BundleType.EFFECT:
                freqPool = m_effectFreqPool;
                break;
            case BundleTask.BundleType.SOUND:
                freqPool = m_soundFreqPool;
                break;
        }
    }

    bool TryLoadBundleFromPool(BundleTaskUnit curUnit)
    {
        if (curUnit == null) return false;
        Dictionary<string, Object> groupPool = null;
        FreqAssetPool freqPool = null;
        GetPoolByType(curUnit.bundleType, out groupPool, out freqPool);

        AssetBundle bundle = null;
        if (curUnit.isGroup)
        {
            if (null != groupPool)
            {
                Object outData = null;
                if (groupPool.TryGetValue(curUnit.bundeName, out outData))
                {
                    bundle = outData as AssetBundle;
                }
            }
        }
        else
        {
            if (null != freqPool)
            {
                bundle = freqPool.GetObjFromFreqPool(curUnit.bundeName) as AssetBundle;
            }
        }

        if (null != bundle)
        {
            string loadName = curUnit.bundeName;
            if (curUnit.isGroup)
            {
                loadName = curUnit.groupSubName;
            }
            else
            {
                if (curUnit.bundleType == BundleTask.BundleType.SOUND)
                {
                    loadName = loadName.Substring(loadName.LastIndexOf('/') + 1);
                }
            }
            curUnit.assetLoaded = bundle.LoadAsset(loadName);
            return true;
        }

        return false;
    }
}
