using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public partial class GC_START_GAME_RET_PAK:GC_PAK_BASE
{
    public Role _Role
    {
        set
        {
            _Response.Parameters.Remove((byte)ParameterCode.Role);
            _Response.Parameters.Add((byte)ParameterCode.Role, ParaTools.GetJson<Role>(value));
        }
        get { return ParaTools.GetParameter<Role>(_Response.Parameters, ParameterCode.Role); }
    }

    public GC_START_GAME_RET_PAK()
    {
        _OperationCode = OperationCode.StartGame;
    }
}

