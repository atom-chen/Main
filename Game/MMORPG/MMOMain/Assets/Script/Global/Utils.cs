using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class Utils
{
    public static T TryAddComponent<T>(GameObject obj) where T : Component
    {
        if (null == obj) return null;
        T curComponent = obj.GetComponent<T>();
        if (null == curComponent)
        {
            curComponent = obj.AddComponent<T>();
        }
        return curComponent;
    }
}

