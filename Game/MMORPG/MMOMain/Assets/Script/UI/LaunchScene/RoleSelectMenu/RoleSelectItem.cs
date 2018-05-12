using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSelectItem : MonoBehaviour {

  private Animation m_Anima;

  public UIButton m_CreateRole;//创建角色
  public UIButton m_SelectRole;//选择角色
  public UILabel m_LevelLabel;
  public UILabel m_NameLabel;
  public GameObject m_InfoBG;
  public Transform m_ModelTrans;


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
    //角色不存在
    if(role==null)
    {
      m_CreateRole.gameObject.SetActive(true);
      m_SelectRole.gameObject.SetActive(false);
      m_InfoBG.SetActive(false);

    }
      //女性角色
    else if(role.Sex==true)
    {
      m_CreateRole.gameObject.SetActive(false);
      m_SelectRole.gameObject.SetActive(true);
      m_InfoBG.SetActive(true);
      m_LevelLabel.text = "Lv."+role.Level;
      m_NameLabel.text = role.Name;
      GameObject girlModel = ResourceManager.Load("Model/GirlShow");
      GameObject obj = NGUITools.AddChild(m_ModelTrans.gameObject, girlModel);
      m_Anima = obj.GetComponent<Animation>();
    }
    else
    {
      m_CreateRole.gameObject.SetActive(false);
      m_SelectRole.gameObject.SetActive(true);
      m_InfoBG.SetActive(true);
      m_LevelLabel.text = "Lv." + role.Level;
      m_NameLabel.text = role.Name;
      GameObject boyModel = ResourceManager.Load("Model/BoyShow");
      GameObject obj=NGUITools.AddChild(m_ModelTrans.gameObject, boyModel);
      m_Anima = obj.GetComponent<Animation>();
    }
    Select(isSelect);
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
