using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Role
{
  public int ID;//ID
  public string Name;//名称
  public uint Level;//等级
  public bool Sex;//性别
  public int UserID;//所属用户
  public string HeadIcon;//头像
  public int Exp;//经验
  public int Coin;//金币
  public int YuanBao;//元宝
  public int Energy;//体力
  public int Toughen;//历练数
  public int EnergyNextRecoverTimer;//距离下次恢复的时间(秒)
  public int ToughenNextRecoverTimer;//距离下次恢复的时间(秒)
  public Role()
  {

  }
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
      EnergyNextRecoverTimer = Table_Role.GetRecoverSpendTime();
      ToughenNextRecoverTimer = Table_Role.GetRecoverSpendTime();
      Recover(db);
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

  private void Recover(DB._DBRole db)
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
    int spend=Table_Role.GetRecoverSpendTime();
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
    if (EnergyNextRecoverTimer>=spend)
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
    if(this.Energy>=energyLimit)
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
  public class _DBRole
  {
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual uint Level { get; set; }
    public virtual bool Sex { get; set; }
    public virtual int UserID { get; set; }
    public virtual string HeadIcon { get; set; }
    public virtual int Exp { get; set; }
    public virtual int Coin { get; set; }
    public virtual int YuanBao { get; set; }
    public virtual int Energy { get; set; }
    public virtual int Toughen { get; set; }
    public virtual string LastDownLine { get; set; }

    public _DBRole()
    {

    }

    public _DBRole(Role role)
    {
      ID = role.ID;
      Name = role.Name;
      Level = role.Level;
      Sex = role.Sex;
      UserID = role.UserID;
      HeadIcon = role.HeadIcon;
      Exp = role.Exp;
      Coin = role.Coin;
      YuanBao = role.YuanBao;
      Energy = role.Energy;
      Toughen = role.Toughen;
    }
  }

  public class _DBRoleMap : ClassMap<_DBRole>
  {
    public _DBRoleMap()
    {
      LazyLoad();
      Id(x => x.ID).Column("Id");
      Map(x => x.Name).Column("name");
      Map(x => x.Level).Column("Level");
      Map(x => x.Sex).Column("Sex");
      Map(x => x.UserID).Column("UserID");
      Map(x => x.HeadIcon).Column("HeadIcon");
      Map(x => x.Exp).Column("Exp");
      Map(x => x.Coin).Column("coin");
      Map(x => x.YuanBao).Column("YuanBao");
      Map(x => x.Energy).Column("Energy");
      Map(x => x.Toughen).Column("Toughen");
      Map(x => x.LastDownLine).Column("LastDownLine");
      Table("role");
    }
  }
}