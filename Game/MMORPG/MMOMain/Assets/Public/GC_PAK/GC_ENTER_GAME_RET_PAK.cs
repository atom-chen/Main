﻿using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public partial class GC_ENTER_GAME_RET_PAK:GC_PAK_BASE
{
    public User _User
    {
        set
        {
            _Response.Parameters.Remove((byte)ParameterCode.User);
            _Response.Parameters.Add((byte)ParameterCode.User, ParaTools.GetJson<User>(value));
        }
        get { return ParaTools.GetParameter<User>(_Response.Parameters, ParameterCode.User); }
    }

    public List<Role> RoleList
    {
        set
        {
            _Response.Parameters.Remove((byte)ParameterCode.RoleList);
            _Response.Parameters.Add((byte)ParameterCode.RoleList, ParaTools.GetJson<List<Role>>(value));
        }
        get { return ParaTools.GetParameter<List<Role>>(_Response.Parameters, ParameterCode.RoleList); }
    }

    public GC_ENTER_GAME_RET_PAK()
    {
        _OperationCode = OperationCode.EnterGame;
    }
}

