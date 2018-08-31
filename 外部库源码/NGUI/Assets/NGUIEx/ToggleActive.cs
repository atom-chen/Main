using System;
using UnityEngine;
using System.Collections;
using Games;
using Games.GlobeDefine;

public class ToggleActive : MonoBehaviour
{
    public GameObject[] items;
    public int StartIndex = -1;
    public bool selfClick = true;

    public Action<int> onToggle;
    
    public delegate bool WillToggleDelegate(int toggle);
    public WillToggleDelegate delOnWillToggle = null;

    private int mIndex;
    public int Index
    {
        set
        {
            Refresh(value,true);
        }
        get { return mIndex; }
    }

    void Awake()
    {
        if (StartIndex == -1)
        {
            return;
        }
        Refresh(StartIndex,true);
    }

    public void Refresh(int index, bool playTween = true, bool triggerToggle = true)
    {
        mIndex = index;
        int count = items.Length;
        if (count <= 0)
        {
            return;
        }
        mIndex = index % count;
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(i == mIndex);
        }
        if (triggerToggle && onToggle != null)
        {
            onToggle(mIndex);
        }
    }

    public void Toggle()
    {
        Index++;
    }

    void OnClick()
    {
        if (selfClick)
        {
            if (delOnWillToggle != null)
            {
                if (false == delOnWillToggle(mIndex))
                {
                    return;
                }
            }

            Toggle();
        }
    }
}
