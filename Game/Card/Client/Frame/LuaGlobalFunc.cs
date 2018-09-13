using System.IO;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class LuaGlobalFunc
{
//    public static byte[] CSharp_ReadFromStreamingAsset(string filepath)
//    {
//#if UNITY_ANDROID && !UNITY_EDITOR
//        UnityEngine.WWW www = new UnityEngine.WWW(filepath);
//        while (true)
//        {
//            if (www.isDone || !string.IsNullOrEmpty(www.error))
//            {
//                System.Threading.Thread.Sleep(50); //比较hacker的做法
//                if (!string.IsNullOrEmpty(www.error))
//                {
//                    return null;
//                }
//                else
//                {
//                    return www.bytes;
//                }
//                break;
//            }
//        }
//#else
//        if (File.Exists(filepath))
//        {
//            // string text = File.ReadAllText(filepath);
//            var bytes = File.ReadAllBytes(filepath);
//            return bytes;
//        }
//        else
//        {
//            return null;
//        }
//#endif
//    }

    public static byte[] CSharp_ReadFromStreamingAsset(string filepath)
    {
        //TOOD,先看AB


        //Resouce里直接load
        //string filename = filepath.Replace('.', '/') + ".lua";

        //Debug.Log(filepath);
        // Load with Unity3D resources
        UnityEngine.TextAsset file = (UnityEngine.TextAsset)UnityEngine.Resources.Load(filepath);

        if (file == null)
        {
            return null;
        }

        return file.bytes;
    }
}

