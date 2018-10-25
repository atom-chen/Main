using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    public UIPlayAnimation mUIPA;
    void Start()
    {
        
    }

    public void OnClick()
    {
        mUIPA.PlayForward();
    }

    public void OnFinish()
    {
        Debug.Log("Finish");
    }
}
