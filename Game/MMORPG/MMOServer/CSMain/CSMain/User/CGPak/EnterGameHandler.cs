using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class EnterGameHandler : HandlerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.EnterGame; }
    }

    //请求登录的消息
    public override void OnHandlerMessage(Photon.SocketServer.OperationRequest request, Photon.SocketServer.OperationResponse response, UserConnect peer, Photon.SocketServer.SendParameters sendParameters)
    {
        //user信息
        Dictionary<byte, object> req = request.Parameters;
        object para = null;
        if (req.TryGetValue((byte)ParameterCode.User, out para))
        {
            User user = LitJson.JsonMapper.ToObject<User>(para.ToString());
            int guid = -1;
            //成功
            if ((guid = UserManager.Login(user)) != Define._INVALID_ID)
            {
                user.Guid = guid;
                peer.LoginUser = user;
                response.ReturnCode = (short)ReturnCode.Success;
                response.Parameters.Add((byte)ParameterCode.User, para);
            }
            //密码错误
            else
            {
                response.ReturnCode = (short)ReturnCode.Fail;
                response.Parameters.Add((byte)ParameterCode.ErrorInfo, "密码错误");
            }
        }
        else
        {
            response.ReturnCode = (short)ReturnCode.Error;
        }

        //拿到该玩家所有角色信息
        List<Role> roleList = RoleManager.GetUserAllRole(peer.LoginUser.Guid);
        //传回去
        response.Parameters.Add((byte)ParameterCode.RoleList, ParaTools.GetJson<List<Role>>(roleList));

        peer.SendOperationResponse(response, sendParameters);
    }
}

