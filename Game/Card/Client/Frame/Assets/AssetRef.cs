using UnityEngine;

public class AssetRef
{
    public int instanceId;
    public AssetRefManager Mgr;

    public int refCount { get; private set; }

    public void Ref()
    {
        refCount++;
    }

    public void UnRef()
    {
        refCount--;
        if (refCount < 0) refCount = 0;
        if (refCount <= 0)
        {
            Mgr.RemoveRef(instanceId);
        }
    }

    public bool HasRef()
    {
        return refCount > 0;
    }
}