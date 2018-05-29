using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Role
{
  private int m_ID;//ID
  private string m_Name;//名称
  private uint m_Level;//等级
  private bool m_Sex;//性别
  private int m_UserID;//所属用户
  private string m_HeadIcon;//头像
  private int m_Exp;//经验
  private int m_Coin;//金币
  private int m_YuanBao;//元宝
  private int m_Energy;//体力
  private int m_Toughen;//历练数
  private int m_EnergyNextRecoverTimer;//距离下次恢复的时间(秒)
  private int m_ToughenNextRecoverTimer;//距离下次恢复的时间(秒)
  public int ID
  {
    get
    {
      return m_ID;
    }
    set
    {
      m_ID = value;
      OnChange();
    }
  }

  public string Name
  {
    get
    {
      return m_Name;
    }
    set
    {
      m_Name = value;
      OnChange();
    }
  }
  public uint Level
  {
    get { return m_Level; }
    set { m_Level = value; OnChange(); }
  }


  public bool Sex
  {
    get { return m_Sex; }
    set { m_Sex = value; OnChange(); }
  }


  public int UserID
  {
    get { return m_UserID; }
    set { m_UserID = value; OnChange(); }
  }


  public string HeadIcon
  {
    get { return m_HeadIcon; }
    set { m_HeadIcon = value; OnChange(); }
  }


  public int Exp
  {
    get { return m_Exp; }
    set { m_Exp = value; OnChange(); }
  }


  public int Coin
  {
    get { return m_Coin; }
    set { m_Coin = value; OnChange(); }
  }


  public int YuanBao
  {
    get { return m_YuanBao; }
    set { m_YuanBao = value; OnChange(); }
  }


  public int Energy
  {
    get { return m_Energy; }
    set { m_Energy = value; OnChange(); }
  }


  public int Toughen
  {
    get { return m_Toughen; }
    set { m_Toughen = value; OnChange(); }
  }


  public int EnergyNextRecoverTimer
  {
    get { return m_EnergyNextRecoverTimer; }
    set { m_EnergyNextRecoverTimer = value; }
  }
  public int ToughenNextRecoverTimer
  {
    get { return m_ToughenNextRecoverTimer; }
    set { m_ToughenNextRecoverTimer = value; }
  }
  
  /// <summary>
  /// 获取体力全部恢复的时间
  /// </summary>
  /// <returns></returns>
  public int GetEnergyAllRecoverTimer()
  {
    int baseTimer=(Table_Role.GetEnergyLimit(m_Level)-m_Energy-1)* Table_Role.GetRecoverSpendTime();
    return baseTimer <= 0 ? 0 : baseTimer + m_EnergyNextRecoverTimer;
  }
  /// <summary>
  /// 获取历练全部恢复的时间
  /// </summary>
  public int GetToughenAllRecoverTimer()
  {
    int baseTimer = (Table_Role.GetToughenLimit(m_Level) - m_Toughen - 1) * Table_Role.GetRecoverSpendTime();
    return baseTimer<=0?0:baseTimer + m_ToughenNextRecoverTimer;
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
