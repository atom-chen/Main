using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class CG_START_GAME_PAK
{
    private Dictionary<byte, object> dic = new Dictionary<byte, object>();
    public Role _Role
    {
        set { dic.Remove((byte)ParameterCode.Role); dic.Add((byte)ParameterCode.Role, ParaTools.GetJson<Role>(value)); }
        get { return ParaTools.GetParameter<Role>(dic, ParameterCode.Role); }
    }
}
