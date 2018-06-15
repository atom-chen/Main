using System.Collections;
using System.Collections.Generic;
using UnityEngine.iOS;
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    private LightShadows shadowType = LightShadows.None;

    void Awake()
    {
        //平台阴影处理
#if ios
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            switch(Device.generation)
            {
                case DeviceGeneration.iPhone5S:
                    shadowType = LightShadows.Hard;
                    break;
                case DeviceGeneration.iPhone6:
                    shadowType = LightShadows.Soft;
                    break;
                case DeviceGeneration.iPhone6S:
                    shadowType = LightShadows.Soft;
                    break;
                case DeviceGeneration.iPhone7:
                    shadowType = LightShadows.Soft;
                    break;
                default:
                    shadowType = LightShadows.None;
                    break;
            } 
        }
#endif

    }

    void Start()
    {
        GameObject lightObj=GameObject.Find("DirectionLight");
        if(lightObj!=null)
        {
            Light light = lightObj.GetComponent<Light>();
            if(light!=null)
            {
                light.shadows = shadowType;
            }
        }
    }
}
