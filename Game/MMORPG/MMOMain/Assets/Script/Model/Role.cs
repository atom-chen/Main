using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public partial class Role
{
    public int id
    {
        get
        {
            return ID;
        }
        set
        {
            ID = value;
            OnChange();
        }
    }

    public string name
    {
        get
        {
            return Name;
        }
        set
        {
            Name = value;
            OnChange();
        }
    }
    public uint level
    {
        get { return Level; }
        set { Level = value; OnChange(); }
    }


    public bool sex
    {
        get { return Sex; }
        set { Sex = value; OnChange(); }
    }


    public int userID
    {
        get { return UserID; }
        set { UserID = value; OnChange(); }
    }


    public string headIcon
    {
        get { return HeadIcon; }
        set { HeadIcon = value; OnChange(); }
    }


    public int exp
    {
        get { return Exp; }
        set { Exp = value; OnChange(); }
    }


    public int coin
    {
        get { return Coin; }
        set { Coin = value; OnChange(); }
    }


    public int yuanBao
    {
        get { return YuanBao; }
        set { YuanBao = value; OnChange(); }
    }


    public int energy
    {
        get { return Energy; }
        set { Energy = value; OnChange(); }
    }


    public int toughen
    {
        get { return Toughen; }
        set { Toughen = value; OnChange(); }
    }


    public int energyNextRecoverTimer
    {
        get { return EnergyNextRecoverTimer; }
        set { EnergyNextRecoverTimer = value; }
    }
    public int toughenNextRecoverTimer
    {
        get { return ToughenNextRecoverTimer; }
        set { ToughenNextRecoverTimer = value; }
    }

    public Bag bag
    {
        get { return BagInfo; }
    }
    /// <summary>
    /// 获取体力全部恢复的时间
    /// </summary>
    /// <returns></returns>
    public int GetEnergyAllRecoverTimer()
    {
        int baseTimer = (Table_Role.GetEnergyLimit((int)Level) - Energy - 1) * Table_Role.GetRecoverSpendTime();
        return baseTimer <= 0 ? 0 : baseTimer + EnergyNextRecoverTimer;
    }
    /// <summary>
    /// 获取历练全部恢复的时间
    /// </summary>
    public int GetToughenAllRecoverTimer()
    {
        int baseTimer = (Table_Role.GetToughenLimit((int)Level) - Toughen - 1) * Table_Role.GetRecoverSpendTime();
        return baseTimer <= 0 ? 0 : baseTimer + ToughenNextRecoverTimer;
    }

    public delegate void de();
    public de OnInfoChange;

    /// <summary>
    /// 信息改变的回调
    /// </summary>
    private void OnChange()
    {
        if (OnInfoChange != null)
        {
            foreach (Action item in OnInfoChange.GetInvocationList())
            {
                item();
            }
        }
    }
}
