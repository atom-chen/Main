using System.Collections.Generic;
using UnityEngine;

//简版引用管理

public class AssetRefManager
{
    private Dictionary<int, AssetRef> assetRefs = new Dictionary<int, AssetRef>();

    public void Clear()
    {
        assetRefs.Clear();
    }

    public AssetRef RefAsset(Object asset)
    {
        if (asset == null)
        {
            return null;
        }
        int instanceId = asset.GetInstanceID();
        AssetRef assetRef;
        if (!assetRefs.TryGetValue(instanceId,out assetRef))
        {
            assetRef = new AssetRef()
            {
                instanceId = instanceId,
                Mgr = this,
            };
            assetRefs.Add(instanceId, assetRef);
        }
        assetRef.Ref();
        return assetRef;
    }

    public bool HasRef(Object asset)
    {
        if (asset == null)
        {
            return false;
        }
        int instanceId = asset.GetInstanceID();
        AssetRef assetRef;
        if (!assetRefs.TryGetValue(instanceId, out assetRef))
        {
            return false;
        }
        return assetRef.HasRef();
    }

    public void RemoveRef(Object asset)
    {
        int instanceId = asset.GetInstanceID();
        assetRefs.Remove(instanceId);
    }

    public void RemoveRef(int instanceId)
    {
        assetRefs.Remove(instanceId);
    }
}