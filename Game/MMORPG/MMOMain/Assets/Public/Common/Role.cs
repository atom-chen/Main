using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public enum INFO_TYPE
{
  INVALID,
  NAME,
  SEX,
  USERID,
  HEADICON,
  EXP,
  COIN,
  YUANBAO,
  ENERGY,
  TOUGHEN,
}

public partial class Role
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
}