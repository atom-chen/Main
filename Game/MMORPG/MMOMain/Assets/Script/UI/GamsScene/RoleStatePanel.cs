using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleStatePanel : MonoBehaviour {
  public UISprite m_HeadIcon;//头像
  public UILabel m_LevelLabel;//等级

  public UILabel m_NameLabel;//名称
  public UILabel m_BattleNumLabel;//战斗力
  public UIButton m_RenameBtn;//重命名

  public UISlider m_ExpSlider;//经验
  public UILabel m_ExpLabel;

  public UILabel m_CoinLabel;//金币
  public UILabel m_YuanBaoLabel;//元宝

  public UILabel m_EnergyLabel;//体力
  public UILabel m_EnergyRecoverNext;
  public UILabel m_EnergyRecoverAll;

  public UILabel m_ToughenLabel;//历练
  public UILabel m_ToughenRecoverNext;
  public UILabel m_ToughenRecoverAll;

  public UIButton m_ExitBtn;
  void Start()
  {
    m_ExitBtn.onClick.Add(new EventDelegate(Exit));
  }
  void OnEnable()
  {
    UpdateInfo();
    PlayData.OnRoleInfoChange += UpdateInfo;
  }
  void OnDisable()
  {
    PlayData.OnRoleInfoChange -= UpdateInfo;
  }

  private void UpdateInfo()
  {
    Role role=PlayData.RoleData;
    m_HeadIcon.spriteName = role.HeadIcon;
    m_NameLabel.text = role.Name;
    m_LevelLabel.text = role.Level.ToString();
    //m_BattleNumLabel.text
    SetExp(role.Exp);
    m_CoinLabel.text = role.Coin.ToString();
    //m_YuanBaoLabel.text=
    m_EnergyLabel.text = role.Energy.ToString();
    m_ToughenLabel.text = role.Toughen.ToString();
  }
  public void SetExp(int value)
  {
    Role role=PlayData.RoleData;
    uint limit = Table_RoleLimit.GetExpLimit(role.Level);
    m_ExpLabel.text = string.Format("{0}/{1}", value, limit);
    m_ExpSlider.value =(float)(role.Exp / limit);
  }
  private void Exit()
  {
    Destroy(this.gameObject);
  }
}
