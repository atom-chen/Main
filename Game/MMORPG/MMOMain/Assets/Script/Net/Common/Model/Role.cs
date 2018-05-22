using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Role
{
  private int m_ID;//ID
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

  private string m_Name;//名称
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

  private uint m_Level;//等级
  public uint Level
  {
    get { return m_Level; }
    set { m_Level = value; OnChange(); }
  }

  private bool m_Sex;//性别
  public bool Sex
  {
    get { return m_Sex; }
    set { m_Sex = value; OnChange(); }
  }

  private int m_UserID;//所属用户
  public int UserID
  {
    get { return m_UserID; }
    set { m_UserID = value; OnChange(); }
  }

  private string m_HeadIcon;//头像
  public string HeadIcon
  {
    get { return m_HeadIcon; }
    set { m_HeadIcon = value; OnChange(); }
  }

  private int m_Exp;//经验
  public int Exp
  {
    get { return m_Exp; }
    set { m_Exp = value; OnChange(); }
  }

  private int m_Coin;//金币
  public int Coin
  {
    get { return m_Coin; }
    set { m_Coin = value; OnChange(); }
  }

  private int m_YuanBao;//元宝
  public int YuanBao
  {
    get { return m_YuanBao; }
    set { m_YuanBao = value; OnChange(); }
  }

  private int m_Energy;//体力
  public int Energy
  {
    get { return m_Energy; }
    set { m_Energy = value; OnChange(); }
  }

  private int m_Toughen;//历练数
  public int Toughen
  {
    get { return m_Toughen; }
    set { m_Toughen = value; OnChange(); }
  }

  public delegate void de();

  public de OnInfoChange;

  private void OnChange()
  {
    if(OnInfoChange!=null)
    {
      foreach(Action item in OnInfoChange.GetInvocationList())
      {
        item();
      }
    }
  }

}
