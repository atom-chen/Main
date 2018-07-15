using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class StartGameHandler : HandlerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.StartGame; }
    }

    //选定角色，开始游戏
    public override void OnHandlerMessage(Photon.SocketServer.OperationRequest request, Photon.SocketServer.OperationResponse response, UserConnect peer, Photon.SocketServer.SendParameters sendParameters)
    {
        Role msgRole = ParaTools.GetParameter<Role>(request.Parameters, ParameterCode.Role);
        if (msgRole != null)
        {
            Role newRole = RoleManager.RoleOnline(msgRole);
            if (newRole != null)
            {
                response.ReturnCode = (short)ReturnCode.Success;
                peer.LoginRole = newRole;
                //传回去
                response.Parameters.Add((byte)ParameterCode.Role, ParaTools.GetJson<Role>(newRole));
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Fail;
                response.Parameters.Add((byte)ParameterCode.ErrorInfo, "角色信息异常！！");
            }
        }
        else
        {
            CSMain.Server.log.Error("收到的角色信息为空");
        }
    }
}

