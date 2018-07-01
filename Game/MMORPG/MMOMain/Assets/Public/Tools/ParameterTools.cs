using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ParaTools
{
  public static T GetParameter<T>(Dictionary<byte, object> dic, ParameterCode code) where T : class
  {
    object para = null;

    if (dic.TryGetValue((byte)code, out para))
    {
      T parameter = LitJson.JsonMapper.ToObject<T>(para.ToString());
      return parameter;
    }
    return null;
  }

  public static string GetJson<T>(T para)
  {
    string json = LitJson.JsonMapper.ToJson(para);
    return json;
  }

  /// <summary>
  /// 获取消息中的错误信息
  /// </summary>
  /// <param name="dic">字典</param>
  /// <returns>错误信息</returns>
  public static string GetErrInfo(Dictionary<byte, object> dic)
  {
      object obj;
      if (dic.TryGetValue((byte)ParameterCode.ErrorInfo, out obj))
      {
          return obj.ToString();
      }
      else
      {
          return null;
      }

  }
}

