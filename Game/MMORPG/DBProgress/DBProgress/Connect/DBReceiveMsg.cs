using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;

partial class IPCMain
{
    public static void ReceiveMsg(MD_LoginMsg message)
    {
        _DBUser user = UserController.GetUserByUserName(message.LoginUserName);
        //账号错误
        if (user == null)
        {
            DM_UserConnectMsg msg = new DM_UserConnectMsg();
            msg.returnCode = RETURN_CODE.FAILD;
            msg.InfoId = 6;
            msg.RequesterIP = message.IPAddrea;
            DB2Main.SendMessage(msg);
        }
        //登录成功
        else if (user.UserName == message.LoginUserName && user.Password == message.LoginPassword)
        {
            //取出数据
            DM_UserConnectMsg msg = new DM_UserConnectMsg();
            msg.returnCode = RETURN_CODE.SUCCESS;
            msg.RequesterIP = message.IPAddrea;
            msg.User = user;
            msg.RoleList = RoleController.GetRoleByUserID(user.Guid);
            msg.ItemList = new List<_DBItem>();
            msg.EquipList = new List<_DBEquip>();
            //装备背包和物品背包
            if(msg.RoleList!=null)
            {
                foreach(_DBRole role in msg.RoleList)
                {
                    IList<_DBItem> itemList = BagController.GetItemFromRole(role.ID);
                    foreach(_DBItem item in itemList)
                    {
                        msg.ItemList.Add(item);
                    }
                    IList<_DBEquip> equipList = EquipController.GetEquipFromRole(role.ID);
                    foreach (_DBEquip item in equipList)
                    {
                        msg.EquipList.Add(item);
                    }
                }
            }
            DB2Main.SendMessage(msg);
        }
        //密码错误
        else
        {
            DM_UserConnectMsg msg = new DM_UserConnectMsg();
            msg.returnCode = RETURN_CODE.FAILD;
            msg.InfoId = 7;
            msg.RequesterIP = message.IPAddrea;
            DB2Main.SendMessage(msg);
        }
    }
    public static void ReceiveMsg(MD_RegisterMsg msg)
    {

    }
    public static void ReceiveMsg(MD_RoleAddMsg msg)
    {

    }
}

