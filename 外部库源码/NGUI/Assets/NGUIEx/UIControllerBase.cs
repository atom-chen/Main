/********************************************************************
	created:	2013/12/24
	created:	24:12:2013   13:59
	filename: 	UIControllerBase.cs
	author:		王迪
	
	purpose:	UI控制器基类，实现单间类，需要子类在Awake()函数中调用SetInstance(this)
*********************************************************************/

using UnityEngine;
using System.Collections;
public class UIControllerBase<T> : MonoBehaviour
{
    private static T _Instance;

    // 不确定窗口是否存在时需要判空
    public static T Instance()
    {
        return _Instance;
    }

    public static void SetInstance(T instance)
    {
        _Instance = instance;
    }

    // 如果覆盖了OnDestroy需要将_Instance置空，防止资源无法释放
    void OnDestroy()
    {
        Release();
    }

    protected void Release()
    {
        _Instance = default(T);      
    }
}