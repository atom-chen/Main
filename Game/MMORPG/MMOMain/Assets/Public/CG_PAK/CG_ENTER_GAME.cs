using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class CG_ENTER_GAME_PAK
{
    private Dictionary<byte, object> dic = new Dictionary<byte, object>();

    public string userName
    {
        set { dic.Remove((byte)ParameterCode.UserName); dic.Add((byte)ParameterCode.UserName, ParaTools.GetJson<string>(value)); }
        get { return ParaTools.GetParameter<string>(dic, ParameterCode.UserName); }
    }

    public string passWord
    {
        set { dic.Remove((byte)ParameterCode.PassWord); dic.Add((byte)ParameterCode.PassWord, ParaTools.GetJson<string>(value)); }
        get { return ParaTools.GetParameter<string>(dic, ParameterCode.PassWord); }
    }
}
