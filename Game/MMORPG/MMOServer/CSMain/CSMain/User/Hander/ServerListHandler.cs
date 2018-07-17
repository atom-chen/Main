using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ServerListHandler : HandlerBase
{

    public override OperationCode OpCode
    {
        get { return OperationCode.GetServer; }
    }

    public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserConnect peer, Photon.SocketServer.SendParameters sendParameters)
    {
        List<ServerPropert> serverList = ServerPropertyManager.GetAllServerPropert();
        if (serverList != null)
        {
            string json = LitJson.JsonMapper.ToJson(serverList);

            response.ReturnCode = (short)ReturnCode.Success;//状态码
            response.Parameters.Add((byte)ParameterCode.ServerList, json);//返回的参数
        }
        else
        {
            response.ReturnCode = (short)ReturnCode.Error;
        }
    }
}

