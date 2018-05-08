using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
}

