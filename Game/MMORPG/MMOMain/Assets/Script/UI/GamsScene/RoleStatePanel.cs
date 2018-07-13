using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleStatePanel : MonoBehaviour
{
  public UISprite m_HeadIcon;//头像
  public UILabel m_LevelLabel;//等级

  public UILabel m_NameLabel;//名称
  public UILabel m_BattleNumLabel;//战斗力
  public UIButton m_RenameBtn;//重命名

  public GameObject m_ReNamePanel;//改名窗口
  public UIInput m_ReNameInput;//改名输入
  public UIButton m_ReNameCommit;//改名确认
  public UIButton m_ReNameCancel;//改名取消

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
    m_RenameBtn.onClick.Add(new EventDelegate(OnClickRename));
    m_ReNameCommit.onClick.Add(new EventDelegate(OnClickReNameCommit));
    m_ReNameCancel.onClick.Add(new EventDelegate(OnClickCancelReName));
  }
  void OnEnable()
  {
    UpdateInfo();
    PlayData.OnRoleInfoChange += UpdateInfo;
    m_ReNamePanel.SetActive(false);
    //开启携程
    StartCoroutine(ShowRecoverData());
  }
  void OnDisable()
  {
    PlayData.OnRoleInfoChange -= UpdateInfo;
  }

  private void UpdateInfo()
  {
    Role role = PlayData.RoleData;
    m_HeadIcon.spriteName = role.headIcon;
    m_NameLabel.text = role.name;
    m_LevelLabel.text = role.level.ToString();
    //m_BattleNumLabel.text
    SetExp(role.exp);
    m_CoinLabel.text = role.coin.ToString();
    m_YuanBaoLabel.text = role.yuanBao.ToString();
    m_EnergyLabel.text = string.Format("{0}/{1}",role.energy.ToString(),Table_Role.GetEnergyLimit(role.level));
    m_ToughenLabel.text = string.Format("{0}/{1}",role.toughen.ToString(),Table_Role.GetToughenLimit(role.level));
  }
  public void SetExp(int value)
  {
    Role role = PlayData.RoleData;
    int limit = Table_Role.GetExpLimit((int)(role.level));
    m_ExpLabel.text = string.Format("{0}/{1}", value, limit);
    m_ExpSlider.value = (float)(role.exp / limit);
  }

  //点击改名，直接弹出界面
  private void OnClickRename()
  {
    m_ReNamePanel.SetActive(true);
  }
  //取消改名
  private void OnClickCancelReName()
  {
    m_ReNamePanel.SetActive(false);
  }
  //确认改名，向服务器发包
  private void OnClickReNameCommit()
  {
    string name = m_ReNameInput.value;
  }

  //向服务器请求同步体力恢复时间的数据
  private void SetPackageToGetRecoverTimer()
  {

  }

  //收到体力恢复时间的数据
  public void ReceiverRecoverTimer(int energyNext, int energyAll, int toughenNext, int toughenAll)
  {

  }

  private void Exit()
  {
    Destroy(this.gameObject);
  }


  IEnumerator ShowRecoverData()
  {
    while (true)
    {
      int timer;
      //拿到PlayerData的值，然后做显示
      if ((timer = PlayData.RoleData.energyNextRecoverTimer) != 0)
      {
        int hour, min, second;
        FormatTools.GetTimeFromInt(timer, out hour, out min, out second);
        m_EnergyRecoverAll.text = string.Format("{0}:{1}:{2}", hour < 10 ? "0" + hour : hour.ToString(), min < 10 ? "0" + min : min.ToString(), second < 10 ? "0" + second : second.ToString());
        if((timer=PlayData.RoleData.GetEnergyAllRecoverTimer())!=0)
        {
          FormatTools.GetTimeFromInt(timer, out hour, out min, out second);
          m_EnergyRecoverNext.text = string.Format("{0}:{1}:{2}", hour < 10 ? "0" + hour : hour.ToString(), min < 10 ? "0" + min : min.ToString(), second < 10 ? "0" + second : second.ToString());
        }
      }

      if ((timer = PlayData.RoleData.toughenNextRecoverTimer) != 0)
      {
        int hour, min, second;
        FormatTools.GetTimeFromInt(timer, out hour, out min, out second);
        m_ToughenRecoverAll.text = string.Format("{0}:{1}:{2}", hour < 10 ? "0" + hour : hour.ToString(), min < 10 ? "0" + min : min.ToString(), second < 10 ? "0" + second : second.ToString());
        if ((timer = PlayData.RoleData.GetToughenAllRecoverTimer()) != 0)
        {
          FormatTools.GetTimeFromInt(timer, out hour, out min, out second);
          m_ToughenRecoverNext.text = string.Format("{0}:{1}:{2}", hour < 10 ? "0" + hour : hour.ToString(), min < 10 ? "0" + min : min.ToString(), second < 10 ? "0" + second : second.ToString());
        }
      }
      yield return new WaitForSeconds(1);
    }
  }
}
