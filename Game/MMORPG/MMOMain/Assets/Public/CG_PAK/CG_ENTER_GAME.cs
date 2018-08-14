﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class CG_ENTER_GAME_PAK
{
    Dictionary<byte, object> dic = new Dictionary<byte, object>();

    public User _User
    {
        set { dic.Add((byte)ParameterCode.User, ParaTools.GetJson<User>(value)); }
        get { return ParaTools.GetParameter<User>(dic, ParameterCode.User); }
    }
}
