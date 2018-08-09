/********************************************************************************
 *	文件名：	Utils.cs
 *	全路径：	\Script\Utility\Utils.cs
 *	创建人：	李嘉
 *	创建时间：2016-12-12
 *
 *	功能说明：工具类，放置一些功能无关的通用工具函数
 *	修改记录：
*********************************************************************************/
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Games
{
    public partial class Utils
    {
        #region file utils

        //检查目录是否存在，不存在则创建对应路径
        public static void CheckTargetPath(string targetPath)
        {
            targetPath = targetPath.Replace('\\', '/');

            int dotPos = targetPath.LastIndexOf('.');
            int lastPathPos = targetPath.LastIndexOf('/');

            if (dotPos > 0 && lastPathPos < dotPos)
            {
                targetPath = targetPath.Substring(0, lastPathPos);
            }
            if (Directory.Exists(targetPath))
            {
                return;
            }

            string[] subPath = targetPath.Split('/');
            string curCheckPath = "";
            int subContentSize = subPath.Length;
            for (int i = 0; i < subContentSize; i++)
            {
                curCheckPath += subPath[i] + '/';
                if (!Directory.Exists(curCheckPath))
                {
                    Directory.CreateDirectory(curCheckPath);
                }
            }
        }

        // 删除一个路径下所有文件
        public static void DeleteFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            string[] strTemp;
            //先删除该目录下的文件
            strTemp = System.IO.Directory.GetFiles(path);
            foreach (string str in strTemp)
            {
                System.IO.File.Delete(str);
            }
            //删除子目录，递归
            strTemp = System.IO.Directory.GetDirectories(path);
            foreach (string str in strTemp)
            {
                DeleteFolder(str);
            }

            System.IO.Directory.Delete(path);
        }

        // 拷贝一个路径下所有的文件，不包含子目录
        public static void CopyPathFile(string srcPath, string distPath)
        {
            if (!Directory.Exists(srcPath))
            {
                return;
            }

            Utils.CheckTargetPath(distPath);

            string[] strLocalFile = System.IO.Directory.GetFiles(srcPath);
            foreach (string curFile in strLocalFile)
            {
                System.IO.File.Copy(curFile, distPath + "/" + Path.GetFileName(curFile), true);
            }
        }

        //根据路径和后缀获取所有的文件
        public static void GetFileListByPathAndType(List<string> retList, string curPath, string fileEnd)
        {
            if (null == retList)
            {
                return;
            }

            string[] fileList = Directory.GetFiles(curPath);
            string[] dictionaryList = Directory.GetDirectories(curPath);

            foreach (string curFile in fileList)
            {
                if (curFile.EndsWith(fileEnd))
                {
                    string curFilePath = curFile.Replace('\\', '/');
                    retList.Add(curFilePath);
                }
            }

            // 逐层目录开始遍历，获取所有的file end的文件
            foreach (string curDic in dictionaryList)
            {
                string curDicName = curDic.Replace('\\', '/');
                GetFileListByPathAndType(retList, curDicName, fileEnd);
            }
        }

        public static string GetStringMD5(string srcString)
        {
            MD5CryptoServiceProvider oMD5Hasher = new MD5CryptoServiceProvider();
            MemoryStream msm = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(srcString));
            byte[] arrbytHashValue = oMD5Hasher.ComputeHash(msm);
            string strHashData = System.BitConverter.ToString(arrbytHashValue);
            return strHashData.Replace("-", "");
        }
        // 获取MD5
        public static string GetMD5Hash(string pathName)
        {

            string strResult = "";
            string strHashData = "";
#if !UNITY_WP8
            byte[] arrbytHashValue;
#endif
            System.IO.FileStream oFileStream = null;
            MD5CryptoServiceProvider oMD5Hasher = new MD5CryptoServiceProvider();
            try
            {
                oFileStream = new System.IO.FileStream(pathName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
#if UNITY_WP8
				strHashData = oMD5Hasher.ComputeHash(oFileStream);
				oFileStream.Close();
#else
                arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream);
                oFileStream.Close();
                strHashData = System.BitConverter.ToString(arrbytHashValue);
                strHashData = strHashData.Replace("-", "");
#endif

                strResult = strHashData;
            }
            catch (System.Exception ex)
            {
                ////LogModule.ErrorLog("read md5 file error :" + pathName + " e: " + ex.ToString());
            }

            return strResult;
        }
        #endregion

        #region String
        /// <summary>
        /// 按照规则严格进行分割
        /// </summary>
        /// <param name="str">原始字符</param>
        /// <param name="nTypeList">字符串类型</param>
        /// <param name="regix">规则词，只有一个</param>
        /// <returns>返回分割的词</returns>
        public static string[] MySplit(string str, string[] nTypeList, string regix)
        {
            if (null == nTypeList)
            {
                return null;
            }

            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            String[] content = new String[nTypeList.Length];
            int nIndex = 0;
            int nstartPos = 0;
            while (nstartPos <= str.Length && nIndex <= nTypeList.Length)
            {
                int nsPos = str.IndexOf(regix, nstartPos);
                if (nsPos < 0)
                {
                    String lastdataString = str.Substring(nstartPos);
                    if (string.IsNullOrEmpty(lastdataString) && nTypeList[nIndex].ToLower() != "string")
                    {
                        content[nIndex++] = "--";
                    }
                    else
                    {
                        content[nIndex++] = lastdataString;
                    }
                    break;
                }
                else
                {
                    if (nstartPos == nsPos)
                    {
                        if (nTypeList[nIndex].ToLower() != "string")
                        {
                            content[nIndex++] = "--";
                        }
                        else
                        {
                            content[nIndex++] = "";
                        }
                    }
                    else
                    {
                        content[nIndex++] = str.Substring(nstartPos, nsPos - nstartPos);
                    }
                    nstartPos = nsPos + 1;
                }
            }

            return content;

        }

        /// <summary>
        /// 切分字符串，将text以sep作为分隔符分割
        /// text="this888is888test888string", sep="888"
        /// 返回("this","is","a","test","string")
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static List<string> Split(string text, string sep)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(sep))
            {
                result.Add(text);
                return result;
            }

            int startPos = 0;
            while (startPos < text.Length)
            {
                int pos = text.IndexOf(sep, startPos);
                if (pos < 0)
                {
                    result.Add(text.Substring(startPos));
                    break;
                }
                else
                {
                    if (pos == startPos)
                    {
                        result.Add("");
                    }
                    else
                    {
                        result.Add(text.Substring(startPos, pos - startPos));
                    }
                    startPos = pos + sep.Length;
                }
            }

            if (startPos == text.Length)
            {
                result.Add("");
            }

            return result;
        }

        /// <summary>
        /// 切分字符串，以leftSep和rightSep作为成对的分隔符
        /// text="(one)(two)(three)",leftSep="(",rightSep=")"
        /// 返回("one","two","three")
        /// </summary>
        /// <param name="text"></param>
        /// <param name="leftSep"></param>
        /// <param name="rightSep"></param>
        /// <returns></returns>
        public static List<string> Split(string text, string leftSep, string rightSep)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(leftSep) || string.IsNullOrEmpty(rightSep))
            {
                result.Add(text);
                return result;
            }

            int startPos = 0;
            bool findLeft = true;
            int tokenPos = 0;
            while (startPos < text.Length)
            {
                if (findLeft)
                {
                    int pos = text.IndexOf(leftSep, startPos);
                    if (pos < 0)
                    {
                        break;
                    }

                    tokenPos = pos + leftSep.Length;
                    startPos = pos + leftSep.Length;
                }
                else
                {
                    int pos = text.IndexOf(rightSep, startPos);
                    if (pos < 0)
                    {
                        break;
                    }

                    if (pos == tokenPos)
                    {
                        result.Add("");
                    }
                    else
                    {
                        result.Add(text.Substring(tokenPos, pos - tokenPos));
                    }
                    startPos = pos + rightSep.Length;
                }

                findLeft = !findLeft;
            }

            return result;
        }

        public static Color GetColorByString(string strColor)
        {
            int r = 0;
            int g = 0;
            int b = 0;
            if (strColor.Length == 8 &&
                strColor.IndexOf("[") == 0 &&
                strColor.IndexOf("]") == 7)
            {
                strColor = strColor.Substring(1, 6);
            }
            if (strColor.Length == 6)
            {
                string strR = strColor[0].ToString() + strColor[1].ToString();
                string strG = strColor[2].ToString() + strColor[3].ToString();
                string strB = strColor[4].ToString() + strColor[5].ToString();
                r = Convert.ToInt32(strR, 16);
                g = Convert.ToInt32(strG, 16);
                b = Convert.ToInt32(strB, 16);
            }
            return new Color((float)r / 255, (float)g / 255, (float)b / 255);
        }

        //获取字符串的大概长度，由于客户端和服务器计算字符串长度算法不统一，所以不要用这个借口计算出来结果之后直接和服务器发过来的长度结果判断。
        //此接口只用在对客户端的字符串进行大致计算的时候使用
        public static int GetStringProbableLength(string szString)
        {
            int nLen = 0;
            foreach (char c in szString)
            {
                if (c >= 0 && c < 128)
                {
                    nLen += 1;
                }
                else
                {
                    nLen += 3;
                }
            }

            return nLen;
        }

        //获取字符串在UI上的可允许长度。
        //UI的规则为，字母占用一个字符，汉字占用两个宽度
        public static int GetUIStringProbableLength(string szString)
        {
            int nLen = 0;
            foreach (char c in szString)
            {
                if (c == 'w' || c == 'm')
                {
                    nLen += 2;
                }
                else if (c >= 0 && (c < 'A' || c > 'Z') && c < 128)
                {
                    nLen += 1;
                }
                else
                {
                    nLen += 2;
                }
            }

            return nLen;
        }

        public static int GetStringLenEncoded(string text, System.Text.Encoding encoding)
        {
            return encoding.GetByteCount(text);
        }

        // 如果text格式为#{dddd}，从客户端表格去取字符串，否则返回自己
        public static string GetClientStringOrSelf(string text, params object[] args)
        {
            if (text.Length > 3 && text.StartsWith("#{") && text.EndsWith("}"))
            {
                bool ok = true;
                for (int i = 2; i < text.Length - 1; ++i)
                {
                    char ch = text[i];
                    if (ch != '0'
                        && ch != '1'
                        && ch != '2'
                        && ch != '3'
                        && ch != '4'
                        && ch != '5'
                        && ch != '6'
                        && ch != '7'
                        && ch != '8'
                        && ch != '9'
                        )
                    {
                        ok = false;
                        break;
                    }

                    if (ok)
                    {
                        return StrDictionary.GetClientDictionaryString(text, args);
                    }
                }
            }
            return text;
        }

        #endregion

        #region General
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

        public static Transform FindChildByName(Transform from, string name)
        {
            Transform t = from.Find(name);
            if (t != null)
            {
                return t;
            }
            for (int i = 0; i < from.childCount; i++)
            {
                Transform tmp = FindChildByName(from.GetChild(i), name);
                if (tmp != null) return tmp;
            }
            return null;
        }

        //获取一个节点在编辑器视图中的完整路径
        public static string GetHierarchyPath(Transform from, Transform to)
        {
            Stack<string> items = new Stack<string>();

            Transform t = from.transform;
            if (null == t)
            {
                return "";
            }

            Transform parent = t.transform.parent;

            while (t != to)
            {
                items.Push(t.name);
                t = parent;
                if (t == null) break;
                parent = t.transform.parent;
            }

            StringBuilder sb = new StringBuilder();
            while (items.Count > 0)
            {
                String path = items.Pop();
                sb.Append(path);
                sb.Append('/');
            }
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
        #endregion


        //分析颜色
        public static string ParseForceColor(string text)
        {
            string result = text;

            if (result.Contains("[c]"))
            {
                return result;
            }

            int nColorCount = 0;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == '[' && i + 7 < result.Length && result[i + 7] == ']')
                {
                    if ((('0' <= result[i + 1] && result[i + 1] <= '9') || ('A' <= result[i + 1] && result[i + 1] <= 'Z') || ('a' <= result[i + 1] && result[i + 1] <= 'z')) &&
                        (('0' <= result[i + 2] && result[i + 2] <= '9') || ('A' <= result[i + 2] && result[i + 2] <= 'Z') || ('a' <= result[i + 2] && result[i + 2] <= 'z')) &&
                        (('0' <= result[i + 3] && result[i + 3] <= '9') || ('A' <= result[i + 3] && result[i + 3] <= 'Z') || ('a' <= result[i + 3] && result[i + 3] <= 'z')) &&
                        (('0' <= result[i + 4] && result[i + 4] <= '9') || ('A' <= result[i + 4] && result[i + 4] <= 'Z') || ('a' <= result[i + 4] && result[i + 4] <= 'z')) &&
                        (('0' <= result[i + 5] && result[i + 5] <= '9') || ('A' <= result[i + 5] && result[i + 5] <= 'Z') || ('a' <= result[i + 5] && result[i + 5] <= 'z')) &&
                        (('0' <= result[i + 6] && result[i + 6] <= '9') || ('A' <= result[i + 6] && result[i + 6] <= 'Z') || ('a' <= result[i + 6] && result[i + 6] <= 'z')))
                    {
                        if (nColorCount == 0)
                        {
                            result = result.Insert(i, "[c]");
                            i += 10;
                        }
                        else
                        {
                            i += 7;
                        }
                        nColorCount += 1;
                        continue;
                    }
                }
                if (result[i] == '[' && i + 9 < result.Length && result[i + 9] == ']')
                {
                    if ((('0' <= result[i + 1] && result[i + 1] <= '9') || ('A' <= result[i + 1] && result[i + 1] <= 'Z') || ('a' <= result[i + 1] && result[i + 1] <= 'z')) &&
                        (('0' <= result[i + 2] && result[i + 2] <= '9') || ('A' <= result[i + 2] && result[i + 2] <= 'Z') || ('a' <= result[i + 2] && result[i + 2] <= 'z')) &&
                        (('0' <= result[i + 3] && result[i + 3] <= '9') || ('A' <= result[i + 3] && result[i + 3] <= 'Z') || ('a' <= result[i + 3] && result[i + 3] <= 'z')) &&
                        (('0' <= result[i + 4] && result[i + 4] <= '9') || ('A' <= result[i + 4] && result[i + 4] <= 'Z') || ('a' <= result[i + 4] && result[i + 4] <= 'z')) &&
                        (('0' <= result[i + 5] && result[i + 5] <= '9') || ('A' <= result[i + 5] && result[i + 5] <= 'Z') || ('a' <= result[i + 5] && result[i + 5] <= 'z')) &&
                        (('0' <= result[i + 6] && result[i + 6] <= '9') || ('A' <= result[i + 6] && result[i + 6] <= 'Z') || ('a' <= result[i + 6] && result[i + 6] <= 'z')) &&
                        (('0' <= result[i + 7] && result[i + 7] <= '9') || ('A' <= result[i + 7] && result[i + 7] <= 'Z') || ('a' <= result[i + 7] && result[i + 7] <= 'z')) &&
                        (('0' <= result[i + 8] && result[i + 8] <= '9') || ('A' <= result[i + 8] && result[i + 8] <= 'Z') || ('a' <= result[i + 8] && result[i + 8] <= 'z')))
                    {
                        if (nColorCount == 0)
                        {
                            result = result.Insert(i, "[c]");
                            i += 12;
                        }
                        else
                        {
                            i += 9;
                        }
                        nColorCount += 1;
                        continue;
                    }
                }
                if (result[i] == '[' && i + 2 < result.Length)
                {
                    if (result[i] == '[' && result[i + 1] == '-' && result[i + 2] == ']')
                    {
                        if (nColorCount == 1)
                        {
                            if (i + 2 == result.Length - 1)
                            {
                                result += "[/c]";
                                nColorCount -= 1;
                                break;
                            }
                            else
                            {
                                result = result.Insert(i + 3, "[/c]");
                                nColorCount -= 1;
                            }
                        }
                        else
                        {
                            i += 2;
                            nColorCount -= 1;
                            continue;
                        }
                    }
                }
            }

            if (nColorCount > 0)
            {
                result += "[/c]";
            }

            return result;
        }

        // 清除GRID所有子ITEM
        public static void CleanGrid(GameObject grid)
        {
            if (null == grid)
                return;

            Transform _gridTrans = grid.transform;
            if (null == _gridTrans)
            {
                return;
            }

            int nChildCount = _gridTrans.childCount;
            for (int i = nChildCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(_gridTrans.GetChild(i).gameObject);
            }

            _gridTrans.DetachChildren();
        }


        public static bool SetFlag64(ref ulong flag, int pos, bool bFlag)
        {
            if (pos < 0 || pos >= 64)
            {
                return false;
            }

            if (bFlag)
            {
                flag |= 1ul << pos;
            }
            else
            {
                flag &= (~(1ul << pos));
            }

            return true;
        }

        //将所有自节点的layer进行修改
        public static void SetAllChildLayer(Transform objTransform, int nLayer, int ignoreLayer)
        {
            if (null == objTransform)
                return;

            //先改变自己的
            if (objTransform.gameObject.layer != ignoreLayer)
            {
                objTransform.gameObject.layer = nLayer;
            }

            //如果有子节点，同时改变字节点的Layer
            int nChildCount = objTransform.childCount;
            for (int i = 0; i < nChildCount; ++i)
            {
                Transform _childTrans = objTransform.GetChild(i);
                if (null != _childTrans)
                {
                    SetAllChildLayer(_childTrans, nLayer, ignoreLayer);
                }
            }
        }

        //将所有自节点的layer进行修改
        public static void SetAllChildLayer(Transform objTransform, int nLayer)
        {
            if (null == objTransform)
                return;

            //先改变自己的
            objTransform.gameObject.layer = nLayer;

            //如果有子节点，同时改变字节点的Layer
            int nChildCount = objTransform.childCount;
            for (int i = 0; i < nChildCount; ++i)
            {
                Transform _childTrans = objTransform.GetChild(i);
                if (null != _childTrans)
                {
                    SetAllChildLayer(_childTrans, nLayer);
                }
            }
        }

        public static void SetAvatarVisible(Transform avatarTrans, bool visible, int layer)
        {
            if (null == avatarTrans)
                return;

            //如果是普通节点，改layer
            //如果是特效，改active
            GameObject gameObject = avatarTrans.gameObject;
            if (gameObject.layer == LayerMask.NameToLayer("ObjEffect"))
            {
                gameObject.SetActive(visible);
            }
            else
            {
                gameObject.layer = visible ? layer : LayerMask.NameToLayer("Invisible");
            }

            int nChildCount = avatarTrans.childCount;
            for (int i = 0; i < nChildCount; ++i)
            {
                Transform _childTrans = avatarTrans.GetChild(i);
                if (null != _childTrans)
                {
                    SetAvatarVisible(_childTrans, visible, layer);
                }
            }
        }

        //将所有包含Renderer子节点的层级全部统一
        //这个接口有问题，特效可能不是Particle，还是需要用层级来确认
        public static void SetAllChildRendererLayer(Transform objTransform, int nLayer)
        {
            if (null == objTransform)
                return;

            Renderer[] childRenderers = objTransform.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < childRenderers.Length; i++)
            {
                bool bChangeLayer = true;

                Renderer childRenderer = childRenderers[i];
                if (childRenderer == null)
                {
                    bChangeLayer = false;
                }
                else
                {
                    if (childRenderer.transform == objTransform)
                    {
                        continue;
                    }
                    if (childRenderer is ParticleSystemRenderer)
                    {
                        bChangeLayer = false;
                    }
#if UNITY_IPHONE
					if (UnityEngine.iOS.DeviceGeneration.iPadAir1 == UnityEngine.iOS.Device.generation ||
						UnityEngine.iOS.DeviceGeneration.iPhone6 == UnityEngine.iOS.Device.generation || 
						UnityEngine.iOS.DeviceGeneration.iPadMini3Gen == UnityEngine.iOS.Device.generation)
					{
						if (childRenderer.material.shader != null &&
							childRenderer.material.shader.name != "Standard" &&
							childRenderer.material.shader.name != "Standard (Specular setup)" &&
							childRenderer.material.shader.name != "Legacy Shaders/Transparent/Cutout/Diffuse" &&
							childRenderer.material.shader.name != "Legacy Shaders/Diffuse")
						{
							bChangeLayer = false;
						}
					}
#endif
                }

                if (null != childRenderer && bChangeLayer)
                {
                    childRenderer.gameObject.layer = nLayer;
                }

                if (null != childRenderer && childRenderer.gameObject.transform.childCount > 0)
                {
                    SetAllChildRendererLayer(childRenderer.gameObject.transform, nLayer);
                }
            }
        }




        //根据lable生成省略号
        public static void MakeEllipsis(UILabel word)
        {
            if (word == null || string.IsNullOrEmpty(word.text)) return;

            var strContent = word.text; // UILabel中显示的内容
            string strOut;
            var bWarp = word.Wrap(strContent, out strOut, word.height);
            if (!bWarp && strOut.Length > 1)
            {
                strOut = strOut.Substring(0, strOut.Length - 1);
                strOut += "...";
                word.text = strOut;
            }

        }


        #region 日期相关
        private static DateTime m_startTime = new DateTime(1970, 1, 1);

        public static DateTime GetAnsiDateTime(long seconds)
        {
            DateTime date = new DateTime((long)seconds * 10000000L + m_startTime.Ticks, DateTimeKind.Unspecified);
            date = date.ToLocalTime();
            return date;
        }

        public static int DateToNumeralDay(DateTime curDate)
        {
            return curDate.Year * 10000 + curDate.Month * 100 + curDate.Day;
        }

        //eg:20180504153120
        public static long DateToNumeralTime(DateTime curDate)
        {
            return 10000000000 * curDate.Year
                 + 100000000 * curDate.Month
                 + 1000000 * curDate.Day
                 + 10000 * curDate.Hour
                 + 100 * curDate.Minute
                 + curDate.Second;
        }

        //根据秒来获取折合多少天
        //比如3600秒折合完之后为0天1小时0分0秒
        public static void GetDurationDayFromSec(int nTotalSec, ref int nDay, ref int nHour, ref int nMin, ref int nSec)
        {
            nSec = nTotalSec % 60;

            int nDurationTotalMinutes = nTotalSec / 60;
            nMin = nDurationTotalMinutes % 60;

            nHour = (int)Mathf.Floor(nDurationTotalMinutes / 60) % 24;
            nDay = (int)Mathf.Floor(nDurationTotalMinutes / 1440);
        }

        //根据秒来获取折合多少小时
        //比如3600秒折合完之后为1小时0分0秒
        public static void GetDurationHourFromSec(int nTotalSec, ref int nHour, ref int nMin, ref int nSec)
        {
            nSec = nTotalSec % 60;

            int nDurationTotalMinutes = nTotalSec / 60;
            nMin = nDurationTotalMinutes % 60;
            nHour = (int)Mathf.Floor(nDurationTotalMinutes / 60);
        }


        //本地使用时间戳
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - m_startTime;
            return Convert.ToInt64(ts.TotalSeconds);
        }

        //当最近产生的时间在今天，显示今天消息产生的时间（时：分）
        //当最近产生的时间在昨天，显示“昨天”
        //当最近产生的时间≥2天，显示消息产生的时间“年:月：日”
        public static string GetChatTimeInfo(long tm)
        {
            if (0 >= tm)
                return "";

            var dt = GetAnsiDateTime(tm);//.ToString("yyyy-MM-dd")
            if (dt.Date == DateTime.Now.Date)
                return dt.ToString("HH:mm");

            TimeSpan ts = DateTime.Now - dt;
            if (ts.TotalDays > 2)
                return dt.ToString("yyyy-MM-dd");

            dt.AddDays(-1);
            if (dt.Date == DateTime.Now.Date)
                return StrDictionary.GetDicByID(5195);


            return dt.ToString("yyyy-MM-dd");
        }


        //今天的信息显示（时分秒），昨天的信息显示（昨天+时分秒），两天以上的信息显示（年月日+时分秒）
        public static string GetChatTimeInfo2(long tm)
        {
            if (0 >= tm)
                return "";

            var dt = GetAnsiDateTime(tm);
            if (dt.Date == DateTime.Now.Date)
                return dt.ToString("HH:mm");

            TimeSpan ts = DateTime.Now - dt;
            if (ts.TotalDays > 2)
                return dt.ToString("yyyy-MM-dd HH:mm");

            dt.AddDays(-1);
            if (dt.Date == DateTime.Now.Date)
                return StrDictionary.GetDicByID(5195) + dt.ToString("HH:mm");


            return dt.ToString("yyyy-MM-dd");
        }

        public static TimeSpan GetTimeFromLastOnline(Int64 lastLogin, Int64 lastLogout)
        {
            if (lastLogin < lastLogout)
            {
                long time = GetAnsiDateTime(lastLogout).Ticks;
                time = DateTime.Now.Ticks - time;
                if (time > 0)
                {
                    return new TimeSpan(time);
                }
            }
            return new TimeSpan(0);
        }

        public static string GetOnlineState(Int64 lastLogin, Int64 lastLogout)
        {
            TimeSpan span = GetTimeFromLastOnline(lastLogin, lastLogout);

            if (span.TotalMinutes <= 0)
            {
                return StrDictionary.GetDicByID(5662);//在线
            }
            else if (span.TotalMinutes < 1)
            {
                return StrDictionary.GetDicByID(5663);//1分钟前
            }
            else if (span.TotalMinutes < 60)
            {
                return StrDictionary.GetDicByID(5664, (int)span.TotalMinutes);//x分钟前
            }
            else if (span.TotalHours < 24)
            {
                return StrDictionary.GetDicByID(5665, (int)span.TotalHours);//x小时前
            }
            else if (span.TotalDays >= 1)
            {
                return StrDictionary.GetDicByID(5666, (int)span.TotalDays);//x天前
            }
            return "";
        }

        public static string GetWeekdayString(DayOfWeek day)
        {
            int dicID = -1;
            switch (day)
            {
                case DayOfWeek.Sunday: dicID = 6030; break;
                case DayOfWeek.Monday: dicID = 6031; break;
                case DayOfWeek.Tuesday: dicID = 6032; break;
                case DayOfWeek.Wednesday: dicID = 6033; break;
                case DayOfWeek.Thursday: dicID = 6034; break;
                case DayOfWeek.Friday: dicID = 6035; break;
                case DayOfWeek.Saturday: dicID = 6036; break;
            }

            return StrDictionary.GetDicByID(dicID);
        }


        #endregion


    }

}


