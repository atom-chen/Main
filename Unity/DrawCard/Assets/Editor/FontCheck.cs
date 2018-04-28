using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

/************************************************ 
 * 文件名:CheckUnityFont.cs 
 * 描述:CheckUnityFont 检测是NGUI的UILabel中引用了Unity自带字体 
 * 创建人:陈鹏 
 * 创建日期：201601015 
 * ************************************************/  

public class FontCheck : MonoBehaviour {
    // 在菜单来创建 选项 ， 点击该选项执行搜索代码      
    [MenuItem("Tools/Check Unity Font")]  
    static void Check()  
    {  
        string[] tmpFilePathArray = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);  
  
        EditorUtility.DisplayProgressBar("CheckUnityFont", "CheckUnityFont", 0f);  
  
        for (int i=0;i<tmpFilePathArray.Length;i++)  
        {  
            EditorUtility.DisplayProgressBar("CheckUnityFont", "CheckUnityFont",( i*1.0f)/tmpFilePathArray.Length);  
  
            string tmpFilePath = tmpFilePathArray[i];  
  
            if(tmpFilePath.EndsWith(".prefab"))  
            {  
                StreamReader tmpStreamReader = new StreamReader(tmpFilePath);  
                string tmpContent = tmpStreamReader.ReadToEnd();  
                if(tmpContent.Contains("mFont: {fileID: 0}"))  
                {  
                    Debug.Log(tmpFilePath);  
                }  
            }  
  
            if (tmpFilePath.EndsWith(".prefab"))  
            {  
                StreamReader tmpStreamReader = new StreamReader(tmpFilePath);  
                string tmpContent = tmpStreamReader.ReadToEnd();  
                if (tmpContent.Contains("guid: 0000000000000000d000000000000000"))  
                {  
                    Debug.Log(tmpFilePath);  
                }  
            }  
        }  
  
        EditorUtility.ClearProgressBar();  
    }  
} 

