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

    public UISlider m_ExpSlider;//经验
    public UILabel m_ExpLabel;

    public UILabel m_CoinLabel;//金币
    public UILabel m_YuanBaoLabel;//元宝

    public UILabel m_EnergyLabel;//体力
    public UILabel m_EnergyRecoverNext;
    public UILabel m_EnergyRecoverAll;
    private int m_EnergyRecoverNextTimer=0;//体力恢复时间
    private int m_EnergyRecoverAllTimer = 0;//体力恢复时间

    public UILabel m_ToughenLabel;//历练
    public UILabel m_ToughenRecoverNext;
    public UILabel m_ToughenRecoverAll;
    private int m_ToughenRecoverNextTimer=0;//历练恢复时间
    private int m_ToughenRecoverAllTimer = 0;//历练恢复时间

    public UIButton m_ExitBtn;
    void Start()
    {
        m_ExitBtn.onClick.Add(new EventDelegate(Exit));
        m_RenameBtn.onClick.Add(new EventDelegate(OnClickRename));
    }
    void OnEnable()
    {
        UpdateInfo();
        PlayData.OnRoleInfoChange += UpdateInfo;
        //开启携程
    }
    void OnDisable()
    {
        PlayData.OnRoleInfoChange -= UpdateInfo;
    }

    private void UpdateInfo()
    {
        Role role = PlayData.RoleData;
        m_HeadIcon.spriteName = role.HeadIcon;
        m_NameLabel.text = role.Name;
        m_LevelLabel.text = role.Level.ToString();
        //m_BattleNumLabel.text
        SetExp(role.Exp);
        m_CoinLabel.text = role.Coin.ToString();
        m_YuanBaoLabel.text = role.YuanBao.ToString();
        m_EnergyLabel.text = role.Energy.ToString();
        m_ToughenLabel.text = role.Toughen.ToString();
    }
    public void SetExp(int value)
    {
        Role role = PlayData.RoleData;
        int limit = Table_Role.GetExpLimit((int)(role.Level));
        m_ExpLabel.text = string.Format("{0}/{1}", value, limit);
        m_ExpSlider.value = (float)(role.Exp / limit);
    }

    private void OnClickRename()
    {
        
    }

    //向服务器请求体力恢复时间的数据
    private void SetPackageToGetRecoverTimer()
    {

    }

    //收到体力恢复时间的数据
    public void ReceiverRecoverTimer(int energyNext,int energyAll,int toughenNext,int toughenAll)
    {

    }

    private void Exit()
    {
        Destroy(this.gameObject);
    }

    IEnumerator ShowRecoverData()
    {
        while(true)
        {
            if (m_EnergyRecoverNextTimer > 0)
            {
                m_EnergyRecoverNextTimer--;
                //写值
            }
            if ( m_ToughenRecoverNextTimer > 0)
            {
                m_ToughenRecoverNextTimer--;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
