﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        }
    }
    public uint level
    {
        get { return Level; }
        set { Level = value;  }
    }

    public bool sex
    {
        get { return Sex; }
        set { Sex = value;  }
    }

    public int userID
    {
        get { return UserID; }
        set { UserID = value;  }
    }

    public string headIcon
    {
        get { return HeadIcon; }
        set { HeadIcon = value;  }
    }

    public int exp
    {
        get { return Exp; }
        set { Exp = value; }
    }

    public int coin
    {
        get { return Coin; }
        set { Coin = value; }
    }

    public int yuanBao
    {
        get { return YuanBao; }
        set { YuanBao = value; }
    }

    public int energy
    {
        get { return Energy; }
        set { Energy = value; }
    }

    public int toughen
    {
        get { return Toughen; }
        set { Toughen = value;}
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
        get { return mBag; }
    }

    public EquipBag equipBag
    {
        get { return this.mEquipBag; }
    }

    public EquipInfo equipInfo
    {
        get { return mEquipInfo; }
    }      

    public Role(DB._DBRole db,IList<DB._DBItem> itemList,IList<DB._DBEquip> equipList)
    {
        try
        {
            this.ID = db.ID;
            this.Name = db.Name;
            this.Level = db.Level;
            this.Sex = db.Sex;
            this.UserID = db.UserID;
            this.HeadIcon = db.HeadIcon;
            this.Exp = db.Exp;
            this.Coin = db.Coin;
            this.YuanBao = db.YuanBao;
            this.Energy = db.Energy;
            this.Toughen = db.Toughen;
            EnergyNextRecoverTimer = TableRoleManager.GetRecoverSpendTime();
            ToughenNextRecoverTimer = TableRoleManager.GetRecoverSpendTime();
            Recover(db);
            foreach(DB._DBItem item in itemList)
            {
                if(item.roleID == this.id)
                {
                    mBag.TryAddItem(new Item(item));
                }
            }
            foreach(DB._DBEquip equip in equipList)
            {
                if (equip.roleId == this.id)
                {
                    if(db.IsEquip(equip.equipId))
                    {
                        mEquipInfo.FuncEquip(new Equip(equip));
                    }
                    else
                    {
                        mEquipBag.AddEquipToBag(new Equip(equip));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error("Role构造报错" + ex.Message);
        }
    }


    public bool CompareToDB(DB._DBRole dbRole)
    {
        if (ID == dbRole.ID && Level == dbRole.Level && UserID == dbRole.UserID && Exp == dbRole.Exp && Coin == dbRole.Coin && YuanBao == dbRole.YuanBao)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 恢复体力
    /// </summary>
    /// <param name="db">DB中存储的数据原型：仅允许在玩家刚上线时调用该函数</param>
    public void Recover(DB._DBRole db)
    {
        //根据上次时间增加体力
        if (db.LastDownLine == null)
        {
            this.Energy = 30;
            this.Toughen = 30;
            return;
        }
        DateTime NowOnLineTime = DateTime.Now;//本次上线时间
        int energyLimit = TableRoleManager.GetEnergyLimit((int)db.Level);
        int toughenLimit = TableRoleManager.GetToughenLimit((int)db.Level);
        int spend = TableRoleManager.GetRecoverSpendTime();
        LogManager.Debug("{1}上次下线时间{0}", db.LastDownLine, db.Name);
        int year, month, day, hour, min, second;
        FormatTools.MySQLDateTimeToString(db.LastDownLine, out year, out month, out day, out hour, out min, out second);
        NowOnLineTime = DateTime.Now;

        if (NowOnLineTime.Year - year >= 1)
        {
            //直接回满
            this.Energy = energyLimit;
            this.Toughen = toughenLimit;
            return;
        }
        if (NowOnLineTime.Month - month >= 1)
        {
            //直接回满
            this.Energy = energyLimit;
            this.Toughen = toughenLimit;
            return;
        }
        if (NowOnLineTime.Day - day >= 1)
        {
            //直接回满
            this.Energy = energyLimit;
            this.Toughen = toughenLimit;
            return;
        }
        //小时
        if (NowOnLineTime.Hour - hour >= 1)
        {
            //获取恢复数
            int time = (NowOnLineTime.Hour - hour) * 3600;
            int recover = time / spend;

            this.Energy = db.Energy + recover >= energyLimit ? energyLimit : db.Energy + recover;
            this.Toughen = db.Toughen + recover >= toughenLimit ? toughenLimit : db.Toughen + recover;
        }
        //分钟
        if (NowOnLineTime.Minute - min >= 1)
        {
            //下线分钟数
            int time = (NowOnLineTime.Minute - min) * 60;
            //本次需要恢复数
            int recover = time / spend;

            this.Energy = db.Energy + recover >= energyLimit ? energyLimit : db.Energy + recover;
            this.Toughen = db.Toughen + recover >= toughenLimit ? toughenLimit : db.Toughen + recover;

            //拿到剩余的分钟数，在里面减少
            int delta = time % spend;
            EnergyNextRecoverTimer -= delta;
            ToughenNextRecoverTimer -= delta;
        }
        //秒：直接在剩余分钟数里减少
        if (NowOnLineTime.Second - second >= 1)
        {
            EnergyNextRecoverTimer -= (NowOnLineTime.Second - second);
            ToughenNextRecoverTimer -= (NowOnLineTime.Second - second);
        }
        //如果积攒的小时间足够恢复
        if (EnergyNextRecoverTimer >= spend)
        {
            Energy += EnergyNextRecoverTimer / spend;
            EnergyNextRecoverTimer = EnergyNextRecoverTimer % spend;
        }
        if (ToughenNextRecoverTimer >= spend)
        {
            Toughen += ToughenNextRecoverTimer / spend;
            ToughenNextRecoverTimer = ToughenNextRecoverTimer % spend;
        }
        //防止溢出
        if (this.Energy >= energyLimit)
        {
            Energy = energyLimit;
            EnergyNextRecoverTimer = 0;
        }
        if (this.Toughen >= toughenLimit)
        {
            Toughen = toughenLimit;
            ToughenNextRecoverTimer = 0;
        }

    }

}

namespace DB
{
    public partial class _DBRole
    {

        public _DBRole(Role role)
        {
            try
            {
                ID = role.id;
                Name = role.name;
                Level = role.level;
                Sex = role.sex;
                UserID = role.userID;
                HeadIcon = role.headIcon;
                Exp = role.exp;
                Coin = role.coin;
                YuanBao = role.yuanBao;
                Energy = role.energy;
                Toughen = role.toughen;

                //构建所穿装备信息
                Equip equip = role.equipInfo.GetEquip(0);
                this.Helm = equip.guid;
                equip = role.equipInfo.GetEquip(1);
                this.nCloth = equip.guid;
                equip = role.equipInfo.GetEquip(2);
                this.Weapon = equip.guid;
                equip = role.equipInfo.GetEquip(3);
                this.Shoes = equip.guid;

                equip = role.equipInfo.GetEquip(4);
                this.NeckLace = equip.guid;
                equip = role.equipInfo.GetEquip(5);
                this.BraceLet = equip.guid;
                equip = role.equipInfo.GetEquip(6);
                this.Ring = equip.guid;
                equip = role.equipInfo.GetEquip(7);
                this.Wing = equip.guid;
            }
            catch (Exception ex)
            {
                LogManager.Error("构造DBROLE错误!" + ex.Message);
            }


        }

        //是否是DB模型的已装备
        public bool IsEquip(int guid)
        {
            if (guid == Helm || guid == nCloth || guid == Weapon || guid == Shoes || guid == NeckLace || guid == BraceLet || guid == BraceLet
                || guid == Ring || guid == Wing)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
