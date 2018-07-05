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
using Games.GlobeDefine;
using Games.LogicObj;
using Games.Table;
using ProtobufPacket;

namespace Games
{
	public partial class Utils
	{
		#region file utils
		// 将一行字符串写入文件
		public static bool WriteStringToFile(string path, string text)
		{
			try
			{
				FileStream fs = new FileStream(path, FileMode.Create);
				StreamWriter sw = new StreamWriter(fs);
				sw.WriteLine(text);

				sw.Close();
				fs.Close();

				return true;
			}
			catch (Exception ex)
			{
				LogModule.ErrorLog(ex);
			}
			return false;
		}

		// 将一行字符串写入文件,如果文件已经存在，则追加字符串
		public static bool AppendStringToFile(string path, string text)
		{
			try
			{
				FileStream fs = new FileStream(path, FileMode.Append);
				StreamWriter sw = new StreamWriter(fs);
				sw.WriteLine(text);

				sw.Close();
				fs.Close();

				return true;
			}
			catch (Exception ex)
			{
				LogModule.ErrorLog(ex);
			}
			return false;
		}

		//从文件读取一个字符串
		public static bool ReadFileString(string path, ref string retString)
		{
			try
			{
				if (!File.Exists(path))
				{
					return false;
				}
				FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
				StreamReader sr = new StreamReader(fs);
				retString = sr.ReadToEnd();
				sr.Close();
				fs.Close();

				return true;
			}
			catch (Exception ex)
			{
				LogModule.ErrorLog(ex.ToString());
				return false;
			}
		}

		// 从文件读取一个INT值
		public static bool ReadFileInt(string path, out int retInt)
		{
			string text = "";
			retInt = 0;
			if (!ReadFileString(path, ref text))
			{
				return false;
			}

			if (!int.TryParse(text, out retInt))
			{
				LogModule.ErrorLog("parse int error path:" + path);
				return false;
			}
			return true;
		}

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
				LogModule.ErrorLog("read md5 file error :" + pathName + " e: " + ex.ToString());
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
				else if(c >=0 && (c<'A' || c > 'Z') && c < 128)
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
	            Transform tmp = FindChildByName(from.GetChild(i),name);
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

		#region UI

        public static string GetIconStrByID(ulong _id, bool needDefalt = false)
        {
            var id = (int) _id;
            var info = TableManager.GetUserIconByID(id, 0);
            if (info != null)
            {
                return info.Icon;
            }

            if (needDefalt)
            {
                var defalt = TableManager.GetUserIconByID(GlobeVar.UserDebfaltIconId, 0);
                if (defalt != null)
                {
                    return defalt.Icon;
                }
            }

            return "";
        }


        //根据变量名称获取UIPathData信息
        //比如传入“MainUI”，获得UIInfo中叫做MainUI的变量
        public static UIPathData GetUIPathDataByMemberName(string szMemberName)
        {
            if (szMemberName.Length <= 0)
            {
                return null;
            }

            //Type t = typeof(UIInfo);
            FieldInfo uiFieldInfo = typeof(UIInfo).GetField(szMemberName);
            if (null != uiFieldInfo)
            {
                return uiFieldInfo.GetValue(new UIInfo()) as UIPathData;
            }

            return null;
        }

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
		public static void SetAllChildLayer(Transform objTransform, int nLayer,int ignoreLayer)
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
			for (int i=0; i < nChildCount; ++i)
			{
				Transform _childTrans = objTransform.GetChild(i);
				if (null != _childTrans)
				{
					SetAllChildLayer(_childTrans, nLayer,ignoreLayer);
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

        public static void SetAvatarVisible(Transform avatarTrans, bool visible,int layer)
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


		//加载一个贴图
		public static Texture2D LoadTexture(string filePath)
		{

			if (filePath == null || filePath.Length <= 0)
			{
				return null;
			}

			string curLoadPath = AssetManager.ResDownloadPath + ("/ui/" + filePath).ToLower() + ".data";
			if(System.IO.File.Exists(curLoadPath))
			{
				string filePathFix = filePath.Replace('\\', '/');
				int lastIndex = filePathFix.LastIndexOf('/');
				string fileName = filePathFix.Substring(lastIndex + 1);

				AssetBundle loadingTextureBundle = AssetBundle.LoadFromFile(curLoadPath);
				if(null != loadingTextureBundle)
				{
					Texture2D _tex = loadingTextureBundle.LoadAsset(fileName) as Texture2D;
					loadingTextureBundle.Unload(false);
					return _tex;
				}
				else
				{
					return null;
				}
			}
			else
			{
				Texture2D _tex = AssetManager.LoadResource(filePath) as Texture2D;
				return _tex;
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
        public static CurrencyShowItem.CurrencyType GetCurrencyByItemId(int nItemId)
        {
            switch ((SpecialItemID)nItemId)
            {
                case SpecialItemID.Yuanbao:
                    return CurrencyShowItem.CurrencyType.Yuanbao;
                case SpecialItemID.Gold:
                    return CurrencyShowItem.CurrencyType.Gold;
                case SpecialItemID.Stamina:
                    return CurrencyShowItem.CurrencyType.Stamina;
                case SpecialItemID.Luck:
                    return CurrencyShowItem.CurrencyType.Luck;
                case SpecialItemID.YinYangPan:
                    return CurrencyShowItem.CurrencyType.YinYangPan;
                case SpecialItemID.AsyncPVPTokens:
                    return CurrencyShowItem.CurrencyType.AsyncPVPTokens;
                case SpecialItemID.Vigour:
                    return CurrencyShowItem.CurrencyType.Vigour;
                case SpecialItemID.Arena:
                    return CurrencyShowItem.CurrencyType.Arena;
                case SpecialItemID.AssistInCoin:
                    return CurrencyShowItem.CurrencyType.AssistInCoin;
                case SpecialItemID.DyeColorItem:
                    return CurrencyShowItem.CurrencyType.DyeColorItem;
                case SpecialItemID.BindYuanbao:
                    return CurrencyShowItem.CurrencyType.BindYuanbao;
                case SpecialItemID.YaoPai:
                    return CurrencyShowItem.CurrencyType.YaoPai;
                case SpecialItemID.GuildGold:
                    return CurrencyShowItem.CurrencyType.GuildGold;
                default:
                    break;
            }
            return CurrencyShowItem.CurrencyType.Invalid;
        }
		#endregion


        #region 日期相关
        private static DateTime m_startTime = new DateTime(1970, 1, 1);
        public static DateTime GetServerDateTime()
        {
            return GetServerAnsiDateTime(GameManager.ServerAnsiTime);
        }
        public static DateTime GetAnsiDateTime(long seconds)
        {
            DateTime date = new DateTime((long)seconds * 10000000L + m_startTime.Ticks, DateTimeKind.Unspecified);
            date = date.ToLocalTime();
            return date;
        }
        public static DateTime GetServerAnsiDateTime(long server_ansi_time)
        { // 参数是服务器ansi时间戳, 指的是距离1970-01-01 00:00:00的秒数
            // 本函数的返回值, 最好不要继续运算, 本函数的返回值,是以服务器时间戳得到的一个时间值,但是这个时间值是以本时区表现的,所以本函数返回值显示的08:00点,并不一定是本设备(本时区)的08:00点
            long server_offset = 8 * 60 * 60; // 默认服务器是东八区
            if (GlobeVar.TYPE_INTERNATION == InternationType.CN)
                server_offset = 8 * 60 * 60;
            else if (GlobeVar.TYPE_INTERNATION == InternationType.TW)
                server_offset = 8 * 60 * 60;

            long local_offset = (long)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds; // 本机的时区

            long s_l_offset = server_offset - local_offset;
            // 举例:
            // 如果服务器是东八区, 手机在东九区, s_l_offset = -3600
            // 如果服务器在东八区, 手机在东七区, s_l_offset = 3600
            // 如果服务器在东八区, 手机在西四区, s_l_offset = 43200 (12小时)
            // 那么,因为本函数的参数是服务器时间戳,例如服务器在东八区, 手机在东九区, 服务器传来的时间戳为+08:00, 那么手机得出的时间就是+09:00
            // 那么,为了拟合到服务器时间, 真实的时间戳 = 服务器时间戳 + s_l_offset偏移
            long real_seconds = server_ansi_time + s_l_offset;

            DateTime date = new DateTime((long)real_seconds * 10000000L + m_startTime.Ticks, DateTimeKind.Unspecified);
            date = date.ToLocalTime();
            return date;
        }
        public static DateTime GetNumeralDateTime(long numeraltime)
        {
            int year = (int)(numeraltime / 10000000000);
            int month = (int)(numeraltime / 100000000 % 100);
            int day = (int)(numeraltime / 1000000 % 100);
            int hour = (int)(numeraltime / 10000 % 100);
            int minute = (int)(numeraltime / 100 % 100);
            int second = (int)(numeraltime % 100);

            DateTime date = new DateTime(year, month, day, hour, minute, second);
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

        //获取本地时间（与服务器校正后的时间）
	    public static long GetCalibratedLoacalTimeStamp()
	    {
	        return GetTimeStamp() + GameManager.DValueTime;
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
            if(dt.Date == DateTime.Now.Date)
                return dt.ToString("HH:mm");

            TimeSpan ts  = DateTime.Now - dt;
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
            int dicID = GlobeVar.INVALID_ID;
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
        
        public static bool IsCopySceneOpen(int id)
        {
            Tab_CopySceneOpen openTab = TableManager.GetCopySceneOpenByID(id, 0);
            if (openTab == null)
            {
                return false;
            }
            DateTime time = Utils.GetServerDateTime();
            int begin = openTab.GetWeekBeginbyIndex((int)time.DayOfWeek);
            int end = openTab.GetWeekEndbyIndex((int)time.DayOfWeek);
            int now = time.Hour * 100 + time.Minute;
            if (begin >= 0 && end >= 0 && begin <= now && now <= end)
            {
                return true;
            }
            return false;
        }

        #endregion

        public static void CenterNotice(string szNotice, bool bFilterRepeat = false)
        {
            string msg = StrDictionary.GetServerDictionaryFormatString(szNotice);
            if (CenterNoticeExController.Ins != null)
            {
                CenterNoticeExController.Ins.NewNotice(msg);
            }
            else
            {
                CenterNoticeExController.CacheNotice(msg);
            }
        }

        public static void CenterNotice(int nDicId, bool bFilterRepeat = false)
        {
            CenterNotice(StrDictionary.GetDicByID(nDicId),bFilterRepeat);
        }

        public static string GetAttrName(AttrType type)
        {
            switch (type)
            {
                case AttrType.Attack:
                    return StrDictionary.GetDicByID(5029);
                case AttrType.Defense:
                    return StrDictionary.GetDicByID(5031);
                case AttrType.MaxHP:
                    return StrDictionary.GetDicByID(5030);
                case AttrType.Speed:
                    return StrDictionary.GetDicByID(5032);
                case AttrType.CritChance:
                    return StrDictionary.GetDicByID(5033);
                case AttrType.CritEffect:
                    return StrDictionary.GetDicByID(5034);
                case AttrType.ImpactChance:
                    return StrDictionary.GetDicByID(5035);
                case AttrType.ImpactResist:
                    return StrDictionary.GetDicByID(5036);
                default:
                    return "";
            }
        }

        public static string GetAttrRefixName(AttrRefixType type)
        {
            switch (type)
            {
                case AttrRefixType.MaxHPAdd:
                    return StrDictionary.GetDicByID(5138);
                case AttrRefixType.MaxHPPercent:
                    return StrDictionary.GetDicByID(5139);
                case AttrRefixType.MaxHPFinal:
                    return StrDictionary.GetDicByID(5140);
                case AttrRefixType.AttackAdd:
                    return StrDictionary.GetDicByID(5141);
                case AttrRefixType.AttackPercent:
                    return StrDictionary.GetDicByID(5142);
                case AttrRefixType.AttackFinal:
                    return StrDictionary.GetDicByID(5143);
                case AttrRefixType.DefenseAdd:
                    return StrDictionary.GetDicByID(5144);
                case AttrRefixType.DefensePercent:
                    return StrDictionary.GetDicByID(5145);
                case AttrRefixType.DefenseFinal:
                    return StrDictionary.GetDicByID(5146);
                case AttrRefixType.SpeedAdd:
                    return StrDictionary.GetDicByID(5147);
                case AttrRefixType.SpeedPercent:
                    return StrDictionary.GetDicByID(5148);
                case AttrRefixType.SpeedFinal:
                    return StrDictionary.GetDicByID(5149);
                case AttrRefixType.CritChanceAdd:
                    return StrDictionary.GetDicByID(5150);
                case AttrRefixType.CritEffectAdd:
                    return StrDictionary.GetDicByID(5151);
                case AttrRefixType.ImpactChanceAdd:
                    return StrDictionary.GetDicByID(5152);
                case AttrRefixType.ImpactResistAdd:
                    return StrDictionary.GetDicByID(5153);
                default:
                    return "";
            }
        }

        public static string GetAttrRefixValueFormatStr(int nRefixType, int nRefixValue)
        {
            string ret = string.Format("+{0}", nRefixValue);
            if (nRefixType == (int)AttrRefixType.MaxHPPercent || nRefixType == (int)AttrRefixType.MaxHPFinal ||
                    nRefixType == (int)AttrRefixType.AttackPercent || nRefixType == (int)AttrRefixType.AttackFinal ||
                    nRefixType == (int)AttrRefixType.DefensePercent || nRefixType == (int)AttrRefixType.DefenseFinal ||
                    nRefixType == (int)AttrRefixType.SpeedPercent || nRefixType == (int)AttrRefixType.SpeedFinal)
            {
                ret = string.Format("+{0}%", (int)(nRefixValue / 100.0f));
            }
            else if (nRefixType == (int)AttrRefixType.CritChanceAdd || nRefixType == (int)AttrRefixType.CritEffectAdd ||
                    nRefixType == (int)AttrRefixType.ImpactChanceAdd || nRefixType == (int)AttrRefixType.ImpactResistAdd)
            {
                ret = string.Format("+{0}%", (int)(nRefixValue / 100.0f));
            }
            return ret;
        }

        public static string GetAttrRefixName(AttrType type, AttrRefixFactor factor)
        {
            switch (type)
            {
                case AttrType.MaxHP:
                    if (factor == AttrRefixFactor.Add)
                    {
                        return GetAttrRefixName(AttrRefixType.MaxHPAdd);
                    }
                    else if (factor == AttrRefixFactor.Percent)
                    {
                        return GetAttrRefixName(AttrRefixType.MaxHPPercent);
                    }
                    else if (factor == AttrRefixFactor.Final)
                    {
                        return GetAttrRefixName(AttrRefixType.MaxHPFinal);
                    }
                    else
                    {
                        return "";
                    }
                case AttrType.Attack:
                    if (factor == AttrRefixFactor.Add)
                    {
                        return GetAttrRefixName(AttrRefixType.AttackAdd);
                    }
                    else if (factor == AttrRefixFactor.Percent)
                    {
                        return GetAttrRefixName(AttrRefixType.AttackPercent);
                    }
                    else if (factor == AttrRefixFactor.Final)
                    {
                        return GetAttrRefixName(AttrRefixType.AttackFinal);
                    }
                    else
                    {
                        return "";
                    }
                case AttrType.Defense:
                    if (factor == AttrRefixFactor.Add)
                    {
                        return GetAttrRefixName(AttrRefixType.DefenseAdd);
                    }
                    else if (factor == AttrRefixFactor.Percent)
                    {
                        return GetAttrRefixName(AttrRefixType.DefensePercent);
                    }
                    else if (factor == AttrRefixFactor.Final)
                    {
                        return GetAttrRefixName(AttrRefixType.DefenseFinal);
                    }
                    else
                    {
                        return "";
                    }
                case AttrType.Speed:
                    if (factor == AttrRefixFactor.Add)
                    {
                        return GetAttrRefixName(AttrRefixType.SpeedAdd);
                    }
                    else if (factor == AttrRefixFactor.Percent)
                    {
                        return GetAttrRefixName(AttrRefixType.SpeedPercent);
                    }
                    else if (factor == AttrRefixFactor.Final)
                    {
                        return GetAttrRefixName(AttrRefixType.SpeedFinal);
                    }
                    else
                    {
                        return "";
                    }
                case AttrType.CritChance:
                    if (factor == AttrRefixFactor.Add)
                    {
                        return GetAttrRefixName(AttrRefixType.CritChanceAdd);
                    }
                    else
                    {
                        return "";
                    }
                case AttrType.CritEffect:
                    if (factor == AttrRefixFactor.Add)
                    {
                        return GetAttrRefixName(AttrRefixType.CritEffectAdd);
                    }
                    else
                    {
                        return "";
                    }
                case AttrType.ImpactChance:
                    if (factor == AttrRefixFactor.Add)
                    {
                        return GetAttrRefixName(AttrRefixType.ImpactChanceAdd);
                    }
                    else
                    {
                        return "";
                    }
                case AttrType.ImpactResist:
                    if (factor == AttrRefixFactor.Add)
                    {
                        return GetAttrRefixName(AttrRefixType.ImpactResistAdd);
                    }
                    else
                    {
                        return "";
                    }
                default:
                    return "";
            }
        }

        public static string GetColorIcon(int color)
        {
            switch ((COLOR)color)
            {
                case COLOR.NONE:
                    return "";
                case COLOR.WHITE:
                    return "XiYouDu_1";
                case COLOR.GREEN:
                    return "XiYouDu_2";
                case COLOR.BLUE:
                    return "XiYouDu_3";
                case COLOR.PURPLE:
                    return "XiYouDu_4";
                case COLOR.ORANGE:
                    return "XiYouDu_5";
                case COLOR.RED:
                    return "XiYouDu_6";
                default:
                    return "";
            }
        }

        // 0-based level
        public static string GetIntimacyIcon(int level)
        {
            switch (level)
            {
                case 0:
                    return "FuLingLu_101";
                case 1:
                    return "FuLingLu_100";
                case 2:
                    return "FuLingLu_99";
            }
            return "";
        }

        public static string GetGuildPriv(ProtobufPacket.GuildPrivilege priv)
        {
            switch (priv)
            {
                case ProtobufPacket.GuildPrivilege.GP_OWNER:
                    return StrDictionary.GetDicByID(5626);
                case ProtobufPacket.GuildPrivilege.GP_ADMIN:
                    return StrDictionary.GetDicByID(5627);
                case ProtobufPacket.GuildPrivilege.GP_REGULAR:
                    return StrDictionary.GetDicByID(5628);
            }
            return "";
        }

        public static void BeginCouterLabel(GameObject go, float from, float to,float duration,float scale = 1.15f)
	    {
	        TweenCounter counter = TweenCounter.Begin(go,duration,from,to);
	        if (scale > 1.0001f)
	        {
                BeginBlinkScale(go, duration, 0.2f, scale);
            }
	    }

        public static TogglePlayTween BeginBlinkScale(GameObject go, float duration, float scaleTime, float scale)
        {
            return BeginBlinkScale(go,duration, scaleTime, new Vector3(scale,scale,1f));
        }

        public static TogglePlayTween BeginBlinkScale(GameObject go, float duration, float scaleTime, Vector3 scale)
        {
            TweenScale tween = go.GetComponent<TweenScale>();
	        if (tween != null)
	        {
	            tween.PlayForward();
                tween.ResetToBeginning();
	        }
            tween = TweenScale.Begin(go, scaleTime, scale);
            return TogglePlayTween.Blink(tween, 1, duration);
        }

	    public static TogglePlayTween BeginBlinkColor(GameObject go, float duration, float colorTime, Color color)
	    {
	        TweenColor tween = go.GetComponent<TweenColor>();
            if (tween != null)
            {
                tween.PlayForward();
                tween.ResetToBeginning();
            }
            tween = TweenColor.Begin(go, colorTime, color);
            return TogglePlayTween.Blink(tween, 1, duration);
        }

        public static string GetSkillDamageTypeName(int type)
        {
            switch ((SkillDamageType)type)
            {
                case SkillDamageType.DamageDealer:
                    return StrDictionary.GetDicByID(5045);  // 单体伤害
                case SkillDamageType.AreaOfEffect:
                    return StrDictionary.GetDicByID(5046);  // 群体伤害
                case SkillDamageType.CrowdControl:
                    return StrDictionary.GetDicByID(5047);  // 控制
                default:
                    return "";
            }
        }

        //检查副本进入
	    public static bool CheckCopySceneRule(int ruleId,bool notice = true, bool checkPlayerNum = false)
	    {
	        Tab_CopySceneRule tab = TableManager.GetCopySceneRuleByID(ruleId, 0);
	        if (tab == null)
	        {
	            return false;
	        }

	        if (tab.MinLevel > 0)
	        {
	            if (GameManager.PlayerDataPool.PlayerHeroData.Level < tab.MinLevel)
	            {
	                if (notice)
	                {
	                    CenterNotice(5388);
	                }
	                return false;
	            }
	        }

            if (tab.MaxLevel > 0)
            {
                if (GameManager.PlayerDataPool.PlayerHeroData.Level > tab.MaxLevel)
                {
                    if (notice)
                    {
                        CenterNotice(5388);
                    }
                    return false;
                }
            }

	        if (tab.Gold > 0)
	        {
	            if (GameManager.PlayerDataPool.BaseAttr.CurGoldCoin < tab.Gold)
	            {
                    if (notice)
                    {
                        CenterNotice(5389);
                    }
                    return false;
	            }
	        }

            if (tab.Stamina > 0)
            {
                if (GameManager.PlayerDataPool.BaseAttr.Stamina < tab.Stamina)
                {
                    if (notice)
                    {
                        if(StoryCopyAssistantManager.IsAssistanting())
                        {
                            if (StoryCopyAssistantStaminaRoot.Ins != null) return false;
                            UIManager.ShowUI(UIInfo.StoryCopyAssistantStaminaRoot);
                        }
                        else
                        {
                            CenterNotice(5387);

                            //如果提示，则弹出提示
                            UIManager.ShowUI(UIInfo.StaminaRoot);
                        }

                    }
                    return false;
                }
            }

	        if (checkPlayerNum)
	        {
                if (tab.PlayerNum > 0)
                {
                    if (GameManager.PlayerDataPool.TeamData.Count() < tab.PlayerNum)
                    {
                        if (notice)
                        {
                            CenterNotice(5667);
                        }
                        return false;
                    }
                }	            
	        }



	        return true;
	    }

	    public static void ShuffleList<T>(List<T> list)
	    {
	        for (int i = 0; i < list.Count-1; i++)
	        {
	            int randIndex = UnityEngine.Random.Range(i+1, list.Count - 1);
                //swap
                T val = list[i];
                list[i] = list[randIndex];
	            list[randIndex] = val;
	        }
	    }

	    public static string GetMonthName(int month)
	    {
	        switch (month)
	        {
                case 1:
	                return StrDictionary.GetDicByID(5450);
                case 2:
                    return StrDictionary.GetDicByID(5451);
                case 3:
                    return StrDictionary.GetDicByID(5452);
                case 4:
                    return StrDictionary.GetDicByID(5453);
                case 5:
                    return StrDictionary.GetDicByID(5454);
                case 6:
                    return StrDictionary.GetDicByID(5455);
                case 7:
                    return StrDictionary.GetDicByID(5456);
                case 8:
                    return StrDictionary.GetDicByID(5457);
                case 9:
                    return StrDictionary.GetDicByID(5458);
                case 10:
                    return StrDictionary.GetDicByID(5459);
                case 11:
                    return StrDictionary.GetDicByID(5460);
                case 12:
                    return StrDictionary.GetDicByID(5461);
                default:
	                return "";
	        }
	    }

	    public static int GetVisibleMask(params ObjVisible[] visible)
	    {
            int mask = 0;
            for (int i = 0; i < visible.Length; i++)
            {
                mask |= (1 << (int)visible[i]);
            }
	        return mask;
	    }

        public static bool IsVisible(int mask, params ObjVisible[] flags)
        {
            return (mask & GetVisibleMask(flags)) == 0;
        }

        public static string GetRareString(CARD_RARE rare)
        {
            switch(rare)
            {
                case CARD_RARE.N: return StrDictionary.GetDicByID(5567);
                case CARD_RARE.R: return StrDictionary.GetDicByID(5568);
                case CARD_RARE.SR: return StrDictionary.GetDicByID(5569);
                case CARD_RARE.SSR: return StrDictionary.GetDicByID(5570);
                case CARD_RARE.UR: return StrDictionary.GetDicByID(5571);
                default: return StrDictionary.GetDicByID(5567);
            }
        }

        public static string GetItemRareString(int rare)
        {
            switch (rare)
            {
                case 0: return StrDictionary.GetDicByID(7060);
                case 1: return StrDictionary.GetDicByID(7061);
                case 2: return StrDictionary.GetDicByID(7062);
                case 3: return StrDictionary.GetDicByID(7063);
                case 4: return StrDictionary.GetDicByID(7064);
                default: return StrDictionary.GetDicByID(7060);
            }
        }

        public static string GetAwakeningItemRareString(int rare)
        {
            switch (rare)
            {
                case 0: return StrDictionary.GetDicByID(7057);
                case 1: return StrDictionary.GetDicByID(7058);
                case 2: return StrDictionary.GetDicByID(7059);
                default: return StrDictionary.GetDicByID(7057);
            }
        }

        #region 图集常量
        public static string CardAtlas = "FulingluIcon";
        public static string CardAtlasLittle = "FulingluIcon_little";
        public static string QuartzAtals = "StarIcon";
	    public static string TalismanAtals = "Icon";
	    public static string QuartzListAtlas = "StarIcon";
        public static string IconRoleAtlas = "IconRole";
        public static string PhotoAtlas = "Photo";
        #endregion

        public static int[] TalismanMatItems = new int[]
        {
            (int)SpecialItemID.TalismanMat_0,
            (int)SpecialItemID.TalismanMat_1,
            (int)SpecialItemID.TalismanMat_2,
            (int)SpecialItemID.TalismanMat_3,
        };

	    public static int GetTalismanMatItemId(int dropId)
	    {
	        if (dropId < 0 || dropId >= TalismanMatItems.Length)
	        {
	            return -1;
	        }
	        return TalismanMatItems[dropId];
	    }

        public static int[] DrawCardMatItems = new int[]
        {
            (int)SpecialItemID.DrawCardMat_0,
            (int)SpecialItemID.DrawCardMat_1,
            (int)SpecialItemID.DrawCardMat_2,
        };

        public static int GetDrawCardMatItemId(int dropId)
	    {
            if (dropId < 0 || dropId >= DrawCardMatItems.Length)
            {
                return -1;
            }
            return DrawCardMatItems[dropId];
        }

        public static void SetIcon(UISprite itemIconSprite, string atalsName, string iconName)
        {
            if (null == itemIconSprite)
            {
                return;
            }

            UIAtlas atlas = AssetManager.GetAtlas(atalsName);
            if (null != atlas)
            {
                itemIconSprite.atlas = atlas;
                itemIconSprite.spriteName = iconName;
            }
        }


        #region Scene

        //创建NPC判断条件
        static public bool IsCanCreateNpcByRule(Tab_SceneNpc tab)
        {
            return tab != null && IsCanCreateNpcByRule(tab.CreateRule, tab.MiscID);
        }

        
        static public bool IsCanCreateNpcByRule(int CreateRuleId, int npcMiscId)
        {
            if (CreateRuleId == GlobeVar.INVALID_ID)
            {
                return true;
            }

            Tab_SceneNpcCreateRule tRule = TableManager.GetSceneNpcCreateRuleByID(CreateRuleId, 0);
            if (null == tRule)
            {
                return false;
            }

            //首先根据条件来判断是否需要隐藏某个NPC
            //判断某个StoryFlag是否被置1，如果为1则隐藏
            if (tRule.HideFlagID > GlobeVar.INVALID_ID && tRule.HideFlagID < GlobeVar.STORY_FLAG_CAP)
            {
                if (null == GameManager.PlayerDataPool.m_bStoryFlagList || GameManager.PlayerDataPool.m_bStoryFlagList[tRule.HideFlagID])
                {
                    return false;
                }
            }

            //判断某个StoryLine是否完成，如果m_Fin次数大于0则隐藏
            if (tRule.HideLineID > GlobeVar.INVALID_ID && tRule.HideLineID < GlobeVar.STORY_LINE_CAP)
            {
                if (null != GameManager.PlayerDataPool.m_nStoryLineStatusList &&
                    null != GameManager.PlayerDataPool.m_nStoryLineStatusList[tRule.HideLineID] &&
                    GameManager.PlayerDataPool.m_nStoryLineStatusList[tRule.HideLineID].m_nFin > 0)
                {
                    return false;
                }
            }

            //根据条件来判断是否可以创建该NPC
            //判断某个StoryFlag是否被置1，如果为1则刷出
            if (tRule.StoryFlagID > GlobeVar.INVALID_ID && tRule.StoryFlagID < GlobeVar.STORY_FLAG_CAP)
            {
                if (null == GameManager.PlayerDataPool.m_bStoryFlagList || GameManager.PlayerDataPool.m_bStoryFlagList[tRule.StoryFlagID] == false)
                {
                    return false;
                }
            }

            //判断某个StoryLine是否完成，如果m_Fin次数大于0则刷出
            if (tRule.StoryLineID > GlobeVar.INVALID_ID && tRule.StoryLineID < GlobeVar.STORY_LINE_CAP)
            {
                if (null == GameManager.PlayerDataPool.m_nStoryLineStatusList)
                {
                    return false;
                }

                if (null != GameManager.PlayerDataPool.m_nStoryLineStatusList[tRule.StoryLineID] &&
                    GameManager.PlayerDataPool.m_nStoryLineStatusList[tRule.StoryLineID].m_nFin <= 0)
                {
                    return false;
                }
            }

            if (tRule.EnvID > GlobeVar.INVALID_ID && tRule.EnvID < (int)SCENE_ENVIRONMENT_TYPE.NUM)
            {
                if (null == GameManager.EnvManager || null == GameManager.EnvManager.CurEvnTable)
                {
                    return false;
                }

                if (GameManager.EnvManager.CurEvnTable.EnvirType != tRule.EnvID)
                {
                    return false;
                }
            }

            if (tRule.HasCardID > GlobeVar.INVALID_ID)
            {
                if (!GameManager.PlayerDataPool.PlayerCardBag.IsHaveCard(tRule.HasCardID))
                {
                    return false;
                }
            }

            if (tRule.FunctionUnlock > GlobeVar.INVALID_ID && TutorialManager.IsFunctionEntrySceneNpc(tRule.FunctionUnlock))
            {
                if (false == TutorialManager.IsFunctionUnlock(tRule.FunctionUnlock))
                {
                    return false;
                }
            }

            if (tRule.HeroID > GlobeVar.INVALID_ID)
            {
                var hero = GameManager.PlayerDataPool.PlayerHeroData.GetCurHero();
                if (hero == null || hero.HeroId != tRule.HeroID)
                {
                    return false;
                }
            }

            // npcMiscId 跨人妖界处理创建NPC
            if (!NPCMiscManager.IsCanCreateNpcByRule(npcMiscId))
            {
                return false;
            }

            if (tRule.ReqTutorialID > GlobeVar.INVALID_ID && !GameManager.PlayerDataPool.IsTutorialQuestFinish(tRule.ReqTutorialID))
            {
                return false;
            }

            return true;
        }



	    public static bool IsCanCreateNpcSpecial(Tab_SceneNpc tSceneNpc)
	    {
	        if (tSceneNpc == null)
	        {
	            return false;
	        }

	        if (tSceneNpc.Id == GlobeVar.TutorialQuest_SceneNpcId || tSceneNpc.Id == GlobeVar.TutorialQuest_SceneNpcId_LuoYang)
	        {
	            // 新手任务NPC特殊处理
	            return !GameManager.PlayerDataPool.IsAllTutorialQuestFinish();
	        }

	        if (tSceneNpc.Id == GlobeVar.CircleQuest_SceneNpcId)
	        {
                // 环任务NPC特殊处理
                return GameManager.PlayerDataPool.IsAllTutorialQuestFinish();
            }

	        return true;
	    }
        /// <summary>
        /// 当前场景是否可以显示npc 功能icon
        /// </summary>
        /// <returns></returns>
        public static bool IsCanShowNpcFunIcon()
        {
            if (GameManager.CurScene == null)
            {
                return false;
            }
         
            if (!GameManager.CurScene.IsStoryScene() && (GameManager.CurScene.IsFunctionScene() 
                || GameManager.CurScene.IsLobby()
                || GameManager.CurScene.IsRealTimeScene()
                || GameManager.CurScene.IsHouseScene()))
            {
                return true;
            }
            return false;
        }
        #endregion
        public static UIWidget GetWidget(Transform t,string name)
        {
            if (t==null)
            {
                return null;
            }
            Transform trans = t.Find(name);
            if (trans == null)
            {
                return null;
            }
            UIWidget wid = trans.GetComponent<UIWidget>();
            if (wid == null)
            {
                return null;
            }
            return wid;
        }
        public static string PrintNum(long nNum, bool bShort = false)
	    {
	        if (bShort && nNum >= 10000)
	        {
                return StrDictionary.GetDicByID(5970, nNum / 10000);
            }
	        else
	        {
	            return nNum.ToString();
	        }
	    }

	    public static string GetChineseNum(int num)
	    {
	        switch (num)
	        {
                case 1:
	                return StrDictionary.GetDicByID(6342);
                case 2:
                    return StrDictionary.GetDicByID(6343);
                case 3:
                    return StrDictionary.GetDicByID(6344);
                case 4:
                    return StrDictionary.GetDicByID(6345);
                case 5:
                    return StrDictionary.GetDicByID(6346);
                case 6:
                    return StrDictionary.GetDicByID(6347);
                case 7:
                    return StrDictionary.GetDicByID(6348);
                case 8:
                    return StrDictionary.GetDicByID(6349);
                case 9:
                    return StrDictionary.GetDicByID(6350);
                case 10:
                    return StrDictionary.GetDicByID(6351);
                default:
	                return "";
	        }
	    }

        public static Quaternion DirServerToClient(float rad)
        {
            return Quaternion.Euler(0, 90.0f - rad * 180.0f / Mathf.PI, 0);
        }

        public static float DirClientToServer(Quaternion rotate)
        {
            return Mathf.PI * 0.5f - rotate.eulerAngles.y * Mathf.PI / 180.0f;
        }

        public static void ReqBeginChat(ulong guid, string szName, ulong icon, int nLevel)
        {
            if (LoginData.user == null || LoginData.user.guid == guid)
            {
                return;
            }

            //UIManager.CloseUI(UIInfo.QuickIntercourseRoot);
            SocialController.Show(guid, szName, icon, nLevel);
        }

        //加好友必须调用接口  //处理黑名单
        public static void ReqAddFriend(ulong guid, string defaltMsg = "")
        {
            if (LoginData.user == null || LoginData.user.guid == guid)
            {
                return;
            }

            if (null == GameManager.PlayerDataPool)
            {
                return;
            }

            if (null != GameManager.PlayerDataPool.Friends && GameManager.PlayerDataPool.Friends.IsExist(guid))
            {
                var relation = GameManager.PlayerDataPool.Friends.RelationDataList[guid];
                if (relation != null)
                {
                    CenterNotice(StrDictionary.GetClientDictionaryString("#{5343}", relation.Name));
                }
                
                return;
            }

            if (null != GameManager.PlayerDataPool.BlackList)
            {
                GameManager.PlayerDataPool.BlackList.DelRelation(guid);
            }

            var msg = defaltMsg;
            if (string.IsNullOrEmpty(msg))
            {
                if (null != LoginData.user)
                {
                    //我是{0}
                    msg = StrDictionary.GetClientDictionaryString("#{5137}", LoginData.user.name);
                }                
            }
        

            var packet = new CG_ADD_FRIEND_PAK();

            packet.data.guid = guid;
            packet.data.realAddFrienType = (int)CG_ADD_FRIEND.AddFrienType.RETTYPE_TRYADD;
            packet.data.addfriendmsg = msg;

            packet.SendPacket();
        }

        public static void ReqAddBlack(ulong guid, string szName, ulong icon, int nLevel, int nHeadFrame)
        {
            if (LoginData.user == null || LoginData.user.guid == guid)
            {
                return;
            }

            if (GameManager.PlayerDataPool.BlackList == null)
            {
                return;
            }

            if (GameManager.PlayerDataPool.BlackList.IsExist(guid))
            {
                CenterNotice(StrDictionary.GetClientDictionaryString("#{6317}", szName));
                return;
            }

            CG_ADDBLACKLIST_PAK packet = new CG_ADDBLACKLIST_PAK();
            packet.data.guid = guid;
            packet.data.Name = szName;
            packet.data.icon = icon;
            packet.data.Level = nLevel;
            packet.data.HeadFrame = nHeadFrame;

            packet.SendPacket();
        }

        public static void ReqInviteGuild(ulong guid)
        {
            if (LoginData.user == null || LoginData.user.guid == guid)
            {
                return;
            }

            if (!GameManager.PlayerDataPool.GuildData.Valid)
            {
                CenterNotice(StrDictionary.GetClientDictionaryString("#{5701}"));
            }
            else
            {
                Guild.SendTypicalOperation(GuildOperation.GO_INVITATION,
                   GameManager.PlayerDataPool.GuildData.Guid,
                   guid);
            }
        }

        public static void OnWindowSocialSpaceShow(bool bSuccess, object param)
        {
            if (false == bSuccess)
            {
                return;
            }

            if (LoginData.user == null)
            {
                return;
            }

            if (SocialController.Instance() != null)
            {
                SocialController.Instance().ShowSocialSpace();
            }

            ulong guid = (ulong)param;
            CG_SOCIALSPACE_VISIT_PAK packet = new CG_SOCIALSPACE_VISIT_PAK();
            packet.data.SpaceOwnerGuid = guid;
            packet.SendPacket();
        }

        //请求别人空间
        public static void ReqVisitSpace(ulong guid)
        {
            if (LoginData.user == null)
            {
                return;
            }
            else if(LoginData.user.guid == guid)
            {
                //自己
                UIManager.ShowUI(UIInfo.SocialIntercourseRoot, OnWindowSocialSpaceShow, guid);
                return;
            }
            else
            UIManager.ShowUI(UIInfo.SocialIntercourseRoot, OnWindowSocialSpaceShow, guid);

        }

        public static void ReqSolo(ulong guid)
	    {
            if (LoginData.user == null || LoginData.user.guid == guid)
            {
                return;
            }

            CG_SOLO_INVITE_PAK pak = new CG_SOLO_INVITE_PAK();
	        pak.data.ReceiverGuid = guid;
            pak.SendPacket();
	    }

        public static Tab_UserIcon RandUserIcon(int type)
        {
            List<Tab_UserIcon> list = new List<Tab_UserIcon>();
            var table = TableManager.GetUserIcon();
            foreach (var data in table)
            {
                foreach (var item in data.Value)
                {
                    if (item.Mark == type)
                    {
                        list.Add(item);
                    }
                }
            }
            if (list.Count == 0)
                return null;

            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        public static Color ParseColor(string colorHex)
        {
            if (colorHex.Length != 6)
            {
                return Color.white;
            }
            Color color = new Color();
            color.a = 1f;
            color.r = int.Parse(colorHex.Substring(0, 2), NumberStyles.HexNumber) / 255.0f;
            color.g = int.Parse(colorHex.Substring(2, 2), NumberStyles.HexNumber) / 255.0f;
            color.b = int.Parse(colorHex.Substring(4, 2), NumberStyles.HexNumber) / 255.0f;
            return color;
        }

        #region 根据平台读取配置
        public static Tab_PlatformPath GetPlatformPath()
        {
            int tabid = (int)PlatformHelper.GetTipUrlType();
            return TableManager.GetPlatformPathByID(tabid, 0);
        }
        #endregion

	    public static void ParseGuid(ulong guid, out int wolrdid, out int type, out int carry, out long serial)
	    {
	        wolrdid = (int)(guid >> 48);
	        type = (int)((guid >> 40) & ((ulong)Mathf.Pow(2, 8) - 1));
	        carry = (int)((guid >> 32) & ((ulong)Mathf.Pow(2, 8) - 1));
            serial = (long)(guid & ((ulong)Mathf.Pow(2, 32) - 1));
        }

	    public static int GetWorldIdByGuid(ulong guid)
	    {
	        int worldid, type, carry;
	        long serial;
            ParseGuid(guid, out worldid, out type, out carry, out serial);

	        return worldid;
	    }

        //判断某个ID是否为主角特殊ID，返回模型ID，否则返回-1
        //参数对应字典HERO_SP
        public static int GetSpecialHeroID(int nHeroSpID)
        {
            if (null != GlobeVar.HERO_SP && GlobeVar.HERO_SP.ContainsKey(nHeroSpID))
            {
                return GlobeVar.HERO_SP[nHeroSpID];
            }

            return GlobeVar.INVALID_ID;
        }

	    public static int GetRealDyeColorId(int colorId)
	    {
	        int realcolor = colorId;
	        int heroid = GetSpecialHeroID(colorId);
	        if (heroid != GlobeVar.INVALID_ID)
	        {
	            if (GameManager.PlayerDataPool != null)
	            {
	                realcolor = GameManager.PlayerDataPool.PlayerHeroData.GetHeroDyeColor(heroid);
	            }
	            else
	            {
	                realcolor = GlobeVar.INVALID_ID;
	            }
	        }
	        return realcolor;
        }

	    public static int GetRealCharModel(int rolebaseId)
	    {
	        int realChar = GlobeVar.INVALID_ID;
            int heroid = GetSpecialHeroID(rolebaseId);
	        if (heroid != GlobeVar.INVALID_ID)
	        {
                if (GameManager.PlayerDataPool != null)
                {
                    var hero = GameManager.PlayerDataPool.PlayerHeroData.GetHero(heroid);
                    if (hero != null)
                    {
                        realChar = hero.GetCharModelId();
                    }
                }
            }
	        else
	        {
	            Tab_RoleBaseAttr tab = TableManager.GetRoleBaseAttrByID(rolebaseId, 0);
	            if (tab != null)
	            {
	                realChar = tab.CharModelID;
	            }
	        }
	        return realChar;
	    }

	    public static string GetRealName(int rolebaseId)
	    {
	        string name = "";
            int heroid = GetSpecialHeroID(rolebaseId);
            if (heroid != GlobeVar.INVALID_ID)
            {
                if (GameManager.PlayerDataPool != null)
                {
                    var hero = GameManager.PlayerDataPool.PlayerHeroData.GetHero(heroid);
                    if (hero != null)
                    {
                        name = hero.GetName();
                    }
                }
            }
            else
            {
                Tab_RoleBaseAttr tab = TableManager.GetRoleBaseAttrByID(rolebaseId, 0);
                if (tab != null)
                {
                    name = tab.Name;
                }
            }
            return name;
        }

	    //读取当前资源版本
        public static int ReadLocalResVersion()
        {
            int localResVersion = 0;
            string LocalVersionPath = Application.persistentDataPath;
            string localVersionFilePath = LocalVersionPath + "/ResData/" + "ResVersion.txt";
          
            // 读取本地资源版本号文件
            if (File.Exists(localVersionFilePath))
            {
                LogModule.DebugLog("read local version:" + localVersionFilePath.ToString());
                if (!ReadFileInt(localVersionFilePath, out localResVersion))
                {
                    LogModule.ErrorLog("parse version error");
                }
            }
            else
            {
                LogModule.DebugLog("read local version.text is not exist");
            }
            return localResVersion;
        }

        //读取灰度更新奖励ID
        public static int ReadLocalGrayUpdateID()
        {
            int grayUpdateID = 0;
            string LocalVersionPath = Application.persistentDataPath;
            string localVersionFilePath = LocalVersionPath + "/ResData/" + "GrayUpdateID.txt";
            // 读取本地资源版本号文件
            if (File.Exists(localVersionFilePath))
            {
                LogModule.DebugLog("read local GrayUpdateID:" + localVersionFilePath.ToString());
                if (!ReadFileInt(localVersionFilePath, out grayUpdateID))
                {
                    LogModule.ErrorLog("parse GrayUpdateID error");
                }
            }
            else
            {
                LogModule.DebugLog("read local GrayUpdateID.text is not exist");
            }
            return grayUpdateID;
        }
        //获取当前货币数量（可是普通物品）
        static public long GetMoneyCount(int nMoneyType)
        {
            PlayerData player = GameManager.PlayerDataPool;
            switch (nMoneyType)
            {
                case (int)SpecialItemID.Yuanbao:
                    return player.GetYuanBao();
                case (int)SpecialItemID.BindYuanbao:
                    return player.GetBindYuanBao();
                case (int)SpecialItemID.Gold:
                    return player.GetGold();
                case (int)SpecialItemID.Stamina:
                    return player.GetStamina();
                case (int)SpecialItemID.Vigour:
                    return player.GetVigour();
                case (int)SpecialItemID.AssistInCoin:
                    return player.BaseAttr.LoanGold;
                case (int)SpecialItemID.GuildGold:
                    return (long)player.GuildData.GuildGold;
                default:
                    return player.GetItemPack(ITEMPACK_TYPE.COMMON).GetItemCountByDataId(nMoneyType);
            }

        }
        //数量不足时 加上红色
        public static string GetPriceStr(int nMoneyType, int nPrice)
        {
            if (nMoneyType < 0 || nPrice < 0)
            {
                return nPrice.ToString();
            }
            long nCurMoney = GetMoneyCount(nMoneyType);
            if (nCurMoney < nPrice)
            {
                return "[c]" + GlobeVar.COLOR_RED + nPrice.ToString() + "[-][/c]";
            }
            return nPrice.ToString();
        }


        public static string GetBubbleText(int cardId, int type)
        {
            Tab_BattleBubbleCard tab = TableManager.GetBattleBubbleCardByID(cardId, 0);
            if (tab == null)
            {
                return string.Empty;
            }

            BubbleType etype = (BubbleType) type;
            switch (etype)
            {
                case BubbleType.BattleStart:
                    return tab.BattleStart;
                case BubbleType.UseSkill:
                    return tab.UseSkill;
                case BubbleType.Die:
                    return tab.Die;
                case BubbleType.Win:
                    return tab.Win;
                case BubbleType.Lose:
                    return tab.Lose;
                case BubbleType.Talk:
                    return tab.Chat;
                case BubbleType.Response:
                    return tab.Response;
                default:
                    return string.Empty;
            }
            return string.Empty;
        }
    }

}


