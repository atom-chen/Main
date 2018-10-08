using System.Collections;
using System.Collections.Generic;
using Games.LogicObj;
using UnityEngine;

public class HouseNavigatorManager : MonoBehaviour
{
    [SerializeField]
    private HouseNavigator[] mNavigators;

    [SerializeField]
    private float mSpeed = 1.5f;
    public float Speed
    {
        get { return mSpeed;}
    }

    int[] mRandIndex;

    private void Awake()
    {
        mRandIndex = new int[mNavigators.Length];
        RandList();
    }

    public HouseNavigator GetNext()
    {
        if (mRandIndex.Length != mNavigators.Length)
        {
            LogModule.ErrorLog("invalid navigator count");
            return null;
        }

        for (int i=0; i < mRandIndex.Length; ++i)
        {
            var nav = mNavigators[mRandIndex[i]];
            if (!nav.HasAgent())
                return nav;
        }

        return null;
    }

    public HouseNavigator GetNavigator(Obj_Char agent)
    {
        foreach (var nav in mNavigators)
        {
            if (nav.HasAgent() && nav.HasAgent(agent))
                return nav;
        }
        return null;
    }

    public bool IsFull()
    {
        foreach (var nav in mNavigators)
        {
            if (!nav.HasAgent())
                return false;
        }
        return true;
    }

    const int RAND_COUNT = 15;
    void RandList()
    {
        int size = mRandIndex.Length;
        if (size == 0)
            return;

        for (int i = 0; i < mRandIndex.Length; ++i)
        {
            mRandIndex[i] = i;
        }

        for (int cnt = 0; cnt < RAND_COUNT; ++cnt)
        {
            int cur = Random.Range(1, size);
            int temp = mRandIndex[0];
            mRandIndex[0] = mRandIndex[cur];
            mRandIndex[cur] = temp;
        }
    }
}
