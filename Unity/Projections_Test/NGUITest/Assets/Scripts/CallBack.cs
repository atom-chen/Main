using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 问题：如何重置tween状态
 */ 
public class CallBack : MonoBehaviour {
    private UITweener m_Tween;
    void Start()
    {
        m_Tween = this.GetComponent<UITweener>();
        m_Tween.AddOnFinished(new EventDelegate(OnTweenFinish));
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            m_Tween.ResetToBeginning(false);
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            m_Tween.ResetToBeginning();
        }
    }
    private void OnTweenFinish()
    {
        print("OnTweenFinish");
    }
}
