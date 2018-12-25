using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class DBRoutinue:RoutinueBase
{
    public override ROUTINUE_CODE Code
    {
        get { return ROUTINUE_CODE.DB; }
    }
    public override int UseResources
    {
        get { return 10; }
    }
    public override void SetUp()
    {

    }
    public override void Tick()
    {
        //读消息
        while (DB2Main.qMessage.Count>0)
        {
            DM_MsgBase msg = DB2Main.qMessage.Dequeue();
            switch(msg.code)
            {
                case DM_MsgCode.USER_CONNECT:
                    NetRoutinue.ReceiverMsg((DM_UserConnectMsg)msg);
                    break;
                case DM_MsgCode.USER:
                    UserRoutinue.ReceiverMsg((DM_UserMsg)msg);
                    break;
                case DM_MsgCode.ROLE:
                    UserRoutinue.ReceiverMsg((DM_RoleMsg)msg);
                    break;
            }
        }
    }

    public static void SendMsgToDB(MD_MsgBase msg)
    {
        CSMain.Server._IPC2DB.SendMessage(msg);
    }
}
