using CSMain;
using ExitGames.Logging;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class HandlerBase
{

    public abstract OperationCode OpCode { get; }
    public abstract void OnHandlerMessage(OperationRequest request, OperationResponse response, UserConnect peer, SendParameters sendParameters);

    public HandlerBase()
    {
        Server.Instance.handlers.Add((byte)OpCode, this);
        LogManager.Debug("Hanlder:" + this.GetType().Name + "  is register.");
    }


}

