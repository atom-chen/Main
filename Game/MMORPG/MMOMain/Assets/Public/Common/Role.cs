using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class Role
{
    private int ID;//ID
    private string Name;//名称
    private uint Level;//等级
    private bool Sex;//性别
    private int UserID;//所属用户
    private string HeadIcon;//头像
    private int Exp;//经验
    private int Coin;//金币
    private int YuanBao;//元宝
    private int Energy;//体力
    private int Toughen;//历练数
    private Bag BagInfo;//背包信息
    private EquipBag EquipInfo;//装备背包
    private int EnergyNextRecoverTimer;//距离下次恢复的时间(秒)
    private int ToughenNextRecoverTimer;//距离下次恢复的时间(秒)
    public Role()
    {

    }
    public void BuildBag(List<Item> itemList)
    {
        BagInfo = new Bag(itemList);
    }
}