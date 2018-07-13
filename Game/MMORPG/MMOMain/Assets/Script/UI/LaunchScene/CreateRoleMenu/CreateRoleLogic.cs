using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoleLogic : MonoBehaviour {
  public SelectRoleItem[] m_Roles;
  private int m_Index = 0;

  public UIInput m_RoleName;
  public UIButton m_Commit;
  public UIButton m_Exit;
  public UIEventTrigger m_LeftBtn;
  public UIEventTrigger m_RightBtn;
  void Start()
  {
    m_Commit.onClick.Add(new EventDelegate(OnCommitClick));
    m_Exit.onClick.Add(new EventDelegate(OnExitClick));
    m_LeftBtn.onClick.Add(new EventDelegate(OnLeftClick));
    m_RightBtn.onClick.Add(new EventDelegate(OnRightClick));
  }
  void OnEnable()
  {
    UpdateCurRole();
  }
  private void UpdateCurRole()
  {
    for (int i = 0; i < m_Roles.Length; i++)
    {
      if (i == m_Index)
      {
        m_Roles[i].gameObject.SetActive(true);
      }
      else
      {
        m_Roles[i].gameObject.SetActive(false);
      }
    }
  }
  //点击向左
  private void OnLeftClick()
  {
    if(m_Index>0)
    {
      m_Index--;
      UpdateCurRole();
    }
  }

  //点击向右
  private void OnRightClick()
  {
    if (m_Index < m_Roles.Length-1)
    {
      m_Index++;
      UpdateCurRole();
    }
  }

  //点击新建角色
  private void OnCommitClick()
  {
    //提交姓名到服务器
    Dictionary<byte, object> dic = new Dictionary<byte, object>();
    Role role=new Role();
    role.name=m_RoleName.value;
    role.sex=m_Index==1?true:false;
    dic.Add((byte)ParameterCode.Role, ParaTools.GetJson<Role>(role));
    PhotoEngine.Instance.SendRequest(OperationCode.RoleAdd, dic);
  }

  //点击退出
  private void OnExitClick()
  {
    //返回
    LaunchSceneLogic.Instance.SwitchToSelectRoleMenu();
  }
}
