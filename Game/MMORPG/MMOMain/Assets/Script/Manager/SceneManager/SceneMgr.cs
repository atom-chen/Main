using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneMgr
{
    public static SceneBase m_CurScene = null;

    private static Dictionary<SCENE_CODE, SceneBase> m_SceneDir = new Dictionary<SCENE_CODE, SceneBase>();


    static SceneMgr()
    {
        Type[] types = Assembly.GetAssembly(typeof(SceneBase)).GetTypes();
        foreach (var type in types)
        {
            if (type.FullName.EndsWith("Scene"))
            {
                SceneBase sb = Activator.CreateInstance(type) as SceneBase;
                if (sb != null)
                {
                    m_SceneDir.Add(sb.SceneCode, sb);
                }
            }
        }
    }

    private static bool mFirst = true;
    public static void LoadScene(SCENE_CODE scene)
    {
        SceneBase nextScene = GetScene(scene);
        //加载场景，首次加载时不走enter逻辑
        if (mFirst)
        {
            m_CurScene = nextScene;
            mFirst = false;
            return;
        }
        if (m_CurScene != null)
        {
            m_CurScene.OnCloseScene();
        }
        SceneManager.LoadScene((int)scene);
        if (nextScene != null)
        {
            m_CurScene = nextScene;
            nextScene.OnLoadScene();
        }
    }

    private static SceneBase GetScene(SCENE_CODE scene)
    {
        SceneBase nextScene = null;
        if (m_SceneDir.TryGetValue(scene, out nextScene))
        {
            return nextScene;
        }
        return null;
    }
}

