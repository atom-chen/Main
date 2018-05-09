using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSelectItem : MonoBehaviour {

  public Animation m_Anima;

  public UIButton m_CreateRole;//创建角色
  public UIButton m_SelectRole;//选择角色

  private Role m_Role;//当前角色信息
  
  void Start()
  {
    m_CreateRole.onClick.Add(new EventDelegate(RoleSelectLogic.Instance.OnClickCreateRole));
    m_Anima = this.GetComponentInChildren<Animation>();
  }

  //初始化，传入角色信息和是否选择
  public void Init(Role role,bool isSelect)
  {
    this.m_Role = role;
    Select(isSelect);
    //根据Role信息构造信息面板
  }

  //是否选择当前角色
  public void Select(bool isSelect)
  {
    if(isSelect)
    {
      m_SelectRole.gameObject.SetActive(false);
    }
    if (m_Anima==null)
    {
      return;
    }
    if(isSelect)
    {
      m_Anima.Play();
    }
    else
    {
      m_Anima.Stop();
    }
  }

  

}
