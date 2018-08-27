using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class CG_REGISTER_PAK
{
    private Dictionary<byte, object> dic = new Dictionary<byte, object>();
    public User _User
    {
        set { dic.Remove((byte)ParameterCode.Server); dic.Add((byte)ParameterCode.Server, ParaTools.GetJson<User>(value)); }
        get { return ParaTools.GetParameter<User>(dic, ParameterCode.User); }
    }
}