
using FluentNHibernate.Mapping;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMain
{
  class User:PeerBase
  {
    public int Guid = Define._INVALID_ID;
    public string UserName;
    public User(IRpcProtocol protocol, IPhotonPeer unmanagedPeer): base(protocol, unmanagedPeer)
    {
      
    }
    protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
    {

    }

    protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
    {
      switch(operationRequest.OperationCode)
      {
        case MessageCode.USER_LOGIN:
          break;
        case MessageCode.USER_REGISTER:
          break;
        case MessageCode.GET_SERVER_LIST:
          break;
      }
    }

    public bool Login(string userName,string password)
    {
      return false;
    }

  }
  class _DBUser
  {
    public virtual int Guid { get; set; }
    public virtual string UserName { get; set; }
    public virtual string Password { get; set; }
  }

  //映射类
  class UserMap:ClassMap<_DBUser>
  {
    private string m_TableName = "User";
    public UserMap()
    {
      Id(x => x.Guid).Column("Guid");//设置GUID为key
      Map(x => x.UserName).Column("UserName");
      Map(x => x.Password).Column("Password");
      Table(m_TableName);
    }
  }
}
