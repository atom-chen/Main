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
    private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();

    public abstract OperationCode OpCode { get; }
    public abstract void OnHandlerMessage(OperationRequest request, OperationResponse response, UserConnect peer, SendParameters sendParameters);

    public HandlerBase()
    {
        Server.Instance.handlers.Add((byte)OpCode, this);
        log.Debug("Hanlder:" + this.GetType().Name + "  is register.");
    }


}

