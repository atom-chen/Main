﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Role
{
    public Role(DB._DBRole db)
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
            EnergyNextRecoverTimer = Table_Role.GetRecoverSpendTime();
            ToughenNextRecoverTimer = Table_Role.GetRecoverSpendTime();
        }
        catch (Exception ex)
        {
            CSMain.Server.log.Error("Role构造报错" + ex.Message);
        }
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
        int energyLimit = Table_Role.GetEnergyLimit((int)db.Level);
        int toughenLimit = Table_Role.GetToughenLimit((int)db.Level);
        int spend = Table_Role.GetRecoverSpendTime();
        CSMain.Server.log.DebugFormat("{1}上次下线时间{0}", db.LastDownLine, db.Name);
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

