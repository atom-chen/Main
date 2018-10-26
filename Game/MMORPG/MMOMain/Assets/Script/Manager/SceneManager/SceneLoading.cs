using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void LoadSceneEvent(bool success,SCENE_CODE scene,object para);
public class SceneLoading : MonoBehaviour
{
    public UISlider m_Slider;
    public UILabel m_Label;
    private const float m_Timer = 0.2f;
    private static int nextSceneID = Define._INVALID_ID;

    private static LoadSceneEvent _LoadSceneEvent;
    private static object _EventPara;
    public static void Show(SCENE_CODE scene,LoadSceneEvent loadSceneEvent = null,object para = null)
    {
        _LoadSceneEvent = null;
        _EventPara = null;
        nextSceneID = (int)scene;
        if(!UIManager.ShowUI(UIInfo.LoadingUI))
        {
            if(loadSceneEvent!=null)
            {
                loadSceneEvent(false, scene, para);
            }
            return;
        }
        _LoadSceneEvent = loadSceneEvent;
        _EventPara = para;
    }

    void OnEnable()
    {
        SetValue(0);
        StartCoroutine(Loading(nextSceneID));
    }
    IEnumerator Loading(int nextSceneID)
    {
        SceneManager.LoadScene(nextSceneID);
        while (m_Slider.value < 1)
        {
            yield return new WaitForSeconds(m_Timer);
            SetValue(m_Slider.value + UnityEngine.Random.Range(0.1f, 0.5f));
        }
        if (_LoadSceneEvent != null)
        {
            _LoadSceneEvent(true, (SCENE_CODE)nextSceneID, _EventPara);
        }
        UIManager.CloseUI(UIInfo.LoadingUI);
    }

    //设置加载总时间
    public void SetValue(float value)
    {
        m_Slider.value = value;
        m_Label.text = Math.Min(Math.Round(value, 3), 1) * 100 + "%";
    }
}
