using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class Role
{
    private int ID;                                //ID
    private string Name;                         //名称
    private uint Level;                         //等级
    private bool Sex;                           //性别
    private int UserID;                         //所属用户
    private string HeadIcon;                     //头像
    private int Exp;                              //经验
    private int Coin;                            //金币
    private int YuanBao;                        //元宝
    private int Energy;                               //体力
    private int Toughen;                             //历练数
    private Bag mBag;                                  //背包信息
    private EquipBag mEquipBag;                           //装备背包

    private EquipInfo mEquipInfo=new EquipInfo();       //当前已装备
    private BATTLE_TYPE mBattleType;                    //战斗类型
    private RoleAttribute mRoleAttr;                   //主角属性点

    private int EnergyNextRecoverTimer;     //距离下次恢复的时间(秒)
    private int ToughenNextRecoverTimer;    //距离下次恢复的时间(秒)
    public Role()
    {
        
    }
    public bool CompareToRole(Role role)
    {
        if (ID == role.ID && UserID == role.UserID)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BuildBag(List<Item> itemList)
    {
        mBag = new Bag(itemList);
    }

    public void BuildEquipBag(List<Equip> equipList)
    {
        //将数据存入到装备背包
        mEquipBag = new EquipBag(equipList);
    }

    //穿上一件装备，若有卸下装备则放入到装备背包
    public void FuncEquip(Equip equip)
    {
        Equip old=mEquipInfo.FuncEquip(equip);
        if(old!=null)
        {
            mEquipBag.AddEquipToBag(old);
        }
    }
}