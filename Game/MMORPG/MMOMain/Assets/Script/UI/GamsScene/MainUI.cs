using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour {
  private static MainUI _Instance;
  public static MainUI Instance
  {
    get
    {
      return _Instance;
    }
  }
  void Awake()
  {
    _Instance = this;
  }


  public UILabel m_RoleNameLabel;//名字
  public UILabel m_LevelLabel;//等级
  public UISprite m_Head;//头像
  public UIEventTrigger m_OpenRoleInfo;//点击头像

  public UISlider m_EnergySlider;//体力条
  public UILabel m_EnergyLabel;//体力值
  public UIButton m_EnergyAddBtn;//增加体力

  public UISlider m_ToughenSlider;//历练条
  public UILabel m_ToughenLabel;//历练值
  public UIButton m_ToughenAddBtn;//增加历练

  public UILabel m_CoinLabel;//金币数
  public UIButton m_CoinAdd;//增加金币

  public UILabel m_YuanBaoLebel;//钻石数
  public UIButton m_YuanBaoAdd;//增加钻石 

  public UIEventTrigger m_SystemConfig;//系统
  public UIEventTrigger m_Battle;//战斗
  public UIEventTrigger m_Task;//任务
  public UIEventTrigger m_Skill;//技能
  public UIEventTrigger m_Shop;//商店
  public UIEventTrigger m_Bag;//背包

  void Start()
  {
    m_OpenRoleInfo.onClick.Add(new EventDelegate(OnClickHead));
    m_EnergyAddBtn.onClick.Add(new EventDelegate(OnClickEnergyAdd));
    m_ToughenAddBtn.onClick.Add(new EventDelegate(OnClickToughenAdd));

    m_CoinAdd.onClick.Add(new EventDelegate(OnClickCoinAdd));
    m_YuanBaoAdd.onClick.Add(new EventDelegate(OnClickYuanBaoAdd));

    m_SystemConfig.onClick.Add(new EventDelegate(OnClickConfig));
    m_Battle.onClick.Add(new EventDelegate(OnClickBattle));
    m_Task.onClick.Add(new EventDelegate(OnClickTask));
    m_Skill.onClick.Add(new EventDelegate(OnClickSkill));
    m_Shop.onClick.Add(new EventDelegate(OnClickShop));
    m_Bag.onClick.Add(new EventDelegate(OnClickBag));
  }
  void OnEnable()
  {
    UpdateRoleInfo();
    PlayData.OnRoleInfoChange += UpdateRoleInfo;
  }
  void OnDisable()
  {
    PlayData.OnRoleInfoChange -= UpdateRoleInfo;
  }

  public void SetEnergy(int value)
  {
    int limit = Table_Role.GetEnergyLimit((int)PlayData.RoleData.level);
    m_EnergySlider.value =(float)(value / limit);
    m_EnergyLabel.text = string.Format("{0}/{1}", value, limit);
  }

  public void SetToughen(int value)
  {
      uint limit = Table_Role.GetToughenLimit(PlayData.RoleData.level);
    m_ToughenSlider.value = (float)(value / limit);
    m_ToughenLabel.text = string.Format("{0}/{1}", value, limit);
  }
  //更新角色信息
  public void UpdateRoleInfo()
  {
    //根据当前role赋值
    Role role = PlayData.RoleData;
    m_RoleNameLabel.text = role.name;
    m_LevelLabel.text = role.level.ToString();
    m_Head.spriteName = role.headIcon;
    SetToughen(role.toughen);
    SetEnergy(role.energy);
    m_YuanBaoLebel.text =role.yuanBao.ToString();
    m_CoinLabel.text = role.coin.ToString();
  }

  //点击头像
  private void OnClickHead()
  {
    UIManager.CreateUI("RoleState", UIManager.PopUI);
  }

  //点击增加体力
  private void OnClickEnergyAdd()
  {

  }
  //点击增加历练
  private void OnClickToughenAdd()
  {

  }
  //点击增加金币
  private void OnClickCoinAdd()
  {

  }
  //点击增加元宝
  private void OnClickYuanBaoAdd()
  {

  }
  //点击系统设置
  private void OnClickConfig()
  {

  }
  //点击战斗
  private void OnClickBattle()
  {

  }
  //点击任务
  private void OnClickTask()
  {

  }
  //点击技能
  private void OnClickSkill()
  {

  }
  //点击商店
  private void OnClickShop()
  {

  }
  //点击背包
  private void OnClickBag()
  {

  }
}
