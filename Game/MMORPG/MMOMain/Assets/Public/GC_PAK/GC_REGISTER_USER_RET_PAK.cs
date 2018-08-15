using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public partial class GC_REGISTER_USER_RET_PAK:GC_PAK_BASE
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

    public GC_REGISTER_USER_RET_PAK()
    {
        _OperationCode = OperationCode.Register;
    }
}

