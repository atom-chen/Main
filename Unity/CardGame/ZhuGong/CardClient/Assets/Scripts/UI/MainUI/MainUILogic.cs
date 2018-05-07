using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUILogic : MonoBehaviour {
  public UIEventTrigger m_Email;  //邮件
  public UIEventTrigger m_Head;  //头像
  public UIEventTrigger m_Chat; //聊天
  public UIEventTrigger m_Friend;//好友

  public UIEventTrigger m_Config;//设置
  public UIEventTrigger m_GongHui;//工会
  public UIEventTrigger m_Bag;  //背包
  public UIEventTrigger m_ZhuangBei;  //装备
  public UIEventTrigger m_BuZhen; //布阵
  public UIEventTrigger m_WuJiang;//武将
  public UIEventTrigger m_BianShen;//变身
  public UIEventTrigger m_RenWu;//任务
  public UIEventTrigger m_Tanchu;//弹出与收回菜单

  public UIEventTrigger m_ShenMoJueXing;//神魔觉醒
  public UIEventTrigger m_JinRiHuoDong;//今日活动
  public UIEventTrigger m_QiRiHaoLi;//今日好礼
  public UIEventTrigger m_ShouChongHaoLi;//首冲

  public UIEventTrigger m_TiLiAdd;//增加体力
  public UIEventTrigger m_YuanBaoAdd;//增加元宝

  void Start()
  {
    m_Email.onClick.Add(new EventDelegate(OnClickMail));
    m_Head.onClick.Add(new EventDelegate(OnClickHead));
    m_Chat.onClick.Add(new EventDelegate(OnClickChat));
    m_Friend.onClick.Add(new EventDelegate(OnClickFriend));

    m_Config.onClick.Add(new EventDelegate(OnClickConfig));
    m_GongHui.onClick.Add(new EventDelegate(OnClickGongHui));
    m_Bag.onClick.Add(new EventDelegate(OnClickBag));
    m_ZhuangBei.onClick.Add(new EventDelegate(OnClickZhuangBei));
    m_BuZhen.onClick.Add(new EventDelegate(OnClickBuZhen));
    m_WuJiang.onClick.Add(new EventDelegate(OnClickWuJiang));
    m_BianShen.onClick.Add(new EventDelegate(OnClickBianShen));
    m_RenWu.onClick.Add(new EventDelegate(OnClickRenWu));
    m_Tanchu.onClick.Add(new EventDelegate(OnClickTanChu));

    m_ShenMoJueXing.onClick.Add(new EventDelegate(OnClickShenMoJueXing));
    m_JinRiHuoDong.onClick.Add(new EventDelegate(OnClickJinRiHuoDong));
    m_QiRiHaoLi.onClick.Add(new EventDelegate(OnClickQiRiHaoLi));
    m_ShouChongHaoLi.onClick.Add(new EventDelegate(OnClickShouChongHaoLi));

    m_TiLiAdd.onClick.Add(new EventDelegate(OnClickTiLiAdd));
    m_YuanBaoAdd.onClick.Add(new EventDelegate(OnClickYuanBaoAdd));

  }

  private void OnClickMail()
  {
    UIManager.CreateUI("Mail",this.transform.root);
  }
  private void OnClickHead()
  {

  }
  private void OnClickChat()
  {

  }
  private void OnClickFriend()
  {

  }
  private void OnClickConfig()
  {

  }
  private void OnClickGongHui()
  {

  }
  private void OnClickBag()
  {

  }
  private void OnClickZhuangBei()
  {

  }
  private void OnClickBuZhen()
  {

  }
  private void OnClickWuJiang()
  {

  }
  private void OnClickBianShen()
  {

  }
  private void OnClickRenWu()
  {

  }
  private void OnClickTanChu()
  {

  }

  private void OnClickShenMoJueXing()
  {

  }
  private void OnClickJinRiHuoDong()
  {

  }
  private void OnClickShouChongHaoLi()
  {

  }
  private void OnClickQiRiHaoLi()
  {

  }

  private void OnClickTiLiAdd()
  {

  }
  private void OnClickYuanBaoAdd()
  {

  }



}
