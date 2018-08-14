using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class RoleAddHandler : HandlerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.RoleAdd; }
    }

    //添加角色的消息处理
    public override void OnHandlerMessage(Photon.SocketServer.OperationRequest request, Photon.SocketServer.OperationResponse response, UserConnect peer, Photon.SocketServer.SendParameters sendParameters)
    {
        Role role = ParaTools.GetParameter<Role>(request.Parameters, ParameterCode.Role);
        if (role != null)
        {
            role.userID = peer.LoginUser.Guid;
            role.level = 1;
            if (peer.LoginUser == null)
            {
                response.ReturnCode = (short)ReturnCode.Error;
                response.Parameters.Add((byte)ParameterCode.ErrorInfo, "未知异常");
                return;
            }
            role.userID = peer.LoginUser.Guid;
            if (RoleManager.CreateRole(role))
            {
                //创建成功
                response.ReturnCode = (short)ReturnCode.Success;
                response.Parameters.Add((byte)ParameterCode.Role, ParaTools.GetJson<Role>(role));
            }
            else
            {
                //创建失败
                response.ReturnCode = (short)ReturnCode.Fail;
                response.Parameters.Add((byte)ParameterCode.ErrorInfo, "此名称已存在");
            }
        }
        else
        {
            response.ReturnCode = (short)ReturnCode.Error;
            response.Parameters.Add((byte)ParameterCode.ErrorInfo, "收到的角色信息为空");
            LogManager.Error("OperationCode.RoleAdd收到的角色信息为空");
        }

    }
}

