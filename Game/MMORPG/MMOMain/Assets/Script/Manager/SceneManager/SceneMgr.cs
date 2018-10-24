using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

public abstract class  SceneBase
{
    public abstract SCENE_CODE SceneCode { get; }

    public abstract void OnLoadScene();

    public abstract void OnCloseScene();
}
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

    public static void LoadScene(SCENE_CODE scene)
    {
        //加载场景
        SceneLoading.Show(scene,OnLoadSceneFinish);
    }
    private static void OnLoadSceneFinish(bool success,SCENE_CODE scene,object para)
    {
        if(success)
        {
            if(m_CurScene!=null)
            {
                m_CurScene.OnCloseScene();
            }
            SceneBase nextScene = GetScene(scene);
            if (nextScene!=null)
            {
                m_CurScene = nextScene;
                nextScene.OnLoadScene();
            }
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

