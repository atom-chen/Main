/********************************************************************************
 *	文件名：	StoryNpcCopy.cs
 *	全路径：	\Assets\Editor\StoryNpcCopy.cs
 *	创建人：	王必宇
 *	创建时间：2018-09-17
 *
 *	功能说明：游戏剧情菜单工具
 *	修改记录：
*********************************************************************************/

using UnityEngine;
using UnityEditor;
using Games.Table;
using System.Collections.Generic;
using Games.GlobeDefine;
using System.IO;
using System.Text;
using System;

public struct FilterPair
{
    public int min;
    public int max;
}


public class StoryNpcCopy : Editor
{
    private static string sceneNpcPath = GetPublicTablePath("SceneNpc.txt");
    private static string storyNpcPath = GetPublicTablePath("StoryNpc.txt");
    private static string[] sceneNpcContentArr = null;
    private static Dictionary<int, string> sceneNpcContentDic = null;
    [MenuItem("Game/剧情/拷贝SceneNpc数据到StoryNpc表")]
    public static void CopyTableNpc()
    {
        FileStream fs = null;
        StreamWriter sw = null;
        FileStream srcFs = null;
        StreamWriter srcSw = null;
        try
        {
            sceneNpcContentArr = File.ReadAllLines(sceneNpcPath, Encoding.Default);         //所有原始数据（包括表头、注释等）
            sceneNpcContentDic = GetSceneNpcDataContent(sceneNpcPath);          //所有有效数据（为了快速索引、删除）
            List<string> abortData = GetAbortData( GetFilter("//F.txt"));
            if( abortData == null || abortData.Count <=0)
            {
                return;
            }
            //打开文件流
            fs = new FileStream(storyNpcPath, FileMode.Append);
            sw = new StreamWriter(fs, Encoding.Default);
            sw.WriteLine();
            //取出所有策划关心的数据
            for (int i = 0; i < abortData.Count;i++ )
            {
                string sceneNpcStr = abortData[i];
                //构造模型
                Tab_SceneNpc sceneNpc = DecodeSceneNpc(sceneNpcStr);
                if (sceneNpc == null)
                {
                    //注释，直接写入
                    sw.WriteLine(sceneNpcStr);
                }
                else
                {
                    //构造string 进行追加写
                    string str = GetStoryNpcStr(sceneNpc);
                    if (string.IsNullOrEmpty(str)) continue;
                    sw.WriteLine(str);
                    //在字典中找到该元组，删除
                    sceneNpcContentDic.Remove(sceneNpc.Id);
                }
            }
            //重新往SceneNpc.txt写入数据
            srcFs = new FileStream(sceneNpcPath, FileMode.Create);
            srcSw = new StreamWriter(srcFs, Encoding.Default);

            //回写数据：判断ID是否在sceneNpcContent中，如果还在，则重新写回到SceneNpc
            foreach (string str in sceneNpcContentArr)
            {
                int id = DecodeNpcId(str);
                if(id == -1)
                {
                    //是表头、注释等直接写入
                    srcSw.WriteLine(str);
                }
                else
                {
                    //检测ID在不在sceneNpcContent里面，如果在才写入
                    if(sceneNpcContentDic.ContainsKey(id))
                    {
                        srcSw.WriteLine(str);
                    }
                }
            }
        }
        catch(System.IO.IOException ioex)
        {
            Debug.LogError("请先关闭对应的表格文件，再点击写入"+ioex.Message);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Source + "    " + ex.Message);
        }
        finally
        {
            if(srcSw!=null)
            {
                srcSw.Close();
            }
            if (srcFs != null) srcFs.Close();
            if (sw != null) sw.Close();
            if (fs != null) fs.Close();

        }
    }

    //一个Id是否在该范围内
    private static bool IsIn(FilterPair pair, int data)
    {
        return data <= pair.max && data >= pair.min;
    }
    
    /// <summary>
    /// 将SceneNpc转化为StoryNpc的格式
    /// </summary>
    /// <param name="npc">源数据</param>
    /// <returns>StoryNpc的字符串格式</returns>
    private static string GetStoryNpcStr(Tab_SceneNpc npc)
    {
        if(npc == null)
        {
            return "";
        }
        //开始解析
        string content = null;
        if(sceneNpcContentDic.TryGetValue(npc.Id,out content))
        {
            string desc = DecodeSceneNpcDesc(content);
            int charModelId = npc.GetDataIDbyIndex(0);
            if (charModelId == -9000 || charModelId == -9001 || charModelId == -9002 || charModelId == -9999)
            {
                //不变
            }
            //如果不在上述特殊情况
            else
            {
                Tab_RoleBaseAttr tRoleBaseAttr = TableManager.GetRoleBaseAttrByID(npc.GetDataIDbyIndex(0), 0);
                if (tRoleBaseAttr != null)
                {
                    charModelId = tRoleBaseAttr.CharModelID;
                }
                else
                {
                    charModelId = GlobeVar.INVALID_ID;
                }
            }

            return string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}	{9}	{10}",npc.Id.ToString(),desc,charModelId.ToString(),
                npc.EffectID.ToString(),npc.DyeColorID.ToString(),npc.PosX.ToString(),
                npc.PosY.ToString(),npc.PosZ.ToString(),npc.FaceDirection.ToString(),npc.Animtion.ToString(),npc.CanBeSelect.ToString());
        }
        return null;
    }

    //获取表格文件路径
    private static string GetPublicTablePath(string fileName)
    {
        string BaseDirectoryPath = Application.dataPath;
        // 向上回退2级，得到需要的目录
        BaseDirectoryPath = BaseDirectoryPath.Substring(0, BaseDirectoryPath.LastIndexOf("/"));
        BaseDirectoryPath = BaseDirectoryPath.Substring(0, BaseDirectoryPath.LastIndexOf("/"));
        BaseDirectoryPath += "/Public/PublicTables/" + fileName;
        return BaseDirectoryPath;
    }

    //private static List<string> GetAbortData1(List<FilterPair> filter)
    //{
    //    if(filter == null || filter.Count <=0)
    //    {
    //        return null;
    //    }
    //    //从Scenenpc中找到筛选范围
    //    List<string> abortData = new List<string>();
    //    int max = GlobeVar.INVALID_ID;              //如果找点成功 记一个上界
    //    foreach (string content in sceneNpcContentArr)
    //    {
    //        Tab_SceneNpc sceneNpc = DecodeSceneNpc(content);
    //        //失败，说明这行是注释，直接加入集合
    //        if(sceneNpc == null)
    //        {
    //            abortData.Add(content);
    //            continue;
    //        }
    //        if (max != GlobeVar.INVALID_ID)
    //        {
    //            //如果仍在上界内，直接存入List
    //            if (sceneNpc.Id <= max)
    //            {
    //                abortData.Add(content);
    //                continue;
    //            }
    //        }
    //        //当前范围已经偏离上界，重新做记录
    //        max = GlobeVar.INVALID_ID;
    //        foreach (FilterPair pair in filter)
    //        {
    //            //如果找点成功，则做记录
    //            if (IsIn(pair, sceneNpc.Id))
    //            {
    //                max = pair.max;
    //                abortData.Add(content);
    //            }
    //        }
    //    }
    //    return abortData;
    //}

    /// <summary>
    /// 返回策划关心的数据List，包括范围内所有已经注释掉的数据
    /// </summary>
    /// <param name="filter">需求数据的筛选范围</param>
    /// <returns>需求数据的string</returns>
    private static List<string> GetAbortData(List<FilterPair> filter)
    {
        if (filter == null || filter.Count <= 0)
        {
            return null;
        }
        //从Scenenpc中找到筛选范围
        List<string> abortData = new List<string>();
        //遍历范围，范围内数据直接做记录
        foreach (FilterPair f in filter)
        {
            for(int i = f.min;i<f.max;i++)
            {
                if (abortData.Contains(sceneNpcContentArr[i]))
                    continue;
                abortData.Add(sceneNpcContentArr[i]);
            }
        }
        return abortData;
    }
    private static Tab_SceneNpc DecodeSceneNpc(string content)
    {
        //解析ID 然后去TableManager读Tab数据
        int id = DecodeNpcId(content);
        if(id!=-1)
        {
            return TableManager.GetSceneNpcByID(id, 0);
        }
        return null;
    }

    /// <summary>
    /// 解析需求拷贝的表格范围 文件
    /// </summary>
    /// <param name="fileName">文件名称，从Assets路径开始找</param>
    /// <returns>返回需求范围</returns>
    private static List<FilterPair> GetFilter(string fileName)
    {
        //读出筛选范围
        string[] src = File.ReadAllLines(Application.dataPath + fileName, Encoding.Default);
        List<FilterPair> filter = new List<FilterPair>();
        foreach (string temp in src)
        {
            string[] sp = temp.Split(',', ' ');
            if (sp.Length >= 2)
            {
                FilterPair fileter = new FilterPair();
                if (int.TryParse(sp[0], out fileter.min))
                {
                    if (int.TryParse(sp[1], out fileter.max))
                    {
                        if (fileter.min > fileter.max)
                        {
                            continue;
                        }
                        filter.Add(fileter);
                    }
                }
            }
        }
        return filter;
    }
    /// <summary>
    /// 解析表格，返回一个可用数据的字典
    /// </summary>
    /// <param name="path">表格路径</param>
    /// <returns>Key = Id，value为其string表示</returns>
    private static Dictionary<int,string> GetSceneNpcDataContent(string path)
    {
        //解析每一行数据，若是正确的数据则按Key存入
        Dictionary<int, string> ret = new Dictionary<int, string>();
        foreach (string content in sceneNpcContentArr)
        {
            int id = DecodeNpcId(content);
            if(id != -1)
            {
                ret.Add(id, content);
            }
        }
        return ret;
    }
    

    /// <summary>
    /// 判断一行表格是否是数据
    /// </summary>
    /// <param name="content">源string</param>
    /// <returns> 是一条数据返回ID 非数据返回-1</returns>
    private static int DecodeNpcId(string content)
    {
        //尝试拆分
        string[] sp = content.Split('	');
        //判断依据：切割长度>2，且index 0的位置可以转换为int
        if (sp.Length >2)
        {
            int id = -1;
            if(int.TryParse(sp[0],out id))
            {
                return id;
            }
        }
        return -1;
    }

    private static string DecodeSceneNpcDesc(string content)
    {
        //尝试拆分
        string[] sp = content.Split('	');
        //判断依据：切割长度>2
        if (sp.Length > 2)
        {
            return sp[1];
        }
        return "";
    }
}